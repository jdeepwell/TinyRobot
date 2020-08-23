using System;
using System.Collections;
using System.Collections.Generic;
using Deepwell;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MoveForthAndBack : MonoBehaviour
{
    [SerializeField] private Transform startTransform = null;
    [SerializeField] private Transform endTransform = null;
    [SerializeField] private Transform movingItemTransform = null;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 _midPosition;
    private Vector3 _moveVector;
    private float _halfMagnitude;

    void Start()
    {
        var startPosition = startTransform.position;
        var endPosition = endTransform.position;
        _midPosition = startPosition + (endPosition - startPosition) / 2f;
        var path = endPosition - startPosition;
        _halfMagnitude = Vector3.Magnitude(path) / 2f;
        _moveVector = Vector3.Normalize(path);
    }

    private void FixedUpdate()
    {
        var adjustedMoveSpeed = moveSpeed / _halfMagnitude;
        // var rb = movingItemTransform.GetComponent<Rigidbody2D>();
        // rb.MovePosition(_midPosition + _moveVector * Mathf.Sin(Time.time * adjustedMoveSpeed * DWMathf.Tau) * _halfMagnitude);
        movingItemTransform.position = _midPosition + _moveVector * Mathf.Sin(Time.time * adjustedMoveSpeed * DWMathf.Tau) * _halfMagnitude;
    }
    
    void Update()
    {
        // var adjustedMoveSpeed = moveSpeed / _halfMagnitude;
        // movingItemTransform.position = _midPosition + _moveVector * Mathf.Sin(Time.time * adjustedMoveSpeed * DWMathf.Tau) * _halfMagnitude;
    }
}
