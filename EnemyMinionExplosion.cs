using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Axelerate.Gameplay;

namespace Axelerate.Gameplay
{
    public class EnemyMinionExplosion : MonoBehaviour
    {
        [SerializeField]
        GameObject ExplosionSphere;
        [SerializeField]
        Rigidbody RB;
        [SerializeField]
        GameObject Mesh;

        [System.NonSerialized]
        public bool wasThrown;

        private void OnCollisionEnter(Collision collision)
        {
            if (!wasThrown)
                return;
            Destroy(Mesh);
            Destroy(gameObject, 1.0f);
            RB.isKinematic = true;
            ExplosionSphere.SetActive(true);
        }
    }
}
