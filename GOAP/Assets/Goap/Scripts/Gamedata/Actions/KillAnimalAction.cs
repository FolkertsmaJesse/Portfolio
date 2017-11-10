using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAnimalAction : GoapAction {

    bool killed = false;
    Animal targetAnimal = null;

    float startTime = 0;
    public float waitTime = 2;

    public KillAnimalAction()
    {
        AddPrecondition("HasWeapon", true);
        AddEffect("KillAnimal", true);
        AddEffect("CarcassExists", true);
    }

    public override bool CheckProceduralPrecondition(GameObject _agent)
    {
        Animal[] animals = FindObjectsOfType<Animal>();
        Animal closest = null;
        float dist = 0;

        foreach (Animal a in animals)
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
            targetAnimal = closest;
            target = targetAnimal.gameObject;
        }

        return closest != null;
    }

    public override bool IsDone()
    {
        return killed;
    }

    public override bool Preform(GameObject _agent)
    {
        if (startTime == 0)
            startTime = Time.time;

        if (targetAnimal == null)
        {
            killed = true;
            return true;
        }

        Caveman cm = _agent.GetComponent<Caveman>();
        if (!cm.ThrowSpear(targetAnimal.gameObject))
        {
            return false;
        }

        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        killed = false;
        targetAnimal = null;
        startTime = 0;
    }
}
