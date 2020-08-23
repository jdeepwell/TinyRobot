using System;
using UnityEngine;

public interface ICanBeKilled
{
    void Kill();
}

public class OnCollisionEventKillIntruder : OnCollisionEvent
{
    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        info.Intruder.GetComponent<ICanBeKilled>()?.Kill();
    }
}