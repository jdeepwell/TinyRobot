using System;
using UnityEngine;

namespace Playground
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Grounding : MonoBehaviour
    {
        private bool _shouldDoJump = false;
        private Rigidbody2D _rigidBody2D;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                _shouldDoJump = true;
            }
        }

        private void LateUpdate()
        {
            foreach (Transform childTransform in GetComponentInChildren<Transform>())
            {
                var maxDistance = 0.3f;
                var rayCastHit = Physics2D.Raycast(childTransform.position, childTransform.up,
                    maxDistance, LayerMask.GetMask("Platforms"));
                Debug.DrawRay(childTransform.position, childTransform.up * maxDistance, Color.red);
            }
            if (_shouldDoJump)
            {
                _rigidBody2D.AddForce(Vector2.up * 500f);
            }
            _shouldDoJump = false;
        }
    }
}
