using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevelEnd : Trigger
{
    public override void Execute()
    {
        Player.GetInstance().LoadNextLevel();
    }
}
