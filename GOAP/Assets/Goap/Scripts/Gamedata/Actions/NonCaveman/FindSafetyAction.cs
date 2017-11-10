using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindSafetyAction : GoapAction {

    bool isSafe = false;
    Caveman targetCaveman = null;

    public FindSafetyAction()
    {
        AddPrecondition("IsSafe", false);
        AddEffect("IsSafe", true);
    }

    public override bool CheckProceduralPrecondition(GameObject _agent)
    {
        Caveman[] men = FindObjectsOfType<Caveman>();
        Caveman closest = null;
        float dist = 0;

        foreach (Caveman w in men)
        {
            if (closest == null)
            {
                closest = w;
                dist = Vector3.Distance(transform.position, w.transform.position);
            }
            else
            {
                float dist2 = Vector3.Distance(transform.position, w.transform.position);
                if (dist > dist2)
                {
                    closest = w;
                    dist = dist2;
                }
            }
        }
        if (closest != null)
        {
            targetCaveman = closest;
            target = targetCaveman.gameObject;
        }

        return closest != null;
    }

    public override bool IsDone()
    {
        return isSafe;
    }

    public override bool Preform(GameObject _agent)
    {
        Deer d =_agent.GetComponent<Deer>();
        d.RunFrom(targetCaveman.transform);
        if (Vector3.Distance(transform.position, targetCaveman.transform.position) > 20)
        {
            isSafe = true;
        }
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        isSafe = false;
        targetCaveman = null;
    }
}
