using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Game_PrologManager : MonoBehaviour
{
    [Header("프롤로그 캔버스 부분 ")] [SerializeField]
    private CanvasGroup prologueCanvas;

    [SerializeField] private TextMeshProUGUI topTextMeshPro;
    [SerializeField] private TextMeshProUGUI middleTextMeshPro;
    [SerializeField] private TextMeshProUGUI lowTextMeshProUGUI;

    [Header("플레이어 상호작용 부분 ")] [SerializeField]
    private TextMeshProUGUI PlayerTextBox;

    [SerializeField] private GameObject Player_Panic;
    [SerializeField] private GameObject Player_Nomaral;
    [SerializeField] private GameObject ShealtherLeader_shadow;

    private List<LoadJson.Dialogue> dialogueList = new List<LoadJson.Dialogue>();
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isFadingOutInProgress = false; // 페이드 아웃 진행 여부

    private string currentText = "";

    public float WaitTimeInSeconds = 2.0f; // 첫 번째 대사 표시 후 대기 시간(초)
    public float TypingSpeed = 0.05f; // 타이핑 속도 조절
    public float FadeOutSpeed = 1.0f; // 페이드 아웃 속도

    private bool cancelFadeOut = false; // 페이드 아웃 취소 플래그
    bool isskipbutton;
    public string jsonFileName;

    void Start()
    {
  
        dialogueList = LoadJson.LoadScriptFromJSON(jsonFileName);

        ShowNextDialogueAsync("prolog").Forget();
    }

    async UniTask ShowNextDialogueAsync(string character)
    {
        if (!prologueCanvas.gameObject.activeSelf)
        {
            await TweenEffect.FadeInPrologueCanvas(prologueCanvas);
        }
    
        await UniTask.WaitUntil(() => dialogueList != null);
        LoadJson.Dialogue currentDialogueOrigin = dialogueList[currentIndex];
        if (currentDialogueOrigin.character ==character )
        {
            if (currentIndex < dialogueList.Count + 1)
            {    
                LoadJson.Dialogue currentDialogue = dialogueList[currentIndex];
                // 대사의 일부분만 출력
                currentText =
                    currentDialogue.text.Substring(0, Mathf.Min(currentText.Length + 1, currentDialogue.text.Length));


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
                    await ShowNextDialogueAsync(character); // 현재 대사를 계속 출력
                }
                else
                { //print(currentDialogue);
                    // 대사 출력이 완료된 경우
                    await UniTask.Delay((int)(WaitTimeInSeconds * 1000)); // 대기 시간(밀리초) 만큼 대기
                    currentText = ""; // 현재 텍스트 초기화

                    // 텍스트 페이드 아웃 효과 추가
                    await FadeOutTextAsync(topTextMeshPro);
                    await FadeOutTextAsync(middleTextMeshPro);
                    await FadeOutTextAsync(lowTextMeshProUGUI);

                    currentIndex++;
                    await ShowNextDialogueAsync(character); // 다음 대사 표시
                }
            }
        }
        else
        {
            Debug.Log("대사 끝.");
            await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
            ShowNextGameDialogueAsync().Forget();
        }
    }


    async UniTask ShowNextGameDialogueAsync()
    {
        await UniTask.WaitUntil(() => dialogueList != null);
        LoadJson.Dialogue currentDialogueOrigin = dialogueList[currentIndex];
        if (currentDialogueOrigin.character != "prolog")
        {
            if (currentIndex < dialogueList.Count + 1)
            {
                LoadJson.Dialogue currentDialogue = dialogueList[currentIndex];
                // 대사의 일부분만 출력
                currentText =
                    currentDialogue.text.Substring(0, Mathf.Min(currentText.Length + 1, currentDialogue.text.Length));

                if (currentDialogue.character=="Player" )
                {
                    ShealtherLeader_shadow.gameObject.SetActive(false);
                    Player_Panic.gameObject.SetActive(true);
                }
                else if (currentDialogue.character == "ShelterLeader")
                {
                    ShealtherLeader_shadow.gameObject.SetActive(true);
                    Player_Panic.gameObject.SetActive(false);
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
                    await ShowNextGameDialogueAsync(); // 현재 대사를 계속 출력
                }
                else
                {
                    //todo 스킾 버튼이 눌리면 
                    if (isskipbutton)
                    {
                        isskipbutton = false;
                        currentText = ""; // 현재 텍스트 초기화
                        currentIndex++;
                        await ShowNextDialogueAsync("prolog"); // 다음 대사 표시
                    }
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
            ShowNextDialogueAsync("prolog2").Forget();
            //  await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
        }
    }


    public void Func_skipText()
    {
 
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
            ShowNextGameDialogueAsync().Forget(); // 대사 표시
        }

        //타이핑 및 페이드 아웃 효과가 진행 중이 아니면 타이핑 효과 시작
        else
        {  currentText = ""; // 현재 텍스트 초기화
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
                    ShowNextDialogueAsync("prolog").Forget(); // 대사 표시
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

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

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
            ShowNextDialogueAsync("prolog").Forget();
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

        if (currentIndex < dialogueList.Count)
        {
            // 새로운 대사 출력을 시작
            ShowNextGameDialogueAsync().Forget();
        }
        else
        {
            // 대사 끝
            Debug.Log("대사 끝.");
            await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
        }
    }
}