using System;
using UnityEngine;

public class OnCollisionEventKillSelf : OnCollisionEvent
{
    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        var go = gameObject;
        _collisionEventSender.DeferredKill(go);
    }
}
