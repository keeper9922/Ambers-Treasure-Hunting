using UnityEngine;
using UnityEngine.Serialization;

public enum TreasureRarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class TreasureObject : MonoBehaviour
{
    [SerializeField] private float price;
    [SerializeField] private TreasureRarity rarity;
    [SerializeField] private Mesh mesh;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
