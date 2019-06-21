using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using UnityEngine;
using UnityEngine.UI;
public class UIStarBuy : UIView
{
    public const string STR_KEYNAME_VIEWALERT_NOAD = "STR_KEYNAME_VIEWALERT_NOAD";
    public const string KEYNAME_VIEWALERT = "viewalert";
    public const string STR_IAP_NOAD = "noad";
    public const string STR_IAP_100STAR = "100stars";
    public const int GOLD_BUY_VALUE = 100;
    public Image imageBg;
    public Image imageBoard;
    public Button btnClose;
    public Button btn100Star;
    public GameObject objContent;

    void Awake()
    {



    }
    void Start()
    {
        btn100Star.gameObject.SetActive(true);
        if (StarViewController.main.starType == StarViewController.TYPE_STAR_RESTORE)
        {
            btn100Star.gameObject.SetActive(false);
        }
        LayOut();

    }
    public override void LayOut()
    {
        float x, y, w, h, ratio, scale;

    }
    public void IAPCallBackNoAd(string str)
    {
        Debug.Log("IAPCallBackNoAd::" + str);
        IAP.main.IAPCallBackBase(str);
    }
    public void IAPCallBack100Star(string str)
    {
        Debug.Log("IAPCallBack100Star::" + str);
        //IAP.main.IAPCallBackBase(str);
        if (str == IAP.UNITY_CALLBACK_BUY_DID_FINISH)
        {
            //viewAlert.Hide();
            Common.gold = Common.gold + GOLD_BUY_VALUE;
            UIGameIceCream game = GameViewController.main.gameBase as UIGameIceCream;
            game.uiGameTopBar.UpdateGold();
        }

        if (str == IAP.UNITY_CALLBACK_BUY_DID_Fail)
        {

            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_BUY_FAIL);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_BUY_FAIL);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_BUY_FAIL);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_BUY_FAIL);

            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, KEYNAME_VIEWALERT, OnUIViewAlertFinished);

        }
        if (str == IAP.UNITY_CALLBACK_DID_BUY)
        {
            ViewAlertManager.main.Hide();
        }
        if (str == IAP.UNITY_CALLBACK_BUY_DID_RESTORE)
        {
            ViewAlertManager.main.Hide();
        }
        if (str == IAP.UNITY_CALLBACK_BUY_CANCEL_BY_USER)
        {
            ViewAlertManager.main.Hide();
        }
    }

    public void DoBtnNoAdAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_NOAD");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_NOAD");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_NOAD");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_NOAD");

        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_NOAD, OnUIViewAlertFinished);
    }
    public void DoBtnNoADIAP()
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBackNoAd");
        IAP.main.StartBuy(STR_IAP_NOAD,false);
    }
    public void DoBtn100StarIAP()
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBack100Star");
        IAP.main.StartBuy(STR_IAP_100STAR);
    }
    //恢复内购
    public void DoBtnRestoreIAP()
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBackNoAd");
        IAP.main.RestoreBuy(STR_IAP_NOAD);

    }
    public void OnUIParentGateDidCloseNoAd(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnNoAdAlert();
        }
    }

    public void OnUIParentGateDidCloseRestoreIAP(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnRestoreIAP();
        }
    }
    public void OnUIParentGateDidClose100Star(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtn100StarIAP();
        }
    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_NOAD == alert.keyName)
        {
            if (isYes)
            {
                DoBtnNoADIAP();
            }
        }

    }

    public void OnClickBtnClose()
    {
        PopViewController p = this.controller as PopViewController;
        if (p != null)
        {
            p.Close();
        }
    }

    //去除广告
    public void OnClickBtnAd()
    {
        if (StarViewController.main.starType == StarViewController.TYPE_STAR_RESTORE)
        {
            OnClickBtnRestoreIAP();
            return;
        }
        if (Config.main.APP_FOR_KIDS)
        {

            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseNoAd;
        }
        else
        {
            DoBtnNoAdAlert();
        }
    }
    //购买100星星
    public void OnClickBtnStar()
    {

        if (Config.main.APP_FOR_KIDS)
        {

            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidClose100Star;
        }
        else
        {
            DoBtn100StarIAP();
        }

    }

    public void OnClickBtnRestoreIAP()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseRestoreIAP;
        }
        else
        {
            DoBtnRestoreIAP();
        }

    }



}
