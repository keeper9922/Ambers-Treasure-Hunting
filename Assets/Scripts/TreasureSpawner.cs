using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public class TreasureSpawner : MonoBehaviour
{
    private readonly List<TreasureSpawnpoint> _spawnpoints = new() { };
    public static SpawnpointCreated OnSpawnPointAdd;
    public SpawnSettings levelSettings;
    public GameObject commonPrefab;
    public GameObject uncommonPrefab;
    public GameObject rarePrefab;
    public GameObject legendaryPrefab;
    private int _objectAmount;

    private void Awake()
    {
        OnSpawnPointAdd ??= new SpawnpointCreated();
        OnSpawnPointAdd.AddListener(AddSpawnPoint);
        _objectAmount = Random.Range(levelSettings.objectRangeAmount.min, levelSettings.objectRangeAmount.max+1);
        Debug.Log($"Object amount: {_objectAmount}");
    }

    private void Start()
    {
        SpawnObjectsByWeight();
    }

    private void AddSpawnPoint(OnSpawnPointEventArgs e)
    {
        var spawnpoint = e.spawnpoint;
        _spawnpoints.Add(spawnpoint);
        Debug.Log($"Spawnpoint {spawnpoint.name} has been added.");
    }
    // random
    private void SpawnObjectsByWeight()
    {
        var weights = levelSettings.rarityWeights;
        var totalWeight = levelSettings.rarityWeights.Sum(rarity => rarity.weight);
        var total = 0f;
        foreach (var r in weights)
        {
            Debug.Log($"{r.rarity} => {r.weight} => {totalWeight-r.weight}");
        }
        for(var i = 0; i < _objectAmount; i++){
            var random = Mathf.Ceil(Random.Range(0f, totalWeight));
            total += random;
            var spawnpoint = _spawnpoints[Random.Range(0, _spawnpoints.Count)];
            var offset = Random.insideUnitCircle * spawnpoint.radius;
            var origin = spawnpoint.transform.position;
            origin.x += offset.x;
            origin.z += offset.y;
            var rayOrigin = new Vector3(origin.x, spawnpoint.transform.position.y + levelSettings.raycastHeight, origin.z);
            if (!Physics.Raycast(
                    rayOrigin,
                    Vector3.down,
                    out var hit,
                    levelSettings.raycastDistance,
                    levelSettings.groundMask))
            {
                continue;
            }
            var curPrefab = commonPrefab;
            if (random >= totalWeight-weights[0].weight)
            {
                curPrefab = uncommonPrefab;
            }else if (random >= totalWeight-weights[1].weight)
            {
                curPrefab = rarePrefab;
            }else if (random >= totalWeight-weights[2].weight)
            {
                curPrefab = legendaryPrefab;
            }
            var treasure = curPrefab.GetComponent<TreasureProp>();
            Debug.Log($"{curPrefab.name} => {treasure}");
            var depth = Random.Range(
                treasure.GetTreasureSpawnRange().min,
                treasure.GetTreasureSpawnRange().max
            );

            var position = hit.point - hit.normal * depth;
            Debug.Log($"Spawnpoint {spawnpoint.name}. Weight: {random}. Prefab: {curPrefab}");
            Object trsr = Instantiate(curPrefab, position, Random.rotation);
        }
        Debug.LogWarning($"Total weight: {totalWeight}. Average weight: {total / _objectAmount}");
    }
}