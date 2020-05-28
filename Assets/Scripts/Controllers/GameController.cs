using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public enum GameState
        {
            StartGame,
            InGame,
            Lose,
            Win,
            Menu,
            ChooseBall,
            ChooseReward,
        }

        public static GameController Instance { get; private set; }
        public static GameState CurrentGameState;
        
        private GameState _previousGameState;
        private GameObject _levelCounterText;
        private GameObject _chooseBallButton;
        private GameObject _coinsAmountText;
        private GameObject _restartText;
        private GameObject _ballChooser;
        private GameObject _backButton;
        private GameObject _panel;

        public void RestartLevel()
        {
            SaveController.Instance.LoadGameFromFile();
            SceneManager.LoadScene(1);
        }

        //TODO: move method to save controller
        public void AddCoins(int amount, Text txtComponent)
        {
            var newAmount = SaveController.Instance.Save.CoinsCount + amount;

            if (newAmount < 0)
                SaveController.Instance.Save.CoinsCount = amount < 0 ? 0 : int.MaxValue;
            else
                SaveController.Instance.Save.CoinsCount = newAmount;
            //TODO: change gameobject for components
            //TODO: adjust offset
            txtComponent.text = "" + SaveController.Instance.Save.CoinsCount;
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.Log("[ATTENTION] Multiple " + this + " found!");
                return;
            }

            if (SaveController.Instance.Save == null)
                SaveController.Instance.LoadGameFromFile();
            if (SaveController.Instance.Save == null)
                SaveController.Instance.SaveGameToFile();
            Application.targetFrameRate = 60;
            SceneManager.sceneLoaded += SetUpLoadedScene;
            SceneManager.LoadScene(1);
        }

        private void Update()
        {
            if (_previousGameState != CurrentGameState)
            {
                _previousGameState = CurrentGameState;
                switch (CurrentGameState)
                {
                    case GameState.StartGame:
                        Time.timeScale = 1;
                        _backButton.SetActive(false);
                        _ballChooser.SetActive(false);
                        _chooseBallButton.SetActive(true);
                        _panel.SetActive(false);
                        break;
                    case GameState.ChooseBall:
                        Time.timeScale = 0;
                        _backButton.SetActive(true);
                        _ballChooser.SetActive(true);
                        _chooseBallButton.SetActive(false);
                        _panel.SetActive(true);
                        break;
                    case GameState.Menu:
                        Time.timeScale = 0;
                        _backButton.SetActive(true);
                        _chooseBallButton.SetActive(false);
                        _panel.SetActive(true);
                        break;
                    case GameState.Lose:
                        _panel.SetActive(true);
                        _restartText.SetActive(true);
                        break;
                    case GameState.Win:
                        WinLevel();
                        //TODO: teleport animation
                        break;
                    case GameState.InGame:
                        _chooseBallButton.SetActive(false);
                        break;
                    case GameState.ChooseReward:
                        SceneManager.sceneLoaded -= SetUpLoadedScene;
                        SceneManager.LoadScene(2);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void SetUpLoadedScene(Scene scene, LoadSceneMode mode)
        {
            _previousGameState = GameState.StartGame;
            CurrentGameState = GameState.StartGame;
            InitUI();

            if (SaveController.Instance.Save.LevelSegmentsIndexes.Count <= 0)
            {
                Debug.Log("[ATTENTION] Level segments indexes array was empty. This should not have happened ");
                SaveController.Instance.Save.LevelSegmentsIndexes = LevelController.Instance.GenerateRandomIndexes();
            }

            LevelController.Instance.SetUpScene();
        }

        private void WinLevel()
        {
            SaveController.Instance.Save.CurrentLevel++;
            SaveController.Instance.Save.LevelSegmentsIndexes = LevelController.Instance.GenerateRandomIndexes();
            SaveController.Instance.SaveGameToFile();
            CurrentGameState = GameState.ChooseReward;
        }

        private void InitUI()
        {
            _chooseBallButton = GameObject.Find("ChooseBallButton");
            _coinsAmountText = GameObject.Find("CoinsAmountText");
            _restartText = GameObject.Find("RestartText");
            _backButton = GameObject.Find("BackButton");
            _ballChooser = GameObject.Find("BallChooser");
            _panel = GameObject.Find("Panel");
            _levelCounterText = GameObject.Find("LevelCounter");
            
            if (!(_restartText && _chooseBallButton && _panel && _coinsAmountText))
            {
                Debug.Log("[note] Check how to fix me ffs.");
                return;
            }

            _restartText.SetActive(false);
            _chooseBallButton.SetActive(true);
            _backButton.SetActive(false);
            _panel.SetActive(false);
            _ballChooser.SetActive(false);
            //TODO: adjust offset
            _coinsAmountText.GetComponent<Text>().text = "" + SaveController.Instance.Save.CoinsCount;
            _levelCounterText.GetComponent<Text>().text = "Level: " + SaveController.Instance.Save.CurrentLevel;
        }
    }
}