using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IncreaseAttackCount : ActionNode
{
    protected override void OnStart() {
        blackboard.attacksPerformed++;
        if(blackboard.attacksPerformed == 3)
        {
            blackboard.rail1.SetActive(true);
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
