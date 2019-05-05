using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CaiCaiLeItemInfo : ItemInfo
{


}

public class UIGameCaiCaiLe : UIGameBase
{

    public GameObject objTopBar;
    public Image imageBg;
    public Image imagePic;
    public Text textTitle;
    public GameObject objGoldBar;
    public Image imageGoldBg;
    public Text textGold;
    public UIShop uiShopPrefab;
    public UIWordBoard uiWordBoard;
    public UIWordBar uiWordBar;
    static public string strWord3500;
    string strPlace;
    float goldBaroffsetYNormal;

    void Awake()
    {

        RectTransform rctran = objTopBar.GetComponent<RectTransform>();
        //bgs

        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_GAME_BG, true);
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
        uiWordBar.callbackGameWin = OnGameWin;
        uiWordBar.callbackGold = OnNotEnoughGold;

        ParseGuanka();
        languageGame = new Language();
        languageGame.Init(Common.GAME_RES_DIR + "/language/language.csv");
        languageGame.SetLanguage(SystemLanguage.Chinese);//(Language.main.GetLanguage());



    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(GameManager.gameLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        AppSceneBase.main.ClearMainWorld();
        InitUI();
        ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
        OnUIDidFinish();
    }
    void InitUI()
    {
        //game pic
        CaiCaiLeItemInfo info = GetItemInfo();
        TextureUtil.UpdateImageTexture(imagePic, info.pic, true);
        UpdateWordBar();
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
        //game pic
        {


            if (Device.isLandscape)
            {
                w = this.frame.width / 2;
                h = this.frame.height - topbarHeightCanvas;
                x = -this.frame.width / 4 - w / 2;
                y = 0 - h / 2;
            }
            else
            {
                ratio = 0.8f;
                w = this.frame.width * ratio;
                h = (this.frame.height / 2) * ratio;
                y = this.frame.height / 4 - h / 2;
                x = 0 - w / 2;
            }
            Rect rectImage = new Rect(x, y, w, h);
            RectTransform rctran = imagePic.GetComponent<RectTransform>();
            float w_image = rctran.rect.width;
            float h_image = rctran.rect.height;
            float scalex = rectImage.width / w_image;
            float scaley = rectImage.height / h_image;
            float scale = Mathf.Min(scalex, scaley);
            imagePic.transform.localScale = new Vector3(scale, scale, 1.0f);
            rctran.anchoredPosition = rectImage.center;
        }

        //wordbar
        {
            RectTransform rctran = uiWordBar.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                x = this.frame.width / 4;
                y = this.frame.height / 4;
            }
            else
            {
                x = 0;
                y = 0;

            }
            rctran.anchoredPosition = new Vector2(x, y);
        }
        //wordboard
        {
            RectTransform rctran = uiWordBoard.GetComponent<RectTransform>();
            GridLayoutGroup gridLayout = uiWordBoard.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            Vector2 space = gridLayout.spacing;

            if (Device.isLandscape)
            {
                x = this.frame.width / 4;
                y = -this.frame.height / 4 + topbarHeightCanvas;

                //6x4
                w = (cellSize.x + space.x) * 6;
                h = (cellSize.y + space.y) * 4;
            }
            else
            {
                x = 0;
                y = -this.frame.height / 4;


                //8x3
                w = (cellSize.x + space.x) * 8;
                h = (cellSize.y + space.y) * 3;
                if (w > this.frame.width)
                {
                    w = (cellSize.x + space.x) * 6;
                    h = (cellSize.y + space.y) * 4;
                }

            }
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
        int idx = GameManager.gameLevel + 1;
        textTitle.text = idx.ToString();
    }
    CaiCaiLeItemInfo GetItemInfo()
    {
        int idx = GameManager.gameLevel;
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        CaiCaiLeItemInfo info = listGuanka[idx] as CaiCaiLeItemInfo;
        return info;
    }

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

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();
        int idx = GameManager.placeLevel;

        ItemInfo infoPlace = GameManager.main.GetPlaceItemInfo(GameManager.placeLevel);
        string filepath = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            filepath = Common.GAME_RES_DIR + "/guanka/item_" + infoPlace.id + ".json";
        }
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);

        JsonData root = JsonMapper.ToObject(json);
        string strPlace = infoPlace.id;
        JsonData items = root["items"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            info.id = (string)item["id"];
            //string str = "aa";// = languageGame.GetString(info.id);
            //Debug.Log(i + ":ParseGame:" + str);
            info.pic = Common.GAME_RES_DIR + "/image/" + strPlace + "/" + info.id + ".png";
            info.icon = Common.GAME_RES_DIR + "/image_thumb/" + strPlace + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = info.pic;
            }
            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        //word3500
        filepath = Common.GAME_DATA_DIR + "/words_3500.json";
        json = FileUtil.ReadStringAsset(filepath);
        root = JsonMapper.ToObject(json);
        strWord3500 = (string)root["words"];
        Debug.Log(strWord3500);

        Debug.Log("ParseGame::count=" + count);
        return count;
    }

    void UpdateWordBar()
    {
        CaiCaiLeItemInfo info = GetItemInfo();
        uiWordBar.UpadteItem(info);
        uiWordBoard.UpadteItem(info);
    }

    void ShowShop()
    {
        // UIShop shop = (UIShop)GameObject.Instantiate(uiShopPrefab);
        // GameScene.uiShop = shop;
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
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_NOT_ENOUGH_GOLD);

            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GOLD, OnUIViewAlertFinished);
        }

    }
    public void OnGameWin(UIWordBar bar)
    {
        //show game win
        GameManager.gameLevelFinish = GameManager.gameLevel;
        //gameEndParticle.Play();
        //  Invoke("ShowGameWin", 1f);
        ShowGameWin();
    }
    void ShowGameWin()
    {
        //GameScene.ShowAdInsert(100);

        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_GAME_FINISH);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_GAME_FINISH);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_GAME_FINISH);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_GAME_FINISH);

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GAME_FINISH, OnUIViewAlertFinished);

        CaiCaiLeItemInfo info = GetItemInfo();
        string str = languageGame.GetString(info.id);
        TTS.main.Speak(str);
    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_GAME_FINISH == alert.keyName)
        {
            if (isYes)
            {
                GameManager.main.GotoNextLevel();
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

    public void OnClickGold()
    {
        ShowShop();
    }

    public override void OnClickBtnBack()
    {
        base.OnClickBtnBack();
    }

}
