using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_NinjaHide : GAction
{
    [Header("GoalSpecific")]
    public float distanceTillGoal;

    public override bool PrePerform()
    {
        List<GameObject> availableSpots = GWorld.Instance.GetAllHidingSpots();

        GameObject closestSpot = null;
        foreach (var spots in availableSpots)
        {
            if (closestSpot == null)
            {
                closestSpot = spots;
            }
            else
            {
                float distanceClosestgun = Vector3.Distance(transform.position, closestSpot.transform.position);
                float distanceNewGun = Vector3.Distance(transform.position, spots.transform.position);
                if (distanceNewGun < distanceClosestgun)
                {
                    closestSpot = spots;
                }
            }
        }
        target = closestSpot;
        if (target == null)
            return false;
        agent.SetDestination(target.transform.position);
        return true;
    }

    public override void ActionUpdate()
    {
        distanceTillGoal = Vector3.Distance(transform.position, target.transform.position);
        if (distanceTillGoal < 1f)
        {
            complete = true;
        }
    }

    public override bool PostPerform()
    {
        return true;
    }
    
}
