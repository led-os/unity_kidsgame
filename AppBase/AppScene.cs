using System.Collections;
using System.Collections.Generic;
using Moonma.AdKit.AdVideo;
using UnityEngine;
using UnityEngine.UI;
public class AppScene : AppSceneBase
{
    public override void RunApp()
    {
        base.RunApp();

         InitAd();
         
        if (Common.isPC)
        {
            SetRootViewController(ScreenShotViewController.main);
        }
        else
        {
            SetRootViewController(MainViewController.main);
        }
       
    }


    bool EnableAdVideo()
    {
        if (Common.noad)
        {
            return false;
        }

        return true;
    }
    void InitAd()
    {
        if (EnableAdVideo())
        {
            //提前加载unity广告, 解决unity视频第一次不显示的问题
                   AdVideo.SetType(AdVideo.ADVIDEO_TYPE_REWARD); 
            {
                AdVideo.PreLoad(Source.UNITY);
                AdVideo.PreLoad(Source.MOBVISTA);
            }

         
        }
        // AdKitCommon.main.InitAdBanner();
        AdKitCommon.main.InitAdInsert();
        //AdKitCommon.main.InitAdVideo();

        // 显示开机插屏
        AdKitCommon.main.ShowAdInsert(100);

    }
}
