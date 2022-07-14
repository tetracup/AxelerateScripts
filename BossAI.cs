using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Axelerate.Gameplay
{
    public class BossAI : MonoBehaviour
    {
        enum BossState
        {
            Sweep,
            Shockwave,
            Throw,
            Idle
        }
        BossState bossState = BossState.Idle;

        enum BossStage
        {
            First,
            Second,
            Third
        }

        BossStage bossStage = BossStage.First;

        float attackTimer = 10.0f;
        float attackTimerMax = 10.0f;
        [SerializeField]
        Rigidbody RHandRB;
        BoxCollider col;
        [SerializeField]
        GameObject player;
        [SerializeField]
        GameObject RHand;
        Animator RHandAnimator;
        Transform iniTransform;

        void Start()
        {
            iniTransform = transform;
            RHandAnimator = RHand.GetComponent<Animator>();
            col = RHand.GetComponent<BoxCollider>();
        }

        void FixedUpdate()
        {
            if (bossState == BossState.Idle && attackTimer < attackTimerMax)
                attackTimer += Time.deltaTime;
            if (attackTimer >= attackTimerMax)
            {
                attackTimer = 0;
                switch(bossStage)
                {
                    case BossStage.First:
                        bossState = BossState.Sweep;
                        break;
                }
            }

            switch(bossState)
            {
                case BossState.Sweep:
                    Sweep();
                    break;
                case BossState.Shockwave:
                    Shockwave();
                    break;
                case BossState.Throw:
                    Throw();
                    break;
            }
        }

        void Sweep()
        {
            Vector3 newPos = new Vector3(iniTransform.position.x, player.transform.position.y + col.bounds.size.y/2, player.transform.position.z);
            if (RHandRB.position == Vector3.MoveTowards(RHandRB.position, newPos, Time.deltaTime * 6))
                RHandAnimator.SetTrigger("SweepRHandRotReverse");
            RHandRB.position = Vector3.MoveTowards(RHandRB.position, newPos, Time.deltaTime * 6);
            RHandAnimator.SetTrigger("SweepRHandRot");
        }

        void Shockwave()
        {

        }

        void Throw()
        {

        }

        bool isPlaying(Animator anim, string stateName)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                return true;
            else
                return false;
        }
    }


}
