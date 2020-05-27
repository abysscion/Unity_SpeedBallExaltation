using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        public const int DefaultSegmentsCountToUse = 4;
        public const float HalfHeight = 12.0f;

        public static LevelController Instance { get; private set; }
        public GameObject[] AvailableSegments { get; private set; }

        private List<int> _easySegments = new List<int>();
        private List<int> _hardSegments = new List<int>();
        private List<int> _moneySegments = new List<int>();


        public List<int> GenerateRandomIndexes()
        {
            return GenerateRandomIndexes(DefaultSegmentsCountToUse);
        }
    
        public List<int> GenerateRandomIndexes(int segmentsCount)
        {
            int currentLevel = SaveController.Instance.Save.CurrentLevel;
            var indexesList = new List<int>();
            // первые 5 уровней всегда легкие
            if (currentLevel <= 5)
            {
                for (var i = 0; i < segmentsCount; i++)
                {
                    int b = Random.Range(0, _easySegments.Count);   
                    indexesList.Add(_easySegments[b]);
                }
            }
            // каждый 10 уровень -- "босс-уровень"
            else if (currentLevel % 10 == 0)
            {
                for (var i = 0; i < segmentsCount; i++)
                    indexesList.Add(_hardSegments[Random.Range(0, _hardSegments.Count)]);
            }
            // каждый 7 уровень денежный
            else if (currentLevel % 7 == 0)
            {
                int normalSegments = Random.Range(1, 3);
                int moneySegments = segmentsCount - normalSegments;
                for (var i = 0; i < segmentsCount; i++)
                {
                    if (normalSegments > 0)
                    {
                        int a = Random.Range(0, 2);
                        if (a == 0)
                        {
                            indexesList.Add(Random.Range(0, AvailableSegments.Length));
                            normalSegments--;
                        }
                        else
                        {
                            indexesList.Add(_moneySegments[Random.Range(0, _moneySegments.Count)]);
                            moneySegments--;
                        }   
                    }
                    else
                        indexesList.Add(_moneySegments[Random.Range(0, _moneySegments.Count)]);
                }
            }
            else
            {
                for (var i = 0; i < segmentsCount; i++) 
                    indexesList.Add(Random.Range(0, AvailableSegments.Length));
            }

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
            
            for (int i = 0; i < AvailableSegments.Length; i++)
            {
                int difficult = AvailableSegments[i].GetComponent<SegmentClass>().hardLevel;
                if (difficult == 1)
                    _easySegments.Add(i);
                else if (difficult == 3)
                    _hardSegments.Add(i);
                else
                    _moneySegments.Add(i);
            }
        }
    }
}
