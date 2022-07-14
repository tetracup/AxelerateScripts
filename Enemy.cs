using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Axelerate.Audio;
using Axelerate.Managers;


namespace Axelerate.Gameplay
{
    public class Enemy : MonoBehaviour
    {
        
        NavMeshAgent agent;

        List<Transform> wayPoints;

        [System.NonSerialized]
        public bool isPlayerTargeted = false;
        [System.NonSerialized]
        public Transform playerTransform;

        [System.NonSerialized]
        public int waypointIndex = 0;

        [System.NonSerialized]
        public float collisionTimer;
        [System.NonSerialized]
        public float collisionTimerMax = 2.0f;

        CapsuleCollider enemyCol;

        [SerializeField]
        public Material[] materialList = new Material[2];

        [System.NonSerialized]
        public Animator animator;
        [System.NonSerialized]
        public MeshRenderer meshRenderer;

        [System.NonSerialized]
        public bool isDead = false;

        [SerializeField]
        GameObject ExplosionPrefab;
        GameObject explosionObj;
        bool HasSpawnedExplosion = false;

        [SerializeField]
        GameObject RightHand;
        [SerializeField]
        GameObject LeftHand;

        [SerializeField]
        SkinnedMeshRenderer headRend;

        [SerializeField]
        List<Texture2D> FaceList;

        [SerializeField]
        public enum FaceType { Anger, Cheer, Dead, Happy, Joyous};
        public Color[] FaceColors = new Color[5];

        PlayerMovement playerMovement;

        [SerializeField]
        GameObject dropPrefab;

        EnemySystem enemySys;
        private void Start()
        {
            headRend.material.SetColor("_BG_colour", FaceColors[(int)FaceType.Happy]);
            enemySys = transform.parent.GetComponent<EnemySystem>();
            wayPoints = enemySys.wayPoints;
            GameManager.Instance.EnemyManager.enemySystems.Add(transform.parent.gameObject);
            SetFace(FaceType.Happy);
            collisionTimer = collisionTimerMax;

            agent = GetComponent<NavMeshAgent>();
            agent.speed = 6.0f;
            agent.acceleration = 40.0f;

            animator = transform.GetChild(0).GetComponent<Animator>();
            meshRenderer = GetComponent<MeshRenderer>();
            enemyCol = GetComponent<CapsuleCollider>();

            RightHand.transform.GetChild(Random.Range(0, 3)).gameObject.SetActive(true);
            LeftHand.transform.GetChild(Random.Range(0, 3)).gameObject.SetActive(true);
        }
        void Update()
        {
            if (isDead)
                return;
            if (collisionTimer < collisionTimerMax)
                collisionTimer += Time.deltaTime;


            if (isPlayerTargeted)
            {
                agent.SetDestination(playerTransform.position);

                if (Vector3.Distance(gameObject.transform.position, playerTransform.position) < 2.0f)
                    agent.isStopped = true;
                else
                    agent.isStopped = false;
            }
            else if (wayPoints.Count != 0)
            {
                agent.isStopped = false;
                if (!agent.hasPath)
                    waypointIndex++;
                waypointIndex = waypointIndex % wayPoints.Count;
                agent.SetDestination(wayPoints[waypointIndex].position);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "PlayerMesh")
            {
                playerMovement = collision.transform.parent.GetChild(2).GetComponent<PlayerMovement>();
                agent.isStopped = true;
            }
                
        }
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "PlayerMesh")
            {
                if (!isDead && (collision.transform.position.y >= enemyCol.bounds.max.y - 0.3f || playerMovement.isDashing))
                {
                    SetFace(FaceType.Dead);
                    //Kills Enemy
                    Destroy(gameObject, 1.8f);
                    Destroy(enemyCol);
                    if(!enemySys.hasDroppedScrap)
                        DropScrap();
                    if(!HasSpawnedExplosion)
                    {
                        explosionObj = Instantiate(ExplosionPrefab, gameObject.transform.position, Quaternion.LookRotation(gameObject.transform.forward, transform.up));
                        HasSpawnedExplosion = true;
                    }
                    animator.SetTrigger("Death");
                    agent.enabled = false;
                    if (isDead == false)
                        AudioManager.Instance.PlaySound(SoundType.sfx_hoverboard_attack);
                    isDead = true;
                    
                }
            }
        }

        public void SetFace(FaceType face)
        {
            headRend.material.SetTexture("_Face", FaceList[(int)face]);
            headRend.material.SetColor("_BG_colour", FaceColors[(int)face]);
        }
        private void OnDestroy()
        {
            Destroy(explosionObj);
        }

        void DropScrap()
        {
            int dropAmount = 3;
            float maxDistance = 1.5f;
            for (int i = 0; i < dropAmount; i++)
            {
                var position = transform.position;
                var unitCircle = Random.insideUnitCircle;
                position += new Vector3(unitCircle.x * maxDistance,
                    0f,
                    unitCircle.y * maxDistance);
                Instantiate(dropPrefab, position, Quaternion.identity);
            }
            enemySys.hasDroppedScrap = true;
            
        }
    }
}

