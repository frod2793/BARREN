using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class LoadJson : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private int currentIndex = 0;

    [System.Serializable]
    public class Dialogue
    {
        public string TextName;
        public string NextTextName;
        public string character;
        public string characterName;
        public string LikeGage;
        public string isLike;
        public string text;
        public string text2;
        public string text3;
        public string btnop1;
        public string btnop2;
        public string btnop3;
        public string Pos; // 대사 위치 (Top, Middle, Bottom 등)
        public string Sound;
        public string isButtonOn;
        public string Selectnumber;
        public string State;
        public string BackGorund;
        public string tutorial;
        public string IsEnd;
    }

    [System.Serializable]
    public class MapData
    {
        public string MapName;
        public string Unlock;
        public string Clear;
        public string OpenStage;
        public string ClearStage;
        public string NextMap;
    }

    public class Wrapper
    {
        public List<Dialogue> data;
    }

    public class MapDataWarapper
    {
        public List<MapData> data;
    }

    // public static List<Dialogue> LoadScriptFromJSON(string filename)
    //   {
    //       List<Dialogue> dialogueList = new List<Dialogue>();
    //
    //       // 안드로이드에서 스트리밍 에셋에 접근하기 위한 경로
    //       string streamingAssetsPath = Application.streamingAssetsPath;
    //       string filePath = Path.Combine(streamingAssetsPath, filename + ".json");
    //
    //       if (Application.platform == RuntimePlatform.Android)
    //       {
    //           // 안드로이드 플랫폼에서는 WWW를 사용하여 파일을 읽어옵니다.
    //           using (WWW www = new WWW(filePath))
    //           {
    //               while (!www.isDone) { } // 로딩이 완료될 때까지 대기
    //
    //               if (!string.IsNullOrEmpty(www.error))
    //               {
    //                   Debug.LogError("Error loading JSON: " + www.error);
    //                   return dialogueList;
    //               }
    //
    //               // JSON 데이터를 파싱하여 리스트에 저장
    //               string jsonString = www.text;
    //               jsonString = "{\"data\":" + jsonString + "}"; // JSON 배열을 객체로 감싸주기
    //               Wrapper wrapper = JsonUtility.FromJson<Wrapper>(jsonString);
    //               if (wrapper != null && wrapper.data != null)
    //               {
    //                   dialogueList = wrapper.data;
    //               }
    //               else
    //               {
    //                   Debug.LogError("Failed to parse JSON data.");
    //               }
    //           }
    //       }
    //       else
    //       {
    //           // 안드로이드 이외의 플랫폼에서는 파일 경로를 직접 사용
    //           if (File.Exists(filePath))
    //           {
    //               string jsonString = File.ReadAllText(filePath);
    //               jsonString = "{\"data\":" + jsonString + "}"; // JSON 배열을 객체로 감싸주기
    //               Wrapper wrapper = JsonUtility.FromJson<Wrapper>(jsonString);
    //               if (wrapper != null && wrapper.data != null)
    //               {
    //                   dialogueList = wrapper.data;
    //               }
    //               else
    //               {
    //                   Debug.LogError("Failed to parse JSON data.");
    //               }
    //           }
    //           else
    //           {
    //               Debug.LogError("JSON 파일을 찾을 수 없습니다.");
    //           }
    //       }
    //
    //       return dialogueList;
    //   }

    public static async Task<List<Dialogue>> LoadScriptFromJSONAsync(string filename)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        string streamingAssetsPath = Application.streamingAssetsPath;
        string filePath = Path.Combine(streamingAssetsPath, filename + ".json");

        UnityWebRequest www;

        if (Application.platform == RuntimePlatform.Android)
        {
            www = UnityWebRequest.Get(filePath);
            await www.SendWebRequest();
        }
        else
        {
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

            return dialogueList;
        }

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Error loading JSON: " + www.error);
        }
        else
        {
            string jsonString = www.downloadHandler.text;
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

        return dialogueList;
    }

    private static List<Dialogue> ParseJson(string jsonString)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        // JSON 파싱 및 dialogueList 채우기
        return dialogueList;
    }

    //todo : 맵데이터 로드 방식 수정 필요 
    public static List<MapData> loadScriptFromMapDatasJson(string filename)
    {
        List<MapData> mapDatas = new List<MapData>();

        // 안드로이드에서 스트리밍 에셋에 접근하기 위한 경로
        string streamingAssetsPath = Application.streamingAssetsPath;
        string filePath = Path.Combine(streamingAssetsPath, filename + ".json");

        if (Application.platform == RuntimePlatform.Android)
        {
            // 안드로이드 플랫폼에서는 WWW를 사용하여 파일을 읽어옵니다.
            using (WWW www = new WWW(filePath))
            {
                while (!www.isDone)
                {
                } // 로딩이 완료될 때까지 대기

                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("Error loading JSON: " + www.error);
                    return mapDatas;
                }

                // JSON 데이터를 파싱하여 리스트에 저장
                string jsonString = www.text;
                jsonString = "{\"data\":" + jsonString + "}"; // JSON 배열을 객체로 감싸주기
                MapDataWarapper wrapper = JsonUtility.FromJson<MapDataWarapper>(jsonString);
                if (wrapper != null && wrapper.data != null)
                {
                    mapDatas = wrapper.data;
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
                MapDataWarapper wrapper = JsonUtility.FromJson<MapDataWarapper>(jsonString);
                if (wrapper != null && wrapper.data != null)
                {
                    mapDatas = wrapper.data;
                    print(mapDatas);
                    print(jsonString);
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

        return mapDatas;
    }

    public static void SaveMapDataToJson(List<MapData> mapDataList, string filename)
    {
        MapDataWarapper wrapper = new MapDataWarapper();
        wrapper.data = mapDataList;

        // JSON 데이터로 변환
        string jsonString = JsonUtility.ToJson(wrapper);

        // 저장할 경로
        string streamingAssetsPath = Application.streamingAssetsPath;
        string filePath = Path.Combine(streamingAssetsPath, filename + ".json");

        // 파일에 쓰기
        File.WriteAllText(filePath, jsonString);
        Debug.Log("JSON 데이터가 저장되었습니다: " + filePath);
    }
}