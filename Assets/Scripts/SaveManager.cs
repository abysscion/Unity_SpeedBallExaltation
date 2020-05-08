using System.IO;
using UnityEngine;

public static class SaveManager
{
    public const string GameName = "SpeedBallExaltation";
    
    private static string _filePath;
    public static string FilePath
    {
        get
        {
            if (_filePath == null)
            {
                _filePath = Path.Combine(Application.persistentDataPath, GameName + "_save");
                _filePath = Path.ChangeExtension(_filePath, "sberbang");
            }
            return _filePath;
        }
    }
    
    public static GameSave LoadGameFromFile()
    {
        if (!File.Exists(FilePath)) return null;
        
        using (var fs = new FileStream(FilePath, FileMode.Open)) 
        using (var reader = new StreamReader(fs)) 
            return JsonUtility.FromJson<GameSave>(reader.ReadToEnd());
    }

    public static void SaveGameToFile(GameSave save)
    {
        if (save == null) return;

        using (var fs = new FileStream(FilePath, FileMode.Create))
        using (var writer = new StreamWriter(fs))
        {
            var x = JsonUtility.ToJson(save, true);
            var y = JsonUtility.ToJson(save);
            writer.Write(x);
        }
    }
}
