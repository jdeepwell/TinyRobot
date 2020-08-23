using System;
using System.Linq;
using Deepwell;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider2D))]
public class CharacterGrounding : MonoBehaviour
{
    [SerializeField] private Transform[] referencePoints = null;
    [SerializeField] private float maxDistance = 0.1f;
    public bool isGrounded = false;
    public Vector2? groundingDirection = null;
    public Vector2? groundMovedBy = null;

    private Collider2D _myCollider;
    private Vector3? _movingGroundLastPosition;
    [CanBeNull] private Transform _lastMovingGround;
    private LayerMask _layerMask;

    private void Awake()
    {
        _myCollider = GetComponent<Collider2D>();
        Assert.AreNotEqual(referencePoints.Length, 0, "CharacterGrounding needs at least one reference point");
        _layerMask = LayerMaskExtensions.GetMaskOrThrow("Platforms", "Items");
    }

    private bool _isMovingTransform(Transform t)
    {
        var mover = t.GetComponent<MoveForthAndBack>() ??
                       t.GetComponentInParent<MoveForthAndBack>();
        return mover != null;
    }
    private void FixedUpdate()
    {
        var newIsGrounded = false;
        Transform movingTransform = null;
        groundingDirection = null;
        foreach (var rp in referencePoints)
        {
            var hitTransform = CheckForGrounding(rp.position, rp.up);
            newIsGrounded |= hitTransform != null;
            movingTransform = (hitTransform && _isMovingTransform(hitTransform) ? hitTransform:movingTransform);
        }
        isGrounded = newIsGrounded;

        groundMovedBy = null;
        if (movingTransform != null)
        {
            var p = movingTransform.position;
            if (_movingGroundLastPosition.HasValue && _lastMovingGround == movingTransform)
            {
                groundMovedBy = p - _movingGroundLastPosition.Value;
            }
            _movingGroundLastPosition = p;
            _lastMovingGround = movingTransform;
        }
        else {
            _movingGroundLastPosition = null;
        }
    }

    private Transform CheckForGrounding(Vector2 from, Vector2 direction)
    {
        var rayCastHit = Physics2D.Raycast(from, direction, maxDistance, _layerMask);
        Debug.DrawRay(from, direction * maxDistance, Color.red);
        var hitCollider = rayCastHit.collider;
        if (hitCollider == null) return null;
        if (hitCollider == _myCollider) return null;
        if (!hitCollider.gameObject.MatchesTagMask<ElementTags>(ElementTags.CanStandOn)) return null;
        groundingDirection = groundingDirection ?? direction;
        return hitCollider.transform;
    }
}
