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

    public GameState currentGameState;
    private GameState _previousGameState;
    // private GameObject _ui;
    private GameObject _restartText;
    private GameObject _pauseButton;


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
        // _ui = GameObject.Find("UI");
        _restartText = GameObject.Find("RestartText");
        _pauseButton = GameObject.Find("PauseButton");
        // _ui.SetActive(true);
        _restartText.SetActive(false);
        _pauseButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_previousGameState != currentGameState)
        {
            _previousGameState = currentGameState;
            if (currentGameState == GameState.Lose)
            {
                _restartText.SetActive(true);
            }
            
            if (currentGameState == GameState.Win)
            {
                WinLevel();
            }

            if (currentGameState == GameState.InGame)
            {
                _pauseButton.SetActive(false);
            }
        }
    }

    private void WinLevel()
    {
        // TODO add timer
        StartNextLevel();
    }
}
