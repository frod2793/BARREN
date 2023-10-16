using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;
    [SerializeField]
    private Button closeBtn;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Button useBtn;
    [SerializeField]
    private Button CancelBtn;
    [SerializeField]
    private GameObject usePopUp;
    
    [SerializeField]
    UsePopUp UsePopUp;
    
    
#if UNITY_EDITOR
    private void OnValidate() {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake() {
        FreshSlot();
        closeBtn.onClick.AddListener(() => {
          TweenEffect.ClosePopup(itemPrefab);
        });
        
        useBtn.onClick.AddListener(() => {
            SelectItem();
            TweenEffect.OpenPopup(usePopUp);
       
        });
        CancelBtn.onClick.AddListener(() => {
            TweenEffect.ClosePopup(usePopUp);
        });
    }

    private void SelectItem()
    {
        // 리스트 초기화
        UsePopUp.items.Clear();

        foreach (Slot itemSlot in slots)
        {
            Toggle[] toggles = itemSlot.GetComponentsInChildren<Toggle>(); // slot의 모든 Toggle 컴포넌트 배열 가져오기

            foreach (Toggle toggle in toggles)
            {
                // Toggle이 체크되어 있다면 해당 slot의 아이템을 리스트에 추가합니다.
                if (toggle.isOn)
                {
                    UsePopUp.items.Add(itemSlot.item);
                }
            }
        }
    }

    public void FreshSlot()
    {
        
        items = PlayerData.Instance.items;
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++) {
            slots[i].item = items[i];
        }
        for (; i < slots.Length; i++) {
            slots[i].item = null;
        }
    }

    public void AddItem(Item _item) {
        if (items.Count < slots.Length) {
            items.Add(_item);
            FreshSlot();
        } else {
            print("슬롯이 가득 차 있습니다.");
        }
    }
}
