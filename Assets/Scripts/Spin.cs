using System;
using System.Collections;
using System.Collections.Generic;
using Deepwell;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Transform movingItemTransform = null;
    [SerializeField] private float spinSpeed = 360f;

    void Update()
    {
        movingItemTransform.Rotate(new Vector3(0f, 0f, Time.deltaTime * spinSpeed));
    }
}
