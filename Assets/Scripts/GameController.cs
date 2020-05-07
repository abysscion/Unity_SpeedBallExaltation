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

    public static GameState CurrentGameState;
    public static GameSave CurrentSave { get; private set; }
    
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

    public static void SaveGame() //todo: exception handling system that prevents broken saves overwriting and etc
    {
        var segs = new List<int>();
        
        for (var i = 0; i < 10; i++)
            segs.Add(Random.Range(0, 9));
        SaveManager.SaveGameToFile(CurrentSave);
    }

    private void Start()
    {
        if (CurrentSave == null)
            CurrentSave = SaveManager.LoadGameFromFile() ?? new GameSave(new List<int>(), 0, 0);
        _previousGameState = GameState.StartGame;
        CurrentGameState = GameState.StartGame;
        _restartText = GameObject.Find("RestartText");
        _menuButton = GameObject.Find("MenuButton");
        _chooseBallButton = GameObject.Find("ChooseBallButton");
        _panel = GameObject.Find("Panel");
        _restartText.SetActive(false);
        _menuButton.SetActive(true);
        _chooseBallButton.SetActive(true);
        _panel.SetActive(false);
    }

    private void Update()
    {
        if (_previousGameState != CurrentGameState)
        {
            _previousGameState = CurrentGameState;
            if (CurrentGameState == GameState.Lose)
            {
                _panel.SetActive(true);
                _restartText.SetActive(true);
            }
            
            if (CurrentGameState == GameState.Win)
            {
                WinLevel();
            }

            if (CurrentGameState == GameState.InGame)
            {
                _menuButton.SetActive(false);
                _chooseBallButton.SetActive(false);
            }
        }
    }

    private void WinLevel()
    {
        // TODO add timer
        SaveGame();
        StartNextLevel();
    }
}