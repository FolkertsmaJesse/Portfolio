using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherWoodAction : GoapAction {
    
    bool pickedUp = false;
    Pickup targetItem = null;

    public GatherWoodAction()
    {
        AddPrecondition("WoodExists", true);
        AddEffect("HasWood", true);
    }

    public override bool CheckProceduralPrecondition(GameObject _agent)
    {
        Pickup[] pickups = FindObjectsOfType<Pickup>();
        Pickup closest = null;
        float dist = 0;

        foreach (Pickup w in pickups)
        {
            if (w.item != Pickup.PickupType.Wood)
                continue;
            if (!w.isFound)
                continue;

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
            targetItem = closest;
            target = targetItem.gameObject;
        }

        return true;
    }

    public override bool IsDone()
    {
        return pickedUp;
    }

    public override bool Preform(GameObject _agent)
    {
        Caveman cm = _agent.GetComponent<Caveman>();
        cm.energy -= cost;
        targetItem.PickupItem(cm);
        pickedUp = true;
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        pickedUp = false;
        targetItem = null;
    }
}
