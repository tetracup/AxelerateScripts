using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPlayerHeight : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.playerMesh.transform.position.y < 25.0f)
            return State.Success;
        else
            return State.Failure;
    }
}
