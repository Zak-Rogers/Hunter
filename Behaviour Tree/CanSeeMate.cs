using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanSeeMate : Behaviour
{
    Sight sight;
    float timer = 1.0f; 
    GameObject mate;

    protected override void OnInitialize()
    {
        sight = gameObject.GetComponent<Sight>();
    }

    protected override Status Update()
    {
        if (timer <= 0)
        {
           // failed to see mate
            timer = 1.0f;
            return Status.BH_FAILED;
        }

        timer -= Time.deltaTime;

        if (IsMateVisible(out mate))
        {
            sight.LastSeenMate = mate;
            mate = null;
            timer = 5.0f;
           // sees mate
            return Status.BH_SUCCESS;
        }
        //looking for mate
        return Status.BH_RUNNING;
    }

    private bool IsMateVisible(out GameObject seenMate)
    {
        try
        {
            if (sight.VisibleObjects.Count > 0)
            {
                foreach (GameObject obj in sight.VisibleObjects)
                {
                    if (obj.tag == gameObject.tag)
                    {
                        seenMate = obj;
                        return true;
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("Mate Error - " + e);
        }
        
        seenMate = null;
        return false;
    }
}
