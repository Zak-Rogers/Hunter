using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mate : Behaviour
{

    NavMeshAgent agent;
    Sight sight;
    float mateTimer;
    [SerializeField] float mateDuration = 5;
    bool mating = false;
    bool particlesTriggered = false;
    [SerializeField] ParticleSystem matingParticles;
    SpawnSystem spawnSystem;

    protected override void OnInitialize()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        sight = gameObject.GetComponent<Sight>();
        mateTimer = mateDuration;
        if(gameObject.tag == "Fox") 
        {
            matingParticles = gameObject.GetComponentsInChildren<ParticleSystem>()[1];
        }
        else if (gameObject.tag == "Rabbit")
        {
            matingParticles = gameObject.GetComponentsInChildren<ParticleSystem>()[2];
        }
        spawnSystem = GameObject.FindGameObjectWithTag("Spawn System").GetComponent<SpawnSystem>();
    }

    protected override Status Update()
    {
        if (agent == null) return Status.BH_INVALID;

        if (sight.LastSeenMate == null) return Status.BH_FAILED;

        float distance = (gameObject.transform.position - sight.LastSeenMate.transform.position).magnitude;

        if (distance >= 1)
        {
            agent.isStopped = false;
            agent.SetDestination(sight.LastSeenMate.transform.position);
            //Moving to mate
        }
        else
        {
            //at mate
            agent.isStopped = true;
            mating = true;
        }

        if(mating)
        {
            mateTimer -= Time.deltaTime;
            if (particlesTriggered == false)
            {
                matingParticles.Play();
                particlesTriggered = true;
            }

            if(mateTimer <= 0)
            {
                mating = false;
                spawnSystem.SpawnObject(gameObject.tag);
                mateTimer = mateDuration;
                sight.LastSeenMate = null;
                //Mate sucessfull
                return Status.BH_SUCCESS;
            }
        }
        
        return Status.BH_RUNNING;
    }

}
