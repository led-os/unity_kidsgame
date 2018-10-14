using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameViewController : PopViewController
{

    public const string STR_KEYNAME_VIEWALERT_COMMENT = "STR_KEYNAME_VIEWALERT_COMMENT";
    public UIGameBase gameBasePrefab;
    static private bool isShowComment = false;

    static private GameViewController _main = null;
    public static GameViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameViewController();
                _main.Init();
            }
            return _main;
        }
    }


    static private UIGameBase _gameBase = null;
    public UIGameBase gameBase
    {
        get
        {
            if (_gameBase == null)
            {
                return main.gameBasePrefab;
            }
            return _gameBase;
        }
    }

    void Init()
    {
        LoadGame();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        Debug.Log("UIGameController.ViewDidLoad");

        CreateUI();
    }


    public override void ViewDidUnLoad()
    {
        base.ViewDidUnLoad();
        Debug.Log("UIGameController.ViewDidUnLoad");
        AppSceneBase.main.ClearMainWorld();
        AdKitCommon.main.ShowAdBanner(false);
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("GameViewController LayOutView");
        if ((gameBasePrefab != null) && (_gameBase != null))
        {
            ViewControllerManager.ClonePrefabRectTransform(gameBasePrefab.gameObject, _gameBase.gameObject);
            _gameBase.LayOut();
        }

    }
    public void CreateUI()
    {

        GotoGame(Common.appType);
    }

    static public string GetGamePrefabName()
    {
        string name = Common.appType;
        //首字母大写
        // string strFirst = name.Substring(0, 1);
        // strFirst.ToUpper();
        // string strOther = name.Substring(1);
        // name = "Game" + strFirst + strOther;

        //Resources.Load 文件可以不区分大小写字母
        name = "UIGame" + name;

        return name;
    }
    void LoadGame()
    {
        string name = Common.appType;
        string strPrefab = "App/Prefab/Game/" + GetGamePrefabName();

        Debug.Log("strPrefab=" + strPrefab);
        //Resources.Load 文件可以不区分大小写字母
        GameObject obj = null;
        if (!Device.isLandscape)
        {
            string strPrefab_shu = strPrefab + "_shu";
            GameObject objShu = PrefabCache.main.Load(strPrefab_shu);
            if (objShu != null)
            {
                obj = objShu;
            }
            else
            {
                obj = PrefabCache.main.Load(strPrefab);
            }
        }
        else
        {
            obj = PrefabCache.main.Load(strPrefab);
        }

        if (obj == null)
        {
            Debug.Log("LoadGame obj is null");
        }


        if (obj != null)
        {
            gameBasePrefab = obj.GetComponent<UIGameBase>();
        }

        //gameBasePrefab.LoadPrefab();
    }


    void GotoGame(string name)
    {
        Debug.Log("GotoGame:" + name);
        _gameBase = (UIGameBase)GameObject.Instantiate(gameBasePrefab);
        //_gameBase.mainCamera = mainCamera;
        // _gameBaseRun.mainCamera = mainCamera;
        _gameBase.Init();
        _gameBase.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(gameBasePrefab.gameObject, _gameBase.gameObject);

        RectTransform rctranParent = objController.transform.GetComponent<RectTransform>();

        RectTransform rctran = _gameBase.GetComponent<RectTransform>();
        float x, y, w, h;
        float adBannerHeight = 160f;

        //poivt (0.5,0.5)
        // w = rctranParent.rect.width;
        // h = rctranParent.rect.height-adBannerHeight;
        // x = 0;
        // y = adBannerHeight/2;
        // rctran.sizeDelta = new Vector2(w,h);
        // rctran.anchoredPosition = new Vector2(x,y);

        Vector2 pt = rctran.offsetMin;
        pt.y = adBannerHeight;
        if (Common.appType == AppType.NONGCHANG)
        {
            pt.y = 0;
        }
        rctran.offsetMin = pt;

        AppSceneBase.main.UpdateMainWorldRect(adBannerHeight);

        //显示横幅广告
        AdKitCommon.main.InitAdBanner();


        ShowUserComment();

    }


    void ShowUserComment()
    {
        if (Application.isEditor)
        {
            return;
        }
        if (!AppVersion.appCheckHasFinished)
        {
            return;
        }
        if (isShowComment)
        {
            return;
        }

        // if (Common.GetDayIndexOfUse() <= AppCommon.NO_USER_COMMENT_DAYS)
        // {
        //     return;
        // }

        string pkey = AppString.STR_KEY_COMMENT_VERSION + Common.GetAppVersion();
        bool isshowplay = Common.GetBool(pkey);
        if (isshowplay == true)
        {
            return;
        }

        int day_cur = Common.GetDayIndexOfUse();
        int day_last = PlayerPrefs.GetInt(AppString.STR_KEY_COMMENT_LAST_TIME);
        int day_step = Mathf.Abs(day_cur - day_last);



        //隔几天弹评论
        if (day_step >= AppCommon.USER_COMMENT_DAY_STEP)
        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_USERCOMMENT);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_USERCOMMENT);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_USERCOMMENT);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_USERCOMMENT);

            ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_COMMENT, OnUIViewAlertFinished);
        }

    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_COMMENT == alert.keyName)
        {
            if (isYes)
            {
                string url = AppVersion.main.strUrlComment;
                if (!Common.BlankString(url))
                {
                    isShowComment = true;
                    string pkey = AppString.STR_KEY_COMMENT_VERSION + Common.GetAppVersion();
                    Common.SetBool(pkey, true);
                    int day = Common.GetDayIndexOfUse();
                    PlayerPrefs.SetInt(AppString.STR_KEY_COMMENT_LAST_TIME, day);
                    Application.OpenURL(url);
                }
            }
        }

    }

}
