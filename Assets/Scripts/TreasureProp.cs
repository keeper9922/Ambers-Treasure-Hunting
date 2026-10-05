using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreasureProp : MonoBehaviour
{
    public TreasureObject treasureObject;
    private void OnEnable()
    {
        var obj = Instantiate(treasureObject.modelPrefabs[Random.Range(0, treasureObject.modelPrefabs.Count)], transform.position, Random.rotation);
    }
}
