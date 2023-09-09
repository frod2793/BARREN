using UnityEngine;
using UnityEngine.UI;

public class MainTItle_UI_Manager : MonoBehaviour
{
    [Header("버튼 목록")] [SerializeField] private Button newGameBtn;

    [Header(("팝업 목록"))] [SerializeField] private GameObject playerSettingPopUp;

    [Header(("팝업 구성체"))] [SerializeField] private InputField nameInputField;

    [Header("결정 버튼")] [SerializeField] private Button comFromBtn;
    // Start is called before the first fram e update
    void Start()
    {
        newGameBtn.onClick.AddListener(Func_NewGameBtn);
        comFromBtn.onClick.AddListener(Func_ComfromBtn);
    }


    private void Func_NewGameBtn()
    {
        TweenEffect.OpenPopup(playerSettingPopUp);    
    }

    private void Func_ComfromBtn()
    {
        
        SceneLoader.Instace.LoadScene("MainScene");
        //todo: 추후 인풋 필드 데이터를 json 이던 싱글턴 이던 저장 후 사용 성별정보도 포함
    }

    void Update()
    {
    }
}