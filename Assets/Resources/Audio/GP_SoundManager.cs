using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> 내    용 : 프로그램의 사운드를 관리하는 클래스 </para>
/// </summary>
public class GP_SoundManager : GP_Singleton<GP_SoundManager>
{
    [Header("- 오디오소스")] public AudioSource bgm;
    public AudioSource effect;
    public AudioSource narration;

    [Header("- 기본 볼륨")] public GP_Volume soundVolume;

    [Header("- 오디오 클립을 키값으로 저장")]
    private Dictionary<string, AudioClip> clipContaniner = new Dictionary<string, AudioClip>();

    [Header("- Value")] [Tooltip("나레이션을 재생할 때 다른 소리들이 방해하지 않도록 볼륨을 줄임")]
    public bool noInterference;

    [SerializeField] private GP_Interference interferenceVolume; // 볼륨을 줄일 양

    private Action noAction; // 나레이션 후 호출할 액션변수

    /// <summary>
    /// <para> 내    용 : 스크립트를 생성하면 오디오소스를 생성하는 메서드</para>
    /// </summary>
    private void Reset()
    {
        bgm = gameObject.AddComponent<AudioSource>();
        effect = gameObject.AddComponent<AudioSource>();
        narration = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        Func_SoundInit(); // 사운드 초기화
        Func_GetVolume(); // 볼륨 설정
        //Func_GetInterference(); // 방해여부 설정
    }

    #region 초기 설정

    /// <summary>
    /// <para> 내    용 : 초기 설정 메서드</para>
    /// </summary>
    protected override void Func_Init()
    {
        base.Func_Init();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// <para> 내    용 : 초기 볼륨 설정 메서드</para>
    /// </summary>
    private void Func_GetVolume()
    {
        // 기본 오디오 볼륨 가져오기
        //soundVolume = GP_ScriptableOBJManager.Instance.Func_GetScriptable<GP_ProgramInfo_Setting>().audioVolume;

        bgm.volume = soundVolume.volume_BGM;
        effect.volume = soundVolume.volume_Effect;
        narration.volume = soundVolume.volume_Narration;
    }

    /// <summary>
    /// <para> 내    용 : 초기 오디오 방해 여부 체크 메서드</para>
    /// </summary>
    private void Func_GetInterference()
    {
        //noInterference = GP_ScriptableOBJManager.Instance.Func_GetScriptable<GP_ProgramInfo_Setting>().noInterference;
        //interferenceVolume = GP_ScriptableOBJManager.Instance.Func_GetScriptable<GP_ProgramInfo_Setting>().interference;
    }

    /// <summary>
    /// <para> 내    용 : 초기 볼륨 설정 메서드</para>
    /// </summary>
    private void Func_SoundInit()
    {
        clipContaniner.Clear(); // 오디오 클립 딕셔너리 초기화
        Func_ClipLoad(); // 클립 저장
    }

    /// <summary>
    /// <para> 내    용 : BGM, 효과음, 나레이션 오디오 클립들을 모두 저장하는 메서드</para>
    /// </summary>
    private void Func_ClipLoad()
    {
        Func_LoadAllAudioClipFromPath(GP_AudioDefine.Audio_BGMPath);
        Func_LoadAllAudioClipFromPath(GP_AudioDefine.Audio_EffectPath);
        Func_LoadAllAudioClipFromPath(GP_AudioDefine.Audio_NarrationPath);
    }

    /// <summary>
    /// <para> 내    용 : 정해진 경로에 있는 오디오 클립들을 모두 저장하는 메서드</para>
    /// </summary>
    private void Func_LoadAllAudioClipFromPath(string _folderPath)
    {
        UnityEngine.Object[] audioClipContainer = Resources.LoadAll(_folderPath);

        if (audioClipContainer == null)
        {
            return;
        }

        for (int i = 0; i < audioClipContainer.Length; i++)
        {
            if (clipContaniner.ContainsKey(audioClipContainer[i].name))
            {
                clipContaniner.Remove(audioClipContainer[i].name);
            }

            clipContaniner.Add(audioClipContainer[i].name, (AudioClip)audioClipContainer[i]);
        }
    }

    #endregion

    #region 오디오 클립 건네주기

    /// <summary>
    /// <para> 내    용 : 오디오 클립의 이름을 넣어서 해당하는 오디오 클립을 건네주는 기능 </para>
    /// </summary>
    public AudioClip Func_GetAudioClip(string _clipName)
    {
        return clipContaniner[_clipName];
    }

    #endregion

    #region Loop 메서드

    /// <summary>
    /// <para> 내    용 : 해당하는 이름의 오디오를 BGM으로 루프시키는 메서드</para>
    /// </summary>
    public void Func_BGMLoop(string _name)
    {
        bgm.clip = (clipContaniner[_name]);
        bgm.loop = true;
        bgm.Play();
    }

    /// <summary>
    /// <para> 내    용 : 해당하는 이름의 오디오를 Effect로 루프시키는 메서드</para>
    /// </summary>
    public void Func_EffectLoop(string _name)
    {
        effect.clip = (clipContaniner[_name]);
        effect.loop = true;
        effect.Play();
    }

    #endregion

    #region PlayOneShot 메서드

    /// <summary>
    /// <para> 내    용 : 해당하는 이름의 오디오를 BGM 소스에서 한번만 재생시키는 메서드</para>
    /// </summary>
    public void Func_BGMPlayOneShot(string _name)
    {
        bgm.PlayOneShot(clipContaniner[_name]);
    }

    /// <summary>
    /// <para> 내    용 : 해당하는 이름의 오디오를 Effect 소스에서 한번만 재생시키는 메서드</para>
    /// </summary>
    public void Func_EffectPlayOneShot(string _name)
    {
        effect.PlayOneShot(clipContaniner[_name]);
    }

    /// <summary>
    /// <para> 내    용 : 해당하는 이름의 오디오를 Narration 소스에서 한번만 재생시키는 메서드</para>
    /// </summary>
    public void Func_NarrationPlayOneShot(string _name)
    {
        narration.PlayOneShot(clipContaniner[_name]);
    }

    /// <summary>
    /// <para> 내    용 : 해당하는 이름의 오디오를 Effect 소스에서 얼마의 시간 후에 한번만 재생시키는 메서드</para>
    /// </summary>
    public void Func_EffectDelayOneShot(string _name, float _time)
    {
        StartCoroutine(Co_EffectDelayOneShot(_name, _time));
    }

    /// <summary>
    /// <para> 내    용 : 해당하는 이름의 오디오를 Effect 소스에서 얼마의 시간 후에 한번만 재생시키는 코루틴</para>
    /// </summary>
    IEnumerator Co_EffectDelayOneShot(string _name, float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        effect.PlayOneShot(clipContaniner[_name]);
    }

    #endregion

    #region Play 메서드

    public void Func_NarrationPlay(string _name, Action _action = null)
    {
        if (narration.isPlaying)
        {
            narration.Stop();
        }

        narration.clip = clipContaniner[_name];
        narration.Play();

        if (noInterference)
        {
            StartCoroutine(Co_NarationInterference());
        }

        if (_action != null)
        {
            StartCoroutine(Co_PlayActionAfterVoice(_action));
        }
    }

    /// <summary>
    /// <para> 내    용 : 나레이션이 재생되는 동안 방해받지않도록 다른 볼륨을 줄였다가 끝나면 다시 원상태로 돌리는 코루틴 </para>
    /// </summary>
    /// <returns></returns>
    private IEnumerator Co_NarationInterference()
    {
        WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        bgm.volume = soundVolume.volume_BGM * interferenceVolume.bgm;
        effect.volume = soundVolume.volume_Effect * interferenceVolume.effect;

        while (narration.isPlaying)
        {
            yield return _waitForFixedUpdate;
        }

        bgm.volume = soundVolume.volume_BGM;
        effect.volume = soundVolume.volume_Effect;
    }

    /// <summary>
    /// <para> 내    용 : 나레이션 재생 후 액션이 있다면 실행해 주는 코루틴 </para>
    /// </summary>
    IEnumerator Co_PlayActionAfterVoice(Action _action = null)
    {
        noAction = _action;
        WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        while (narration.isPlaying)
        {
            yield return _waitForFixedUpdate;
        }

        if (noAction != null)
        {
            noAction.Invoke();
        }
    }

    /// <summary>
    /// <para> 내    용 : 실행해야 할 액션을 삭제해주는 메서드 </para>
    /// </summary>
    public void Func_DeleteAction()
    {
        noAction = null;
    }

    #endregion

    #region 일시정지 메서드

    /// <summary>
    /// <para> 내    용 : BGM을 일시정지 시키는 메서드</para>
    /// </summary>
    public void Func_BGMPause()
    {
        bgm.Pause();
    }

    /// <summary>
    /// <para> 내    용 : Effect를 일시정지 시키는 메서드</para>
    /// </summary>
    public void Func_EffectPause()
    {
        effect.Pause();
    }

    /// <summary>
    /// <para> 내    용 : 나레이션을 일시정지 시키는 메서드</para>
    /// </summary>
    public void Func_NarrationPause()
    {
        effect.Pause();
    }

    #endregion

    #region 일시정지 해제

    /// <summary>
    /// <para> 내    용 : BGM 일시정지 해제 메서드 </para>
    /// </summary>
    public void Func_BGMUnPause()
    {
        bgm.UnPause();
    }

    /// <summary>
    /// <para> 내    용 : 효과음 일시정지 해제 메서드 </para>
    /// </summary>
    public void Func_EffectUnPause()
    {
        effect.UnPause();
    }

    /// <summary>
    /// <para> 내    용 : 나레이션 일시정지 해제 메서드 </para>
    /// </summary>
    public void Func_NarrationUnPause()
    {
        narration.UnPause();
    }

    #endregion

    #region 정지 메서드

    /// <summary>
    /// <para> 내    용 : BGM 정지 메서드 </para>
    /// </summary>
    public void Func_BGMStop()
    {
        bgm.loop = false;
        bgm.Stop();
    }

    /// <summary>
    /// <para> 내    용 : 효과음 정지 메서드 </para>
    /// </summary>
    public void Func_EffectStop()
    {
        effect.Stop();
    }

    /// <summary>
    /// <para> 내    용 : 나레이션 정지 메서드 </para>
    /// </summary>
    public void Func_NarrationStop()
    {
        narration.Stop();
    }

    #endregion

    #region 볼륨을 점점 작게

    /// <summary>
    /// <para> 내    용 : BGM 볼륨을 주어진 시간동안 점점 작게 </para>
    /// </summary>
    public void Func_VolumeDecrescendo(float _time)
    {
        bgm.DOFade(0f, _time);
        effect.DOFade(0f, _time);
        narration.DOFade(0f, _time).OnComplete(() =>
        {
            bgm.Stop();
            effect.Stop();
            narration.Stop();
            bgm.volume = soundVolume.volume_BGM;
            effect.volume = soundVolume.volume_Effect;
            narration.volume = soundVolume.volume_Narration;
        });
    }

    public void Func_StopEffectPlay(string _name)
    {
        effect.clip = clipContaniner[_name];
        effect.Play();
        
        if (effect.volume <= 0)
            effect.volume = 1f;
    }

    /// <summary>
    /// Effect 사운드만 점점 줄어듦
    /// </summary>
    /// <param name="_time"></param>
    public void Func_EffectDecrescendo(float _time)
    {
        effect.DOFade(0f, _time).OnComplete(()=>effect.Stop());
    }

    #endregion

    #region 음소거 메서드

    /// <summary>
    /// <para> 내    용 : 매개변수에 따라 소리 켜고 끔 </para>
    /// </summary>
    public void Func_VolumeMuteOnOFF(bool _isMute)
    {
        bgm.mute = _isMute;
        effect.mute = _isMute;
        narration.mute = _isMute;
    }

    #endregion
}