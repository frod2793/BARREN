using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Button touchToStartBtn;
    // Start is called before the first frame update
    void Start()
    {
        touchToStartBtn.onClick.AddListener(Func_MainTitleLoad);
        
        SoundManager.Instance.Func_BGMLoop(AudioDefine.Main_Bgm);
    }

    // Update is called once per frame
  

    private void Func_MainTitleLoad()
    {   
        SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.ButtonClick);
        SceneLoader.Instace.LoadScene("MainTitleScene");
    }
    
    
}
