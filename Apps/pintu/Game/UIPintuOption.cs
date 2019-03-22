using System.Collections;
using System.Collections.Generic;
using Moonma.SysImageLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public delegate void OnUIPintuOptionDidCloseDelegate(UIPintuOption pintuOption);
public delegate void OnUIPintuOptionDidSliderDelegate(UIPintuOption pintuOption);
public class UIPintuOption : UIView//, ISysImageLibDelegate
{
    public const string STR_KEY_PINTU_OPTION_BLOCK_N = "STR_KEY_PINTU_OPTION_BLOCK_N";
    public const int PINTU_OPTION_BLOCK_N_DEFAULT = 3;
    public Slider slider;
    public Text textTitle;
    public Button btnBack;
    public Button btnPlay;
    //public GameObject objLayoutBtn;
    public OnUIPintuOptionDidCloseDelegate callbackClose { get; set; }
    public OnUIPintuOptionDidSliderDelegate callbackSlider { get; set; }

    AudioClip audioClipBtn;

    private int[] pintuBlockNum = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    private int heightAdBanner;

    private bool isNeedLayout = true;
    bool isTouchUp;
    float slideValue;
    UIGamePintu game;
    GamePintu.ImageSource imageSource;
    public bool isNeedAdVideoTips
    {
        get
        {
            return Common.Int2Bool(PlayerPrefs.GetInt("KEY_ADVIDEO_TIPS", Common.Bool2Int(true)));
        }
        set
        {

            PlayerPrefs.SetInt("KEY_ADVIDEO_TIPS", Common.Bool2Int(value));
        }
    }

    void Awake()
    {
        isNeedLayout = true;
        isTouchUp = false;
        slideValue = slider.value;
        // textTitle.gameObject.SetActive(false);

        audioClipBtn = AudioCache.main.Load(AppRes.AUDIO_BTN_CLICK);
        if (Device.isLandscape)
        {
            //slider.direction = Slider.Direction.BottomToTop;
        }
        else
        {
            //slider.direction = Slider.Direction.LeftToRight;
        }

        if (UIGamePintu.rowGame <= 0)
        {
            int v = PlayerPrefs.GetInt(STR_KEY_PINTU_OPTION_BLOCK_N);
            if (v <= 0)
            {
                int v_default = PINTU_OPTION_BLOCK_N_DEFAULT;
                PlayerPrefs.SetInt(STR_KEY_PINTU_OPTION_BLOCK_N, v_default);
                v = v_default;
            }
            UIGamePintu.rowGame = v;
            UIGamePintu.colGame = UIGamePintu.rowGame;
        }

        UITouchEventWithMove touch_ev = slider.gameObject.AddComponent<UITouchEventWithMove>();
        touch_ev.callBackTouch = OnSliderTouchEvent;
    }


    // Use this for initialization
    void Start()
    {
        game = GameViewController.main.gameBase as UIGamePintu;
        UpdateTitle();
        UpdateSlider();

        LayOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }

        if (isNeedLayout)
        {
            isNeedLayout = false;
            // RunLayoutDelay();
        }


    }



    void RunLayoutDelay()
    {

        LayOut();
    }


    public override void LayOut()
    {
        float x, y, w, h;
        Vector2 sizeCanvas = this.frame.size;
        float adbaner_h_canvas = Common.ScreenToCanvasHeigt(sizeCanvas, heightAdBanner);
        adbaner_h_canvas = 160;
        float topbarHeightCanvas = 160;
        RectTransform rctranParent = this.GetComponent<RectTransform>();
        //text
        {
            RectTransform rctran = textTitle.GetComponent<RectTransform>();
            w = rctran.rect.width;
            h = rctran.rect.height;
            if (Device.isLandscape)
            {

                x = sizeCanvas.x / 4;
                y = h;
                y += adbaner_h_canvas;
            }
            else
            {
                x = 0;
                // y = -sizeCanvas.y / 4 + h / 2;
                y = rctranParent.rect.height / 2 - topbarHeightCanvas - h / 2 - 16;
            }

            rctran.anchoredPosition = new Vector2(x, y);
        }

        //btnplay

        {
            //RectTransform rctranPlay = btnPlay.GetComponent<RectTransform>();
            RectTransform rctran = btnPlay.GetComponent<RectTransform>();
            w = rctran.rect.width * 3;
            h = rctran.rect.height;
            if (Device.isLandscape)
            {
                x = sizeCanvas.x / 4;
                y = -h;
                y += adbaner_h_canvas;
            }
            else
            {
                x = rctranParent.rect.width / 2 - w / 2 - 16;
                y = rctranParent.rect.height / 2 - h / 2 - 16;
            }

            rctran.anchoredPosition = new Vector2(x, y);
        }

        //slider
        {
            RectTransform rctran = slider.GetComponent<RectTransform>();
            float oft = 20;
            if (Device.isLandscape)
            {
                w = sizeCanvas.x / 2 - oft * 2;
                x = sizeCanvas.x / 4;
            }
            else
            {
                w = sizeCanvas.x - oft * 2;
                x = 0;
            }
            h = rctran.rect.height;
            y = -sizeCanvas.y / 2;
            y += (adbaner_h_canvas + Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemHomeBar));
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
        }



    }
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
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
            if (game != null)
            {
                UIGamePintu.texGamePic = tex;
                UIGamePintu.imageSource = GamePintu.ImageSource.SYSTEM_IMAGE_LIB;
                game.UpdateGuankaLevel(GameManager.gameLevel);
            }

        }


        //恢复原来的
        UIGamePintu.imageSource = imageSource;
    }
 

    public void OnSliderTouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {

        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                isTouchUp = false;
                break;

            case UITouchEvent.STATUS_TOUCH_MOVE:
                isTouchUp = false;
                break;

            case UITouchEvent.STATUS_TOUCH_UP:
                isTouchUp = true;
                OnSliderValueChanged(slider.value);

                break;

        }
    }

    void PlayBtnClickSound()
    {
        {
            //AudioPlayer对象在场景切换后可能从当前scene移除了
            GameObject audioPlayer = GameObject.Find("AudioPlayer");
            if (audioPlayer != null)
            {
                AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioClipBtn);
            }
        }
    }
    public void OnClickBtnBack()
    {
        PlayBtnClickSound();
        if (game != null)
        {
            game.OnClickBtnBack();
        }

    }

    public void OnClickBtnPhoto()
    {
        imageSource = UIGamePintu.imageSource;

        SysImageLib.main.SetObjectInfo(this.gameObject.name, "OnSysImageLibDidOpenFinish");
        SysImageLib.main.OpenImage();
    }
    public void OnClickBtnCamera()
    {
        imageSource = UIGamePintu.imageSource;

        SysImageLib.main.SetObjectInfo(this.gameObject.name, "OnSysImageLibDidOpenFinish");
        SysImageLib.main.OpenCamera();
    }

    public void OnBtnClickPlay()
    {
        PlayBtnClickSound();
        Show(false);
        if (this.callbackClose != null)
        {
            this.callbackClose(this);
        }


        //GameScene.ShowAdInsert(100);
    }

    public void OnSliderValueChanged(float value)
    {
        if (value == slideValue)
        {
            Debug.Log("Slider no value need to change");
            return;
        }


        if (isTouchUp)
        {
            slideValue = slider.value;
            if (Application.isEditor)
            {
                DoSliderValueChanged();
                return;
            }
            if (Common.noad || (!AppVersion.appCheckHasFinished))
            {
                DoSliderValueChanged();
                return;
            }
            if (isNeedAdVideoTips)
            {
                game.ShowAdVideoTips();
            }
            else
            {
                game.OnShowAdVideo();
            }
        }
    }
    public void DoSliderValueChanged()
    {
        float value = slideValue;
        isNeedAdVideoTips = false;



        // Slider.SliderEvent e;
        int length = pintuBlockNum.Length;
        int i = (int)((length - 1) * value);
        int n = pintuBlockNum[i];

        UIGamePintu.rowGame = n;
        UIGamePintu.colGame = n;

        PlayerPrefs.SetInt(STR_KEY_PINTU_OPTION_BLOCK_N, n);
        UpdateTitle();


        if (isTouchUp)
        {
            slideValue = slider.value;
            if (this.callbackSlider != null)
            {
                this.callbackSlider(this);
            }
        }


    }



    void UpdateTitle()
    {

        int total = UIGamePintu.rowGame * UIGamePintu.colGame;
        textTitle.text = Language.main.GetString("STR_PINTU_OPTION_BLOCK") + ":" + total;
    }
    void UpdateSlider()
    {
        int len = pintuBlockNum.Length;
        for (int i = 0; i < len; i++)
        {
            if (pintuBlockNum[i] == UIGamePintu.rowGame)
            {
                float value = i * 1.0f / (len - 1);
                slider.value = value;
                break;
            }
        }
    }



    public void AdBannerDidReceiveAd(int w, int h)
    {
        heightAdBanner = h;
        // Debug.LogFormat("AdBanner h={0}", h);
        LayOut();
        //Invoke("LayOutChild", 0.12f);
    }

}
