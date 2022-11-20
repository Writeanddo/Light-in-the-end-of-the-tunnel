using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : Trigger
{
    [SerializeField]
    GameObject[] gameObjects;
    [SerializeField]
    bool targetState;
    public override void Execute()
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(targetState);
        }
    }
}
