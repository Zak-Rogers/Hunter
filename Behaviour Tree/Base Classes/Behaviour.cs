using UnityEngine;
    public enum Status
    {
        BH_INVALID = 0,
        BH_SUCCESS = 1,
        BH_FAILED = 2,
        BH_RUNNING = 3,
        BH_ABORTED = 4
    };

public class Behaviour
{
    public GameObject gameObject;
    public Status currentStatus;
    public string behaviourName;

    public Behaviour()
    {
        SetName("no name");
        currentStatus = Status.BH_INVALID;
    }

    protected virtual Status Update ()
    {
        return Status.BH_INVALID;
    }

    protected virtual void OnTerminate(Status status)
    {
        if (status == Status.BH_SUCCESS)
        {

        }
    }

    protected virtual void OnInitialize()
    {

    }

    public Status Tick()
    {
        if(currentStatus == Status.BH_INVALID|| currentStatus == Status.BH_SUCCESS) 
        {
            OnInitialize();
        }

        currentStatus = Update();

        if(currentStatus != Status.BH_RUNNING)
        {
            OnTerminate(currentStatus);
        }

        return currentStatus;
    }

    public void SetName (string name)
    {
        behaviourName = name;
    }

    public void SetGameObject(GameObject obj)
    {
        gameObject = obj;
    }

}
