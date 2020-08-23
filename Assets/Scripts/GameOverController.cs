using System;
using System.Collections;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText = null;
    [SerializeField] private AudioSource gameOverAudioSource = null;

    private void OnEnable()
    {
        gameOverText.SetActive(false);
        StaticGameManager.Instance.GameOverEvent += GameOver;
    }

    private void OnDisable()
    {
        StaticGameManager.Instance.GameOverEvent -= GameOver;
    }

    private void GameOver()
    {
        gameOverText.SetActive(true);
        StartCoroutine(WaitAndReload(3f));
        gameOverAudioSource.Play();
    }

    private IEnumerator WaitAndReload(float wt)
    {
        yield return new WaitForSeconds(wt);
        StaticGameManager.Instance.ReloadLevel();
    }
}
