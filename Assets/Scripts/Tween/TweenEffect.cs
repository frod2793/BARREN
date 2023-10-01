using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TweenEffect : MonoBehaviour
{
    private static bool isFadingOutInProgress; // isFadingOutInProgress를 클래스 멤버 변수로 선언

    private static float FadeOutSpeed = 1.0f;

    public static void OpenPopup(GameObject popUoObj)
    {
        popUoObj.SetActive(true);
        popUoObj.transform.localScale = Vector3.zero;

        popUoObj.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);
    }

    public static void ClosePopup(GameObject popUoObj)
    {
        popUoObj.transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => popUoObj.SetActive(false));
    }

    
    public static async UniTask FadeOutPrologueCanvas(CanvasGroup canvasGroup)
    {
        if (isFadingOutInProgress)
        {
            return;
        }

        isFadingOutInProgress = true;

        canvasGroup.DOFade(0.0f, FadeOutSpeed).OnComplete(() =>
        {
            canvasGroup.gameObject.SetActive(false);
            isFadingOutInProgress = false;
        });
    }
    public static async UniTask FadeInPrologueCanvas(CanvasGroup canvasGroup)
    {
        if (isFadingOutInProgress)
        {
            return;
        }

        isFadingOutInProgress = true;

        // Ensure the canvas is active and visible before starting the fade in
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0.0f;

        canvasGroup.DOFade(1.0f, FadeOutSpeed).OnComplete(() =>
        {
            isFadingOutInProgress = false;
        });
    }
}