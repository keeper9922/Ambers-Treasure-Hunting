using System;
using UnityEngine;

public static class MetalDetectorEvents
{
    public static event Action<ScanResult> TargetDetected;
    public static event Action<ScanResult> TargetUpdated;
    public static event Action<string> TargetLost;

    public static void RaiseDetected(ScanResult result)
        => TargetDetected?.Invoke(result);

    public static void RaiseUpdated(ScanResult result)
        => TargetUpdated?.Invoke(result);

    public static void RaiseLost(string targetId)
        => TargetLost?.Invoke(targetId);
}

public readonly struct ScanResult
{
    public readonly int ConeId;
    private readonly Transform _target;
    public readonly float Distance;

    public ScanResult(
        int coneId,
        Transform target,
        float distance)
    {
        ConeId = coneId;
        _target = target;
        Distance = distance;
    }

    public bool HasTarget => _target != null;
}