using System.Collections;
using System.Collections.Generic;
using Moonma.SysImageLib;
using UnityEngine;
using UnityEngine.UI;

public class UIHomePintu : UIHomeBase//, ISysImageLibDelegate
{
    public LayOutGrid layoutBtn;
    public LayOutGrid layoutBtnSide;
    public ActionHomeBtn actionPlay;
    public ActionHomeBtn actionPhoto;
    public ActionHomeBtn actionCamera;
    public ActionHomeBtn actionNetImage;
    public ActionHomeBtn actionLearn;
    public ActionHomeBtn actionAdVideo;
    List<object> listItem;
    void Awake()
    {
        base.Awake();
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret && Config.main.APP_FOR_KIDS)
        {
            TTS.main.Speak(appname);
        }
        if (!AppVersion.appCheckHasFinished)
        {
            actionPhoto.gameObject.SetActive(false);
            actionCamera.gameObject.SetActive(false);
            actionNetImage.gameObject.SetActive(false);
        }

        if (!Config.main.APP_FOR_KIDS)
        {
            actionLearn.gameObject.SetActive(false);
        }

        if (actionAdVideo != null)
        {
            actionAdVideo.gameObject.SetActive(true);
            if ((Common.noad) || (!AppVersion.appCheckHasFinished))
            {
                actionAdVideo.gameObject.SetActive(false);
            }
            if (Common.isAndroid)
            {
                if (Config.main.channel == Source.GP)
                {
                    //GP市场不显示
                    actionAdVideo.gameObject.SetActive(false);
                }
            }
        }

        listItem = new List<object>();
        listItem.Add(actionPlay);
        if (actionPhoto.gameObject.activeSelf)
        {
            listItem.Add(actionPhoto);
        }
        if (actionCamera.gameObject.activeSelf)
        {
            listItem.Add(actionCamera);
        }
        listItem.Add(actionNetImage);
        if (actionAdVideo.gameObject.activeSelf)
        {
            listItem.Add(actionAdVideo);
        }
        if (actionLearn.gameObject.activeSelf)
        {
            listItem.Add(actionLearn);
        }

        layoutBtn.enableLayout = false;
        UpdateLayoutBtn();
        UpdateLayoutBtnSide();

    }

    // Use this for initialization
    void Start()
    {
        base.Start();
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

    public void UpdateLayoutBtnSide()
    {
        float w_item = 160;
        float h_item = 160;
        float oft = 8;
        float x, y, w, h;
        Vector2 sizeCanvas = this.frame.size;

        layoutBtnSide.enableHide = false;

        int child_count = layoutBtnSide.GetChildCount(false);
        if (Device.isLandscape)
        {
            layoutBtnSide.row = 1;
            layoutBtnSide.col = child_count;
        }
        else
        {
            layoutBtnSide.row = child_count;
            layoutBtnSide.col = 1;
        }

        RectTransform rctran = layoutBtnSide.GetComponent<RectTransform>();
        rctran.sizeDelta = new Vector2((w_item + oft) * layoutBtnSide.col, (h_item + oft) * layoutBtnSide.row);
        layoutBtnSide.LayOut();

        GridLayoutGroup gridLayout = uiHomeAppCenter.GetComponent<GridLayoutGroup>();
        Vector2 cellSize = gridLayout.cellSize;
        w = rctran.rect.size.x;
        h = rctran.rect.size.y;
        if (Device.isLandscape)
        {
            x = 0;
            y = -sizeCanvas.y / 2 + h / 2;
        }
        else
        {
            x = sizeCanvas.x / 2 - w / 2;
            y = -sizeCanvas.y / 2 + cellSize.y + h / 2;
        }

        //Debug.Log("layout y1=" + y1 + " y2=" + y2 + " y=" + y + " rctranAppIcon.rect.size.y=" + rctranAppIcon.rect.size.y);


        rctran.anchoredPosition = new Vector2(x, y);

    }
    void UpdateLayoutBtn()
    {
        if(listItem==null){
            return;
        }
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        int row = 0;
        int col = 0;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;
        RectTransform rctranImageName = imageBgName.GetComponent<RectTransform>();
        {
            LayOutGrid lygrid = layoutBtn;
            RectTransform rctran = layoutBtn.gameObject.GetComponent<RectTransform>();
            int h_item = 256;
            int oft = 16;
            if (Device.isLandscape)
            {
                lygrid.row = 1;
                lygrid.col = 6;


            }
            else
            {
                lygrid.row = 3;
                lygrid.col = 2;

            }
            if (!AppVersion.appCheckHasFinished)
            {
                lygrid.row = 2;
                if (!actionLearn.gameObject.activeSelf)
                {
                    lygrid.row = 1;
                }
                lygrid.col = 2;
            }

            int idx = 0;
            foreach (object obj in listItem)
            {
                row = idx / lygrid.col;
                row = lygrid.row - 1 - row;
                col = idx % lygrid.col;
                ActionHomeBtn action = obj as ActionHomeBtn;
                action.ptNormal = layoutBtn.GetItemPostion(actionPlay.gameObject, row, col);
                idx++;
            }

            w = (h_item + oft) * lygrid.col;
            h = (h_item + oft) * lygrid.row;
            x = 0;
            float y_bottom = -sizeCanvas.y / 2 + topBarHeight + 16;
            Debug.Log("y_bottom=" + y_bottom);
            y = y_bottom / 2;

            GridLayoutGroup gridLayout = uiHomeAppCenter.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            float height_appcenter = cellSize.y;
            if (!Device.isLandscape)
            {
                //居中
                float y1 = -sizeCanvas.y / 2 + height_appcenter;
                float y2 = rctranImageName.anchoredPosition.y - rctranImageName.rect.height / 2;
                y = (y1 + y2) / 2;
                // Debug.Log("layoutBtn=" + " y1=" + y1 + " y2=" + y2 + " rctranAppIcon.rect.height=" + rctranAppIcon.rect.height);
            }

            if ((y - h / 2) < y_bottom)
            {
                y = y_bottom + h / 2;
            }
            if (Device.isLandscape)
            {
                y = 0;
            }
            rctran.sizeDelta = new Vector2(w, h);
            // rctran.offsetMin = new Vector2(0, rctran.offsetMin.y);
            // rctran.offsetMax = new Vector2(0, rctran.offsetMax.y);
            rctran.anchoredPosition = new Vector2(x, y);

            lygrid.LayOut();
        }

    }


    public void OnClickBtnPlay()
    {
        //AudioPlay.main.PlayAudioClip(audioClipBtn); 
        UIGamePintu.imageSource = GamePintu.ImageSource.GAME_INNER;
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            int total = LevelManager.main.placeTotal;
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

    public void OnClickBtnPhoto()
    {
        SysImageLib.main.SetObjectInfo(this.gameObject.name, "OnSysImageLibDidOpenFinish");
        SysImageLib.main.OpenImage();
    }
    public void OnClickBtnCamera()
    {
        SysImageLib.main.SetObjectInfo(this.gameObject.name, "OnSysImageLibDidOpenFinish");
        SysImageLib.main.OpenCamera();
    }

    public void OnClickBtnNetImage()
    {
        UIGamePintu.imageSource = GamePintu.ImageSource.NET;
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(NetImageViewController.main);
        }
    }

    public void OnClickBtnLearn()
    {
        NaviViewController navi = this.controller.naviController;
        navi.Push(LearnProgressViewController.main);

    }

    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
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
            //  rctran.anchoredPosition = new Vector2(x, y);
        }



        // {
        //     RectTransform rctran = imageBgName.GetComponent<RectTransform>();
        //     float w, h;
        //     int fontSize = TextName.fontSize;
        //     int r = fontSize / 2;
        //     w = Common.GetStringLength(TextName.text, AppString.STR_FONT_NAME, fontSize) + r * 2;
        //     h = fontSize * 2;
        //     rctran.sizeDelta = new Vector2(w, h);
        // }

        UpdateLayoutBtn();
        UpdateLayoutBtnSide();
        LayoutChildBase();
    }

    void OnSysImageLibDidOpenFinish(string file)
    {
        Texture2D tex = null;
        if (Common.isAndroid)
        {
            int w, h;
            using (var javaClass = new AndroidJavaClass(SysImageLib.JAVA_CLASS))
            {

                //安卓系统解码
                // w = javaClass.CallStatic<int>("GetRGBDataWidth");
                // h = javaClass.CallStatic<int>("GetRGBDataHeight");
                // byte[] dataRGB = javaClass.CallStatic<byte[]>("GetRGBData");
                //   tex = LoadTexture.LoadFromRGBData(dataRGB, w, h);
            }

            //unity解码
            tex = LoadTexture.LoadFromFile(file);

        }
        else
        {
            tex = LoadTexture.LoadFromFile(file);
        }

        if (tex != null)
        {
            UIGamePintu.texGamePic = tex;
            UIGamePintu.imageSource = GamePintu.ImageSource.SYSTEM_IMAGE_LIB;
            if (this.controller != null)
            {
                NaviViewController navi = this.controller.naviController;
                navi.Push(GameViewController.main);
            }

        }
    }
}
