using System;
using System.Collections;
using Deepwell;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticGameManager : MonoSingleton<StaticGameManager>
{
    public event Action SceneResetEvent;
    public event Action GameOverEvent;
    public ObservableValue<int> lives = new ObservableValue<int>();
    public ObservableValue<int> coins = new ObservableValue<int>();
    public Vector2 spawnPoint;
    [NonSerialized] public int level = 1;
    public bool backGroundMusicOn = true;

    private int _coinsAtLevelStart; 
    private int _numLevels = 4;

    public void KillPlayer()
    {
        lives.value--;
        if (lives.value == 0)
        {
            coins.value = _coinsAtLevelStart;
            DWInput.userInputEnabled = false;
            GameOverEvent?.Invoke();
        }
        else
        {
            SceneResetEvent?.Invoke();
        }
    }

    private void SetStartValues()
    {
        lives.value = 3;
    }

    public void ReloadLevel()
    {
        _coinsAtLevelStart = coins.value;
        SetStartValues();
        SceneManager.LoadScene(level);
        DWInput.userInputEnabled = true;
    }

    public void FinishReached()
    {
        if (level < _numLevels)
        {
            level++;
            ReloadLevel();
        }
        else
        {
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        coins.value = 0;
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        coins.value = 0;
        SetStartValues();
        level = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnDisable()
    {
    }
}