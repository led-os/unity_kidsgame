using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShotDeviceInfo
{
    public string name;
    public int width;
    public int height;
    public bool isIconHd;
    public SystemLanguage lan;
    public bool isMain;
    public int index;
}

public class UIScreenShotController : UIView
{

    //public Camera mainCam;
    public GameObject objContent;
    public Text textScreen;
    public Button btnClear;
    RenderTexture renderTextureScreen;
    IconConvert iconConvert;
    //ipadpro
    public const int SCREEN_WIDTH_IPADPRO = 2732;
    public const int SCREEN_HEIGHT_IPADPRO = 2048;
    public const string DEVICE_NAME_IPADPRO = "ipadpro";
    //ipad
    public const int SCREEN_WIDTH_IPAD = 2048;
    public const int SCREEN_HEIGHT_IPAD = 1536;
    public const string DEVICE_NAME_IPAD = "ipad";

    //iphone
    public const int SCREEN_WIDTH_IPHONE = 2208;
    public const int SCREEN_HEIGHT_IPHONE = 1242;
    public const string DEVICE_NAME_IPHONE = "iphone";

    //1080p
    public const int SCREEN_WIDTH_1080P = 1920;
    public const int SCREEN_HEIGHT_1080P = 1080;
    public const string DEVICE_NAME_1080P = "1080p";

    //
    public const int SCREEN_WIDTH_480P = 800;
    public const int SCREEN_HEIGHT_480P = 480;
    public const string DEVICE_NAME_480P = "480p";

    //weibo
    public const int SCREEN_WIDTH_WEIBO = 450;
    public const int SCREEN_HEIGHT_WEIBO = 300;
    public const string DEVICE_NAME_WEIBO = "weibo";

    //ad
    public const string DEVICE_NAME_AD = "ad";

    //icon
    public const string DEVICE_NAME_ICON = "icon";
    public const int SCREEN_WIDTH_ICON = 1024;
    public const int SCREEN_HEIGHT_ICON = 1024;

    //copy right huawei
    public const string DEVICE_NAME_COPY_RIGHT_HUAWEI = "copyright";
    public const string DEVICE_NAME_COPY_RIGHT_HD_HUAWEI = "copyright_hd";
    public const int SCREEN_WIDTH_COPY_RIGHT_HUAWEI = 750;
    public const int SCREEN_HEIGHT_COPY_RIGHT_HUAWEI = 1334;


    //pc
    public const int WIDTH_SCREEN_PC = 1920;
    public const int HEIGHT_SCREEN_PC = 1080;

    float screenDisplayWordWidth;
    float screenDisplayWordHeight;

    List<SystemLanguage> listLanguage;
    List<ShotDeviceInfo> listDevice;
    ShotDeviceInfo deviceInfoNow;
    public int indexScreenShot;
    public int totalScreenShot;
    int indexDevice;
    bool isClickNextPreDevice = false;
    long tickTotalTimeSecond;
    //static public ShotBase shotBase;
    bool isRunDoAutoSaveOneScreenShot = false;
    bool isAllMainDeviceFinish;
    ShotItemInfo shotItemInfo;

    ScreenShotConfig screenShotConfig;
    float delayTimeDeviceChange
    {
        get
        {
            float ret = 1f;
            if (Application.isEditor)
            {
                ret = 0.1f;
            }
            else
            {
                ret = 3f;//3f
                if (Common.isWin)
                {
                    ret = 0.2f;//0.2f
                }
            }
            return ret;
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        iconConvert = this.gameObject.AddComponent<IconConvert>();
        screenShotConfig = new ScreenShotConfig();
        //mainCam = Common.GetMainCamera();


        InitDevice();
        SetScreen(deviceInfoNow.width, deviceInfoNow.height);
    }
    // Use this for initialization
    void Start()
    {
        InitShowShot();
    }

    // Update is called once per frame
    void Update()
    {
        if (Device.isScreenDidChange)
        {
            if (isClickNextPreDevice)
            {
                isClickNextPreDevice = false;

                GotoScrenShot(indexScreenShot);

                //ScreenShotViewController controller = ScreenShotViewController.main;
                //  AppSceneBase.main.SetRootViewController(controller);
                //  controller.LayOutView();
                // SceneManager.LoadScene("ScreenShotScene");
            }
            UpdateTitle();
            LayOutChild();
        }
    }
    public void InitShowShot()
    {
        LoadShot();
        UpdateTitle();
        LayOutChild();
    }
    public void InitDevice()
    {
        indexScreenShot = 0;
        indexDevice = 0;

        listLanguage = new List<SystemLanguage>();
        listLanguage.Add(SystemLanguage.Chinese);
        listLanguage.Add(SystemLanguage.English);

        listDevice = new List<ShotDeviceInfo>();


        {


            // //ICON
            CreateDevice(DEVICE_NAME_ICON, SCREEN_WIDTH_ICON, SCREEN_HEIGHT_ICON, false, true);
            //adhome
            CreateDevice(DEVICE_NAME_AD, 1024, 500, false, true);
            CreateDevice(DEVICE_NAME_AD, 1080, 480, false, true);

            // //ipadpro
            CreateDevice(DEVICE_NAME_IPADPRO, SCREEN_WIDTH_IPADPRO, SCREEN_HEIGHT_IPADPRO, true
            , true);
            //iphone
            CreateDevice(DEVICE_NAME_IPHONE, SCREEN_WIDTH_IPHONE, SCREEN_HEIGHT_IPHONE, true, true);


            //  ipad
            CreateDevice(DEVICE_NAME_IPAD, SCREEN_WIDTH_IPAD, SCREEN_HEIGHT_IPAD, true, false);

            //  1080p
            CreateDevice(DEVICE_NAME_1080P, SCREEN_WIDTH_1080P, SCREEN_HEIGHT_1080P, true, false);
            //  480p
            CreateDevice(DEVICE_NAME_480P, SCREEN_WIDTH_480P, SCREEN_HEIGHT_480P, true, true);


            //   weibo
            CreateDevice(DEVICE_NAME_WEIBO, SCREEN_WIDTH_WEIBO, SCREEN_HEIGHT_WEIBO, false, true);

            //   copy right huawei
            CreateDevice(DEVICE_NAME_COPY_RIGHT_HUAWEI, SCREEN_WIDTH_COPY_RIGHT_HUAWEI, SCREEN_HEIGHT_COPY_RIGHT_HUAWEI, false, true);

        }
        deviceInfoNow = listDevice[0];

    }
    void LayOutChild()
    {
        screenDisplayWordWidth = Common.ScreenToWorldWidth(mainCam, deviceInfoNow.width);
        screenDisplayWordHeight = Common.ScreenToWorldHeight(mainCam, deviceInfoNow.height);


        // {
        //     SpriteRenderer spRender = objSpriteBg.GetComponent<SpriteRenderer>();
        //     float scalex = screenDisplayWordWidth / spRender.size.x;
        //     float scaley = screenDisplayWordHeight / spRender.size.y;
        //     float scale = Mathf.Max(scalex, scaley);
        //     Debug.Log("spRender.size=" + spRender.size);
        //     objSpriteBg.transform.localScale = new Vector3(scalex, scaley, 1f);

        // }
    }
    ShotDeviceInfo CreateDeviceItem(string name, int w, int h, SystemLanguage lan, bool isMain, bool isHd)
    {
        ShotDeviceInfo info = new ShotDeviceInfo();
        info.width = w;
        info.height = h;
        info.lan = lan;
        info.name = name;
        info.isIconHd = isHd;
        info.isMain = isMain;
        info.index = listDevice.Count;
        listDevice.Add(info);
        return info;
    }
    void CreateDevice(string name, int w, int h, bool isBoth, bool isMain)
    {
        if (name == DEVICE_NAME_ICON)
        {
            {
                //icon
                ShotDeviceInfo info = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, false);

            }

            {
                //iconhd
                ShotDeviceInfo info = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, true);
            }
            return;
        }


        if (name == DEVICE_NAME_COPY_RIGHT_HUAWEI)
        {
            //copyright
            ShotDeviceInfo info = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, false);
            ShotDeviceInfo infohd = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, true);
            return;
        }

        if (name == DEVICE_NAME_COPY_RIGHT_HD_HUAWEI)
        {
            //copyright hd
            //ShotDeviceInfo infohd = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, true);
            return;
        }

        if (isBoth)
        {
            foreach (SystemLanguage lan in listLanguage)
            {
                //竖屏
                ShotDeviceInfo info = CreateDeviceItem(name, Mathf.Min(w, h), Mathf.Max(w, h), lan, isMain, false);

            }

            foreach (SystemLanguage lan in listLanguage)
            {
                //横屏
                ShotDeviceInfo info = CreateDeviceItem(name, Mathf.Max(w, h), Mathf.Min(w, h), lan, isMain, false);
            }
        }
        else
        {
            foreach (SystemLanguage lan in listLanguage)
            {

                ShotDeviceInfo info = CreateDeviceItem(name, w, h, lan, isMain, false);
            }

        }


    }
    void UpdateTitle()
    {
        string strlan = GetLanguageNameKey();
        string str = "dev:" + GetDeviceNameKey() + " lan:" + strlan + " shot:" + (indexScreenShot + 1) + " deviceinfo:w=" + deviceInfoNow.width + " h=" + deviceInfoNow.height + " screen:w=" + Screen.width + " h=" + Screen.height;
        Debug.Log(str);
        textScreen.text = str;

    }
    void SetScreen(int w, int h)
    {
        if ((w == Screen.width) && (h == Screen.height))
        {
            return;
        }
        int w_new = w;
        int h_new = h;
        if (Common.isWin)
        {
            //windows 超出屏幕不显示,需要適配屏幕大小
            float scalex = WIDTH_SCREEN_PC * 1.0f / w;
            float scaley = HEIGHT_SCREEN_PC * 1.0f / h;
            float scale = Mathf.Min(scalex, scaley);
            w_new = (int)(w * scale);
            h_new = (int)(h * scale);
        }
        Screen.SetResolution(w_new, h_new, false);//false
    }

    public virtual ShotBase CreateGameShot()
    {
        return null;
    }
    public void LoadShot()
    {
        UpdateTotal();
        // shotBase = CreateGameShot();
        // if (shotBase != null)
        // {
        //     shotBase.mainCamera = mainCam;

        //     shotBase.deviceInfo = deviceInfoNow;
        //     shotBase.callbackShotPageFinish = OnShotPageFinish;
        // }
        Debug.Log("totalScreenShot=" + totalScreenShot);
        GotoScrenShot(indexScreenShot);


    }
    public RenderTexture CreateRenderTexture(Texture inputTex)
    {
        //RenderTexture temp = RenderTexture.GetTemporary(inputTex.width, inputTex.height, 0, RenderTextureFormat.ARGB32);  
        RenderTexture rt = new RenderTexture((int)inputTex.width, (int)inputTex.height, 0);
        Graphics.Blit(inputTex, rt);
        // bool ret = SaveRenderTextureToPNG(temp,pngName);  
        // RenderTexture.ReleaseTemporary(temp);  
        return rt;

    }


    //将RenderTexture保存成一张png图片  
    public void SaveRenderTextureToPNG(RenderTexture rt, string filepath)
    {
        Texture2D tex = TextureUtil.RenderTexture2Texture2D(rt);
        byte[] bytes = tex.EncodeToPNG();
        FileStream file = File.Open(filepath, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(tex);
        tex = null;

    }
    string GetLanguageNameKey()
    {
        string strlan = "en";
        if (Language.main.GetLanguage() == SystemLanguage.Chinese)
        {
            strlan = "cn";
        }
        if (Language.main.GetLanguage() == SystemLanguage.English)
        {
            strlan = "en";
        }
        return strlan;
    }
    string GetDeviceNameKey()
    {
        string ret = deviceInfoNow.name;

        if (deviceInfoNow.name == DEVICE_NAME_AD)
        {
            ret = "AdHome";
        }

        return ret;
    }
    public static string GetRootDir()
    {
        string ret = "";
        //         ret = "/Users/moon/sourcecode/unity/product_unity/ScreenShot";
        // #if UNITY_STANDALONE_WIN
        //         ret = "/moon/ScreenShot";
        // #endif
        ret = AppsConfig.ROOT_DIR_PC + "/ProjectIcon/" + Common.appType + "/" + Common.appKeyName;
        return ret;
    }
    string GetSaveDir(ShotDeviceInfo info)
    {
        string ret = "";
        string rootDir = GetRootDir();
        if (info.width > info.height)
        {
            ret = rootDir + "/screenshot/heng";
        }
        else
        {
            ret = rootDir + "/screenshot/shu";
        }

        string strlan = GetLanguageNameKey();


        if (deviceInfoNow.name == DEVICE_NAME_AD)
        {
            ret = rootDir + "/" + info.name;
            ret += "/" + strlan;
        }
        else if (deviceInfoNow.name == DEVICE_NAME_ICON)
        {
            ret = rootDir + "/" + info.name;
            if (info.isIconHd)
            {
                ret += "hd";
            }
        }
        else if ((deviceInfoNow.name == DEVICE_NAME_COPY_RIGHT_HUAWEI) || (deviceInfoNow.name == DEVICE_NAME_COPY_RIGHT_HD_HUAWEI))
        {
            ret = rootDir + "/" + info.name;
            if (info.isIconHd)
            {
                ret += "hd";
            }
        }
        else
        {
            ret += "/" + strlan;
            ret += "/" + info.name;
        }
        //创建文件夹
        Directory.CreateDirectory(ret);
        return ret;
    }

    public void GotoScrenShot(int idx)
    {
        //StartCoroutine(OnGotoScrenShotEnumerator(idx));
        OnGotoScrenShot(idx);
    }

    void OnGotoScrenShot(int idx)
    {
        if (!deviceInfoNow.isMain)
        {
            return;
        }
        // if (shotBase != null)
        // {
        //     shotBase.ShowPage(idx);
        // }
        shotItemInfo = screenShotConfig.GetPage(deviceInfoNow, idx);

        NaviViewController navi = this.controller as NaviViewController;
        if (navi != null)
        {
            navi.Pop();
            navi.Push(shotItemInfo.controller);
        }

        if (shotItemInfo.controller != null)
        {
            shotItemInfo.controller.callbackUIFinish = OnUIDidFinishCallBack;
        }

    }

    public void OnUIDidFinishCallBack()
    {
        if (isRunDoAutoSaveOneScreenShot)
        {
            OnClickSave();
        }
    }


    IEnumerator OnGotoScrenShotEnumerator(int idx)
    {
        yield return null;
        OnGotoScrenShot(idx);
    }
    void UpdateTotal()
    {
        if (deviceInfoNow == null)
        {
            return;
        }
        if ((deviceInfoNow.name == DEVICE_NAME_ICON) || (deviceInfoNow.name == DEVICE_NAME_AD)
         || (deviceInfoNow.name == DEVICE_NAME_COPY_RIGHT_HUAWEI)
          || (deviceInfoNow.name == DEVICE_NAME_COPY_RIGHT_HD_HUAWEI)
         )
        {
            totalScreenShot = 1;
        }
        else
        {
            //totalScreenShot = shotBase.totalShot;
            totalScreenShot = screenShotConfig.GetTotalPage();
        }
    }

    int GetFirstDevice(bool isMain)
    {
        int ret = 0;
        foreach (ShotDeviceInfo info in listDevice)
        {
            bool is_filter = isMain ? info.isMain : (!info.isMain);
            if (is_filter)
            {
                ret = info.index;
                break;
            }

        }
        return ret;
    }

    int GetLastDevice(bool isMain)
    {
        int ret = 0;
        for (int i = listDevice.Count - 1; i >= 0; i--)
        {
            ShotDeviceInfo info = listDevice[i];
            bool is_filter = isMain ? info.isMain : (!info.isMain);
            if (is_filter)
            {
                ret = info.index;
                break;
            }

        }
        return ret;
    }
    void OnDeviceNext(bool isMain, out bool isfind)
    {

        // indexDevice++;
        // if (indexDevice >= listDevice.Count)
        // {
        //     indexDevice = 0;
        // } 
        bool is_find = false;
        int first_device = 0;
        int index = 0;
        foreach (ShotDeviceInfo info in listDevice)
        {
            bool is_filter = isMain ? info.isMain : (!info.isMain);
            if (is_filter)
            {
                if (info.index > indexDevice)
                {
                    indexDevice = info.index;
                    is_find = true;
                    break;
                }
                if (index == 0)
                {
                    first_device = info.index;
                }
                index++;
            }

        }
        if (!is_find)
        {
            //切换到第一个
            indexDevice = first_device;
        }
        isfind = is_find;
        deviceInfoNow = listDevice[indexDevice];
        UpdateTotal();
        screenShotConfig.deviceInfo = deviceInfoNow;

        Language.main.SetLanguage(deviceInfoNow.lan);
        if (isMain)
        {
            SetScreen(deviceInfoNow.width, deviceInfoNow.height);
        }
        //更新
        if (Application.isEditor)
        {
            GotoScrenShot(indexScreenShot);
        }
    }

    public void OnClickDeviceNext()
    {
        isClickNextPreDevice = true;
        bool isFind = false;
        OnDeviceNext(true, out isFind);
    }
    public void OnClickDevicePre()
    {
        isClickNextPreDevice = true;
        OnDevicePre(true);
    }
    void OnDevicePre(bool isMain)
    {

        // indexDevice--;
        // if (indexDevice < 0)
        // {
        //     indexDevice = listDevice.Count - 1;
        // }

        bool is_find = false;
        int first_device = 0;
        int index = 0;
        for (int i = listDevice.Count - 1; i >= 0; i--)
        {
            ShotDeviceInfo info = listDevice[i];
            bool is_filter = isMain ? info.isMain : (!info.isMain);
            if (is_filter)
            {
                if (info.index < indexDevice)
                {
                    indexDevice = info.index;
                    is_find = true;
                    break;
                }
                if (index == 0)
                {
                    first_device = info.index;
                }
                index++;
            }

        }
        if (!is_find)
        {
            //切换到最后一个
            indexDevice = first_device;
        }

        deviceInfoNow = listDevice[indexDevice];

        UpdateTotal();
        screenShotConfig.deviceInfo = deviceInfoNow;

        Language.main.SetLanguage(deviceInfoNow.lan);
        if (isMain)
        {
            SetScreen(deviceInfoNow.width, deviceInfoNow.height);
        }
        UpdateTitle();

        //更新
        if (Application.isEditor)
        {
            GotoScrenShot(indexScreenShot);
        }
    }
    public void OnClickLanguageCN()
    {
        Language.main.SetLanguage(SystemLanguage.Chinese);
        UpdateTitle();
    }

    public void OnClickLanguageEn()
    {
        Language.main.SetLanguage(SystemLanguage.English);
        UpdateTitle();
    }

    public void OnClickScreenShotNext()
    {
        indexScreenShot++;
        if (indexScreenShot >= totalScreenShot)
        {
            indexScreenShot = 0;
        }
        GotoScrenShot(indexScreenShot);
        UpdateTitle();
    }

    public void OnClickScreenShotPre()
    {
        indexScreenShot--;
        if (indexScreenShot < 0)
        {
            indexScreenShot = totalScreenShot - 1;
        }
        GotoScrenShot(indexScreenShot);
        UpdateTitle();
    }
    public void OnClickClearScreenShot()
    {
        if (btnClear != null)
        {
            btnClear.gameObject.SetActive(false);
        }

        string dir = GetRootDir();
        if (Directory.Exists(dir))
        {
            //删除文件夹
            Directory.Delete(dir, true);
        }

    }

    int FindDeviceByName(string name)
    {
        int idx = -1;
        foreach (ShotDeviceInfo info in listDevice)
        {
            {
                if (info.name == name)
                {
                    idx = info.index;
                    break;
                }
            }

        }

        return idx;
    }

    void GotoDevice(int index)
    {
        int idx = index;
        if (idx < 0)
        {
            //切换到第一个
            idx = 0;
        }
        deviceInfoNow = listDevice[idx];
        UpdateTotal();
        screenShotConfig.deviceInfo = deviceInfoNow;

        Language.main.SetLanguage(deviceInfoNow.lan);
        // if (isMain)
        {
            SetScreen(deviceInfoNow.width, deviceInfoNow.height);
        }
        //更新
        if (Application.isEditor)
        {
            GotoScrenShot(indexScreenShot);
        }
        else
        {

            if ((deviceInfoNow.width == Screen.width) && (deviceInfoNow.height == Screen.height))
            {
                //分辨率没有变化
                GotoScrenShot(indexScreenShot);
            }
        }
    }

    public void OnClickBtnWeibo()
    {
        isClickNextPreDevice = true;
        indexDevice = FindDeviceByName(DEVICE_NAME_WEIBO);
        GotoDevice(indexDevice);

    }
    public void OnClickBtnIcon()
    {
        isClickNextPreDevice = true;
        indexDevice = FindDeviceByName(DEVICE_NAME_ICON);
        GotoDevice(indexDevice);
    }

    public void OnClickBtnCopyRight()
    {
        indexScreenShot = 0;
        indexDevice = 0;
        listDevice.Clear();
        {           //   copy right huawei
            CreateDevice(DEVICE_NAME_COPY_RIGHT_HUAWEI, SCREEN_WIDTH_COPY_RIGHT_HUAWEI, SCREEN_HEIGHT_COPY_RIGHT_HUAWEI, false, true);
            //CreateDevice(DEVICE_NAME_COPY_RIGHT_HD_HUAWEI, SCREEN_WIDTH_COPY_RIGHT_HUAWEI, SCREEN_HEIGHT_COPY_RIGHT_HUAWEI, false, true);
        }
        deviceInfoNow = listDevice[0];

        SetScreen(deviceInfoNow.width, deviceInfoNow.height);
        Invoke("OnClickSaveAuto", 2f);
    }

    public void OnClickBtnIconConVert()
    {
        iconConvert.OnConvertAll();
    }

    public void OnClickBtnPngConVert()
    {
        Debug.Log("OnClickBtnPngConVert start");
        OnScreenShotImageConvert();
    }


    //自动保存截图
    void DoAutoSaveOneScreenShot()
    {
        // foreach (ItemInfo info in listGuanka)

        UpdateTotal();
        screenShotConfig.deviceInfo = deviceInfoNow;

        if (indexScreenShot < totalScreenShot)
        {
            long tick = Common.GetCurrentTimeMs();
            Debug.Log("DoAutoSaveOneScreenShot:GotoScrenShot=" + indexScreenShot + " totalScreenShot=" + totalScreenShot);
            // shotBase.ClearAllChild();
            UpdateTitle();
            GotoScrenShot(indexScreenShot);
            // OnClickSave();

            // indexScreenShot++;
            // Invoke("DoAutoSaveOneScreenShot", 0.5f);

            tick = Common.GetCurrentTimeMs() - tick;

            Debug.Log("DoAutoSaveOneScreenShot:idx=" + indexScreenShot + " tick=" + tick + "ms");


        }
        else
        {
            // if (indexDevice == GetLastDevice(true))
            // {


            // else
            {
                indexScreenShot = 0;
                bool isFind = false;
                OnDeviceNext(true, out isFind);
                if (isFind)
                {
                    Invoke("DoAutoSaveOneScreenShot", delayTimeDeviceChange);
                }
                else
                {
                    isAllMainDeviceFinish = true;
                    float tickshot = Common.GetCurrentTimeSecond() - tickTotalTimeSecond;
                    string str = "Time=" + tickshot + "s,all main device has screenShot";
                    Debug.Log(str);
                    textScreen.text = str;

                    // indexScreenShot = 0;
                    // bool isHaveDevice = false;
                    // InitConvertDevice(out isHaveDevice);
                    // if (isHaveDevice)
                    // {
                    //     DoConvertScreenImageSize();
                    // }

                    OnScreenShotImageConvert();
                }


            }

        }


    }

    void OnScreenShotImageConvert()
    {

        indexScreenShot = 0;
        bool isHaveDevice = false;
        InitConvertDevice(out isHaveDevice);
        if (isHaveDevice)
        {
            DoConvertScreenImageSize();
        }

    }

    //获取长宽比例一致的mian device
    ShotDeviceInfo GetMainDevice(ShotDeviceInfo info)
    {
        ShotDeviceInfo infoRet = null;
        foreach (ShotDeviceInfo infotmp in listDevice)
        {
            if (infotmp.isMain)
            {
                float dif = Mathf.Abs(infotmp.width * 1f / infotmp.height - info.width * 1f / info.height);
                Debug.Log("dif=" + dif + " infotmp:w=" + infotmp.width + " h=" + infotmp.height + " info:w=" + info.width + " h=" + info.height);
                if (dif < 0.1f)
                {
                    infoRet = infotmp;
                    break;
                }
            }
        }
        return infoRet;
    }
    void OnConvertImage()
    {
        ShotDeviceInfo infoMain = GetMainDevice(deviceInfoNow);
        if (infoMain == null)
        {
            return;
        }
        Debug.Log("OnConvertImage name =" + infoMain.name);
        string filepath_src = GetSaveScreenShotFilePath(infoMain, false);

        //jpg
        {
            string filepath_dst = GetSaveScreenShotFilePath(deviceInfoNow, false);
            Texture2D texSrc = LoadTexture.LoadFromFile(filepath_src);
            Texture2D texDst = TextureUtil.ConvertSize(texSrc, deviceInfoNow.width, deviceInfoNow.height);
            TextureUtil.SaveTextureToFile(texDst, filepath_dst);
        }

        //png
        {
            string filepath_dst = GetSaveScreenShotFilePath(deviceInfoNow, true);
            Texture2D texSrc = LoadTexture.LoadFromFile(filepath_src);
            Texture2D texDst = TextureUtil.ConvertSize(texSrc, deviceInfoNow.width, deviceInfoNow.height);
            TextureUtil.SaveTextureToFile(texDst, filepath_dst);
        }

    }
    void InitConvertDevice(out bool isHaveDevice)
    {
        isHaveDevice = false;
        //初始化第一个想要转换的设备 
        foreach (ShotDeviceInfo info in listDevice)
        {
            if (!info.isMain)
            {
                indexDevice = info.index;
                isHaveDevice = true;
                break;
            }

        }

        deviceInfoNow = listDevice[indexDevice];
        Debug.Log("InitConvertDevice:indexDevice=" + indexDevice + "name=" + deviceInfoNow.name);
    }
    //转换分辨率
    void DoConvertScreenImageSize()
    {
        UpdateTotal();
        screenShotConfig.deviceInfo = deviceInfoNow;

        if (indexScreenShot < totalScreenShot)
        {
            long tick = Common.GetCurrentTimeMs();

            UpdateTitle();

            OnConvertImage();
            tick = Common.GetCurrentTimeMs() - tick;
            indexScreenShot++;
            Invoke("DoConvertScreenImageSize", 0.1f);
        }
        else
        {
            if (indexDevice == GetLastDevice(false))
            {

                tickTotalTimeSecond = Common.GetCurrentTimeSecond() - tickTotalTimeSecond;
                string str = "Time=" + tickTotalTimeSecond + "s,all  has finished";
                Debug.Log(str);
                textScreen.text = str;
                ShowScreenShotUI(true);
                isRunDoAutoSaveOneScreenShot = false;
            }
            else
            {
                indexScreenShot = 0;
                bool isFind = false;
                OnDeviceNext(false, out isFind);
                Invoke("DoConvertScreenImageSize", 0.1f);
            }

        }

    }

    void GotoMainScene()
    {
        SceneManager.LoadScene(AppCommon.NAME_SCENE_MAIN);//MainScene
    }

    void ShowScreenShotUI(bool isshow)
    {
        objContent.SetActive(isshow);
    }
    public void OnClickSaveAuto()
    {
        // OnClickClearScreenShot();

        isAllMainDeviceFinish = false;
        indexDevice = 0;
        indexScreenShot = 0;
        tickTotalTimeSecond = Common.GetCurrentTimeSecond();
        isRunDoAutoSaveOneScreenShot = true;
        ShowScreenShotUI(false);
        DoAutoSaveOneScreenShot();


    }
    public void OnClickSave()
    {
        StartCoroutine(OnSaveEnumerator());
    }
    void OnSave()
    {
        DoSave();
        if (!isAllMainDeviceFinish)
        {
            indexScreenShot++;
            Invoke("DoAutoSaveOneScreenShot", 0.5f);
        }

    }
    IEnumerator OnSaveEnumerator()
    {
        float time = 0;
        if (Common.isWin)
        {
            time = delayTimeDeviceChange;
        }
        //yield return null;
        yield return new WaitForSeconds(time);
        OnSave();
    }
    Texture2D GetTextureOfScreen()
    {
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        return screenShot;
    }
    Texture2D GetTextureOfCamera(Vector2 size)
    {
        Camera camera = mainCam;
        Rect rect = new Rect(0, 0, deviceInfoNow.width, deviceInfoNow.height);
        Debug.Log("camera.rect=" + camera.rect);
        // 创建一个RenderTexture对象  
        //RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0); 
        Rect camPixselRectOrign = camera.pixelRect;
        //@moon RenderTexture 如果是屏幕的话必须为屏幕大小，不然局部截图时候图片会出错
        RenderTexture rt = new RenderTexture((int)size.x, (int)size.y, 0);


        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        // camera.pixelRect = new Rect((Screen.width - size.x) / 2, (Screen.height - size.y) / 2, size.x, size.y);
        camera.pixelRect = new Rect(0, 0, Screen.width, Screen.height);
        //camera.fieldOfView +=5;
        //camera.rect = new Rect(0, 0, size.x/100, size.y/100);
        camera.targetTexture = rt;
        camera.Render();
        Debug.Log("camera.rect=" + camera.rect);
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
        //ps: camera2.targetTexture = rt;  
        //ps: camera2.Render();  
        //ps: -------------------------------------------------------------------  

        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
        // rect.x =0;
        // rect.y = 0;
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        camera.pixelRect = camPixselRectOrign;

        GameObject.Destroy(rt);
        return screenShot;
    }


    string GetSaveScreenShotFilePath(ShotDeviceInfo info, bool isPng)
    {
        string filedir = GetSaveDir(info);
        //microsoft 需要png
        string ext = ".jpg";
        string filepath = filedir + "/" + (indexScreenShot + 1) + ext;
        if (isPng)
        {
            ext = ".png";
            filepath = filedir + "/png/" + (indexScreenShot + 1) + ext;
            FileUtil.CreateDir(FileUtil.GetFileDir(filepath));
        }

        return filepath;
    }
    public void DoSave()
    {

        string filedir = GetSaveDir(deviceInfoNow);
        bool isSavePng24Bit = false;

        string filepath = GetSaveScreenShotFilePath(deviceInfoNow, false);
        if (deviceInfoNow.name == DEVICE_NAME_AD)
        {
            filepath = filedir + "/ad_home_" + deviceInfoNow.width + "x" + deviceInfoNow.height + ".png";
            isSavePng24Bit = true;
        }
        if (deviceInfoNow.name == DEVICE_NAME_ICON)
        {
            filepath = filedir + "/icon.png";
            //没有alpha
            isSavePng24Bit = true;
        }
        if (deviceInfoNow.name == DEVICE_NAME_COPY_RIGHT_HUAWEI)
        {
            filepath = filedir + "/huawei.png";
        }

        bool isRealGameUI = false;
        if (shotItemInfo != null)
        {
            isRealGameUI = shotItemInfo.isRealGameUI;
        }
        Texture2D screenShot = null;
        if (isRealGameUI)
        {
            screenShot = GetTextureOfScreen();
        }
        else
        {
            //save camera 
            Vector2 size = new Vector2(deviceInfoNow.width, deviceInfoNow.height);
            screenShot = GetTextureOfCamera(size);
        }


        Texture2D texSave = null;
        if (isSavePng24Bit)
        {
            texSave = TextureUtil.ConvertSize(screenShot, screenShot.width, screenShot.height, TextureFormat.RGB24);
        }
        else
        {
            texSave = screenShot;
        }

        //拉伸大小
        if ((texSave.width != deviceInfoNow.width) && (texSave.height != deviceInfoNow.height))
        {
            texSave = TextureUtil.ConvertSize(texSave, deviceInfoNow.width, deviceInfoNow.height, texSave.format);
        }
        bool isSaveFile = true;
        //if (deviceInfoNow.name == DEVICE_NAME_ICON)
        {
            if (FileUtil.FileIsExist(filepath))
            {
                //不覆盖原来的文件
                isSaveFile = false;
            }
        }
        // 最后将这些纹理数据，成一个png图片文件  
        if (isSaveFile)
        {
            TextureUtil.SaveTextureToFile(texSave, filepath);
        }


        //ScreenCapture.CaptureScreenshot(filepath);

        //保存圆角的android  icon
        // if (deviceInfoNow.name == DEVICE_NAME_ICON)
        // {
        //     filepath = filedir + "/icon_android.png";
        //     Texture2D texTmp = RoundRectTexture(screenShot);
        //     TextureUtil.SaveTextureToFile(texTmp, filepath);

        //     //512x512
        //     Texture2D tex512 = TextureUtil.ConvertSize(texTmp, 512, 512);
        //     filepath = filedir + "/icon_android_512.png";
        //     TextureUtil.SaveTextureToFile(tex512, filepath);
        // }

        Debug.Log(string.Format("截屏了一张照片: {0}", filepath));


    }
}
