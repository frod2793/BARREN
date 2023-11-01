using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetcharacterData_Manager : MonoBehaviour
{  
    [Serializable]
    public struct CharacterData
    {
        public string characterListName;
        public GameObject characterCard;
        public string characterName;
        public Slider characterSlider;
        public Text characterSliderText;
        public Image characterImage;
    }
    public List<CharacterData> characterDataList = new List<CharacterData>();

    void Start()
    {
        SetCharacterData();
    }

    private void SetCharacterData()
    {
        for (int i = 0; i < characterDataList.Count; i++)
        {
            if (characterDataList[i].characterListName == PlayerData.Instance.CharacterData_List[i].CharacterListName)
            {
                characterDataList[i].characterSlider.value = PlayerData.Instance.CharacterData_List[i].CharacterSlider *0.01f;
                characterDataList[i].characterSliderText.text = PlayerData.Instance.CharacterData_List[i].CharacterSlider.ToString();
            }
        }
    }
}
