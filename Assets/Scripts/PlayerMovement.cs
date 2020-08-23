using System;
using System.Collections;
using Deepwell;
using UnityEngine;

[RequireComponent(typeof(CharacterGrounding))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour, IMovementSpeedProvider, ICanBounce
{
    [SerializeField] private float speedFactor = 2f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private float reboundForce = 200f;
    [SerializeField] private float jumpXInertia = 50f;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip jumpClip = null;
    public float NormalizedMovementSpeed { get; private set; }

    private float _bounceForce = 400f;
    private Rigidbody2D _rigidBody2D;
    private CharacterGrounding _characterGrounding;
    private bool _shouldDoJump = false;
    private SpriteRenderer _spriteRenderer;

    public void StopPlayer()
    {
        _rigidBody2D.velocity = Vector2.zero;
    }

    // ICanBounce
    public void Bounce()
    {
        _rigidBody2D.AddForce(new Vector2(0f, _bounceForce));
        audioSource.InstantPlay(jumpClip);
    }
    
    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _characterGrounding = GetComponent<CharacterGrounding>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        DWInput.jumpEvent += doJump;
    }

    private void OnDisable()
    {
        DWInput.jumpEvent -= doJump;
    }

    private void doJump()
    {
        _shouldDoJump = true;
    }
    
    private void Update()
    {
        DWInput.CheckForJump();
    }

    private void FixedUpdate()
    {
        var isGrounded = _characterGrounding.isGrounded;
        var groundMovedBy = _characterGrounding.groundMovedBy;
        var currentSpeed = DWInput.GetAxis("Horizontal") * speedFactor;
        NormalizedMovementSpeed = Mathf.Abs(currentSpeed / speedFactor);

        // move with platform
        var jumpXVelocity = 0f;
        if (groundMovedBy.HasValue)
        {
            // _rigidBody2D.MovePosition(_rigidBody2D.position + groundMovedBy.Value);
            Vector3 moveBy = groundMovedBy.Value;
            transform.position += moveBy;
            jumpXVelocity = (moveBy.x / Time.fixedDeltaTime) * _rigidBody2D.mass * jumpXInertia;
        }

        // move
        var movementVector = new Vector3(currentSpeed * Time.fixedDeltaTime, 0f, 0f);
        transform.position += movementVector;

        // orient
        if (!Mathf.Approximately(currentSpeed, 0f))
        {
            _spriteRenderer.flipX = currentSpeed > 0f;
        }

        // jump
        if (_shouldDoJump && isGrounded)
        {
            // it is safe to assume that if the character is grounded it also has a groundingDirection
            var force = new Vector2(jumpXVelocity, 1f * jumpForce) - _characterGrounding.groundingDirection.Value * reboundForce;
            _rigidBody2D.AddForce(force);
            audioSource.InstantPlay(jumpClip);
        }
        _shouldDoJump = false;
    }
}
