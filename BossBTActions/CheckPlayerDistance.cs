using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Axelerate.Audio;
using Axelerate.Managers;

public class CheckPlayerDistance : ActionNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }
    protected override State OnUpdate()
    {
        
        if (Vector3.Distance(blackboard.RHandParent.transform.parent.position, blackboard.playerMesh.transform.position) > 50.0f)
            return State.Running;
        else
        {
            GameManager.Instance.SceneManager.PlayBossMusic();
            return State.Success;
        }
            

    }
}
