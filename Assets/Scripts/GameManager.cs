using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public List<string> userName;
    public List<int> bestScore;

    public string tempUserName = "";

    [System.Serializable]
    public class SaveData
    {
        public string userName;
        public string bestScore;
    }

    public void SaveDataToJSON()
    {
        SaveData data = new SaveData();
        
        data.userName = ConvertToString(userName);
        data.bestScore = ConvertToString(bestScore);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/pongsavefile.json", json);
    }

    public void LoadFileFromJSON()
    {
        string path = Application.persistentDataPath + "/pongsavefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            userName = ConvertStringToListString(data.userName);
            bestScore = ConvertStringToListInt(data.bestScore);
        }
    }

    private void Awake()
    {
        if (Instance ==  null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string ConvertToString(List<string> list)
    {
        string result = "";

        foreach  (string item in list)
        {
            result += item + ",";
        }
        return result;
    }

    private string ConvertToString(List<int> list)
    {
        string result = "";

        foreach (int item in list)
        {
            result += item + ",";
        }
        return result;
    }

    private List<string> ConvertStringToListString(string input)
    {
        string[] array = input.Split(char.Parse(","));
        List<string> result = new List<string>();
        result.AddRange(array);
        result.RemoveAt(result.Count - 1);
        return result;
    }

    private List<int> ConvertStringToListInt(string input)
    {
        string[] array = input.Split(char.Parse(","));
        Debug.Log(array);
        List<int> result = new List<int>();

        foreach (string item in array)
        {
            if (item != "")
            {
                result.Add(int.Parse(item));
            }
        }
        return result;
    }
}
