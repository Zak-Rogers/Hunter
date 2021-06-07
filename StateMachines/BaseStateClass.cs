using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseStateClass : MonoBehaviour
{


    protected NavMeshAgent agent;
    protected Animator animator;
    [SerializeField] protected string stateName;
    protected StateMachine parentFSM;
    protected PerformanceGather gather;


    public StateMachine ParentFSM
    {
        set { parentFSM = value; }
    }

    public string StateName
    {
        get { return stateName; }
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if(agent.velocity.magnitude > 0)
        {
            parentFSM.Energy -= Time.deltaTime;  
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false ;
        }

        if(parentFSM.Energy <= 0)
        {
            agent.isStopped = true;
            agent = null; 
            Destroy(gameObject);
        }


    }

    protected Vector3 RandomPosition()
    {
        Vector3 rand = Random.insideUnitSphere * Random.Range(2.50f, 20.0f);
        Vector3 position = transform.position + rand;
        NavMeshHit hit;
        for (int i = 0; i < 20; i++)
        {
            if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
            {
                position = hit.position;
                return position;
            }
        }
        position = transform.position;
        return position;
    }

    protected void FaceAway(Vector3 position)
    {
        transform.LookAt(-position);
    }

    protected void MoveForward(float speed)
    {
        if (agent != null)
        {
            agent.Move(transform.forward * speed * Time.deltaTime);
        }
    }

    protected void MoveToPosition(Vector3 position)
    {
        if(agent != null)
        {
            agent.SetDestination(position);
        }
    }

    protected float DistanceToTarget(Vector3 target)
    {
        float distance = (target - transform.position).magnitude;

        return distance;
    }

    // 5 ray casts 1 forward 2 either side.
    protected List<GameObject> WhatCanBeSeen()
    {
        List<GameObject> gameObjects = new List<GameObject>();
        Vector3[] viewDirections = new Vector3[5];
        viewDirections[0] = -transform.right;
        viewDirections[1] = (transform.forward - transform.right).normalized;
        viewDirections[2] = transform.forward;
        viewDirections[3] = (transform.forward + transform.right).normalized;
        viewDirections[4] = transform.right;

        foreach (Vector3 dir in viewDirections)
        {
            RaycastHit hit;
            GameObject hitObj;
            Debug.DrawRay(transform.position, dir, Color.red);

            if (Physics.Raycast(transform.position, dir, out hit, parentFSM.SightDistance))
            {
                hitObj = hit.collider.gameObject;
                // add food
                if(hitObj.tag == parentFSM.FoodSource.tag)
                {
                    gameObjects.Add(hitObj);
                }
                // add preditors
                foreach(GameObject pred in parentFSM.Preditors)
                {
                    if(hitObj.tag == pred.tag)
                    {
                        gameObjects.Add(hitObj);
                    }
                }
                // add mates
                if(hitObj.tag == gameObject.tag)
                {
                    gameObjects.Add(hitObj);
                }
            }
        }
        
        return gameObjects;
    }

    /// <summary>
    /// Checks if there is a gameobject visible that matches the animals food source.
    /// </summary>
    /// <param name="food"> sets the transform parameter to the food gameobject's transform if sucessful and null if not</param>
    /// <returns> returns true if found and returns the foods transform to the varible set in parameter else returns false and sets the parameter to null</returns>
    protected bool CanSeeFood(out Transform food)
    {
        //check is anything visible matchs a food gameobject 
        List<GameObject> visibleObjects = WhatCanBeSeen();
        //loop visible objects
        foreach(GameObject obj in visibleObjects)
        {
            //compare with foodSource
            if (obj.tag == parentFSM.FoodSource.tag)
            {
                // if found return true;
                food = obj.transform;
                return true;
            }
        }
        // failed to find food return false;
        food = null;
        return false; 
    }

    protected bool CanSeeMate(out Transform mate)
    {
        //check is anything visible matchs a food gameobject 
        List<GameObject> visibleObjects = WhatCanBeSeen();
        //loop visible objects
        foreach (GameObject obj in visibleObjects)
        {
            //compare with mate
            if (obj.tag == gameObject.tag)
            {
                // if found return true;
                mate = obj.transform;
                return true;
            }
        }
        // failed to find mate return false; 
        mate = null;
        return false;
    }

    protected bool CanSeePreditor(out Transform preditor)
    {
        //check is anything visible matchs a peditor gameobject 
        List<GameObject> visibleObjects = WhatCanBeSeen();
        // loop objects
        foreach(GameObject obj in visibleObjects)
        {
            //check if obj is a preditor
            foreach (GameObject pred in parentFSM.Preditors)
            {
                if (obj == pred)
                {
                    //if found return true
                    preditor = obj.transform;
                    return true;
                }
            }
        }
        // failed to find preditor return false;
        preditor= null;
        return false; 
    }


    protected void CheckIfStateShouldChange()
    {
        Transform targetTransform;
        List<GameObject> visableObjs;

        // Set to hunt state if food is visible and in range.
        if (CanSeeFood(out targetTransform) && DistanceToTarget(targetTransform.position) < parentFSM.HuntDistance && parentFSM.CurrentState != "Hunt" && parentFSM.Energy <100)
        {
            parentFSM.SetState("Hunt");
        }
        // Set to flee state if preditor is visible and in range.
        if (CanSeePreditor(out targetTransform) && DistanceToTarget(targetTransform.position) < parentFSM.FleeDistance)
        {
            parentFSM.SetState("Flee");
        }
        // Set to mate state if mate is visible and in range and energy
        if (CanSeeMate(out targetTransform) && DistanceToTarget(targetTransform.position) < parentFSM.MateDistance  && parentFSM.Energy > parentFSM.EnergyNeededToMate && parentFSM.CurrentState != "Mate" && !parentFSM.RecentlyMated && targetTransform.gameObject.GetComponent<StateMachine>().CanMate)//
        {
            parentFSM.SetState("Mate");
        }
        //Set to explore state if nothing can be seen
        visableObjs = WhatCanBeSeen();
        
        if(visableObjs.Count == 0 && parentFSM.CurrentState != "Explore")
        {
            parentFSM.SetState("Explore");
        }
    }

    void OnDestroy()
    {
        agent = null;
    }
}
