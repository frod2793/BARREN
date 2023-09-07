using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }
    void Start()
    {
        
    }

    private void Button_Init()
    {
        //Todo:버튼에 리스너를 추가하여 함수 연결 
    }


    private void Func_SettingBtn()
    {
        //Todo:설정 프리펩 인스테이트 
    }

    private void Func_NewsBtn()
    {
        //todo:공지사항 프리펩 인스테이트 
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    
}
