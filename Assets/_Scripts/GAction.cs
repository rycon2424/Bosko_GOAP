using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1;
    public GameObject target;
    public string targetTag;
    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public NavMeshAgent agent;
    public Animator anim;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentState;

    public bool running = false;
    public bool complete = false;

    public GInventory inventory;
    public WorldStates beliefs;

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        GameObject parent = transform.parent.gameObject;
        agent = parent.GetComponent<NavMeshAgent>();
        anim = parent.GetComponent<Animator>();
        if(preconditions != null)
        {
            foreach (var w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }
        if (afterEffects != null)
        {
            foreach (var w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }
        inventory = parent.GetComponent<GAgent>().inventory;
        beliefs = parent.GetComponent<GAgent>().beliefs;
    }
    
    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> p in preconditions)
        {
            if (!conditions.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true;
    }

    public abstract void ActionUpdate();
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
