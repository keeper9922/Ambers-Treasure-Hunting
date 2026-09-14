using System;
using UnityEngine;
using UnityEngine.Events;

public class OnSpawnPointEventArgs : EventArgs{
    public TreasureSpawnpoint spawnpoint { get; }

    public OnSpawnPointEventArgs(TreasureSpawnpoint spawnpoint)
    {
        this.spawnpoint = spawnpoint;
    }
}

public class SpawnpointCreated : UnityEvent<OnSpawnPointEventArgs> {}
public class TreasureSpawnpoint : MonoBehaviour
{
    public float radius;

    private void Start()
    {
        TreasureSpawner.OnSpawnPointAdd?.Invoke(new OnSpawnPointEventArgs(this));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
