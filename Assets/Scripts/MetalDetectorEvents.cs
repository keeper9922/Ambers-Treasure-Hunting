using System;
using UnityEngine;

public static class MetalDetectorEvents
{
    public static event Action<ScanResult?> DetectionChanged;
    public static event Action<ScanMode> ModeChanged;
    public static event Action<bool> ScanStateChanged;
    public static event Action<float> OnSignalChanged;

    public static void OnScanStateChanged(bool obj)
    {
        ScanStateChanged?.Invoke(obj);
    }

    public static void OnModeChanged(ScanMode obj)
    {
        ModeChanged?.Invoke(obj);
    }

    public static void OnDetectionChanged(ScanResult? obj)
    {
        DetectionChanged?.Invoke(obj);
    }

    public static void OnOnSignalChanged(float obj)
    {
        OnSignalChanged?.Invoke(obj);
    }
}

public readonly struct ScanResult
{
    public readonly int ConeId;
    public readonly float Distance;

    public ScanResult(
        int coneId,
        Transform target,
        float distance)
    {
        ConeId = coneId;
        Target = target;
        Distance = distance;
    }

    public bool HasTarget => Target != null;
    public Transform Target { get; }
}