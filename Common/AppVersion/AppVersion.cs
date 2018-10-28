using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
public delegate void OnAppVersionFinishedDelegate(AppVersion app);
public delegate void OnAppVersionUpdateDelegate(AppVersion app);

public class AppVersion
{
    public const string STRING_KEY_APP_CHECK_FINISHED = "app_check_finished";
    static private AppVersion _main = null;
    public static AppVersion main
    {
        get
        {
            if (_main == null)
            {
                _main = new AppVersion();
                _main.Init();
            }
            return _main;
        }
    }

    AppVersionBase appVersionBase;




    public static bool appCheckForXiaomi = false;//xiao app审核中
    static public bool appCheckHasFinished
    //app审核完成
    {
        get
        {
            if (Common.isAndroid)
            {
                if (Config.main.channel == Source.TAPTAP)
                {
                    return true;
                }
                // if (!IPInfo.isInChina)
                // {
                //     //android 国外 直接当作 审核通过
                //   //  return true;
                // }
            }
            bool ret = Common.Int2Bool(PlayerPrefs.GetInt(STRING_KEY_APP_CHECK_FINISHED));

            if (ret)
            {
                Debug.Log("appCheckHasFinished:ret=true");
            }
            else
            {

                Debug.Log("appCheckHasFinished:ret=false");
            }
            return ret;
        }
    }

    static public bool appForPad//
    {
        get
        {
            string str = Common.GetAppPackage();
            bool ret = false;
            if (str.Contains(".pad") || str.Contains(".ipad"))
            {
                ret = true;
            }
            if (Common.isWinUWP)
            {
                if (Common.GetAppName().ToLower().Contains("hd"))
                {
                    ret = true;
                }

            }

            return ret;
        }
    }

    public bool appNeedUpdate//
    {
        get
        {
            bool ret = false;
            if (appVersionBase != null)
            {
                ret = appVersionBase.appNeedUpdate;
            }
            return ret;
        }
    }

    public string strUpdateNote
    {
        get
        {
            string ret = "";
            if (appVersionBase != null)
            {
                ret = appVersionBase.strUpdateNote;
            }
            return ret;
        }
    }

    public string strUrlComment
    {
        get
        {
            string ret = "";
            if (appVersionBase != null)
            {
                ret = appVersionBase.strUrlComment;
            }
            return ret;
        }
    }
    public string strUrlAppstore
    {
        get
        {
            string ret = "";
            if (appVersionBase != null)
            {
                ret = appVersionBase.strUrlAppstore;
            }
            return ret;
        }
    }




    public OnUICommentDidClickDelegate callBackCommentClick { get; set; }

    public OnAppVersionFinishedDelegate callbackFinished { get; set; }



    void Init()
    {
        Debug.Log("AppVersion Init");
        // appNeedUpdate = false;
        // isFirstCreat = false;
        // appCheckForAppstore = false;

    }

    public void OnUICommentDidClick(ItemInfo item)
    {
        if (callBackCommentClick != null)
        {
            callBackCommentClick(item);
        }
    }

    public void OnComment()
    {
        //if (Common.isAndroid)
        if (Config.main.listAppStore.Count > 1)
        {
            CommentViewController.main.callBackClick = OnUICommentDidClick;
            CommentViewController.main.Show(null, null);
            return;
        }
        ItemInfo info = Config.main.listAppStore[0];
        DoComment(info);
    }
    public void DoComment(ItemInfo info)
    {
        string strappid = Config.main.GetAppIdOfStore(info.source);
        string strUrlComment = "";
        switch (info.source)
        {
            case Source.APPSTORE:
                {
                    strUrlComment = "https://itunes.apple.com/cn/app/id" + strappid;
                    if (!IPInfo.isInChina)
                    {
                        strUrlComment = "https://itunes.apple.com/us/app/id" + strappid;
                    }
                }
                break;
            case Source.TAPTAP:
                {
                    strUrlComment = "https://www.taptap.com/app/" + strappid + "/review";
                }
                break;
            case Source.XIAOMI:
                {
                    strUrlComment = "http://app.xiaomi.com/details?id=" + Common.GetAppPackage();
                }
                break;
            case Source.HUAWEI:
                {
                    //http://appstore.huawei.com/app/C100270155
                    strUrlComment = "http://appstore.huawei.com/app/C" + strappid;
                }
                break;


        }
        string url = strUrlComment;
        if (!Common.BlankString(url))
        {
            OnUICommentDidClick(null);
            Debug.Log("strUrlComment::" + url);
            Application.OpenURL(url);
        }
        else
        {
            Debug.Log("strUrlComment is Empty");
        }
    }
    public void StartParseVersion()
    {
        //   startParserVersionXiaomi();
        //   return;
        //android
        if (Common.isAndroid)
        {
            switch (Config.main.channel)
            {
                case Source.XIAOMI:
                    {
                        appVersionBase = new AppVersionXiaomi();
                        break;
                    }
                case Source.TAPTAP:
                    {
                        appVersionBase = new AppVersionTaptap();
                        break;
                    }
                case Source.HUAWEI:
                    {
                        appVersionBase = new AppVersionHuawei();
                        break;
                    }
                case Source.GP:
                    {
                        appVersionBase = new AppVersionGP();
                        break;
                    }
                default:
                    {
                        appVersionBase = new AppVersionBase();
                    }
                    break;

            }

        }
        else if (Common.isiOS)
        {
            appVersionBase = new AppVersionAppstore();
        }

        else if (Common.isWinUWP)
        {
            appVersionBase = new AppVersionWin();
        }
        else
        {
            appVersionBase = new AppVersionBase();
        }

        //appVersionBase = new AppVersionXiaomi();

        appVersionBase.callbackFinished = OnAppVersionBaseFinished;
        appVersionBase.Init();
        appVersionBase.StartParseVersion();
    }



    public void OnAppVersionBaseFinished(AppVersionBase app)
    {
        if (this.callbackFinished != null)
        {
            this.callbackFinished(this);
        }
    }




}
