using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ToggleInfo
{
    public bool IsMoved { get; set; }
}

public class MainSceneManager : MonoBehaviour
{
    
    //이미지와 이미지의 이름을 갖고있는 구조체생성
    [Serializable]
    public struct BackGround
    {
        public string name;
        public Sprite image;
    }
 
    //BackGround 리스트 생성
    public List<BackGround> backGroundList = new List<BackGround>();

    
    
    [Header("프롤로그 캔버스 부분 ")] [SerializeField]
    private CanvasGroup prologueCanvas;

    [SerializeField] private Image Background;
    [SerializeField] private TextMeshProUGUI topTextMeshPro;
    [SerializeField] private TextMeshProUGUI middleTextMeshPro;
    [SerializeField] private TextMeshProUGUI lowTextMeshProUGUI;

    private List<LoadJson.Dialogue> dialogueList = new List<LoadJson.Dialogue>();
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isFadingOutInProgress = false; // 페이드 아웃 진행 여부

    private string currentText = "";

    public float WaitTimeInSeconds = 2.0f; // 첫 번째 대사 표시 후 대기 시간(초)
    public float TypingSpeed = 0.05f; // 타이핑 속도 조절
    public float FadeOutSpeed = 1.0f; // 페이드 아웃 속도

    private bool cancelFadeOut = false; // 페이드 아웃 취소 플래그

    [Header("메인 캔버스")] [SerializeField] private GameObject mainCanvas;

    [Header("출석 팝업")] [SerializeField] private GameObject attanacePopUp;

    [Header("탐색 지도 팝업")] [SerializeField] private GameObject map_PopUp;

    [Header("스테이지 선택 팝업")] [SerializeField]
    private GameObject SageSelect_PopUp;

    [SerializeField] private GameObject CharacterProsessPopUp;
    private bool _isMoved = false;
    private bool _isEventMoved = false;
    private bool _isContinueMoved = false;

    [Header("게임씬")] [SerializeField] private GameObject gamescene;
    void Start()
    {
        dialogueList = LoadJson.LoadScriptFromJSON("prolog");
        if (  PlayerData.Instance.IsTutorial)
        {
            prologueCanvas.gameObject.SetActive(false);
        }
        else
        {
            
            ShowNextDialogueAsync().Forget();
        }
    }

    async UniTask ShowNextDialogueAsync()
    {
        await UniTask.WaitUntil(() => dialogueList != null);
        if (currentIndex < dialogueList.Count)
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
            //TweenEffect.OpenPopup(attanacePopUp);
            gamescene.SetActive(true);
        //    SceneLoader.Instace.LoadScene("GameScene");
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
                    ShowNextDialogueAsync().Forget(); // 대사 표시
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


    public void OnToggleValueChangedY(bool isOn, GameObject targetObject, float move)
    {
        // 토글이 클릭되면 DOTween을 사용하여 게임 오브젝트를 움직입니다.
        if (targetObject != null)
        {
            RectTransform rectTransform = targetObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                if (isOn && !_isMoved)
                {
                    // 아래로 move만큼 움직이도록 DOTween을 사용합니다.
                    rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - move, 0.5f)
                        .SetEase(Ease.OutQuad) // 이징(Easing) 설정
                        .OnComplete(() => _isMoved = true); // 이동이 완료되면 상태 변경
                }
                else if (!isOn && _isMoved)
                {
                    // 원래 위치로 되돌아오도록 DOTween을 사용합니다.
                    rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + move, 0.5f)
                        .SetEase(Ease.OutQuad) // 이징(Easing) 설정
                        .OnComplete(() => _isMoved = false); // 이동이 완료되면 상태 변경
                }
            }
        }
    }

    public void OnToggleValueChangedX(bool isOn, GameObject targetObject, float originalPoint,float movepoint, ToggleInfo toggleInfo)
    {
      

        if (targetObject != null)
        {
            RectTransform rectTransform = targetObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                float targetX = isOn ? originalPoint : movepoint;

                // OnComplete에 Action 사용
                Action onCompleteAction = () =>
                {
                    ToggleMoveComplete(isOn, targetObject, toggleInfo);

                };

                rectTransform.DOAnchorPosX(targetX, 0.5f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => onCompleteAction());
            }
        }
    }

    private void ToggleMoveComplete(bool isOn, GameObject targetObject, ToggleInfo toggleInfo)
    {
        // 여기서 toggleInfo를 통해 toggleMoved에 대한 작업을 수행
        toggleInfo.IsMoved = isOn;
    }
    
   
    public void EnableAttendpPopup()
    {
        TweenEffect.OpenPopup(attanacePopUp);
    }
    
    public void EnableMap_Popup()
    {
        TweenEffect.OpenPopup(map_PopUp);
    }
    public void DisableMap_Popup()
    {
        TweenEffect.ClosePopup(map_PopUp);
    }
    public void EnableStageSelect_PopUp()
    {
        TweenEffect.OpenPopup(SageSelect_PopUp);
    }

    public void EnableCharacterProssePopUp()
    {
        TweenEffect.OpenPopup(CharacterProsessPopUp);
    }
    public void disnbleStageSelect_PopUp()
    {
        TweenEffect.ClosePopup(CharacterProsessPopUp);
    }
    public void CloseEveryPopup()
    {
        TweenEffect.ClosePopup(attanacePopUp);
        TweenEffect.ClosePopup(map_PopUp);
        TweenEffect.ClosePopup(SageSelect_PopUp);
        TweenEffect.ClosePopup(CharacterProsessPopUp);
    }
    
}