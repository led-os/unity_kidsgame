using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class a : UIHomeBase
{
    public Image imageGoldBg;
    public Text textGold;

    public Button btnHistory;
    public Button btnNormalWrite;
    public Button btnFreeWrite;
    public LayOutGrid layoutBtn;
    ActionHomeBtn actionBtnNormalWrite;
    ActionHomeBtn actionBtnFreeWrite;

    ActionHomeBtn actionBtnHistory;
    float layoutBtnOffsetYNormal;
    void Awake()
    {
        base.Awake();
        //bg
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);

        layoutBtn.enableLayout = false;
        UpdateLayoutBtn();


        actionBtnNormalWrite = btnNormalWrite.gameObject.GetComponent<ActionHomeBtn>();
        actionBtnNormalWrite.ptNormal = layoutBtn.GetItemPostion(actionBtnNormalWrite.gameObject, 0, 0);

        actionBtnFreeWrite = btnFreeWrite.gameObject.GetComponent<ActionHomeBtn>();
        actionBtnFreeWrite.ptNormal = layoutBtn.GetItemPostion(actionBtnFreeWrite.gameObject, 0, 1);


        actionBtnHistory = btnHistory.gameObject.GetComponent<ActionHomeBtn>();
        actionBtnHistory.ptNormal = layoutBtn.GetItemPostion(actionBtnHistory.gameObject, 0, 2);




    }
    // Use this for initialization
    void Start()
    {
        base.Start();
        InitUI();
        actionBtnNormalWrite.RunAction();
        actionBtnFreeWrite.RunAction();
        actionBtnHistory.RunAction();
        Invoke("OnUIDidFinish", 2f);
    }
    // Update is called once per frame
    void Update()
    {
        base.UpdateBase();

    }

    void InitUI()
    {
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            TTS.main.Speak(appname);
        }


        UpdateGold();
        if (!Config.main.isHaveShop)
        {
            imageGoldBg.gameObject.SetActive(false);
        }
        LayOut();

    }

    public void UpdateLayoutBtn()
    {
        float x = 0, y = 0, w = 0, h = 0;
        float w_item = 256;
        float h_item = 256;
        float oft = 64;
        layoutBtn.row = 1;
        layoutBtn.col = 3;

        Vector2 sizeCanvas = this.frame.size;
        float h_play = 0;

        RectTransform rctran = layoutBtn.GetComponent<RectTransform>();
        rctran.sizeDelta = new Vector2((w_item + oft) * layoutBtn.col, (h_item + oft) * layoutBtn.row);


        x = 0;
        float offy = h_play / 2;
        float offy_bottom = 0;
        if (!Device.isLandscape)
        {

            GridLayoutGroup gridLayout = uiHomeAppCenter.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            offy_bottom = cellSize.y;
        }
        Debug.Log("offy_bottom=" + offy_bottom);
        y = -offy - (this.frame.height / 2 - offy - offy_bottom) / 2;
        rctran.anchoredPosition = new Vector2(x, y);


        // RectTransform rcbtn = btnHistory.GetComponent<RectTransform>();

        // w = (hrLy.spacing + rcbtn.rect.width) * 3 * 1.1f;
        // h = rctran.rect.height;
        // rctran.sizeDelta = new Vector2(w, h);



        layoutBtn.LayOut();
    }
    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0;

        Vector2 sizeCanvas = this.frame.size;
        float h_play = 0;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;

        //image name
        {
            RectTransform rctran = imageBgName.GetComponent<RectTransform>();

            int fontSize = TextName.fontSize;
            int r = fontSize / 2;
            w = Common.GetStringLength(TextName.text, AppString.STR_FONT_NAME, fontSize) + r * 2;
            h = fontSize * 1.5f;
            if (!Device.isLandscape)
            {

                if ((w + r * 2 + topBarHeight * 2) > sizeCanvas.x)
                {
                    w = w / 2 + r * 2;
                    h = h * 2;
                }
            }

            rctran.sizeDelta = new Vector2(w, h);
            x = 0;
            float offy = h_play / 2;
            y = offy + (this.frame.height / 2 - topBarHeight - offy) / 2;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        //btnlayout
        {
            UpdateLayoutBtn();
        }


        LayoutChildBase();
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

    public void GotoGame(int mode)
    {
        GameManager.main.gameMode = mode;
        UIViewController controller = null;

        if (mode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            controller = GameViewController.main;
        }
        else
        {

            int total = LevelManager.main.placeTotal;
            if (total > 1)
            {
                controller = PlaceViewController.main;
            }
            else
            {
                controller = GuankaViewController.main;
            }
        }


        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_GUANKA;
            navi.Push(controller);

        }
    }

    public void OnClickGold()
    {

    }
    public void OnClickBtnHistory()
    {
        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_HISTORY;
            navi.Push(HistoryViewController.main);
        }
        //HistoryViewController.main.Show(null, null);
    }




    public void OnClickBtnNormalWrite()
    {
        GotoGame(GameXieHanzi.GAME_MODE_NORMAL);
    }

    public void OnClickBtnFreeWrite()
    {
        AdKitCommon.main.ShowAdVideo();
        GotoGame(GameXieHanzi.GAME_MODE_FREE_WRITE);
    }




}
