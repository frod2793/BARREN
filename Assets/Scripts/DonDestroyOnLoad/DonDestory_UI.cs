using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DonDestory_UI : MonoBehaviour
{
    private static DonDestory_UI instance;
    
    [Header("- 버튼 목록")]
    [SerializeField] private Button settingButton;
    [SerializeField] private Button newsButton;

    [Header("- 팝업 프리펩  목록")]
    [SerializeField] private GameObject settingPopUp;
    [SerializeField] private GameObject newsPopUp;

    [Header("- 캔버스")] [SerializeField] private GameObject uiCanvas;
    
    void Awake()
    {
        // 이미 인스턴스가 있는 경우, 새 인스턴스를 파괴하고 기존 인스턴스를 유지합니다.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스가 없는 경우, 현재 오브젝트를 인스턴스로 설정하고 파괴하지 않도록 설정합니다.
        instance = this;
        DontDestroyOnLoad(gameObject);
        

        // 씬 전환 이벤트에 대한 리스너 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        Button_Init();
    }

    private void Button_Init()
    {
        //Todo:버튼에 리스너를 추가하여 함수 연결 
        settingButton.onClick.AddListener(Func_SettingBtn);
        
     
    }

    // 씬 전환 이벤트 핸들러
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 씬이 Main 씬인 경우, 현재 게임 오브젝트 파괴
        if (scene.name == "MainScene")
        {
            Destroy(gameObject);
        }
    }
    private void Func_SettingBtn()
    {
        // 로드한 프리펩을 인스턴스화
      //  부모는 현재 게임 오브젝트로 한다 
        GameObject settingInstance = Instantiate(settingPopUp,uiCanvas.transform);

        // 설정 팝업 열기 
        TweenEffect.OpenPopup(settingInstance);
    }

    private void Func_NewsBtn()
    {
        //todo:공지사항 프리펩 인스테이트 
    }
   
}
