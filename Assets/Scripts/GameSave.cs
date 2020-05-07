using System.Collections.Generic;

public class GameSave
{
    private List<int> _currentLevelSegments;
    public List<int> CurrentLevelSegments
    {
        get => _currentLevelSegments;
        set => _currentLevelSegments = value ?? new List<int>();
    }
    
    private int _coinsCount;
    public int CoinsCount
    {
        get => _coinsCount;
        set
        {
            var newCount = _coinsCount + value;
            _coinsCount = newCount <= 0 ? 0 : newCount;
        }
    }

    private int _totalLevelsComplete;
    public int TotalLevelsComplete
    {
        get => _totalLevelsComplete;
        set
        {
            var newCount = _totalLevelsComplete + value;
            //complete level count couldn't be lower than previous? :/
            _totalLevelsComplete = newCount <= _totalLevelsComplete ? _totalLevelsComplete : newCount;
        }
    }


    public GameSave(List<int> currentLevelSegments, int coinsCount, int totalLevelsComplete)
    {
        CurrentLevelSegments = currentLevelSegments;
        CoinsCount = coinsCount;
        TotalLevelsComplete = totalLevelsComplete;
    }
}