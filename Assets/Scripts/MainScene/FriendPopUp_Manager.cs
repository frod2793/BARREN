using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FriendPopUp_Manager : MonoBehaviour
{
    [SerializeField] private Button addfriendBtn;
    [SerializeField] private Button FriendBtn;
    [FormerlySerializedAs("misuryPopUp")] [SerializeField] private GameObject addfriendPopUp;
    [FormerlySerializedAs("closetPopUp")] [SerializeField] private GameObject friendpopup;
    // Start is called before the first frame update
    void Start()
    {
        addfriendBtn.onClick.AddListener(Func_CloseBtn);
        FriendBtn.onClick.AddListener(Func_MisuryBtn);
    }

    private void Func_CloseBtn()
    {
        friendpopup.gameObject.SetActive(false);
        addfriendPopUp.gameObject.SetActive(true);
    }
    
    private void Func_MisuryBtn()
    {
        friendpopup.gameObject.SetActive(true);
        addfriendPopUp.gameObject.SetActive(false);
    }
}
