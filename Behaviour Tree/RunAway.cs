using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAway : Behaviour
{


    Sight sight;
    NavMeshAgent agent;
    bool gotLocation = false;
    Vector3 position;
    protected override void OnInitialize()
    {
        sight = gameObject.GetComponent<Sight>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    
    protected override Status Update()
    {
        if(agent && sight.LastSeenPred)
        {
            if (!gotLocation)
            {
                gotLocation = GetFleeLocation(out position);
            }


            float distance = (gameObject.transform.position - position).magnitude;

            if(distance >= 1.0f)
            {
                agent.isStopped = false;
                agent.SetDestination(position);
                //Fleeing
                return Status.BH_RUNNING;
            }
            else
            {
                agent.isStopped = true;
                sight.LastSeenPred = null;
                gotLocation = false;
                //Flee Sucessful
                return Status.BH_SUCCESS;
            }
        }
        return Status.BH_INVALID;
    }

    private bool GetFleeLocation(out Vector3 _position)
    {
        _position = gameObject.transform.position;
        Vector3 direction = (_position - sight.LastSeenPred.transform.position).normalized;

        _position += direction * 10;
        Vector3 rand;

        NavMeshHit hit;
        while (!NavMesh.SamplePosition(_position, out hit, 1.0f, NavMesh.AllAreas))
        {
            rand = UnityEngine.Random.insideUnitSphere;
            _position = _position + rand;
        }

        return true;
    }
}
