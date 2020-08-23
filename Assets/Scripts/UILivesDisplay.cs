using System;
using TMPro;
using UnityEngine;

public class UILivesDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpProText = null;
    private Animator _livesTextAnimator;
    private int _lightItUpHash;
    private bool _doAnimation = false;

    private void Awake()
    {
        _lightItUpHash = Animator.StringToHash("LightItUp");
        _livesTextAnimator = tmpProText.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StaticGameManager.Instance.lives.observers += OnLivesChanged;
        OnLivesChanged(StaticGameManager.Instance.lives);
        _doAnimation = true;
    }

    private void OnDisable()
    {
        StaticGameManager.Instance.lives.observers -= OnLivesChanged;
    }

    private void OnLivesChanged(int lives)
    {
        tmpProText.text = lives.ToString();
        if (!_doAnimation) return;
        _livesTextAnimator.SetTrigger(_lightItUpHash);
    }
}
