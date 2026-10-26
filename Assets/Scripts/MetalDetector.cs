using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ScanMode
{
    Standard,
    Gold
}
public class MetalDetector : MonoBehaviour
{
    [SerializeField] private List<ScanConeConfig> cones = new();
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
            // Debug.DrawLine(detectorPosition, col.transform.position, Color.red);
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

    private bool IsInsideCone(Vector3 point, Vector3 apex, Vector3 direction, float height, float radius)
    {
        var to = point - apex; // to point
        var dot = Vector3.Dot(direction, to); // distance
        if (dot < 0 || dot > height) return false;
        var radialOffset =
            to - direction * dot;

        var allowedRadius =
            radius * dot / height;

        return radialOffset.sqrMagnitude
               <= allowedRadius * allowedRadius;
    }

    private void OnDrawGizmos()
    {
        foreach (var cone in cones)
        {
            Gizmos.color = cone.DebugColor;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * cone.Height + Vector3.right * cone.Radius);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * cone.Height + Vector3.left * cone.Radius);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * cone.Height + Vector3.back * cone.Radius);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * cone.Height + Vector3.forward * cone.Radius);
            DrawGizmoCircle(transform.position + Vector3.down * cone.Height, Vector3.down, cone.Radius);
        }
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, maxRadius);
        // if (!_treasureColliders[0] || !_treasureColliders[1]) return;
        // if (!_treasure) return;
        // Gizmos.DrawLine(transform.position, _treasure.transform.position);
    }
    
    private void DrawGizmoCircle(Vector3 circleCenter, Vector3 circleNormal, float circleRadius, int segments = 32)
    {
        Vector3 radiusVector = Mathf.Abs(Vector3.Dot(circleNormal, Vector3.right)) - 1f <= Mathf.Epsilon 
            ? Vector3.Cross(circleNormal, Vector3.forward).normalized 
            : Vector3.Cross(circleNormal, Vector3.right).normalized;
        radiusVector *= circleRadius;
        float angleBetweenSegments = 360f / segments;
        Vector3 previousCircumferencePoint = circleCenter + radiusVector;
        for (int i = 0; i < segments; ++i)
        {
            radiusVector = Quaternion.AngleAxis(angleBetweenSegments, circleNormal) * radiusVector;
            Vector3 newCircumferencePoint = circleCenter + radiusVector;
            Gizmos.DrawLine(previousCircumferencePoint, newCircumferencePoint);
            previousCircumferencePoint = newCircumferencePoint;
        }
    }
}
