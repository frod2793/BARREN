using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class MainSceneBtnManager : MonoBehaviour
{
    [Header("메인 메니져")] 
    private MainSceneManager _manager;
    
    [Header("상단 ui")] 
    [SerializeField] private Text coinText;
    [SerializeField] private Text diamondText;
    [SerializeField] private Text datText;
    [SerializeField] private Toggle topButtonGroupToggle;

    [Header("오른쪽 ui")]
    [SerializeField] private Button attendBtn;

    [SerializeField] private GameObject toggleMark;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _manager = gameObject.GetComponent<MainSceneManager>();
        
        
        topButtonGroupToggle.onValueChanged.AddListener((isOn) =>
        {
            _manager.OnToggleValueChanged(isOn, topButtonGroupToggle.gameObject, 796f);
            func_ToggleAction(isOn);
        }); attendBtn.onClick.AddListener(_manager.EnableAttendpPopup);
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
