using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasEnergyToMate : Behaviour
{


    Sight sight;

    protected override void OnInitialize()
    {
        sight = gameObject.GetComponent<Sight>();
    }

    protected override Status Update()
    {
        if (sight.Energy >= sight.EnergyRequiredToMate)
        {
            return Status.BH_SUCCESS;
        }

        return Status.BH_FAILED;
    }
}
