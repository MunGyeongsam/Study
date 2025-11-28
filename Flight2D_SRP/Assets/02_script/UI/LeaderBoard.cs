using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[Serializable]
public class LeaderBoardEntry
{
    public string playerName;
    public int score;

    public LeaderBoardEntry(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}

[Serializable]
public class LeaderBoardData
{
    public List<LeaderBoardEntry> entries = new List<LeaderBoardEntry>();
}

public class LeaderBoard : MonoBehaviour
{
    
    TextMeshProUGUI _textMesh;
    
    private LeaderBoardData _data = new LeaderBoardData();
    private string _savePath;

    void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        LoadData();
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    void OnDestroy()
    {
        SaveData();
    }

    public void AddEntry(string playerName, int score)
    {
        _data.entries.Add(new LeaderBoardEntry(playerName, score));
        _data.entries.Sort((a, b) => b.score.CompareTo(a.score)); // 점수 내림차순
        
        // 상위 10개만 유지
        if (_data.entries.Count > 10)
            _data.entries.RemoveRange(10, _data.entries.Count - 10);
    }

    public List<LeaderBoardEntry> GetEntries()
    {
        return _data.entries;
    }

    public void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(_data, true);
            File.WriteAllText(_savePath, json);
            Debug.Log($"LeaderBoard saved to {_savePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save leaderboard: {e.Message}");
        }
    }

    void LoadData()
    {
        if (File.Exists(_savePath))
        {
            try
            {
                string json = File.ReadAllText(_savePath);
                _data = JsonUtility.FromJson<LeaderBoardData>(json);
                Debug.Log($"LeaderBoard loaded: {_data.entries.Count} entries");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load leaderboard: {e.Message}");
                _data = new LeaderBoardData();
            }
        }
        else
        {
            Debug.Log("No leaderboard file found, creating new one");
        }
    }
}
