using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAir : State
{
    public override void OnStateEnter(Player pb)
    {
        pb.cc.enabled = true;
    }

    public override void OnStateExit(Player pb)
    {

    }

    public override void StateUpdate(Player pb)
    {
        pb.Grounded();
        if (pb.anim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            if (!pb.RayHit(pb.transform.position + pb.transform.up, pb.transform.forward, 1, pb.everything))
            {
                pb.cc.Move(pb.transform.forward * Time.deltaTime * 3);
            }
        }
        if (pb.grounded && pb.ccGrounded)
        {
            pb.stateMachine.GoToState(pb, "Locomotion");
        }
    }
}
