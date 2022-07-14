using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PrepareThrow : ActionNode
{
    Vector3 newPos;
    GameObject pickedUpEnemy = null;
    Rigidbody pickedUpEnemyRB;
    Animator animator;
    
    protected override void OnStart()
    {
        
        blackboard.RHandAnimator.SetTrigger("ThrowRot");
        GameObject closestMinion = blackboard.RHandEnemyList[0];
        for(int i = 1; i < blackboard.RHandEnemyList.Count; i++)
        {
            if (Vector3.Distance(closestMinion.transform.position, blackboard.RHandParent.transform.position) >
                Vector3.Distance(blackboard.RHandEnemyList[i].transform.position, blackboard.RHandParent.transform.position))
            {
                closestMinion = blackboard.RHandEnemyList[i];
            }
        }
        pickedUpEnemy = closestMinion;
        blackboard.pickedupEnemy = pickedUpEnemy;
        Vector3 handToEnemyMinion = blackboard.RHandEnemyList[0].transform.position - blackboard.RHandParent.transform.position;
        newPos = handToEnemyMinion.normalized * 5 + blackboard.RHandParent.transform.position;
        newPos.y += 5.0f;
        pickedUpEnemyRB = pickedUpEnemy.GetComponent<Rigidbody>();
        animator = pickedUpEnemy.transform.GetChild(0).GetComponent<Animator>();

        blackboard.pickedupEnemyIniPos = pickedUpEnemy.transform.position;
        blackboard.pickedupEnemyIniRot = pickedUpEnemy.transform.rotation;
        Debug.Log("Prepare");
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

        if (Vector3.Distance(blackboard.RHandParent.transform.position, newPos) < 0.5f)
        {
            Transform HandTransform = blackboard.RHandParent.transform;
            Vector3 handEnemyNewPos = HandTransform.position - (HandTransform.forward * 10);
            if (Vector3.Distance(handEnemyNewPos, pickedUpEnemy.transform.position) < 0.5f)
            {
                Debug.Log("PrepareEnd");
                return State.Success;
            }
                
                
            if (pickedUpEnemy != null)
            {
                animator.SetTrigger("Idle");
                pickedUpEnemy.transform.parent = null;
                
                Vector3 newForce = handEnemyNewPos - pickedUpEnemy.transform.position;
                pickedUpEnemyRB.velocity = newForce.normalized * 16;
            }
        }
        else
            blackboard.RHandParent.transform.position = Vector3.MoveTowards(blackboard.RHandParent.transform.position, newPos, Time.deltaTime * 12);
        return State.Running;


    }
}
