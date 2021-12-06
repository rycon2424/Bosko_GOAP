using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GAgent
{
    public override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("killPlayer", 1, false);
        goals.Add(s1, 3);
    }
}
