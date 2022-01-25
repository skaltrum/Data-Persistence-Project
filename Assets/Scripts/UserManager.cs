using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserManager : MonoBehaviour
{
    public class SaveData
    {
        public string Name;
        public int HighScore;
    }


    public static UserManager Instance;

    private string playerName;
    public string PlayerName {
        get 
        {
            return playerName ?? "Unknown";
        }
        set
        {
            playerName = value;
        }
    }

    private SaveData highScoreData;

    public SaveData HighScoreData
    {
        get { 
            //if(highScoreData == null) 
            LoadHighScore();
            return highScoreData;
        }
    }

    public void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveHighScore(string Name, int Score)
    {
        Debug.Log($"Saving new high score for: {Name} | {Score}");

        SaveData data = new SaveData
        {
            Name = Name,
            HighScore = Score
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            highScoreData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            highScoreData = new SaveData() { HighScore = 0, Name = "none" };
        }
    }
}
