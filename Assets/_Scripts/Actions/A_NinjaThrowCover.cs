using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class A_NinjaThrowCover : GAction
{
    [Header("GoalSpecific")]
    public GameObject smoke;
    public float smokeDuration = 2;
    public float cooldownTime = 3;
    private bool cooldown;

    public override bool PrePerform()
    {
        List<GameObject> enemies = GWorld.Instance.GetChasingEnemy();
        if (enemies == null)
            return false;

        target = enemies[Random.Range(0, enemies.Count)];

        if (target == null)
            return false;
        anim.SetBool("Hide", false);
        anim.SetBool("Walking", false);
        return true;
    }

    public override void ActionUpdate()
    {
        if (cooldown == false)
        {
            cooldown = true;
            StartCoroutine(SmokeCooldown());
        }
        if (GWorld.Instance.GetWorld().StateValue("ChasingPlayer") < 1)
        {
            complete = true;
        }
    }

    IEnumerator SmokeCooldown()
    {
        NavMeshAgent enemyAgent = target.GetComponentInParent<NavMeshAgent>();
        float agentOldSpeed = enemyAgent.speed;
        enemyAgent.speed = 1;
            
        smoke.transform.position = target.transform.position;
        smoke.transform.parent = null;
        smoke.SetActive(true);
        smoke.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(smokeDuration);

        enemyAgent.speed = agentOldSpeed;
        smoke.SetActive(false);

        yield return new WaitForSeconds(cooldownTime);

        cooldown = false;
    }

    public override bool PostPerform()
    {
        target = null;
        if (smoke.activeSelf == true)
        {
            Invoke("HideSmoke", 1);
        }
        anim.SetBool("Hide", false);
        anim.SetBool("Walking", false);
        return true;
    }

    void HideSmoke()
    {
        smoke.SetActive(false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

}
