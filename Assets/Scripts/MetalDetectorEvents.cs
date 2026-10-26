using System;
using UnityEngine;

public static class MetalDetectorEvents
{
    public static event Action<ScanResult?> DetectionChanged;
    public static event Action<ScanMode> ModeChanged;
    public static event Action<bool> ScanStateChanged;

    private static void OnScanStateChanged(bool obj)
    {
        ScanStateChanged?.Invoke(obj);
    }

    private static void OnModeChanged(ScanMode obj)
    {
        ModeChanged?.Invoke(obj);
    }

    private static void OnDetectionChanged(ScanResult? obj)
    {
        DetectionChanged?.Invoke(obj);
    }
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