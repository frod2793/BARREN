using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDataList : MonoBehaviour
{
    [Serializable]
    public struct CharacterData
    {
        public string CharacterListName;
        public GameObject CharacterCard;
        public Text CharacterName;
        public Slider CharacterSlider;
        public Image characterImage;
        public Button CharacterTalkBtn;
    }

    [SerializeField] private Text listcount;
    [SerializeField] private Button StartButton;
    public List<CharacterData> CharacterData_List = new List<CharacterData>();


    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerData.Instance.IsTutorial )
        {  
            
            DataSetting();
            firstSetting();
            PlayerData.Instance.isDay[0] = true;
        }
        else
        {
          
             string count = CharacterData_List.FindAll(x => x.CharacterCard.activeSelf == true).Count.ToString();
             listcount.text = "[" + count + "/3]";
            
            // print("buttonsetting");  
             DataSetting();
        }
        
      
    }

    private void firstSetting()
    { 
        var tempCharacter = CharacterData_List[0];
        CharacterData_List[0].CharacterCard.SetActive(true);
        tempCharacter.CharacterName.text = "??";
        tempCharacter.CharacterSlider.value = 0;
        tempCharacter.characterImage=CharacterData_List[0].characterImage;
        tempCharacter.characterImage.color = new Color(1,1,1);;
        tempCharacter.CharacterTalkBtn = CharacterData_List[0].CharacterTalkBtn;
        CharacterData_List[0] = tempCharacter;

    }

    private void DataSetting()
    {
        InitCharacterData();
        //CharacterData_List 중 CharacterCard 오브젝트가 활성화 되어있는 오브젝트의 갯수를 count.text에 넣는다
        string count = CharacterData_List.FindAll(x => x.CharacterCard.activeSelf == true).Count.ToString();
        listcount.text = "[" + count + "/3]";
        
    }

    private void InitCharacterData()
    {

        for (int i = 0; i < CharacterData_List.Count; i++)
        {
            for (int j = 0; j < PlayerData.Instance.CharacterData_List.Count; j++)
            {


                if (CharacterData_List[j].CharacterName.text == PlayerData.Instance.CharacterData_List[j].CharacterName)
                {
                    var tempCharacter = CharacterData_List[0];
                    tempCharacter.CharacterName.text = PlayerData.Instance.CharacterData_List[j].CharacterName;
                    tempCharacter.CharacterSlider.value = PlayerData.Instance.CharacterData_List[j].CharacterSlider;
                    tempCharacter.characterImage = PlayerData.Instance.CharacterData_List[j].characterImage;
                    CharacterData_List[0] = tempCharacter;
                }

                
            }

        }
         
        
        if (CharacterData_List[0].CharacterCard == null)
        {
            // Find the character card GameObject
            GameObject characterCard = GameObject.FindGameObjectWithTag("Cure");
        
            if (characterCard != null)
            {
                // Extract required components from the characterCard GameObject
                Text CharacterName = characterCard.GetComponentInChildren<Text>();
                Slider CharacterSlider = characterCard.GetComponentInChildren<Slider>();
                Image characterImage = characterCard.GetComponentInChildren<Image>();
                Button CharacterTalkBtn = characterCard.GetComponentInChildren<Button>();
            
                // Update the CharacterCard and related properties
                var tempCharacter = CharacterData_List[0];
                tempCharacter.CharacterCard = characterCard;
                tempCharacter.CharacterName = CharacterName;
                tempCharacter.CharacterSlider = CharacterSlider;
                tempCharacter.characterImage = characterImage;
               CharacterData_List[0] = tempCharacter;
            }
            else
            {
                Debug.LogError("Character card not found.");
            }
        }
    }
}