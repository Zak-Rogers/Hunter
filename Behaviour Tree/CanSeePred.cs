using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeePred : Behaviour
{
    
    
    Sight sight;
    float timer = 1.0f; 
    GameObject preditor;

    protected override void OnInitialize()
    {
        sight = gameObject.GetComponent<Sight>();
    }

    protected override Status Update()
    {
        if (timer <= 0)
        {
            //fails to find preditor in time
            timer = 1.0f;
            return Status.BH_FAILED;
        }

        timer -= Time.deltaTime;

        if(IsPreditorVisible(out preditor))
        {
            sight.LastSeenPred = preditor;
            preditor = null;
            timer = 5.0f;
            //found preditor
            return Status.BH_SUCCESS;
        }
        //looking for pred.
        return Status.BH_RUNNING;
    }

    private bool IsPreditorVisible(out GameObject seenPreditor)
    {
        try
        {
            foreach (GameObject pred in sight.Preditors)
            {
                if (sight.VisibleObjects.Count > 0)
                {
                    foreach (GameObject obj in sight.VisibleObjects)
                    {
                        if (obj != null)
                        {
                            if (obj.tag == pred.tag)
                            {
                                seenPreditor = obj;
                                return true;
                            }
                        }
                    }

                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("Error finding pred - " + e);
        }

        seenPreditor = null;
        return false;
    }
}
