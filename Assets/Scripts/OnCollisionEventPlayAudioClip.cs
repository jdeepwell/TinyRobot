using System;
using System.Collections;
using Deepwell;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class OnCollisionEventPlayAudioClip : OnCollisionEvent
{
    [Header("Optionally explicit AudioSource")]
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip audioClip = null;

    private AudioSource _audioSource;
    
    private new void Awake()
    {
        _audioSource = audioSource ? audioSource : GetComponent<AudioSource>();
        base.Awake();
    }
    
    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        _collisionEventSender.AddWaitForBeforeKill(this);
        _audioSource.InstantPlay(audioClip);
        StartCoroutine(_waitForAudioClipToFinish());
    }

    private IEnumerator _waitForAudioClipToFinish()
    {
        yield return new WaitUntil(() => _audioSource.isPlaying == false);
        _collisionEventSender.RemoveWaitForBeforeKill(this);
    }
}