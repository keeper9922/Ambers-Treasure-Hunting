using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MetalDetector : MonoBehaviour
{
    [Header("Settings")]
    public float maxRadius = 20f;
    public LayerMask treasureLayerMask;
    private Collider[] _treasureColliders;
    private Collider _treasureCollider;
    public event Action<float> OnSignalChanged;
    public event Action<GameObject> OnTreasureDetected;
    public event Action<bool> OnModeChanged;

    private float _currentSignal;
    private bool _isActive;

    public void SetActive(bool active)
    {
        if (_isActive == active)
            return;

        _isActive = active;

        OnModeChanged?.Invoke(_isActive);
    }

    private void UpdateSignal(float signal)
    {
        if (Mathf.Approximately(_currentSignal, signal))
            return;

        _currentSignal = signal;
        OnSignalChanged?.Invoke(_currentSignal);
    }

    private void DetectTreasure(GameObject treasure)
    {
        OnTreasureDetected?.Invoke(treasure);
    }
    
    [SerializeField] private float detectionInterval = 0.5f;
    private float _detectionTimer;

    private void Update()
    {
        _detectionTimer += Time.deltaTime;

        if (!(_detectionTimer >= detectionInterval)) return;
        _detectionTimer -= detectionInterval;
        Physics.OverlapSphereNonAlloc(transform.position, maxRadius, _treasureColliders, treasureLayerMask);
        if (_treasureColliders == null || _treasureColliders.Length == 0) return;
        var distance = FindNearestObject(_treasureColliders, out _treasureCollider);
        DetectTreasure(_treasureCollider.gameObject);
        ComputeNormalSignal(distance);
        UpdateSignal(distance);
    }
    
    private float FindNearestObject(Collider[] colliders, out Collider closestCollider)
    {
        Collider nearest = null;
        var minDistanceSqr = float.MaxValue;
        var detectorPosition = transform.position;

        foreach (var col in colliders)
        {
            var distanceSqr =
                (col.transform.position - detectorPosition).sqrMagnitude;

            if (!(distanceSqr < minDistanceSqr)) continue;
            minDistanceSqr = distanceSqr;
            nearest = col;
        }

        closestCollider = nearest;
        return minDistanceSqr;
    }

    private float ComputeNormalSignal(float distance)
    {
        return Mathf.Max(0, 1 - distance / maxRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
