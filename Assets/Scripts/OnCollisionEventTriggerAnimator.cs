using System;
using UnityEngine;

public class OnCollisionEventTriggerAnimator : OnCollisionEvent
{
    [SerializeField] private String triggerName = "";
    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        gameObject.GetComponent<Animator>().SetTrigger(triggerName);
    }
}