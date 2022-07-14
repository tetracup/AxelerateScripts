using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Axelerate.Channels;

namespace Axelerate
{
    public class KillBoss : MonoBehaviour
    {
        [SerializeField]
        VoidEventChannelSO winChannel;
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "PlayerMesh")
            {
                winChannel.RaiseEvent();
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
