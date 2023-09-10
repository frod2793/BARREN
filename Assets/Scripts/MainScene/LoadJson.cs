using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class LoadJson : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private int currentIndex = 0;

    [System.Serializable]
    public class Dialogue
    {
        public string character;
        public string text;
        public string Pos; // 대사 위치 (Top, Middle, Bottom 등)
    }

    public class Wrapper
    {
        public List<Dialogue> data;
    }

    void Start()
    {
    }
  public static List<Dialogue> LoadScriptFromJSON(string filename)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        // 안드로이드에서 스트리밍 에셋에 접근하기 위한 경로
        string streamingAssetsPath = Application.streamingAssetsPath;
        string filePath = Path.Combine(streamingAssetsPath, filename + ".json");

        if (Application.platform == RuntimePlatform.Android)
        {
            // 안드로이드 플랫폼에서는 WWW를 사용하여 파일을 읽어옵니다.
            using (WWW www = new WWW(filePath))
            {
                while (!www.isDone) { } // 로딩이 완료될 때까지 대기

                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("Error loading JSON: " + www.error);
                    return dialogueList;
                }

                // JSON 데이터를 파싱하여 리스트에 저장
                string jsonString = www.text;
                jsonString = "{\"data\":" + jsonString + "}"; // JSON 배열을 객체로 감싸주기
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(jsonString);
                if (wrapper != null && wrapper.data != null)
                {
                    dialogueList = wrapper.data;
                }
                else
                {
                    Debug.LogError("Failed to parse JSON data.");
                }
            }
        }
        else
        {
            // 안드로이드 이외의 플랫폼에서는 파일 경로를 직접 사용
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                jsonString = "{\"data\":" + jsonString + "}"; // JSON 배열을 객체로 감싸주기
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(jsonString);
                if (wrapper != null && wrapper.data != null)
                {
                    dialogueList = wrapper.data;
                }
                else
                {
                    Debug.LogError("Failed to parse JSON data.");
                }
            }
            else
            {
                Debug.LogError("JSON 파일을 찾을 수 없습니다.");
            }
        }

        return dialogueList;
    }
}