using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : State
{

    public override void AnimatorIKUpdate(Player pb)
    {

    }

    public override void OnStateEnter(Player pb)
    {
        pb.anim.ResetTrigger("Jump");
        pb.anim.applyRootMotion = true;
        pb.cc.enabled = true;
        pb.airtime = 0;
    }

    public override void OnStateExit(Player pb)
    {

    }

    float turnSmoothVelocity;

    public override void StateUpdate(Player pb)
    {
        pb.Grounded();
        if (!pb.lockedOn)
        {
            RotateToCam(pb);
        }
        Movement(pb);
        pb.CanTarget();
        if (pb.airtime > 0.75f)
        {
            pb.anim.SetTrigger("fall");
            pb.stateMachine.GoToState(pb, "InAir");
        }
    }

    void RotateToCam(Player pb)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(x, 0f, y).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + pb.oc.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(pb.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            pb.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

    }
    
    void Movement(Player pb)
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        pb.anim.SetFloat("x", x);
        pb.anim.SetFloat("y", y);
        pb.anim.SetFloat("y+x", (Mathf.Abs(x) + Mathf.Abs(y)));

        //Walking
        if (x != 0 || y != 0)
        {
            pb.anim.SetBool("Walking", true);
        }
        else
        {
            pb.anim.SetBool("Walking", false);
        }
        //Sprinting
        if (Input.GetKey(pb.pc.sprint))
        {
            pb.anim.SetBool("Sprinting", true);
        }
        else
        {
            pb.anim.SetBool("Sprinting", false);
        }
        //Jump
        if (Input.GetKeyDown(pb.pc.jump))
        {
            pb.anim.SetTrigger("Jump");
            pb.stateMachine.GoToState(pb, "InAir");
        }
    }
}
