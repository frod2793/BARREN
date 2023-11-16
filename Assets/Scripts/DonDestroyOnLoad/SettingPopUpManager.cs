using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUpManager : MonoBehaviour
{
    [Header("닫기 버튼")] [SerializeField] private Button closeButton;

    [Header("배경음 슬라이더")] [SerializeField] private Slider bgmSlider;

    [Header("효과음 슬라이더")] [SerializeField] private Slider sfxSlider;


    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        _soundManager = SoundManager.Instance;
        closeButton.onClick.AddListener(() => func_closeButton().Forget());
        bgmSlider.onValueChanged.AddListener(func_bgmVolume);
        sfxSlider.onValueChanged.AddListener(func_sfxVolume);

        bgmSlider.value = _soundManager.bgm.volume;
        sfxSlider.value = _soundManager.effect.volume;
    }

    private async UniTask func_closeButton()
    {
        TweenEffect.ClosePopup(gameObject);
        await UniTask.WaitUntil(() => gameObject.activeSelf == false);
        Destroy(this.gameObject);
    }

    private void func_bgmVolume(float volume)
    {
        _soundManager.bgm.volume = volume;
    }

    private void func_sfxVolume(float volume)
    {
        _soundManager.effect.volume = volume;
    }

    // Update is called once per frame
    void Update()
    { if (Application.platform== RuntimePlatform.Android)
        {
           
                if (Input.GetKey(KeyCode.Escape))
                {
                    func_closeButton().Forget();
                }
            
        }
    }
}