using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hunt : Behaviour
{

    NavMeshAgent agent;
    Sight sight;
    bool eating = false;
    float eatTimer;
    [SerializeField] float eatDuration = 5;
    ParticleSystem eatingParticles;
    ParticleSystem diggingParticles;
    bool particlesTriggered = false;

    protected override void OnInitialize()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        sight = gameObject.GetComponent<Sight>();
        eatingParticles = gameObject.GetComponentsInChildren<ParticleSystem>()[0];
        if(gameObject.tag == "Rabbit")
        {
            diggingParticles = gameObject.GetComponentsInChildren<ParticleSystem>()[1];
        }
        eatTimer = eatDuration;
    }

    protected override Status Update()
    {
        if (agent == null) return Status.BH_INVALID;

        if (sight.LastSeenFood == null) return Status.BH_FAILED;

        float distance = (gameObject.transform.position - sight.LastSeenFood.transform.position).magnitude; 

        if (distance >= 1)
        {
            agent.isStopped = false;
            agent.SetDestination(sight.LastSeenFood.transform.position);
           // Moving to food
        }
        else
        {
            //at food
            agent.isStopped = true;
            eating = true;

        }

        if(eating)
        {
            eatTimer -= Time.deltaTime;

            if (particlesTriggered == false)
            {
                eatingParticles.Play();
                particlesTriggered = true;
            }

            if (eatTimer <= 0)
            {
                eating = false;
                Object.Destroy(sight.LastSeenFood);
                eatTimer = eatDuration;
                sight.LastSeenFood = null;
                sight.Energy += sight.EnergyReplenishment;
                //Finished eating
                return Status.BH_SUCCESS;
            }
        }

        return Status.BH_RUNNING;
    }
    
}
