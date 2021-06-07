using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMateInRange : Behaviour
{
    float range;
    Sight sight;

    public CheckMateInRange(float _range)
    {
        range = _range;
    }

    protected override void OnInitialize()
    {
        sight = gameObject.GetComponent<Sight>();
    }

    protected override Status Update()
    {
        if (sight.LastSeenMate)
        {
            float distance = (gameObject.transform.position - sight.LastSeenMate.transform.position).magnitude;

            if (distance <= range)
            {
               // Mate in range
                return Status.BH_SUCCESS;
            }
            else
            {
                sight.LastSeenMate = null;
               // Mate not in range
                return Status.BH_FAILED;
            }
        }
        return Status.BH_INVALID;
    }
}
