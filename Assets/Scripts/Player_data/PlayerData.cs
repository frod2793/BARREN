using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerData : MonoBehaviour
{
    // Singleton instance
    private static PlayerData instance;

    public List<CharacterDataList.CharacterData> CharacterData_List = new List<CharacterDataList.CharacterData>();

    private int experience = 0;
    private string playerName = string.Empty;
    private string gender = string.Empty;
    private bool isTutorial = false;
    private bool[] isUnlock = new bool[5];
    public int Experience
    {
        get => experience;
        set => experience = value;
    }

    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }

    public string Gender
    {
        get => gender;
        set => gender = value;
    }

    public bool IsTutorial
    {
        get => isTutorial;
        set => isTutorial = value;
    }

    public bool IsUnlock(int index)
    {
        if (index >= 0 && index < isUnlock.Length)
        {
            return isUnlock[index];
        }
        return false;
    }

    public void SetUnlock(int index, bool value)
    {
        if (index >= 0 && index < isUnlock.Length)
        {
            isUnlock[index] = value;
        }
    }
    // Singleton instance property
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                // Create a new GameObject to hold the instance
                GameObject singletonObject = new GameObject("PlayerDataSingleton");
                instance = singletonObject.AddComponent<PlayerData>();
            }
            return instance;
        }
    }

    // Ensure the instance is not destroyed when loading a new scene
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        // Initialize default values
        experience = 0;
        playerName = "Player";
        gender = "Unknown";
    }

    // Update is called once per frame

    public float GetLikeGage(string character)
    {
        try
        {
            return CharacterData_List.Find(x => x.CharacterListName == character).CharacterSlider.value;

        }
        catch
        {
            return 0;
        }
        
    }
    
 
}