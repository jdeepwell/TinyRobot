using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MoveRigidbody : MonoBehaviour
{
    [SerializeField] private Vector2 velocity;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    public void Flip()
    {
        velocity *= -1f;
        _spriteRenderer.flipX ^= true;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
