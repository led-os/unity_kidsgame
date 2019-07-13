using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeAdDemo : UIHomeBase
{
    public LayOutGrid layoutBtn;
    public ActionHomeBtn actionAdVideo;
    public ActionHomeBtn actionAdBanner;
    public ActionHomeBtn actionAdInsert;
    public ActionHomeBtn actionAdofferWall;
    List<object> listItem;
    float timeAction;
    bool isActionFinish;
    

    void Awake()
    {
        PlayerPrefs.SetInt(AppVersion.STRING_KEY_APP_CHECK_FINISHED, Common.Bool2Int(true));

        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);

        timeAction = 0.3f;
        isActionFinish = false;


        listItem = new List<object>();
        listItem.Add(actionAdBanner);
        listItem.Add(actionAdInsert);
        listItem.Add(actionAdVideo);
        listItem.Add(actionAdofferWall);


        layoutBtn.enableLayout = false;
        UpdateLayoutBtn();
    }

    // Use this for initialization
    void Start()
    {
        isActionFinish = false;
        LayOut();
        foreach (object obj in listItem)
        {
            ActionHomeBtn action = obj as ActionHomeBtn;
            action.RunAction();
        }
        Invoke("OnUIDidFinish", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }
    void UpdateLayoutBtn()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        int row = 0;
        int col = 0;

        {
            LayOutGrid lygrid = layoutBtn;
            RectTransform rctran = layoutBtn.gameObject.GetComponent<RectTransform>();
            int h_item = 256;
            int oft = 16;

            lygrid.row = 2;
            lygrid.col = 2;



            int idx = 0;
            foreach (object obj in listItem)
            {
                row = idx / lygrid.col;
                row = lygrid.row - 1 - row;
                col = idx % lygrid.col;
                ActionHomeBtn action = obj as ActionHomeBtn;
                action.ptNormal = layoutBtn.GetItemPostion(row, col);
                idx++;
            }

            w = (h_item + oft) * lygrid.col;
            h = (h_item + oft) * lygrid.row;
            x = 0;
            float y_bottom = -sizeCanvas.y / 2 + topBarHeight + 16;
            Debug.Log("y_bottom=" + y_bottom);
            y = y_bottom / 2;


            float h_appcenter = 0;
            //居中
            float y1 = -sizeCanvas.y / 2 + h_appcenter;
            float y2 = sizeCanvas.y / 2;
            y = (y1 + y2) / 2;


            if ((y - h / 2) < y_bottom)
            {
                y = y_bottom + h / 2;
            }

            rctran.sizeDelta = new Vector2(w, h);
            // rctran.offsetMin = new Vector2(0, rctran.offsetMin.y);
            // rctran.offsetMax = new Vector2(0, rctran.offsetMax.y);
            rctran.anchoredPosition = new Vector2(x, y);

            lygrid.LayOut();
        }

    }
    public void OnClickBtnAdBanner()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(AdBannerViewController.main);
        }

    }

    public void OnClickBtnAdInsert()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(AdInsertViewController.main);
        }

    }
    public void OnClickBtnAdVideo()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(AdVideoViewController.main);
        }

    }
    public void OnClickBtnAdOfferWall()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(AdOfferWallViewController.main);
        }

    }

    public override void LayOut()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        UpdateLayoutBtn();

        LayoutChildBase();
    }
}
