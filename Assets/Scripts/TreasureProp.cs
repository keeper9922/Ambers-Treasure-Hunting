using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Collider))]
public class TreasureProp : MonoBehaviour
{
    public TreasureObject treasureObject;
    private GameObject _curPrefab;
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private MeshFilter _meshFilter;
    private void OnEnable()
    {
        var curModel = treasureObject.modelPrefabs[Random.Range(0, treasureObject.modelPrefabs.Count)];
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter.mesh = curModel.GetComponent<MeshFilter>().mesh;
        _meshRenderer.sharedMaterial = new Material(curModel.GetComponent<MeshRenderer>().sharedMaterial);
    }

    public FloatRange GetTreasureSpawnRange()
    {
        return treasureObject.spawnDepth;
    }
}
