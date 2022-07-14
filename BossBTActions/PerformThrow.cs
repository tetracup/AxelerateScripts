using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TheKiwiCoder;
using Axelerate.Gameplay;

public class PerformThrow : ActionNode
{
    BoxCollider col;
    GameObject pickedUpEnemy = null;
    Rigidbody pickedUpEnemyRB;
    Vector3 newPos;
    Vector3 iniEnemyPos;
    Rigidbody playerRB;
    
    protected override void OnStart() {
        playerRB = blackboard.playerMesh.transform.parent.GetChild(1).GetComponent<Rigidbody>();
       
        pickedUpEnemy = blackboard.pickedupEnemy;
        pickedUpEnemyRB = pickedUpEnemy.GetComponent<Rigidbody>();
        pickedUpEnemy.GetComponent<EnemyMinionExplosion>().wasThrown = true;
        pickedUpEnemy.layer = 0;

        //Vector3 handOffsetStart = blackboard.RHandParent.transform.position - blackboard.RHandParent.transform.forward * 10;
        //Vector3 playerFuturePos;
        //if (!blackboard.isPlayerGrinding)
        //    playerFuturePos = playerRB.velocity;
        //else
        //    playerFuturePos = blackboard.playerMesh.transform.position + blackboard.playerMesh.transform.forward * 10.0f;
        //Vector3 handToPlayer = playerFuturePos - handOffsetStart;
        //newPos = blackboard.playerMesh.transform.position;

        //newPos = blackboard.RHandParent.transform.position + (handToPlayer.normalized * 20);

        newPos = blackboard.playerMesh.transform.position;
        blackboard.RHandAnimator.SetTrigger("ThrowPerform");
        Debug.Log("Perform");
    }
    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        //

        //
        if (pickedUpEnemy != null)
        {
            Vector3 playerFuturePos;
            if (!blackboard.isPlayerGrinding)
                playerFuturePos = blackboard.playerMesh.transform.position + playerRB.velocity;
            else
                playerFuturePos = blackboard.playerMesh.transform.position + blackboard.playerMesh.transform.forward * 10.0f;
            Vector3 handToPlayer = playerFuturePos - blackboard.pickedupEnemy.transform.position;
            newPos = Vector3.MoveTowards(newPos, blackboard.RHandParent.transform.position + (handToPlayer.normalized * 15), Time.deltaTime * 30.0f);

            pickedUpEnemy.transform.parent = null;
            Transform HandTransform = blackboard.RHandParent.transform;
            Vector3 newForce = (HandTransform.position - (HandTransform.forward * 10)) - pickedUpEnemy.transform.position;
            pickedUpEnemyRB.velocity = newForce.normalized * 13;
        }
        else
            return State.Success;

        //if (Vector3.Distance(blackboard.RHandParent.transform.position, newPos) < 0.5f)
        //{
        //    InitialiseEnemyMinion();
        //    return State.Success;
        //}
        if (Vector3.Distance(blackboard.playerMesh.transform.position, blackboard.RHandParent.transform.position) < 15.0f)
        {
            InitialiseEnemyMinion();
            Debug.Log("PerformEnd");
            return State.Success;
        }

        blackboard.RHandParent.transform.position = Vector3.MoveTowards(blackboard.RHandParent.transform.position, newPos, Time.deltaTime * 15);
        return State.Running;


    }
    void InitialiseEnemyMinion()
    {
        GameObject newMinion = Instantiate(blackboard.EnemyMinion);
        newMinion.transform.parent = blackboard.EnemyMinions.transform;
        newMinion.transform.position = blackboard.pickedupEnemyIniPos;
        newMinion.transform.rotation = blackboard.pickedupEnemyIniRot;
        blackboard.RHandEnemyList.Add(newMinion);
        blackboard.RHandEnemyList.Remove(pickedUpEnemy);
    }

    
    
}
