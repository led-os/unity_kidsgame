﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonma.AdKit.AdInsert;

public class HomeViewController : UIViewController
{
    public const int RUN_COUNT_SHOW_AD = 2;
    UIHomeBase uiHomePrefab;
    UIHomeBase uiHome;

    public static bool isAdVideoHasFinish = false;
    public static int runCount = 0;

    static private HomeViewController _main = null;
    public static HomeViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new HomeViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        string strPrefab = "AppCommon/Prefab/Home/" + GetPrefabName();
        string strPrefabDefault = "Common/Prefab/Home/UIHomeDefault";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        if (obj == null)
        {
            obj = PrefabCache.main.Load(strPrefabDefault);
        }

        uiHomePrefab = obj.GetComponent<UIHomeBase>();
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
            //   if (uiHome != null)
            // {
            //     uiHome.OnClickBtnAdVideo();
            // }


            AdKitCommon.main.callbackFinish = OnAdKitCallBack;
            if (Common.isiOS)
            {
                //原生开机插屏
                AdKitCommon.main.ShowAdNativeSplash(Source.ADMOB);
            }
            else
            {
                //至少在home界面显示一次开机插屏  
                ShowAdInsert();

            }

        }
        runCount++;

        AppVersionHuawei app = new AppVersionHuawei();
        app.StartParseVersion();
    }

    public override void ViewDidUnLoad()
    {
        base.ViewDidUnLoad();
        Debug.Log("HomeViewController ViewDidUnLoad");
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("HomeViewController LayOutView ");
        if (uiHome != null)
        {
            uiHome.LayOut();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        uiHome = (UIHomeBase)GameObject.Instantiate(uiHomePrefab);
        uiHome.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiHomePrefab.gameObject, uiHome.gameObject);
        uiHome.Init();
    }


    string GetPrefabName()
    {
        //Resources.Load 文件可以不区分大小写字母
        name = "UIHome" + Common.appType;
        return name;
    }


    void ShowAdInsert()
    {
        if (Config.main.channel == Source.HUAWEI)
        {
            // return;
        }
        string source = Source.GDT;//GDT
        if (Common.isiOS)
        {
            source = Source.CHSJ;
        }
        AdInsert.InitAd(source);
        AdKitCommon.main.ShowAdInsert(100);
    }
    public void OnAdKitCallBack(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.NATIVE)
        {
            if (status == AdKitCommon.AdStatus.FAIL)
            {
                ShowAdInsert();
            }
        }
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
