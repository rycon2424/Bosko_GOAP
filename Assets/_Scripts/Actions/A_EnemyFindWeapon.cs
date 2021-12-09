using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_EnemyFindWeapon : GAction
{
    [Header("GoalSpecific")]
    public float distanceTillGoal;
    public GameObject weapon;

    public override bool PrePerform()
    {
        List<GameObject> availableWeapons = GWorld.Instance.GetAllWeapons();

        GameObject closestWeapon = null;
        foreach (var weap in availableWeapons)
        {
            if (closestWeapon == null)
            {
                closestWeapon = weap;
            }
            else
            {
                float distanceClosestgun = Vector3.Distance(transform.position, closestWeapon.transform.position);
                float distanceNewGun = Vector3.Distance(transform.position, weap.transform.position);
                if (distanceNewGun < distanceClosestgun)
                {
                    closestWeapon = weap;
                }
            }
        }
        target = closestWeapon;
        if (target == null)
            return false;
        inventory.AddItem(target);
        GWorld.Instance.RemoveWeapon(target);
        agent.SetDestination(target.transform.position);
        anim.SetBool("Running", true);
        return true;
    }

    public override bool PostPerform()
    {
        weapon.SetActive(true);
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
