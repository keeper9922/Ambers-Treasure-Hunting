using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MetalDetector : MonoBehaviour
{
    public float maxRadius = 20f;

    private void Update()
    {
        // TODO: код для обнаружения 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
