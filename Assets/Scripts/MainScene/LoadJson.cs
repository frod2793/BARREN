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
        string filePath = Path.Combine(Application.streamingAssetsPath, filename + ".json");

        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            jsonString = "{\"data\":" + jsonString + "}"; // JSON 배열을 객체로 감싸주기
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(jsonString);
            return wrapper.data;
        }
        else
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다.");
            return null;
        }
    }
}