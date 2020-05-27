using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSave
{
    [SerializeField] private List<int> _levelSegmentsIndexes;
    [SerializeField] private int _coinsCount;
    [SerializeField] private int _currentLevel;
    [SerializeField] private int _playerSkin;
    [SerializeField] private List<bool> _purchasedSkins;
    
    public List<int> LevelSegmentsIndexes
    {
        get => _levelSegmentsIndexes;
        set => _levelSegmentsIndexes = value ?? new List<int>();
    }
    
    public int CoinsCount
    {
        get => _coinsCount;
        //TODO: should be private and AddCoins method from GameController could be there instead
        set => _coinsCount = value < 0 ? 0 : value;
    }

    public int CurrentLevel
    {
        get => _currentLevel;
        set => _currentLevel = value < _currentLevel ? _currentLevel : value;
    }

    public int PlayerSkin
    {
        get => _playerSkin;
        set => _playerSkin = value;
    }

    public List<bool> PurchasedSkins
    {
        get => _purchasedSkins;
        set => _purchasedSkins = value;
    }

    public GameSave()
    {
        LevelSegmentsIndexes = new List<int>();
        CoinsCount = 0;
        CurrentLevel = 1;
        _playerSkin = 0;
        PurchasedSkins = new List<bool>() {true, false, false, false, false, false, false, false, false};
    }
    
    public GameSave(List<int> levelSegmentsIndexes, int coinsCount, int currentLevel, int playerSkin, List<bool> purchasedSkins)
    {
        LevelSegmentsIndexes = levelSegmentsIndexes;
        CoinsCount = coinsCount;
        CurrentLevel = currentLevel;
        PlayerSkin = playerSkin;
        PurchasedSkins = purchasedSkins;
    }
}