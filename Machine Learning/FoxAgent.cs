using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class FoxAgent : Agent
{

    NavMeshAgent navAgent;
    [SerializeField] float rotationSpeed = 10;
    [SerializeField] float directionMultiplier = 2;
    public float energy = 75; 
    bool wait = false; 
    float waitTimer;
    [SerializeField] float waitDuration = 5;
    [SerializeField] GameObject eatingParticles;
    bool eating = false;
    bool mating = false;
    bool particlesTriggered = false;
    [SerializeField] GameObject matingParticles;
    SpawnSystem spawnSystem;

    [Header("Training")]
    [SerializeField] Material win;
    [SerializeField] Material loss;
    [SerializeField] Material ground;
    [SerializeField] Material rabbitEaten;
    [SerializeField] Renderer groundRenderer;
    [SerializeField] float minSpawnArea = -40.0f;
    [SerializeField] float maxSpawnArea = 40.0f;
    public override void Initialize()
    {
        navAgent = GetComponent<NavMeshAgent>();
        waitTimer = waitDuration;
        groundRenderer = GameObject.FindGameObjectWithTag("Ground").GetComponent<Renderer>();
        spawnSystem = GameObject.FindGameObjectWithTag("Spawn System").GetComponent<SpawnSystem>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(maxSpawnArea, minSpawnArea), 1, Random.Range(maxSpawnArea, minSpawnArea));
        navAgent.velocity = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        energy = 75;
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0.0f;
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 3;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Quaternion localRotation = transform.localRotation;
        Vector3 normalizedRotation = localRotation.eulerAngles / 360;
        sensor.AddObservation(normalizedRotation);
        sensor.AddObservation((transform.localPosition.x - -40) / (40 - -40)); // normalizedValue = (currentValue - minValue)/(maxValue - minValue)
        sensor.AddObservation((transform.localPosition.y - -40) / (40 - -40));
        sensor.AddObservation((transform.localPosition.z - -40) / (40 - -40));
        sensor.AddObservation(energy / 100);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 direction = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        float actionD = Mathf.FloorToInt(vectorAction[0]);

        switch (actionD)
        {
            case 1:
                direction = transform.forward;
                //SetReward(0.0001f); -- Removed during training.
                break;
            case 2:
                rotation = transform.up;
                break;
            case 3:
                rotation = -transform.up;
                break;
        }
        NavMeshHit hit;
        transform.Rotate(rotation * rotationSpeed);
        if (!navAgent.hasPath && !wait)
        {
            if (NavMesh.SamplePosition(direction + transform.localPosition, out hit, 1.0f, NavMesh.AllAreas)) 
            {
                navAgent.Move(direction/15); // /4 -- Changed during training.
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        if (other.tag == "Player")
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        if (other.tag == "Rabbit")
        {
            if (energy < 100)
            {
                energy += 20;
                AddReward(0.5f);
                wait = true;
                eating = true; 
            }

            other.transform.localPosition = new Vector3(Random.Range(maxSpawnArea, minSpawnArea), 1, Random.Range(maxSpawnArea, minSpawnArea));
        }

        if (other.tag == "Fox")
        {
            if (energy >= 80)
            {
                AddReward(1f);
                energy -= 20;
                wait = true; 
                mating = true;               
                spawnSystem.SpawnObject("Fox");
            }
            else
            {
                other.transform.localPosition = new Vector3(Random.Range(maxSpawnArea, minSpawnArea), 1, Random.Range(maxSpawnArea, minSpawnArea));
            }
        }
    }

    void FixedUpdate()
    {
        energy -= Time.deltaTime/15; // /4 -- Changed during training.
        if (energy <= 0)
        {
            SetReward(-1);
            EndEpisode();
        }
        if (wait)
        {
            if (eating)
            {
                if (!particlesTriggered)
                {
                    particlesTriggered = TriggerParticles(eatingParticles);
                }
            }
            if (mating)
            {
                if (!particlesTriggered)
                {
                    particlesTriggered = TriggerParticles(matingParticles);
                }
            }

            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                wait = false;
                eating = false;
                particlesTriggered = false;
                waitTimer = waitDuration;
                if (mating)
                {
                    EndEpisode(); 
                }
                mating = false;
            }
        }
    }

    IEnumerator SwapGroundMaterial(Material mat, float time)
    {
        groundRenderer.material = mat;
        yield return new WaitForSeconds(time);
        groundRenderer.material = ground;
    }

    bool TriggerParticles(GameObject particle)
    {
        particle.GetComponent<ParticleSystem>().Play();
        return true;
    }
}
