using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

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

    [Header("StageSelect")] private Button gangseo_Btn;

    private bool isEventToggleOn = false; // 상태 변수

    private bool isContinueToggleOn = false; // 상태 변수

    // Start is called before the first frame update
    void Start()
    {
        _manager = gameObject.GetComponent<MainSceneManager>();

        buttons_Init();

        
        

        if (PlayerData.Instance.PlayerName != null)
        {
            playerName.text = PlayerData.Instance.PlayerName;
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
        
        gangseo_Btn.onClick.AddListener(() => {
            //todo: 추후 팝업네에 스테이지 목록 추가 될 예정
            SceneLoader.Instace.LoadScene("GameScene");
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