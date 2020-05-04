using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject[] segments;
    public int[] randomNum;
    private float _halfHeight = 12.0f;

    // Start is called before the first frame update
    void Start()
    {
        //TODO установить правила рандома ???
        randomNum = takeRandomSegments(4);
        float yPos = -12.0f;
        Vector3 position = new Vector3();
        for (int i = 0; i < randomNum.Length; i++)
        {
            yPos = yPos + _halfHeight * 2;
            position = new Vector3(0.0f, yPos, 0.0f);
            Instantiate(segments[randomNum[i]], position, Quaternion.identity);
        }
    }

    private int[] takeRandomSegments(int num)
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
        
        if (num > segments.Length)
            return null;
        int[] segmentsNum = new int[num];
        for (int i = 0; i < num; i++)
        {
            segmentsNum[i] = Random.Range(0, 7);
        }

        return segmentsNum;
    }
    
}
