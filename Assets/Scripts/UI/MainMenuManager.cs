using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private int _playSceneIndex;
    
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _creditsMenu;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _optionBackButton;
    [SerializeField] private Button _creditsBackButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(OnPlayButtonClick);
        _optionButton.onClick.AddListener(OnOptionButtonClick);
        _creditsButton.onClick.AddListener(OnCreditsButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
        
        _optionBackButton.onClick.AddListener(OnOptionBackButtonClick);
        _creditsBackButton.onClick.AddListener(OnCreditsBackButtonClick);
    }
    
    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
        _optionButton.onClick.RemoveAllListeners();
        _creditsButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();
        
        _optionBackButton.onClick.RemoveAllListeners();
        _creditsBackButton.onClick.RemoveAllListeners();
    }

    // Start is called before the first frame update
    void Start()
    {
        _optionsMenu.SetActive(false);
        _creditsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnPlayButtonClick()
    {
        SceneManager.LoadScene(_playSceneIndex);
    }
    
    private void OnOptionButtonClick()
    {
        _optionsMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }
    
    private void OnCreditsButtonClick()
    {
        _creditsMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }
    
    private void OnOptionBackButtonClick()
    {
        _optionsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }
    
    private void OnCreditsBackButtonClick()
    {
        _creditsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
