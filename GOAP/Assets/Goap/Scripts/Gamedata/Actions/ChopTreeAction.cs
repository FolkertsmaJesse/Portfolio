using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTreeAction : GoapAction {

    bool chopped = false;
    ChopableTree targetTree = null;

    float startTime = 0;
    public float harvestDuration = 2;

    public ChopTreeAction()
    {
        AddPrecondition("HasAxe", true);
        AddEffect("ChopDownTree", true);
        AddEffect("WoodExists", true);
    }

    public override bool CheckProceduralPrecondition(GameObject _agent)
    {
        ChopableTree[] animals = FindObjectsOfType<ChopableTree>();
        ChopableTree closest = null;
        float dist = 0;

        foreach (ChopableTree a in animals)
        {
            if (a.isFound)
            {
                if (closest == null)
                {
                    closest = a;
                    dist = Vector3.Distance(transform.position, a.transform.position);
                }
                else
                {
                    float dist2 = Vector3.Distance(transform.position, a.transform.position);
                    if (dist > dist2)
                    {
                        closest = a;
                        dist = dist2;
                    }
                }
            }
        }
        if (closest != null)
        {
            targetTree = closest;
            target = targetTree.gameObject;
        }

        return closest != null;
    }

    public override bool IsDone()
    {
        return chopped;
    }

    public override bool Preform(GameObject _agent)
    {
        if (startTime == 0)
            startTime = Time.time;

        if (Time.time - startTime > harvestDuration)
        {
            Caveman cm = _agent.GetComponent<Caveman>();
            cm.energy -= cost;
            targetTree.ChopDown();
            chopped = true;
        }
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        chopped = false;
        targetTree = null;
        startTime = 0;
    }
}
