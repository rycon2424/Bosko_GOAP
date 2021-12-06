using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
    
public class SubGoal
{
    public Dictionary<string, int> subgoals;
    public bool remove;

    public SubGoal(string _string, int _int, bool _remove)
    {
        subgoals = new Dictionary<string, int>();
        subgoals.Add(_string, _int);
        remove = _remove;
    }
}


public class GAgent : MonoBehaviour
{
    public GInventory inventory = new GInventory();
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public WorldStates beliefs = new WorldStates();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;
    Text actionText;

    // Start is called before the first frame update
    public virtual void Start()
    {
        GAction[] acts = GetComponentsInChildren<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a);
        }
        actionText = GetComponentInChildren<Text>();
    }

    bool invoked = false;
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        currentAction.complete = false;
        invoked = false;
    }

    private void Update()
    {
        if (currentAction != null && currentAction.running)
        {
            currentAction.ActionUpdate();
        }
    }

    private void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            actionText.text = currentAction.actionName;
            if (currentAction.complete)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.Plan(actions, sg.Key.subgoals, beliefs);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null; // Forces to create new plan
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }
                if (currentAction.target != null)
                {
                    currentAction.running = true;
                }
            }
            else
            {
                actionQueue = null; // Forces to create new plan
            }
        }
    }
}
