
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class Caracterinfo_PopUp : MonoBehaviour
{
    [SerializeField] private Button closeBtn;

    [SerializeField] private Image BackgorundImage;
    [SerializeField] private Sprite[] CharacterImage;

    [SerializeField] private ToggleGroup toggleGroup;
    private void Start()
    {
        closeBtn.onClick.AddListener(Func_CloseBtn);
     
    }

    private void Func_CloseBtn()
    {
        TweenEffect.ClosePopup(gameObject);
    }
    
    public void Func_Toggle(int index)
    {
        var toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (toggle != null)
        {
            BackgorundImage.sprite = CharacterImage[index];
        }
      
    }
}
