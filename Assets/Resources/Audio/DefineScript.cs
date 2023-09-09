using UnityEngine;

    #region 열거형

    /// <summary>
    /// <para> 내    용 : Fade 열거형</para>
    /// </summary>
    public enum FADE
    {
        In, Out
    }

    /// <summary>
    /// <para> 내    용 : 씬의 종류 </para>
    /// </summary>
    public enum SCENE_KIND
    {
        Intro, Play
    }

    /// <summary>
    /// <para> 내    용 : 플레이어 컨트롤러의 종류 </para>
    /// </summary>
    public enum PLAYER_CONTROLLER
    {
        Pointer, Grab
    }

    /// <summary>
    /// <para> 내    용 : NPC 자동차의 상태 종류 </para>
    /// </summary>
    public enum NPC_CAR_STATE
    {
        Move, Detect, Stop
    }

    #endregion

    #region 구조체

    /// <summary>
    /// <para> 내    용 : 사운드 볼륨 구조체 </para>
    /// </summary>
    [System.Serializable]
    public struct Volume
    {
        /// <summary>
        /// BGM 볼륨
        /// </summary>
        [Range(0f, 1f)]
        public float volume_BGM;

        /// <summary>
        /// Effect 볼륨
        /// </summary>
        [Range(0f, 1f)]
        public float volume_Effect;

        /// <summary>
        /// 나레이션 볼륨
        /// </summary>
        [Range(0f, 1f)]
        public float volume_Narration;
    }

    /// <summary>
    /// <para> 내    용 : 방해받지 않게 얼마나 줄여야 하는지에 대한 구조체 </para>
    /// </summary>
    [System.Serializable]
    public struct Interference
    {
        [Range(0, 1)] public float bgm;
        [Range(0, 1)] public float effect;
    }

    /// <summary>
    /// <para> 내    용 : 씬 이름에 대한 구조체 </para>
    /// </summary>
    public struct DefineSceneName
    {
        public const string IntroScene = "#01.Intro";
        public const string LoadingScene = "#00.Loading";
        public const string PlayScene = "#02.Play";
    }


    /// <summary>
    /// <para> 내    용 : 나레이션 키에 대한 구조체 </para>
    /// </summary>
    public struct NarrationDefine
    {
        // ****************** 일반 나레이션 ******************
        public const string PDD_0 = "A-01";
        public const string PDD_1 = "A-02";
        public const string PDD_2 = "A-03";
        public const string PDD_3 = "A-04";
        public const string PDD_4 = "B-01";
        public const string learn01_01 = "B-02";
        public const string learn02_01 = "C-01";
        public const string learn02_02 = "C-02";
        public const string learn02_03 = "C-03";
        public const string learn02_04 = "C-04";
        public const string learn03_01 = "D-01";
        public const string learn03_02 = "D-02";
        public const string learn03_03 = "D-03";
        public const string learn04_01 = "E-01";
        public const string learn05_01 = "F-01";
        public const string learn06_01 = "G-01";
        public const string learn06_02 = "G-02";
        public const string learn07_01 = "H-01";
        public const string learn07_02 = "H-02";
        // ****************** 히든 나레이션 ******************
    }


    #endregion

    #region 경로

    /// <summary>
    /// <para> 내    용 : 프로그램의 오디오 경로를 관리하는 구조체 </para>
    /// </summary>
    public struct AudioDefine
    {
        // **************************************************************************
        // Audio Path
        // **************************************************************************
        public const string Audio_BGMPath = "Audio/BGM";
        public const string Audio_EffectPath = "Audio/Effect";
        public const string Audio_NarrationPath = "Audio/Narration";

        #region Public
        // **************************************************************************
        // public
        // **************************************************************************
        public const string UpScore = "Public Confirm Score";
        public const string AddScore = "Public Adding Score";
        public const string TimerWarning = "Public Timer Warning";
        #endregion

        #region Heap
        // ******************************************************************************************************
        // BGM
        // ******************************************************************************************************
        public const string Heap_Audio_BGM = "Heap_BGM";
        public const string Heap_Audio_BGM_Intro = "Heap_BGM_Intro";
        public const string Heap_Audio_BGM_Play = "Heap_BGM_Play";

        // ******************************************************************************************************
        // Effect
        // ******************************************************************************************************
        public const string Heap_Audio_EffectButtonClick = "Heap_ButtonClick";
        public const string Heap_Audio_TopSound = "Heap_Top";
        public const string Heap_Audio_CrowHowl = "Heap_Crow";
        #endregion

        #region Found
        // ******************************************************************************************************
        // BGM
        // ******************************************************************************************************
        public const string Found_Audio_BGM = "Found_BGM";
        public const string Found_Audio_BGM_Intro = "Found_BGM_Intro";
        public const string Found_Audio_BGM_Play = "Found_BGM_Play";
        public const string TakeYongSanE_BGM = "TakeYongSanBGM";
        public const string ProtectYongSanE_BGM = "ProtectYongSanE_BGM";
        public const string Lobby_BGM = "Lobby_BGM";

        // ******************************************************************************************************
        // Effect
        // ******************************************************************************************************
        public const string Virgo = "Virgo";
        public const string Bootes = "Bootes";
        public const string Leo = "Leo";
        public const string Aquila = "Aquila";
        public const string Cygnus = "Cygnus";
        public const string Lyra = "Lyra";
        public const string Pisces = "Pisces";
        public const string Andromeda = "Andromeda";
        public const string Pegasus = "Pegasus";
        public const string Orion = "Orion";
        public const string CanisMajor = "CanisMajor";
        public const string Gemini = "Gemini";
        public const string Satumn = "Satumn";
        public const string Planet = "Planet";
        public const string YonSanBGM = "YonSanBGM";
        public const string YonSan = "YonSan";
        public const string ButtonClick = "ButtonClick";
        public const string takeYongSanE_TakeMeto = "TakeMeto";
        public const string takeTongSanE_TakeYongsane = "TakeYongsan";
        public const string ProtectYongSanE_PopSound = "ProtectYongSanE_PopSound";
        #endregion

        #region Stepping
        // ******************************************************************************************************
        // BGM
        // ******************************************************************************************************
        public const string Step_Bgm = "Stepping_BGM";

        // ******************************************************************************************************
        // Effect
        // ******************************************************************************************************
        public const string Step_Boom = "Boom";
        public const string Step_CrashGlass = "CrashGlass";
        public const string Step_PlayerLanding = "PlayerLanding";
        public const string Step_Falling = "Falling";
        public const string Step_Success = "Success";
        public const string Step_Failed = "Failed";

        #endregion

        #region Chef
        // BGM
        public const string Chef_BGM = "Chef_BGM";
        
        // Effect
        public const string Chef_Angry = "Chef_Angry";
        public const string Chef_Happy = "Chef_Happy";
        public const string Chef_HoldOff = "Chef_HoldOff";
        public const string Chef_Take = "Chef_Take";
    #endregion

    #region Dance
        public const string Dance_BGM = "Dance_BGM";
        public const string Dance_Music = "Dance_Dance";
        public const string Dance_Shutter = "Dance_Shutter";
        public const string Dance_Picture = "Dance_Picture";
    #endregion

    #region TakBR
    public const string TakBR_BGM = "BR_BGM";
    public const string TakBR_Boom = "Tak_Boom";
    public const string TakBR_OnePanCrash = "OnePanCrash";
    #endregion
}
#endregion