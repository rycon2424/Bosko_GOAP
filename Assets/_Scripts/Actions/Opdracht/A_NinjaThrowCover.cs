using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_NinjaThrowCover : GAction
{
    public override bool PrePerform()
    {
        Debug.Log("start throwing smoke");
        target = FindObjectOfType<Player>().gameObject;
        return true;
    }

    public override void ActionUpdate()
    {
        if (GWorld.Instance.GetWorld().StateValue("ChasingPlayer") < 1)
        {
            Debug.Log("Reset");
            complete = true;
        }
    }

    public override bool PostPerform()
    {
        Debug.Log("Done with smoking");
        return true;
    }

}
