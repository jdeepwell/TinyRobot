using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesBlink : MonoBehaviour
{
    [SerializeField] private Sprite eyesClosed = null;
    [SerializeField] private float blinkIntervalTime = 4f;
    [SerializeField] private float blinkTime = .1f;

    private SpriteRenderer _spriteRenderer;
    private Sprite _eyesOpen;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _eyesOpen = _spriteRenderer.sprite;
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            _spriteRenderer.sprite = _eyesOpen;
            yield return new WaitForSeconds(blinkIntervalTime);
            _spriteRenderer.sprite = eyesClosed;
            yield return new WaitForSeconds(blinkTime);
        }
    }
    
}
