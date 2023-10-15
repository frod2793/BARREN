using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerData : MonoBehaviour,IobjectItem
{
    // Singleton instance
    private static PlayerData instance;
    [Serializable]
    public struct CharacterData
    {
        public string CharacterListName;
        public GameObject CharacterCard;
        public string CharacterName;
        public float CharacterSlider;
        public Image characterImage;
        public Button CharacterTalkBtn;
    }
    public List<CharacterData> CharacterData_List = new List<CharacterData>();

    private int experience = 0;
    private string playerName = string.Empty;
    private string gender = string.Empty;
    public bool isTutorial = false;
    private bool[] isUnlock = new bool[5];
    public bool[] isDay = new bool[5];
    public string JsonName;
    public string startbtntext;
    
    [Header("아이템")]
    public Item[] item;
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
    public Item ClickItem(int index) {
        return item[index];
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
    { string name = null;
        if (character.Contains("Cure"))
        {
            name = "Cure";
            print(name);
        }
        else if (character.Contains("Kid"))
        {
            name = "Kid";
        }
        else
        if (character.Contains("GangMin"))
        {
            name = "GangMin";
        }

        try
        {
            return CharacterData_List.Find(x => x.CharacterListName == name).CharacterSlider;

        }
        catch
        {
            return 0;
        }
        
    }
    public void SetLikeGage(string character,float value)
    {
        print("이름 : "+character);
      //CharacterData_List 에 CharacterData_List.CharacterListName 가 character와 같은 것을 찾아 CharacterData_List.CharacterSlider를 value로 바꾼다
        var tempCharacter = CharacterData_List.Find(x => x.CharacterListName == character);
     
        float temp = tempCharacter.CharacterSlider +value;
        tempCharacter.CharacterSlider = temp;
        CharacterData_List[CharacterData_List.FindIndex(x => x.CharacterListName == character)] = tempCharacter;
    }
    
    
 
}