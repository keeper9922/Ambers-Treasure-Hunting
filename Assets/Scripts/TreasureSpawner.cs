using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class TreasureSpawner
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
        foreach (var spawnpoint in _spawnpoints)
        {
            // TODO логика спавна, выбирая случайную точку спавна и создавая в ней объект, подходящий по весу.
            // TODO при этом выбор точки спавна случайный, как и выбор точки в ней, основываясь на радиус и глубину.
        }
    }
}