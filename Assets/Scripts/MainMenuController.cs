using System;
using Deepwell;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject leftRightText = null;
    [SerializeField] private GameObject jumpText = null;

    private void Awake()
    {
#if UNITY_IOS || UNITY_ANDROID
        leftRightText.SetActive(false);
        jumpText.SetActive(false);
#else
        leftRightText.SetActive(true);
        jumpText.SetActive(true);
#endif
    }

    public void NewGame()
    {
        StaticGameManager.Instance.level = 1;
        StaticGameManager.Instance.ReloadLevel();
    }

    private void Update()
    {
        if (DWInput.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (DWInput.GetKeyDown(KeyCode.Return) || DWInput.GetKeyDown(KeyCode.Space))
        {
            NewGame();
        }
    }
}
