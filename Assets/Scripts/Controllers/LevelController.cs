using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController Instance { get; private set; }
        
        private GameObject[] AvailableSegments { get; set; }
        private readonly List<int> _moneySegments = new List<int>();
        private readonly List<int> _easySegments = new List<int>();
        private readonly List<int> _hardSegments = new List<int>();
        private const float HalfHeight = 12.0f;
        private const int DefaultSegmentsCountToUse = 4;
        
        public List<int> GenerateRandomIndexes()
        {
            return GenerateRandomIndexes(DefaultSegmentsCountToUse);
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
        
        private List<int> GenerateRandomIndexes(int segmentsCount)
        {
            var currentLevel = SaveController.Instance.Save.CurrentLevel;
            var indexesList = new List<int>();
            
            if (currentLevel <= 5)
            {
                for (var i = 0; i < segmentsCount; i++)
                    indexesList.Add(_easySegments[Random.Range(0, _easySegments.Count)]);
            }
            else if (currentLevel % 10 == 0)
            {
                for (var i = 0; i < segmentsCount; i++)
                    indexesList.Add(_hardSegments[Random.Range(0, _hardSegments.Count)]);
            }
            else if (currentLevel % 7 == 0)
            {
                var normalSegments = Random.Range(1, 3);

                for (var i = 0; i < segmentsCount; i++)
                {
                    if (normalSegments > 0)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            indexesList.Add(Random.Range(0, AvailableSegments.Length));
                            normalSegments--;
                        }
                        else
                            indexesList.Add(_moneySegments[Random.Range(0, _moneySegments.Count)]);
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
            for (var i = 0; i < AvailableSegments.Length; i++)
            {
                switch (AvailableSegments[i].GetComponent<SegmentClass>().hardLevel)
                {
                    case 1:
                        _easySegments.Add(i);
                        break;
                    case 3:
                        _hardSegments.Add(i);
                        break;
                    default:
                        _moneySegments.Add(i);
                        break;
                }
            }
        }
    }
}
