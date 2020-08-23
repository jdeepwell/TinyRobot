using System;
using UnityEngine;

public class OnCollisionEventSpawn : OnCollisionEvent
{
    [SerializeField] private GameObject prefabToSpawn = null;
    
    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        var t = transform;
        Instantiate(prefabToSpawn, t.position, t.rotation);
    }
}