using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCampFireAction : GoapAction {

    bool made = false;

    Cave targetCave;
    float startTime = 0;
    public float makeDuration = 3;

    public CreateCampFireAction()
    {
        //AddPrecondition("HasShelter", true);
        AddPrecondition("HasWood", true);
        AddEffect("FireExists", true);
    }

    public override bool CheckProceduralPrecondition(GameObject _agent)
    {
        Cave[] caves = FindObjectsOfType<Cave>();
        Cave closest = null;
        float dist = 0;

        foreach (Cave a in caves)
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
            targetCave = closest;
            target = targetCave.gameObject;
        }

        return closest != null;
    }

    public override bool IsDone()
    {
        return made;
    }

    public override bool Preform(GameObject _agent)
    {
        if (startTime == 0)
            startTime = Time.time;

        if (Time.time - startTime > makeDuration)
        {
            Caveman cm = _agent.GetComponent<Caveman>();
            cm.energy -= cost;
            BackpackComponent bp = GetComponent<BackpackComponent>();
            bp.wood--;
            GameObject prefab = Resources.Load<GameObject>("CampFire");
            Instantiate(prefab, transform.position + transform.forward * 2 - transform.up, transform.rotation);
            made = true;
        }
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        made = false;
        startTime = 0;
    }
}
