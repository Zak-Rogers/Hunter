using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Explore : Behaviour
{


    NavMeshAgent agent;
    bool gotPosition = false;
    Vector3 position;
    bool agentStuck;
    float stuckTimer;
    [SerializeField] float stuckDuration = 20;

    protected override void OnInitialize()
    {
        stuckTimer = stuckDuration;
        agentStuck = false;
        agent = gameObject.GetComponent<NavMeshAgent>();
    }
    protected override Status Update()
    {
        if(agent)
        {
            if(!gotPosition)
            {
                gotPosition = GetRandomPosition(out position);
            }
            float distance = (gameObject.transform.position - position).magnitude;
            position = new Vector3(position.x, gameObject.transform.position.y, position.z);
            
            if (distance >= 1)
            {
                agent.isStopped = false;
                agent.SetDestination(position);
                if(agent.velocity.magnitude == 0)
                {
                    stuckTimer -= Time.deltaTime;

                    if(stuckTimer <= 0)
                    {
                        agentStuck = true;
                        stuckTimer = stuckDuration;
                    }
                }
                else
                {
                    agentStuck = false;
                    stuckTimer = stuckDuration;
                }

                if(agent.isPathStale || agentStuck)
                {
                    gotPosition = false;
                }
                //moving to new position.
                return Status.BH_RUNNING;
            }
            else
            {
               // Explore Sucess
                agent.isStopped = true;
                gotPosition = false;
                return Status.BH_SUCCESS;
            }
        }
        return Status.BH_INVALID;
    }
    private bool GetRandomPosition(out Vector3 position)
    {
        Vector3 rand = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(10.0f, 20.0f);
        position = gameObject.transform.position + rand;
        NavMeshHit hit;
        for (int i = 0; i<40; i++)
        {
            if(NavMesh.SamplePosition(position, out hit, 5.0f, NavMesh.AllAreas))
            {
               // Getting random postion
                position = hit.position;
                return true;
            }
            rand = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(10.0f, 20.0f);
            position = gameObject.transform.position + rand;
        }
       // failed to find position
        position = gameObject.transform.position;
        return false;
    }
}
