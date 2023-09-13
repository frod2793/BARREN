using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

public class MainSceneManager : MonoBehaviour
{
    [Header("프롤로그 캔버스 부분 ")] 
    [SerializeField]
    private CanvasGroup prologueCanvas;
    [SerializeField] 
    private TextMeshProUGUI topTextMeshPro;
    [SerializeField] 
    private TextMeshProUGUI middleTextMeshPro;
    [SerializeField] 
    private TextMeshProUGUI lowTextMeshProUGUI;

    private List<LoadJson.Dialogue> dialogueList = new List<LoadJson.Dialogue>();
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isFadingOutInProgress = false; // 페이드 아웃 진행 여부

    private string currentText = "";

    public float WaitTimeInSeconds = 2.0f; // 첫 번째 대사 표시 후 대기 시간(초)
    public float TypingSpeed = 0.05f; // 타이핑 속도 조절
    public float FadeOutSpeed = 1.0f; // 페이드 아웃 속도

    [Header("메인 캔버스")] 
    [SerializeField]
    private GameObject mainCanvas;
    
    [Header("출석 팝업")] 
    [SerializeField]
    private GameObject attanacePopUp;

    

    private bool _isMoved = false;
    void Start()
    {
        dialogueList = LoadJson.LoadScriptFromJSON("prolog");

        ShowNextDialogueAsync().Forget();
    }

    async UniTask ShowNextDialogueAsync()
    {
        await UniTask.WaitUntil(()=> dialogueList !=  null);
        if (currentIndex < dialogueList.Count)
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
                await ShowNextDialogueAsync(); // 현재 대사를 계속 출력
            }
            else
            {
                // 대사 출력이 완료된 경우
                await UniTask.Delay((int)(WaitTimeInSeconds * 1000)); // 대기 시간(밀리초) 만큼 대기
                currentText = ""; // 현재 텍스트 초기화

                // 텍스트 페이드 아웃 효과 추가
                await FadeOutTextAsync(topTextMeshPro);
                await FadeOutTextAsync(middleTextMeshPro);
                await FadeOutTextAsync(lowTextMeshProUGUI);

                currentIndex++;
                await ShowNextDialogueAsync(); // 다음 대사 표시
            }
        }
        else
        {
            Debug.Log("대사 끝.");
            await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
            TweenEffect.OpenPopup(attanacePopUp);
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
        if (Input.GetKeyDown(KeyCode.Space)||Input.touchCount > 0)
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
    }

    // 타이핑 효과 시작 메서드
    void StartTypingEffect()
    {
        isTyping = true;
    }

    // 타이핑 효과 정지 메서드
    void StopTypingEffect()
    {
        isTyping = false;
        // 현재 대사를 전부 출력
        LoadJson.Dialogue currentDialogue = dialogueList[currentIndex];
        currentText = currentDialogue.text;
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
            ShowNextDialogueAsync().Forget();
        }
        else
        {
            // 대사 끝
            Debug.Log("대사 끝.");
            await TweenEffect.FadeOutPrologueCanvas(prologueCanvas);
            
            TweenEffect.OpenPopup(attanacePopUp);
        }
    }
    
    
    public void OnToggleValueChanged(bool isOn, GameObject targetObject)
    {
        // 토글이 클릭되면 DOTween을 사용하여 게임 오브젝트를 움직입니다.
        if (targetObject != null)
        {
            RectTransform rectTransform = targetObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                if (isOn && !_isMoved)
                {
                    // 아래로 -320만큼 움직이도록 DOTween을 사용합니다.
                    rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - 320f, 0.5f)
                        .SetEase(Ease.OutQuad) // 이징(Easing) 설정
                        .OnComplete(() => _isMoved = true); // 이동이 완료되면 상태 변경
                }
                else if (!isOn && _isMoved)
                {
                    // 원래 위치로 되돌아오도록 DOTween을 사용합니다.
                    rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 320f, 0.5f)
                        .SetEase(Ease.OutQuad) // 이징(Easing) 설정
                        .OnComplete(() => _isMoved = false); // 이동이 완료되면 상태 변경
                }
            }
        }
    }

}






