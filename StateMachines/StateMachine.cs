using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{


    private string currentState ="";
    Dictionary<string, BaseStateClass> stateDict = new Dictionary<string, BaseStateClass>();


    [Header("Animal's stats")]
    [SerializeField] private GameObject foodSource;
    [SerializeField] private GameObject[] preditors;
    [SerializeField] private float sightDistance = 5;
    [SerializeField] private float energy = 100; 
    [SerializeField] private float energyNeededToMate = 80;
    private bool canMate;
    private bool recentlyMated;
    [SerializeField] private float matingCooldown = 5;
    private float coolDownTimer; 


    [Header("State Trigger Distances")]
    [SerializeField] private float huntDistance = 10;
    [SerializeField] private float fleeDistance = 10;
    [SerializeField] private float mateDistance = 10;

    // Public Getters for Animal's Stats
    public string CurrentState
    {
        get { return currentState; }
    }
    public GameObject FoodSource 
    {
        get { return foodSource; }
    }

    public GameObject[] Preditors
    {
        get { return preditors; }
    }

    public float SightDistance
    {
        get { return sightDistance; }
    }

    public float Energy
    {
        get { return energy; }

        set { energy = value; }
    }
    public float EnergyNeededToMate
    {
        get { return energyNeededToMate; }
    }

    public float HuntDistance
    {
        get { return huntDistance; }
    }

    public float FleeDistance
    {
        get { return fleeDistance; }
    }

    public float MateDistance
    {
        get { return mateDistance; }
    }
    public bool CanMate
    {
        get { return canMate; }

        set { canMate = value; }
    }

    public bool RecentlyMated
    {
        get { return recentlyMated; }

        set { recentlyMated = value; }
    }

    void Start()
    {
        Component[] tempList = GetComponents<BaseStateClass>();

        foreach(BaseStateClass state in tempList)
        {
            stateDict.Add(state.StateName, state);
            state.enabled = false;
            state.ParentFSM = this;
        }
        canMate = true;
        coolDownTimer = matingCooldown;
        SetState("Explore");
    }

    
    void Update()
    {
        if(recentlyMated)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer <= 0)
            {
                coolDownTimer = matingCooldown;
                recentlyMated = false;
            }
        }
    }

    public void SetState(string state)
    {
        //disable currentState
        BaseStateClass stateClass;
        stateDict.TryGetValue(currentState, out stateClass);
        
        if(stateClass != null)
        {
            stateDict[currentState].enabled = false;
        }
       
        //enable component
        currentState = state;
        stateDict[state].enabled = true;
    }
}
