using System.Collections;
using System.Collections.Generic;
using Moonma.SysImageLib;
using UnityEngine;
using UnityEngine.UI;

public class UIHomePintu : UIHomeBase//, ISysImageLibDelegate
{
    public GameObject objLayoutBtn;
    public Button btnPlay;
    public Button btnPhoto;
    public Button btnCamera;
    public Button btnNetImage;
    void Awake()
    {
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            TTS.Speek(appname);
        }

        // btnPhoto.gameObject.SetActive(false);
        // btnCamera.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        LayOut();
        OnUIDidFinish();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }

    public void OnClickBtnPlay()
    {
        //AudioPlay.main.PlayAudioClip(audioClipBtn); 
        UIGamePintu.imageSource = GamePintu.ImageSource.GAME_INNER;
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
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
    public override void LayOut()
    {
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
            rctran.anchoredPosition = new Vector2(x, y);
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

        {
            LayOutGrid lygrid = objLayoutBtn.GetComponent<LayOutGrid>();
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            int h_item = 256;
            if (Device.isLandscape)
            {
                lygrid.row = 1;
                lygrid.col = 5;

            }
            else
            {
                lygrid.row = 3;
                lygrid.col = 2;

            }
            w = rctran.rect.width;
            h = h_item * lygrid.row;
            x = 0;
            float y_bottom = -sizeCanvas.y / 2 + topBarHeight + 16;
            y = y_bottom / 2;
            if ((y - h / 2) < y_bottom)
            {
                y = y_bottom + h / 2;
            }

            rctran.sizeDelta = new Vector2(w, h);
            rctran.offsetMin = new Vector2(0, rctran.offsetMin.y);
            rctran.offsetMax = new Vector2(0, rctran.offsetMax.y);
            rctran.anchoredPosition = new Vector2(x, y);

            lygrid.LayOut();
        }

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
