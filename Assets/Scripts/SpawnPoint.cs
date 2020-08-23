using UnityEngine;

public class SpawnPoint : OnCollisionEvent
{
    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        StaticGameManager.Instance.spawnPoint = gameObject.transform.position;
    }
}
