using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameCaiCaiLe : UIGameBase, IPopViewControllerDelegate, IUIWordBoardDelegate, IUIWordContentBaseDelegate, IUIWordBarDelegate
{


    public GameObject objTopBar;
    public Button btnHelp;
    public Button btnTips;
    public Button btnRetry;
    public RawImage imageBg;
    public GameObject objLayouBtn;
    public Text textTitle;


    public UIWordFillBox uiWordFillBoxPrefab;
    //public UIWordFillBox uiWordFillBox;

    public UIWordImageText uiWordImageTextPrefab;
    //public UIWordImageText uiWordImageText;
    UIWordContentBase uiWordContent;

    public GameObject objGoldBar;
    public Image imageGoldBg;
    public Text textGold;
    public UIShop uiShopPrefab;
    public UIWordBoard uiWordBoard;
    public UIWordBar uiWordBar;
    string strPlace;
    float goldBaroffsetYNormal;

    GameBase gameBase;
    static public Language languageWord;

    int rowWordBoard = 3;
    int colWordBoard = 8;

    void Awake()
    {
        LoadPrefab();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        gameBase = this.gameObject.AddComponent<GameBase>();
        if (gameBase == null)
        {
            Debug.Log("gameBase is null");
        }
        UpdateLanguageWord();
        btnTips.gameObject.SetActive(Config.main.isHaveShop);

        if (info.gameType == GameRes.GAME_TYPE_TEXT)
        {
            btnRetry.gameObject.SetActive(true);
        }
        else
        {
            btnRetry.gameObject.SetActive(false);
        }

        btnHelp.gameObject.SetActive(false);
        if (Common.appKeyName == GameRes.GAME_IdiomConnect)
        {
            btnTips.gameObject.SetActive(true);
            btnRetry.gameObject.SetActive(true);
            btnHelp.gameObject.SetActive(true);
        }

        objTopBar.SetActive(AppVersion.appCheckHasFinished);


        RectTransform rctran = objTopBar.GetComponent<RectTransform>();
        //bgs

        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_GAME_BG, true);
        if (objGoldBar != null)
        {
            RectTransform rctranGold = objGoldBar.GetComponent<RectTransform>();
            goldBaroffsetYNormal = rctranGold.offsetMax.y;
            if (!Config.main.isHaveShop)
            {
                objGoldBar.SetActive(false);
            }
        }

        uiWordBar.iDelegate = this;
        //uiWordBoard.wordBar = uiWordBar;
        uiWordBar.callbackGameFinish = OnGameWinFinish;
        uiWordBar.callbackGold = OnNotEnoughGold;

        uiWordBoard.iDelegate = this;

        LanguageManager.main.UpdateLanguage(LevelManager.main.placeLevel);
        UpdateLanguage();
        UpdateBtnMusic();

        switch (info.gameType)
        {
            case GameRes.GAME_TYPE_IMAGE:
                {
                    uiWordBar.gameObject.SetActive(true);
                }
                break;
            case GameRes.GAME_TYPE_IMAGE_TEXT:
                {
                    uiWordBar.gameObject.SetActive(true);
                }
                break;
            case GameRes.GAME_TYPE_TEXT:
                {
                    uiWordBar.gameObject.SetActive(false);
                    if (Common.appKeyName == GameRes.GAME_RIDDLE)
                    {
                        uiWordBar.gameObject.SetActive(true);
                    }
                }
                break;
            case GameRes.GAME_TYPE_CONNECT:
                {
                    uiWordBar.gameObject.SetActive(false);
                }
                break;

        }

        Common.SetButtonText(btnHelp, Language.main.GetString("STR_BTN_HELP"), 64);
        Common.SetButtonText(btnTips, Language.main.GetString("STR_BTN_TIPS"), 64);
        Common.SetButtonText(btnRetry, Language.main.GetString("STR_BTN_Retry"), 64);


        // Common.GetButtonText(btnTips).color = GameRes.main.colorTitle;
        // Common.GetButtonText(btnRetry).color = GameRes.main.colorTitle;

    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
        //OnGameWinFinish(uiWordBar, false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateLanguageWord()
    {
        ItemInfo info = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string strlan = Common.GAME_RES_DIR + "/language/" + info.language + ".csv";
        languageWord = new Language();
        languageWord.Init(strlan);
        languageWord.SetLanguage(SystemLanguage.Chinese);

    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        AppSceneBase.main.ClearMainWorld();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        GameLevelParse.main.ParseItem(info);
        switch (info.gameType)
        {
            case GameRes.GAME_TYPE_CONNECT:
                {
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordFillBoxPrefab);
                }
                break;
            case GameRes.GAME_TYPE_IMAGE:
            case GameRes.GAME_TYPE_TEXT:
            case GameRes.GAME_TYPE_IMAGE_TEXT:
                {
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordImageTextPrefab);
                }
                break;


            default:
                break;
        }
        if (uiWordContent != null)
        {
            uiWordContent.transform.SetParent(this.transform);
            uiWordContent.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(uiWordFillBoxPrefab.gameObject, uiWordContent.gameObject);
            uiWordContent.iDelegate = this;
            uiWordContent.infoItem = info;
            uiWordContent.UpdateGuankaLevel(level);
        }
        // ShowAdInsert(GAME_AD_INSERT_SHOW_STEP,true);
        if (gameBase != null)
        {
            if (gameBase.GetGameItemStatus(info) == GameBase.GAME_STATUS_UN_START)
            {
                gameBase.SetGameItemStatus(info, GameBase.GAME_STATUS_PLAY);
            }

        }

        //  TextureUtil.UpdateImageTexture(imagePicBoard, "App/UI/Game/BoardPic", true);

        objLayouBtn.transform.SetAsLastSibling();

        UpdateWord();
        UpdateTitle();
        UpdateGold();
        LayOut();

        OnUIDidFinish();
    }

    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIWordFillBox");
            if (obj != null)
            {
                uiWordFillBoxPrefab = obj.GetComponent<UIWordFillBox>();
            }
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIWordImageText");
            if (obj != null)
            {
                uiWordImageTextPrefab = obj.GetComponent<UIWordImageText>();
            }
        }
    }

    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w_image = rctran.rect.width;
            float h_image = rctran.rect.height;

            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            print("LayOutChild:" + rctran.rect + "sizeCanvas:" + sizeCanvas + "scale:" + scale);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        float ratio = 1f;
        float topbarHeightCanvas = 160;

        Rect rectImage = Rect.zero;
        //game pic
        {

            ratio = 0.9f;
            if (Device.isLandscape)
            {
                w = (this.frame.width / 2) * ratio;
                x = -this.frame.width / 4 - w / 2;
                if (Common.appKeyName == GameRes.GAME_XIEHOUYU)
                {
                    h = (this.frame.height - topbarHeightCanvas * 3) * ratio;
                    float y1 = -sizeCanvas.y / 2 + topbarHeightCanvas * 2;
                    float y2 = sizeCanvas.y / 2 - topbarHeightCanvas;
                    y = (y1 + y2) / 2 - h / 2;
                }
                else
                {

                    h = (this.frame.height - topbarHeightCanvas * 2) * ratio;
                    y = 0 - h / 2;
                }


            }
            else
            {

                // w = this.frame.width - topbarHeightCanvas * 2;
                w = this.frame.width;
                h = (this.frame.height / 2 - topbarHeightCanvas * 2);
                if (Common.appKeyName == GameRes.GAME_XIEHOUYU)
                {
                    h = (this.frame.height / 2 - topbarHeightCanvas * 3);
                }
                y = this.frame.height / 4 - h / 2;
                x = 0 - w / 2;
            }

            if (Common.appKeyName == GameRes.GAME_IdiomConnect)
            {
                if (Device.isLandscape)
                {
                }
                else
                {
                    w = this.frame.width * 0.9f;
                    h = (this.frame.height / 2);
                    y = this.frame.height / 2 - topbarHeightCanvas - h;
                    x = 0 - w / 2;
                }

            }



            rectImage = new Rect(x, y, w, h);
            Debug.Log("rectImage =" + rectImage);

            if (uiWordContent != null)
            {
                RectTransform rctran = uiWordContent.GetComponent<RectTransform>();
                UIWordFillBox ui = uiWordContent as UIWordFillBox;
                if (ui != null)
                {
                    w = Mathf.Min(w, h);
                    h = w;
                }
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = rectImage.center;
                uiWordContent.LayOut();
            }
        }



        //wordboard
        {




            RectTransform rctran = uiWordBoard.GetComponent<RectTransform>();
            GridLayoutGroup gridLayout = uiWordBoard.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            Vector2 space = gridLayout.spacing;

            if ((info.gameType == GameRes.GAME_TYPE_TEXT) && (Common.appKeyName != GameRes.GAME_RIDDLE))
            {
                gridLayout.cellSize = new Vector2(160, 160);
                gridLayout.spacing = new Vector2(16, 16);

                cellSize = gridLayout.cellSize;
                space = gridLayout.spacing;
                rowWordBoard = 2;
                colWordBoard = 4;

            }
            else
            {
                if (Device.isLandscape)
                {
                    rowWordBoard = 3;
                    colWordBoard = 8;
                }
                else
                {
                    rowWordBoard = 3;
                    colWordBoard = 8;
                    w = (cellSize.x + space.x) * colWordBoard;
                    if (w > this.frame.width)
                    {
                        rowWordBoard = 4;
                        colWordBoard = 6;
                    }

                }
            }

            if (Device.isLandscape)
            {
                float x1 = rectImage.center.x + rectImage.size.x / 2;
                float x2 = this.frame.width / 2;
                x = (x1 + x2) / 2;
                y = -this.frame.height / 4;

                //6x4
                w = (cellSize.x + space.x) * colWordBoard;
                h = (cellSize.y + space.y) * rowWordBoard;
            }
            else
            {
                x = 0;
                y = -this.frame.height / 4;


                //8x3
                w = (cellSize.x + space.x) * colWordBoard;
                h = (cellSize.y + space.y) * rowWordBoard;

            }




            float y_bottom_limite = -sizeCanvas.y / 2 + topbarHeightCanvas + 16;
            if ((y - h / 2) < y_bottom_limite)
            {
                y = y_bottom_limite + h / 2;
            }
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);

            uiWordBoard.row = rowWordBoard;
            uiWordBoard.col = colWordBoard;

        }

        RectTransform rctranWordBar = uiWordBar.GetComponent<RectTransform>();
        RectTransform rctranBoard = uiWordBoard.GetComponent<RectTransform>();
        //wordbar
        {
            ratio = 0.9f;

            RectTransform rctran = uiWordBar.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                x = rctranBoard.anchoredPosition.x;
                float y1 = rctranBoard.anchoredPosition.y + rctranBoard.rect.height / 2;
                float y2 = this.frame.height / 2 - topbarHeightCanvas;
                y = (y1 + y2) / 2;
                w = (this.frame.width / 2 - topbarHeightCanvas * 2) * ratio;
            }
            else
            {
                w = (this.frame.width - topbarHeightCanvas * 2) * ratio;
                x = 0;
                y = 0;

            }


            h = topbarHeightCanvas;
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
        }

        //leftbtn
        {
            RectTransform rctran = objLayouBtn.GetComponent<RectTransform>();
            LayOutGrid lg = objLayouBtn.GetComponent<LayOutGrid>();
            lg.enableHide = false;

            if (info.gameType == GameRes.GAME_TYPE_CONNECT)
            {
                //横排显示
                w = rctranBoard.rect.size.x;
                h = topbarHeightCanvas;
                float y1 = rctranBoard.anchoredPosition.y + rctranBoard.rect.height / 2;
                float y2 = rectImage.center.y - rectImage.size.y / 2;
                if (Device.isLandscape)
                {
                    y2 = this.frame.height / 2 - topbarHeightCanvas;
                }
                y = (y1 + y2) / 2;
                x = rctranBoard.anchoredPosition.x;

                lg.row = 1;
                lg.col = lg.GetChildCount(lg.enableHide);
            }
            else
            {

                {
                    //竖排显示
                    w = topbarHeightCanvas;
                    h = rectImage.size.y;
                    x = rectImage.center.x - rectImage.size.x / 2 + w / 2 + 32;
                    y = rectImage.center.y;
                    lg.col = 1;
                    lg.row = lg.GetChildCount(lg.enableHide);
                }

            }

            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
            if (lg != null)
            {
                lg.LayOut();
            }
        }


    }

    void UpdateGold()
    {

        string str = Language.main.GetString("STR_GOLD") + ":" + Common.gold.ToString();
        textGold.text = str;
        int fontsize = textGold.fontSize;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        RectTransform rctran = imageGoldBg.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;

        sizeDelta.x = str_w + fontsize;
        rctran.sizeDelta = sizeDelta;
    }
    void UpdateTitle()
    {
        int idx = LevelManager.main.gameLevel + 1;
        textTitle.text = idx.ToString();
    }


    void UpdateWord()
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        if (uiWordContent != null)
        {
            uiWordContent.UpdateWord();
        }
        //先计算行列数
        LayOut();
        uiWordBoard.InitItem();
        string strBoard = GameAnswer.main.GetWordBoardString(info, uiWordBoard.row, uiWordBoard.col);
        uiWordBoard.UpadteItem(info, strBoard);
        uiWordBar.UpadteItem(info);
    }

    void ShowShop()
    {
        ShopViewController.main.Show(null, this);
    }


    public bool CheckAllAnswerFinish()
    {
        bool ret = false;
        if (uiWordContent != null)
        {
            ret = uiWordContent.CheckAllAnswerFinish();
        }
        return ret;
    }

    public void OnNotEnoughGold(UIWordBar bar, bool isUpdate)
    {
        if (isUpdate)
        {
            UpdateGold();
        }
        else
        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_NOT_ENOUGH_GOLD);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_NOT_ENOUGH_GOLD);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_NOT_ENOUGH_GOLD);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_NOT_ENOUGH_GOLD);

            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GOLD, OnUIViewAlertFinished);
        }

    }



    public void UIWordBoardDidClick(UIWordBoard bd, UIWordItem item)
    {
        Debug.Log("UIWordBoardDidClick");
        CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();

        if (infoGuanka.gameType == GameRes.GAME_TYPE_TEXT)
        {
            if (uiWordContent.CheckAllFill())
            {
                uiWordContent.OnAddWord(item.wordDisplay);
                item.ShowContent(false);
                if (CheckAllAnswerFinish())
                {
                    OnGameWinFinish(uiWordBar, false);
                }
                else
                {
                    OnGameWinFinish(uiWordBar, true);
                }
            }
        }
        if (uiWordBar.gameObject.activeSelf)
        {
            if (!uiWordBar.CheckAllFill())
            {
                uiWordBar.AddWord(item.wordDisplay);
                item.ShowContent(false);
            }
        }
        else
        {
            if (uiWordContent != null)
            {
                uiWordContent.OnAddWord(item.wordDisplay);
                item.ShowContent(false);
                bool ret = uiWordContent.CheckAllAnswerFinish();
                Debug.Log("CheckAllAnswer ret=" + ret);
                if (ret)
                {
                    OnGameWinFinish(uiWordBar, false);
                }
            }

        }

    }
    public void OnGameWinFinish(UIWordBar bar, bool isFail)
    {
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        //show game win
        if (isFail)
        {
            PopUpManager.main.Show<UIGameFail>("App/Prefab/Game/UIGameFail");
        }
        else
        {
            Debug.Log("caicaile OnGameWin");
            LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;
            //gameEndParticle.Play();
            //  Invoke("ShowGameWin", 1f);
            OnGameWinBase();

            if (gameBase != null)
            {

                CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
                Debug.Log("caicaile OnGameWin GAME_STATUS_FINISH+info.id=" + info.id);
                gameBase.SetGameItemStatus(info, GameBase.GAME_STATUS_FINISH);
            }

            string strPrefab = "";
            switch (infoPlace.id)
            {
                case GameRes.GAME_IdiomConnect:
                    {
                        strPrefab = "App/Prefab/Game/UIGameWinIdiomConnect";
                    }
                    break;

                default:
                    {
                        strPrefab = "App/Prefab/Game/UIGameWin";
                        break;
                    }
            }
            PopUpManager.main.Show<UIGameWinBase>(strPrefab);
        }

    }
    void ShowGameWin()
    {
        //GameScene.ShowAdInsert(100);

        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_GAME_FINISH);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_GAME_FINISH);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_GAME_FINISH);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_GAME_FINISH);

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GAME_FINISH, OnUIViewAlertFinished);

        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        string str = languageGame.GetString(info.id);
        TTS.main.Speak(str);
    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_GAME_FINISH == alert.keyName)
        {
            if (isYes)
            {
                LevelManager.main.GotoNextLevel();
            }
        }

        if (STR_KEYNAME_VIEWALERT_GOLD == alert.keyName)
        {
            if (isYes)
            {
                ShowShop();
            }
        }



    }

    public void UIWordContentBaseDidBackWord(UIWordContentBase ui, string word)
    {
        uiWordBoard.BackWord(word);
    }
    public void UIWordContentBaseDidTipsWord(UIWordContentBase ui, string word)
    {
        uiWordBoard.HideWord(word);
    }

    public void UIWordBarDidBackWord(UIWordBar ui, string word)
    {
        uiWordBoard.BackWord(word);
    }
    public void UIWordBarDidTipsWord(UIWordBar ui, string word)
    {
        uiWordBoard.HideWord(word);
    }

    public void OnClickBtnHelp()
    {
        HowToPlayViewController.main.Show(null, null);
    }
    public void OnClickBtnRetry()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
    }
    public void OnClickBtnTips()
    {

        if (Common.gold <= 0)
        {
            OnNotEnoughGold(uiWordBar, false);
            return;
        }

        //if (isonlytext && (Common.appKeyName != GameRes.GAME_RIDDLE))
        if (!uiWordBar.gameObject.activeSelf)
        {


            if (uiWordContent != null)
            {
                uiWordContent.OnTips();
            }

            Common.gold--;
            if (Common.gold < 0)
            {
                Common.gold = 0;
            }
            OnNotEnoughGold(uiWordBar, true);
            if (CheckAllAnswerFinish())
            {
                OnGameWinFinish(uiWordBar, false);
            }
        }
        else
        {
            if (uiWordBar != null)
            {
                uiWordBar.OnTips();
            }
        }

    }


    public void OnClickGold()
    {
        ShowShop();
    }

    public override void OnClickBtnBack()
    {
        base.OnClickBtnBack();
    }

    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        UpdateGold();
    }
}
