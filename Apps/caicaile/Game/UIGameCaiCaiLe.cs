using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameCaiCaiLe : UIGameBase, IPopViewControllerDelegate, IUIWordBoardDelegate, IUIWordFillBoxDelegate
{


    public GameObject objTopBar;
    public Button btnTips;
    public Button btnRetry;
    public RawImage imageBg;
    public GameObject objLeftBtn;
    public Text textTitle;


    public UIWordFillBox uiWordFillBoxPrefab;
    public UIWordFillBox uiWordFillBox;

    public UIWordImageText uiWordImageTextPrefab;
    public UIWordImageText uiWordImageText;


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
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        gameBase = this.gameObject.AddComponent<GameBase>();
        if (gameBase == null)
        {
            Debug.Log("gameBase is null");
        }
        UpdateLanguageWord();
        btnTips.gameObject.SetActive(Config.main.isHaveShop);

        btnRetry.gameObject.SetActive(GameGuankaParse.main.OnlyTextGame());


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

        uiWordBar.wordBoard = uiWordBoard;
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


        Common.SetButtonText(btnTips, Language.main.GetString("STR_BTN_TIPS"), 64);
        Common.SetButtonText(btnRetry, Language.main.GetString("STR_BTN_Retry"), 64);


        // Common.GetButtonText(btnTips).color = GameRes.main.colorTitle;
        // Common.GetButtonText(btnRetry).color = GameRes.main.colorTitle;

    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
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
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        GameGuankaParse.main.ParseItem(info);

        if (info.gameType == GameRes.GAME_TYPE_CONNECT)
        {
            uiWordFillBox = (UIWordFillBox)GameObject.Instantiate(uiWordFillBoxPrefab);
            uiWordFillBox.transform.SetParent(this.transform);
            uiWordFillBox.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(uiWordFillBoxPrefab.gameObject, uiWordFillBox.gameObject);
            uiWordFillBox.iDelegate = this;
            uiWordFillBox.UpdateGuankaLevel(level);
        }
        if ((info.gameType == GameRes.GAME_TYPE_IMAGE) || (info.gameType == GameRes.GAME_TYPE_IMAGE_TEXT))
        {
            uiWordImageText = (UIWordImageText)GameObject.Instantiate(uiWordImageTextPrefab);
            uiWordImageText.transform.SetParent(this.transform);
            uiWordImageText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(uiWordImageTextPrefab.gameObject, uiWordImageText.gameObject);
            uiWordImageText.UpdateGuankaLevel(level);
        }


        // ShowAdInsert(GAME_AD_INSERT_SHOW_STEP,true);
        if (gameBase != null)
        {
            if (gameBase.GetGameItemStatus(info) == GameBase.GAME_STATUS_UN_START)
            {
                gameBase.SetGameItemStatus(info, GameBase.GAME_STATUS_PLAY);
            }

        }
        //ool isonlytext = GameGuankaParse.main.OnlyTextGame();
        // if (!isonlytext)


        //  TextureUtil.UpdateImageTexture(imagePicBoard, "App/UI/Game/BoardPic", true);


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
            rectImage = new Rect(x, y, w, h);
            Debug.Log("rectImage =" + rectImage);




            if (uiWordImageText != null)
            {
                RectTransform rctran = uiWordImageText.GetComponent<RectTransform>();
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = rectImage.center;
                uiWordImageText.LayOut();
            }

            if (uiWordFillBox != null)
            {
                RectTransform rctran = uiWordFillBox.GetComponent<RectTransform>();
                w = Mathf.Min(w, h);
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = rectImage.center;
                uiWordFillBox.LayOut();
            }
        }



        //wordboard
        {



            RectTransform rctran = uiWordBoard.GetComponent<RectTransform>();
            GridLayoutGroup gridLayout = uiWordBoard.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            Vector2 space = gridLayout.spacing;

            bool isonlytext = GameGuankaParse.main.OnlyTextGame();
            if (isonlytext && (Common.appKeyName != GameRes.GAME_RIDDLE))
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
        //wordbar
        {
            ratio = 0.9f;
            RectTransform rctranBoard = uiWordBoard.GetComponent<RectTransform>();
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

            RectTransform rctran = objLeftBtn.GetComponent<RectTransform>();
            w = rctran.rect.size.x;
            h = rectImage.size.y;
            x = -32;
            // y = rctranWordBar.anchoredPosition.y;
            y = rectImage.center.y;
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
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
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        if (uiWordImageText != null)
        {
            uiWordImageText.UpdateWord();
        }
        //先计算行列数
        LayOut();
        uiWordBoard.InitItem();


        uiWordBoard.UpadteItem(info);
        uiWordBar.UpadteItem(info);
    }

    void ShowShop()
    {
        ShopViewController.main.Show(null, this);
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


        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        int index = 0;

        if (isonlytext)
        {


            bool isInAnswerList = false;
            foreach (AnswerInfo info in uiWordImageText.listAnswerInfo)
            {
                if (info.word == item.wordDisplay)
                {
                    //回答正确
                    Debug.Log("GetDisplayText ok index =" + index);
                    uiWordImageText.UpdateGameWordString(uiWordImageText.GetDisplayText(true, true, index, ""));
                    info.isFinish = true;
                    isInAnswerList = true;
                    break;

                }
                index++;
            }

            if (!isInAnswerList)
            {
                //回答错误
                index = uiWordImageText.GetFirstUnFillAnswer();
                Debug.Log("GetDisplayText error index=" + index);

                uiWordImageText.UpdateGameWordString(uiWordImageText.GetDisplayText(true, false, index, item.wordDisplay));
            }

            bool isAllFill = true;
            foreach (AnswerInfo info in uiWordImageText.listAnswerInfo)
            {
                if (!info.isFillWord)
                {
                    isAllFill = false;
                }
            }
            if (isAllFill)
            {
                if (uiWordImageText.CheckAllAnswerFinish())
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
            if (!uiWordBar.IsWordFull())
            {
                uiWordBar.AddWord(item.wordDisplay);
                item.ShowContent(false);
            }
        }
        else
        {
            if (uiWordFillBox != null)
            {
                uiWordFillBox.OnAddWord(item.wordDisplay);
                item.ShowContent(false);
                bool ret = uiWordFillBox.CheckAllAnswer();
                if (ret)
                {
                    OnGameWinFinish(uiWordBar, false);
                }
            }

        }

    }
    public void OnGameWinFinish(UIWordBar bar, bool isFail)
    {
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

                CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
                Debug.Log("caicaile OnGameWin GAME_STATUS_FINISH+info.id=" + info.id);
                gameBase.SetGameItemStatus(info, GameBase.GAME_STATUS_FINISH);
            }
            PopUpManager.main.Show<UIGameWin>("App/Prefab/Game/UIGameWin");
            // ShowGameWin();
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

        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
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

    public void UIWordFillBoxDidBackWord(UIWordFillBox ui, string word)
    {
        uiWordBoard.BackWord(word);
    }
    public void OnClickBtnRetry()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
    }
    public void OnClickBtnTips()
    {
        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        //if (isonlytext && (Common.appKeyName != GameRes.GAME_RIDDLE))
        if (!uiWordBar.gameObject.activeSelf)
        {
            if (Common.gold <= 0)
            {
                OnNotEnoughGold(uiWordBar, false);
                return;
            }

            int index = uiWordImageText.GetFirstUnFinishAnswer();
            AnswerInfo info = uiWordImageText.listAnswerInfo[index];
            uiWordImageText.UpdateGameWordString(uiWordImageText.GetDisplayText(true, true, index, ""));
            info.isFinish = true;
            Common.gold--;
            if (Common.gold < 0)
            {
                Common.gold = 0;
            }
            OnNotEnoughGold(uiWordBar, true);
            if (uiWordImageText.CheckAllAnswerFinish())
            {
                OnGameWinFinish(uiWordBar, false);
            }
        }
        else
        {
            if (uiWordBar != null)
            {
                uiWordBar.OnClickBtnTips();
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
