using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deer : MonoBehaviour, IGoap {
    
    NavMeshAgent navAgent;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void RunFrom(Transform _target)
    {
        navAgent.Resume();
        transform.rotation = Quaternion.LookRotation(transform.position - _target.position);

        Vector3 runTo = transform.position + transform.forward * 10;

        NavMeshHit hit;
        NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Walkable"));

        navAgent.SetDestination(hit.position);
    }

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        bool isSafe = true;
        Collider[] cols = Physics.OverlapSphere(transform.position, 10f);

        foreach(Collider c in cols)
        {
            if(c.transform.root.tag == "Caveman")
            {
                isSafe = false;
            }
        }

        worldData.Add(new KeyValuePair<string, object>("IsSafe", isSafe));

        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("Wander", true));

        return goal;
    }

    public bool MoveAgent(GoapAction nextAction)
    {
        //float step = moveSpeed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, nextAction.target.transform.position, step);
        navAgent.Resume();
        navAgent.SetDestination(nextAction.target.transform.position);

        if (Vector3.Distance(transform.position, nextAction.target.transform.position) < nextAction.ReachDistance())
        {
            nextAction.SetInRange(true);
            navAgent.Stop();

            return true;
        }
        else
            return false;
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        Debug.Log("<color=red>Plan failed!</color> " + GoapAgent.PrettyPrint(failedGoal));
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> action)
    {
        Debug.Log("<color=green>Plan found</color> " + GoapAgent.PrettyPrint(action, goal));
    }

    public void ActionsFinished()
    {
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public void PlanAborted(GoapAction aborter)
    {
        Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.PrettyPrint(aborter));
    }
}
