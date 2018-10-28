﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIHomeShapeColor : UIHomeBase, IPopViewControllerDelegate
{
    public UILearnProgress uiLearnProgressPrefab;
    public Image imageGoldBg;
    public Text textGold;
    public Button btnShape;
    public Button btnColor;
    public Button btnShapeColor;
    public GameObject objLayoutBtn;

    float layoutBtnOffsetYNormal;
    // [SerializeField] protected GameObject objBtn;  
    void Awake()
    {

        //bg
        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            TTS.Speek(appname);
        }

    }
    // Use this for initialization
    void Start()
    {
        LayOut();
        OnUIDidFinish();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public override void LayOut()
    {
        float x = 0, y = 0;
        Vector2 sizeCanvas = this.frame.size;
        float w, h;

        //image name
        {
            RectTransform rctran = imageBgName.GetComponent<RectTransform>();

            int fontSize = TextName.fontSize;
            int r = fontSize / 2;
            w = Common.GetStringLength(TextName.text, AppString.STR_FONT_NAME, fontSize) + r * 2;
            h = fontSize * 1.5f;
            if (!Device.isLandscape)
            {
                RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;
                if ((w + r * 2 + topBarHeight * 2) > sizeCanvas.x)
                {
                    w = w / 2 + r * 2;
                    h = h * 2;
                }
            }

            rctran.sizeDelta = new Vector2(w, h);
            x = 0;
            y = (sizeCanvas.y - topBarHeight) / 4;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        LayoutChildBase();
    }


    void UpdateGold()
    {


    }

    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        //MainScene.isInMainUI = true;
    }
    void ShowLearnProgress()
    {
        // MainScene.isInMainUI = false;
        LearnProgressViewController.main.Show(null, this);
    }


    public void OnClickGold()
    {

    }
    public void OnClickBtnHistory()
    {

    }



    public void OnClickBtnShape()
    {
        GotoGameByMode(UIGameShapeColor.GAME_MODE_SHAPE);
    }
    public void OnClickBtnShapeColor()
    {
        GotoGameByMode(UIGameShapeColor.GAME_MODE_SHAPE_COLOR);
    }
    public void OnClickBtnColor()
    {
        GotoGameByMode(UIGameShapeColor.GAME_MODE_COLOR);
    }

    public void OnClickBtnBoard()
    {
        ShowLearnProgress();
    }

    void GotoGameByMode(int mode)
    {
        AudioPlay.main.PlayFile(AppCommon.AUDIO_BTN_CLICK);
        GameManager.gameMode = mode;
        GameManager.placeLevel = mode;
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(GuankaViewController.main);
        }
    }
}
