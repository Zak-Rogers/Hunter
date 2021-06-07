using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour 
{
    [SerializeField] private Vector3 offset; 
    [SerializeField] private float sightDistance;

    [Header("BT_Blackboard")]
    [SerializeField] private GameObject foodSource;
    [SerializeField] private GameObject[] preditors;
    private List<GameObject> visibleObjects = new List<GameObject>();

    private GameObject lastSeenFood; 
    private GameObject lastSeenMate; 
    private GameObject lastSeenPred;
    private float energy = 75;
    [SerializeField] private float energyReplenishment = 20; 
    [SerializeField] private float energyRequiredToMate = 60; 
    public List<GameObject> VisibleObjects
    {
        get { return visibleObjects; }
    }

    public GameObject FoodSource
    {
        get { return foodSource; }
    }
    public GameObject[] Preditors
    {
        get { return preditors; }
    }

    public GameObject LastSeenFood
    {
        get { return lastSeenFood; }

        set { lastSeenFood = value; }
    }

    public GameObject LastSeenMate
    {
        get { return lastSeenMate; }

        set { lastSeenMate = value; }
    }

    public GameObject LastSeenPred
    {
        get { return lastSeenPred; }

        set { lastSeenPred = value; }
    }

    public float Energy
    {
        get { return energy; }
        set { energy = value; }
    }

    public float EnergyReplenishment
    {
        get { return energyReplenishment; }
    }

    public float EnergyRequiredToMate
    {
        get { return energyRequiredToMate; }
    }

    void Update()
    {
        // 5 ray casts 1 forward 2 either side.
        Vector3[] viewDirections = new Vector3[5];
        viewDirections[0] = -transform.right;
        viewDirections[1] = (transform.forward - transform.right).normalized;
        viewDirections[2] = transform.forward;
        viewDirections[3] = (transform.forward + transform.right).normalized;
        viewDirections[4] = transform.right;

        visibleObjects.Clear();

        foreach (Vector3 dir in viewDirections)
        {
            RaycastHit hit;
            GameObject hitObj;
            Debug.DrawRay(transform.position + offset, dir * sightDistance, Color.red);

            if (Physics.Raycast(transform.position + offset, dir, out hit, sightDistance))
            {
                hitObj = hit.collider.gameObject;

                if (hitObj == gameObject) return; 
                // add food
                if (hitObj.tag == foodSource.tag)
                {
                    visibleObjects.Add(hitObj);
                }
                // add preditors
                foreach (GameObject pred in preditors)
                {
                    if (hitObj.tag == pred.tag)
                    {
                        visibleObjects.Add(hitObj);
                    }
                }
                // add mates
                if (hitObj.tag == gameObject.tag)
                {
                    visibleObjects.Add(hitObj);
                }
            }
        }

    }
}
