using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    
    private string SavePath => Application.persistentDataPath + "/GameSave.json";
    
    public List<Campfire> campfires = new List<Campfire>();
    private string lastCampfireId = "";
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 确保SaveManager不会跨场景销毁
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void AddCampfire(Campfire campfire)
    {
        if (!campfires.Contains(campfire))
        {
            campfires.Add(campfire);
        }
    }
    
    public void RemoveCampfire(Campfire campfire)
    {
        campfires.Remove(campfire);
    }
    
    public void SaveGame(string campfireId)
    {
        lastCampfireId = campfireId;
        
        SaveData saveData = new SaveData
        {
            lastCampfireId = lastCampfireId,
            campfireStates = new Dictionary<string, bool>(),
            playerPosition = Player.instance != null ? Player.instance.transform.position : Vector3.zero
        };
        
        foreach (Campfire campfire in campfires)
        {
            saveData.campfireStates[campfire.campfireId] = campfire.isActivated;
        }
        
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("游戏已保存");
    }
    
    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("没有找到存档文件");
            return;
        }
        
        string json = File.ReadAllText(SavePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        
        lastCampfireId = saveData.lastCampfireId;
        
        foreach (Campfire campfire in campfires)
        {
            if (saveData.campfireStates.TryGetValue(campfire.campfireId, out bool isActivated))
            {
                campfire.isActivated = isActivated;
            }
        }
        
        Debug.Log("游戏已加载");
    }
    
    public Vector3 GetRespawnPosition()
    {
        if (string.IsNullOrEmpty(lastCampfireId))
        {
            return Vector3.zero;
        }
        
        foreach (Campfire campfire in campfires)
        {
            if (campfire.campfireId == lastCampfireId)
            {
                return campfire.transform.position;
            }
        }
        
        return Vector3.zero;
    }
    
    public bool HasSaveData()
    {
        return File.Exists(SavePath);
    }
}

[System.Serializable]
public class SaveData
{
    public string lastCampfireId;
    public Dictionary<string, bool> campfireStates;
    public Vector3 playerPosition;
}