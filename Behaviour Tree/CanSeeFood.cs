using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanSeeFood : Behaviour
{
    Sight sight;
    float timer = 1.0f; 
    GameObject food;

    protected override void OnInitialize()
    {
        sight = gameObject.GetComponent<Sight>();
    }

    protected override Status Update()
    {
        if (timer <= 0)
        {
            //failed to see food
            timer = 1.0f;
            return Status.BH_FAILED;
        }

        timer -= Time.deltaTime;

        if (IsFoodVisible(out food))
        {
            sight.LastSeenFood = food;
            food = null;
            timer = 5.0f;
            //see food
            return Status.BH_SUCCESS;
        }
       // looking for food
        return Status.BH_RUNNING;
    }

    private bool IsFoodVisible(out GameObject seenFood)
    {
        try
        {
            if(sight.VisibleObjects.Count > 0)
            {
                foreach (GameObject obj in sight.VisibleObjects)
                {
                    if (obj.tag == sight.FoodSource.tag)
                    {
                        seenFood = obj;
                        return true;
                    }
                }
            }
        }
       catch(Exception e)
        {
            Debug.Log("error finding food - " + e);    
        }
        
        seenFood = null;
        return false;
    }

}
