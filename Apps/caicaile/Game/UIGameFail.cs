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

    protected override void Awake()
    {
        base.Awake();
        textTitle.text = Language.main.GetString("STR_GameFail");
        Common.SetButtonText(btnRevive, Language.main.GetString("STR_GameFail_btnRevive"));
        Common.SetButtonText(btnRestart, Language.main.GetString("STR_GameFail_btnRestart"));
    }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
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
        AudioPlay.main.PlayBtnSound();
        Close();
    }
    public void OnClickBtnRestart()
    {
        AudioPlay.main.PlayBtnSound();
        Close();
        LevelManager.main.placeLevel = 0;
        LevelManager.main.gameLevel = 0;
        LevelManager.main.gameLevelFinish = -1;
        GameManager.main.GotoPlayAgain();
    }
}
