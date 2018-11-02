using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShapeColorItemInfo : ItemInfo
{
    public string picInner;
    public string picOuter;
    public Color color;
    public bool isMain;//主要项
    public bool isInner;
    public string colorid;
    public int i;
    public int j;
    public bool textureHasLoad;//web 纹理是否下载完成
    public List<object> listColorFilter;

}

public class UIGameShapeColor : UIGameBase
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public const int GUANKA_NUM_PER_ITEM = 5;
    public const int GAME_MODE_SHAPE = 0;
    public const int GAME_MODE_COLOR = 1;
    public const int GAME_MODE_SHAPE_COLOR = 2;

    public const string STR_KEY_GAME_STATUS_SHAPE = "KEY_GAME_STATUS_SHAPE_";
    public const string STR_KEY_GAME_STATUS_COLOR = "KEY_GAME_STATUS_COLOR_";
    public const int GAME_STATUS_UN_START = 0;//没有开始
    public const int GAME_STATUS_PLAY = 1;//进行中
    public const int GAME_STATUS_FINISH = 2;//完成

    public const int TAG_ITEM_LOCK = -1;
    public const int TAG_ITEM_UNLOCK = 0;

    public Image imagePic;

    // public GameEndParticle gameEndParticle;
    static public string strWord3500;
    string strPlace;

    List<object> listBg;
    static public List<object> listShape;
    static public List<object> listColor;
    List<object> listColorShow;
    List<object> listItem;
    bool isItemHasSel;
    Vector2 ptDownScreen;
    Vector3 posItemWorld;
    ShapeColorItemInfo itemInfoSel;
    float itemPosZ = -20f;

    Shader shaderColor;

    AudioClip audioClipItemFinish;
    Camera mainCamera;

    int totalRow = 0;
    int totalCol = 0;
    public HttpRequest httpReqBgJson;
    public HttpRequest httpReqGuankaJsonShape;
    public HttpRequest httpReqGuankaJsonColor;
    void Awake()
    {
        mainCamera = AppSceneBase.main.mainCamera;
        //bg

        string strshader = "Custom/ShapeColor";
        shaderColor = Shader.Find(strshader);

        // audioClipItemFinish = (AudioClip)Resources.Load(AppResAudio.RES_AUDIO_PINTU_BLOCK_FINISH);
        InitLanguage();
        ParseGuanka();
    }


    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(GameManager.gameLevel);
    }

    // Update is called once per frame 
    void Update()
    {


        // mobile touch
        if ((Input.touchCount > 0))
        {
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    onTouchDown();
                    break;

                case TouchPhase.Moved:
                    onTouchMove();
                    break;

                case TouchPhase.Ended:
                    onTouchUp();
                    break;

            }
        }


        //pc mouse
        //#if UNITY_EDITOR
        if ((Input.touchCount == 0))
        {
            if (Input.GetMouseButtonUp(0))
            {
                onTouchUp();
            }



            if (Input.GetMouseButtonDown(0))
            {
                //  Debug.Log("Input:" + Input.mousePosition);
                onTouchDown();
            }


            if (Input.GetMouseButton(0))
            {
                onTouchMove();
            }

        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }

    public override void PreLoadDataForWeb()
    {
        ParseGuanka();
    }
    public override void UpdateGuankaLevel(int level)
    {
        AppSceneBase.main.ClearMainWorld();
        LoadGame(gameMode);
        //必须在LoadGame之后执行
        LoadBg();

        ShowUserGuide();
        OnUIDidFinish();
    }
    void InitLanguage()
    {
        string filepath = Common.GAME_RES_DIR + "/language/language.csv";
        if (Common.isWeb)
        {
            httpReqLanguage = new HttpRequest(OnHttpRequestFinished);
            httpReqLanguage.Get(HttpRequest.GetWebUrlOfAsset(filepath));
        }
        else
        {
            byte[] data = FileUtil.ReadDataAuto(filepath);
            OnGetLanguageFileDidFinish(FileUtil.FileIsExistAsset(filepath), data, true);
        }


    }

    void OnGetBgJsonDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (isSuccess)
        {
            ParseBgList(data);
            if (!isLocal)
            {
                LoadBg();
            }

        }
    }

    void OnGetLanguageFileDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (languageGame != null)
        {
            languageGame.SetLanguage(Language.main.GetLanguage());
            return;
        }
        languageGame = new Language();
        languageGame.Init(data);
        languageGame.SetLanguage(Language.main.GetLanguage());
    }

    void OnGetGuankaJsonShapeDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (isSuccess)
        {
            ParseShape(data);
        }
    }
    void OnGetGuankaJsonColorDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (isSuccess)
        {
            ParseColor(data);
        }
    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (req == httpReqLanguage)
        {
            OnGetLanguageFileDidFinish(isSuccess, data, false);
        }
        if (req == httpReqGuankaJsonShape)
        {
            OnGetGuankaJsonShapeDidFinish(isSuccess, data, false);
        }
        if (req == httpReqGuankaJsonColor)
        {
            OnGetGuankaJsonColorDidFinish(isSuccess, data, false);
        }
        if (req == httpReqBgJson)
        {
            OnGetBgJsonDidFinish(isSuccess, data, false);
        }

    }
    void onTouchDown()
    {


        isItemHasSel = false;
        Vector2 pos = Common.GetInputPosition();
        ptDownScreen = pos;

        Vector3 posword = mainCamera.ScreenToWorldPoint(pos);
        Debug.Log("onTouchDown: " + posword);
        foreach (ShapeColorItemInfo info in listItem)
        {
            bool isLock = IsItemLock(info);
            if (isLock)
            {
                continue;
            }

            Bounds bd = info.obj.GetComponent<SpriteRenderer>().bounds;

            posword.z = bd.center.z;
            Rect rc = new Rect(info.obj.transform.position.x - bd.size.x / 2, info.obj.transform.position.y - bd.size.y / 2, bd.size.x, bd.size.y);
            //Debug.Log("left:"+bd+"rc="+rc);
            if (rc.Contains(posword))
            {

                posItemWorld = info.obj.transform.position;
                itemInfoSel = info;
                isItemHasSel = true;
                Debug.Log("itemInfoSel:id:" + itemInfoSel.id + " color:" + itemInfoSel.color);
                break;
            }
        }


    }
    void onTouchMove()
    {
        if (!isItemHasSel)
        {
            Debug.Log("onTouchMove ng 1");
            return;
        }
        bool isLock = IsItemLock(itemInfoSel);
        if (isLock)
        {
            Debug.Log("onTouchMove ng 2");
            return;
        }
        float x, y, w, h;
        Vector2 pos = Common.GetInputPosition();

        Vector2 ptStep = pos - ptDownScreen;
        Vector2 ptStepWorld = Common.ScreenToWorldSize(mainCamera, ptStep);
        Vector3 posStepWorld = new Vector3(ptStepWorld.x, ptStepWorld.y, 0);
        Vector3 posword = posItemWorld + posStepWorld;

        //将选中item暂时置顶
        posword.z = itemPosZ - 2;
        itemInfoSel.obj.transform.position = posword;


        foreach (ShapeColorItemInfo info in listItem)
        {
            isLock = IsItemLock(info);
            if (isLock)
            {
                //  Debug.Log("onTouchMove ng 3");
                // continue;
            }

            if (info == itemInfoSel)
            {
                continue;
            }
            Bounds bd = info.obj.GetComponent<SpriteRenderer>().bounds;

            posword.z = bd.center.z;
            w = bd.size.x / 4;
            h = bd.size.y / 4;
            Rect rc = new Rect(info.obj.transform.position.x - w / 2, info.obj.transform.position.y - h / 2, w, h);
            Debug.Log("onTouchMove:posword=" + posword + "rc=" + rc);
            if ((rc.Contains(posword)) && (itemInfoSel.id == info.id) && (itemInfoSel.color == info.color))
            {
                Debug.Log("合并正确");
                //合并正确
                SetItemLock(info, true);
                SetItemLock(itemInfoSel, true);

                itemInfoSel.obj.transform.position = info.obj.transform.position;
                RunDisapperAnimation(itemInfoSel);
                PlayAudioItemFinish(itemInfoSel);


                //记录游戏开始进行中
                int level = GameManager.gameLevel;
                int idx = level / GUANKA_NUM_PER_ITEM;
                int idx_sub = level % GUANKA_NUM_PER_ITEM;
                if (idx_sub == 0)
                {
                    ShapeColorItemInfo infoitem;
                    string key;
                    if (gameMode == GAME_MODE_SHAPE)
                    {
                        infoitem = GetItemInfoShapeColor(idx, listShape);
                        key = STR_KEY_GAME_STATUS_SHAPE + infoitem.id;
                        PlayerPrefs.SetInt(key, GAME_STATUS_PLAY);
                    }
                    if (gameMode == GAME_MODE_COLOR)
                    {
                        infoitem = GetItemInfoShapeColor(idx, listColor);
                        key = STR_KEY_GAME_STATUS_COLOR + infoitem.id;
                        PlayerPrefs.SetInt(key, GAME_STATUS_PLAY);
                    }

                }



                break;
            }
        }

        CheckGameWin();

    }
    void onTouchUp()
    {
        if (!isItemHasSel)
        {
            return;
        }
        bool isLock = IsItemLock(itemInfoSel);
        if (isLock)
        {
            return;
        }

        //将选中item清除置顶
        Vector3 pos = itemInfoSel.obj.transform.position;
        itemInfoSel.obj.transform.position = pos;
    }
    public ShapeColorItemInfo GetItemInfoById(string strid)
    {
        ShapeColorItemInfo inforet = null;
        foreach (ShapeColorItemInfo info in listItem)
        {
            if (info.id == strid)
            {
                inforet = info;
                break;
            }
        }
        return inforet;
    }

    public override void LayOut()
    {
        foreach (ShapeColorItemInfo info in listItem)
        {
            // if (Common.isWeb)
            {
                if (!info.textureHasLoad)
                {
                    //  continue;
                }
            }

            GameObject obj = info.obj;

            SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();
            if (objSR.sprite == null)
            {
                continue;
            }
            Rect rc = GetRectItem(info.i, info.j, totalRow, totalCol);
            float scale = GetItmeScaleInRect(rc, obj);
            obj.transform.localScale = new Vector3(scale, scale, 1f);



            Bounds bd = objSR.bounds;
            float offsetx = bd.size.x / 2;
            //offsetx =0;
            float offsety = bd.size.y / 2;
            //offsety=0;
            Vector2 pt = RandomPointOfRect(rc, offsetx, offsety);
            Debug.Log("LayOut:i=" + info.i + " j=" + info.j + " rc=" + rc + " pt=" + pt + " bd=" + bd.size);
            obj.transform.position = new Vector3(pt.x, pt.y, itemPosZ);

        }
    }



    bool IsInColorFilter(ShapeColorItemInfo colorfilter, ShapeColorItemInfo info)
    {
        bool isfilter = false;
        foreach (ShapeColorItemInfo infocolor in colorfilter.listColorFilter)
        {
            if (info.id == infocolor.id)
            {
                isfilter = true;
                break;
            }
        }
        return isfilter;
    }

    void LoadBg()
    {
        List<object> listBgNew = new List<object>();
        foreach (ShapeColorItemInfo infobg in listBg)
        {
            bool isColorFilter = false;
            foreach (ShapeColorItemInfo infocolor in listColorShow)
            {
                isColorFilter = IsInColorFilter(infobg, infocolor);
                if (isColorFilter)
                {
                    break;
                }
            }
            if (!isColorFilter)
            {
                listBgNew.Add(infobg);
            }
        }
        Debug.Log("listBgNew.count = " + listBgNew.Count);

        int rdm = Random.Range(0, listBgNew.Count);

        ShapeColorItemInfo info = GetItemInfoShapeColor(rdm, listBgNew);
        AppSceneBase.main.UpdateWorldBg(info.pic);
        LayOut();
    }

    static public string StringOfGameStatus(int status)
    {
        string str = "";
        switch (status)
        {
            case GAME_STATUS_UN_START:
                str = Language.main.GetString("STR_GAME_STATUS_UN_START");
                break;
            case GAME_STATUS_PLAY:
                str = Language.main.GetString("STR_GAME_STATUS_PLAY");
                break;
            case GAME_STATUS_FINISH:
                str = Language.main.GetString("STR_GAME_STATUS_FINISH");
                break;
        }

        return str;
    }
    public string GameStatusOfShape(ShapeColorItemInfo info)
    {
        int status = PlayerPrefs.GetInt(STR_KEY_GAME_STATUS_SHAPE + info.id);
        string str = StringOfGameStatus(status);
        return str;
    }
    public string GameStatusOfColor(ShapeColorItemInfo info)
    {
        int status = PlayerPrefs.GetInt(STR_KEY_GAME_STATUS_COLOR + info.id);
        string str = StringOfGameStatus(status);
        return str;
    }
    public string ShapeTitleOfItem(ShapeColorItemInfo info)
    {
        // InitLanguage();
        if (languageGame == null)
        {
            return null;
        }
        string str = languageGame.GetString("SHAPE_TITLE_" + info.id);
        return str;
    }
    public string ColorTitleOfItem(ShapeColorItemInfo info)
    {
        //InitLanguage();
        if (languageGame == null)
        {
            return null;
        }
        string str = languageGame.GetString("COLOR_TITLE_" + info.id);
        return str;
    }
    string StringOfItem(ShapeColorItemInfo info)
    {
        string str = "";
        string strColor = languageGame.GetString("COLOR_TITLE_" + info.colorid);
        string strShape = languageGame.GetString("SHAPE_TITLE_" + info.id);
        str = strColor + strShape;
        switch (Language.main.GetLanguage())
        {
            case SystemLanguage.Chinese:

                break;

        }
        return str;
    }
    void PlayAudioItemFinish(ShapeColorItemInfo info)
    {
        //AudioPlayer对象在场景切换后可能从当前scene移除了
        GameObject audioPlayer = GameObject.Find("AudioPlayer");
        if (audioPlayer != null)
        {
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClipItemFinish);
        }

        //speak
        string str = StringOfItem(info);
        TTS.Speek(str);

    }
    void RunDisapperAnimation(ShapeColorItemInfo info)
    {
        iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
    }

    ShapeColorItemInfo GetItemInfo()
    {
        int idx = GameManager.gameLevel;
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ShapeColorItemInfo info = listGuanka[idx] as ShapeColorItemInfo;
        return info;
    }

    ShapeColorItemInfo GetItemInfoShapeColor(int idx, List<object> list)
    {
        if (list == null)
        {
            return null;
        }
        if (idx >= list.Count)
        {
            return null;
        }
        ShapeColorItemInfo info = list[idx] as ShapeColorItemInfo;
        return info;
    }

    List<object> GetOtherItemList(ShapeColorItemInfo info, List<object> list)
    {
        if (list == null)
        {
            return null;
        }
        List<object> listother = new List<object>();
        foreach (ShapeColorItemInfo infotmp in list)
        {
            if (infotmp != info)
            {
                listother.Add(infotmp);
            }
        }
        return listother;

    }

    public override int GetGuankaTotal()
    {
        int count = ParseGuanka();
        return count;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
        if (listShape != null)
        {
            //listShape.Clear();
        }
        if (listColor != null)
        {
            //listColor.Clear();
        }
    }

    void ParseBgList(byte[] data)
    {
        if ((listBg != null) && (listBg.Count != 0))
        {
            return;
        }
        listBg = new List<object>();
        string json = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["list"];
        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            string strdir = Common.GAME_RES_DIR + "/image/bg";
            info.pic = strdir + "/" + (string)item["pic"];
            info.listColorFilter = new List<object>();
            JsonData colorFilter = item["color_filter"];
            for (int j = 0; j < colorFilter.Count; j++)
            {
                JsonData itemtmp = colorFilter[j];
                ShapeColorItemInfo infotmp = new ShapeColorItemInfo();
                infotmp.id = (string)itemtmp["color_id"];
                info.listColorFilter.Add(infotmp);

            }
            listBg.Add(info);
        }
    }
    static public void ParseShape(byte[] data)
    {
        if ((listShape != null) && (listShape.Count != 0))
        {
            return;
        }

        listShape = new List<object>();
        int idx = GameManager.placeLevel;
        string json = Encoding.UTF8.GetString(data);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["list"];
        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            info.id = (string)item["id"];
            string picdir = Common.GAME_RES_DIR + "/image/" + info.id;
            info.pic = picdir + "/" + info.id + ".png";
            info.picInner = picdir + "/" + info.id + "_inner.png";
            info.picOuter = picdir + "/" + info.id + "_outer.png";
            listShape.Add(info);
        }

    }

    static public void ParseColor(byte[] data)
    {
        if ((listColor != null) && (listColor.Count != 0))
        {
            return;
        }

        listColor = new List<object>();
        int idx = GameManager.placeLevel;
        string json = Encoding.UTF8.GetString(data);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["list"];
        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            info.id = (string)item["id"];
            info.color = Common.RGBString2Color((string)item["color"]);
            listColor.Add(info);
        }

    }


    public override int ParseGuanka()
    {
        //ParseBgList();
        {

            string filepath = Common.GAME_RES_DIR + "/image/bg/bg.json";
            if (Common.isWeb)
            {
                httpReqBgJson = new HttpRequest(OnHttpRequestFinished);
                httpReqBgJson.Get(HttpRequest.GetWebUrlOfAsset(filepath));
            }
            else
            {
                byte[] data = FileUtil.ReadDataAuto(filepath);
                OnGetBgJsonDidFinish(true, data, true);
            }

        }



        //  ParseShape();
        {

            string filepath = Common.GAME_RES_DIR + "/guanka/shape.json";
            if (Common.isWeb)
            {
                httpReqGuankaJsonShape = new HttpRequest(OnHttpRequestFinished);
                httpReqGuankaJsonShape.Get(HttpRequest.GetWebUrlOfAsset(filepath));
            }
            else
            {
                byte[] data = FileUtil.ReadDataAuto(filepath);
                OnGetGuankaJsonShapeDidFinish(true, data, true);
            }

        }
        // ParseColor();
        {

            string filepath = Common.GAME_RES_DIR + "/guanka/color.json";
            if (Common.isWeb)
            {
                httpReqGuankaJsonColor = new HttpRequest(OnHttpRequestFinished);
                httpReqGuankaJsonColor.Get(HttpRequest.GetWebUrlOfAsset(filepath));
            }
            else
            {
                byte[] data = FileUtil.ReadDataAuto(filepath);
                OnGetGuankaJsonColorDidFinish(true, data, true);
            }

        }
        int count = 0;
        if (listShape != null)
        {
            count = GUANKA_NUM_PER_ITEM * listShape.Count;
        }
        return count;

    }

    void SetItemLock(ItemInfo info, bool isLock)
    {
        if (isLock)
        {
            info.tag = TAG_ITEM_LOCK;
        }
        else
        {
            info.tag = TAG_ITEM_UNLOCK;
        }
    }
    bool IsItemLock(ItemInfo info)
    {
        bool ret = false;
        if (info.tag == TAG_ITEM_LOCK)
        {
            ret = true;
        }
        return ret;
    }

    float GetItmeScaleInRect(Rect rc, GameObject obj)
    {
        float scale = 1f;
        SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();
        Bounds bd = objSR.bounds;
        float ratio = 0.7f;
        float scalex = rc.width * ratio / bd.size.x;
        float scaley = rc.height * ratio / bd.size.y;
        scale = Mathf.Min(scalex, scaley);
        if (scale > 1f)
        {
            //scale = 1f;
        }
        return scale;
    }
    Rect GetRectItem(int i, int j, int totalRow, int totalCol)
    {
        Rect rc = Rect.zero;
        float x, y, w, h;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float w_world = Common.GetCameraWorldSizeWidth(mainCamera) * 2;
        float height_topbar_canvas = 160f;
        float height_ad_canvas = 128f;
        float height_topbar_world = Common.CanvasToWorldHeight(mainCamera, sizeCanvas, height_topbar_canvas);
        float height_ad_world = Common.CanvasToWorldHeight(mainCamera, sizeCanvas, height_ad_canvas);
        height_ad_world += Common.ScreenToWorldHeight(mainCamera, Device.heightSystemHomeBar);
        if (!Device.isLandscape)
        {
            height_topbar_world += Common.ScreenToWorldHeight(mainCamera, Device.heightSystemTopBar);
        }
        float h_world = mainCamera.orthographicSize * 2;
        w = w_world / totalCol;
        h = (h_world - height_topbar_world - height_ad_world) / totalRow;
        float oftx = -w_world / 2;
        float ofty = -h_world / 2 + height_ad_world;
        x = oftx + w * i;
        y = ofty + h * j;
        rc = new Rect(x, y, w, h);
        return rc;
    }
    Vector2 RandomPointOfRect(Rect rc, float offsetx, float offsety)
    {
        float x, y, w, h;
        w = rc.width - offsetx * 2;
        h = rc.height - offsety * 2;
        int rdx = Random.Range(0, 99);
        //rdx = 50;
        x = rc.x + (offsetx + w * rdx / 100);

        rdx = Random.Range(0, 99);

        //rdx = 50;
        y = rc.y + (offsety + h * rdx / 100);


        return new Vector2(x, y);
    }

    //从数组里随机抽取newsize个元素
    int[] RandomIndex(int size, int newsize)
    {
        List<object> listIndex = new List<object>();
        int total = size;
        for (int i = 0; i < total; i++)
        {
            listIndex.Add(i);
        }

        int[] idxTmp = new int[newsize];
        for (int i = 0; i < newsize; i++)
        {
            total = listIndex.Count;
            int rdm = Random.Range(0, total);
            int idx = (int)listIndex[rdm];
            idxTmp[i] = idx;
            listIndex.RemoveAt(rdm);
        }

        return idxTmp;
    }
    public void OnTextureHttpRequestFinished(bool isSuccess,Texture2D tex, object data)
    {
        ShapeColorItemInfo info = data as ShapeColorItemInfo;
        if (info != null)
        {
            // ShapeColorItemInfo infoitem = GetItemInfoById(info.id);
            // if (infoitem != null)
            // {
            //     infoitem.textureHasLoad = true;
            // }
        }
        LayOut();
    }
    GameObject CreateItem(ShapeColorItemInfo info, bool isInner, Color color)
    {
        float x, y, w, h;
        string name = info.id + "_outer";
        if (isInner)
        {
            name = info.id + "_inner";
        }

        GameObject obj = new GameObject(name);
        obj.transform.parent = AppSceneBase.main.objMainWorld.transform;

        RectTransform rcTran = obj.AddComponent<RectTransform>();
        SpriteRenderer objSR = obj.AddComponent<SpriteRenderer>();
        string pic = info.picOuter;
        if (isInner)
        {
            pic = info.picInner;
        }
        //Sprite sprite = LoadTexture.CreateSprieFromAsset(pic);

        if (Common.isWeb)
        {
            TextureUtil texutil = new TextureUtil();
            info.textureHasLoad = false;
            texutil.UpdateSpriteTextureWeb(obj, HttpRequest.GetWebUrlOfAsset(pic), OnTextureHttpRequestFinished, info);
        }
        else
        {
            TextureUtil.UpdateSpriteTexture(obj, pic);
            info.textureHasLoad = true;
            objSR.sprite.name = info.id;
        }



        obj.transform.position = new Vector3(0, 0, itemPosZ);

        //color

        objSR.material = new Material(shaderColor);
        Material mat = objSR.material;
        mat.SetColor("_ColorShape", color);



        return obj;
    }

    void LoadGame(int mode)
    {
        Debug.Log("LoadGame:mode=" + mode);
        if (listItem != null)
        {
            listItem.Clear();
        }
        else
        {
            listItem = new List<object>();
        }

        if (listColorShow == null)
        {
            listColorShow = new List<object>();
        }
        else
        {
            listColorShow.Clear();
        }

        switch (mode)
        {
            case GAME_MODE_SHAPE:
                LoadGameByShape(mode);
                break;

            case GAME_MODE_COLOR:
                LoadGameByColor(mode);
                break;
            case GAME_MODE_SHAPE_COLOR:
                LoadGameByShapeColor(mode);
                break;


        }
    }

    int CalcRowCol(int total)
    {
        int sqr = (int)Mathf.Sqrt(total);
        if (total > sqr * sqr)
        {
            sqr++;
        }
        return sqr;
    }
    ShapeColorItemInfo AddItem(Rect rc, ShapeColorItemInfo infoshape, ShapeColorItemInfo infocolor, GameObject obj, bool isInner, bool isMain)
    {
        ShapeColorItemInfo infoItem = new ShapeColorItemInfo();
        infoItem.obj = obj;
        infoItem.id = infoshape.id;
        infoItem.color = infocolor.color;
        infoItem.isMain = isMain;
        infoItem.isInner = isInner;
        infoItem.colorid = infocolor.id;
        infoItem.textureHasLoad = infoshape.textureHasLoad;

        if (isInner)
        {
            SetItemLock(infoItem, false);
        }
        else
        {
            SetItemLock(infoItem, true);
        }
        listItem.Add(infoItem);

        /* 
                float scale = GetItmeScaleInRect(rc, obj);
                obj.transform.localScale = new Vector3(scale, scale, 1f);


                SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();
                Bounds bd = objSR.bounds;
                float offsetx = bd.size.x / 2;
                //offsetx =0;
                float offsety = bd.size.y / 2;
                //offsety=0;
                Vector2 pt = RandomPointOfRect(rc, offsetx, offsety);
                // Debug.Log("CreateItem:i=" + i + " j=" + j + " rc=" + rc + " pt=" + pt + " bd=" + bd.size);
                obj.transform.position = new Vector3(pt.x, pt.y, obj.transform.position.z);
        */
        return infoItem;
    }

    void LoadGameByShape(int mode)
    {

        Debug.Log("LoadGameByShape start");
        int level = GameManager.gameLevel;
        int idx = level / GUANKA_NUM_PER_ITEM;
        ShapeColorItemInfo infoshape = GetItemInfoShapeColor(idx, listShape);
        if (infoshape == null)
        {
            Debug.Log("LoadGameByShape null");
            return;
        }

        //shape 数量（单位 inner和outer一对）
        int[] mainShapePair = { 1, 2, 2, 2, 3 };
        int[] otherShapeNum = { 0, 0, 1, 2, 2 };

        int idx_sub = level % GUANKA_NUM_PER_ITEM;
        int totalMainShape = mainShapePair[idx_sub] * 2;
        int totalOtherShape = otherShapeNum[idx_sub];

        int totalItem = totalMainShape + totalOtherShape;
        totalRow = CalcRowCol(totalItem);
        totalCol = totalRow;

        Debug.Log("totalItem=" + totalItem + " row=" + totalRow + " col=" + totalCol);

        int[] indexRectList = RandomIndex(totalRow * totalCol, totalItem);

        //mainshape


        int[] indexColor = RandomIndex(listColor.Count, totalMainShape / 2);
        for (int k = 0; k < totalMainShape; k++)
        {
            int indexRect = indexRectList[k];
            int i = indexRect % totalCol;
            int j = indexRect / totalRow;

            int idx_color = indexColor[k / 2];
            ShapeColorItemInfo infocolor = listColor[idx_color] as ShapeColorItemInfo;

            GameObject obj = null;
            bool isInner = (k % 2 == 0) ? true : false;
            listColorShow.Add(infocolor);
            obj = CreateItem(infoshape, isInner, infocolor.color);
            Rect rc = GetRectItem(i, j, totalRow, totalCol);


            ShapeColorItemInfo infoitem = AddItem(rc, infoshape, infocolor, obj, isInner, true);
            infoitem.i = i;
            infoitem.j = j;
        }

        //othershape
        indexColor = RandomIndex(listColor.Count, totalOtherShape);
        List<object> listOther = GetOtherItemList(infoshape, listShape);
        int[] indexOther = RandomIndex(listOther.Count, totalOtherShape);
        for (int k = 0; k < totalOtherShape; k++)
        {
            int indexRect = indexRectList[totalMainShape + k];
            int i = indexRect % totalCol;
            int j = indexRect / totalRow;

            int idxtmp = indexOther[k];
            ShapeColorItemInfo infoOther = listOther[idxtmp] as ShapeColorItemInfo;

            int idx_color = indexColor[k];
            if (mode == GAME_MODE_SHAPE)
            {
                // 统一颜色
                idx_color = indexColor[0];
            }
            ShapeColorItemInfo infocolor = listColor[idx_color] as ShapeColorItemInfo;
            Color color = infocolor.color;
            listColorShow.Add(infocolor);
            GameObject obj = CreateItem(infoOther, true, color);
            Rect rc = GetRectItem(i, j, totalRow, totalCol);

            ShapeColorItemInfo infoitem = AddItem(rc, infoOther, infocolor, obj, true, false);
            infoitem.i = i;
            infoitem.j = j;
        }

    }

    void LoadGameByColor(int mode)
    {


        int level = GameManager.gameLevel;
        int idx = level / GUANKA_NUM_PER_ITEM;
        ShapeColorItemInfo infocolor = GetItemInfoShapeColor(idx, listColor);
        if (infocolor == null)
        {
            return;
        }

        listColorShow.Add(infocolor);

        //color 数量（单位 inner和outer一对）
        int[] mainColorPair = { 1, 2, 2, 2, 3 };
        int[] otherColorNum = { 0, 0, 1, 2, 2 };

        int idx_sub = level % GUANKA_NUM_PER_ITEM;
        int totalMainColor = mainColorPair[idx_sub] * 2;
        int totalOtherColor = otherColorNum[idx_sub];

        int totalItem = totalMainColor + totalOtherColor;
        totalRow = CalcRowCol(totalItem);
        totalCol = totalRow;


        int[] indexRectList = RandomIndex(totalRow * totalCol, totalItem);

        //maincolor


        int[] indexShape = RandomIndex(listShape.Count, totalMainColor / 2);
        for (int k = 0; k < totalMainColor; k++)
        {
            int indexRect = indexRectList[k];
            int i = indexRect % totalCol;
            int j = indexRect / totalRow;

            int idx_shape = indexShape[k / 2];
            if (mode == GAME_MODE_COLOR)
            {
                // 统一形状
                //idx_shape = indexShape[0];
            }
            ShapeColorItemInfo infoshape = listShape[idx_shape] as ShapeColorItemInfo;

            GameObject obj = null;
            bool isInner = (k % 2 == 0) ? true : false;
            obj = CreateItem(infoshape, isInner, infocolor.color);
            Rect rc = GetRectItem(i, j, totalRow, totalCol);

            ShapeColorItemInfo infoitem = AddItem(rc, infoshape, infocolor, obj, isInner, true);
            infoitem.i = i;
            infoitem.j = j;
        }

        //othershape
        indexShape = RandomIndex(listShape.Count, totalOtherColor);
        List<object> listOther = GetOtherItemList(infocolor, listColor);
        int[] indexOther = RandomIndex(listOther.Count, totalOtherColor);
        for (int k = 0; k < totalOtherColor; k++)
        {
            int indexRect = indexRectList[totalMainColor + k];
            int i = indexRect % totalCol;
            int j = indexRect / totalRow;

            int idxtmp = indexOther[k];
            ShapeColorItemInfo infoOther = listOther[idxtmp] as ShapeColorItemInfo;

            int idx_shape = indexShape[k];
            if (mode == GAME_MODE_COLOR)
            {
                // 统一颜色
                //idx_shape = indexShape[0];
            }
            ShapeColorItemInfo infoshape = listShape[idx_shape] as ShapeColorItemInfo;
            Color color = infoOther.color;

            GameObject obj = CreateItem(infoshape, true, color);
            Rect rc = GetRectItem(i, j, totalRow, totalCol);

            ShapeColorItemInfo infoItem = AddItem(rc, infoshape, infoOther, obj, true, false);
            infoItem.i = i;
            infoItem.j = j;
        }

    }


    void LoadGameByShapeColor(int mode)
    {

        Debug.Log("LoadGameByShape start");
        int level = GameManager.gameLevel;
        int idx = level / GUANKA_NUM_PER_ITEM;
        ShapeColorItemInfo infoshape = GetItemInfoShapeColor(idx, listShape);
        if (infoshape == null)
        {
            Debug.Log("LoadGameByShape null");
            return;
        }

        //shape 数量（单位 inner和outer一对）
        int[] mainShapePair = { 2, 3, 3, 3, 4 };
        int[] otherShapeNum = { 0, 1, 2, 3, 3 };

        int idx_sub = level % GUANKA_NUM_PER_ITEM;
        int totalMainShape = mainShapePair[idx_sub] * 2;
        int totalOtherShape = otherShapeNum[idx_sub];


        int totalItem = totalMainShape + totalOtherShape;
        totalRow = CalcRowCol(totalItem);
        totalCol = totalRow;
        Debug.Log("totalItem=" + totalItem + " row=" + totalRow + " col=" + totalCol);

        List<object> listShapeOther = GetOtherItemList(infoshape, listShape);
        int[] indexShapeOther = RandomIndex(listShapeOther.Count, (totalMainShape / 2 - 1));

        int[] indexRectList = RandomIndex(totalRow * totalCol, totalItem);

        //mainshape 
        int[] indexColor = RandomIndex(listColor.Count, totalMainShape / 2);
        for (int k = 0; k < totalMainShape; k++)
        {
            int indexRect = indexRectList[k];
            int i = indexRect % totalCol;
            int j = indexRect / totalRow;

            int idx_color = indexColor[k / 2];
            ShapeColorItemInfo infocolor = listColor[idx_color] as ShapeColorItemInfo;

            GameObject obj = null;
            //mainshape
            bool isInner = (k % 2 == 0) ? true : false;
            listColorShow.Add(infocolor);
            if (k < 2)
            {
                obj = CreateItem(infoshape, isInner, infocolor.color);
            }
            else
            {
                //other
                int idx_ohter = (k - 2) / 2;
                ShapeColorItemInfo infoshape_other = listShapeOther[indexShapeOther[idx_ohter]] as ShapeColorItemInfo;
                obj = CreateItem(infoshape_other, isInner, infocolor.color);
            }

            Rect rc = GetRectItem(i, j, totalRow, totalCol);

            ShapeColorItemInfo infoitem = AddItem(rc, infoshape, infocolor, obj, isInner, true);
            infoitem.i = i;
            infoitem.j = j;
        }

        //othershape
        indexColor = RandomIndex(listColor.Count, totalOtherShape);
        List<object> listOther = GetOtherItemList(infoshape, listShape);
        int[] indexOther = RandomIndex(listOther.Count, totalOtherShape);
        for (int k = 0; k < totalOtherShape; k++)
        {
            int indexRect = indexRectList[totalMainShape + k];
            int i = indexRect % totalCol;
            int j = indexRect / totalRow;

            int idxtmp = indexOther[k];
            ShapeColorItemInfo infoOther = listOther[idxtmp] as ShapeColorItemInfo;

            int idx_color = indexColor[k];
            if (mode == GAME_MODE_SHAPE)
            {
                // 统一颜色
                idx_color = indexColor[0];
            }
            ShapeColorItemInfo infocolor = listColor[idx_color] as ShapeColorItemInfo;
            Color color = infocolor.color;
            listColorShow.Add(infocolor);
            GameObject obj = CreateItem(infoOther, true, color);
            Rect rc = GetRectItem(i, j, totalRow, totalCol);


            ShapeColorItemInfo infoitem = AddItem(rc, infoOther, infocolor, obj, true, false);
            infoitem.i = i;
            infoitem.j = j;
        }

    }


    void CheckGameWin()
    {
        bool isAllItemLock = true;

        foreach (ShapeColorItemInfo info in listItem)
        {
            if (info.isMain)
            {
                bool isLock = IsItemLock(info);
                if (!isLock)
                {
                    isAllItemLock = false;
                }
            }

        }

        if (isAllItemLock)
        {
            //show game win
            // gameEndParticle.Play();
            Invoke("OnGameWin", 1f);

        }

    }
    public void OnGameWin()
    {
        //show game win
        GameManager.gameLevelFinish = GameManager.gameLevel;
        //gameEndParticle.Play();

        //记录游戏完成
        int level = GameManager.gameLevel;
        int idx = level / GUANKA_NUM_PER_ITEM;
        int idx_sub = level % GUANKA_NUM_PER_ITEM;
        if ((idx_sub + 1) == GUANKA_NUM_PER_ITEM)
        {
            ShapeColorItemInfo infoitem;
            string key;
            if (gameMode == GAME_MODE_SHAPE)
            {
                infoitem = GetItemInfoShapeColor(idx, listShape);
                key = STR_KEY_GAME_STATUS_SHAPE + infoitem.id;
                PlayerPrefs.SetInt(key, GAME_STATUS_FINISH);
            }
            if (gameMode == GAME_MODE_COLOR)
            {
                infoitem = GetItemInfoShapeColor(idx, listColor);
                key = STR_KEY_GAME_STATUS_COLOR + infoitem.id;
                PlayerPrefs.SetInt(key, GAME_STATUS_FINISH);
            }

        }


        //Invoke("ShowGameWin", 1f);
        ShowGameWin();
    }

    void ShowUserGuide()
    {

        if (Common.isMonoPlayer)
        {
            return;
        }
        string pkey = AppString.STR_KEY_USER_GUIDE + Common.GetAppVersion();
        bool isshowplay = Common.GetBool(pkey);
        if (isshowplay == true)
        {
            return;
        }


        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_USER_GUIDE);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_USER_GUIDE);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_USER_GUIDE);
            string no = yes;
            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_USER_GUIDE, OnUIViewAlertFinished);
        }

    }
    void ShowGameWin()
    {
        Debug.Log("ShowGameWin");
        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_GAME_FINISH);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_GAME_FINISH);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_GAME_FINISH);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_GAME_FINISH);

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GAME_FINISH, OnUIViewAlertFinished);

        ShapeColorItemInfo info = GetItemInfo();
        string str = "";
        TTS.Speek(str);


    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_GAME_FINISH == alert.keyName)
        {
            if (isYes)
            {
                GameManager.GotoNextLevelWithoutPlace();
            }
            else
            {

            }
        }


        if (STR_KEYNAME_VIEWALERT_GAME_FINISH == alert.keyName)
        {

            string pkey = AppString.STR_KEY_USER_GUIDE + Common.GetAppVersion();
            Common.SetBool(pkey, true);
        }


    }
    public override void OnClickBtnBack()
    {

        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        Debug.Log("OnClickBtnBack");
        base.OnClickBtnBack();
    }
}
