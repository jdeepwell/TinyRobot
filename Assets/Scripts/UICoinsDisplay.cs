using System;
using TMPro;
using UnityEngine;

public class UICoinsDisplay : MonoBehaviour
{
    [SerializeField] private Animator coinsAnimator = null;
    [SerializeField] private TextMeshProUGUI tmpProText = null;

    private AudioSource _kaChingAudioSource;
    private int _pulseAnimHash;

    private void Awake()
    {
        _pulseAnimHash = Animator.StringToHash("PulseTheCoin");
        _kaChingAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        StaticGameManager.Instance.coins.observers += OnCoinsChanged;
        updateCoinsText(StaticGameManager.Instance.coins);
    }

    private void OnDisable()
    {
        StaticGameManager.Instance.coins.observers -= OnCoinsChanged;
    }

    private void updateCoinsText(int coins)
    {
        tmpProText.text = $"{coins:D2}";
    }
    
    private void OnCoinsChanged(int coins)
    {
        updateCoinsText(coins);
        coinsAnimator.SetTrigger(_pulseAnimHash);
        if (coins != 0) _kaChingAudioSource.Play();
    }
}