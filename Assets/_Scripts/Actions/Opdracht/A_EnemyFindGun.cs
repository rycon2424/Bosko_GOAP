using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_EnemyFindGun : GAction
{
    [Header("GoalSpecific")]
    public float distanceTillGoal;

    public override bool PrePerform()
    {
        List<GameObject> availableGuns = GWorld.Instance.GetAllGuns();

        GameObject closestGun = null;
        foreach (var gun in availableGuns)
        {
            if (closestGun == null)
            {
                closestGun = gun;
            }
            else
            {
                float distanceClosestgun = Vector3.Distance(transform.position, closestGun.transform.position);
                float distanceNewGun = Vector3.Distance(transform.position, gun.transform.position);
                if (distanceNewGun < distanceClosestgun)
                {
                    closestGun = gun;
                }
            }
        }
        target = closestGun;
        if (target == null)
            return false;
        inventory.AddItem(target);
        GWorld.Instance.RemoveGun(target);
        agent.SetDestination(target.transform.position);
        return true;
    }

    public override bool PostPerform()
    {
        target.SetActive(false);
        return true;
    }

    public override void ActionUpdate()
    {
        distanceTillGoal = Vector3.Distance(transform.position, target.transform.position);
        if (agent.hasPath && distanceTillGoal < 1.5f)
        {
            complete = true;
            agent.SetDestination(transform.position);
        }
    }
}
