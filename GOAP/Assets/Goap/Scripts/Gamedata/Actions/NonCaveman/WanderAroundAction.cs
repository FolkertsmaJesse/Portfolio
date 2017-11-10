using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAroundAction : GoapAction {

    bool wandered = false;

    public WanderAroundAction()
    {
        AddPrecondition("IsSafe", true);
        AddEffect("Wander", true);
    }

    public override bool CheckProceduralPrecondition(GameObject _agent)
    {
        return true;
    }

    public override bool IsDone()
    {
        return wandered;
    }

    public override bool Preform(GameObject _agent)
    {
        wandered = true;
        return true;
    }

    public override bool RequiresInRange()
    {
        return false;
    }

    public override void Reset()
    {
        wandered = false;
    }
}
