using System;
using UnityEngine;

public interface ICanBounce
{
    void Bounce();
}

public class OnCollisionEventBounceIntruder : OnCollisionEvent
{
    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        info.Intruder.GetComponent<ICanBounce>()?.Bounce();
    }
}
