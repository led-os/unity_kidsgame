﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonma.AdKit.AdInsert;

public class WordListViewController : UIViewController
{
    public const int RUN_COUNT_SHOW_AD = 2;
    UIWordList uiPrefab;
    UIWordList ui;

    public static bool isAdVideoHasFinish = false;
    public static int runCount = 0;

    static private WordListViewController _main = null;
    public static WordListViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new WordListViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        string strPrefab = "AppCommon/Prefab/WordList/UIWordList";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIWordList>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        CreateUI();
        Debug.Log("HomeViewCon)troller ViewDidLoad");

        //if ((!isAdVideoHasFinish) && (runCount >= RUN_COUNT_SHOW_AD) && (!GameManager.main.isShowGameAdInsert))
        if (runCount == 0)
        {
            //至少在home界面显示一次视频广告
            //AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;
            //   if (ui != null)
            // {
            //     ui.OnClickBtnAdVideo();
            // }

            //至少在home界面显示一次开机插屏
            int type = AdConfigParser.SOURCE_TYPE_INSERT;
            string source = Source.GDT;
            AdInsert.InitAd(source);
            AdKitCommon.main.ShowAdInsert(100);

        }
        runCount++;
    }
    public override void ViewDidUnLoad()
    {
        base.ViewDidUnLoad();
        Debug.Log("UIWordList ViewDidUnLoad");
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("UIWordList LayOutView ");
        if (ui != null)
        {
            ui.LayOut();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIWordList)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
        // ui.Init();
    }



    public void OnAdKitAdVideoFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        //if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                isAdVideoHasFinish = true;
            }
        }
    }
}