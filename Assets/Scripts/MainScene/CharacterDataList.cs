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

    public List<CharacterData> CharacterData_List = new List<CharacterData>();


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.Instance.IsTutorial )
        {
            DataSetting();
        }
        else
        {
             PlayerData.Instance.CharacterData_List=CharacterData_List;
             string count = CharacterData_List.FindAll(x => x.CharacterCard.activeSelf == true).Count.ToString();
             listcount.text = "[" + count + "/3]";
        
             CharacterData_List[0].CharacterTalkBtn.onClick.AddListener(() =>
             {
                 SceneLoader.Instace.LoadScene("Travel1");
             }); 
        }
        
      
    }

    private void DataSetting()
    {
        CharacterData_List = PlayerData.Instance.CharacterData_List;

        //CharacterData_List 중 CharacterCard 오브젝트가 활성화 되어있는 오브젝트의 갯수를 count.text에 넣는다
        string count = CharacterData_List.FindAll(x => x.CharacterCard.activeSelf == true).Count.ToString();
        listcount.text = "[" + count + "/3]";
        
        CharacterData_List[0].CharacterTalkBtn.onClick.AddListener(() =>
        {
            SceneLoader.Instace.LoadScene("Travel1");
        }); 
        
    }
}