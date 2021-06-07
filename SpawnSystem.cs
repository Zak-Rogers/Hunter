using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnSystem : MonoBehaviour
{
    [Header("EcoSystem Settings")]
    [SerializeField] private int numOfCarrots = 5;
    [SerializeField] private int numOfRabbits = 5;
    [SerializeField] private int numOfFoxs = 5;

    [SerializeField] private int minSpawnPosition = 0;
    [SerializeField] private int maxSpawnPosition = 1000;

    [SerializeField] private GameObject carrot;
    [SerializeField] private GameObject rabbit;
    [SerializeField] private GameObject fox;

    private List<GameObject> carrots = new List<GameObject>();
    private List<GameObject> rabbits = new List<GameObject>();
    private List<GameObject> foxes = new List<GameObject>();

    [Header("performance testing")]
    private float rabbitCount;
    private float foxCount;
    private float numOfRSpawned;
    private float numOfFSpawned;
    bool record = false;

    [Header("Enviroment Settings")]
    [SerializeField] private float numOfTrees = 5;
    [SerializeField] private float numOfRocks = 5;
    [SerializeField] private GameObject[] trees;
    [SerializeField] private GameObject[] rocks;
    [SerializeField] private Transform enviromentProps;
    [SerializeField] private Transform ecoSystem;


    public List<GameObject> Carrots
    {
        get { return carrots; }
    }

    public List<GameObject> Rabbits
    {
        get { return rabbits; }
    }

    public List<GameObject> Foxes
    {
        get { return foxes; }
    }

    public float RabbitCount
    {
        get { return rabbitCount; }
    }

    public float FoxCount
    {
        get { return foxCount; }
    }
    public float NumOfRSpawned
    {
        get { return numOfRSpawned; }
    }
    public float NumOfFSpawned
    {
        get { return numOfFSpawned; }
    }

    void Awake()
    {
        for ( int i = 0; i < numOfCarrots; i++)
        {
            SpawnObject("Carrot");
        }

        for (int i = 0; i < numOfRabbits; i++)
        {
            SpawnObject("Rabbit");
        }

        for (int i = 0; i < numOfFoxs; i++)
        {
            SpawnObject("Fox");
        }

        record = true;
    }

    private void Update()
    {
        if(carrots.Count <= numOfCarrots)
        {
            SpawnObject("Carrot");
        }

        rabbitCount = rabbits.Count;
        foxCount = foxes.Count;
    }

    //Function for use in the editor.
    public void SpawnEnviroment()
    {
        for (int i = 0; i < numOfTrees; i++)
        {
            SpawnObject("Tree");
        }

        for (int i = 0; i < numOfRocks; i++)
        {
            SpawnObject("Rock");
        }
    }

    public void SpawnObject(string type)
    {
        GameObject obj;
        switch(type)
        {
            case "Carrot":
                obj = Instantiate(carrot, RandomPosition(), Quaternion.identity, ecoSystem);
                carrots.Add(obj);
                break;
            case "Rabbit":
                obj = Instantiate(rabbit, RandomPosition(), Quaternion.identity, ecoSystem);
                rabbits.Add(obj);
                if (record) numOfRSpawned++;
                break;
            case "Fox":
                obj = Instantiate(fox, RandomPosition(), Quaternion.identity, ecoSystem);
                foxes.Add(obj);
                if (record) numOfFSpawned++;
                break;
            case "Tree":
                obj = Instantiate(trees[Random.Range(0, trees.Length)], RandomPosition(), Quaternion.identity, enviromentProps);
                break;
            case "Rock":
                obj = Instantiate(rocks[Random.Range(0, rocks.Length)], RandomPosition(), Quaternion.identity, enviromentProps);
                break;
        }

    }
    
    private Vector3 RandomPosition()
    {
        int x = Random.Range(minSpawnPosition, maxSpawnPosition);
        int y = 0;
        int z = Random.Range(minSpawnPosition, maxSpawnPosition);

        Vector3 position = new Vector3(x, y, z);
        NavMeshHit hit;

        while(!NavMesh.SamplePosition(position, out hit, 1, NavMesh.AllAreas))
        {
            x = Random.Range(minSpawnPosition, maxSpawnPosition);
            y = 0;
            z = Random.Range(minSpawnPosition, maxSpawnPosition);
            position = new Vector3(x, y, z);

        }
        return position;

    }
}
