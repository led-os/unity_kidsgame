using System.Collections;
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


    public UIViewController controllerHome;
    void Awake()
    {
        controllerHome = HomeViewController.main;
        if (!Config.main.APP_FOR_KIDS)
        {
            btnLearn.gameObject.SetActive(false);
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

        if (Config.main.APP_FOR_KIDS)
        {
            btnAddLove.gameObject.SetActive(false);
        }
        else
        {
        }

        if (Common.appKeyName == GameRes.GAME_Image)
        {
            btnAddLove.gameObject.SetActive(false);
        }
        if (Common.appKeyName == GameRes.GAME_RIDDLE)
        {
            btnAddLove.gameObject.SetActive(false);
        }
        if (Common.appKeyName == GameRes.GAME_XIEHOUYU)
        {
            btnAddLove.gameObject.SetActive(false);
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




    public void OnClickBtnLearn()
    {

        if (controllerHome != null)
        {
            NaviViewController navi = controllerHome.naviController;
            //  navi.Push(LearnViewController.main);

        }
    }

    public void OnClickBtnAdVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }
    public void OnClickBtnAddLove()
    {
        if (controllerHome != null)
        {
            NaviViewController navi = controllerHome.naviController;
            navi.Push(LoveViewController.main);
        }
    }
}
