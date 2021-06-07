using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateState : BaseStateClass
{
    Transform selectedMate;
    bool mating = false;
    float mateTimer; 
    [SerializeField] float mateDuration = 5;
    SpawnSystem spawnSystem;
    [SerializeField] ParticleSystem matingParticles;
    private bool particlesTriggered;

    protected override void Awake()
    {
        base.Awake();
        stateName = "Mate";        
    }

    private void Start()
    {
        spawnSystem = GameObject.FindGameObjectWithTag("Spawn System").GetComponent<SpawnSystem>();
    }

    private void OnEnable()
    {
        mateTimer = mateDuration;
    }

    protected override void Update()
    {
        base.Update();

        if(mating)
        {
            mateTimer -= Time.deltaTime; 
            parentFSM.CanMate = false;
            
            if(particlesTriggered == false)
            {
                particlesTriggered = TriggerParticles();
            }
                

            if (mateTimer <= 0)
            {
                agent.ResetPath();
                selectedMate = null;
                mateTimer = mateDuration;
                mating = false;
                
                spawnSystem.SpawnObject(gameObject.tag);
                parentFSM.RecentlyMated = true;
                parentFSM.CanMate = true;
            }
        }
        else
        {
            Mate();
            CheckIfStateShouldChange();
        }

    }

    void Mate()
    {
        Transform mate;

        if (CanSeeMate(out mate) && DistanceToTarget(mate.position) <= parentFSM.MateDistance)
        {
            MoveToPosition(mate.position);
            selectedMate = mate;
        }

        if (selectedMate != null && DistanceToTarget(selectedMate.position) < 0.8f && parentFSM.Energy > parentFSM.EnergyNeededToMate && !parentFSM.RecentlyMated)
        {
            mating = true;
        }
    }

    bool TriggerParticles()
    {
        matingParticles.Play();
        return true;
    }
}
