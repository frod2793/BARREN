using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_PrologManager : MonoBehaviour
{
    //이미지와 이미지의 이름을 갖고있는 구조체생성
    [Serializable]
    public struct BackGround
    {
        public string name;
        public Sprite image;
    }

    [Serializable]
    public struct Tutorial
    {
        public string name;
        public GameObject TutorialObj;
        public UnityEngine.Events.UnityEvent TutorialEvent;
    }

    [Serializable]
    public struct Character
    {
        public string name;
        public GameObject Panicobj;
        public GameObject Normalobj;
        public GameObject Shadowobj;
        public GameObject Smileobj;
        public GameObject Phsyconicobj;
        public GameObject Lovelyobj;
    }

    //BackGround 리스트 생성
    public List<BackGround> backGroundList = new List<BackGround>();
    [Header("캐릭터 그룹")] public List<Character> charactersList = new List<Character>();
    [Header("튜토리얼 그룹")] public List<Tutorial> tutorialGroupList = new List<Tutorial>();

    [Header("프롤로그 캔버스 부분 ")] [SerializeField]
    private CanvasGroup prologueCanvas;

    [SerializeField] private Image Background;
    [SerializeField] private TextMeshProUGUI topTextMeshPro;
    [SerializeField] private TextMeshProUGUI middleTextMeshPro;
    [SerializeField] private TextMeshProUGUI lowTextMeshProUGUI;

    [Header("플레이어 상호작용 부분 ")] [SerializeField]
    private TextMeshProUGUI PlayerTextBox;

    [SerializeField] GameObject PlayerTextBoxObj;
    [SerializeField] Image PlayerTextBoxImage;
    [SerializeField] private TextMeshProUGUI PlayerNameBox;
    [SerializeField] private Text loctionText;


    [Header("버튼 레이아웃")] public string Chosebtn1Text;
    public string Chosebtn2Text;
    public string Chosebtn3Text;

    [Header("플레이어 캐릭터 상태")] [SerializeField]
    private GameObject Player_Panic;

    [SerializeField] private GameObject Player_Nomaral;
    [SerializeField] private GameObject Player_Smile;
    [SerializeField] private GameObject Player_phsyconic;

    [Header("쉘터지도자 캐릭터 상태")] [SerializeField]
    private GameObject ShealtherLeader_shadow;

    [Header("강유리 캐릭터 상태")] [SerializeField]
    private GameObject Gangyuri_shadow;

    [SerializeField] private GameObject Gangyuri_Normal;

    [Header(" 캐릭터 리스트")] [SerializeField] private GameObject CharactersList;

    [Header("게임씬")] [SerializeField] private GameObject gamescene;
    [SerializeField] private GameObject MainPlayer;
    [SerializeField] private GameObject AttendPopUp;
    private List<LoadJson.Dialogue> dialogueList = new List<LoadJson.Dialogue>();
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isFadingOutInProgress = false; // 페이드 아웃 진행 여부
    private int playerdialogcount;
    private string currentText = "";

    public float WaitTimeInSeconds = 2.0f; // 첫 번째 대사 표시 후 대기 시간(초)
    public float TypingSpeed = 0.05f; // 타이핑 속도 조절
    public float FadeOutSpeed = 0.5f; // 페이드 아웃 속도

    private bool cancelFadeOut = false; // 페이드 아웃 취소 플래그
    bool isskipbutton;
    public string jsonFileName;

    public bool isEnd;
    private bool[] isChapterClear;

    public bool isButtonOn;
    private bool isSound;
    private bool IsActiveEvent;
    private bool isCliled;


    public string TextName = "Start1";

    [Header(" GAmemanager")] [SerializeField]
    private GameManager _gameManager;

    void Start()
    {
        isChapterClear = new bool[6];
        
        isChapterClear[0] = true;
        if (PlayerData.Instance.JsonName != null)
        {
            jsonFileName = PlayerData.Instance.JsonName;
            print(jsonFileName);
        }

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            jsonFileName = "yongdeangpo";
        }

        if (jsonFileName == "yongdeangpo")
        {
            PlayerData.Instance.IsTutorial = true;
        }
        else
        {
            if (SoundManager.Instance != null)
            {
                if (SceneManager.GetActiveScene().name != "MainScene")
                {
                    SoundManager.Instance.Func_BGMLoop(AudioDefine.Tamsa_Bgm);
                }
            }
        }

        DataStageTrangister();


        dialogueList = LoadJson.LoadScriptFromJSON(jsonFileName);


        ShowNextDialogueAsyncActiv().Forget();
    }

    private void DataStageTrangister()
    {
        if (jsonFileName == "Prol_Day1")
        {
            PlayerData.Instance.SetUnlock(1, true);

            PlayerData.Instance.isDay[1] = true;
        }

        if (jsonFileName == "Prol_Day2")
        {
            PlayerData.Instance.SetUnlock(2, true);
        }

        if (jsonFileName == "Prol_Day3")
        {
            PlayerData.Instance.SetUnlock(3, true);
        }

        if (jsonFileName == "Prol_Day4")
        {
            PlayerData.Instance.SetUnlock(4, true);
        }

        if (jsonFileName == "Prol_Day5")
        {
            PlayerData.Instance.SetUnlock(5, true);
        }

        if (jsonFileName == "Cure_Day0")
        {
            PlayerData.Instance.isDay[0] = true;
        }
    }

    async UniTask ShowNextDialogueAsyncActiv()
    {
        string nextChapter = GetNextChapter();
        if (!string.IsNullOrEmpty(nextChapter))
        {
            ShowNextDialogueAsync(nextChapter).Forget();
        }
    }

    async UniTask ShowNextGameDialogueAsyncActiv()
    {
        string nextChapter = GetNextChapter();
        print("startlog: "+nextChapter);
        await UniTask.WaitUntil(() => nextChapter != null);
   
        if (!string.IsNullOrEmpty(nextChapter))
        {
            print("startlog: "+TextName);
            if (TextName == "Start1")
            {
                TextName = "Start";

           //     await UniTask.WaitUntil(() => TextName == "Start");
            }
            ShowNextGameDialogueAsync(nextChapter).Forget();
           

         
        }
    }

    private string GetNextChapter()
    {
        if (isChapterClear[0] && !isChapterClear[1] && !isChapterClear[2] && !isChapterClear[3])
            return "Chapter1";
        else if (isChapterClear[0] && isChapterClear[1] && !isChapterClear[2] && !isChapterClear[3])
            return "Chapter2";
        else if (isChapterClear[0] && isChapterClear[1] && isChapterClear[2] && !isChapterClear[3])
            return "Chapter3";
        else if (isChapterClear[0] && isChapterClear[1] && isChapterClear[2] && isChapterClear[3] && !isChapterClear[4])
            return "Chapter4";
        else if (isChapterClear[0] && isChapterClear[1] && isChapterClear[2] && isChapterClear[3] &&
                 isChapterClear[4] && !isChapterClear[5])
            return "Chapter5";
        else if (isChapterClear[0] && isChapterClear[1] && isChapterClear[2] && isChapterClear[3] &&
                 isChapterClear[4] && isChapterClear[5])
            return "Chapter6";

        return null;
    }

    async UniTask ShowNextDialogueAsync(string Chapter)
    {
        if (!prologueCanvas.gameObject.activeSelf)
        {
            await TweenEffect.FadeInPrologueCanvas(prologueCanvas);
        }

        string character = null;
        if (Chapter == "Chapter1")
        {
            //print("챕터1");
            character = "prolog";
        }
        else if (Chapter == "Chapter2")
        {
            character = "prolog2";
        }

        await UniTask.WaitUntil(() => dialogueList != null);

        LoadJson.Dialogue currentDialogueOrigin = dialogueList[currentIndex];

        if (currentDialogueOrigin.character == character)
        {
            if (currentIndex < dialogueList.Count + 1)
            {
                LoadJson.Dialogue currentDialogue = dialogueList[currentIndex];
                // 대사의 일부분만 출력

             //   print("배경이름 :" + currentDialogueOrigin.BackGorund);
                if (currentDialogue.BackGorund != null)
                {
                  //  print("배경있음 ");
                    Background.sprite = backGroundList.Find(x => x.name == currentDialogueOrigin.BackGorund).image;
                }
                else
                {
                  //  print("배경없음 ");
                }

                if (currentDialogue.text == null)
                {
                    Debug.Log("강제종료 .");
                    await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
                    ShowNextGameDialogueAsyncActiv().Forget();
                    //함수 정지
                    return;
                }
                else
                {
                    currentText =
                        currentDialogue.text.Substring(0,
                            Mathf.Min(currentText.Length + 1, currentDialogue.text.Length));
                }


                PlayerData.Instance.startbtntext = currentDialogue.Selectnumber;


                if (currentDialogue.IsEnd == "true")
                {
                    isEnd = true;
                }
                else
                {
                    isEnd = false;
                }


                if (currentDialogue.isButtonOn == "true")
                {
                    Chosebtn1Text = currentDialogue.text;
                    Chosebtn2Text = currentDialogue.text2;
                    isButtonOn = true;
                }


                if (currentDialogue.tutorial != null && !IsActiveEvent)
                {
                    IsActiveEvent = true;
                    print("튜토리얼 실행");
                    tutorialGroupList.Find(x => x.name == currentDialogue.tutorial).TutorialEvent.Invoke();
                }

                if (currentDialogue.Pos.ToLower() == "top" && !string.IsNullOrEmpty(topTextMeshPro.text))
                {
                    topTextMeshPro.gameObject.SetActive(true);
                    topTextMeshPro.text = currentText;
                    topTextMeshPro.alpha = 1.0f; // 알파값을 1로 설정하여 텍스트 표시
                }
                else if (currentDialogue.Pos.ToLower() == "middle" && !string.IsNullOrEmpty(middleTextMeshPro.text))
                {
                    middleTextMeshPro.gameObject.SetActive(true);
                    middleTextMeshPro.text = currentText;
                    middleTextMeshPro.alpha = 1.0f; // 알파값을 1로 설정하여 텍스트 표시
                }
                else if (currentDialogue.Pos.ToLower() == "low" && !string.IsNullOrEmpty(lowTextMeshProUGUI.text))
                {
                    lowTextMeshProUGUI.gameObject.SetActive(true);
                    lowTextMeshProUGUI.text = currentText;
                    lowTextMeshProUGUI.alpha = 1.0f; // 알파값을 1로 설정하여 텍스트 표시
                }


                // isButtonOn 이 false 가 될때까지 대기한다.


                if (currentText.Length < currentDialogue.text.Length)
                {
                    // 아직 대사 출력이 완료되지 않은 경우
                    await UniTask.Delay((int)(TypingSpeed * 1000)); // 타이핑 속도만큼 대기
                    try
                    {
                        await ShowNextDialogueAsyncActiv(); // 현재 대사를 계속 출력
                    }
                    catch
                    {
                        Debug.Log("강제종료 .");
                        await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
                        ShowNextGameDialogueAsyncActiv().Forget();
                    }
                }
                else
                {
                    if (!isButtonOn)
                    {
                        await UniTask.Delay((int)(WaitTimeInSeconds * 1000)); // 대기 시간(밀리초) 만큼 대기
                    }


                    //print(currentDialogue);
                    // 대사 출력이 완료된 경우
                    currentText = ""; // 현재 텍스트 초기화

                    // 텍스트 페이드 아웃 효과 추가
                    await FadeOutTextAsync(topTextMeshPro);
                    await FadeOutTextAsync(middleTextMeshPro);
                    await FadeOutTextAsync(lowTextMeshProUGUI);
                    isSound = false;
                    IsActiveEvent = false;
                    await UniTask.WaitUntil(() => isButtonOn == false);
                    currentIndex++;
                    await ShowNextDialogueAsyncActiv(); // 다음 대사 표시
                }
            }
        }
        else
        {
            Debug.Log("대사 끝.");

            if (TextName == "Startprol")
            {
                TextName = "P12";
            }


            if (isEnd)
            {
                if (SceneManager.GetActiveScene().name != "MainScene")
                {
                    isEnd = false; // 현재 씬이 게임 씬인 경우 메인 씬으로 로드
                    SceneLoader.Instace.LoadScene("MainScene");
                    return;
                }
            }


            await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
            ShowNextGameDialogueAsyncActiv().Forget();
        }
    }


    async UniTask ShowNextGameDialogueAsync(string Chapter)
    {
        await UniTask.WaitUntil(() => dialogueList != null);

     //   await UniTask.WaitUntil(() => TextName != null);
        print("find :" + TextName);
        LoadJson.Dialogue currentDialogueOrigin = dialogueList.Find(dialogue => dialogue.TextName == TextName);

        string character = "prolog";
        print("다이얼로그: " + currentDialogueOrigin);
        if (Chapter == "Chapter1")
        {
            print("챕터1");
            character = "prolog";

            playerdialogcount = dialogueList
                .Where((dialogue, index) => index < dialogueList.FindIndex(d => d.character == "prolog2"))
                .Count();
        }
        else if (Chapter == "Chapter2")
        {
            print("챕터2");
            character = "prolog2";

            playerdialogcount = dialogueList
                .Where((dialogue, index) => index < dialogueList.FindIndex(d => d.character == "SelectBtn"))
                .Count();
        }

        if (TextName == "Startprol")
        {
            await TweenEffect.FadeInPrologueCanvas(prologueCanvas);
            if (Chapter == "Chapter1")
            {
                print("챕터1");
                isChapterClear[1] = true;
            }
            else if (Chapter == "Chapter2")
            {
                print("챕터2");
                isChapterClear[2] = true;
            }

            ShowNextDialogueAsyncActiv().Forget();


            return;
        }


        if (currentDialogueOrigin == null)
        {
            if (isEnd)
            {
                if (SceneManager.GetActiveScene().name != "MainScene")
                {
                    isEnd = false; // 현재 씬이 게임 씬인 경우 메인 씬으로 로드
                    SceneLoader.Instace.LoadScene("MainScene");
                    return;
                }
                else
                {
                    MainPlayer.SetActive(true);
                    gamescene.SetActive(false);
                    TweenEffect.OpenPopup(AttendPopUp);
                }
            }
        }

        float likegage = 0;
        likegage = PlayerData.Instance?.GetLikeGage(jsonFileName) ?? 0;

        print("호감도 :" + likegage);
        //     print("호감도데상 :" + currentDialogueOrigin.character);
        if (currentDialogueOrigin.character != character)
        {
            print("dialogcount :" + playerdialogcount);

            LoadJson.Dialogue currentDialogue = dialogueList.Find(dialogue => dialogue.TextName == TextName);

            if (!string.IsNullOrEmpty(currentDialogue.LikeGage))
            {
                if (float.Parse(currentDialogue.LikeGage) <= likegage)
                {
                    print("호감도 비교");

                    print("호감도 비교true:" + float.Parse(currentDialogue.LikeGage));
                    currentDialogue = dialogueList.Find(dialogue =>
                        dialogue.TextName == TextName && dialogue.isLike == "true");
                }
                else
                {
                    print("호감도 비교");
                    print("호감도 비교false:" + float.Parse(currentDialogue.LikeGage));
                    currentDialogue = dialogueList.Find(dialogue =>
                        dialogue.TextName == TextName && dialogue.isLike == "false");
                }
            }
            else
            {
                print("dial Null");
            }


            if (!string.IsNullOrEmpty(currentDialogue.Selectnumber))
            {
                PlayerData.Instance.startbtntext = currentDialogue.Selectnumber;
            }


            print(" 출력할 땍스트: " + currentDialogue.text);
            //currentDialogue.text 중에 "(이름)" 이라는 텍스트가있다면  PlayerData.Instance.PlayerName 으로 바꾼다 
            currentDialogue.text = currentDialogue.text.Replace("(이름)", PlayerData.Instance.PlayerName);
            // 대사의 일부분만 출력


            currentText =
                currentDialogue.text.Substring(0, Mathf.Min(currentText.Length + 1, currentDialogue.text.Length));


            if (currentDialogue.BackGorund != null)
            {
                if (currentDialogue.BackGorund == "Alpahzero")
                {
                    //PlayerTextBoxImage 의 알파 값을 0으로 한다.
                    PlayerTextBoxImage.color = new Color(1, 1, 1, 0);
                }
                else
                {
                    if (SceneManager.GetActiveScene().name == "Travel5")
                    {
                        if (currentDialogue.BackGorund == "MindControl")
                        {
                            // PlayerTextBoxObj.SetActive(false);
                            CharactersList.SetActive(false);
                        }
                        else
                        {
                            // PlayerTextBoxObj.SetActive(true);
                            CharactersList.SetActive(true);
                        }
                    }

                    PlayerTextBoxImage.color = new Color(1, 1, 1, 1);
                    PlayerTextBoxImage.sprite =
                        backGroundList.Find(x => x.name == currentDialogue.BackGorund).image;
                    if (loctionText != null)
                    {
                        if (currentDialogue.BackGorund == "shelter")
                        {
                            loctionText.text = "영등포 쉘터 내부";
                        }
                        else if (currentDialogue.BackGorund == "shelterOutside")
                        {
                            loctionText.text = "영등포 쉘터 외부";
                        }
                        else if (currentDialogue.BackGorund == "HomeGround")
                        {
                            loctionText.text = "영등포 쉘터 탐색꾼 숙소";
                        }
                    }
                }
            }


            if (!isButtonOn)
            {
                if (currentDialogue.character == "Button")
                {
                    if (currentDialogue.isButtonOn == "true")
                    {
                        Chosebtn1Text = currentDialogue.text;
                        Chosebtn2Text = currentDialogue.text2;
                        Chosebtn3Text = currentDialogue.text3;
                        // print(currentDialogue.text);
                        // print(currentDialogue.text2);
                        // print(currentDialogue.text3);
                        isButtonOn = true;
                        PlayerTextBoxObj.SetActive(false);
                    }
                }
                else if (!isButtonOn)
                {
                    PlayerTextBoxObj.SetActive(true);
                    characterState(currentDialogue.character, currentDialogue.characterName, currentDialogue.State);
                }
            }


            if (currentDialogue.tutorial != null && !IsActiveEvent)
            {
                IsActiveEvent = true;

                //tutorialGroupList.name 과 currentDialogue.tutorial 이 동일한 게임 오브젝트를 활성화한다.
                //tutorialGroupList.Find(x => x.name == currentDialogue.tutorial).TutorialObj.SetActive(true);
                if (currentDialogue.tutorial == "EnableEndingScene")
                {
                    tutorialGroupList.Find(x => x.name == currentDialogue.tutorial).TutorialEvent.Invoke();
                }
                else
                {
                    SetNextTextName(currentDialogue, currentDialogue.btnop1, currentDialogue.btnop2,
                        currentDialogue.btnop3, currentDialogue.characterName);
                }
            }

            if (currentDialogue.IsEnd == "true")
            {
                isEnd = true;
            }
            else
            {
                isEnd = false;
            }


            if (currentDialogue.Pos == "PlayerText" && !string.IsNullOrEmpty(PlayerTextBox.text))
            {
                PlayerTextBox.gameObject.SetActive(true);
                PlayerTextBox.text = currentText;
                PlayerTextBox.alpha = 1.0f; // 알파값을 1로 설정하여 텍스트 표시
                if (SoundManager.Instance != null && !isSound)
                {
                    isSound = true;
                    if (currentDialogue.Sound.ToString() == "OneBreath")
                    {
                        SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.OneBreath);
                    }

                    if (currentDialogue.Sound == "WomanLaugh")
                    {
                        SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.WomanLaugh);
                    }

                    if (currentDialogue.Sound == "Walk")
                    {
                        SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.Walk);
                    }

                    if (currentDialogue.Sound == "bagsound")
                    {
                        SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.bagsearch);
                    }
                }

                //todo: currentDialogue.isButtonOn 이 true 이면 선택권 오브젝트를 띄운다. 버튼으로 선택한 번호와 currentDialogue.Selectnumber 가 같은 currentDialogue.text를 재생한다  
            }


            if (currentText.Length < currentDialogue.text.Length)
            {
                // 아직 대사 출력이 완료되지 않은 경우
                await UniTask.Delay((int)(TypingSpeed * 1000)); // 타이핑 속도만큼 대기
                await ShowNextGameDialogueAsyncActiv(); // 현재 대사를 계속 출력
            }
            else
            {
                // 대사 출력이 완료된 경우
                //  await UniTask.Delay((int)(WaitTimeInSeconds * 1000)); // 대기 시간(밀리초) 만큼 대기
                // currentText = ""; // 현재 텍스트 초기화
                //
                // // 텍스트 페이드 아웃 효과 추가
                // await FadeOutTextAsync(topTextMeshPro);
                // await FadeOutTextAsync(middleTextMeshPro);
                // await FadeOutTextAsync(lowTextMeshProUGUI);
                //
                // currentIndex++;
                // await ShowNextDialogueAsync(); // 다음 대사 표시
                print(currentDialogue.NextTextName);
                if (currentDialogue.NextTextName != null)
                {
                    print("insert: " + currentDialogue.NextTextName);
                    TextName = currentDialogue.NextTextName;
                }

                isCliled = false;
            }
            //}
        }
        else
        {
            Debug.Log("대사 끝.");
            await TweenEffect.FadeInPrologueCanvas(prologueCanvas);
            if (Chapter == "Chapter1")
            {
                isChapterClear[1] = true;
            }
            else if (Chapter == "Chapter2")
            {
                isChapterClear[2] = true;
            }

            ShowNextDialogueAsyncActiv().Forget();
        }
    }

    private void characterState(string character, string characterName, string state)
    {
        Player_Nomaral.gameObject.SetActive(false);
        Player_Panic.gameObject.SetActive(false);
        ShealtherLeader_shadow.gameObject.SetActive(false);
        Gangyuri_Normal.gameObject.SetActive(false);
        Gangyuri_shadow.gameObject.SetActive(false);
        Player_Smile.gameObject.SetActive(false);
        Player_phsyconic.gameObject.SetActive(false);


        if (characterName == "(이름)")
        {
            PlayerNameBox.text = PlayerData.Instance.PlayerName;
        }
        else
        {
            PlayerNameBox.text = characterName;
        }

        switch (character)
        {
            case "Player":
                if (state == "Normal")
                    Player_Nomaral.gameObject.SetActive(true);
                else if (state == "Panic")
                    Player_Panic.gameObject.SetActive(true);
                else if (state == "smile")
                {
                    Player_Smile.gameObject.SetActive(true);
                }
                else if (state == "phsyconic")
                {
                    Player_phsyconic.gameObject.SetActive(true);
                }

                break;

            case "ShelterLeader":
                PlayerNameBox.text = "쉘터지도자";
                ShealtherLeader_shadow.gameObject.SetActive(true);
                break;

            case "Gangyuri":
                PlayerNameBox.text = "강유리";
                if (state == "Normal")
                    Gangyuri_Normal.gameObject.SetActive(true);
                else if (state == "shadow")
                    Gangyuri_shadow.gameObject.SetActive(true);
                break;

            default:
                Debug.LogWarning("Unknown character: " + character);
                break;
        }


        string lowerCaseState = state;

        if (lowerCaseState == "Normal" || lowerCaseState == "Panic" || lowerCaseState == "shadow" ||
            lowerCaseState == "smile" || lowerCaseState == "phsyconic" || lowerCaseState == "Lovely")
        {
            // 상태에 따라 해당 오브젝트를 활성화
            var characterObject = charactersList.Find(x => x.name == character);


            // 모든 상태에 대해 오브젝트를 비활성화
            foreach (var obj in charactersList)
            {
                obj.Normalobj.SetActive(false);
                obj.Panicobj.SetActive(false);
                obj.Shadowobj.SetActive(false);
                obj.Smileobj.SetActive(false);
                obj.Phsyconicobj.SetActive(false);
                if (obj.Lovelyobj != null)
                {
                    obj.Lovelyobj.SetActive(false);
                }
            }


            if (lowerCaseState == "Normal")
            {
                if (characterObject.Normalobj != null)
                {
                    characterObject.Normalobj.SetActive(true);
                }
            }
            else if (lowerCaseState == "Panic")
            {
                if (characterObject.Panicobj != null)
                {
                    characterObject.Panicobj.SetActive(true);
                }
            }
            else if (lowerCaseState == "shadow")
            {
                if (characterObject.Shadowobj != null)
                {
                    characterObject.Shadowobj.SetActive(true);
                }
            }
            else if (lowerCaseState == "smile")
            {
                if (characterObject.Smileobj != null)
                {
                    characterObject.Smileobj.SetActive(true);
                }
            }
            else if (lowerCaseState == "phsyconic")
            {
                if (characterObject.Phsyconicobj != null)
                {
                    characterObject.Phsyconicobj.SetActive(true);
                }
            }
            else if (lowerCaseState == "Lovely")
            {
                if (characterObject.Lovelyobj != null)
                {
                    characterObject.Lovelyobj.SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogWarning("Unknown state: " + state);
        }
    }

    public void Func_skipText()
    {
        if (!isCliled && !isButtonOn)
        {
            if (isTyping)
            {
                print("pace0");
                // 타이핑 효과가 진행 중이라면 타이핑 효과를 정지하고 해당 문단을 전체 출력
                StopTypingEffect();
            }
            else if (cancelFadeOut)
            {
                print("pace1");
                // 페이드 아웃 효과가 진행 중이라면 페이드 아웃 효과를 정지하고 다음 문단으로 넘어감
                currentText = ""; // 현재 텍스트 초기화
                currentIndex++; // 넘어감
                ShowNextGameDialogueAsyncActiv().Forget();
            }
            //타이핑 및 페이드 아웃 효과가 진행 중이 아니면 타이핑 효과 시작
            else
            {
                print(" pace2");
                currentText = ""; // 현재 텍스트 초기화
                StopFadeOutEffect();
                ContinueToNextGameDialogue();
            }

            isSound = false;
            IsActiveEvent = false;
        }
        else
        {
            //     print("qqq");
            StartTypingEffect();
        }
    }

    async UniTask FadeOutTextAsync(TextMeshProUGUI textMeshPro)
    {
        if (!string.IsNullOrEmpty(textMeshPro.text)) // 텍스트가 비어있지 않을 때만 페이드 아웃
        {
            for (float t = 0; t < 1; t += Time.deltaTime / FadeOutSpeed)
            {
                if (!isFadingOutInProgress) // 페이드 아웃이 진행 중이 아닌 경우에만 처리
                {
                    textMeshPro.alpha = 1 - t;
                    await UniTask.Yield(); // 다음 프레임까지 대기
                }
            }

            if (!isFadingOutInProgress) // 페이드 아웃이 진행 중이 아닌 경우에만 처리
            {
                textMeshPro.alpha = 0; // 완전히 사라지도록 설정
                textMeshPro.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Space) && !isButtonOn)
        {
            if (prologueCanvas.gameObject.activeSelf)
            {
                //  print("1");
                if (isTyping)
                {
                    // 타이핑 효과가 진행 중이라면 타이핑 효과를 정지하고 해당 문단을 전체 출력
                    StopTypingEffect();
                }
                else if (isFadingOutInProgress)
                {
                    // 페이드 아웃 효과가 진행 중이라면 페이드 아웃 효과를 정지하고 다음 문단으로 넘어감
                    if (cancelFadeOut)
                    {
                        print("1");
                        cancelFadeOut = false; // 페이드 아웃 취소
                        topTextMeshPro.alpha = 1.0f;
                        middleTextMeshPro.alpha = 1.0f;
                        lowTextMeshProUGUI.alpha = 1.0f;
                        topTextMeshPro.gameObject.SetActive(false);
                        middleTextMeshPro.gameObject.SetActive(false);
                        lowTextMeshProUGUI.gameObject.SetActive(false);
                        currentIndex++; // 넘어감
                        ShowNextDialogueAsyncActiv().Forget(); // 대사 표시
                    }
                    else
                    {
                        print("1");
                        StopFadeOutEffect();
                        ContinueToNextDialogue();
                    }
                }
                else
                {
                    //  print("1");
                    //타이핑 및 페이드 아웃 효과가 진행 중이 아니면 타이핑 효과 시작
                    StartTypingEffect();
                }
            }
            else
            {
                //   print("1");
                Func_skipText();
            }
        }

#elif UNITY_ANDROID
        if (Input.touchCount > 0&&!isButtonOn)
        {
              if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began&&!isButtonOn)
            {       
                Touch touch = Input.GetTouch(0);
                if (prologueCanvas.gameObject.activeSelf)
                {
                    if (isTyping)
                    {
                        // 타이핑 효과가 진행 중이라면 타이핑 효과를 정지하고 해당 문단을 전체 출력
                        StopTypingEffect();
                    }
                    else if (isFadingOutInProgress)
                    {
                        // 페이드 아웃 효과가 진행 중이라면 페이드 아웃 효과를 정지하고 다음 문단으로 넘어감
                        StopFadeOutEffect();
                        ContinueToNextDialogue();
                    }
                    else
                    {
                        //타이핑 및 페이드 아웃 효과가 진행 중이 아니면 타이핑 효과 시작
                        StartTypingEffect();
                    }
                }
                else
                {
                    Func_skipText();
                }
            }
        }
#endif
    }


    // 타이핑 효과 시작 메서드
    void StartTypingEffect()
    {
        isTyping = true;
        // if (currentIndex < dialogueList.Count)
        //  {
        // 현재 대사를 전부 출력
        //LoadJson.Dialogue currentDialogue = dialogueList[currentIndex];
        print("TextName: " + TextName);
        LoadJson.Dialogue currentDialogue = dialogueList.Find(dialogue => dialogue.TextName == TextName);
        if (currentDialogue == null)
        {
            print("currentDialogue null .");
        }
        else
        {
            currentText = currentDialogue.text;
        }
        //  }
    }

    // 타이핑 효과 정지 메서드
    void StopTypingEffect()
    {
        isTyping = false;
        // 현재 대사를 전부 출력
        try
        {
            LoadJson.Dialogue currentDialogue = dialogueList[currentIndex];

            currentText = currentDialogue.text;
        }
        catch
        {
        }
    }

    // 페이드 아웃 효과 정지 메서드
    void StopFadeOutEffect()
    {
        isFadingOutInProgress = false;
    }

    // 다음 대사 출력으로 넘어가는 메서드
    async void ContinueToNextDialogue() // async로 메서드 선언
    {
        currentIndex++;

        if (currentIndex < dialogueList.Count)
        {
            // 새로운 대사 출력을 시작
            ShowNextDialogueAsyncActiv().Forget();
        }
        else
        {
            // 대사 끝
            Debug.Log("대사 끝.");
            if (isEnd)
            {
                if (SceneManager.GetActiveScene().name != "MainScene")
                {
                    isEnd = false; // 현재 씬이 게임 씬인 경우 메인 씬으로 로드
                    SceneLoader.Instace.LoadScene("MainScene");
                }
                else
                {
                    MainPlayer.SetActive(true);
                    gamescene.SetActive(false);
                    TweenEffect.OpenPopup(AttendPopUp);
                }
            }

            await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
        }
    }

    async void ContinueToNextGameDialogue() // async로 메서드 선언
    {
        isCliled = true;
        currentIndex++;

        if (string.IsNullOrEmpty(currentText))
        {
            // 새로운 대사 출력을 시작
            ShowNextGameDialogueAsyncActiv().Forget();
            print("TextNotEnd");
            print("currentIndex: " + currentIndex);
            print("dialogcount:" + playerdialogcount);
            print("dialogueList.Count: " + dialogueList.Count);
        }
        else
        {
            // 대사 끝
            print("TextEnd");
            if (isChapterClear[0])
            {
                isChapterClear[1] = true;
            }
            else if (isChapterClear[1])
            {
                isChapterClear[2] = true;
            }

            await TweenEffect.FadeInPrologueCanvas(prologueCanvas);
            if (isEnd)
            {
                if (SceneManager.GetActiveScene().name != "MainScene")
                {
                    isEnd = false; // 현재 씬이 게임 씬인 경우 메인 씬으로 로드
                    SceneLoader.Instace.LoadScene("MainScene");
                }
                else
                {
                    MainPlayer.SetActive(true);
                    gamescene.SetActive(false);
                    TweenEffect.OpenPopup(AttendPopUp);
                }
            }
            else
            {
                ShowNextDialogueAsyncActiv().Forget();
            }

            //  playerdialogcount
        }
    }

    public void SetNextTextName(LoadJson.Dialogue dialogue, string op1, string op2, string op3, string characterName)
    {
        // print(op1);
        // print(op2);
        // print(op3);
        //
        //      TextName = textName;
        string name = null;
        if (jsonFileName.Contains("Cure"))
        {
            name = "Cure";
            print(name);
        }
        else if (jsonFileName.Contains("Kid"))
        {
            name = "Kid";
        }
        else if (jsonFileName.Contains("GangMin"))
        {
            name = "GangMin";
        }


        if (_gameManager == null)
        {
        }
        else
        {
            _gameManager.ButtonGroupList.Find(x => x.GroupName == dialogue.tutorial).Chosebtn1.onClick.AddListener(() =>
            {
                if (dialogue.text.Contains("선물"))
                {
                    print("호감도 추가");
                    PlayerData.Instance.SetLikeGage(name, 20);
                }

                TextName = op1;
            });
            _gameManager.ButtonGroupList.Find(x => x.GroupName == dialogue.tutorial).Chosebtn2.onClick.AddListener(() =>
            {
                if (dialogue.text2.Contains("선물"))
                {
                    print("호감도 추가");
                    PlayerData.Instance.SetLikeGage(name, 20);
                }

                TextName = op2;
            });
            if (string.IsNullOrEmpty(dialogue.text3))
            {
                _gameManager.ButtonGroupList.Find(x => x.GroupName == dialogue.tutorial).Chosebtn3.gameObject
                    .SetActive(false);
            }
            else
            {
                _gameManager.ButtonGroupList.Find(x => x.GroupName == dialogue.tutorial).Chosebtn3.gameObject
                    .SetActive(true);
                _gameManager.ButtonGroupList.Find(x => x.GroupName == dialogue.tutorial).Chosebtn3.onClick
                    .AddListener(() => { TextName = op3; });
            }
        }

        tutorialGroupList.Find(x => x.name == dialogue.tutorial).TutorialEvent.Invoke();
    }
}