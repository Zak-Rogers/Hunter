using UnityEngine;

public class Sequence : Composite
{
    private int currentChild;
    private Behaviour currentChildBehavior;

    protected override void OnInitialize()
    {
        currentChild = 0;
        currentChildBehavior = children[currentChild];
    }

    protected override Status Update()
    {
        while(true)
        {
            Status s = currentChildBehavior.Tick();

            if (s != Status.BH_SUCCESS)
            {
                return s;
            }

            if (++currentChild == children.Count)
            {
                currentChild = 0;
                return Status.BH_SUCCESS;
            }
            currentChildBehavior = children[currentChild];
        }

        return Status.BH_INVALID;
    }
}
