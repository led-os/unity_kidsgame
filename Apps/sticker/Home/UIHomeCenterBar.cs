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
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            // navi.Push(HistoryViewController.main);
        }
    }

public void OnClickBtnGameMode2()
    {
        LevelManager.main.ParseGuanka();
        GameManager.main.gameMode = GameSticker.GAME_MODE_RANDOM;

        LevelManager.main.placeLevel = 0;
        //必须在placeLevel设置之后再设置gameLevel
        LevelManager.main.gameLevel = 0;

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_GUANKA;
            navi.Push(GameViewController.main);
        }
    }


}
