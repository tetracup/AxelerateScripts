using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ReturnToOriginalPosition : ActionNode
{
    protected override void OnStart()
    {
        blackboard.RHandAnimator.SetTrigger("Return");
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(Vector2.Distance(blackboard.RHandParent.transform.position, blackboard.iniPos.transform.position) < 0.5f)
        {
            return State.Success;
        }
        blackboard.RHandParent.transform.localRotation = Quaternion.RotateTowards(blackboard.RHandParent.transform.localRotation, Quaternion.Euler(Vector2.zero), Time.deltaTime * 18);
        blackboard.RHandParent.transform.position = Vector3.MoveTowards(blackboard.RHandParent.transform.position, blackboard.iniPos.transform.position, Time.deltaTime * 16);
        return State.Running;
    }
}
