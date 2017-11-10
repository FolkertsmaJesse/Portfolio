using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeaponAction : GoapAction {

    bool pickedUp = false;
    Weapon targetWeapon = null;

    public PickupWeaponAction()
    {
        AddPrecondition("HasWeapon", false);
        AddEffect("HasWeapon", true);
    }

    public override bool CheckProceduralPrecondition(GameObject _agent)
    {
        Weapon[] weapons = FindObjectsOfType<Weapon>();
        Weapon closest = null;
        float dist = 0;

        foreach (Weapon w in weapons)
        {
            if (w.isFound)
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
        }
        if (closest != null)
        {
            targetWeapon = closest;
            target = targetWeapon.gameObject;
        }

        return closest != null;
    }

    public override bool IsDone()
    {
        return pickedUp;
    }

    public override bool Preform(GameObject _agent)
    {
        Caveman cm = _agent.GetComponent<Caveman>();
        cm.energy -= cost;
        cm.AddSpears(3);
        //BackpackComponent bp = _agent.GetComponent<BackpackComponent>();
        //bp.spears++;
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
        targetWeapon = null;
    }
}
