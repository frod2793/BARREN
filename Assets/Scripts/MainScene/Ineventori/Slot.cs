using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text Nametext;
    [SerializeField] Text Numtext;
    private Item _item;
    public Item item {
        get { return _item; }
        set {
            _item = value;
            if (_item != null) {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
                Nametext.text = item.itemName;
                Numtext.text = item.itemNum;
            } else {
                image.color = new Color(1, 1, 1, 0);
                Nametext.text = "";
                Numtext.text = "";
            }
        }
    }
}
