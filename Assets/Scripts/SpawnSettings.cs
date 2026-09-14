using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RarityWeight
{
        public TreasureRarity rarity;
        public float weight;
}

[CreateAssetMenu(fileName = "SpawnSettings", menuName = "Treasure Objects/Spawn Settings")]
public class SpawnSettings : ScriptableObject
{
        public string levelName;
        [Header("Веса спавна каждой редкости")]
        public List<RarityWeight> rarityWeights;
        public IntRange objectRangeAmount;
}