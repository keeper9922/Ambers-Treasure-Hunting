using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MetalDetector : MonoBehaviour
{
    [Header("Settings")]
    public float maxRadius = 20f;
    public LayerMask treasureLayerMask;
    private readonly Collider[] _treasureColliders = new Collider[6];
    private Collider _treasure;
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

    private void Start()
    {
        SetActive(true);
    }

    private void Update()
    {
        _detectionTimer += Time.deltaTime;

        if (!(_detectionTimer >= detectionInterval)) return;
        _detectionTimer -= detectionInterval;
        Physics.OverlapSphereNonAlloc(transform.position, maxRadius, _treasureColliders, treasureLayerMask, 
            QueryTriggerInteraction.Collide);
        if (!_treasureColliders[0]) return;
        _treasure = FindNearestObject(_treasureColliders);
        if (!_treasure) return;
        DetectTreasure(_treasure.gameObject);
        var distance = Vector3.Distance(_treasure.transform.position, transform.position);
        var signal = ComputeNormalSignal(distance);
        UpdateSignal(signal);
        // Debug.Log("Object: " + treasure.name + "; Distance: " + distance + "; Signal: " + signal);
    }
    
    private Collider FindNearestObject(Collider[] colliders)
    {
        if (!colliders[0]) return null;
        Collider nearest = null;
        var minDistance = float.MaxValue;
        var detectorPosition = transform.position;
        // Debug.LogWarning("Finding distance:");
        foreach (var col in colliders)
        {
            var distance =
                Vector3.Distance(col.transform.position, detectorPosition);
            Debug.DrawLine(detectorPosition, col.transform.position, Color.red);
            // Debug.Log(col.name + " = " + distance);
            if (!(distance < minDistance)) continue;
            minDistance = distance;
            
            nearest = col;
        }
        
        return nearest;
    }

    private float ComputeNormalSignal(float distance)
    {
        return Mathf.Max(0, 1 - distance / maxRadius);
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, maxRadius);
        if (!_treasureColliders[0] || !_treasureColliders[1]) return;
        if (!_treasure) return;
        Gizmos.DrawLine(transform.position, _treasure.transform.position);
    }
}
