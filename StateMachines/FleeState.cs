using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : BaseStateClass
{


    [SerializeField] private float fleeSpeed = 5.0f;
    private Vector3 lastSeenPreditor;
    private bool fleeing = false;
    
    protected override void Awake()
    {
        base.Awake();
        stateName = "Flee";
    }

    protected override void Update()
    {
        base.Update();

        Flee();
        if (fleeing)
        {
            agent.ResetPath();
            FaceAway(lastSeenPreditor.normalized);
            MoveForward(fleeSpeed);
        }
        else
        {
            CheckIfStateShouldChange();
        }

    }

    void Flee()
    {
        Transform preditor;
        //if preditor is visible
        if (CanSeePreditor(out preditor))
        {
            //if preditor is in flee range
            if (DistanceToTarget(preditor.position) < parentFSM.FleeDistance) 
            {
                lastSeenPreditor = preditor.position;
                fleeing = true;

            }
        }


        if (DistanceToTarget(lastSeenPreditor) >= parentFSM.FleeDistance) 
        {
            fleeing = false;
            agent.ResetPath();
        }
    }
}
