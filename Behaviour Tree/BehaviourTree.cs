using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourTree : MonoBehaviour
{


    [SerializeField] float fleeRange = 15.0f;
    [SerializeField] float mateRange = 15.0f;
    [SerializeField] float huntRange = 10.0f;

    NavMeshAgent agent;
    Sight sight;
    Animator anim;

    bool active = true;

    Selector behaviourTreeRoot;
    Sequence fleeSequence;
    Sequence mateSequence;
    Sequence huntSequence;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sight = GetComponent<Sight>();
        anim = GetComponent <Animator>();


        behaviourTreeRoot = new Selector();
        behaviourTreeRoot.SetName("Main Root - Selector");
        behaviourTreeRoot.SetGameObject(gameObject);
        //
        fleeSequence = new Sequence();
        fleeSequence.SetName("Flee Sequence");
        fleeSequence.SetGameObject(gameObject);

        CanSeePred canSeePred = new CanSeePred();
        CheckPredInRange preditorInRange = new CheckPredInRange(fleeRange);
        RunAway runAway = new RunAway();


        fleeSequence.AddChild(canSeePred);
        fleeSequence.AddChild(preditorInRange);
        fleeSequence.AddChild(runAway);
        //
        mateSequence = new Sequence();
        mateSequence.SetName("Mate Sequence");
        mateSequence.SetGameObject(gameObject);

        CanSeeMate canSeeMate = new CanSeeMate();
        CheckMateInRange mateInRange = new CheckMateInRange(mateRange);
        HasEnergyToMate hasEnergy = new HasEnergyToMate();
        Mate mate = new Mate();

        mateSequence.AddChild(canSeeMate);
        mateSequence.AddChild(mateInRange);
        mateSequence.AddChild(hasEnergy);
        mateSequence.AddChild(mate);
        //
        huntSequence = new Sequence();
        huntSequence.SetName("Hunt Sequence");
        huntSequence.SetGameObject(gameObject);

        CanSeeFood canSeeFood = new CanSeeFood();
        canSeeFood.SetName("canSeeFood"); 
        CheckFoodInRange foodInRange = new CheckFoodInRange(huntRange);
        Hunt hunt = new Hunt();

        huntSequence.AddChild(canSeeFood);
        huntSequence.AddChild(foodInRange);
        huntSequence.AddChild(hunt);
        //
        Explore explore = new Explore();
        //
        behaviourTreeRoot.AddChild(fleeSequence);
        behaviourTreeRoot.AddChild(mateSequence); 
        behaviourTreeRoot.AddChild(huntSequence); 
        behaviourTreeRoot.AddChild(explore);

    }

    
    void Update()
    {
        if (!active) return;

        Status tickResult = behaviourTreeRoot.Tick();
        if(tickResult == Status.BH_SUCCESS)
        {
            active = false;
            StartCoroutine(RestartTree());
            behaviourTreeRoot.currentStatus = Status.BH_INVALID;
        }
        if(agent.velocity.magnitude > 0 )
        {
            sight.Energy -= Time.deltaTime;
            anim.enabled = true;
        }
        else
        {
            anim.enabled = false;
        }

        if(sight.Energy <= 0 )
        {
            Destroy(gameObject);
        }
    }

    IEnumerator RestartTree()
    {
        yield return new WaitForSeconds(2); 
        active = true;
    }
}

