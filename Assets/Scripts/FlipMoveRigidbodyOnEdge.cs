using System;
using Deepwell;
using UnityEngine;

[RequireComponent(typeof(MoveRigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class FlipMoveRigidbodyOnEdge : MonoBehaviour
{
    [SerializeField] private float maxDistance = 0.1f;
    [SerializeField] private bool initialDirectionIsLeft = true;

    private MoveRigidbody _moveRigidbody;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private LayerMask _layerMask;
    
    private void Awake()
    {
        _moveRigidbody = GetComponent<MoveRigidbody>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _layerMask = LayerMaskExtensions.GetMaskOrThrow("Platforms", "Items");
    }

    private void LateUpdate()
    {
        var sensorPos = transform.position;
        var offset = _collider.bounds.size.x / 2f;
        offset *= (_spriteRenderer.flipX ^ initialDirectionIsLeft) ? -1f : 1f;
        sensorPos.x += offset;
        var rayCastHit = Physics2D.Raycast(sensorPos, Vector2.down, maxDistance, _layerMask);
        Debug.DrawRay(sensorPos, Vector2.down * maxDistance, Color.green);
        var hitCollider = rayCastHit.collider;
        if (hitCollider == null)
        {
            _moveRigidbody.Flip();
        }
    }
}
