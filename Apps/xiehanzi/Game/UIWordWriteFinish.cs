using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUIWordWriteFinishDelegate(UIWordWriteFinish ui);
public class UIWordWriteFinish : UIView
{
    public Image imageBg;
    public Image imageWord;
    public Image imageWrite;
    public Image imageBar;
    public Text textTitle;
    public Button btnBack;
    public Button btnHistory;
    public Button btnRewrite;
    public Button btnShare;
    public GameObject objBtnLayout;
    public GameObject objTopBar;

    public int gameLevelNow;
    WordItemInfo wordInfo;

    public OnUIWordWriteFinishDelegate callbackClose { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {


        //bg

        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_GAME_BG, true);


        {
            string str = Language.main.GetString(AppXieHanzi.STR_WORD_WRITE_FINISH_TITLE);
            textTitle.text = str;
            int fontsize = textTitle.fontSize;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
            RectTransform rctran = imageBar.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + fontsize + oft * 2;
            rctran.sizeDelta = sizeDelta;
            //rctran.anchoredPosition = new Vector2(sizeCanvas.x / 2, rctran.anchoredPosition.y);
        }

        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }
    }

    // Use this for initialization
    void Start()
    {

        InitUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }

    }

    void InitUI()
    {

        LayOutChild();
    }
    void LayOutChild()
    {
        float x, y, w = 0, h = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        //bg
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            w = imageBg.sprite.texture.width;
            h = imageBg.sprite.texture.height;
            rctran.sizeDelta = new Vector2(w, h);
            float scalex = sizeCanvas.x / w;
            float scaley = sizeCanvas.y / h;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }

        {
            RectTransform rctran = objBtnLayout.GetComponent<RectTransform>();
            float ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemHomeBar);
            if (Device.isLandscape)
            {
                h = rctran.rect.size.y;
                x = -sizeCanvas.x / 4;
                y = -sizeCanvas.y / 2 + AppCommon.HEIGHT_AD_BANNER_CANVAS + h / 2 + ofty;
            }
            else
            {
                x = 0;
                y = 0;
            }
            rctran.anchoredPosition = new Vector2(x, y);

        }

        //word image
        {
            RectTransform rctran = imageWord.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                x = -sizeCanvas.x / 4;
                y = 0;
                w = sizeCanvas.x / 4;
                h = w;
            }
            else
            {
                x = 0;
                y = sizeCanvas.y / 4;
                w = sizeCanvas.y / 4;
                h = w;

            }
            rctran.anchoredPosition = new Vector2(x, y);
            rctran.sizeDelta = new Vector2(w, h);
        }


        //word write
        {
            RectTransform rctran = imageWrite.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                x = sizeCanvas.x / 4;
                y = 0;
                w = sizeCanvas.x / 4;
                h = w;
            }
            else
            {
                x = 0;
                y = -sizeCanvas.y / 4;
                w = sizeCanvas.y / 4;
                h = w;

            }
            rctran.anchoredPosition = new Vector2(x, y);
            rctran.sizeDelta = new Vector2(w, h);
        }


    }

    public void UpdateItem(WordItemInfo info)
    {
        wordInfo = info;
        TextureUtil.UpdateImageTexture(imageWord, wordInfo.imageLetter, true);
        TextureUtil.UpdateImageTexture(imageWrite, wordInfo.fileSaveWord, true);
        LayOutChild();

    }
    void ShowShare()
    {

        ShareViewController.main.callBackClick = OnUIShareDidClick;
        ShareViewController.main.Show(null, null);
    }

    public void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIWRITE_FINISH_SHARE_TITLE");
        string detail = Language.main.GetString("UIWRITE_FINISH_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }

    void OnClose()
    {
        if (callbackClose != null)
        {
            callbackClose(this);
        }
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
    }
    public void OnClickBtnBack()
    {
        OnClose();
    }
    public void OnClickBtnHistory()
    {
        OnClose();
       // HistoryViewController.main.Show(null, null);
        // if (this.controller != null)
        {
            NaviViewController navi = GameViewController.main.naviController;
            //navi.source = AppRes.SOURCE_NAVI_HISTORY;
            navi.Push(HistoryViewController.main);
        }

    }
    public void OnClickBtnRewrite()
    {
        OnClose();
        GameManager.gameLevel = gameLevelNow;
        GameManager.main.GotoGame(GuankaViewController.main);
    }

    public void OnClickBtnShare()
    {
        ShowShare();
    }

}
