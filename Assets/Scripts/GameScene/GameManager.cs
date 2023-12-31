using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct ButtonGroup
    {
        [SerializeField] public string GroupName;
        [SerializeField] public GameObject ButtonGroup1;
        [SerializeField] public Button Chosebtn1;
        [SerializeField] public Button Chosebtn2;
        [SerializeField] public Button Chosebtn3;
        [SerializeField] public TextMeshProUGUI Chosebtn1Text;
        [SerializeField] public TextMeshProUGUI Chosebtn2Text;
        [SerializeField] public TextMeshProUGUI Chosebtn3Text;
    }

    public List<ButtonGroup> ButtonGroupList = new List<ButtonGroup>();

    [SerializeField] private Button skipTextBtn;
    [Header("버튼 레이아웃")] [SerializeField] private GameObject ButtonLayout;

    [Header("버튼 그룹 1")] private Game_PrologManager _gamePrologManager;

    public Image EndingEffectObj;
    
    public GameObject LikeEffectObj;
    public GameObject DisLikeEffectObj;
    public Text LikeText;
    public Text DisLikeText;
    [Header("인벤토리 버튼")] [SerializeField] private Button Inventory_Btn;
    [SerializeField] private GameObject inventoryObjPrefab;
    [SerializeField] private GameObject inventoryObjaprent;

    private GameObject inventoryObj;
    // Start is called before the first frame update
    void Start()
    {
        _gamePrologManager = FindObjectOfType<Game_PrologManager>();
        try
        {
             inventoryObj =  Instantiate(inventoryObjPrefab, inventoryObjaprent.transform);
           
            Inventory_Btn.onClick.AddListener((() =>
            {
                //todo: 인벤토리 버튼 누르면 인벤토리 창 띄우기
                TweenEffect.OpenPopup(inventoryObj);
            }));

        }
        catch 
        {
    
        }
       
       
    }

    public void OpenInventory()
    {
        TweenEffect.OpenPopup(inventoryObj);
    }
    public void ActiveButtonGroup1()
    {
        //ButtonGroupList 중 name이 Group1 인 오브젝트를 활성화 시킨다 

        skipTextBtn.gameObject.SetActive(false);
        if (ButtonGroupList[0].GroupName == "Group1")
        {
            ButtonGroupList[0].ButtonGroup1.SetActive(true);
            ButtonGroupList[0].Chosebtn1Text.text = _gamePrologManager.Chosebtn1Text;
            ButtonGroupList[0].Chosebtn2Text.text = _gamePrologManager.Chosebtn2Text;
            ButtonGroupList[0].Chosebtn1.onClick.AddListener((() =>
            {
                StartCoroutine(co_Delaybool());
                ButtonGroupList[0].ButtonGroup1.SetActive(false);
            }));
        }
    }

    public void ActiveButtonGroup1Player()
    {
        skipTextBtn.gameObject.SetActive(false);
        if (ButtonGroupList[1].GroupName == "EnableBtnPlayGroup1")
        {
            ButtonGroupList[1].ButtonGroup1.SetActive(true);
            ButtonGroupList[1].Chosebtn1Text.text = _gamePrologManager.Chosebtn1Text;
            ButtonGroupList[1].Chosebtn2Text.text = _gamePrologManager.Chosebtn2Text;
            if (ButtonGroupList[1].Chosebtn3Text != null)
            {
                ButtonGroupList[1].Chosebtn3Text.text = _gamePrologManager.Chosebtn3Text;
            }

            ButtonGroupList[1].Chosebtn1.onClick.AddListener((() =>
            {
                //todo: 선택한것 구분 짓기
                ButtonGroupList[1].ButtonGroup1.SetActive(false);

                _gamePrologManager.Func_skipText();
                if ( ButtonGroupList[1].Chosebtn1Text.text.Contains("선물"))
                {
                  
                }
                else
                {
                    StartCoroutine(co_Delaybool());
                }
            }));
            ButtonGroupList[1].Chosebtn2.onClick.AddListener((() =>
            {
                //todo: 선택한것 구분 짓기
                ButtonGroupList[1].ButtonGroup1.SetActive(false);

                _gamePrologManager.Func_skipText();
                if (ButtonGroupList[1].Chosebtn2Text.text.Contains("선물"))
                {
                  print("선물: "+ButtonGroupList[1].Chosebtn2Text.text);
                }
                else
                {
                    StartCoroutine(co_Delaybool());
                }
            }));
            if (ButtonGroupList[1].Chosebtn3 != null)
            {
                ButtonGroupList[1].Chosebtn3.onClick.AddListener((() =>
                {
                    //todo: 선택한것 구분 짓기
                    ButtonGroupList[1].ButtonGroup1.SetActive(false);

                    _gamePrologManager.Func_skipText();

                    StartCoroutine(co_Delaybool());
                }));
            }
        }
    }

    public void ActiveButtonGroup2Player()
    {
        skipTextBtn.gameObject.SetActive(false);
        if (ButtonGroupList[2].GroupName == "EnableBtnPlayGroup2")
        {
            ButtonGroupList[2].ButtonGroup1.SetActive(true);
            ButtonGroupList[2].Chosebtn1Text.text = _gamePrologManager.Chosebtn1Text;
            ButtonGroupList[2].Chosebtn2Text.text = _gamePrologManager.Chosebtn2Text;
            if (ButtonGroupList[2].Chosebtn3Text != null)
            {
                ButtonGroupList[2].Chosebtn3Text.text = _gamePrologManager.Chosebtn3Text;
            }

            ButtonGroupList[2].Chosebtn1.onClick.AddListener((() =>
            {
                //todo: 선택한것 구분 짓기
                ButtonGroupList[2].ButtonGroup1.SetActive(false);

                _gamePrologManager.Func_skipText();

                StartCoroutine(co_Delaybool());
            }));
            ButtonGroupList[2].Chosebtn2.onClick.AddListener((() =>
            {
                //todo: 선택한것 구분 짓기
                ButtonGroupList[2].ButtonGroup1.SetActive(false);

                _gamePrologManager.Func_skipText();

                StartCoroutine(co_Delaybool());
            }));
            if (ButtonGroupList[2].Chosebtn3 != null)
            {
                ButtonGroupList[2].Chosebtn3.onClick.AddListener((() =>
                {
                    ButtonGroupList[2].ButtonGroup1.SetActive(false);

                    _gamePrologManager.Func_skipText();

                    StartCoroutine(co_Delaybool());
                }));
            }
        }
    }

    public void ActiveButtonGroup3Player()
    {
        if (ButtonGroupList[3].GroupName == "Group3Player")
        {
            ButtonGroupList[3].ButtonGroup1.SetActive(true);
            ButtonGroupList[3].Chosebtn1Text.text = _gamePrologManager.Chosebtn1Text;
            ButtonGroupList[3].Chosebtn2Text.text = _gamePrologManager.Chosebtn2Text;
            if (ButtonGroupList[3].Chosebtn3Text != null)
            {
                ButtonGroupList[3].Chosebtn3Text.text = _gamePrologManager.Chosebtn3Text;
            }

            ButtonGroupList[3].Chosebtn1.onClick.AddListener((() =>
            {
                //todo: 선택한것 구분 짓기
                ButtonGroupList[3].ButtonGroup1.SetActive(false);

                _gamePrologManager.Func_skipText();

                StartCoroutine(co_Delaybool());
            }));
            if (ButtonGroupList[3].Chosebtn2 != null)
            {
                ButtonGroupList[3].Chosebtn2.onClick.AddListener((() =>
                {
                    //todo: 선택한것 구분 짓기
                    ButtonGroupList[3].ButtonGroup1.SetActive(false);

                    _gamePrologManager.Func_skipText();

                    StartCoroutine(co_Delaybool());
                }));
            }

            if (ButtonGroupList[3].Chosebtn3 != null)
            {
                ButtonGroupList[3].Chosebtn3.onClick.AddListener((() =>
                {
                    //todo: 선택한것 구분 짓기
                    ButtonGroupList[3].ButtonGroup1.SetActive(false);

                    _gamePrologManager.Func_skipText();

                    StartCoroutine(co_Delaybool());
                }));
            }
        }
    }

    public void ActiveButtonGroup4Player()
    {
        if (ButtonGroupList[4].GroupName == "Group4Player")
        {
            ButtonGroupList[4].ButtonGroup1.SetActive(true);
            ButtonGroupList[4].Chosebtn1Text.text = _gamePrologManager.Chosebtn1Text;
            ButtonGroupList[4].Chosebtn2Text.text = _gamePrologManager.Chosebtn2Text;
            if (ButtonGroupList[4].Chosebtn3Text != null)
            {
                ButtonGroupList[4].Chosebtn3Text.text = _gamePrologManager.Chosebtn3Text;
            }

            ButtonGroupList[4].Chosebtn1.onClick.AddListener((() =>
            {
                //todo: 선택한것 구분 짓기
                ButtonGroupList[4].ButtonGroup1.SetActive(false);
                _gamePrologManager.Func_skipText();

                StartCoroutine(co_Delaybool());
            }));

            ButtonGroupList[4].Chosebtn2.onClick.AddListener((() =>
            {
                //todo: 선택한것 구분 짓기
                ButtonGroupList[4].ButtonGroup1.SetActive(false);
                _gamePrologManager.Func_skipText();

                StartCoroutine(co_Delaybool());
            }));

            if (ButtonGroupList[4].Chosebtn3 != null)
            {
                ButtonGroupList[4].Chosebtn2.onClick.AddListener((() =>
                {
                    //todo: 선택한것 구분 짓기
                    ButtonGroupList[4].ButtonGroup1.SetActive(false);
                    _gamePrologManager.Func_skipText();

                    StartCoroutine(co_Delaybool());
                }));
            }
        }
    }

    public void ActiveButtonGroup5Player()
    {
        if (ButtonGroupList[5].GroupName == "Group5Player")
        {
            ButtonGroupList[5].ButtonGroup1.SetActive(true);
            ButtonGroupList[5].Chosebtn1Text.text = _gamePrologManager.Chosebtn1Text;
            ButtonGroupList[5].Chosebtn2Text.text = _gamePrologManager.Chosebtn2Text;
            if (ButtonGroupList[5].Chosebtn3Text != null)
            {
                ButtonGroupList[5].Chosebtn3Text.text = _gamePrologManager.Chosebtn3Text;
            }

            ButtonGroupList[5].Chosebtn1.onClick.AddListener((() =>
            {
                //todo: 선택한것 구분 짓기
                ButtonGroupList[5].ButtonGroup1.SetActive(false);
                _gamePrologManager.Func_skipText();

                StartCoroutine(co_Delaybool());
            }));

            if (ButtonGroupList[5].Chosebtn3 != null)
            {
                ButtonGroupList[5].Chosebtn3.onClick.AddListener((() =>
                {
                    //todo: 선택한것 구분 짓기
                    ButtonGroupList[5].ButtonGroup1.SetActive(false);
                    _gamePrologManager.Func_skipText();

                    StartCoroutine(co_Delaybool());


                    //현재 씬이 Travel2 이면 메인 씬으로 이동
                }));
            }
        }
    }

    public async void EndingEffect()
    {
        _gamePrologManager.isButtonOn = await TweenEffect.EndingEffect(EndingEffectObj);
        _gamePrologManager.TextName = "P122-1";
        _gamePrologManager.Func_skipText();
    }

    public IEnumerator co_Delaybool()
    {
        yield return new WaitForSeconds(0.4f);
        _gamePrologManager.isButtonOn = false;
        
        print(  "_gamePrologManager.isButtonOn = false: "+  _gamePrologManager.isButtonOn);
        yield return new WaitForSeconds(0.5f);
        skipTextBtn.gameObject.SetActive(true);
        
        
        yield return new WaitForSeconds(0.4f);
        _gamePrologManager.isButtonOn = false;
        print("sdasdasdas" +_gamePrologManager.isButtonOn);
        
        skipTextBtn.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        print("sdasdasdas" +_gamePrologManager.isButtonOn);
    }
}