using System;
using UnityEngine;

[Serializable]
public class ScanConeConfig
{
    [field: SerializeField]
    public int Id { get; private set; }

    [field: SerializeField]
    public ScanMode Mode { get; private set; } = ScanMode.Standard;
    
    [field: SerializeField, Min(0.01f)] 
    public float Radius { get; private set; } = 5f;
    
    [field: SerializeField, Min(0.02f)] 
    public float Height { get; private set; } = 5f;
    
    [field: SerializeField]
    public Vector3 OriginOffset { get; private set; }
    
    [field: SerializeField]
    public Vector3 LocalDirection { get; private set; } = Vector3.down;
    
    [Header("Debug only")]
    [field: SerializeField]
    public Color DebugColor { get; private set; } = Color.white;
    
    [field: SerializeField]
    public bool EnableDebugDraw { get; private set; } = true;
    
}
