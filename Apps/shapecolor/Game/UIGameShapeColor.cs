using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameShapeColor : UIGameBase, IGameShapeColorDelegate
{
    public const int ERROR_STATUS_NONE = 0;
    public const int ERROR_STATUS_SHAPE = 1;
    public const int ERROR_STATUS_COLOR = 2;
    public const int ERROR_STATUS_SHAPE_COLOR = 3;

    public const int ERROR_STATUS_HIDE = 4;
    public const int ERROR_STATUS_SHOW = 5;


    public const string STR_KEYNAME_VIEWALERT_GAME_BOMB = "keyname_viewalert_game_bomb";
    GameShapeColor gameShapeColorPrefab;
    GameShapeColor gameShapeColor;

    static public Language languageColor;


    public Text textTitle;
    void Awake()
    {

        //bg
        LoadPrefab();

        textTitle.gameObject.SetActive(false);
        // audioClipItemFinish = (AudioClip)Resources.Load(AppResAudio.RES_AUDIO_PINTU_BLOCK_FINISH);

        UpdateBtnMusic();
    }


    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
        LayOut();
    }


    // Update is called once per frame 
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }

    void LoadPrefab()
    {
        {
            GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/GameShapeColor");
            if (obj != null)
            {
                gameShapeColorPrefab = obj.GetComponent<GameShapeColor>();

            }

        }



    }
    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        UpdateLanguageColor();
        AppSceneBase.main.ClearMainWorld();
        {
            gameShapeColor = (GameShapeColor)GameObject.Instantiate(gameShapeColorPrefab);
            AppSceneBase.main.AddObjToMainWorld(gameShapeColor.gameObject);
            gameShapeColor.transform.localPosition = new Vector3(0f, 0f, -1f);
            RectTransform rctranPrefab = gameShapeColorPrefab.transform as RectTransform;
            RectTransform rctran = gameShapeColor.transform as RectTransform;
            // 初始化rect
            rctran.offsetMin = rctranPrefab.offsetMin;
            rctran.offsetMax = rctranPrefab.offsetMax;
            gameShapeColor.transform.localPosition = new Vector3(0f, 0f, -1f);

        }
        gameShapeColor.iDelegate = this;
        gameShapeColor.listShape = GameLevelParse.main.listShape;
        gameShapeColor.listColor = GameLevelParse.main.listColor;

        gameShapeColor.LoadGame(GameManager.main.gameMode);
        //必须在LoadGame之后执行 
        LoadBg();

        ShowUserGuide();


        Invoke("OnUIDidFinish", 2f);
    }
    public override void PreLoadDataForWeb()
    {

    }


    public void UpdateLanguageColor()
    {

        if (languageColor != null)
        {
            languageColor.SetLanguage(Language.main.GetLanguage());
            return;
        }
        string strlan = Common.GAME_RES_DIR + "/language/language_color.csv";
        languageColor = new Language();
        languageColor.Init(strlan);
        languageColor.SetLanguage(Language.main.GetLanguage());

    }

    public void UpdateError(int error, string str)
    {
        string title = "";
        Color color = Color.white;
        textTitle.gameObject.SetActive(true);
        Debug.Log("UpdateError error=" + error + " str=" + str);
        switch (error)
        {
            case ERROR_STATUS_NONE:
                title = str;
                color = Color.white;

                break;
            case ERROR_STATUS_SHAPE:
                title = Language.main.GetString("STR_ERROR_SHAPE");
                color = Color.red;
                TTS.main.Speak(title);
                break;
            case ERROR_STATUS_COLOR:
                title = Language.main.GetString("STR_ERROR_COLOR");
                color = Color.red;
                TTS.main.Speak(title);
                break;
            case ERROR_STATUS_SHAPE_COLOR:
                title = Language.main.GetString("STR_ERROR_SHAPE_COLOR");
                color = Color.red;
                TTS.main.Speak(title);
                break;
            case ERROR_STATUS_HIDE:
                textTitle.gameObject.SetActive(false);
                break;
            case ERROR_STATUS_SHOW:
                textTitle.gameObject.SetActive(true);
                break;
        }
        textTitle.text = title;
        textTitle.color = color;
    }


    void OnGetLanguageFileDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (languageGame != null)
        {
            languageGame.SetLanguage(Language.main.GetLanguage());
            return;
        }
        languageGame = new Language();
        languageGame.Init(data);
        languageGame.SetLanguage(Language.main.GetLanguage());
    }


    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (req == httpReqLanguage)
        {
            OnGetLanguageFileDidFinish(isSuccess, data, false);
        }

    }

    public override void LayOut()
    {
        if (gameShapeColor != null)
        {
            gameShapeColor.LayOut();
        }
    }


    bool IsInColorFilter(ShapeColorItemInfo colorfilter, ShapeColorItemInfo info)
    {
        bool isfilter = false;
        foreach (ShapeColorItemInfo infocolor in colorfilter.listColorFilter)
        {
            if (info.id == infocolor.id)
            {
                isfilter = true;
                break;
            }
        }
        return isfilter;
    }
    void LoadBg()
    {
        List<object> listBgNew = new List<object>();
        foreach (ShapeColorItemInfo infobg in GameLevelParse.main.listBg)
        {
            bool isColorFilter = false;
            foreach (ShapeColorItemInfo infocolor in gameShapeColor.listColorShow)
            {
                isColorFilter = IsInColorFilter(infobg, infocolor);
                if (isColorFilter)
                {
                    break;
                }
            }
            if (!isColorFilter)
            {
                listBgNew.Add(infobg);
            }
        }
        Debug.Log("listBgNew.count = " + listBgNew.Count);

        int rdm = Random.Range(0, listBgNew.Count);

        ShapeColorItemInfo info = gameShapeColor.GetItemInfoShapeColor(rdm, listBgNew);
        if (info != null)
        {
            AppSceneBase.main.UpdateWorldBg(info.pic);
            LayOut();
        }

    }

    static public string StringOfGameStatus(int status)
    {
        string str = "";
        switch (status)
        {
            case GameBase.GAME_STATUS_UN_START:
                str = Language.main.GetString("STR_GAME_STATUS_UN_START");
                break;
            case GameBase.GAME_STATUS_PLAY:
                str = Language.main.GetString("STR_GAME_STATUS_PLAY");
                break;
            case GameBase.GAME_STATUS_FINISH:
                str = Language.main.GetString("STR_GAME_STATUS_FINISH");
                break;
        }

        return str;
    }
    public string GameStatusOfShape(ShapeColorItemInfo info)
    {
        int status = PlayerPrefs.GetInt(GameShapeColor.STR_KEY_GAME_STATUS_SHAPE + info.id);
        string str = StringOfGameStatus(status);
        return str;
    }
    public string GameStatusOfColor(ShapeColorItemInfo info)
    {
        int status = PlayerPrefs.GetInt(GameShapeColor.STR_KEY_GAME_STATUS_COLOR + info.id);
        string str = StringOfGameStatus(status);
        return str;
    }
    public string ShapeTitleOfItem(ShapeColorItemInfo info)
    {
        if (languageGame == null)
        {
            return null;
        }
        string key = LanguageKeyOfShape(info);
        string str = languageGame.GetString(key);
        return str;
    }
    public string ColorTitleOfItem(ShapeColorItemInfo info)
    {
        //InitLanguage();
        if (languageColor == null)
        {
            return null;
        }
        string str = languageColor.GetString(info.id);
        return str;
    }

    string StringOfItem(ShapeColorItemInfo info)
    {
        string str = "";
        string strColor = UIGameShapeColor.languageColor.GetString(info.colorid);
        string strShape = languageGame.GetString(LanguageKeyOfShape(info));
        str = strColor + strShape;
        switch (Language.main.GetLanguage())
        {
            case SystemLanguage.Chinese:

                break;

        }
        return str;
    }

    public string LanguageKeyOfShape(ShapeColorItemInfo info)
    {
        string key = info.id;
        return key;
    }

    public void UpdateBtnMusic()
    {
        UIHomeBase.UpdateBtnMusic(btnMusic);
    }

    public void OnClickBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        bool value = !ret;
        Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, value);
        if (value)
        {
            AudioPlay.main.Play();
        }
        else
        {
            AudioPlay.main.Stop();
        }
        UpdateBtnMusic();
    }



    void ShowGameWin()
    {
        Debug.Log("ShowGameWin");
        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_GAME_FINISH);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_GAME_FINISH);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_GAME_FINISH);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_GAME_FINISH);

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GAME_FINISH, OnUIViewAlertFinished);

    }

    void ShowGameBomb()
    {
        Debug.Log("ShowGameBomb");
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_GAME_BOMB");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_BOMB");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_GAME_BOMB");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_GAME_BOMB");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_GAME_BOMB, OnUIViewAlertFinished);

    }

    void ShowUserGuide()
    {

        if (Common.isMonoPlayer)
        {
            return;
        }
        string pkey = AppString.STR_KEY_USER_GUIDE + Common.GetAppVersion();
        bool isshowplay = Common.GetBool(pkey);
        if (isshowplay == true)
        {
            return;
        }


        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_USER_GUIDE);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_USER_GUIDE);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_USER_GUIDE);
            string no = yes;
            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_USER_GUIDE, OnUIViewAlertFinished);
        }

    }

    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_GAME_FINISH == alert.keyName)
        {
            if (isYes)
            {
                LevelManager.main.GotoNextLevelWithoutPlace();
            }
            else
            {

            }
        }


        if (STR_KEYNAME_VIEWALERT_USER_GUIDE == alert.keyName)
        {

            string pkey = AppString.STR_KEY_USER_GUIDE + Common.GetAppVersion();
            Common.SetBool(pkey, true);
        }

        if (STR_KEYNAME_VIEWALERT_GAME_BOMB == alert.keyName)
        {
            if (isYes)
            {
                GameManager.main.GotoPlayAgain();
            }
            else
            {
                //show ad video
                AdKitCommon.main.callbackAdVideoFinish = OnAdKitFinishAdVideo;
                AdKitCommon.main.ShowAdVideo();
            }
        }


    }
    public override void OnClickBtnBack()
    {
        Debug.Log("OnClickBtnBack");
        base.OnClickBtnBack();
    }

    #region GameShapeColor delegate
    public void OnGameShapeColorDidWin(GameShapeColor g)
    {
        ShowGameWin();
        OnGameWinBase();
    }
    public void OnGameShapeColorDidBomb(GameShapeColor g)
    {
        ShowGameBomb();
    }
    public void OnGameShapeColorDidError(GameShapeColor g, int error, ShapeColorItemInfo info)
    {
        //speak
        string str = "";
        if (info != null)
        {
            str = StringOfItem(info);
            TTS.main.Speak(str);
            Debug.Log(str);
        }

        UpdateError(error, str);
    }

    #endregion

    public void OnAdKitFinishAdVideo(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {

            }

            if (status == AdKitCommon.AdStatus.FAIL)
            {
                //观看视频失败 重玩
                GameManager.main.GotoPlayAgain();
            }
        }
    }


    public override void OnGameAdKitFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        base.OnGameAdKitFinish(type, status, str);
        if (type == AdKitCommon.AdType.BANNER)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                LayOut();

            }

            if (status == AdKitCommon.AdStatus.FAIL)
            {

            }
        }
    }
}
