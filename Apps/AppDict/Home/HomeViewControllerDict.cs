using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonma.AdKit.AdInsert;
using Moonma.AdKit.AdBanner;

public class HomeViewControllerDict : PopViewController
{
    public UIAdBanner uiAdBannerPrefab;
    public UIAdBanner uiAdBanner;
    UIHome uiPrefab;
    UIHome ui;

    public static int runCount = 0;
    static private HomeViewControllerDict _main = null;
    public static HomeViewControllerDict main
    {
        get
        {
            if (_main == null)
            {
                _main = new HomeViewControllerDict();
                _main.Init();
            }
            return _main;
        }
    }
    void Init()
    {
        GameObject obj = PrefabCache.main.Load("App/Prefab/Home/UIHome");
        uiPrefab = obj.GetComponent<UIHome>();
        AdKitCommon.main.heightAdCanvas = 160;
        LoadUIAdBanner();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();

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

            //显示横幅广告
            AdKitCommon.main.InitAdBanner();
            Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

            AdBanner.SetScreenOffset(0, Device.heightSystemHomeBar + (int)Common.CanvasToScreenHeight(sizeCanvas, MainViewController.main.GetBarHeight()));
            if (EnableUIAdBanner())
            {
                GameViewController.main.isActive = true;
                uiAdBanner = (UIAdBanner)GameObject.Instantiate(uiAdBannerPrefab);
                uiAdBanner.offsetY = MainViewController.main.GetBarHeight();
                uiAdBanner.SetViewParent(AppSceneBase.main.canvasMain.gameObject);
                //    uiAdBanner.SetViewParent(this.objController);
                UIViewController.ClonePrefabRectTransform(uiAdBannerPrefab.gameObject, uiAdBanner.gameObject);
                uiAdBanner.gameObject.SetActive(true);

            }


            AdKitCommon.main.ShowAdBanner(true);

        }
        runCount++;

    }

    void LoadUIAdBanner()
    {
        if (!EnableUIAdBanner())
        {
            return;
        }
        GameObject obj = PrefabCache.main.Load(UIAdBanner.PREFAB_UIAdBanner);
        if (obj != null)
        {
            uiAdBannerPrefab = obj.GetComponent<UIAdBanner>();
        }
    }

    public void SetAdBannerTop()
    {
        if (uiAdBanner != null)
        {
            uiAdBanner.transform.SetAsLastSibling();
            // uiAdBanner.LayOut();
        }
    }

    public void UpdateAdBanner()
    {
        if (uiAdBanner != null)
        {
            uiAdBanner.LayOut();
        }
    }

    void DestroyUIAdBanner()
    {
        if (!EnableUIAdBanner())
        {
            return;
        }
        if (uiAdBanner != null)
        {
            GameObject.Destroy(uiAdBanner.gameObject);
            uiAdBanner = null;
        }
    }

    bool EnableUIAdBanner()
    {
        if (!AdKitCommon.main.enableBanner)
        {
            return false;
        }
        if (Application.isEditor)
        {
            //编辑器
            return true;
        }

        if (Common.isMonoPlayer)
        {
            return false;
        }

        bool ret = false;

        if (Common.isiOS && !AppVersion.appCheckHasFinished)
        {
            //ios 审核中
            ret = true;
        }
        if (Common.isRemoveAd)
        {
            ret = false;
        }
        return ret;
    }
    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIHome)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);

        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

    void ShowAdInsert()
    {
        if (Config.main.channel == Source.HUAWEI)
        {
            // return;
        }
        string source = Source.GDT;
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
}
