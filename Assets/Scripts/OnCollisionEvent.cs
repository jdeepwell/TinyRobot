using System;
using System.Collections;
using Deepwell;
using UnityEngine;

[RequireComponent(typeof(CollisionEventSender))]
public abstract class OnCollisionEvent : MonoBehaviour, IReceiveCollisionEvents
{
    [Header("Use Masks or chain parent:")]
    [SerializeField] private CollisionEventType collisionMask = 0;
    [SerializeField] private DWTagValue elementTags = 0;
    [SerializeField] private OnCollisionEvent eventChainParent = null;
    [Header("Continue with next un-chained OnCollision...?")]
    [SerializeField] private bool stopEventPropagation = false;
    [Header("Delay action:")]
    [SerializeField, Range(0, 10)] private float actionDelay = 0f;

    protected CollisionEventSender _collisionEventSender; 

    private OnCollisionEvent _eventChainChild;
    
    protected void Awake()
    {
        _collisionEventSender = GetComponent<CollisionEventSender>();
        if (eventChainParent != null)
        {
            eventChainParent._eventChainChild = this;
        }
    }

    public bool ReceiveCollisionEvent(CollisionEventInfo info)
    {
        if (!info.Intruder.MatchesTagMask<ElementTags>(elementTags)) return false;
        if (!info.Type.MatchesMask(collisionMask)) return false;
        actOnEvent(info);
        return stopEventPropagation;
    }

    private void passEventToChild(CollisionEventInfo info)
    {
        if (_eventChainChild != null) _eventChainChild.actOnEvent(info);
    }

    private void actOnEvent(CollisionEventInfo info)
    {
        if (actionDelay < float.Epsilon)
        {
            ActOnCollisionEvent(info);
            passEventToChild(info);
        }
        else StartCoroutine(delayedActOnEvent(info));
    }
    
    private IEnumerator delayedActOnEvent(CollisionEventInfo info)
    {
        _collisionEventSender.AddWaitForBeforeKill(this);
        yield return new WaitForSeconds(actionDelay);
        ActOnCollisionEvent(info);
        passEventToChild(info);
        _collisionEventSender.RemoveWaitForBeforeKill(this);
    }

    protected abstract void ActOnCollisionEvent(CollisionEventInfo info);
}
