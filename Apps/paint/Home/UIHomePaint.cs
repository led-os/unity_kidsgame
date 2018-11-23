using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHomePaint : UIHomeBase
{

    public Button btnHistory;
    public Button btnPlay;


    public GameObject objLayoutBtn;

    void Awake()
    {

        //bg
        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);//IMAGE_HOME_BG

    }
    // Use this for initialization
    void Start()
    {

        InitUI();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBase();
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
                h = fontSize * 2;
                if ((w + r * 2) > sizeCanvas.x)
                {
                    //显示成两行文字
                    w = w / 2 + r * 2;
                    h = h * 2;
                    // RectTransform rctranText = TextName.GetComponent<RectTransform>();
                    // float w_text = rctranText.sizeDelta.x;
                    // rctranText.sizeDelta = new Vector2(w_text, h);
                }
            }

            rctran.sizeDelta = new Vector2(w, h);
            x = 0;
            y = (sizeCanvas.y - topBarHeight) / 4;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        //btnlayout
        {
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
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
        }


        LayoutChildBase();

    }

    void InitUI()
    {

        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            TTS.Speek(appname);
        }


        LayOut();

        OnUIDidFinish();
    }


    public void OnUIColorHistoryDidClose(UIColorHistory ui)
    {
        //   MainScene.isInMainUI = true;
    }
    public void OnClickBtnFreeDraw()
    {
        GameManager.placeLevel = 0;
        //必须在placeLevel设置之后再设置gameLevel
        GameManager.gameLevel = 0;
        GotoGame(UIGamePaint.GAME_MODE_FREE_DRAW);
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
    }


    public void OnClickBtnPlay()
    {
        GotoGame(UIGamePaint.GAME_MODE_NORMAL);
    }
    public void GotoGame(int mode)
    {
        UIGamePaint.gameMode = mode;
        UIViewController controller = null;

        if (UIGamePaint.gameMode == UIGamePaint.GAME_MODE_FREE_DRAW)
        {
            controller = GameViewController.main;
        }
        else
        {

            int total = GameManager.placeTotal;
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



}
