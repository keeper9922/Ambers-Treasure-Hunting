using System;
using UnityEngine;
using Random = UnityEngine.Random;

/*[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Collider))]*/
public class TreasureProp : MonoBehaviour
{
    public TreasureObject treasureObject;
    /*private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;*/
    private void OnEnable()
    {
        var curModel = treasureObject.modelPrefabs[Random.Range(0, treasureObject.modelPrefabs.Count)];
        var obj = Instantiate(curModel, transform.position, Random.rotation);
        /*_meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        var sourceMeshFilter = curModel.GetComponentInChildren<MeshFilter>();
        var sourceMeshRenderer = curModel.GetComponentInChildren<MeshRenderer>();
        
        _meshFilter.sharedMesh = sourceMeshFilter.sharedMesh;
        _meshRenderer.sharedMaterial = sourceMeshRenderer.sharedMaterial;*/
    }

    public FloatRange GetTreasureSpawnRange()
    {
        return treasureObject.spawnDepth;
    }
}
