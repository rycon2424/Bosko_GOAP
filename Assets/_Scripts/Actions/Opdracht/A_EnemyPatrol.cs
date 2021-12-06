using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_EnemyPatrol : GAction
{
    [Header("GoalSpecific")]
    public Transform[] waypoints;
    public float distanceTillGoal;
    private Transform currentWaypoints;
    private Player p;

    public override bool PrePerform()
    {
        p = FindObjectOfType<Player>();
        target = waypoints[Random.Range(0, waypoints.Length)].gameObject;
        agent.SetDestination(target.transform.position);
        GWorld.Instance.GetWorld().ModifyState("ChasingPlayer", 0);
        return true;
    }
    
    public override void ActionUpdate()
    {
        distanceTillGoal = Vector3.Distance(transform.position, target.transform.position);
        if (agent.hasPath && distanceTillGoal < 1f)
        {
            target = waypoints[Random.Range(0, waypoints.Length)].gameObject;
            agent.SetDestination(target.transform.position);
        }
        if (CheckIfPlayerVisible())
        {
            complete = true;
        }
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("ChasingPlayer", 1);
        return true;
    }

    bool CheckIfPlayerVisible()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, p.transform.position - transform.position);
        Debug.DrawRay(transform.position, (p.transform.position - transform.position) * 10, Color.red);
        if (Physics.Raycast(ray, out hit, 4))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (waypoints.Length > 0)
        {
            foreach (var w in waypoints)
            {
                Gizmos.DrawWireCube(w.position, Vector3.one);
            }
        }
    }
}
