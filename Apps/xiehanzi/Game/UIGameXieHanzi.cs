using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Moonma.Share;
//https://play.google.com/store/apps/details?id=com.moonma.xiehanzi



public class ColorItemInfo : ItemInfo
{
    public List<Color> listColor;
    public string name;
    public string picmask;
    public string colorJson;
    public Vector2 pt;
    public Color colorOrigin;//填充前原来颜色
    public Color colorFill;//当前填充颜色
    public Color colorMask;
    public Color32 color32Fill;
    public string fileSave;
    public string fileSaveWord;
    public string addtime;
    public string date;
    public Rect rectFill;
}

public class UIGameXieHanzi : UIGameBase
{
    public const int GAME_MODE_NORMAL = 0;
    public const int GAME_MODE_FREE_WRITE = 1;

    public const int TAG_ITEM_LOCK = -1;
    public const int TAG_ITEM_UNLOCK = 0;
    public const string GAME_RES_DIR = Common.GAME_RES_DIR + "/image";

    // public const int BIHUA_POINT_PIC_BASE_WIDTH = 1024;
    // public const int BIHUA_POINT_PIC_BASE_HEIGHT = 768;
    public const float FADE_ANIMATE_TIME = 1.0f;

    public float letterImageZ = -20f;
    static public int rowGame = 0;
    static public int colGame = 0;
    public Camera cameraWordWrite;
    public GameObject objSpriteWordWriteBg;
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

    MeshPaint meshPaintPrefab;
    public MeshPaint meshPaint;

    public WordItemInfo infoFreeWrite;
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
    List<GameObject> listObjBihua;
    List<GameObject> listObjBihuaGuide;
    int indexBihua;
    int indexBihuaPoint;
    long tickCapture;

    int offsetTopbarY;
    int heightTopbar;
    float heightTopbarWorld;
    float itemPosZ;

    AudioClip audioClipBlockFinish;

    public WordWrite wordWritePrefab;
    WordWrite wordWrite;
    int indexWordWrite = 0;

    float scaleLetter;
    Vector2 texSizeLetter;
    Bounds boundsLetter;
    Dictionary<string, object> dicRoot;

    WordWriteMode writeModeCur;
    WordWriteMode writeModePre;//
    GameObject objLetter;
    GameObject objLetterBihua;
    float guideImageOffsetX;
    float guideImageOffsetY;
    Vector2 sizeGuideImage;

    Rect rectWordWrite;

    bool isTouchDownForWrite;
    bool isShowBihuaImage = false;
    int adHeightScreen = 0;
    bool isOnTimerWriteFail = false;

    float widthImage;
    float heightImage;
    float drawLineWidth = 0.5f;

    static public string strSaveWordShotDir//字截图保存目录
    {
        get
        {
            return Application.temporaryCachePath + "/word";
        }
    }

    Rect _imageRectOfLetter = Rect.zero;
    Rect imageRectOfLetter //tex坐标
    {
        get
        {
            if (_imageRectOfLetter.Equals(Rect.zero))
            {
                SpriteRenderer objSR = objLetter.GetComponent<SpriteRenderer>();
                Texture2D tex = objSR.sprite.texture;
                //_imageRectOfLetter = TextureUtil.GetRectNotAlpha(tex);
                _imageRectOfLetter = new Rect(0, 0, tex.width, tex.height);

            }
            return _imageRectOfLetter;
        }
    }


    void Awake()
    {
        Debug.Log("GameXieHanzi awake");
        adHeightScreen = (int)(128 * AppCommon.scaleBase);
        isShowBihuaImage = false;
        isSavingImage = false;
        saveImageIndex = 0;
        isAddSaveImageBg = false;
        isOnTimerWriteFail = false;
        listObjBihua = new List<GameObject>();

        uiToolBar.uiGameXieHanzi = this;


        ParseGuanka();
        LoadPrefab();
        if (gameMode == GAME_MODE_FREE_WRITE)
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

        AppSceneBase.main.AddObjToMainWorld(objSpriteWordWriteBg.gameObject);
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);
        UpdateColorSelect();
    }
    // Use this for initialization
    void Start()
    {
        listTexTure = new List<Texture2D>();
        languageGame = new Language();
        string fileName = Common.GAME_RES_DIR + "/language/language.csv";
        languageGame.Init(fileName);
        languageGame.SetLanguage(Language.main.GetLanguage());
        StartGame();

        InitUI();
    }


    // Update is called once per frame
    void Update()
    {


        // mobile touch
        if ((Input.touchCount > 0))
        {
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    onTouchDown();
                    break;

                case TouchPhase.Moved:
                    onTouchMove();
                    break;

                case TouchPhase.Ended:
                    onTouchUp();
                    break;

            }
        }


        //pc mouse
        //#if UNITY_EDITOR
        if ((Input.touchCount == 0))
        {
            if (Input.GetMouseButtonUp(0))
            {
                onTouchUp();
            }



            if (Input.GetMouseButtonDown(0))
            {
                //  Debug.Log("Input:" + Input.mousePosition);
                onTouchDown();
            }


            if (Input.GetMouseButton(0))
            {
                onTouchMove();
            }

        }


        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
 
    }

    void LoadPrefab()
    {

        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Game/UIColorBoard");
            if (obj != null)
            {
                uiColorBoardPrefab = obj.GetComponent<UIColorBoard>();
                uiColorBoard = (UIColorBoard)GameObject.Instantiate(uiColorBoardPrefab);
                uiColorBoard.gameObject.SetActive(false);
                RectTransform rctranPrefab = uiColorBoardPrefab.transform as RectTransform;

                AppSceneBase.main.AddObjToMainCanvas(uiColorBoard.gameObject);
                //uiColorBoard.transform.SetParent(this.controller.objController.transform);
                //uiColorInput.transform.SetParent(this.controller.objController.transform);
                uiColorBoard.transform.localScale = new Vector3(1f, 1f, 1f);

                RectTransform rctran = uiColorBoard.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;

                uiColorBoard.callBackClick = OnUIColorBoardDidClick;

            }
        }
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Game/UIColorInput");
            if (obj != null)
            {
                uiColorInputPrefab = obj.GetComponent<UIColorInput>();
                uiColorInput = (UIColorInput)GameObject.Instantiate(uiColorInputPrefab);
                uiColorInput.gameObject.SetActive(false);

                RectTransform rctranPrefab = uiColorInputPrefab.transform as RectTransform;
                Debug.Log("uiColorInputPrefab :offsetMin=" + rctranPrefab.offsetMin + " offsetMax=" + rctranPrefab.offsetMax);


                AppSceneBase.main.AddObjToMainCanvas(uiColorInput.gameObject);
                //uiColorInput.transform.SetParent(this.controller.objController.transform);

                uiColorInput.transform.localScale = new Vector3(1f, 1f, 1f);

                RectTransform rctran = uiColorInput.transform as RectTransform;
                Debug.Log("uiColorInput 1:offsetMin=" + rctran.offsetMin + " offsetMax=" + rctran.offsetMax);
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;
                Debug.Log("uiColorInput 2:offsetMin=" + rctran.offsetMin + " offsetMax=" + rctran.offsetMax);
                uiColorInput.callBackUpdateColor = OnUIColorInputUpdateColor;
            }
        }

        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Game/UILineSetting");
            if (obj != null)
            {
                uiLineSettingPrefab = obj.GetComponent<UILineSetting>();
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
        if (gameMode == GAME_MODE_FREE_WRITE)
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Game/MeshPaint");
            if (obj != null)
            {
                meshPaintPrefab = obj.GetComponent<MeshPaint>();
                meshPaint = (MeshPaint)GameObject.Instantiate(meshPaintPrefab);
                AppSceneBase.main.AddObjToMainWorld(meshPaint.gameObject);
            }
        }



        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Game/WordWrite");
            wordWritePrefab = obj.GetComponent<WordWrite>();
        }
    }

    void onTouchDown()
    {
        isTouchDownForWrite = false;
        if (gameMode == GAME_MODE_NORMAL)
        {
            if ((writeModeCur == WordWriteMode.WriteWithNone) || (writeModeCur == WordWriteMode.WriteWithOneWord) || (writeModeCur == WordWriteMode.WriteWithOneBihua))
            {
                if (!isOnTimerWriteFail)
                {
                    Vector2 pos = Common.GetInputPosition();
                    Vector2 posword = mainCam.ScreenToWorldPoint(pos);
                    if (!rectWordWrite.Contains(posword))
                    {
                        return;
                    }
                    isTouchDownForWrite = true;
                    CreateObjectWordWrite();
                    wordWrite.ClearDraw();
                }

            }

        }

    }
    void onTouchMove()
    {
        if (!isTouchDownForWrite)
        {
            return;
        }
        if (gameMode == GAME_MODE_NORMAL)
        {
            wordWrite.OnDraw();
        }

    }
    void onTouchUp()
    {
        if (!isTouchDownForWrite)
        {
            return;
        }
        isTouchDownForWrite = false;
        if (gameMode == GAME_MODE_NORMAL)
        {
            CheckBihuaWrite();
        }


    }

    void InitUI()
    {


        uiLineSetting.transform.SetParent(this.controller.objController.transform);
        uiColorBoard.transform.SetParent(this.controller.objController.transform);
        uiColorInput.transform.SetParent(this.controller.objController.transform);

        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        if (heightAdBanner == 0)
        {
            heightAdBanner = (int)(160 * AppCommon.scaleBase);
            heightAdBannerWorld = Common.ScreenToWorldHeight(mainCam, heightAdBanner);
        }

        offsetTopbarY = 128 + 16 * 2;
        heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);
        if (meshPaint != null)
        {
            uiLineSetting.lineWidthPixsel = meshPaint.lineWidthPixsel;
            meshPaint.SetColor(uiToolBar.colorWord);
        }


        UpdateGold();
        if (!Config.main.isHaveShop)
        {
            uiToolBar.imageGoldBg.gameObject.SetActive(false);
        }
        UpdateImageWord();
        UpdateLetterImage();
        //GotoWordWriteMode 先初始化


        GotoWordWriteMode(WordWriteMode.WriteWithOneWord);
        LayOut();

        OnUIDidFinish();
        //LayOutChild();
    }

    void CheckBihuaWrite()
    {
        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }
        if (wordWrite == null)
        {
            return;
        }

        List<object> listBihua = info.listGuidePoint[indexBihua] as List<object>;
        GuideItemInfo infoStart = listBihua[0] as GuideItemInfo;
        GuideItemInfo infoEnd = listBihua[listBihua.Count - 1] as GuideItemInfo;

        bool isWriteFinish = false;
        bool isWriteStart = false;
        bool isWriteEnd = false;
        foreach (Vector3 pt in wordWrite.listPoint)
        {

            {
                Vector2 point = new Vector2(0, 0);
                point.x = infoStart.point.x + guideImageOffsetX;
                point.y = infoStart.point.y + guideImageOffsetY;
                Vector2 pt_world = ImagePoint2World(point);
                float oft = Common.ScreenToWorldWidth(mainCam, 60 * AppCommon.scaleBase);
                float w = sizeGuideImage.x + oft;
                float h = sizeGuideImage.y + oft;
                Rect rc = new Rect(pt_world.x - w / 2, pt_world.y - h / 2, w, h);
                Vector2 ptwrite = new Vector2(pt.x, pt.y);
                if (rc.Contains(ptwrite))
                {
                    isWriteStart = true;
                }
            }

            {
                Vector2 point = new Vector2(0, 0);
                point.x = infoEnd.point.x + guideImageOffsetX;
                point.y = infoEnd.point.y + guideImageOffsetY;
                Vector2 pt_world = ImagePoint2World(point);
                float oft = Common.ScreenToWorldWidth(mainCam, 60 * AppCommon.scaleBase);
                float w = sizeGuideImage.x + oft;
                float h = sizeGuideImage.y + oft;
                Rect rc = new Rect(pt_world.x - w / 2, pt_world.y - h / 2, w, h);
                Vector2 ptwrite = new Vector2(pt.x, pt.y);
                if (rc.Contains(ptwrite))
                {
                    isWriteEnd = true;
                }
            }
        }

        if (isWriteStart && isWriteEnd)
        {
            isWriteFinish = true;

        }

        if (isWriteFinish)
        {
            //书写正确 
            indexBihua++;
            Debug.Log("WriteFinish:indexBihua=" + indexBihua + " total=" + info.listImageBihua0.Count);
            if (indexBihua >= info.listImageBihua0.Count)
            {
                //所有笔画写完
                indexBihua = 0;
                Debug.Log("next mode");
                GotoNextWordWriteMode();
            }
            else
            {
                //写下一笔
                Debug.Log("next bihua");
                UpdateLetterBihuaGuide();
            }

        }
        else
        {
            //书写失败
            float t = 0.15f;
            wordWrite.totalPoint = wordWrite.listPoint.Count;
            InvokeRepeating("OnTimerWriteFail", t, t);
            isOnTimerWriteFail = true;
        }
    }


    public override void LayOut()
    {
        float x, y, w, h, z;
        float scale = 0; float scalex = 0; float scaley = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        Vector2 worldSize = Common.GetWorldSize(mainCam);
        float oft_top = Common.ScreenToWorldHeight(mainCam, Device.offsetTop);
        float oft_bottom = Common.ScreenToWorldHeight(mainCam, Device.offsetBottom);
        float oft_left = Common.ScreenToWorldHeight(mainCam, Device.offsetLeft);
        float oft_right = Common.ScreenToWorldHeight(mainCam, Device.offsetRight);
        float topbar_height = Common.CanvasToWorldHeight(mainCam, sizeCanvas, 160);


        offsetTopbarY = 160;
        heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);
        heightAdBannerWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);
        heightAdBanner = (int)Common.WorldToScreenHeight(mainCam, heightAdBannerWorld);


        //写字区域
        {
            SpriteRenderer spRender = objSpriteWordWriteBg.GetComponent<SpriteRenderer>();
            Texture2D tex2d = spRender.sprite.texture;
            Vector2 spsize = new Vector2(tex2d.width / 100f, tex2d.height / 100f);
            z = letterImageZ + 1;

            z = letterImageZ + 1;
            float disp_w = 0, disp_h = 0;
            float ratio = 0.8f;

            if (Device.isLandscape)
            {
                disp_w = (Common.GetCameraWorldSizeWidth(mainCam)) * ratio;
                float oft_y = Mathf.Max(heightTopbarWorld, heightAdBannerWorld) + oft_bottom;
                disp_h = (mainCam.orthographicSize * 2 - oft_y * 2) * ratio;
                x = Common.GetCameraWorldSizeWidth(mainCam) / 2;
                y = 0;

                Debug.Log("disp_w=" + disp_w + " disp_h=" + disp_h + " spsize=" + spsize);
            }
            else
            {
                ratio = 0.9f;
                disp_w = (Common.GetCameraWorldSizeWidth(mainCam) * 2) * ratio;

                float oft_y = Mathf.Max((topbar_height + oft_top), (heightAdBannerWorld * 2 + oft_bottom));
                disp_h = (mainCam.orthographicSize * 2 - oft_y * 2) * ratio;
                x = 0;
                y = 0;/// -mainCam.orthographicSize / 2;
            }
            if (GAME_MODE_FREE_WRITE == gameMode)
            {
                ratio = 0.9f;
                disp_w = (worldSize.x - Mathf.Max(oft_left, oft_right) * 2) * ratio;
                float oft_y = Mathf.Max((topbar_height + oft_top), (heightAdBannerWorld + oft_bottom));
                disp_h = (worldSize.y - oft_y * 2) * ratio;
                x = 0;
                y = 0;
            }
            scalex = disp_w / spsize.x;
            scaley = disp_h / spsize.y;
            scale = Mathf.Min(scalex, scaley);
            objSpriteWordWriteBg.transform.localScale = new Vector3(scale, scale, 1f);
            objSpriteWordWriteBg.transform.position = new Vector3(x, y, z);
            if (GAME_MODE_FREE_WRITE == gameMode)
            {
                Rect rc = new Rect(spRender.bounds.center.x - spRender.bounds.size.x / 2, spRender.bounds.center.y - spRender.bounds.size.y / 2, spRender.bounds.size.x, spRender.bounds.size.y);
                meshPaint.UpdateRectPaint(rc);
            }

        }



        //update Letter
        if (objLetter != null)
        {
            SpriteRenderer spRender = objSpriteWordWriteBg.GetComponent<SpriteRenderer>();
            w = spRender.bounds.size.x;
            h = spRender.bounds.size.y;
            x = spRender.bounds.center.x - w / 2;
            y = spRender.bounds.center.y - h / 2;
            // x = spRender.bounds.center.x
            SpriteRenderer objSR = objLetter.GetComponent<SpriteRenderer>();
            Rect rcImage = imageRectOfLetter;//GetImageRect();

            float tex_w = objSR.sprite.texture.width;
            float tex_h = objSR.sprite.texture.height;
            float ratio = 0.8f;
            float image_world_x = rcImage.size.x / 100;
            float image_world_y = rcImage.size.y / 100;
            scalex = w * ratio / (image_world_x);
            scaley = h * ratio / (image_world_y);
            scale = Mathf.Min(scalex, scaley);
            Debug.Log("rcImage=" + rcImage + " scalex=" + scalex + " scaley=" + scaley);
            // scale = 0.5f;
            objLetter.transform.localScale = new Vector3(scale, scale, 1f);
            float oft_x_image = (rcImage.center.x - tex_w / 2) * scale / 100;
            float oft_y_image = (rcImage.center.y - tex_h / 2) * scale / 100;
            x = objSpriteWordWriteBg.transform.position.x - (oft_x_image);
            y = spRender.bounds.center.y - (oft_y_image);

            // RectTransform rctran = objLetter.GetComponent<RectTransform>();
            // rctran.pivot = new Vector2(rcImage.center.x / tex_w, 0.5f);
            // x = objSpriteWordWriteBg.transform.position.x;
            // y = objSpriteWordWriteBg.transform.position.y;
            objLetter.transform.position = new Vector3(x, y, letterImageZ - 1);

            drawLineWidth = 0.5f * ratio;
            if (!Device.isLandscape)
            {
                drawLineWidth = drawLineWidth * (1536f / 2048f) * 0.8f;
            }
            // w = image_world_x * scale;
            // h = image_world_y * scale;
            w = spRender.bounds.size.x;
            h = spRender.bounds.size.y;
            x = spRender.bounds.center.x - w / 2;
            y = spRender.bounds.center.y - h / 2;
            rectWordWrite = new Rect(x, y, w, h);

            boundsLetter = objLetter.GetComponent<SpriteRenderer>().bounds;
        }

        //word image
        {

            RectTransform rctran = imageWord.GetComponent<RectTransform>();
            SpriteRenderer spRender = objSpriteWordWriteBg.GetComponent<SpriteRenderer>();
            if (Device.isLandscape)
            {
                float x_left = spRender.bounds.center.x - spRender.bounds.size.x / 2;
                float x_canvas = Common.WorldToCanvasPoint(mainCam, sizeCanvas, new Vector2(x_left, 0)).x;
                Debug.Log("x_left=" + x_left + " x_canvas=" + x_canvas + " sizeCanvas=" + sizeCanvas);
                x = (-sizeCanvas.x / 2 + (x_canvas - sizeCanvas.x / 2)) / 2;
                y = 0;
                w = sizeCanvas.x / 4;
                h = w;
                imageWord.gameObject.SetActive(true);
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
                y = -sizeCanvas.y / 2 + Common.ScreenToCanvasHeigt(sizeCanvas, heightAdBanner) + h / 2 + ofty;
            }
            else
            {
                x = 0;
                // y = 0;
                h = rctran.rect.size.y;
                y = -sizeCanvas.y / 2 + Common.ScreenToCanvasHeigt(sizeCanvas, heightAdBanner) + h / 2 + ofty;
            }
            rctran.anchoredPosition = new Vector2(x, y);

        }

    }

    public void OnUIColorBoardDidClick(UIColorBoard ui, UIColorBoardCellItem item, bool isOutSide)
    {
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
        meshPaint.SetLineWidthPixsel(width);

    }
    void UpdateColorSelect()
    {
        uiToolBar.btnColorInput.GetComponent<Image>().color = uiToolBar.colorWord;
        if (meshPaint != null)
        {
            meshPaint.SetColor(uiToolBar.colorWord);
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
        if (gameMode == GAME_MODE_FREE_WRITE)
        {
            return;
        }
        WordItemInfo info = GetItemInfo();
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
        ParseGuanka();
        ItemInfo info = listGuanka[lv] as ItemInfo;
        return info.pic;
    }

    static public string GetHistorySortCellItemBg()
    {
        string strDirRoot = GAME_RES_DIR;
        string ret = strDirRoot + "/common/historyBubbleRed_up@2x~ipad.png";
        return ret;
    }
    static public string GetItemThumb(string id)
    {
        string strDirRoot = Common.GAME_RES_DIR;
        string strDirRootImage = strDirRoot + "/image/" + id;
        //thumb
        string ret = strDirRootImage + "/" + id + "_thumb.png";

        return ret;
    }

    // public override int GetPlaceTotal()
    // {
    //     return 4;
    // }

    public override int GetGuankaTotal()
    {
        ParseGuanka();
        if (listGuanka != null)
        {
            return listGuanka.Count;
        }
        return 0;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
    }

    public override int ParseGuanka()
    {
        int count = 0;
        Debug.Log("ParseGuanka2 start");
        long tickGuanka = Common.GetCurrentTimeMs();
        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }
        listGuanka = new List<object>();
        int idx = GameManager.placeLevel;
        long tickJson = Common.GetCurrentTimeMs();
        string fileName = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            WordItemInfo info = new WordItemInfo();
            info.id = (string)item["id"];
            info.icon = GetItemThumb(info.id);

            listGuanka.Add(info);
        }
        tickJson = Common.GetCurrentTimeMs() - tickJson;
        count = listGuanka.Count;

        WordItemInfo infonow = GetItemInfo();
        long tickItem = Common.GetCurrentTimeMs();
        //ParserGuankaItem2(infonow);
        tickItem = Common.GetCurrentTimeMs() - tickItem;

        tickGuanka = Common.GetCurrentTimeMs() - tickGuanka;
        Debug.Log("ParseGuanka2: tickGuanka=" + tickGuanka + " tickJson=" + tickJson + " tickItem=" + tickItem);
        return count;
    }

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
        if (gameMode == GAME_MODE_FREE_WRITE)
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


    Rect GetImageRect()
    {
        float x, y, w, h;
        float pt_image_left = widthImage;//BIHUA_POINT_PIC_BASE_WIDTH;
        float pt_image_right = 0;
        float pt_image_top = heightImage;//BIHUA_POINT_PIC_BASE_HEIGHT;
        float pt_image_bottom = 0;

        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            // return;
        }
        foreach (List<object> listPoint in info.listDemoPoint)
        {

            foreach (Vector2 pt in listPoint)
            {
                if (pt.x < pt_image_left)
                {
                    pt_image_left = pt.x;
                }
                if (pt.x > pt_image_right)
                {
                    pt_image_right = pt.x;
                }

                if (pt.y < pt_image_top)
                {
                    pt_image_top = pt.y;
                }

                if (pt.y > pt_image_bottom)
                {

                    pt_image_bottom = pt.y;
                }

            }

        }

        x = pt_image_left;
        w = pt_image_right - pt_image_left;
        y = heightImage - pt_image_bottom;//BIHUA_POINT_PIC_BASE_HEIGHT
        h = pt_image_bottom - pt_image_top;

        return new Rect(x, y, w, h);
    }


    // {621.14, 146.24} to Vector2
    public Vector2 WordString2Point(string str)
    {
        float x, y;
        string strtmp = str.Substring(str.IndexOf("{") + 1);
        int idx = strtmp.IndexOf(",");
        string strx = strtmp.Substring(0, idx);
        float.TryParse(strx, out x);

        string strtmpy = strtmp.Substring(idx + 1);
        idx = strtmpy.IndexOf("}");
        string stry = strtmpy.Substring(0, idx);
        float.TryParse(stry, out y);

        return new Vector2(x, y);
    }


    void DestroyLetterBihuaGuide()
    {
        if (listObjBihuaGuide == null)
        {
            return;
        }

        StopCoroutine("ShowGuideImageFadeAnimate");

        foreach (GameObject obj in listObjBihuaGuide)
        {
            iTween.Stop(obj);
            GameObject.DestroyImmediate(obj);
        }
        listObjBihuaGuide.Clear();

    }
    //提示图片
    void UpdateLetterBihuaGuide()
    {
        if (gameMode == GAME_MODE_FREE_WRITE)
        {
            return;
        }
        float x, y, w, h;
        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }
        DestroyLetterBihuaGuide();
        List<object> listBihua = info.listGuidePoint[indexBihua] as List<object>;
        float z = letterImageZ - 2;
        int idx = 0;
        int animate_count = 0;
        foreach (GuideItemInfo itemInfo in listBihua)
        {
            string strPic = "";
            string strDirRoot = GAME_RES_DIR + "/common";
            switch (itemInfo.type)
            {
                case GuideItemInfo.IMAGE_TYPE_START:
                    strPic = strDirRoot + "/writing_guide_point_start@2x~ipad.png";
                    //
                    break;
                case GuideItemInfo.IMAGE_TYPE_MIDDLE_ANIMATE:
                    strPic = strDirRoot + "/writing_guide_point_animating@2x~ipad.png";
                    break;
                case GuideItemInfo.IMAGE_TYPE_MIDDLE:
                    strPic = strDirRoot + "/writing_guide_point_middle@2x~ipad.png";

                    break;


                case GuideItemInfo.IMAGE_TYPE_END:
                    strPic = strDirRoot + "/writing_guide_point_end@2x~ipad.png";
                    z--;
                    //writing_guide_point_end@2x~ipad
                    break;

                default:
                    strPic = strDirRoot + "/writing_guide_point_animating@2x~ipad.png";
                    break;

            }

            Debug.Log("guide image type:" + itemInfo.type);
            //Texture2D tex = (Texture2D)Resources.Load(strPic);
            Texture2D tex = LoadTexture.LoadFromAsset(strPic);
            w = tex.width / 2;
            h = tex.height / 2;
            // if(itemInfo.type==GuideItemInfo.IMAGE_TYPE_MIDDLE)
            // {
            //     //偏移量按writing_guide_point_middle 大小计算
            //     w = 90/2;
            //     h = 90/2;
            // }
            listTexTure.Add(tex);
            guideImageOffsetX = w / 2;
            guideImageOffsetY = h / 2;
            Vector2 point = new Vector2(0, 0);
            point.x = itemInfo.point.x + guideImageOffsetX;
            point.y = itemInfo.point.y + guideImageOffsetY;
            Vector2 pt_world = ImagePoint2World(point);

            Vector3 pt = new Vector3(pt_world.x, pt_world.y, z);//mainCam.ScreenToWorldPoint(ptscreen);
            pt.z = z;

            if (!Device.isLandscape)
            {

            }
            GameObject obj = new GameObject("guide" + idx);
            AppSceneBase.main.AddObjToMainWorld(obj);
            if (listObjBihuaGuide == null)
            {
                listObjBihuaGuide = new List<GameObject>();
            }
            listObjBihuaGuide.Add(obj);
            obj.AddComponent<RectTransform>();
            obj.AddComponent<SpriteRenderer>();
            RectTransform rcTran = obj.GetComponent<RectTransform>();
            SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();


            w = tex.width;
            h = tex.height;
            x = 0;
            y = 0;

            Sprite sprite = Sprite.Create(tex, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));
            sprite.name = "item";
            objSR.sprite = sprite;
            float scale = objLetter.transform.localScale.x;//0.8f;
            if (!Device.isLandscape)
            {
                //scale = scale*1536/2048;
            }
            obj.transform.localScale = new Vector3(scale, scale, 1f);
            obj.transform.rotation = Quaternion.Euler(0, 0, itemInfo.angle);
            obj.transform.position = new Vector3(pt.x, pt.y, z);
            //obj.transform.position = new Vector3(pt.x+objSR.bounds.size.x/2, pt.y-objSR.bounds.size.y/2, -20f);
            sizeGuideImage = objSR.bounds.size;


            if (itemInfo.type == GuideItemInfo.IMAGE_TYPE_MIDDLE_ANIMATE)
            {
                obj.SetActive(false);
                //Invoke("ShowGuideImageFadeAnimate",FADE_ANIMATE_TIME*animate_count);
                //StartCoroutine(ShowGuideImageFadeAnimate(obj,FADE_ANIMATE_TIME*animate_count));
                ItemInfo info_coroutine = new ItemInfo();
                info_coroutine.obj = obj;
                info_coroutine.time = FADE_ANIMATE_TIME * animate_count;
                StartCoroutine("ShowGuideImageFadeAnimate", info_coroutine);
                animate_count++;

            }

            idx++;
        }
    }

    //  Sprite *sp = (Sprite *)node;
    // auto action =   RepeatForever::create( Sequence::create(FadeIn::create(FADE_ANIMATE_TIME),FadeOut::create(FADE_ANIMATE_TIME), NULL));
    //  sp->setVisible(true);
    // sp->runAction(action);


    //如果想要传递参数，并且实现延迟调用，可以考虑采用Coroutine
    IEnumerator ShowGuideImageFadeAnimate(ItemInfo info)

    {
        float delaySeconds = info.time;
        yield return new WaitForSeconds(delaySeconds);
        if ((listObjBihuaGuide != null) && (listObjBihuaGuide.Count > 0))
        {
            GameObject obj = info.obj;
            if (obj != null)
            {
                obj.SetActive(true);
                //淡入淡出动画
                iTween.FadeTo(obj, iTween.Hash("alpha", 0, "time", FADE_ANIMATE_TIME, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong));
            }

        }

    }

    void ShowLetterBihuaImage(bool isShow)
    {

        isShowBihuaImage = isShow;

        float x, y, w, h, z;

        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }

        if (objLetterBihua == null)
        {
            objLetterBihua = new GameObject("letterBihua");
            objLetterBihua.AddComponent<RectTransform>();
            objLetterBihua.AddComponent<SpriteRenderer>();
        }
        RectTransform rcTran = objLetterBihua.GetComponent<RectTransform>();
        SpriteRenderer objSR = objLetterBihua.GetComponent<SpriteRenderer>();
        string strPic = info.imageBihua;
        objLetterBihua.SetActive(isShow);

        if (isShow)
        {
            //Texture2D tex = (Texture2D)Resources.Load(strPic);
            Texture2D tex = LoadTexture.LoadFromAsset(strPic);
            listTexTure.Add(tex);

            w = tex.width;
            h = tex.height;
            //  w  = tex.width;
            // h =  tex.height;
            Debug.Log("tex,w:" + w + " h:" + h);
            x = 0;
            y = 0;

            Sprite sprite = Sprite.Create(tex, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));
            sprite.name = "item";
            objSR.sprite = sprite;


            Vector2 spsize = new Vector2(tex.width / 100f, tex.height / 100f);
            Vector2 world_size = rectWordWrite.size;
            float scalex = world_size.x / spsize.x;
            float scaley = world_size.y / spsize.y;
            float scale = Mathf.Min(scalex, scaley);
            objLetterBihua.transform.localScale = new Vector3(scale, scale, 1f);

            x = rectWordWrite.position.x + rectWordWrite.size.x / 2;
            y = rectWordWrite.position.y + rectWordWrite.size.y / 2;
            z = letterImageZ - 10f;

            objLetterBihua.transform.position = new Vector3(x, y, z);
        }

    }
    void UpdateLetterImage()
    {
        int x, y, w, h;
        if (gameMode == GAME_MODE_FREE_WRITE)
        {
            return;
        }
        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }

        if (objLetter == null)
        {
            objLetter = new GameObject("letter");
            AppSceneBase.main.AddObjToMainWorld(objLetter);
            objLetter.AddComponent<RectTransform>();
            objLetter.AddComponent<SpriteRenderer>();
        }
        RectTransform rcTran = objLetter.GetComponent<RectTransform>();
        SpriteRenderer objSR = objLetter.GetComponent<SpriteRenderer>();
        string strPic = info.pic;
        objLetter.SetActive(true);
        if (gameMode == GAME_MODE_FREE_WRITE)
        {
            objLetter.SetActive(false);
        }

        switch (writeModeCur)
        {
            case WordWriteMode.WriteWithOneWord:
                {
                    strPic = info.pic;
                }
                break;
            case WordWriteMode.WriteWithOneBihua:
                {
                    strPic = info.listImageBihua0[indexBihua];
                }
                break;
            case WordWriteMode.WriteWithNone:
                {
                    objLetter.SetActive(false);
                    return;
                }

                break;


        }
        Debug.Log(strPic);
        //Texture2D tex = (Texture2D)Resources.Load(strPic);
        Texture2D tex = LoadTexture.LoadFromAsset(strPic);
        if (writeModeCur == WordWriteMode.WriteWithOneWord)
        {
            widthImage = tex.width;
            heightImage = tex.height;

        }
        if (writeModeCur == WordWriteMode.WriteWithOneBihua)
        {
            if (tex == null)
            {
                strPic = info.listImageBihua1[indexBihua];
                Debug.Log(strPic);
                //tex = (Texture2D)Resources.Load(strPic);
                tex = LoadTexture.LoadFromAsset(strPic);
            }
        }
        listTexTure.Add(tex);

        w = tex.width;
        h = tex.height;
        //  w  = tex.width;
        // h =  tex.height;
        Debug.Log("tex,w:" + w + " h:" + h);
        x = 0;
        y = 0;
        texSizeLetter = new Vector2(w, h);
        Sprite sprite = Sprite.Create(tex, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));
        sprite.name = "item";
        objSR.sprite = sprite;



    }

    void DestroyObjectWordWrite()
    {
        indexWordWrite = 0;
        foreach (GameObject obj in listObjBihua)
        {
            GameObject.DestroyImmediate(obj);
        }
        listObjBihua.Clear();

    }
    void CreateObjectWordWrite()
    {

        wordWrite = (WordWrite)GameObject.Instantiate(wordWritePrefab);
        wordWrite.gameObject.name = "WordWrite" + indexWordWrite;
        AppSceneBase.main.AddObjToMainWorld(wordWrite.gameObject);
        wordWrite.mainCamera = mainCam;
        wordWrite.rectDraw = rectWordWrite;
        wordWrite.setDrawLineWidth(drawLineWidth);
        wordWrite.setColor(uiToolBar.colorWord);
        indexWordWrite++;
        wordWrite.transform.position = new Vector3(0, 0, letterImageZ - 1);
        listObjBihua.Add(wordWrite.gameObject);
    }



    void GotoNextWordWriteMode()
    {
        if (writeModeCur == WordWriteMode.WriteWithOneWord)
        {
            GotoWordWriteMode(WordWriteMode.WriteWithOneBihua);
        }
        else if (writeModeCur == WordWriteMode.WriteWithOneBihua)
        {
            GotoWordWriteMode(WordWriteMode.WriteWithNone);
        }
        else if (writeModeCur == WordWriteMode.WriteWithNone)
        {
            //练字完成 准备写下一个汉字
            ShowGameWin();
        }
    }

    void ButtonSetImage(Button btn, string file)
    {
        btn.GetComponent<Image>().sprite = LoadTexture.CreateSprieFromResource(file);
    }
    public void GotoWordWriteMode(WordWriteMode mode)
    {
        writeModeCur = mode;

        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }
        switch (mode)
        {
            case WordWriteMode.WriteDemo:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                    StopWordWriteDemo();
                    DestroyObjectWordWrite();
                    DestroyLetterBihuaGuide();
                    if (isShowBihuaImage)
                    {
                        ShowLetterBihuaImage(false);
                    }
                    float t = 0.2f;
                    indexBihua = 0;
                    indexBihuaPoint = 0;
                    InvokeRepeating("OnTimerDemo", t, t);

                }
                break;

            case WordWriteMode.WriteWithOneWord:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_SEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                    DestroyObjectWordWrite();
                    StopWordWriteDemo();
                    UpdateLetterImage();
                    UpdateLetterBihuaGuide();
                    if (isShowBihuaImage)
                    {
                        ShowLetterBihuaImage(false);
                    }
                }
                break;
            case WordWriteMode.WriteWithOneBihua:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_SEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                    DestroyObjectWordWrite();
                    StopWordWriteDemo();
                    UpdateLetterImage();
                    UpdateLetterBihuaGuide();
                    if (isShowBihuaImage)
                    {
                        ShowLetterBihuaImage(false);
                    }
                }
                break;
            case WordWriteMode.WriteWithNone:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_SEL);
                    DestroyObjectWordWrite();
                    StopWordWriteDemo();
                    UpdateLetterImage();
                    UpdateLetterBihuaGuide();
                    if (isShowBihuaImage)
                    {
                        ShowLetterBihuaImage(false);
                    }
                }
                break;

            case WordWriteMode.ShowBihua:
                {
                    ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                    //DestroyObjectWordWrite();
                    //StopWordWriteDemo();
                    //UpdateLetterImage();
                    //UpdateLetterBihuaGuide();
                    ShowLetterBihuaImage(!isShowBihuaImage);

                }
                break;

            default:
                break;


        }


    }

    void StopWordWriteDemo()
    {
        if (IsInvoking("OnTimerDemo"))
        {
            CancelInvoke("OnTimerDemo");
        }
    }

    Vector2 ImagePoint2World(Vector2 pt)
    {

        Vector2 point = pt;
        //文件读取的点坐标是居于1024x768图片的坐标

        float pic_w = widthImage;
        float pic_h = heightImage;

        float ratio_x = (point.x) * 1.0f / pic_w;//范围0-1f
        float x = ratio_x * boundsLetter.size.x;

        //Debug.Log("boundsLetter.center:"+boundsLetter.center+" boundsLetter.size:"+boundsLetter.size);
        //字的左边界坐标
        float letter_x_left = boundsLetter.center.x - boundsLetter.size.x / 2;//(Common.GetCameraWorldSizeWidth(mainCam) * 2 - boundSizeLetter.x) / 2
        x = letter_x_left + x;

        float ratio_y = (pic_h - point.y) * 1.0f / pic_h;//范围0-1f
        //if (imageScaleFactor == 1f)
        {
            //坐标原点在底部
            ratio_y = (point.y) * 1.0f / pic_h;
        }

        float y = ratio_y * boundsLetter.size.y;

        //字的底边界坐标
        float letter_y_bottom = boundsLetter.center.y - boundsLetter.size.y / 2;//(mainCam.orthographicSize * 2 - boundSizeLetter.y) / 2;
        y = letter_y_bottom + y;
        return new Vector2(x, y);
    }

    void OnTimerWriteFail()
    {
        if (wordWrite)
        {
            wordWrite.OnDrawFail();
            if (wordWrite.listPoint.Count == 0)
            {
                CancelInvoke("OnTimerWriteFail");
                isOnTimerWriteFail = false;
                if (wordWrite != null)
                {
                    Destroy(wordWrite.gameObject);
                    indexWordWrite--;
                    if (indexWordWrite < 0)
                    {
                        indexWordWrite = 0;
                    }
                }

            }
        }
    }

    void OnTimerDemo()
    {

        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }


        List<object> listBihua = info.listDemoPoint[indexBihua] as List<object>;
        if (indexBihuaPoint == 0)
        {
            //一个笔画
            CreateObjectWordWrite();
        }


        //保证画每个笔画的总时间一致
        int one_bihua_draw_count = 5;

        int point_step_min = 1;
        int point_step = listBihua.Count / one_bihua_draw_count;
        if (point_step < point_step_min)
        {
            point_step = point_step_min;
        }
        //foreach (Vector2 point in listBihua)
        for (int i = 0; i < point_step; i++)
        {
            Debug.Log("i=" + i + " listBihua.count=" + listBihua.Count + " indexBihuaPoint=" + indexBihuaPoint);
            Vector2 point = (Vector2)listBihua[indexBihuaPoint];
            //文件读取的点坐标是居于1024x768图片的坐标
            float z;
            z = letterImageZ;

            Vector2 pt_world = ImagePoint2World(point);

            Vector3 pt = new Vector3(pt_world.x, pt_world.y, z);//mainCam.ScreenToWorldPoint(ptscreen);
            pt.z = z;

            //Debug.Log("AddPoint:" + pt + " ptscreen=" + ptscreen + " point" + point + " screen:" + Screen.width + " " + Screen.height);
            // pt.y -=17;
            // pt.x -=8f;
            wordWrite.AddPoint(pt);
            indexBihuaPoint++;
            if (indexBihuaPoint >= listBihua.Count)
            {
                //下一笔画
                indexBihuaPoint = 0;
                indexBihua++;
                break;
            }
        }

        wordWrite.DrawLine();



        // wordWrite.transform.localScale = new Vector3(scaleLetter,scaleLetter,1f);

        if (indexBihua >= info.listDemoPoint.Count)
        {
            indexBihua = 0;
            indexBihuaPoint = 0;
            //整个字写完
            CancelInvoke("OnTimerDemo");
        }

    }
    void StartGame()
    {
        WordItemInfo infonow = GetItemInfo();
        long tickItem = Common.GetCurrentTimeMs();
        ParseGuankaItem(infonow);
        tickItem = Common.GetCurrentTimeMs() - tickItem;
        Debug.Log("ParserGuankaItem: tickItem=" + tickItem);
    }


    public override ItemInfo GetGuankaItemInfo(int idx)
    {
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ItemInfo info = listGuanka[idx] as ItemInfo;
        return info;
    }


    public int GetGuankaIndexByWord(WordItemInfo info)
    {
        int idx = 0;
        foreach (WordItemInfo infotmp in listGuanka)
        {
            if (infotmp.id == info.id)
            {
                break;
            }
            idx++;
        }
        return idx;
    }
    public WordItemInfo GetItemInfo()
    {
        if (gameMode == GAME_MODE_FREE_WRITE)
        {
            if (infoFreeWrite == null)
            {
                infoFreeWrite = new WordItemInfo();
            }
            return infoFreeWrite;
        }
        int idx = GameManager.gameLevel;
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        WordItemInfo info = listGuanka[idx] as WordItemInfo;
        return info;
    }

    void CheckGameWin()
    {
        bool isAllItemLock = true;
        Debug.Log("CheckGameWin");
    }

    void ShowGameWin()
    {
        Debug.Log("ShowGameWin");
        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }
        //GameScene.ShowAdInsert(100);


        //remove guide image
        DestroyLetterBihuaGuide();

        Debug.Log("ShowGameWin 1");
        CaptureWordWrite();
        Debug.Log("ShowGameWin 2");
        DBWord.main.AddItem(info);

        WordWriteFinishViewController controller = WordWriteFinishViewController.main;
        controller.Show(null, null);
        controller.ui.callbackClose = OnUIWordWriteFinish;
        controller.ui.gameLevelNow = GameManager.gameLevel;
        controller.ui.UpdateItem(GetItemInfo());
        Debug.Log("ShowGameWin end");
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

    public string GetSavePath()
    {
        WordItemInfo info = GetItemInfo();

        string filedir = UIGameXieHanzi.strSaveWordShotDir;

        //创建文件夹
        Directory.CreateDirectory(filedir);
        string filepath = filedir + "/" + info.id + ".png";
        info.fileSaveWord = filepath;

        return filepath;
    }
    void CaptureWordWrite()
    {
        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }
        float x, y, w, h;

        Vector2 size = new Vector2(Screen.width, Screen.height);


        Rect rc = Common.WorldToScreenRect(mainCam, rectWordWrite);
        string filepath = GetSavePath();

        Debug.Log("CaptureCamera:rect:" + rc);
        Common.CaptureCamera(mainCam, rc, filepath, size);


    }


    public void OnUIWordWriteFinish(UIWordWriteFinish ui)
    {
        GotoWordWriteMode(WordWriteMode.WriteWithOneWord);
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
    public void OnClickGold()
    {

    }

    public override void OnClickBtnBack()
    {
        uiToolBar.OnClickBtnBack();

    }


    public void OnClickBtnDemo()
    {
        GotoWordWriteMode(WordWriteMode.WriteDemo);
    }
    //笔画示意图
    public void OnClickBtnBihua()
    {
        if (!isShowBihuaImage)
        {
            writeModePre = writeModeCur;
        }
        GotoWordWriteMode(WordWriteMode.ShowBihua);
        if (!isShowBihuaImage)
        {
            //隐藏后恢复之前的模式
            writeModeCur = writeModePre;
        }
    }
    public void OnClickBtnPutonghua()
    {
        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }
        PlaySoundFromResource(info.soundPutonghua);
    }
    public void OnClickBtnGuangdonghua()
    {
        WordItemInfo info = GetItemInfo();
        if (info == null)
        {
            return;
        }
        PlaySoundFromResource(info.soundGuangdonghua);
    }


    //#region AD

    public override void AdBannerDidReceiveAd(int w, int h)
    {
        if (h > 0)
        {
            heightAdBanner = h;
            heightAdBannerWorld = Common.ScreenToWorldHeight(mainCam, h);

        }

        Debug.LogFormat("AdBanner h={0}", h);


        LayOut();
    }

    public override void AdVideoDidFail(string str)
    {
        ShowAdVideoFailAlert();
    }

    public override void AdVideoDidStart(string str)
    {

    }
    public override void AdVideoDidFinish(string str)
    {
        //  DoClickBtnColorInput();
    }


}
