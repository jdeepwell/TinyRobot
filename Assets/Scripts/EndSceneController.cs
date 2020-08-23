using System;
using System.Collections;
using System.Collections.Generic;
using Deepwell;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
public class EndSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshPro _tmproText = null;
    
    private AudioSource _audioSource;
    private ParticleSystem _particles;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _particles = GetComponent<ParticleSystem>();
        _tmproText.text = $"{StaticGameManager.Instance.coins.value} coins earned!";
    }

    private IEnumerator Start()
    {
        _particles.Play();
        yield return new WaitForSeconds(.5f);
        _audioSource.Play();
        yield return new WaitForSeconds(10f);
        StaticGameManager.Instance.LoadMainMenu();
    }

    private void Update()
    {
        if (DWInput.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
