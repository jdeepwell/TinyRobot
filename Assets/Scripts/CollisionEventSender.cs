using System;
using System.Collections.Generic;
using System.Linq;
using Deepwell;
using UnityEngine;

[Serializable, Flags]
public enum CollisionEventType
{
    Stomped =     1 << 0,
    HeadBumped =  1 << 1, // from below
    RunInto =     1 << 2,
}

public struct CollisionEventInfo
{
    public CollisionEventType Type;
    public GameObject Intruder;
}

public static class CollisionEventTypeExtensions
{
    public static bool MatchesAllMask(this CollisionEventType type, CollisionEventType mask)
    {
        return (type & mask) == type;
    }
    
    public static bool MatchesMask(this CollisionEventType type, CollisionEventType mask)
    {
        return (type & mask) != 0;
    }
}

public interface IReceiveCollisionEvents
{
    bool ReceiveCollisionEvent(CollisionEventInfo info); // return true if event propagation should stop
}

public class CollisionEventSender : MonoBehaviour
{
    private List<OnCollisionEvent> _waitForThese = new List<OnCollisionEvent>();
    private List<GameObject> _gameObjectstToKill = new List<GameObject>();
    
    private CollisionEventType typeForAngle(float angle)
    {
        var absAngle = Mathf.Abs(angle);
        if (absAngle < 30f) return CollisionEventType.Stomped;
        if (absAngle > 150f) return CollisionEventType.HeadBumped;
        else return CollisionEventType.RunInto;
    }
    
    private void sendEvent(CollisionEventInfo info)
    {
        var recipients = GetComponents<IReceiveCollisionEvents>();
        foreach (var recipient in recipients)
        {
            if (recipient.ReceiveCollisionEvent(info)) // clients can stop event propagation by returning true
            {
                break;
            }
        }
    }

    public void DeferredKill(GameObject go)
    {
        var hisRenderer = go.GetComponent<SpriteRenderer>();
        if (hisRenderer) hisRenderer.enabled = false;
        var hisCollider = go.GetComponent<Collider2D>();
        if (hisCollider) hisCollider.enabled = false;
        _gameObjectstToKill.Add(go);
        tryToKillObjects();
    }

    public void AddWaitForBeforeKill(OnCollisionEvent behaviour)
    {
        _waitForThese.Add(behaviour);
    }

    public void RemoveWaitForBeforeKill(OnCollisionEvent behaviour)
    {
        _waitForThese.Remove(behaviour);
        tryToKillObjects();
    }

    private void tryToKillObjects()
    {
        if (_waitForThese.Count == 0)
        {
            foreach (var go in _gameObjectstToKill)
            {
                var killable = go.GetComponent<ICanBeKilled>();
                if (killable != null)
                {
                    killable.Kill();
                }
                else
                {
                    go.SetActive(false);
                    Destroy(go);
                }
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D theCollision)
    {
        var angle = theCollision.GetContact(0).Angle();
        CollisionEventInfo info;
        info.Type = typeForAngle(angle);
        info.Intruder = theCollision.gameObject;
        sendEvent(info);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionEventInfo info;
        info.Type = CollisionEventType.RunInto;
        info.Intruder = other.gameObject;
        sendEvent(info);
    }
}
