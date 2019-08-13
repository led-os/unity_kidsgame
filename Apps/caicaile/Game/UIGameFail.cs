using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameFail : UIViewPop
{
    public Image imageBg;
    public Text textTitle;
    public Button btnRevive;//复活
    public Button btnRestart; //从第一关开始

    void Awake()
    {
        textTitle.text = Language.main.GetString("STR_GameFail");
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0;

    }

    public void OnClickBtnRevive()
    {

    }
    public void OnClickBtnRestart()
    {

    }
}
