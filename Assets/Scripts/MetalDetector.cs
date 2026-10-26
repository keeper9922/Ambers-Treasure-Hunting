using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum ScanMode
{
    Disabled,
    Standard,
    Gold,
    Precise,
}
public class MetalDetector : MonoBehaviour
{
    [SerializeField] private List<ScanConeConfig> cones = new();
    [Header("Settings")]
    [SerializeField]
    private LayerMask treasureLayerMask;

    [SerializeField, Min(1)]
    private int bufferSize = 64;

    private Collider[] _treasureColliders;

    private ScanResult? _currentScan;

    private float _currentSignal;
    private ScanMode _currentScanMode = ScanMode.Standard;
    
    private void Awake()
    {
        _treasureColliders = new Collider[bufferSize];
    }
    
    [SerializeField] private float detectionInterval = 0.5f;
    private float _detectionTimer;

    private void Start()
    {
        SetMode(ScanMode.Standard);
    }

    private void Update()
    {
        _detectionTimer += Time.deltaTime;

        if (!(_detectionTimer >= detectionInterval)) return;
        _detectionTimer -= detectionInterval;
        
        var scanConfig = cones.FirstOrDefault(s => s.Mode == _currentScanMode);
        _currentScan = FindNearestTreasure(scanConfig);
        DetectTreasure(_currentScan);
        // Physics.OverlapSphereNonAlloc(transform.position, maxRadius, _treasureColliders, treasureLayerMask, 
        //     QueryTriggerInteraction.Collide);
        // if (!_treasureColliders[0]) return;
        // _treasure = FindNearestObject(_treasureColliders);
        // if (!_treasure) return;
        // DetectTreasure(_treasure.gameObject);
        // var distance = Vector3.Distance(_treasure.transform.position, transform.position);
        // var signal = ComputeNormalSignal(distance);
        // UpdateSignal(signal);
        // Debug.Log("Object: " + treasure.name + "; Distance: " + distance + "; Signal: " + signal);
    }

    private void SetMode(ScanMode mode)
    {
        _currentScanMode = mode;
        MetalDetectorEvents.OnModeChanged(_currentScanMode);
    }

    private void UpdateSignal(float signal)
    {
        if (Mathf.Approximately(_currentSignal, signal))
            return;
    
        _currentSignal = signal;
        MetalDetectorEvents.OnOnSignalChanged(signal);
    }

    private void DetectTreasure(ScanResult? next)
    {
        var changed =
            _currentScan.HasValue != next.HasValue;

        if (!changed && _currentScan.HasValue)
        {
            if (next != null)
                changed =
                    _currentScan.Value.Target != next.Value.Target 
                    || 
                    _currentScan.Value.ConeId != next.Value.ConeId;
        }

        if (!changed)
            return;

        _currentScan = next;
        MetalDetectorEvents.OnDetectionChanged(_currentScan);
    }

    private ScanResult? FindNearestTreasure(ScanConeConfig config)
    {
        var height = config.Height;
        var radius = config.Radius;
        var apex = transform.TransformPoint(config.OriginOffset);
        var direction = transform.TransformDirection(config.LocalDirection);
        direction.Normalize();
        var sphereRadius = Mathf.Sqrt(height * height + radius * radius);
        var count = Physics.OverlapSphereNonAlloc(
            apex,
            sphereRadius,
            _treasureColliders,
            treasureLayerMask,
            QueryTriggerInteraction.Collide);
        
        Debug.LogWarning(count);
        
        if(count >= bufferSize) Debug.LogWarning("Результат поиска потенциально неполон.");
        
        Transform nearest = null;
        var nearestDistanceSqr = float.MaxValue;
        for (var i = 0; i < count; i++)
        {
            var candidate = _treasureColliders[i];

            if (!candidate)
                continue;
            
            var target = candidate.transform;
            var point = candidate.ClosestPoint(apex);
            if (!IsInsideCone(point, apex, direction, height, radius)) continue;
            var distanceSqr = (target.position - apex).sqrMagnitude;
            if (distanceSqr >= nearestDistanceSqr)
                continue;
            
            nearestDistanceSqr = distanceSqr;
            nearest = target;
        }
        if (!nearest)
            return null;
        
        return new ScanResult(
            config.Id,
            nearest,
            Mathf.Sqrt(nearestDistanceSqr));
    }

    private static bool IsInsideCone(Vector3 point, Vector3 apex, Vector3 direction, float height, float radius)
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
            Gizmos.DrawLine(transform.position + cone.OriginOffset, transform.position + Vector3.down * cone.Height + Vector3.right * cone.Radius);
            Gizmos.DrawLine(transform.position + cone.OriginOffset, transform.position + Vector3.down * cone.Height + Vector3.left * cone.Radius);
            Gizmos.DrawLine(transform.position + cone.OriginOffset, transform.position + Vector3.down * cone.Height + Vector3.back * cone.Radius);
            Gizmos.DrawLine(transform.position + cone.OriginOffset, transform.position + Vector3.down * cone.Height + Vector3.forward * cone.Radius);
            DrawGizmoCircle(transform.position + Vector3.down * cone.Height, Vector3.down, cone.Radius);
            Gizmos.DrawSphere(transform.position + cone.OriginOffset, Mathf.Sqrt(cone.Height * cone.Height + cone.Radius * cone.Radius));
            Gizmos.color = Color.red;
            if (_currentScan != null)
                Gizmos.DrawLine(transform.position + cone.OriginOffset, _currentScan.Value.Target.position);
        }
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, maxRadius);
        // if (!_treasureColliders[0] || !_treasureColliders[1]) return;
        // if (!_treasure) return;
        // Gizmos.DrawLine(transform.position, _treasure.transform.position);
    }
    
    private void DrawGizmoCircle(Vector3 circleCenter, Vector3 circleNormal, float circleRadius, int segments = 32)
    {
        var radiusVector = Mathf.Abs(Vector3.Dot(circleNormal, Vector3.right)) - 1f <= Mathf.Epsilon 
            ? Vector3.Cross(circleNormal, Vector3.forward).normalized 
            : Vector3.Cross(circleNormal, Vector3.right).normalized;
        radiusVector *= circleRadius;
        var angleBetweenSegments = 360f / segments;
        var previousCircumferencePoint = circleCenter + radiusVector;
        for (var i = 0; i < segments; ++i)
        {
            radiusVector = Quaternion.AngleAxis(angleBetweenSegments, circleNormal) * radiusVector;
            var newCircumferencePoint = circleCenter + radiusVector;
            Gizmos.DrawLine(previousCircumferencePoint, newCircumferencePoint);
            previousCircumferencePoint = newCircumferencePoint;
        }
    }
    
}
