using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_NinjaFollow : GAction
{
    [Header("GoalSpecific")]
    public float distanceTillGoal;

    public override bool PrePerform()
    {
        target = FindObjectOfType<Player>().gameObject;
        return true;
    }

    public override void ActionUpdate()
    {
        distanceTillGoal = Vector3.Distance(transform.position, target.transform.position);
        agent.SetDestination(target.transform.position + (transform.position - target.transform.position).normalized * 2);
        if (GWorld.Instance.GetWorld().StateValue("ChasingPlayer") > 0)
        {
            complete = true;
        }
    }

    public override bool PostPerform()
    {
        return true;
    }

}
