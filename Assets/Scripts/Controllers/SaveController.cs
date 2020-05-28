using System.IO;
using UnityEngine;

namespace Controllers
{
    public class SaveController : MonoBehaviour
    {
        private string _filePath;
    
        public static SaveController Instance { get; private set; }
        public GameSave Save { get; private set; }

        private string FilePath
        {
            get
            {
                if (_filePath == null)
                {
                    _filePath = Path.Combine(Application.persistentDataPath, "SpeedBallExaltation_Save");
                    _filePath = Path.ChangeExtension(_filePath, "sberbang");
                }
                return _filePath;
            }
        }
    
        public void LoadGameFromFile()
        {
            if (!File.Exists(FilePath))
                return;
        
            using (var fs = new FileStream(FilePath, FileMode.Open))
            using (var reader = new StreamReader(fs))
                Save = JsonUtility.FromJson<GameSave>(reader.ReadToEnd());
        }

        public void SaveGameToFile()
        {
            if (Save == null)
                Save = new GameSave();

            using (var fs = new FileStream(FilePath, FileMode.Create))
            using (var writer = new StreamWriter(fs))
                writer.Write(JsonUtility.ToJson(Save, true));
        }
    
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Debug.Log("[ATTENTION] Multiple " + this + " found!");
        }
    }
}
