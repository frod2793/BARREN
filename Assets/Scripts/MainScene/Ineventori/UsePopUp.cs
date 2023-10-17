using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UsePopUp : MonoBehaviour,IobjectItem
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
    private Button itemUseBtn;
    
    [SerializeField]
    private Button itemcancelBtn;
    [SerializeField]
    private Inventory Inventory;
    
    public delegate void Game_PrologManager();
    public static event Game_PrologManager OnPrologManagerAction;
    public static event Game_PrologManager justCloseAction;
    // Start is called before the first frame update
#if UNITY_EDITOR
    private void OnValidate() {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake() {;
        closeBtn.onClick.AddListener(() => {
            TweenEffect.ClosePopup(itemUseobj);
            items.Clear();
            FreshSlot();
            justCloseAction?.Invoke();
        });
        itemcancelBtn.onClick.AddListener(() => {
            TweenEffect.ClosePopup(itemUseobj);
            items.Clear();
            FreshSlot();
            justCloseAction?.Invoke();
        });
        
        itemUseBtn.onClick.AddListener((() =>
        { 
            TweenEffect.ClosePopup(itemUseobj);
            UseItem();
            Inventory.FreshSlot();
            Inventory.Closeinventoy();
            FreshSlot();
        }));


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


    public void UseItem()
    {
        // 선택된 아이템들을 PlayerData.Instance.items에서 삭제
        foreach (Item selectedItem in items)
        {
            PlayerData.Instance.items.Remove(selectedItem);
        }

        // 선택된 아이템들을 UsePopUp.items에서 제거
       items.Clear();
       FreshSlot();

       
           OnPrologManagerAction?.Invoke();
      
      
       
    }
}
