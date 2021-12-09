using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_EnemyHuntPlayer : GAction
{
    [Header("GoalSpecific")]
    public float distanceTillGoal;
    public GameObject weapon;
    private Player p;

    public override bool PrePerform()
    {
        p = FindObjectOfType<Player>();
        target = p.gameObject;
        if (target == null)
            return false;
        agent.SetDestination(target.transform.position);
        GWorld.Instance.AddChasingEnemy(gameObject);
        anim.SetBool("Running", true);
        return true;
    }

    public override bool PostPerform()
    {
        GameObject g = inventory.FindItemWithTag("Gun");
        inventory.RemoveItem(g);
        GWorld.Instance.AddWeapon(g);
        g.SetActive(true);
        weapon.SetActive(false);

        GWorld.Instance.GetWorld().ModifyState("ChasingPlayer", -1);
        GWorld.Instance.RemoveChasingEnemy(gameObject);

        anim.SetBool("Attack", false);
        return true;
    }

    public override void ActionUpdate()
    {
        distanceTillGoal = Vector3.Distance(transform.position, target.transform.position);
        if (distanceTillGoal < 1.5f)
        {
            agent.SetDestination(transform.position);
            anim.SetBool("Attack", true);
        }
        if (distanceTillGoal < 5f)
        {
            agent.SetDestination(target.transform.position + (transform.position - target.transform.position).normalized);
        }
        else
        {
            agent.SetDestination(transform.position);
            complete = true;
        }
        if (p.dead)
        {
            complete = true;
        }
    }
}
