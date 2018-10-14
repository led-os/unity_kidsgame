using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moonma.Tongji;

public class AppSceneBase : ScriptBase
{
    AppVersion appVersion;
    public Image imageBg;
    public UIViewController rootViewController;
    public Canvas canvasMain;
    public GameObject objMainWorld;
    public GameObject objSpriteBg;

    //public Camera mainCamera;
    float _adBannerHeightCanvas = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary> 
    public static AppSceneBase main;

    bool isReLayout = false;

    void Awake()
    {
        isReLayout = false;
        IPInfo.main.StartParserInfo();
        InitScalerMatch();
        Common.CleanCache();
        InitValue();

        //Component
        this.gameObject.AddComponent<AdKitCommon>();
        this.gameObject.AddComponent<IAPCommon>();
        this.gameObject.AddComponent<ShareCommon>();
        this.gameObject.AddComponent<TTSCommon>();
        this.gameObject.AddComponent<AudioPlay>();

        //app启动初始化多线程工具LOOM
        Loom loom = Loom.Current;

        //bg
        // Texture2D tex = AppResImage.texMainBg;
        // if (tex)
        {
            //ios unity 2017.3.1 Sprite.Create 会出现crash
            // Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            // imageBg.sprite = sp;
            // RectTransform rctan = imageBg.GetComponent<RectTransform>();
            // rctan.sizeDelta = new Vector2(tex.width, tex.height);
        }



    }

    // Use this for initialization
    void Start()
    {
        isHasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHasStarted)
        {
            isHasStarted = false;
            InitUiScaler();
            UpdateMainWorldRect(0);
            LayoutChild();
            RunCheckApp();
        }
        if (Device.isScreenDidChange)
        {
            InitScalerMatch();
            isReLayout = true;

        }

        if (isReLayout)
        {
            isReLayout = false;
            //InitScalerMatch 和 InitUiScaler 异步执行
            InitUiScaler();
            UpdateMainWorldRect(_adBannerHeightCanvas);
            LayoutChild();
            if (rootViewController != null)
            {
                rootViewController.UpdateCanvasSize(sizeCanvas);
            }
            // Debug.Log("Device.isScreenDidChange:sizeCanvas = " + sizeCanvas);

        }
    }



    void OnAppVersionFinished(AppVersion app)
    {
        Debug.Log("StartScene OnAppVersionFinished");
        RunApp();
    }

    void InitValue()
    {
        bool isFirstRun = !Common.GetBool(AppString.STR_KEY_NOT_FIRST_RUN);
        mainCamera = Common.GetMainCamera();
        if (main == null)
        {
            main = this;
        }

        Tongji.Init(Config.main.GetString("APPTONGJI_ID", "0"));
        Device.InitOrientation();

        if (isFirstRun)
        {
            Common.gold = AppRes.GOLD_INIT_VALUE;
            //第一次安装
            Common.SetBool(AppString.STR_KEY_NOT_FIRST_RUN, true);

            Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, true);

            int lanTag = (int)Application.systemLanguage;
            PlayerPrefs.SetInt(AppString.STR_KEY_LANGUAGE, lanTag);
            SystemLanguage lan = (SystemLanguage)lanTag;
            Language.main.SetLanguage(lan);
        }
        else
        {

            int lanTag = PlayerPrefs.GetInt(AppString.STR_KEY_LANGUAGE);
            SystemLanguage lan = (SystemLanguage)lanTag;
            //lan = SystemLanguage.English;

            Language.main.SetLanguage(lan);

        }


    }

    void RunCheckApp()
    {
        appVersion = AppVersion.main;
        if (!AppVersion.appCheckHasFinished)
        {
            appVersion.callbackFinished = OnAppVersionFinished;
        }
        else
        {
            appVersion.callbackFinished = null;
            RunApp();
        }
        appVersion.StartParseVersion();
    }

    public virtual void RunApp()
    {
        Debug.Log("base RunApp");

        Common.UnityStartUpFinish();
    }

    public void SetRootViewController(UIViewController controller)
    {
        float x = 0, y = 0;
        if (rootViewController != null)
        {
            rootViewController.DestroyObjController();
        }
        rootViewController = controller;
        rootViewController.SetViewParent(canvasMain.gameObject);


        float oft_top = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetTop);
        float oft_bottom = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetBottom);
        float oft_left = Common.ScreenToCanvasWidth(sizeCanvas, Device.offsetLeft);
        float oft_right = Common.ScreenToCanvasWidth(sizeCanvas, Device.offsetRight);
        RectTransform rctran = rootViewController.objController.GetComponent<RectTransform>();
        y = oft_bottom - oft_top;
        x = oft_left - oft_right;
        //rctran.anchoredPosition = new Vector2(x, y);
        rctran.offsetMin = new Vector2(oft_left, oft_bottom);
        rctran.offsetMax = new Vector2(-oft_right, -oft_top);
    }

    public void ClearMainWorld()
    {
        int idx = 0;
        //FindObjectOfType

        // 
        // Transform[] allChild = objMainWorld.transform.FindObjectOfType(typeof(Transform)) as Transform[];

        /* 
        foreach (Transform child in objMainWorld.transform)这种方式遍历子元素会漏掉部分子元素
        */

        //GetComponentsInChildren寻找的子对象也包括父对象自己本身和子对象的子对象
        foreach (Transform child in objMainWorld.GetComponentsInChildren<Transform>(true))
        {
            if (child == null)
            {
                // 过滤已经销毁的嵌套子对象
                Debug.Log("ClearMainWorld child is null idx=" + idx);
                continue;
            }
            GameObject objtmp = child.gameObject;
            if ((objMainWorld == objtmp) || (objSpriteBg == objtmp))
            {
                continue;
            }
            //Debug.Log("ClearMainWorld idx=" + idx + " name=" + objtmp.name);
            GameObject.DestroyImmediate(objtmp);//Destroy
            objtmp = null;

            idx++;
        }
        // Debug.Log("ClearMainWorld idx=" + idx);
    }

    public void UpdateMainWorldRect(float adBannerHeightCanvas)
    {
        float x, y, w, h;
        _adBannerHeightCanvas = adBannerHeightCanvas;
        float adBannerHeight_world = Common.CanvasToWorldHeight(mainCamera, sizeCanvas, adBannerHeightCanvas);
        //world
        float oft_top = Common.ScreenToWorldHeight(mainCamera, Device.offsetTop);
        float oft_bottom = Common.ScreenToWorldHeight(mainCamera, Device.offsetBottom);
        float oft_left = Common.ScreenToWorldWidth(mainCamera, Device.offsetLeft);
        float oft_right = Common.ScreenToWorldWidth(mainCamera, Device.offsetRight);


        RectTransform rctranWorld = objMainWorld.GetComponent<RectTransform>();
        Vector2 size = Common.GetWorldSize(mainCamera);
        w = size.x - oft_left - oft_right;
        h = size.y - adBannerHeight_world - oft_top - oft_bottom;
        y = mainCamera.orthographicSize - oft_top - h / 2;
        x = 0;
        float z = rctranWorld.position.z;
        rctranWorld.sizeDelta = new Vector2(w, h);
        rctranWorld.position = new Vector3(x, y, z);
        LayoutChild();
    }

    public void LayoutChild()
    {
        if (imageBg != null)
        {
            RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
            float w_image = rectTransform.rect.width;
            float h_image = rectTransform.rect.height;
            print(rectTransform.rect);
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        if (objSpriteBg != null)
        {
            SpriteRenderer render = objSpriteBg.GetComponent<SpriteRenderer>();
            Vector2 worldsize = Common.GetWorldSize(mainCamera);
            Sprite sp = render.sprite;
            if (sp != null)
            {
                Texture2D tex = sp.texture;
                float w = tex.width / 100f;//render.size.x;
                float h = tex.height / 100f;//render.size.y;
                if ((w != 0) && (h != 0)){
                    float scalex = worldsize.x / w;
                    float scaley = worldsize.y / h;
                    float scale = Mathf.Max(scalex, scaley);
                    objSpriteBg.transform.localScale = new Vector3(scale, scale, 1.0f);
                    objSpriteBg.transform.position = new Vector3(0, 0, objSpriteBg.transform.position.z);
                }

            }

        }
    }

    public void UpdateWorldBg(string pic)
    {
        Texture2D tex = TextureCache.main.Load(pic);
        SpriteRenderer render = objSpriteBg.GetComponent<SpriteRenderer>();
        render.sprite = LoadTexture.CreateSprieFromTex(tex);
        LayoutChild();
    }

    public void AddObjToMainWorld(GameObject obj)
    {
        obj.transform.parent = objMainWorld.transform;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    public void AddObjToMainCanvas(GameObject obj)
    {
        obj.transform.parent = canvasMain.transform;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    public RectTransform GetRectMainWorld()
    {
        RectTransform rcTran = objMainWorld.GetComponent<RectTransform>();
        return rcTran;
    }

}
