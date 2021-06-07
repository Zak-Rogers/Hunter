using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPredInRange : Behaviour
{


    float range;
    Sight sight;

    public CheckPredInRange(float _range)
    {
        range = _range;
    }

    protected override void OnInitialize()
    {
        sight = gameObject.GetComponent<Sight>();
    }

    protected override Status Update()
    {
        if(sight.LastSeenPred)
        {
            float distance = (gameObject.transform.position - sight.LastSeenPred.transform.position).magnitude;
            if (distance <= range)
            {
               // Pred in range
                return Status.BH_SUCCESS;
            }
            else
            {
                sight.LastSeenPred = null;
                //Pred not in range
                return Status.BH_FAILED;
            }
        }
        return Status.BH_INVALID;
    }
}
