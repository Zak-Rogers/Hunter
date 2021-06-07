using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : BaseStateClass
{

    Transform lastSeenFoodSouce;
    bool eatting = false;
    private float eatTimer; 
    [SerializeField] private float eatDuration = 5;

    [SerializeField] ParticleSystem[] particleSystems;
    private bool triggeredParticles;

    protected override void Awake()
    {
        base.Awake();
        stateName = "Hunt";
    }

    protected void OnEnable()
    {
        eatTimer = eatDuration;
    }

    protected override void Update()
    {
        base.Update();

        Hunt();

        if(eatting)
        {
            eatTimer -= Time.deltaTime;

            if (triggeredParticles == false)
            {
                triggeredParticles = TriggerParticles();
            }

            parentFSM.Energy += 5 * Time.deltaTime; 

            if(eatTimer <= 0)
            {
                eatting = false;
                if (lastSeenFoodSouce != null && DistanceToTarget(lastSeenFoodSouce.position) < 1.5f)
                {
                    Destroy(lastSeenFoodSouce.gameObject);
                    lastSeenFoodSouce = null;
                }
            }
        }
        else
        {
            CheckIfStateShouldChange();
        }


    }

    void Hunt()
    {
        Transform food;
        Vector3 adjustedPosition;
        if(CanSeeFood(out food) && DistanceToTarget(food.position) <= parentFSM.HuntDistance)
        {
            MoveToPosition(food.position);
            lastSeenFoodSouce = food;
        }

        

        if(lastSeenFoodSouce != null) 
        {
            //adjust food souce's Y position to match the gameObjects.
            adjustedPosition = new Vector3(lastSeenFoodSouce.position.x, transform.position.y, lastSeenFoodSouce.position.z);
            if(DistanceToTarget(adjustedPosition) < 1.0f)
            {
                eatting = true;

                if (lastSeenFoodSouce.tag == "Carrot")
                {
                    agent.ResetPath();
                }
            }
        }
    }

    private bool TriggerParticles()
    {
        foreach (ParticleSystem part in particleSystems)
        {
            part.Play();
        }
        return true;
    }
}
