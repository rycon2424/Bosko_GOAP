using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public Node(Node _parent, float _cost, Dictionary<string, int> allstates, GAction _action)
    {
        parent = _parent;
        cost = _cost;
        state = new Dictionary<string, int>(allstates);
        action = _action;
    }

    public Node(Node _parent, float _cost, Dictionary<string, int> allstates, Dictionary<string, int> beliefsStates, GAction _action)
    {
        parent = _parent;
        cost = _cost;
        state = new Dictionary<string, int>(allstates);
        foreach (KeyValuePair<string, int> b in beliefsStates)
        {
            if (!state.ContainsKey(b.Key))
            {
                state.Add(b.Key, b.Value);
            }
        }
        action = _action;
    }
}

public class GPlanner
{
    public Queue<GAction> Plan(List<GAction> actions ,Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach (GAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool succes = Buildgraph(start, leaves, usableActions, goal);

        if (!succes)
        {
            Debug.Log("No plan possible");
            return null;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<GAction> result = new List<GAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction a in result)
        {
            queue.Enqueue(a);
        }
        string debug = "The plan is:";
        foreach (GAction a in queue)
        {
            debug += (" " + a.actionName);
        }
        //Debug.Log(debug);

        return queue;
    }

    private bool Buildgraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (GAction action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> e in action.effects)
                {
                    if (!currentState.ContainsKey(e.Key))
                    {
                        currentState.Add(e.Key, e.Value);
                    }
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubSet(usableActions, action);
                    bool found = Buildgraph(node, leaves, subset, goal);
                    if (found == true)
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    private List<GAction> ActionSubSet(List<GAction> actions, GAction unusableAction)
    {
        List<GAction> subset = new List<GAction>();
        foreach (GAction a in actions)
        {
            if (!a.Equals(unusableAction))
            {
                subset.Add(a);
            }
        }
        return subset;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }
}
