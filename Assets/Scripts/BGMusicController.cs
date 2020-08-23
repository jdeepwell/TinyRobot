using System;
using System.Collections;
using System.Collections.Generic;
using Deepwell;
using UnityEngine;

public class BGMusicController : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.enabled = StaticGameManager.Instance.backGroundMusicOn;
    }

    private void Update()
    {
        if (DWInput.GetKeyDown(KeyCode.M))
        {
            StaticGameManager.Instance.backGroundMusicOn ^= true;
            _audioSource.enabled = StaticGameManager.Instance.backGroundMusicOn;
        }
    }
}
