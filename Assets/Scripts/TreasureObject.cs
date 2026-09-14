using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum TreasureRarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}
[Serializable] public class FloatRange { public float min; public float max; }
[Serializable] public class IntRange { public int min; public int max; }
[CreateAssetMenu(fileName = "TreasureObject", menuName = "Treasure Objects/Treasure Object")]
[Serializable] public class TreasureObject : ScriptableObject
{
    public string objectName;
    public FloatRange priceRange;
    public FloatRange spawnDepth;
    public TreasureRarity rarity;
    public List<GameObject> modelPrefabs;
}
