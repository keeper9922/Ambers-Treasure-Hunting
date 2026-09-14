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
    private int objectAmount;

    private void Awake()
    {
        OnSpawnPointAdd ??= new SpawnpointCreated();
        OnSpawnPointAdd.AddListener(AddSpawnPoint);
        objectAmount = Random.Range(levelSettings.objectRangeAmount.min, levelSettings.objectRangeAmount.max+1);
    }

    private void AddSpawnPoint(OnSpawnPointEventArgs e)
    {
        var spawnpoint = e.spawnpoint;
        _spawnpoints.Add(spawnpoint);
        Debug.Log($"Spawnpoint {spawnpoint.name} has been added.");
    }
    // random
    private void spawnObjectsByWeight()
    {
        var weights = levelSettings.rarityWeights;
        var totalWeight = levelSettings.rarityWeights.Sum(rarity => rarity.weight);
        var random = Mathf.Ceil(Random.Range(0f, totalWeight));
        for(var i = 0; i < objectAmount; i++){
            var spawnpoint = _spawnpoints[Random.Range(0, _spawnpoints.Count)];
            // var u = Random.Range(0f, 1f);
            /*var r = spawnpoint.radius * Mathf.Pow(u, 1f/3f);
            var v = Random.Range(0f, 2*Mathf.PI);
            var theta = 2 * Mathf.PI * v;
            var phi = Mathf.Acos(2 * u - 1);
            var newX = */
            var r = spawnpoint.radius;
            var phi = Random.Range(0f, 180f);
            var theta = Random.Range(0f, 360f);
            var newX = r * Mathf.Sin(phi) * Mathf.Cos(theta);
            var newY = r * Mathf.Sin(phi) * Mathf.Sin(theta);
            var newZ = r * Mathf.Cos(phi);
            Object trsr = Instantiate(spawnpoint);
        }
    }
}