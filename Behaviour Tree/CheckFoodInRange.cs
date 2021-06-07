using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFoodInRange : Behaviour
{
    float range;
    Sight sight;

    public CheckFoodInRange(float _range)
    {
        range = _range;
    }

    protected override void OnInitialize()
    {
        sight = gameObject.GetComponent<Sight>();
    }

    protected override Status Update()
    {
        if (sight.LastSeenFood)
        {
            float distance = (gameObject.transform.position - sight.LastSeenFood.transform.position).magnitude;
            if (distance <= range)
            {
               // Food in range
                return Status.BH_SUCCESS;
            }
            else
            {
                sight.LastSeenFood = null;
               // Food not in range
                return Status.BH_FAILED;
            }
        }
        return Status.BH_INVALID;
    }
}
