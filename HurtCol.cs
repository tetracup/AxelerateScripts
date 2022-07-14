using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Axelerate.Gameplay
{
    public class HurtCol : MonoBehaviour
    {
        float hurtTimer = 0.0f;
        float hurtTimerMax = 5.0f;
        bool canHurtPlayer = true;

        private void OnTriggerEnter(Collider other)
        {
            if (canHurtPlayer && other.tag == "PlayerMesh")
            {
                other.transform.parent.GetChild(2).GetComponent<PlayerHealth>().LoseLives(1);
                canHurtPlayer = false;
            }
        }

        private void Update()
        {
            if(!canHurtPlayer)
            {
                if(hurtTimer < hurtTimerMax)
                    hurtTimer += Time.deltaTime;
                else
                {
                    hurtTimer = 0.0f;
                    canHurtPlayer = true;
                }
            }
        }
    }
}
