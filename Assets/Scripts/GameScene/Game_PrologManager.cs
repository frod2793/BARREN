using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
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

    //BackGround 리스트 생성
    public List<BackGround> backGroundList = new List<BackGround>();

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

    [Header("버튼 레이아웃")] 
    [SerializeField] private TMPro.TextMeshProUGUI Chosebtn1Text;
    [SerializeField] private TMPro.TextMeshProUGUI Chosebtn2Text;

    [Header("플레이어 캐릭터 상태")] [SerializeField]
    private GameObject Player_Panic;

    [SerializeField] private GameObject Player_Nomaral;

    [Header("쉘터지도자 캐릭터 상태")] [SerializeField]
    private GameObject ShealtherLeader_shadow;

    [Header("강유리 캐릭터 상태")] [SerializeField]
    private GameObject Gangyuri_shadow;

    [SerializeField] private GameObject Gangyuri_Normal;

    [Header("게임씬")] [SerializeField] private GameObject gamescene;
    private List<LoadJson.Dialogue> dialogueList = new List<LoadJson.Dialogue>();
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isFadingOutInProgress = false; // 페이드 아웃 진행 여부
    private int playerdialogcount;
    private string currentText = "";

    public float WaitTimeInSeconds = 2.0f; // 첫 번째 대사 표시 후 대기 시간(초)
    public float TypingSpeed = 0.05f; // 타이핑 속도 조절
    public float FadeOutSpeed = 1.0f; // 페이드 아웃 속도

    private bool cancelFadeOut = false; // 페이드 아웃 취소 플래그
    bool isskipbutton;
    public string jsonFileName;

    private bool isEnd;
    private bool[] isChapterClear;

    void Start()
    {
        isChapterClear = new bool[6];

        dialogueList = LoadJson.LoadScriptFromJSON(jsonFileName);
        isChapterClear[0] = true;
        ShowNextDialogueAsyncActiv().Forget();
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
        if (!string.IsNullOrEmpty(nextChapter))
        {
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
                currentText =
                    currentDialogue.text.Substring(0, Mathf.Min(currentText.Length + 1, currentDialogue.text.Length));

                if (currentDialogue.BackGorund != null)
                {
                    Background.sprite = backGroundList.Find(x => x.name == currentDialogue.BackGorund).image;
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
                    //print(currentDialogue);
                    // 대사 출력이 완료된 경우
                    await UniTask.Delay((int)(WaitTimeInSeconds * 1000)); // 대기 시간(밀리초) 만큼 대기
                    currentText = ""; // 현재 텍스트 초기화

                    // 텍스트 페이드 아웃 효과 추가
                    await FadeOutTextAsync(topTextMeshPro);
                    await FadeOutTextAsync(middleTextMeshPro);
                    await FadeOutTextAsync(lowTextMeshProUGUI);

                    currentIndex++;
                    await ShowNextDialogueAsyncActiv(); // 다음 대사 표시
                }
            }
        }
        else
        {
            Debug.Log("대사 끝.");
            await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
            ShowNextGameDialogueAsyncActiv().Forget();
        }
    }


    async UniTask ShowNextGameDialogueAsync(string Chapter)
    {
        await UniTask.WaitUntil(() => dialogueList != null);
        LoadJson.Dialogue currentDialogueOrigin = dialogueList[currentIndex];
        //playerdialogcount = dialogueList.FindAll(x => x.character == "Player"|| x.character == "prolog").Count + 1;
        //playerdialogcount 에 dialogueList.count를 저장하는 데 조건은 character가 Player 와 prolog 이고 prolog2의 전 개수만 카운트한다.


        string character = "prolog";

        if (Chapter == "Chapter1")
        {
            character = "prolog";

            playerdialogcount = dialogueList
                .Where((dialogue, index) => index < dialogueList.FindIndex(d => d.character == "prolog2"))
                .Count();
        }
        else if (Chapter == "Chapter2")
        {
            character = "prolog2";

            playerdialogcount = dialogueList
                .Where((dialogue, index) => index < dialogueList.FindIndex(d => d.character == "SelectBtn"))
                .Count();
        }

        if (currentDialogueOrigin.character != character || currentDialogueOrigin.character != character)
        {
            print("dialogcount :" + playerdialogcount);
            if (currentIndex < dialogueList.Count + 1)
            {
                //currentDialogueOrigin.character != "prolog" 조건의 dialogueList 카운트를 dialogcount 에 저장한다
                LoadJson.Dialogue currentDialogue = dialogueList[currentIndex];
                //currentDialogue.text 중에 "(이름)" 이라는 텍스트가있다면  PlayerData.Instance.PlayerName 으로 바꾼다 
                currentDialogue.text = currentDialogue.text.Replace("(이름)", PlayerData.Instance.PlayerName);
                // 대사의 일부분만 출력


                currentText =
                    currentDialogue.text.Substring(0, Mathf.Min(currentText.Length + 1, currentDialogue.text.Length));

                characterState(currentDialogue.character, currentDialogue.State);
                if (currentDialogue.BackGorund != null)
                {
                    if (currentDialogue.BackGorund == "Alpahzero")
                    {
                        //PlayerTextBoxImage 의 알파 값을 0으로 한다.
                        PlayerTextBoxImage.color = new Color(1, 1, 1, 0);
                    }
                    else
                    {
                        PlayerTextBoxImage.color = new Color(1, 1, 1, 1);
                        PlayerTextBoxImage.sprite =
                            backGroundList.Find(x => x.name == currentDialogue.BackGorund).image;
                    }
                }

                if (currentDialogue.tutorial != null)
                {
                    //tutorialGroupList.name 과 currentDialogue.tutorial 이 동일한 게임 오브젝트를 활성화한다.
                    tutorialGroupList.Find(x => x.name == currentDialogue.tutorial).TutorialObj.SetActive(true);
                    tutorialGroupList.Find(x => x.name == currentDialogue.tutorial).TutorialEvent.Invoke();
                }
                else
                {
                    //tutorialGroupList 의 모든 게임 오브젝트 비활성화
                    tutorialGroupList.ForEach(x => x.TutorialObj.SetActive(false));
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
                    if (SoundManager.Instance != null)
                    {
                        if (currentDialogue.Sound.ToString() == "OneBreath")
                        {
                            SoundManager.Instance.Func_EffectPlayOneShot(AudioDefine.OneBreath);
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
                }
            }
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

    private void characterState(string character, string state)
    {
        Player_Nomaral.gameObject.SetActive(false);
        Player_Panic.gameObject.SetActive(false);
        ShealtherLeader_shadow.gameObject.SetActive(false);
        Gangyuri_Normal.gameObject.SetActive(false);
        Gangyuri_shadow.gameObject.SetActive(false);

        switch (character)
        {
            case "Player":
                if (state == "Normal")
                    Player_Nomaral.gameObject.SetActive(true);
                else if (state == "Panic")
                    Player_Panic.gameObject.SetActive(true);
                break;

            case "ShelterLeader":
                ShealtherLeader_shadow.gameObject.SetActive(true);
                break;

            case "Gangyuri":
                if (state == "Normal")
                    Gangyuri_Normal.gameObject.SetActive(true);
                else if (state == "shadow")
                    Gangyuri_shadow.gameObject.SetActive(true);
                break;

            default:
                Debug.LogWarning("Unknown character: " + character);
                break;
        }
    }

    public void Func_skipText()
    {
        print("pace");
        if (isTyping)
        {
            // 타이핑 효과가 진행 중이라면 타이핑 효과를 정지하고 해당 문단을 전체 출력
            StopTypingEffect();
        }
        else if (cancelFadeOut)
        {
            // 페이드 아웃 효과가 진행 중이라면 페이드 아웃 효과를 정지하고 다음 문단으로 넘어감
            currentText = ""; // 현재 텍스트 초기화
            currentIndex++; // 넘어감
            ShowNextGameDialogueAsyncActiv().Forget();
        }
        //타이핑 및 페이드 아웃 효과가 진행 중이 아니면 타이핑 효과 시작
        else
        {
            currentText = ""; // 현재 텍스트 초기화
            StopFadeOutEffect();
            ContinueToNextGameDialogue();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("press Space");
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
                    if (cancelFadeOut)
                    {
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
                        StopFadeOutEffect();
                        ContinueToNextDialogue();
                    }
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

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (prologueCanvas.gameObject)
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
        if (currentIndex < dialogueList.Count)
        {
            // 현재 대사를 전부 출력
            LoadJson.Dialogue currentDialogue = dialogueList[currentIndex];
            currentText = currentDialogue.text;
        }
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
            await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
        }
    }

    async void ContinueToNextGameDialogue() // async로 메서드 선언
    {
        currentIndex++;

        if (currentIndex < playerdialogcount)
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
                gamescene.SetActive(false);
            }
            else
            {
                ShowNextDialogueAsyncActiv().Forget();
            }
         
            //  playerdialogcount
        }
    }
}