using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private string mapDataFileName;
    [SerializeField] private List<LoadJson.MapData> mapDatas = new List<LoadJson.MapData>();
    // Start is called before the first frame update
    void Start()
    {
      //  mapDatas = LoadJson.loadScriptFromMapDatasJson(mapDataFileName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
