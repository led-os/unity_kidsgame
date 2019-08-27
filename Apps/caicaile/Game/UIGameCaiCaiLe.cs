using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameCaiCaiLe : UIGameBase, IPopViewControllerDelegate, IUIWordBoardDelegate
{
    public const string STR_UNKNOWN_WORD = "__";

    public GameObject objTopBar;
    public Button btnTips;
    public Button btnRetry;
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
    List<AnswerInfo> listAnswerInfo;//
    int[] listIndexAnswer;
    void Awake()
    {
        gameBase = this.gameObject.AddComponent<GameBase>();
        if (gameBase == null)
        {
            Debug.Log("gameBase is null");
        }
        UpdateLanguageWord();
        btnTips.gameObject.SetActive(Config.main.isHaveShop);

        btnRetry.gameObject.SetActive(GameGuankaParse.main.OnlyTextGame());

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

        uiWordBoard.iDelegate = this;

        LanguageManager.main.UpdateLanguage(LevelManager.main.placeLevel);
        UpdateLanguage();
        UpdateBtnMusic();

        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        objText.gameObject.SetActive(isonlytext);
        imagePic.gameObject.SetActive(!isonlytext);
        uiWordBar.gameObject.SetActive(!isonlytext);

        Common.SetButtonText(btnTips, Language.main.GetString("STR_BTN_TIPS"), 64);
        Common.SetButtonText(btnRetry, Language.main.GetString("STR_BTN_Retry"), 64);
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
        GameGuankaParse.main.ParsePoemItem(info);

        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        if (isonlytext)
        {
            PoemContentInfo infopoem0 = info.listPoemContent[0];
            string strPoem = infopoem0.content;
            //过虑标点符号
            List<int> listIndexGuanka = GameGuankaParse.main.IndexListNotPunctuation(strPoem);

            listIndexAnswer = Util.main.RandomIndex(listIndexGuanka.Count, 2);
            ListSorter.EbullitionSort(listIndexAnswer);

            listAnswerInfo = new List<AnswerInfo>();
            uiWordBoard.strWordAnswer = "";
            for (int i = 0; i < listIndexAnswer.Length; i++)
            {
                int idx = listIndexGuanka[listIndexAnswer[i]];
                string word_answer = strPoem.Substring(idx, 1);
                uiWordBoard.strWordAnswer += word_answer;

                AnswerInfo infoanswer = new AnswerInfo();
                infoanswer.word = word_answer;
                infoanswer.index = idx;
                infoanswer.isFinish = false;
                infoanswer.isFillWord = false;
                listAnswerInfo.Add(infoanswer);
                Debug.Log("listAnswerInfo add " + word_answer);
            }
        }


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

        TextureUtil.UpdateImageTexture(imagePicBoard, "App/UI/Game/BoardPic", true);


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
            x = -32;
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


    List<string> GetSplitStringByAnswerIndex(int[] listIndex, List<int> listIndexGuanka)
    {
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        PoemContentInfo infopoem0 = info.listPoemContent[0];
        string strPoem = infopoem0.content;


        List<string> listStr = new List<string>();
        for (int i = 0; i < listIndex.Length; i++)
        {
            int idx = listIndexGuanka[listIndex[i]];
            int idx_pre = 0;
            if (i > 0)
            {
                idx_pre = listIndexGuanka[listIndex[i - 1]] + 1;
            }
            int len = idx - idx_pre;
            string str = strPoem.Substring(idx_pre, len);
            listStr.Add(str);
            //最后一个
            if (i == listIndex.Length - 1)
            {
                len = (strPoem.Length - 1) - idx;
                if (len > 0)
                {
                    str = strPoem.Substring(idx + 1, len);
                    listStr.Add(str);
                }
            }

        }
        return listStr;
    }

    int GetFirstUnFillAnswer()
    {
        int index = 0;
        foreach (AnswerInfo info in listAnswerInfo)
        {
            if (info.isFillWord == false)
            {
                break;
            }
            index++;
        }
        return index;
    }

    int GetFirstUnFinishAnswer()
    {
        int index = 0;
        foreach (AnswerInfo info in listAnswerInfo)
        {
            if (info.isFinish == false)
            {
                break;
            }
            index++;
        }
        return index;
    }

    bool CheckAllAnswerFinish()
    {
        bool ret = true;
        foreach (AnswerInfo info in listAnswerInfo)
        {
            if (info.isFinish == false)
            {
                ret = false;
                break;
            }

        }
        return ret;
    }
    string GetDisplayText(bool isAnswr, bool isSucces, int indexAnswer, string word)
    {
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        PoemContentInfo infopoem0 = info.listPoemContent[0];
        string strPoem = infopoem0.content;

        //过虑标点符号
        List<int> listIndexGuanka = GameGuankaParse.main.IndexListNotPunctuation(strPoem);
        List<string> listStr = GetSplitStringByAnswerIndex(listIndexAnswer, listIndexGuanka);
        string strShow = "";
        for (int i = 0; i < listStr.Count; i++)
        {
            string tmp = listStr[i];
            if (i == (listStr.Count - 1))
            {
                strShow += tmp;
            }
            else
            {
                if (isAnswr)
                {
                    AnswerInfo infoanswer = listAnswerInfo[i];

                    if ((infoanswer.isFinish))
                    {
                        strShow += (tmp + infoanswer.word);
                    }
                    else
                    {
                        AnswerInfo infotmp = listAnswerInfo[indexAnswer];
                        if (indexAnswer == i)
                        {
                            infotmp.isFillWord = true;
                            Debug.Log("GetDisplayText isFillWord  i=" + i);
                            string word_show = "";
                            if (isSucces)
                            {
                                word_show = infotmp.word;
                            }
                            else
                            {
                                word_show = word;
                            }

                            //<color=#00ffffff>TestTest</color>
                            Color color = new Color32(255, 0, 0, 255);
                            word_show = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + word_show + "</color>";
                            strShow += (tmp + word_show);
                            infotmp.wordFill = word_show;
                        }
                        else
                        {
                            if (infoanswer.isFillWord)
                            {
                                strShow += (tmp + infoanswer.wordFill);
                            }
                            else
                            {
                                strShow += (tmp + STR_UNKNOWN_WORD);
                            }


                        }

                    }

                }

                else
                {
                    strShow += (tmp + STR_UNKNOWN_WORD);
                }
            }

        }
        return strShow;
    }
    void UpdateWord()
    {
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();

        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        if (isonlytext)
        {
            textLine0.text = GetDisplayText(false, false, 0, "");
            // PoemContentInfo infopoem1 = info.listPoemContent[1];  
            // textLine1.text = infopoem1.content;
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
            foreach (AnswerInfo info in listAnswerInfo)
            {
                if (info.word == item.strWord)
                {
                    //回答正确
                    Debug.Log("GetDisplayText ok index =" + index);
                    textLine0.text = GetDisplayText(true, true, index, "");
                    info.isFinish = true;
                    isInAnswerList = true;
                    break;

                }
                index++;
            }

            if (!isInAnswerList)
            {
                //回答错误
                index = GetFirstUnFillAnswer();
                Debug.Log("GetDisplayText error index=" + index);

                textLine0.text = GetDisplayText(true, false, index, item.strWord);
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

    public void OnClickBtnRetry()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
    }
    public void OnClickBtnTips()
    {
        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        if (isonlytext)
        {

            int index = GetFirstUnFinishAnswer();
            AnswerInfo info = listAnswerInfo[index];
            textLine0.text = GetDisplayText(true, true, index, "");
            info.isFinish = true;

            if (CheckAllAnswerFinish())
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
