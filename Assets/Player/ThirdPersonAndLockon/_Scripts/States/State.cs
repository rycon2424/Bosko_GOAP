using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public virtual void OnStateEnter(Player pb)
    {
    }

    public virtual void OnStateExit(Player pb)
    {
    }

    public virtual void StateUpdate(Player pb)
    {
    }

    public virtual void StateLateUpdate(Player pb)
    {
    }

    public virtual void StateFixedUpdate(Player pb)
    {
    }

    public virtual void AnimatorIKUpdate(Player pb)
    {
    }

}
