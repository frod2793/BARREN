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



    [Header("맵 팝업 내부 지역선택 버튼")] 
    [SerializeField]
    private Button gangseo_Btn;

    [SerializeField] private Button yangcheon_Btn;
    [SerializeField] private Button gurogButton;
    [SerializeField] private Button YeongdeungpoBtn;

    [Header("스테이지 선택 버튼")] 
    
    [SerializeField] private Button firstStageBtn;
    [SerializeField] private Button sceondStageBtn;
    [SerializeField] private Button thirdStageBtn;
    [SerializeField] private Button fourthStageBtn;
    [SerializeField] private Button fifthStageBtn;
    [Header("스테이지 선택 버튼 이미지")]
    [SerializeField] private Sprite unlockStage;
    [SerializeField] private Sprite lockStage;
    [SerializeField] private Sprite clearStage;
    
    [Header("스테이지 팝업 내부 지역선택 버튼")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button shealterPopUpExit;
    private bool isEventToggleOn = false; // 상태 변수

    private bool isContinueToggleOn = false; // 상태 변수
public string SceneName;
    // Start is called before the first frame update
    void Start()
    {
        _manager = gameObject.GetComponent<MainSceneManager>();

        buttons_Init();


        stageData();

        PlayerData.Instance.SetUnlock(0,true);
        if (PlayerData.Instance.PlayerName != null)
        {
            playerName.text = PlayerData.Instance.PlayerName;
        }
    }

    private void stageData()
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
                }
                break;
            case 2:
                stageButton = thirdStageBtn;
                if (PlayerData.Instance.IsUnlock(stageIndex))
                {
                    sceondStageBtn.image.sprite = clearSprite;
                }
                break;
            case 3:
                stageButton = fourthStageBtn;
                if (PlayerData.Instance.IsUnlock(stageIndex))
                {
                    thirdStageBtn.image.sprite = clearSprite;
                }
                break;
            case 4:
                stageButton = fifthStageBtn;
                if (PlayerData.Instance.IsUnlock(stageIndex))
                {
                    fourthStageBtn.image.sprite = clearSprite;
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
    private void buttons_Init()
    {
        
        topButtonGroupToggle.onValueChanged.AddListener((isOn) =>
        {
            _manager.OnToggleValueChangedY(isOn, topButtonGroupToggle.gameObject, 796f);
            func_ToggleAction(isOn);
        });
        attendBtn.onClick.AddListener(_manager.EnableAttendpPopup);

        ToggleInfo eventToggleInfo = new ToggleInfo();
        ToggleInfo continueToggleInfo = new ToggleInfo();

        eventToggle.onValueChanged.AddListener((isOn) =>
        {
            _manager.OnToggleValueChangedX(isOn, eventToggle.gameObject, -63f, -212f, eventToggleInfo);
        });

        continueToggle.onValueChanged.AddListener((isOn) =>
        {
            _manager.OnToggleValueChangedX(isOn, continueToggle.gameObject, 117f, -216f, continueToggleInfo);
        });
        
        map_Btn.onClick.AddListener(() =>
        {
            _manager.EnableMap_Popup();
        });
        
        YeongdeungpoBtn.onClick.AddListener(() => {
            //todo: 추후 팝업네에 스테이지 목록 추가 될 예정
            _manager.yeongDeungPoBtn();
            _manager.player.SetActive(false);
            _manager.CloseEveryPopup();

        });
        firstStageBtn.onClick.AddListener((() =>
        {
            _manager.EnableCharacterProssePopUp();
            SceneName= "Travel1";
        }));
        
        sceondStageBtn.onClick.AddListener((() =>
        {
            _manager.EnableCharacterProssePopUp();
            SceneName = "Travel2";
        }));
        
        thirdStageBtn.onClick.AddListener((() =>
        {
            _manager.EnableCharacterProssePopUp();
            SceneName = "Travel3";
        }));
        
        fourthStageBtn.onClick.AddListener((() =>
        {
            _manager.EnableCharacterProssePopUp();
            SceneName = "Travel4";
        }));
        
        fifthStageBtn.onClick.AddListener((() =>
        {
            _manager.EnableCharacterProssePopUp();
            SceneName = "Travel5";
        }));
        
        shealterPopUpExit.onClick.AddListener(_manager.disnbleStageSelect_PopUp);
     
        startButton.onClick.AddListener(() =>
        {
            SceneLoader.Instace.LoadScene(SceneName);
        });
    }
    
    private void func_ToggleAction(bool isOn)
    {
        toggleMark.SetActive(!isOn);
    }

    // Update is called once per frame
    void Update()
    {
    }
}