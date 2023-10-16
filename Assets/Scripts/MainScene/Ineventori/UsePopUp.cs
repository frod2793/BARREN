using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UsePopUp : MonoBehaviour
{ 
    public List<Item> items; [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;
    [SerializeField]
    private Button closeBtn;
    [SerializeField]
    private GameObject itemUseobj;
    
    
    
    [SerializeField]
    private ToggleGroup toggleGroup;
    // Start is called before the first frame update
#if UNITY_EDITOR
    private void OnValidate() {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake() {
       
        closeBtn.onClick.AddListener(() => {
            TweenEffect.ClosePopup(itemUseobj);
            items.Clear();
        });
    }

    private void OnEnable()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {

        int i = 0;
        for (; i < items.Count && i < slots.Length; i++) {
            slots[i].item = items[i];
            slots[i].gameObject.SetActive(true);
        }
        for (; i < slots.Length; i++) {
            slots[i].item = null;
            slots[i].gameObject.SetActive(false);
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
