using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : GAgent
{
    public override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("killPlayer", 1, false);
        goals.Add(s1, 3);
    }

    public void KillPlayer()
    {
        if (Vector3.Distance(transform.position, FindObjectOfType<Player>().transform.position) < 1.6f)
        {
            GWorld.Instance.ResetWorld();
            SceneManager.LoadScene(0);
        }
    }

}
