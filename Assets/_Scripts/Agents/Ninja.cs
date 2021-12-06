using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : GAgent
{
    public override void Start()
    {
        base.Start();
        beliefs.ModifyState("PlayerInDanger", 0);
        SubGoal s1 = new SubGoal("protectPlayer", 1, false);
        goals.Add(s1, 3);
    }
}
