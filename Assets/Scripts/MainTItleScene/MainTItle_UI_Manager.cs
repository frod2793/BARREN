using UnityEngine;
using UnityEngine.UI;

public class MainTItle_UI_Manager : MonoBehaviour
{
    [Header("버튼 목록")] 
    [SerializeField] private Button newGameBtn;

    [Header(("팝업 목록"))] 
    [SerializeField] private GameObject playerSettingPopUp;

    [Header(("팝업 구성체"))] 
    [SerializeField] private InputField nameInputField;

    [SerializeField] private GameObject rejected_nickname;
    [SerializeField] private GameObject available_nickname;

    [SerializeField] private Toggle maleToggle;
    [SerializeField] private Toggle fmaleToggle;
    
    [Header("결정 버튼")] 
    [SerializeField] private Button comFromBtn;
    // Start is called before the first fram e update
    void Start()
    {
        newGameBtn.onClick.AddListener(Func_NewGameBtn);
        comFromBtn.onClick.AddListener(Func_ComfromBtn);
        nameInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        comFromBtn.interactable = false;
    }


    private void Func_NewGameBtn()
    {
        TweenEffect.OpenPopup(playerSettingPopUp);    
        
        SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
    }

    private void Func_ComfromBtn()
    {
        PlayerData.Instance.PlayerName = nameInputField.text;
        
        if (maleToggle)
        {
            
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            PlayerData.Instance.Gender = "male";
        }
        else
        { 
            
            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
            PlayerData.Instance.Gender = "fmale";
        }
   
        
        SceneLoader.Instace.LoadScene("MainScene");
        //todo: 추후 인풋 필드 데이터를 json 이던 싱글턴 이던 저장 후 사용 성별정보도 포함
    }
    private void OnInputFieldValueChanged(string newValue)
    {
        if (newValue.Length > 8)
        {
            // 글자 수가 8을 넘으면 특정 게임 오브젝트 활성화
            rejected_nickname.SetActive(true);
            available_nickname.SetActive(false);
            comFromBtn.interactable = false;

        }
        else  if (newValue.Length > 1 && newValue.Length <= 8)
        {
            // 글자 수가 8을 넘지 않으면 특정 게임 오브젝트 비활성화
            rejected_nickname.SetActive(false);
            available_nickname.SetActive(true);
            comFromBtn.interactable = true;
        }
        else
        {          
            available_nickname.SetActive(false);
            comFromBtn.interactable = false;
        }
    }
}