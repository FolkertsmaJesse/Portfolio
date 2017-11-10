using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeAxeAction : GoapAction {

    bool made = false;

    float startTime = 0;
    public float makeDuration = 2;

    public MakeAxeAction()
    {
        AddPrecondition("HasAxe", false);
        AddPrecondition("HasStick", true);
        AddPrecondition("HasRock", true);
        AddEffect("HasAxe", true);
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
            cm.AddAxe();
            BackpackComponent bp = _agent.GetComponent<BackpackComponent>();
            bp.hasAxe = true;
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
        made = false;;
        startTime = 0;
    }
}
