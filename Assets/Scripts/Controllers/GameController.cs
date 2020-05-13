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
            RewardChosen
        }

        public static GameState CurrentGameState;

        private GameState _previousGameState;
        private GameObject _chooseBallButton;
        private GameObject _coinsAmountText;
        private GameObject _restartText;
        private GameObject _ballChooser;
        private GameObject _backButton;
        private GameObject _menuButton;
        private GameObject _panel;

        public static GameController Instance { get; private set; }


        public void RestartLevel()
        {
            SaveController.Instance.LoadGameFromFile();
            SceneManager.LoadScene(1);
        }

        public void StartNextLevel()
        {
            SceneManager.LoadScene(1);
        }

        public void AddCoins(int amount)
        {
            var newAmount = SaveController.Instance.Save.CoinsCount + amount;

            if (newAmount < 0)
                SaveController.Instance.Save.CoinsCount = amount < 0 ? 0 : int.MaxValue;
            else
                SaveController.Instance.Save.CoinsCount = newAmount;
            //TODO: change gameobject for components
            //TODO: adjust offset
            _coinsAmountText.GetComponent<Text>().text = "Coins: " + SaveController.Instance.Save.CoinsCount;
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
                        _menuButton.SetActive(true);
                        _chooseBallButton.SetActive(true);
                        _panel.SetActive(false);
                        break;
                    case GameState.ChooseBall:
                        Time.timeScale = 0;
                        _backButton.SetActive(true);
                        _ballChooser.SetActive(true);
                        _menuButton.SetActive(false);
                        _chooseBallButton.SetActive(false);
                        _panel.SetActive(true);
                        break;
                    case GameState.Menu:
                        Time.timeScale = 0;
                        _backButton.SetActive(true);
                        _menuButton.SetActive(false);
                        _chooseBallButton.SetActive(false);
                        _panel.SetActive(true);
                        break;
                    case GameState.Lose:
                        _panel.SetActive(true);
                        _restartText.SetActive(true);
                        break;
                    case GameState.Win:
                        WinLevel();
                        //play animation of smth
                        break;
                    case GameState.InGame:
                        _menuButton.SetActive(false);
                        _chooseBallButton.SetActive(false);
                        break;
                    case GameState.ChooseReward:
                        ShowRewardScreen();
                        break;
                    case GameState.RewardChosen:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SetUpLoadedScene(Scene scene, LoadSceneMode mode)
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
            // TODO add timer
            SaveController.Instance.Save.TotalLevelsComplete++;
            SaveController.Instance.Save.LevelSegmentsIndexes = LevelController.Instance.GenerateRandomIndexes();
            SaveController.Instance.SaveGameToFile();
            CurrentGameState = GameState.ChooseReward;
        }

        private void ShowRewardScreen()
        {
            var sceneObjects = GameObject.FindObjectsOfType<GameObject>();
            var cameraTransform = Camera.main.transform;

            foreach (var obj in sceneObjects)
                if (obj != this.gameObject)
                    obj.SetActive(false);

            Instantiate(Resources.Load("Prefabs/asteroid_0"), cameraTransform.position + cameraTransform.forward, Quaternion.identity);
        }

        private void InitUI()
        {
            _chooseBallButton = GameObject.Find("ChooseBallButton");
            _coinsAmountText = GameObject.Find("CoinsAmountText");
            _restartText = GameObject.Find("RestartText");
            _menuButton = GameObject.Find("MenuButton");
            _backButton = GameObject.Find("BackButton");
            _ballChooser = GameObject.Find("BallChooser");
            _panel = GameObject.Find("Panel");

            if (!(_restartText && _menuButton && _chooseBallButton && _panel && _coinsAmountText))
            {
                Debug.Log("[note] Check how to fix me ffs.");
                return;
            }

            _restartText.SetActive(false);
            _menuButton.SetActive(true);
            _chooseBallButton.SetActive(true);
            _backButton.SetActive(false);
            _panel.SetActive(false);
            _ballChooser.SetActive(false);
            //TODO: change gameobject for components
            //TODO: adjust offset
            _coinsAmountText.GetComponent<Text>().text = "Coins: " + SaveController.Instance.Save.CoinsCount;
        }
    }
}