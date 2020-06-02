using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Moonma.Share;
//https://play.google.com/store/apps/details?id=com.moonma.xiehanzi



public class UIGameXieHanzi : UIGameBase, IGameXieHanziDelegate
{


    public const int TAG_ITEM_LOCK = -1;
    public const int TAG_ITEM_UNLOCK = 0;


    // public const int BIHUA_POINT_PIC_BASE_WIDTH = 1024;
    // public const int BIHUA_POINT_PIC_BASE_HEIGHT = 768;


    public float letterImageZ = -20f;
    static public int rowGame = 0;
    static public int colGame = 0;
    public Camera cameraWordWrite;

    public UIToolBar uiToolBar;
    public GameObject objBtnListOther;
    public Image imageWord;


    public Button btnDemo;
    public Button btnBihua;
    public Button btnPutonghua;
    public Button btnGuangdonghua;

    UIColorBoard uiColorBoardPrefab;
    public UIColorBoard uiColorBoard;

    UIColorInput uiColorInputPrefab;
    public UIColorInput uiColorInput;
    UILineSetting uiLineSettingPrefab;
    public UILineSetting uiLineSetting;
    GameXieHanzi gameXieHanziPrefab;
    public GameXieHanzi gameXieHanzi;
    // MeshPaint meshPaintPrefab;
    // public MeshPaint meshPaint;

    float gameScaleX = 1f;
    float gameScaleY = 1f;

    float itemWidthWorld;
    float itemHeightWorld;

    Vector2 ptDownScreen;
    Vector3 posItemWorld;
    ItemInfo itemInfoSel;
    bool isItemHasSel;

    string strPlace;
    int save_image_w = 800;
    int save_image_h = 800;
    bool isSavingImage;
    int saveImageIndex;
    bool isAddSaveImageBg;
    long tickSaveImageSecond;
    GameObject objSaveImage;
    long tickCapture;

    int offsetTopbarY;
    int heightTopbar;
    float heightTopbarWorld;
    float itemPosZ;

    AudioClip audioClipBlockFinish;



    Dictionary<string, object> dicRoot;


    Rect rectWordWrite;

    int adHeightScreen = 0;


    static public string strSaveWordShotDir//字截图保存目录
    {
        get
        {
            return Application.temporaryCachePath + "/word";
        }
    }




    void Awake()
    {
        Debug.Log("GameXieHanzi awake");
        adHeightScreen = (int)(128 * AppCommon.scaleBase);

        isSavingImage = false;
        saveImageIndex = 0;
        isAddSaveImageBg = false;


        uiToolBar.uiGameXieHanzi = this;

        languageGame = new Language();
        string fileName = Common.GAME_RES_DIR + "/language/language.csv";
        languageGame.Init(fileName);

        // ParseGuanka();
        LoadPrefab();
        if (gameMode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            uiToolBar.btnLineSetting.gameObject.SetActive(true);

            imageWord.gameObject.SetActive(false);
            objBtnListOther.SetActive(false);
            uiToolBar.btnModeAll.gameObject.SetActive(false);
            uiToolBar.btnModeNone.gameObject.SetActive(false);
            uiToolBar.btnModeOne.gameObject.SetActive(false);
            uiToolBar.btnSave.gameObject.SetActive(true);
            uiToolBar.btnDel.gameObject.SetActive(true);
        }
        else
        {
            uiToolBar.btnLineSetting.gameObject.SetActive(false);

            imageWord.gameObject.SetActive(true);
            objBtnListOther.SetActive(true);
            uiToolBar.btnModeAll.gameObject.SetActive(true);
            uiToolBar.btnModeNone.gameObject.SetActive(true);
            uiToolBar.btnModeOne.gameObject.SetActive(true);
            uiToolBar.btnSave.gameObject.SetActive(false);
            uiToolBar.btnDel.gameObject.SetActive(false);
        }
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);
        UpdateColorSelect();
    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
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
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIColorBoard");
            if (obj != null)
            {
                uiColorBoardPrefab = obj.GetComponent<UIColorBoard>();

            }
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIColorInput");
            if (obj != null)
            {
                uiColorInputPrefab = obj.GetComponent<UIColorInput>();

            }
        }

        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UILineSetting");
            if (obj != null)
            {
                uiLineSettingPrefab = obj.GetComponent<UILineSetting>();

            }
        }


        {
            GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/GameXieHanzi");
            if (obj != null)
            {
                gameXieHanziPrefab = obj.GetComponent<GameXieHanzi>();
            }

        }





    }

    void InitUI()
    {
        {
            {
                uiColorBoard = (UIColorBoard)GameObject.Instantiate(uiColorBoardPrefab);
                uiColorBoard.gameObject.SetActive(false);
                // RectTransform rctranPrefab = uiColorBoardPrefab.transform as RectTransform;

                AppSceneBase.main.AddObjToMainCanvas(uiColorBoard.gameObject);
                //uiColorBoard.transform.SetParent(this.controller.objController.transform);
                //uiColorInput.transform.SetParent(this.controller.objController.transform);
                uiColorBoard.transform.localScale = new Vector3(1f, 1f, 1f);

                // RectTransform rctran = uiColorBoard.transform as RectTransform;
                // // 初始化rect
                // rctran.offsetMin = rctranPrefab.offsetMin;
                // rctran.offsetMax = rctranPrefab.offsetMax;

                UIViewController.ClonePrefabRectTransform(uiColorBoardPrefab.gameObject, uiColorBoard.gameObject);


                uiColorBoard.callBackClick = OnUIColorBoardDidClick;

            }
        }
        {
            {
                uiColorInput = (UIColorInput)GameObject.Instantiate(uiColorInputPrefab);
                uiColorInput.gameObject.SetActive(false);

                // RectTransform rctranPrefab = uiColorInputPrefab.transform as RectTransform;
                // Debug.Log("uiColorInputPrefab :offsetMin=" + rctranPrefab.offsetMin + " offsetMax=" + rctranPrefab.offsetMax);


                AppSceneBase.main.AddObjToMainCanvas(uiColorInput.gameObject);
                //uiColorInput.transform.SetParent(this.controller.objController.transform);

                uiColorInput.transform.localScale = new Vector3(1f, 1f, 1f);

                // RectTransform rctran = uiColorInput.transform as RectTransform;
                // Debug.Log("uiColorInput 1:offsetMin=" + rctran.offsetMin + " offsetMax=" + rctran.offsetMax);
                // // 初始化rect
                // rctran.offsetMin = rctranPrefab.offsetMin;
                // rctran.offsetMax = rctranPrefab.offsetMax;
                // Debug.Log("uiColorInput 2:offsetMin=" + rctran.offsetMin + " offsetMax=" + rctran.offsetMax);
                UIViewController.ClonePrefabRectTransform(uiColorInputPrefab.gameObject, uiColorInput.gameObject);


                uiColorInput.callBackUpdateColor = OnUIColorInputUpdateColor;
            }
        }

        {
            {

                uiLineSetting = (UILineSetting)GameObject.Instantiate(uiLineSettingPrefab);
                uiLineSetting.gameObject.SetActive(false);
                RectTransform rctranPrefab = uiLineSettingPrefab.transform as RectTransform;
                AppSceneBase.main.AddObjToMainCanvas(uiLineSetting.gameObject);

                uiLineSetting.transform.localScale = new Vector3(1f, 1f, 1f);

                RectTransform rctran = uiLineSetting.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;
                uiLineSetting.LINE_WIDTH_PIXSEL_MIN = 4;
                uiLineSetting.LINE_WIDTH_PIXSEL_MAX = 256;
                uiLineSetting.callBackSettingLineWidth = OnUILineSettingLineWidth;

            }
        }

        uiLineSetting.transform.SetParent(this.controller.objController.transform);
        uiColorBoard.transform.SetParent(this.controller.objController.transform);
        uiColorInput.transform.SetParent(this.controller.objController.transform);

        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        offsetTopbarY = 128 + 16 * 2;
        heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);
        // if (meshPaint != null)
        // {
        //     uiLineSetting.lineWidthPixsel = meshPaint.lineWidthPixsel;
        //     meshPaint.SetColor(uiToolBar.colorWord);
        // }


        UpdateGold();
        if (!Config.main.isHaveShop)
        {
            uiToolBar.imageGoldBg.gameObject.SetActive(false);
        }
        UpdateImageWord();
        //  UpdateLetterImage(); 

        //GotoWordWriteMode 之前多调用一次以初始化boundsLetter相关参数
        LayOut();

        gameXieHanzi.GotoWordWriteMode(WordWriteMode.WriteWithOneWord);
        LayOut();

        OnUIDidFinish();
    }


    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        AppSceneBase.main.ClearMainWorld();
        languageGame.SetLanguage(Language.main.GetLanguage());
        WordItemInfo infonow = GameLevelParse.main.GetItemInfo();
        long tickItem = Common.GetCurrentTimeMs();
        ParseGuankaItem(infonow);
        tickItem = Common.GetCurrentTimeMs() - tickItem;
        Debug.Log("ParserGuankaItem: tickItem=" + tickItem);


        if (GameManager.main.gameMode != GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            OnGameWinBase();
        }

        {


            gameXieHanzi = (GameXieHanzi)GameObject.Instantiate(gameXieHanziPrefab);

            RectTransform rctranPrefab = gameXieHanziPrefab.transform as RectTransform;
            AppSceneBase.main.AddObjToMainWorld(gameXieHanzi.gameObject);
            RectTransform rctran = gameXieHanzi.transform as RectTransform;
            // 初始化rect
            rctran.offsetMin = rctranPrefab.offsetMin;
            rctran.offsetMax = rctranPrefab.offsetMax;

            gameXieHanzi.transform.localPosition = Vector3.zero;
            //屏幕居中显示
            float z = gameXieHanzi.transform.position.z;
            gameXieHanzi.transform.position = new Vector3(0, 0, z);


            gameXieHanzi.colorWord = uiToolBar.colorWord;
            gameXieHanzi.infoWord = infonow;
            gameXieHanzi.iDelegate = this;
            gameXieHanzi.InitValue();
        }
        InitUI();
    }
    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0, z = 0;
        float scale = 0; float scalex = 0; float scaley = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        Vector2 worldSize = Common.GetWorldSize(mainCam);
        float oft_top = Common.ScreenToWorldHeight(mainCam, Device.offsetTop);
        float oft_bottom = Common.ScreenToWorldHeight(mainCam, Device.offsetBottom);
        float oft_left = Common.ScreenToWorldHeight(mainCam, Device.offsetLeft);
        float oft_right = Common.ScreenToWorldHeight(mainCam, Device.offsetRight);
        float topbar_height = Common.CanvasToWorldHeight(mainCam, sizeCanvas, 160);
        RectTransform rctranWorld = AppSceneBase.main.GetRectMainWorld();

        offsetTopbarY = 160;
        heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY) + Device.offsetTopWorld;
        float heightAdBannerWorld = Device.offsetBottomWithAdBannerWorld;
        float ratio = 1f;
        //UpdateRect
        if (Device.isLandscape)
        {
            ratio = 0.8f;
            w = (rctranWorld.rect.width / 2) * ratio;
            float oft_y = Mathf.Max(heightTopbarWorld, heightAdBannerWorld);
            h = (rctranWorld.rect.height - oft_y) * ratio;
            x = rctranWorld.rect.width / 4 - w / 2;
            y = 0 - h / 2;
        }
        else
        {
            ratio = 0.8f;
            w = (rctranWorld.rect.width) * ratio;
            float oft_y = Mathf.Max(heightTopbarWorld, heightAdBannerWorld);
            h = (rctranWorld.rect.height - oft_y) * ratio;
            x = 0 - w / 2;
            y = 0 - h / 2;
        }
        if (GameXieHanzi.GAME_MODE_FREE_WRITE == gameMode)
        {

            ratio = 0.8f;
            w = (rctranWorld.rect.width) * ratio;
            float oft_y = Mathf.Max(heightTopbarWorld, heightAdBannerWorld);
            h = (rctranWorld.rect.height - oft_y) * ratio;
            x = 0 - w / 2;
            y = 0 - h / 2;
            Rect rc = new Rect(x, y, w, h);
        }

        gameXieHanzi.UpdateRect(new Rect(x, y, w, h));


        if (gameXieHanzi != null)
        {
            gameXieHanzi.LayOut();
        }

        //写字区域
        // if (gameXieHanzi != null)
        // {
        //     ratio = 0.8f;
        //     SpriteRenderer spRender = gameXieHanzi.objSpriteWordWriteBg.GetComponent<SpriteRenderer>();
        //     Texture2D tex2d = spRender.sprite.texture;
        //     Vector2 spsize = new Vector2(tex2d.width / 100f, tex2d.height / 100f);
        //     z = letterImageZ + 1;

        //     z = letterImageZ + 1;
        //     float disp_w = 0, disp_h = 0;


        //     if (Device.isLandscape)
        //     {
        //         disp_w = (Common.GetCameraWorldSizeWidth(mainCam)) * ratio;
        //         float oft_y = Mathf.Max(heightTopbarWorld, heightAdBannerWorld) + oft_bottom;
        //         disp_h = (mainCam.orthographicSize * 2 - oft_y * 2) * ratio;
        //         x = Common.GetCameraWorldSizeWidth(mainCam) / 2;
        //         y = 0;

        //         Debug.Log("disp_w=" + disp_w + " disp_h=" + disp_h + " spsize=" + spsize);
        //     }
        //     else
        //     {
        //         ratio = 0.9f;
        //         disp_w = (Common.GetCameraWorldSizeWidth(mainCam) * 2) * ratio;

        //         float oft_y = Mathf.Max((topbar_height + oft_top), (heightAdBannerWorld * 2 + oft_bottom));
        //         disp_h = (mainCam.orthographicSize * 2 - oft_y * 2) * ratio;
        //         x = 0;
        //         y = 0;/// -mainCam.orthographicSize / 2;
        //     }
        //     if (GameXieHanzi.GAME_MODE_FREE_WRITE == gameMode)
        //     {
        //         ratio = 0.9f;
        //         disp_w = (worldSize.x - Mathf.Max(oft_left, oft_right) * 2) * ratio;
        //         float oft_y = Mathf.Max((topbar_height + oft_top), (heightAdBannerWorld + oft_bottom));
        //         disp_h = (worldSize.y - oft_y * 2) * ratio;
        //         x = 0;
        //         y = 0;
        //     }
        //     scalex = disp_w / spsize.x;
        //     scaley = disp_h / spsize.y;
        //     scale = Mathf.Min(scalex, scaley);
        //     gameXieHanzi.objSpriteWordWriteBg.transform.localScale = new Vector3(scale, scale, 1f);
        //     gameXieHanzi.objSpriteWordWriteBg.transform.position = new Vector3(x, y, z);
        //     if (GameXieHanzi.GAME_MODE_FREE_WRITE == gameMode)
        //     {
        //         Rect rc = new Rect(spRender.bounds.center.x - spRender.bounds.size.x / 2, spRender.bounds.center.y - spRender.bounds.size.y / 2, spRender.bounds.size.x, spRender.bounds.size.y);
        //         meshPaint.UpdateRectPaint(rc);
        //     }

        // }




        //word image
        {

            RectTransform rctran = imageWord.GetComponent<RectTransform>();
            SpriteRenderer spRender = gameXieHanzi.objSpriteWordWriteBg.GetComponent<SpriteRenderer>();
            if (Device.isLandscape)
            {
                float x_left = spRender.bounds.center.x - spRender.bounds.size.x / 2;
                float x_canvas = Common.WorldToCanvasPoint(mainCam, sizeCanvas, new Vector2(x_left, 0)).x;
                Debug.Log("x_left=" + x_left + " x_canvas=" + x_canvas + " sizeCanvas=" + sizeCanvas);
                x = (-sizeCanvas.x / 2 + (x_canvas - sizeCanvas.x / 2)) / 2;
                y = 0;
                w = sizeCanvas.x / 4;
                h = w;
                if (gameMode == GameXieHanzi.GAME_MODE_NORMAL)
                {
                    imageWord.gameObject.SetActive(true);
                }

            }
            else
            {
                x = 0;
                y = sizeCanvas.y / 4;
                w = Common.WorldToCanvasWidth(mainCam, sizeCanvas, spRender.bounds.size.x);
                h = Common.WorldToCanvasWidth(mainCam, sizeCanvas, spRender.bounds.size.y);
                imageWord.gameObject.SetActive(false);
            }
            rctran.anchoredPosition = new Vector2(x, y);
            rctran.sizeDelta = new Vector2(w, h);
        }

        {
            RectTransform rctran = objBtnListOther.GetComponent<RectTransform>();
            float ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemHomeBar);
            if (Device.isLandscape)
            {
                h = rctran.rect.size.y;
                x = -sizeCanvas.x / 4;
                y = -sizeCanvas.y / 2 + GameManager.main.heightAdCanvas + h / 2 + ofty;
            }
            else
            {
                x = 0;
                // y = 0;
                h = rctran.rect.size.y;
                y = -sizeCanvas.y / 2 + GameManager.main.heightAdCanvas + h / 2 + ofty;
            }
            rctran.anchoredPosition = new Vector2(x, y);

        }


    }

    public void OnUIColorBoardDidClick(UIColorBoard ui, UIColorBoardCellItem item, bool isOutSide)
    {
        //  Debug.Log("OnUIColorBoardDidClick isOutSide=" + isOutSide + " item.color=" + item.color);
        if (isOutSide)
        {

        }
        else
        {
            uiToolBar.colorWord = item.color;
            UpdateColorSelect();
        }

        uiColorBoard.gameObject.SetActive(false);
    }
    public void OnUIColorInputUpdateColor(Color color)
    {
        uiToolBar.colorWord = color;
        UpdateColorSelect();
    }
    public void OnUILineSettingLineWidth(int width)
    {
        Debug.Log("OnUILineSettingLineWidth w=" + width);
        gameXieHanzi.SetLineWidthPixsel(width);

    }
    void UpdateColorSelect()
    {
        uiToolBar.btnColorInput.GetComponent<Image>().color = uiToolBar.colorWord;
        if (gameXieHanzi != null)
        {
            gameXieHanzi.UpdateColor(uiToolBar.colorWord);
        }
    }
    void UpdateGold()
    {

        string str = Language.main.GetString("STR_GOLD") + ":" + Common.gold.ToString();
        uiToolBar.textGold.text = str;
        int fontsize = uiToolBar.textGold.fontSize;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        RectTransform rctran = uiToolBar.imageGoldBg.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;
        sizeDelta.x = str_w + fontsize;
        rctran.sizeDelta = sizeDelta;


    }
    void UpdateImageWord()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float x, y, w, h;
        if (gameMode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            return;
        }
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        if (info == null)
        {
            Debug.Log("UpdateImageWord info null");
            return;
        }
        //Texture2D tex = (Texture2D)Resources.Load(info.imageLetter);
        Texture2D tex = LoadTexture.LoadFromAsset(info.imageLetter);
        w = tex.width;
        h = tex.height;
        x = 0;
        y = 0;
        Sprite sprite = Sprite.Create(tex, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));
        imageWord.sprite = sprite;
        RectTransform rctran = imageWord.GetComponent<RectTransform>();
        w = imageWord.sprite.texture.width;//rectTransform.rect.width;
        h = imageWord.sprite.texture.height;//rectTransform.rect.height;
        print("imageBg size:w=" + w + " h=" + h);
        rctran.sizeDelta = new Vector2(w, h);
        float scalex = sizeCanvas.x / w;
        float scaley = sizeCanvas.y / h;
        float scale = Mathf.Max(scalex, scaley);
        // imageWord.transform.localScale = new Vector3(scale, scale, 1.0f);

    }


    public string GetImageResourcePath(int lv)
    {
        //ParseGuanka();
        ItemInfo info = GameLevelParse.main.listGuanka[lv] as ItemInfo;
        return info.pic;
    }

    static public string GetHistorySortCellItemBg()
    {
        string strDirRoot = Common.GAME_RES_DIR + "/image_common";
        string ret = strDirRoot + "/historyBubbleRed_up@2x~ipad.png";
        return ret;
    }


    // public override int GetPlaceTotal()
    // {
    //     return 4;
    // }


    public override void OnUIShareDidClick(ItemInfo item)
    {
        Debug.Log("GameXiehanzi OnUIShareDidClick");
        string title = Language.main.GetString("UIGAME_SHARE_TITLE");
        string detail = Language.main.GetString("UIGAME_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }
    void UpdateWordImageInfoJson(WordItemInfo info)
    {

        string strDirRoot = Common.GAME_RES_DIR;

        string strDirRootImage = strDirRoot + "/image/" + info.id;
        info.pic = strDirRootImage + "/" + info.id + ".png";

        //thumb
        info.thumbLetter = strDirRootImage + "/" + info.id + "_thumb.png";
        //image 
        info.imageLetter = strDirRootImage + "/" + info.id + "_image.png";
        //笔顺示意图
        info.imageBihua = strDirRootImage + "/" + info.id + "_stroke.png";


        //笔顺图片
        if (info.listImageBihua0 == null)
        {
            info.listImageBihua0 = new List<string>();
        }
        info.listImageBihua0.Clear();

        for (int i = 0; i < info.countBihua; i++)
        {
            string strtmp0 = strDirRootImage + "/bihua/" + info.id + "_" + i + ".png";
            info.listImageBihua0.Add(strtmp0);
        }

        //sound 
        string strDirRootSound = "Game/hanziyuan";
        //普通话
        info.soundPutonghua = strDirRootSound + "/sound/" + info.id + "_cn";//".mp3"

        //广东话
        info.soundGuangdonghua = strDirRootSound + "/sound/" + info.id + "_gd";//".mp3"


    }
    public void ParseGuankaItem(WordItemInfo info)
    {
        if (gameMode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            return;
        }
        // string strFileJson = Common.GAME_RES_DIR + "/image/letter/" + info.id + "/" + info.id + ".json";

        string strDir = Common.GAME_RES_DIR + "/image/" + info.id;
        string strFileJson = strDir + "/" + info.id + ".json";

        string json = FileUtil.ReadStringAsset(strFileJson);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        Debug.Log("strFileJson::" + strFileJson);
        JsonData root = JsonMapper.ToObject(json);
        int item_count = (int)root["count"];
        Debug.Log("item_count=" + item_count);
        info.listGuidePoint = new List<object>();

        //示例笔画
        {

            info.listDemoPoint = new List<object>();

            //demo point 
            JsonData jsonDemoPoint = root["demo_point"];
            for (int i = 0; i < item_count; i++)
            {
                string key = "bihua_" + i;
                JsonData jsonBihua = jsonDemoPoint[key];
                List<object> listPoint = new List<object>();
                List<object> listPointGuide = new List<object>();
                for (int j = 0; j < jsonBihua.Count; j++)
                {
                    JsonData item = jsonBihua[j];
                    Vector2 pt = new Vector2(0, 0);
                    string str_x = (string)item["x"];
                    string str_y = (string)item["y"];
                    pt.x = Common.String2Int(str_x);
                    pt.y = Common.String2Int(str_y);
                    //pt *= imageScaleFactor;

                    listPoint.Add(pt);
                }

                info.listDemoPoint.Add(listPoint);
            }
        }

        //提示图片
        {

            info.listGuidePoint = new List<object>();

            //guide point 
            JsonData jsonGuidePoint = root["guide_point"];
            for (int i = 0; i < item_count; i++)
            {
                string key = "bihua_" + i;
                JsonData jsonBihua = jsonGuidePoint[key];
                List<object> listPointGuide = new List<object>();
                for (int j = 0; j < jsonBihua.Count; j++)
                {
                    JsonData item = jsonBihua[j];
                    Vector2 pt = new Vector2(0, 0);
                    string str_x = (string)item["x"];
                    string str_y = (string)item["y"];
                    string str_angle = (string)item["angle"];
                    string str_type = (string)item["type"];
                    string str_direction = (string)item["direction"];

                    pt.x = Common.String2Int(str_x);
                    pt.y = Common.String2Int(str_y);

                    //guide
                    GuideItemInfo guideItemInfo = new GuideItemInfo();
                    guideItemInfo.angle = Common.String2Float(str_angle);
                    guideItemInfo.type = Common.String2Int(str_type);

                    guideItemInfo.direction = Common.String2Int(str_direction);


                    guideItemInfo.point = pt;
                    listPointGuide.Add(guideItemInfo);

                }
                info.listGuidePoint.Add(listPointGuide);
            }
        }

        info.countBihua = info.listDemoPoint.Count;

        UpdateWordImageInfoJson(info);

    }


    void ButtonSetImage(Button btn, string file)
    {
        btn.GetComponent<Image>().sprite = TextureUtil.CreateSpriteFromResource(file);
    }


    public int GetGuankaIndexByWord(WordItemInfo info)
    {
        int idx = 0;
        foreach (WordItemInfo infotmp in GameLevelParse.main.listGuanka)
        {
            if (infotmp.id == info.id)
            {
                break;
            }
            idx++;
        }
        return idx;
    }
    void PlayAudioBlockFinish()
    {
        //AudioPlayer对象在场景切换后可能从当前scene移除了
        GameObject audioPlayer = GameObject.Find("AudioPlayer");
        if (audioPlayer != null)
        {
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClipBlockFinish);
        }
    }
 

    public void OnUIWordWriteFinish(UIWordWriteFinish ui)
    {
        /*
    GotoWordWriteMode(WordWriteMode.WriteWithOneWord);
    */
    }

    void ShowShop()
    {

    }

    // public void OnUIParentGateDidClose(UIParentGate ui, bool isLongPress)
    // {
    //     if (isLongPress)
    //     {
    //         ShowFirstUseAlert();
    //     }
    // }

    public void OnGameXieHanziDidUpdateMode(GameXieHanzi game, WordWriteMode mode)
    {
        switch (mode)
        {
            case WordWriteMode.WriteDemo:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                }
                break;

            case WordWriteMode.WriteWithOneWord:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_SEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);

                }
                break;
            case WordWriteMode.WriteWithOneBihua:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_SEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);

                }
                break;
            case WordWriteMode.WriteWithNone:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_SEL);

                }
                break;

            case WordWriteMode.ShowBihua:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);

                }
                break;

            default:
                break;


        }


    }
    public void OnClickGold()
    {

    }

    public override void OnClickBtnBack()
    {
        uiToolBar.OnClickBtnBack();

    }


    public void OnClickBtnDemo()
    {

        gameXieHanzi.GotoWordWriteMode(WordWriteMode.WriteDemo);

    }
    //笔画示意图
    public void OnClickBtnBihua()
    {

        // if (!isShowBihuaImage)
        // {
        //     writeModePre = writeModeCur;
        // }
        gameXieHanzi.GotoWordWriteMode(WordWriteMode.ShowBihua);
        // if (!isShowBihuaImage)
        // {
        //     //隐藏后恢复之前的模式
        //     writeModeCur = writeModePre;
        // }

    }
    public void OnClickBtnPutonghua()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        if (info == null)
        {
            return;
        }
        PlaySoundFromResource(info.soundPutonghua);
    }
    public void OnClickBtnGuangdonghua()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        if (info == null)
        {
            return;
        }
        PlaySoundFromResource(info.soundGuangdonghua);
    }


    //#region AD


    // public override void AdVideoDidFail(string str)
    // {
    //     ShowAdVideoFailAlert();
    // }



}
