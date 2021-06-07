using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreState : BaseStateClass
{

    Vector3 newPosition;
    bool moving;
    float waitTimer;
    [SerializeField] private float initalWaitDuration = 2;

    protected override void Awake()
    {
        base.Awake();
        stateName = "Explore";
    }

    private void OnEnable()
    {
        newPosition = RandomPosition();
        waitTimer = initalWaitDuration;
    }

    protected override void Update()
    {
        base.Update();

        waitTimer -= Time.deltaTime;

        if (DistanceToTarget(newPosition) <= 3.0f  && !moving) 
        {
            newPosition = RandomPosition();
        }
        if(agent != null)
        {
            if(agent.velocity != Vector3.zero )
            {
                moving = true;
            }
            else
            {
                moving = false;
            }

        }

        Explore();
        CheckIfStateShouldChange();
        
    }

    void Explore()
    {
        if(!moving && waitTimer <= 0)
        {
            MoveToPosition(newPosition);
            waitTimer = Random.Range(1.0f, 10.0f);
        }
    }
}
