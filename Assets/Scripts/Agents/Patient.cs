using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GAgent
{
    public override void Start()
    {
        base.Start();
        SubGoal newGoal = new SubGoal("isWaiting", 1, true);
        goals.Add(newGoal, 3);
    }
}
