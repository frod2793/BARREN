using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Button skipTextBtn;
    [Header("버튼 레이아웃")] [SerializeField] private GameObject ButtonLayout;
    [SerializeField] private Button Chosebtn1;
    [SerializeField] private Button Chosebtn2;
    private Game_PrologManager _gamePrologManager;
    // Start is called before the first frame update
    void Start()
    {
        _gamePrologManager = FindObjectOfType<Game_PrologManager>();
        
        skipTextBtn.onClick.AddListener((() =>
        {
          //  _gamePrologManager.Func_skipText();
        }));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
