using System.Collections.Generic;
using UnityEngine;

public class Composite : Behaviour
{


    protected List<Behaviour> children = new List<Behaviour>();

    public void AddChild(Behaviour child)
    {
        children.Add(child);
        child.SetGameObject(gameObject);
    }
}
