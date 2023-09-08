using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainTItle_UI_Manager : MonoBehaviour
{
    [Header("버튼 목록")] [SerializeField] private Button newGameBtn;

    [Header(("팝업 목록"))] [SerializeField] private GameObject playerSettingPopUp;

    [Header(("팝업 구성체"))] [SerializeField] private InputField nameInputField;

    // Start is called before the first fram e update
    void Start()
    {
        newGameBtn.onClick.AddListener(Func_NewGameBtn);
    }


    private void Func_NewGameBtn()
    {
        OpenPopup();
    }

    void Update()
    {
    }
    
    public void OpenPopup()
    {
        // 팝업을 활성화하고 스케일링 애니메이션을 적용합니다.
        playerSettingPopUp.SetActive(true);
        playerSettingPopUp.transform.localScale = Vector3.zero; // 시작 스케일을 0으로 설정합니다.

        // 스케일링 애니메이션을 만들고 재생합니다.
        playerSettingPopUp.transform.DOScale(Vector3.one, 0.5f) // 0.5초 동안 스케일을 원래 크기로 키웁니다.
            .SetEase(Ease.OutBack); // 이징(Easing)을 설정하여 애니메이션 효과를 부드럽게 만듭니다.
    }

    public void ClosePopup()
    {
        // 스케일링 애니메이션을 역으로 실행하여 팝업을 닫습니다.
        playerSettingPopUp.transform.DOScale(Vector3.zero, 0.5f) // 0.5초 동안 스케일을 0으로 줄입니다.
            .SetEase(Ease.InBack) // 닫히는 애니메이션의 이징(Easing) 설정.
            .OnComplete(() => playerSettingPopUp.SetActive(false)); // 애니메이션이 완료된 후 팝업을 비활성화합니다.
    }
}