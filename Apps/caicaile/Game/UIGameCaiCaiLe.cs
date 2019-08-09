using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameCaiCaiLe : UIGameBase, IPopViewControllerDelegate
{
    public const string STR_UNKNOWN_WORD = "__";

    public GameObject objTopBar;
    public Button btnTips;
    public RawImage imageBg;
    public RawImage imagePic;
    public Image imagePicBoard;
    public GameObject objLeftBtn;
    public GameObject objContentPic;
    public Text textTitle;

    public Text textLine0;
    public Text textLine1;
    public GameObject objText;

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
        gameBase = this.gameObject.AddComponent<GameBase>();
        if (gameBase == null)
        {
            Debug.Log("gameBase is null");
        }
        UpdateLanguageWord();
        btnTips.gameObject.SetActive(Config.main.isHaveShop);
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
        uiWordBoard.wordBar = uiWordBar;
        uiWordBar.callbackGameFinish = OnGameWinFinish;
        uiWordBar.callbackGold = OnNotEnoughGold;

        UpdateLanguage();
        UpdateBtnMusic();

        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        objText.gameObject.SetActive(isonlytext);
        imagePic.gameObject.SetActive(!isonlytext);
        uiWordBar.gameObject.SetActive(!isonlytext);

    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
        Common.SetButtonText(btnTips, Language.main.GetString("STR_BTN_TIPS"), 16);
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
        GameGuankaParse.main.ParsePoemItem(info);
        InitUI();
        // ShowAdInsert(GAME_AD_INSERT_SHOW_STEP,true);
        if (gameBase != null)
        {
            if (gameBase.GetGameItemStatus(info) == GameBase.GAME_STATUS_UN_START)
            {
                gameBase.SetGameItemStatus(info, GameBase.GAME_STATUS_PLAY);
            }

        }


        OnUIDidFinish();
    }
    void InitUI()
    {
        //game pic
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        if (!isonlytext)
        {
            TextureUtil.UpdateRawImageTexture(imagePic, info.pic, true);
        }

        TextureUtil.UpdateImageTexture(imagePicBoard, "AppCommon/UI/Game/BoardPic", true);


        UpdateWord();
        UpdateTitle();
        UpdateGold();
        LayOut();
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
        RectTransform rctranContentPic = objContentPic.GetComponent<RectTransform>();
        //game pic
        {

            ratio = 0.9f;
            if (Device.isLandscape)
            {
                w = (this.frame.width / 2) * ratio;
                h = (this.frame.height - topbarHeightCanvas * 2) * ratio;
                x = -this.frame.width / 4 - w / 2;
                y = 0 - h / 2;
            }
            else
            {

                // w = this.frame.width - topbarHeightCanvas * 2;
                w = this.frame.width;
                h = (this.frame.height / 2 - topbarHeightCanvas * 2);
                y = this.frame.height / 4 - h / 2;
                x = 0 - w / 2;
            }
            Rect rectImage = new Rect(x, y, w, h);
            Debug.Log("rectImage =" + rectImage);

            rctranContentPic.sizeDelta = new Vector2(w, h);
            rctranContentPic.anchoredPosition = rectImage.center;

            //imagePicBoard
            {
                RectTransform rctran = imagePicBoard.GetComponent<RectTransform>();
                float oft = 16;
                rctran.offsetMin = new Vector2(oft, oft);
                rctran.offsetMax = new Vector2(-oft, -oft);
            }


            // RectTransform rctran = imagePic.GetComponent<RectTransform>();
            // float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, rectImage.width, rectImage.height) * ratio;
            // imagePic.transform.localScale = new Vector3(scale, scale, 1.0f);
            // rctran.anchoredPosition = rectImage.center;

            // {
            //     rctran = imagePicBoard.GetComponent<RectTransform>();
            //     ratio = 0.9f;
            //     scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, rectImage.width, rectImage.height) * ratio;
            //     rctran.transform.localScale = new Vector3(scale, scale, 1.0f);
            //     rctran.anchoredPosition = rectImage.center;
            // }
        }



        //wordboard
        {



            RectTransform rctran = uiWordBoard.GetComponent<RectTransform>();
            GridLayoutGroup gridLayout = uiWordBoard.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            Vector2 space = gridLayout.spacing;

            bool isonlytext = GameGuankaParse.main.OnlyTextGame();
            if (isonlytext)
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
                float x1 = rctranContentPic.anchoredPosition.x + rctranContentPic.rect.size.x / 2;
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
            h = rctranContentPic.rect.size.y;
            x = 0;
            y = rctranWordBar.anchoredPosition.y;
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

        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        if (isonlytext)
        {
            PoemContentInfo infopoem0 = info.listPoemContent[0];
            string str = infopoem0.content;
            //最后一个标点符号
            int idx = Random.Range(0, str.Length - 1);

            string answer = str.Substring(idx, 1);
            uiWordBoard.strWordAnswer = answer;
            string strHead = str.Substring(0, idx);
            string strEnd = str.Substring(idx + 1);
            textLine0.text = strHead + STR_UNKNOWN_WORD + strEnd;

            PoemContentInfo infopoem1 = info.listPoemContent[1];
            textLine1.text = infopoem1.content;

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

    public void OnGameWinFinish(UIWordBar bar, bool isFail)
    {
        //show game win
        if (isFail)
        {
            PopUpManager.main.Show<UIGameFail>("AppCommon/Prefab/Game/UIGameFail");
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
            PopUpManager.main.Show<UIGameWin>("AppCommon/Prefab/Game/UIGameWin");
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

    public void OnClickBtnTips()
    {
        if (uiWordBar != null)
        {
            uiWordBar.OnClickBtnTips();
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
