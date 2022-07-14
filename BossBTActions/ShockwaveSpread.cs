using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveSpread : MonoBehaviour
{
    [SerializeField]
    float ShockwaveLifespan;

    float scaleIncrease = 0.0f;

    Vector3 iniPos;
    private void Start()
    {
        scaleIncrease = 12;
        Destroy(gameObject, ShockwaveLifespan);
    }
    private void FixedUpdate()
    {
        Vector3 shockwaveScale = transform.localScale;
        transform.localScale = new Vector3(shockwaveScale.x += Time.deltaTime * scaleIncrease, shockwaveScale.y, shockwaveScale.z += Time.deltaTime * scaleIncrease);
        
    }
}
