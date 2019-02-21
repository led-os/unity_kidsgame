using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIHomeFillColor : UIHomeBase, IPopViewControllerDelegate
{


    public Button btnHistory;
    public Button btnMore;
    public Button btnPlay;
    public Button btnSetting;
    public GameObject objLayoutBtn;


    // [SerializeField] protected GameObject objBtn;  
    void Awake()
    {
        if (btnAdVideo != null)
        {
            btnAdVideo.gameObject.SetActive(true);
            if ((Common.noad) || (!AppVersion.appCheckHasFinished))
            {
                btnAdVideo.gameObject.SetActive(false);
            }

        }
        if (!AppVersion.appCheckHasFinished)
        {
            btnMore.gameObject.SetActive(false);
        }
        //bg
        int oft = 16;
        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        Common.SetButtonText(btnHistory, Language.main.GetString("STR_HISTORY_TITLE"), oft);
        Common.SetButtonText(btnMore, Language.main.GetString("STR_MORE"), oft);
        Common.SetButtonText(btnSetting, Language.main.GetString("STR_SETTING"), oft);
        Common.SetButtonText(btnAdVideo, Language.main.GetString("BTN_ADVIDEO"), oft);


    }
    // Use this for initialization
    void Start()
    {
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (!Config.main.APP_FOR_KIDS)
        {
            ret = false;
        }
        if (ret)
        {
            TTS.Speek(appname);
        }





        LayOutChild();

        OnUIDidFinish();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }


    void LayOutChild()
    {
        float x = 0, y = 0, w = 0, h = 0;
        Vector2 sizeCanvas = this.frame.size;
        float h_play = 0;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;
        //play
        {
            RectTransform rctran = btnPlay.GetComponent<RectTransform>();
            x = 0;
            y = 0;
            rctran.anchoredPosition = new Vector2(x, y);
            h_play = rctran.rect.height;
        }

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
    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        //MainScene.isInMainUI = true;
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
        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_GUANKA;
            int total = GameManager.placeTotal;
            if (total > 1)
            {
                navi.Push(PlaceViewController.main);
            }
            else
            {
                navi.Push(GuankaViewController.main);
            }
        }

    }



}
