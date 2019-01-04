using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameShapeColor : UIGameBase, IGameShapeColorDelegate
{

    public const string STR_KEYNAME_VIEWALERT_GAME_BOMB = "keyname_viewalert_game_bomb";
    GameShapeColor gameShapeColorPrefab;
    GameShapeColor gameShapeColor;

    public HttpRequest httpReqBgJson;
    public HttpRequest httpReqGuankaJsonShape;
    public HttpRequest httpReqGuankaJsonColor;

    List<object> listBg;
    static public List<object> listShape;
    static public List<object> listColor;
    void Awake()
    {

        //bg
        LoadPrefab();


        // audioClipItemFinish = (AudioClip)Resources.Load(AppResAudio.RES_AUDIO_PINTU_BLOCK_FINISH);
        InitLanguage();
        ParseGuanka();
    }


    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(GameManager.gameLevel);
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
            GameObject obj = (GameObject)Resources.Load("App/Prefab/Game/GameShapeColor");
            if (obj != null)
            {
                gameShapeColorPrefab = obj.GetComponent<GameShapeColor>();

            }

        }



    }
    public override void UpdateGuankaLevel(int level)
    {
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
        gameShapeColor.listShape = listShape;
        gameShapeColor.listColor = listColor;




        gameShapeColor.LoadGame(GameManager.gameMode);
        //必须在LoadGame之后执行 
        LoadBg();

        ShowUserGuide();
        OnUIDidFinish();
    }
    public override void PreLoadDataForWeb()
    {
        ParseGuanka();
    }
    void InitLanguage()
    {
        if (languageGame != null)
        {
            return;
        }
        string filepath = Common.GAME_RES_DIR + "/language/language.csv";
        if (Common.isWeb)
        {
            httpReqLanguage = new HttpRequest(OnHttpRequestFinished);
            httpReqLanguage.Get(HttpRequest.GetWebUrlOfAsset(filepath));
        }
        else
        {
            byte[] data = FileUtil.ReadDataAuto(filepath);
            OnGetLanguageFileDidFinish(FileUtil.FileIsExistAsset(filepath), data, true);
        }


    }

    void OnGetBgJsonDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (isSuccess)
        {
            ParseBgList(data);
            if (!isLocal)
            {
                LoadBg();
            }

        }
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

    void OnGetGuankaJsonShapeDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (isSuccess)
        {
            ParseShape(data);
        }
    }
    void OnGetGuankaJsonColorDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (isSuccess)
        {
            ParseColor(data);
        }
    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (req == httpReqLanguage)
        {
            OnGetLanguageFileDidFinish(isSuccess, data, false);
        }
        if (req == httpReqGuankaJsonShape)
        {
            OnGetGuankaJsonShapeDidFinish(isSuccess, data, false);
        }
        if (req == httpReqGuankaJsonColor)
        {
            OnGetGuankaJsonColorDidFinish(isSuccess, data, false);
        }
        if (req == httpReqBgJson)
        {
            OnGetBgJsonDidFinish(isSuccess, data, false);
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
        foreach (ShapeColorItemInfo infobg in listBg)
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
        AppSceneBase.main.UpdateWorldBg(info.pic);
        LayOut();
    }

    static public string StringOfGameStatus(int status)
    {
        string str = "";
        switch (status)
        {
            case GameShapeColor.GAME_STATUS_UN_START:
                str = Language.main.GetString("STR_GAME_STATUS_UN_START");
                break;
            case GameShapeColor.GAME_STATUS_PLAY:
                str = Language.main.GetString("STR_GAME_STATUS_PLAY");
                break;
            case GameShapeColor.GAME_STATUS_FINISH:
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
        InitLanguage();
        if (languageGame == null)
        {
            return null;
        }
        string key = GameShapeColor.LanguageKeyOfShape(info);
        string str = languageGame.GetString(key);
        return str;
    }
    public string ColorTitleOfItem(ShapeColorItemInfo info)
    {
        //InitLanguage();
        if (languageGame == null)
        {
            return null;
        }
        string str = languageGame.GetString("COLOR_TITLE_" + info.id);
        return str;
    }




    public override int GetGuankaTotal()
    {
        int count = ParseGuanka();
        return count;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
        if (listShape != null)
        {
            listShape.Clear();
        }
        if (listColor != null)
        {
            //listColor.Clear();
        }
    }

    void ParseBgList(byte[] data)
    {
        if ((listBg != null) && (listBg.Count != 0))
        {
            return;
        }
        listBg = new List<object>();
        string json = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["list"];
        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            string strdir = Common.GAME_RES_DIR + "/image_bg";
            info.pic = strdir + "/" + (string)item["pic"];
            info.listColorFilter = new List<object>();
            JsonData colorFilter = item["color_filter"];
            for (int j = 0; j < colorFilter.Count; j++)
            {
                JsonData itemtmp = colorFilter[j];
                ShapeColorItemInfo infotmp = new ShapeColorItemInfo();
                infotmp.id = (string)itemtmp["color_id"];
                info.listColorFilter.Add(infotmp);

            }
            listBg.Add(info);
        }
    }
    static public void ParseShape(byte[] data)
    {
        if ((listShape != null) && (listShape.Count != 0))
        {
            return;
        }
        if (listGuanka == null)
        {
            listGuanka = new List<object>();
        }
        listShape = new List<object>();
        int idx = GameManager.placeLevel;
        string json = Encoding.UTF8.GetString(data);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string strPlace = (string)root["place"];
        JsonData items = null;
        string key = "list";
        if (Common.JsonDataContainsKey(root, key))
        {
            items = root[key];
        }
        else
        {
            items = root["items"];
        }

        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            info.id = (string)item["id"];
            //string picdir = Common.GAME_RES_DIR + "/image/" + info.id;
            string picdir = Common.GAME_RES_DIR + "/image/" + info.id;
            if (Common.appKeyName != AppType.SHAPECOLOR)
            {
                picdir = Common.GAME_RES_DIR + "/image/" + strPlace;

            }
            info.pic = picdir + "/" + info.id + ".png";
            info.picInner = picdir + "/" + info.id + "_inner.png";
            info.picOuter = picdir + "/" + info.id + "_outer.png";
            if (Common.appKeyName != AppType.SHAPECOLOR)
            {
                info.picInner = info.pic;
                info.picOuter = info.pic;
            }
            listShape.Add(info);
            listGuanka.Add(info);
        }

    }

    static public void ParseColor(byte[] data)
    {
        if ((listColor != null) && (listColor.Count != 0))
        {
            return;
        }

        listColor = new List<object>();
        int idx = GameManager.placeLevel;
        string json = Encoding.UTF8.GetString(data);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["list"];
        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            info.id = (string)item["id"];
            info.color = Common.RGBString2Color((string)item["color"]);
            listColor.Add(info);
        }

    }


    public override int ParseGuanka()
    {
        Debug.Log("ParseGuanka UIGameShapeColor");
        //ParseBgList();
        {

            string filepath = Common.GAME_RES_DIR + "/image_bg/bg.json";
            if (Common.isWeb)
            {
                httpReqBgJson = new HttpRequest(OnHttpRequestFinished);
                httpReqBgJson.Get(HttpRequest.GetWebUrlOfAsset(filepath));
            }
            else
            {
                byte[] data = FileUtil.ReadDataAuto(filepath);
                OnGetBgJsonDidFinish(true, data, true);
            }

        }



        //  ParseShape();
        {

            string filepath = Common.GAME_RES_DIR + "/guanka/shape_list_place" + GameManager.placeLevel + ".json";
            Debug.Log("filepath=" + filepath);
            if (Common.isWeb)
            {
                httpReqGuankaJsonShape = new HttpRequest(OnHttpRequestFinished);
                httpReqGuankaJsonShape.Get(HttpRequest.GetWebUrlOfAsset(filepath));
            }
            else
            {
                byte[] data = FileUtil.ReadDataAuto(filepath);
                OnGetGuankaJsonShapeDidFinish(true, data, true);
            }

        }
        // ParseColor();
        {

            string filepath = Common.GAME_RES_DIR + "/guanka/color.json";
            if (Common.isWeb)
            {
                httpReqGuankaJsonColor = new HttpRequest(OnHttpRequestFinished);
                httpReqGuankaJsonColor.Get(HttpRequest.GetWebUrlOfAsset(filepath));
            }
            else
            {
                byte[] data = FileUtil.ReadDataAuto(filepath);
                OnGetGuankaJsonColorDidFinish(true, data, true);
            }

        }
        int count = 0;
        if (listShape != null)
        {
            count = GameShapeColor.GUANKA_NUM_PER_ITEM * listShape.Count;
        }
        return count;

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

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GAME_BOMB, OnUIViewAlertFinished);

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
                GameManager.GotoNextLevelWithoutPlace();
            }
            else
            {

            }
        }


        if (STR_KEYNAME_VIEWALERT_GAME_FINISH == alert.keyName)
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

            }
        }


    }
    public override void OnClickBtnBack()
    {

        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        Debug.Log("OnClickBtnBack");
        base.OnClickBtnBack();
    }

    #region GameShapeColor delegate
    public void OnGameShapeColorDidWin(GameShapeColor g)
    {
        ShowGameWin();
    }
    public void OnGameShapeColorDidBomb(GameShapeColor g)
    {
        ShowGameBomb();
    }

    #endregion
}
