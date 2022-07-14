using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class WaitForAttackTimer : ActionNode
{
    float attackTimer = 0.0f;
    float attackTimerMax = 1.5f;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        attackTimer += Time.deltaTime;

        Vector3 dir = blackboard.playerMesh.transform.position - blackboard.bossCenter.transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(-dir);
        blackboard.bossCenter.transform.rotation = Quaternion.Slerp(blackboard.bossCenter.transform.rotation, rot, 3.0f * Time.deltaTime);

        if (attackTimer >= attackTimerMax)
        {
            attackTimer = 0.0f;
            return State.Success;
        }
        return State.Running;
    }
}
