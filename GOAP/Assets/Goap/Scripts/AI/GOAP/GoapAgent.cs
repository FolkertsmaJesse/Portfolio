using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GoapAgent : MonoBehaviour {
    
    FSM stateMachine;

    FSM.FSMState idleState;
    FSM.FSMState moveToState;
    FSM.FSMState performActionState;

    HashSet<GoapAction> availableActions;
    Queue<GoapAction> currentActions;

    IGoap dataProvider;

    GoapPlanner planner;

    void Start()
    {
        stateMachine = new FSM();
        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();
        FindDataProvider();
        CreateIdleState();
        CreateMoveToState();
        CreatePreformActionState();
        stateMachine.PushState(idleState);
        LoadActions();
    }

    void Update()
    {
        stateMachine.Update(gameObject);
    }

    public void AddAction(GoapAction _action)
    {
        availableActions.Add(_action);
    }

    public GoapAction GetAction(Type _action)
    {
        foreach(GoapAction g in availableActions)
        {
            if (g.GetType().Equals(_action))
                return g;
        }
        return null;
    }

    public void RemoveAction(GoapAction _action)
    {
        availableActions.Remove(_action);
    }

    bool HasActionPlan()
    {
        return currentActions.Count > 0;
    }

    //bool isPlanning = false;
    //HashSet<KeyValuePair<string, object>> worldState;
    //HashSet<KeyValuePair<string, object>> goal;

    void CreateIdleState()
    {
        idleState = (fsm, gameObj) =>
        {
            HashSet<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState();
            HashSet<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState();

            Queue<GoapAction> plan = planner.Plan(gameObject, availableActions, worldState, goal);
            if (plan != null)
            {
                currentActions = plan;
                dataProvider.PlanFound(goal, plan);

                fsm.PopState();
                fsm.PushState(performActionState);
            }
            else
            {
                Debug.Log("<color=orange>Failed Plan</color>" + PrettyPrint(goal));
                dataProvider.PlanFailed(goal);
                fsm.PopState();
                fsm.PushState(idleState);
            }

            //if (!isPlanning)
            //{
            //    worldState = dataProvider.GetWorldState();
            //    goal = dataProvider.CreateGoalState();

            //    planner.donePlanning = false;
            //    planner.finalPlan = null;
            //    StartCoroutine(planner.Plan(gameObject, availableActions, worldState, goal, this));
            //    isPlanning = true;
            //}
            //else
            //{
            //    if (planner.donePlanning)
            //    {
            //        if (planner.finalPlan != null)
            //        {
            //            currentActions = planner.finalPlan;
            //            dataProvider.PlanFound(goal, planner.finalPlan);

            //            fsm.PopState();
            //            fsm.PushState(performActionState);
            //        }
            //        else
            //        {
            //            Debug.Log("<color=orange>Failed Plan</color>" + PrettyPrint(goal));
            //            dataProvider.PlanFailed(goal);
            //            fsm.PopState();
            //            fsm.PushState(idleState);
            //        }
            //        isPlanning = false;
            //    }
            //}
        };
    }

    void CreateMoveToState()
    {
        moveToState = (fsm, gameObj) =>
        {
            GoapAction action = currentActions.Peek();
            if(action.RequiresInRange() && action.target == null)
            {
                Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                fsm.PopState();
                fsm.PopState();
                fsm.PushState(idleState);
                return;
            }

            if (dataProvider.MoveAgent(action))
            {
                fsm.PopState();
            }
        };
    }

    void CreatePreformActionState()
    {
        performActionState = (fsm, gameObj) =>
        {
            if (!HasActionPlan())
            {
                Debug.Log("<color=red>Done actions</color>");
                fsm.PopState();
                fsm.PushState(idleState);
                dataProvider.ActionsFinished();
                return;
            }

            GoapAction action = currentActions.Peek();
            if (action.IsDone())
            {
                currentActions.Dequeue();
            }

            if (HasActionPlan())
            {
                action = currentActions.Peek();
                bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

                if (inRange)
                {
                    bool succes = action.Preform(gameObj);

                    if (!succes)
                    {
                        fsm.PopState();
                        fsm.PushState(idleState);
                        dataProvider.PlanAborted(action);
                    }
                }
                else
                {
                    fsm.PushState(moveToState);
                }
            }
            else
            {
                fsm.PopState();
                fsm.PushState(idleState);
                dataProvider.ActionsFinished();
            }
        };
    }

    void FindDataProvider()
    {
		foreach (Component comp in GetComponents(typeof(Component)) ) {
			if ( typeof(IGoap).IsAssignableFrom(comp.GetType()) ) {
				dataProvider = (IGoap)comp;
				return;
			}
		}
    }

    void LoadActions()
    {
        GoapAction[] actions = GetComponents<GoapAction>();
        foreach(GoapAction a in actions)
        {
            if(a.canUse)
                availableActions.Add(a);
        }
        Debug.Log("Found Actions" + PrettyPrint(actions));
    }

    public static string PrettyPrint(HashSet<KeyValuePair<string, object>> state)
    {
        string s = "";
        foreach(KeyValuePair<string,object> kvp in state)
        {
            s += kvp.Key + ":" + kvp.Value.ToString();
            s += ", ";
        }
        return s;
    }

    public static string PrettyPrint(Queue<GoapAction> actions, HashSet<KeyValuePair<string, object>> goal)
    {
        string s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += "-> ";
        }
        s += "GOALS REACHED: ";
        foreach (KeyValuePair<string, object> kvp in goal)
        {
            s += kvp.Key;
            s += "; ";
        }
        //s += "GOAL";
        return s;
    }

    public static string PrettyPrint(GoapAction[] actions)
    {
        string s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += ", ";
        }
        return s;
    }

    public static string PrettyPrint(GoapAction action)
    {
        string s = "" + action.GetType().Name;
        return s;
    }
}
