using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Axelerate.Managers;
using Axelerate.Audio;

public class PerformShockwave : ActionNode
{
    float waitTimer = 0.0f;
    float waitTimerMax = 0.5f;
    Vector3 newPos;
    Vector3 playerPos;
    SphereCollider shockwaveCol;
    GameObject shockwaveObj;
    protected override void OnStart()
    {
        newPos = new Vector3(blackboard.RHandParent.transform.position.x, 21.5f, blackboard.RHandParent.transform.position.z);
        blackboard.RHandAnimator.SetTrigger("ShockwavePerform");
    }

    protected override void OnStop()
    {
        waitTimer = 0.0f;
        shockwaveObj = null;
    }

    protected override State OnUpdate()
    {

        if (waitTimer >= waitTimerMax)
            return State.Success;

        if (Vector3.Distance(blackboard.RHandParent.transform.position, newPos) < 0.5f)
        {
            if(shockwaveObj == null)
            {
                AudioManager.Instance.PlaySound(SoundType.sfx_boss_slam);
                shockwaveObj = Instantiate(blackboard.Shockwave);
                Vector3 newShockwavePos = blackboard.RHandParent.transform.position;
                newShockwavePos.y = 22.5f;
                shockwaveObj.transform.position = newShockwavePos;


            }
            waitTimer += Time.deltaTime;
            return State.Running;
        }

        blackboard.RHandParent.transform.position = Vector3.MoveTowards(blackboard.RHandParent.transform.position, newPos, Time.deltaTime * 12);
        return State.Running;
    }
}
