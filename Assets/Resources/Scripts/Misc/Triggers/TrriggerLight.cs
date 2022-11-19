using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrriggerLight : Trigger
{
    [SerializeField]
    LightController effectedLight;
    [SerializeField]
    bool targetState;

    public override void Execute()
    {
        effectedLight.isOn = targetState;
    }
}
