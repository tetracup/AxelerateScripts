using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PrepareShockwave : ActionNode
{
    BoxCollider col;
    float shockwaveTimer = 0.0f;
    float shockwaveTimerMax = 2.5f;

    protected override void OnStart()
    {
        blackboard.RHandAnimator.SetTrigger("ShockwaveRot");
    }

    protected override void OnStop()
    {
        shockwaveTimer = 0.0f;
    }

    protected override State OnUpdate()
    {
        shockwaveTimer += Time.deltaTime;

        if (shockwaveTimer >= shockwaveTimerMax)
            return State.Success;
        Vector3 playerPos = blackboard.playerMesh.transform.position;

        Vector2 XZbossCenter = new Vector2(blackboard.bossCenter.transform.position.x, blackboard.bossCenter.transform.position.z);
        Vector3 newPos = new Vector3(playerPos.x, 35, playerPos.z);
        Vector3 targetPos = Vector3.MoveTowards(blackboard.RHandParent.transform.position, newPos, Time.deltaTime * 16);
        Vector2 XZRHand = new Vector2(targetPos.x, targetPos.z);
        Vector3 playerLocalPos = blackboard.bossCenter.transform.InverseTransformPoint(playerPos);

        if (8.0f < Vector2.Distance(XZRHand, XZbossCenter) && Vector2.Distance(XZRHand, XZbossCenter) < 25.0f && playerLocalPos.z < -8.0f)
            blackboard.RHandParent.transform.position = targetPos;

        //BossRot
        Vector3 dir = blackboard.playerMesh.transform.position - blackboard.bossCenter.transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(-dir);
        blackboard.bossCenter.transform.rotation = Quaternion.Slerp(blackboard.bossCenter.transform.rotation, rot, 3.0f * Time.deltaTime);
        return State.Running;
        
    }
}
