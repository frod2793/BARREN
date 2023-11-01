
using UnityEngine;
using UnityEngine.UI;
public class AttendancePopUp : MonoBehaviour
{
  [SerializeField] private Button closeBtn;

  private void Start()
  {
    closeBtn.onClick.AddListener(Func_CloseBtn);
 
  }

  private void Func_CloseBtn()
  {
    TweenEffect.ClosePopup(gameObject);
  }



  
}
