using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSave
{
    [SerializeField] private List<int> _levelSegmentsIndexes;
    [SerializeField] private int _coinsCount;
    [SerializeField] private int _totalLevelsComplete;
    
    public List<int> LevelSegmentsIndexes
    {
        get => _levelSegmentsIndexes;
        set => _levelSegmentsIndexes = value ?? new List<int>();
    }
    
    public int CoinsCount
    {
        get => _coinsCount;
        set => _coinsCount = value < 0 ? 0 : value;
    }

    public int TotalLevelsComplete
    {
        get => _totalLevelsComplete;
        //complete level count couldn't be lower than previous? :/
        set => _totalLevelsComplete = value < _totalLevelsComplete ? _totalLevelsComplete : value;
    }


    public GameSave(List<int> levelSegmentsIndexes, int coinsCount, int totalLevelsComplete)
    {
        LevelSegmentsIndexes = levelSegmentsIndexes;
        CoinsCount = coinsCount;
        TotalLevelsComplete = totalLevelsComplete;
    }
}