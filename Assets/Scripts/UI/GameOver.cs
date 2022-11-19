using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private int _mainMenuSceneIndex = 0;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;

    private void Start()
    {
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnDestroy()
    {
        _mainMenuButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }

    private void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene(_mainMenuSceneIndex);
    }
}
