using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MainSceneBtnManager : MonoBehaviour
{
    [Header("메인 메니져")] private MainSceneManager _manager;

    [Header("플레이어 정보")] [SerializeField] private Text playerName;

    [Header("상단 ui")] [SerializeField] private Text coinText;
    [SerializeField] private Text diamondText;
    [SerializeField] private Text datText;
    [SerializeField] private Toggle topButtonGroupToggle;

    [Header("오른쪽 ui")] [SerializeField] private Button attendBtn;
    [SerializeField] private GameObject toggleMark;
    [SerializeField] private Toggle eventToggle;
    [SerializeField] private Toggle continueToggle;

    [Header("왼쪽 UI")] [SerializeField] private Button map_Btn;
    [SerializeField] private Button misson_Btn;
    [SerializeField] private Button shope_Btn;

    [Header("하단 UI")] [SerializeField] private Button friend_Btn;
    [SerializeField] private Button talk_Btn;
    [SerializeField] private Button character_Btn;
    [SerializeField] private Button closet_Btn;
    [SerializeField] private Button inventori_Btn;

    [Header("맵 팝업 내부 지역선택 버튼")] [SerializeField]
    private Button gangseo_Btn;

    [SerializeField] private Button yangcheon_Btn;
    [SerializeField] private Button gurogButton;
    [SerializeField] private Button YeongdeungpoBtn;

    [Header("스테이지 선택 버튼")] [SerializeField]
    private Button firstStageBtn;

    [SerializeField] private Button sceondStageBtn;
    [SerializeField] private Button thirdStageBtn;
    [SerializeField] private Button fourthStageBtn;
    [SerializeField] private Button fifthStageBtn;

    [Header("스테이지 선택 버튼 이미지")] [SerializeField]
    private Sprite unlockStage;

    [SerializeField] private Sprite lockStage;
    [SerializeField] private Sprite clearStage;

    [Header("스테이지 팝업 내부 지역선택 버튼")] [SerializeField]
    private Button startButton;

    [SerializeField] private Button shealterPopUpExit;
    private bool isEventToggleOn = false; // 상태 변수

    private bool isContinueToggleOn = false; // 상태 변수
    public string SceneName;
    [Header("캐릭터 팝업")] [SerializeField] private GameObject chacracterPopup;
    [SerializeField] private Button chacracterPopupCloseBtn;
    [Header("캐릭터 데이터 ")] public CharacterDataList CharacterDataList;

    // Start is called before the first frame update
    void Start()
    {
        _manager = gameObject.GetComponent<MainSceneManager>();


        StageData();

        PlayerData.Instance.SetUnlock(0, true);
        if (PlayerData.Instance.PlayerName != null)
        {
            playerName.text = PlayerData.Instance.PlayerName;
        }

        mainBtnSetting();
        buttons_Init();
    }


    private void StageData()
    {
        for (int i = 0; i < 5; i++)
        {
            UpdateStageButton(i);
        }
    }

    private void UpdateStageButton(int stageIndex)
    {
        Button stageButton = null;
        Sprite unlockSprite = unlockStage;
        Sprite clearSprite = clearStage;

        switch (stageIndex)
        {
            case 0:
                stageButton = firstStageBtn;
                break;
            case 1:
                stageButton = sceondStageBtn;
                if (PlayerData.Instance.IsUnlock(stageIndex))
                {
                    firstStageBtn.image.sprite = clearSprite;
                    firstStageBtn.interactable = false;
                }

                break;
            case 2:
                stageButton = thirdStageBtn;
                if (PlayerData.Instance.IsUnlock(stageIndex))
                {
                    sceondStageBtn.image.sprite = clearSprite;
                    startButton.interactable = false;
                }

                break;
            case 3:
                stageButton = fourthStageBtn;
                if (PlayerData.Instance.IsUnlock(stageIndex))
                {
                    thirdStageBtn.image.sprite = clearSprite;
                    thirdStageBtn.interactable = false;
                }

                break;
            case 4:
                stageButton = fifthStageBtn;
                if (PlayerData.Instance.IsUnlock(stageIndex))
                {
                    fourthStageBtn.image.sprite = clearSprite;
                    fourthStageBtn.interactable = false;
                }

                break;
            default:
                Debug.LogWarning("Invalid stage index: " + stageIndex);
                return;
        }

        if (PlayerData.Instance.IsUnlock(stageIndex))
        {
            stageButton.interactable = true;
            stageButton.image.sprite = unlockSprite;
        }
    }

    private void mainBtnSetting()
    {
        talk_Btn.onClick.AddListener(() => _manager.EnableTalk_Popup());

        topButtonGroupToggle.onValueChanged.AddListener((isOn) =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);

            _manager.OnToggleValueChangedY(isOn, topButtonGroupToggle.gameObject, 796f);
            func_ToggleAction(isOn);
        });

        attendBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.EnableAttendpPopup();
        });
        ToggleInfo eventToggleInfo = new ToggleInfo();
        ToggleInfo continueToggleInfo = new ToggleInfo();

        eventToggle.onValueChanged.AddListener((isOn) =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.OnToggleValueChangedX(isOn, eventToggle.gameObject, -63f, -212f, eventToggleInfo);
        });

        continueToggle.onValueChanged.AddListener((isOn) =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.OnToggleValueChangedX(isOn, continueToggle.gameObject, 117f, -216f, continueToggleInfo);
        });

        map_Btn.onClick.AddListener(() =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.EnableMap_Popup();
        });

        character_Btn.onClick.AddListener(() =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            TweenEffect.OpenPopup(chacracterPopup);
        });

        chacracterPopupCloseBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            TweenEffect.ClosePopup(chacracterPopup);
        });
    }

    private void buttons_Init()
    {
        startButton.interactable = false;

        YeongdeungpoBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            //todo: 추후 팝업네에 스테이지 목록 추가 될 예정
            _manager.yeongDeungPoBtn();
            _manager.player.SetActive(false);
            _manager.CloseEveryPopup();
        });
        firstStageBtn.onClick.AddListener((() =>
        {
            BtnReset();

            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.EnableCharacterProssePopUp1btn();
            PlayerData.Instance.JsonName = "Prol_Day0";
            SceneName = "Prolouge";

            SetCharactersBtn(false, false, true, false, "0", true);

            print("데이터: " + PlayerData.Instance.startbtntext);

            if (PlayerData.Instance.startbtntext == "Day1Start")
            {
                SetCharactersBtn(false, false, true, false, "0", false);

                startButton.interactable = true;
                startButton.onClick.AddListener(() =>
                {
                    SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
                    PlayerData.Instance.JsonName = "Prol_Day1";
                    SceneLoader.Instace.LoadScene(SceneName);
                });
            }
            else
            {
                startButton.interactable = false;
            }
        }));

        sceondStageBtn.onClick.AddListener((() =>
        {
            BtnReset();
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.EnableCharacterProssePopUp1btn();
            SceneName = "Prolouge";


            if (PlayerData.Instance.startbtntext == "EnableGangMin")
            {
                SetCharactersBtn(false, false, true, false, "1", false);

                SetCharactersBtn(false, true, false, false, "1", true);
            }
            else
            {
                SetCharactersBtn(false, false, true, false, "1", true);
            }


            print("데이터: " + PlayerData.Instance.startbtntext);


            if (PlayerData.Instance.startbtntext == "Day2Start")
            {
                SetCharactersBtn(false, false, true, false, "1", false);

                SetCharactersBtn(false, true, false, false, "1", false);
                startButton.interactable = true;
                startButton.onClick.AddListener(() =>
                {
                    SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
                    PlayerData.Instance.JsonName = "Prol_Day2";
                    SceneLoader.Instace.LoadScene(SceneName);
                });
            }
            else
            {
                startButton.interactable = false;
            }
        }));

        thirdStageBtn.onClick.AddListener((() =>
        {
            BtnReset();
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.EnableCharacterProssePopUp1btn();
            SceneName = "Prolouge";


            if (PlayerData.Instance.startbtntext == "EnableCure")
            {
                SetCharactersBtn(false, false, true, false, "2", true);
            }
            else
            {
                SetCharactersBtn(false, true, false, false, "2", true);
            }


            if (PlayerData.Instance.startbtntext == "EnableKid")
            {
                SetCharactersBtn(false, false, true, false, "2", false);
                SetCharactersBtn(false, true, false, false, "2", false);
                SetCharactersBtn(false, false, false, true, "2", true);
            }


            if (PlayerData.Instance.startbtntext == "Day3Start")
            {
                SetCharactersBtn(false, false, true, false, "2", false);
                SetCharactersBtn(false, true, false, false, "2", false);
                SetCharactersBtn(false, false, false, true, "2", false);

                startButton.interactable = true;
                startButton.onClick.AddListener(() =>
                {
                    SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
                    PlayerData.Instance.JsonName = "Prol_Day3";
                    SceneLoader.Instace.LoadScene(SceneName);
                });
            }
            else
            {
                startButton.interactable = false;
            }
        }));

        fourthStageBtn.onClick.AddListener((() =>
        {
            BtnReset();
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.EnableCharacterProssePopUp1btn();
            SceneName = "Prolouge";

       

            if (PlayerData.Instance.startbtntext == "EnableMingi")
            {
                SetCharactersBtn(true, false, false, false, "3", true);
            }
            else
            {
                SetCharactersBtn(false, false, true, false, "3", true);

            }


            if (PlayerData.Instance.startbtntext == "Day4Start")
            {
                SetCharactersBtn(true, false, false, false, "3", false);
                SetCharactersBtn(false, false, true, false, "3", false);

                startButton.interactable = true;
                startButton.onClick.AddListener(() =>
                {
                    SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
                    PlayerData.Instance.JsonName = "Prol_Day4";
                    SceneLoader.Instace.LoadScene(SceneName);
                });
            }
            else
            {
                startButton.interactable = false;
            }
        }));

        fifthStageBtn.onClick.AddListener((() =>
        {
            BtnReset();
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.EnableCharacterProssePopUp1btn();
            SceneName = "Prolouge";

            
                SetCharactersBtn(true, false, false, false, "4", true);
           

            if (PlayerData.Instance.startbtntext == "StartDay5")
            {
                startButton.interactable = true;
                startButton.onClick.AddListener(() =>
                {
                    SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
                    PlayerData.Instance.JsonName = "Prol_Day5";
                    SceneLoader.Instace.LoadScene(SceneName);
                });
            }
            else
            {
                startButton.interactable = false;
            }


            if (PlayerData.Instance.startbtntext == "Day5end")
            {
                //todo 다섯번째이후
                SetCharactersBtn(true, false, false, false, "4", false);

                SetCharactersBtn(false, false, true, false, "5", true);
            }
        
        }));

        shealterPopUpExit.onClick.AddListener((() =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            _manager.disnbleStageSelect_PopUp();
        }));
    }

    private void func_ToggleAction(bool isOn)
    {
        toggleMark.SetActive(!isOn);
    }

    private void SetCharactersBtn(bool isMinGi, bool isGangMin, bool isCure, bool isKid, string day, bool istalk)
    {
        foreach (var character in CharacterDataList.CharacterData_List)
        {
            character.CharacterCard.SetActive(false);
        }

        if (isMinGi) SetupCharacterButton("MinGi", day, "Travel1", istalk);
        if (isGangMin) SetupCharacterButton("GangMin", day, "Travel1", istalk);
        if (isCure) SetupCharacterButton("Cure", day, "Travel1", istalk);
        if (isKid) SetupCharacterButton("Kid", day, "Travel1", istalk);
    }

    private void SetupCharacterButton(string characterName, string day, string sceneName, bool istalk)
    {
        var character = CharacterDataList.CharacterData_List.Find(x => x.CharacterListName == characterName);
        character.CharacterCard.SetActive(true);
        character.CharacterTalkBtn.interactable = istalk;
        character.CharacterTalkBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);

            PlayerData.Instance.JsonName = $"{characterName}_Day{day}";
            SceneLoader.Instace.LoadScene(sceneName);
        });
    }

    private void BtnReset()
    {
        for (int i = 0; i < CharacterDataList.CharacterData_List.Count; i++)
        {
            CharacterDataList.CharacterData_List[i].CharacterTalkBtn.onClick.RemoveAllListeners();
        }
    }
}