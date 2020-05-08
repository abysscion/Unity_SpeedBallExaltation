using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject[] availableSegments;
    public int segmentsCountToUse = 4;
    public int[] generatedSegmentsIndexes;

    private const float HalfHeight = 12.0f;

    private void Start()
    {
        if (availableSegments.Length <= 0)
            availableSegments = Resources.LoadAll<GameObject>("Prefabs/Segments");
        if (GameController.CurrentSave == null)
            GameController.CurrentSave = SaveManager.LoadGameFromFile() ?? new GameSave(
                this.GenerateRandomIndexes(Random.Range(2, 4)).ToList(), 
                0, 
                0);
        //TODO установить правила рандома ???
        var yPos = -HalfHeight;
        
        generatedSegmentsIndexes = GenerateRandomIndexes(segmentsCountToUse);
        foreach (var index in generatedSegmentsIndexes)
        {
            yPos += HalfHeight * 2;
            Instantiate(availableSegments[index], new Vector3(0.0f, yPos, 0.0f), Quaternion.identity);
        }
    }

    public int[] GenerateRandomIndexes(int segmentsCount)
    {
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
        
        //TODO выборка неповторяющихся чисел ???
        
        // there is no problem yet imo bcs of repeatability
        // if (segmentsCount > availableSegments.Length)
        //     return null; 
        
        var indexesArr = new int[segmentsCount];
        
        for (var i = 0; i < segmentsCount; i++)
            indexesArr[i] = Random.Range(0, availableSegments.Length);
        return indexesArr;
    }
}
