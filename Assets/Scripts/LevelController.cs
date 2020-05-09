using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public const int DefaultSegmentsCountToUse = 4;
    public const float HalfHeight = 12.0f;
    
    public static LevelController Instance { get; private set; }
    public GameObject[] AvailableSegments { get; private set; }


    public List<int> GenerateRandomIndexes()
    {
        return GenerateRandomIndexes(DefaultSegmentsCountToUse);
    }
    
    public List<int> GenerateRandomIndexes(int segmentsCount)
    {
        //TODO выборка неповторяющихся чисел ???
        var indexesList = new List<int>();

        for (var i = 0; i < segmentsCount; i++)
            indexesList.Add(Random.Range(0, AvailableSegments.Length));
        return indexesList;
    }

    public void SetUpScene()
    {
        var yPos = -HalfHeight;

        foreach (var index in SaveController.Instance.Save.LevelSegmentsIndexes)
        {
            yPos += HalfHeight * 2;
            Instantiate(AvailableSegments[index], new Vector3(0.0f, yPos, 0.0f), Quaternion.identity);
        }
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
        
        AvailableSegments = Resources.LoadAll<GameObject>("Prefabs/Segments");
    }
}

/*
        // if (num > segments.Length)
        //     return null;
        // int[] segmentsNum = new int[num];
        // int maxRandom = segments.Length;
        // int minRandom = num;
        // int alreadyTaken = 0;
        // for (int i = 0; i < segments.Length; i++)
        // {
        //     int a = Random.Range(i, maxRandom);
        //     if (a < minRandom + i)
        //     {
        //         segmentsNum[alreadyTaken] = i;
        //         maxRandom -= 1;
        //         minRandom -= 1;
        //         alreadyTaken += 1;
        //     }
        //
        //     if (alreadyTaken == num - 1)
        //         break;
        // }
        //
        // return segmentsNum;
 */
