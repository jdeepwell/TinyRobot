using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : OnCollisionEvent
{
    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        StaticGameManager.Instance.FinishReached();
    }
}
