using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Axelerate.Audio;
using Axelerate.Managers;

public class PerformSweep : ActionNode
{
    float waitTimer = 0.0f;
    float waitTimerMax = 0.5f;
    float revTimer = 0.0f;
    float revTimerMax = 0.0f;
    float anglesPerSecond = 135.0f;
    float endX;
    Vector3 iniPos;
    protected override void OnStart() {
        revTimerMax = 360 / anglesPerSecond;
        endX = -blackboard.RHandParent.transform.localPosition.x;
        AudioManager.Instance.PlaySound(SoundType.sfx_boss_swoop);
    }

    protected override void OnStop() {
        revTimer = 0.0f;
    }

    protected override State OnUpdate() {
        revTimer += Time.deltaTime;
        Vector3 playerPos = blackboard.playerMesh.transform.position;
        if(revTimer >= revTimerMax) //blackboard.RHandParent.transform.localPosition.x >= endX
        {
            if (waitTimer < waitTimerMax)
            {
                waitTimer += Time.deltaTime;
                return State.Running;
            }
            else
            {
                waitTimer = 0.0f;
                return State.Success;
            }
        }
        Vector3 curPos = blackboard.RHandParent.transform.localPosition;
        Vector3 curRot = blackboard.RHandParent.transform.localRotation.eulerAngles;

        Vector2 vecFromBoss = new Vector2(curPos.x, curPos.z);
        curRot.y = Vector2.Angle(vecFromBoss, new Vector2(blackboard.iniPos.transform.localPosition.z, blackboard.iniPos.transform.localPosition.x));
        if (curPos.x > 0)
            curRot.y = -curRot.y;
        blackboard.RHandParent.transform.localRotation = Quaternion.Euler(curRot);

        //blackboard.RHandParent.transform.localPosition = Vector3.MoveTowards(blackboard.RHandParent.transform.localPosition, newPos, Time.deltaTime * 18);
        blackboard.RHandParent.transform.position = RotatePointAroundPivot(blackboard.RHandParent.transform.position, blackboard.bossCenter.transform.position, new Vector3(0, -Time.deltaTime * anglesPerSecond,0));
        return State.Running;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
