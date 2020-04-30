using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        StartGame,
        InGame,
        Lose,
        Win,
        Menu
    }

    public static GameState currentGameState;
    private GameState _previousGameState;
    private GameObject _restartText;
    private GameObject _menuButton;
    private GameObject _chooseBallButton;
    private GameObject _panel;


    public static void RestartLevel()
    {
        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }
    
    public static void StartNextLevel()
    {
        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        var nextIndex = SceneManager.sceneCountInBuildSettings > currentIndex + 1 ? currentIndex + 1 : 0;
        
        SceneManager.LoadScene(nextIndex);
    }

    // Start is called before the first frame update
    void Start()
    {
        _previousGameState = GameState.StartGame;
        currentGameState = GameState.StartGame;
        _restartText = GameObject.Find("RestartText");
        _menuButton = GameObject.Find("MenuButton");
        _chooseBallButton = GameObject.Find("ChooseBallButton");
        _panel = GameObject.Find("Panel");
        _restartText.SetActive(false);
        _menuButton.SetActive(true);
        _chooseBallButton.SetActive(true);
        _panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_previousGameState != currentGameState)
        {
            _previousGameState = currentGameState;
            if (currentGameState == GameState.Lose)
            {
                _panel.SetActive(true);
                _restartText.SetActive(true);
            }
            
            if (currentGameState == GameState.Win)
            {
                WinLevel();
            }

            if (currentGameState == GameState.InGame)
            {
                _menuButton.SetActive(false);
                _chooseBallButton.SetActive(false);
            }
        }
    }

    private void WinLevel()
    {
        // TODO add timer
        StartNextLevel();
    }
}
