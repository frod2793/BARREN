using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Closet_Manager : MonoBehaviour
{
    
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button misuryBtn;
    [SerializeField] private GameObject misuryPopUp;
    [SerializeField] private GameObject closetPopUp;
    // Start is called before the first frame update
    void Start()
    {
        closeBtn.onClick.AddListener(Func_CloseBtn);
        misuryBtn.onClick.AddListener(Func_MisuryBtn);
    }

    private void Func_CloseBtn()
    {
        closetPopUp.gameObject.SetActive(false);
        misuryPopUp.gameObject.SetActive(true);
    }
    
    private void Func_MisuryBtn()
    {
        closetPopUp.gameObject.SetActive(true);
        misuryPopUp.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
