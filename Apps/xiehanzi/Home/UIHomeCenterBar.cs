﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeCenterBar : UIView
{

    public Button btnLearn;
    public Button btnAdVideo;
    public Button btnAddLove;

    void Awake()
    {
        // if (!Config.main.APP_FOR_KIDS)

        if (btnLearn != null)
        {
            btnLearn.gameObject.SetActive(false);
        }
        if (btnAddLove != null)
        {
            btnAddLove.gameObject.SetActive(false);
        }

        if (btnAdVideo != null)
        {
            btnAdVideo.gameObject.SetActive(true);
            if ((Common.noad) || (!AppVersion.appCheckHasFinished))
            {
                btnAdVideo.gameObject.SetActive(false);
            }
            if (Common.isAndroid)
            {
                if (Config.main.channel == Source.GP)
                {
                    //GP市场不显示
                    btnAdVideo.gameObject.SetActive(false);
                }
            }
        }

    }
    // Use this for initialization
    void Start()
    {
        LayOut();

    }



    public override void LayOut()
    {

    }



    public void GotoGame(int mode)
    {
        GameManager.main.gameMode = mode;
        UIViewController controller = null;

        if (mode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            controller = GameViewController.main;
        }
        else
        {

            int total = LevelManager.main.placeTotal;
            if (total > 1)
            {
                controller = PlaceViewController.main;
            }
            else
            {
                controller = GuankaViewController.main;
            }
        }


        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_GUANKA;
            navi.Push(controller);

        }
    }

    public void OnClickBtnLearn()
    {

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            //  navi.Push(LearnViewController.main);

        }
    }

    public void OnClickBtnAdVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }
    public void OnClickBtnAddLove()
    {

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            //  navi.Push(SettingViewController.main);
        }
    }

    public void OnClickBtnHistory()
    {
        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_HISTORY;
            navi.Push(HistoryViewController.main);
        }
        //HistoryViewController.main.Show(null, null);
    }




    public void OnClickBtnNormalWrite()
    {
        GotoGame(GameXieHanzi.GAME_MODE_NORMAL);
    }

    public void OnClickBtnFreeWrite()
    {
        AdKitCommon.main.ShowAdVideo();
        GotoGame(GameXieHanzi.GAME_MODE_FREE_WRITE);
    }

}
