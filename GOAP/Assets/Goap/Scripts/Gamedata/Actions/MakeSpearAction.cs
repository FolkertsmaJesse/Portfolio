using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSpearAction : GoapAction {

    bool made = false;

    float startTime = 0;
    public float makeDuration = 2;

    public MakeSpearAction()
    {
        AddPrecondition("HasWeapon", false);
        AddPrecondition("HasStick", true);
        AddPrecondition("HasRock", true);
        AddEffect("HasWeapon", true);
    }

    public override bool CheckProceduralPrecondition(GameObject _agent)
    {
        return true;
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
            cm.AddSpears(3);
            BackpackComponent bp = _agent.GetComponent<BackpackComponent>();
            bp.rocks--;
            bp.sticks--;
            made = true;
        }
        return true;
    }

    public override bool RequiresInRange()
    {
        return false;
    }

    public override void Reset()
    {
        made = false; ;
        startTime = 0;
    }
}
