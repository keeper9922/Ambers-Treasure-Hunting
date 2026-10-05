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
            // var u = Random.Range(0f, 1f);
            /*var r = spawnpoint.radius * Mathf.Pow(u, 1f/3f);
            var v = Random.Range(0f, 2*Mathf.PI);
            var theta = 2 * Mathf.PI * v;
            var phi = Mathf.Acos(2 * u - 1);
            var newX = */
            var r = spawnpoint.radius;
            var phi = Random.Range(0f, Mathf.PI);
            var theta = Random.Range(0f, Mathf.PI);
            var newX = spawnpoint.transform.position.x + r * Mathf.Sin(phi) * Mathf.Cos(theta);
            var newY = spawnpoint.transform.position.y + r * Mathf.Sin(phi) * -Mathf.Sin(theta);
            var newZ = spawnpoint.transform.position.z + r * Mathf.Cos(phi);
            var spawnPosition = new Vector3(newX, newY, newZ);
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
            Debug.Log($"Spawnpoint {spawnpoint.name}. Weight: {random}. Prefab: {curPrefab}");
            Object trsr = Instantiate(curPrefab, spawnPosition, Quaternion.identity);
        }
        Debug.LogWarning($"Total weight: {totalWeight}. Average weight: {total / _objectAmount}");
    }
}