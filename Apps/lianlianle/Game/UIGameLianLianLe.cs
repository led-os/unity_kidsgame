using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LianLianLeItemInfo : ItemInfo
{
    public string colorid;
    public List<object> listPic0;
    public List<object> listPic1;
    public List<object> listColorFilter;
    public Vector3 posNormalWorld;
    public int count;
    public bool isColor;
}

public class UIGameLianLianLe : UIGameBase
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary> 
    public const int TAG_ITEM_LOCK = -1;
    public const int TAG_ITEM_UNLOCK = 0;
    public const int GUANKA_ITEM_NUM_ONE_GROUP = 5;
    public GameObject objTopBar;
    public Text textTitle;
    public Image imageBar;

    static public List<object> listAutoGuanka;//item index list
    AutoMakeGuanka autoMakeGuanka;

    static public string strWord3500;
    string strPlace;

    List<object> listBg;

    List<object> listColorShow;
    List<object> listItem;
    bool isItemHasSel;
    Vector2 ptDownScreen;
    Vector3 posItemWorld;
    LianLianLeItemInfo itemInfoSel;
    float itemPosZ = -20f;

    AudioClip audioClipItemFail;
    AudioClip audioClipItemFinish;
    AudioClip audioClipGuankaFinish;
    float heightImageBarNormal;
    Shader shaderColor;
    public static string keyGameGuide;
    void Awake()
    {
        shaderColor = Shader.Find("Custom/ShapeColor");
        //bg
        audioClipItemFail = (AudioClip)Resources.Load("App/Audio/DragFail");
        audioClipItemFinish = (AudioClip)Resources.Load("App/Audio/DragOk");
        audioClipGuankaFinish = (AudioClip)Resources.Load("App/Audio/GuankaOk");
        InitLanguage();
        ParseGuanka();
    }


    // Use this for initialization
    void Start()
    {

        RectTransform rctran = imageBar.transform as RectTransform;
        heightImageBarNormal = rctran.sizeDelta.y;


        ShowUserGuide();
        UpdateGuankaLevel(GameManager.gameLevel);

    }

    // Update is called once per frame 
    void Update()
    {

        // mobile touch
        if (Input.touchCount > 0)
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
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonUp(0))
            {
                onTouchUp();
                UpdateTitle();
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

    public override void UpdateGuankaLevel(int level)
    {
        AppSceneBase.main.ClearMainWorld();
        InitUI();
    }

    static public void InitLanguage()
    {
        if (languageGame != null)
        {
            languageGame.SetLanguage(Language.main.GetLanguage());
            return;
        }
        languageGame = new Language();
        languageGame.Init(Common.GAME_RES_DIR + "/language/language.csv");
        languageGame.SetLanguage(Language.main.GetLanguage());
    }

    void onTouchDown()
    {


        isItemHasSel = false;
        Vector2 pos = Common.GetInputPosition();
        ptDownScreen = pos;

        Vector3 posword = mainCam.ScreenToWorldPoint(pos);
        Debug.Log("onTouchDown: " + posword);
        foreach (LianLianLeItemInfo info in listItem)
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
        Vector2 ptStepWorld = Common.ScreenToWorldSize(mainCam, ptStep);
        Vector3 posStepWorld = new Vector3(ptStepWorld.x, ptStepWorld.y, 0);
        Vector3 posword = posItemWorld + posStepWorld;

        //将选中item暂时置顶
        posword.z = itemPosZ - 1;
        itemInfoSel.obj.transform.position = posword;


        foreach (LianLianLeItemInfo info in listItem)
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
            //Debug.Log("left:"+bd+"rc="+rc);
            if ((rc.Contains(posword)) && (itemInfoSel.category == info.category))
            {
                Debug.Log("合并正确");
                //合并正确
                SetItemLock(info, true);
                SetItemLock(itemInfoSel, true);

                RunDisapperAnimation(itemInfoSel, info);

                bool isAllItemLock = IsAllItemLock();
                if (!isAllItemLock)
                {
                    PlayAudioItem(audioClipItemFinish);
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

        //将选中item还原位置
        Vector3 pos = itemInfoSel.posNormalWorld;
        iTween.MoveTo(itemInfoSel.obj, pos, 0.8f);
        PlayAudioItem(audioClipItemFail);
    }

    void InitUI()
    {

        LoadGame();
        //必须在LoadGame之后执行
        LoadBg();
        // LayOutChild();

        UpdateTitle();
        LayOutChild();

        OnUIDidFinish();
    }
    void LayOutChild()
    {

    }
    void UpdateTitle()
    {
        string str = StringOfItem(GetItemInfo());
        textTitle.text = str;
        RectTransform rctranTopbar = objTopBar.transform as RectTransform;

        int fontsize = textTitle.fontSize;
        int str_w = (int)Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);

        int disp_w = (int)rctranTopbar.rect.width;
        int str_line = str_w / disp_w;
        if ((str_w % disp_w) != 0)
        {
            str_line++;
        }

        RectTransform rctran = imageBar.transform as RectTransform;

        Vector2 sizeDelta = rctran.sizeDelta;
        float oft = 0;
        sizeDelta.x = str_w + fontsize + oft * 2;
        if (sizeDelta.x > disp_w)
        {
            sizeDelta.x = disp_w;
        }
        sizeDelta.y = heightImageBarNormal;
        if (str_line > 2)
        {
            sizeDelta.y = heightImageBarNormal * str_line / 2;
        }

        rctran.sizeDelta = sizeDelta;


        RectTransform rctranTitle = textTitle.transform as RectTransform;
        Vector2 sizeDeltaTitle = rctranTitle.sizeDelta;
        sizeDeltaTitle.y = sizeDelta.y;
        rctranTitle.sizeDelta = sizeDeltaTitle;

        TTS.main.Speak(str);
    }
    bool IsInColorFilter(LianLianLeItemInfo colorfilter, LianLianLeItemInfo info)
    {
        bool isfilter = false;
        foreach (LianLianLeItemInfo infocolor in colorfilter.listColorFilter)
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
        foreach (LianLianLeItemInfo infobg in listBg)
        {
            bool isColorFilter = false;
            foreach (LianLianLeItemInfo infocolor in listColorShow)
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
        LianLianLeItemInfo info = listBgNew[rdm] as LianLianLeItemInfo;
        Texture2D tex = LoadTexture.LoadFromAsset(info.pic);
        AppSceneBase.main.UpdateWorldBg(info.pic);

    }


    string StringOfItem(LianLianLeItemInfo info)
    {

        ItemInfo infoplace = GameManager.main.GetPlaceItemInfo(GameManager.placeLevel);
        //STR_PLACE_LIVINGROOM_GUANKA0
        string key = keyGameGuide;//infoplace.title + "_GUANKA" + GameManager.gameLevel;
        Debug.Log("key=" + key);
        if (Common.BlankString(key))
        {
            return "";
        }
        if (languageGame == null)
        {
            return "";
        }

        string str = languageGame.GetString(key);

        return str;
    }
    void PlayAudioItem(AudioClip audioclip)
    {
        //AudioPlayer对象在场景切换后可能从当前scene移除了
        GameObject audioPlayer = GameObject.Find("AudioPlayer");
        if (audioPlayer != null)
        {
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioclip);
        }
    }
    void RunDisapperAnimation(LianLianLeItemInfo infoSel, LianLianLeItemInfo info)
    {
        iTween.MoveTo(infoSel.obj, info.obj.transform.position, 0.8f);
    }

    LianLianLeItemInfo GetItemInfo()
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
        LianLianLeItemInfo info = listGuanka[idx] as LianLianLeItemInfo;
        return info;
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

    }

    void ParseBgList()
    {
        if ((listBg != null) && (listBg.Count != 0))
        {
            return;
        }
        listBg = new List<object>();
        string fileName = Common.GAME_RES_DIR + "/image/bg/bg.json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["list"];
        for (int i = 0; i < items.Count; i++)
        {
            LianLianLeItemInfo info = new LianLianLeItemInfo();
            JsonData item = items[i];
            string strdir = Common.GAME_RES_DIR + "/image/bg";
            info.pic = strdir + "/" + (string)item["pic"];
            info.listColorFilter = new List<object>();
            JsonData colorFilter = item["color_filter"];
            for (int j = 0; j < colorFilter.Count; j++)
            {
                JsonData itemtmp = colorFilter[j];
                LianLianLeItemInfo infotmp = new LianLianLeItemInfo();
                infotmp.id = (string)itemtmp["color_id"];
                info.listColorFilter.Add(infotmp);

            }
            listBg.Add(info);
        }
    }

    void AddItemImage(List<object> list, ItemInfo infoId, bool enable_color)
    {
        ItemInfo infoPlace = GameManager.main.GetPlaceItemInfo(GameManager.placeLevel);
        LianLianLeItemInfo infopic = new LianLianLeItemInfo();
        infopic.id = infoId.id;
        infopic.pic = infoId.pic;
        infopic.category = infoPlace.id;
        infopic.isColor = false;

        //随机打乱
        int rdm = Random.Range(0, list.Count + 1);

        if (enable_color)
        {
            int rdm_tmp = Random.Range(0, 2);
            bool is_up = (rdm_tmp == 0) ? true : false;
            if (rdm > list.Count / 2)
            {
                infopic.isColor = is_up;
            }
            else
            {
                infopic.isColor = !is_up;
            }
        }
        list.Insert(rdm, infopic);
    }

    void ParseAutoGuanka()
    {
        if (autoMakeGuanka == null)
        {
            // autoMakeGuanka = this.gameObject.AddComponent<AutoMakeGuanka>();
            autoMakeGuanka = new AutoMakeGuanka();
            autoMakeGuanka.Init();
            listAutoGuanka = autoMakeGuanka.ParseAutoGuankaJson();
        }

        ItemInfo infoPlace = GameManager.main.GetPlaceItemInfo(GameManager.placeLevel);
        keyGameGuide = "STR_GAME_GUIDE_" + infoPlace.id;
        ParseGuankaItemId(GUANKA_ITEM_NUM_ONE_GROUP);
        int group = listGuankaItemId.Count / GUANKA_ITEM_NUM_ONE_GROUP;
        int idx = 0;
        int count = listAutoGuanka.Count;
        for (int g = 0; g < group; g++)
        {

            for (int i = 0; i < count; i++)
            {
                LianLianLeItemInfo infoAutoGuanka = listAutoGuanka[i] as LianLianLeItemInfo;
                string strcontent = infoAutoGuanka.id;
                string[] strArray = strcontent.Split(',');

                LianLianLeItemInfo info = new LianLianLeItemInfo();
                info.listPic0 = new List<object>();
                info.listPic1 = new List<object>();
                string strDirPic = Common.GAME_RES_DIR + "/image/" + infoPlace.id;
                //pics0 
                int pos_index = 0;
                idx = 0;
                foreach (string stritem in strArray)
                {
                    idx = Common.String2Int(stritem) + g * GUANKA_ITEM_NUM_ONE_GROUP;
                    ItemInfo infoId = listGuankaItemId[idx] as ItemInfo;
                    int rdm = Random.Range(0, 2);
                    bool enable_color = (rdm == 0) ? true : false;
                    AddItemImage(info.listPic0, infoId, enable_color);
                    AddItemImage(info.listPic1, infoId, !enable_color);

                    pos_index++;
                }




                listGuanka.Add(info);
            }

        }

    }
    void ParseGuankaJson()
    {
        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return;
        }

        listGuanka = new List<object>();
        int idx = GameManager.placeLevel;
        string fileName = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";

        if (!FileUtil.FileIsExistAsset(fileName))
        {
            ParseAutoGuanka();
            return;
        }
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
                                                         // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];

        ItemInfo infoPlace = GameManager.main.GetPlaceItemInfo(GameManager.placeLevel);
        keyGameGuide = "STR_PLACE_" + infoPlace.id + "_GUANKA" + GameManager.gameLevel;
        for (int i = 0; i < items.Count; i++)
        {
            LianLianLeItemInfo info = new LianLianLeItemInfo();
            JsonData item = items[i];
            info.listPic0 = new List<object>();
            info.listPic1 = new List<object>();
            string strDirPic = Common.GAME_RES_DIR + "/image/" + infoPlace.id;
            JsonData jsonPic0 = item["pics0"];
            foreach (JsonData item_pic in jsonPic0)
            {
                LianLianLeItemInfo infopic = new LianLianLeItemInfo();
                infopic.pic = strDirPic + "/" + (string)item_pic["name"];
                infopic.category = (string)item_pic["type"];
                //随机打乱
                int rdm = Random.Range(0, info.listPic0.Count + 1);
                info.listPic0.Insert(rdm, infopic);
                if (Common.BlankString(info.pic))
                {
                    info.pic = infopic.pic;
                }

            }


            JsonData jsonPic1 = item["pics1"];
            foreach (JsonData item_pic in jsonPic1)
            {
                LianLianLeItemInfo infopic = new LianLianLeItemInfo();
                infopic.pic = strDirPic + "/" + (string)item_pic["name"];
                infopic.category = (string)item_pic["type"];
                //随机打乱
                int rdm = Random.Range(0, info.listPic1.Count + 1);
                info.listPic1.Insert(rdm, infopic);
            }


            listGuanka.Add(info);
        }

    }




    public override int ParseGuanka()
    {
        ParseBgList();
        ParseGuankaJson();
        int count = listGuanka.Count;
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
            // scale = 1f;
        }
        return scale;
    }
    Rect GetRectItem(int i, int j, int totalRow, int totalCol)
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        Rect rc = Rect.zero;
        float x, y, w, h;
        float w_world = Common.GetCameraWorldSizeWidth(mainCam) * 2;
        float height_topbar_canvas = 160f;
        float height_ad_canvas = 128f;
        float height_topbar_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, height_topbar_canvas);
        float height_ad_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, height_ad_canvas);

        float h_world = mainCam.orthographicSize * 2;
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

    GameObject CreateItem(LianLianLeItemInfo info)
    {
        float x, y, w, h;
        string name = FileUtil.GetFileName(info.pic);


        GameObject obj = new GameObject(name);
        AppSceneBase.main.AddObjToMainWorld(obj);
        RectTransform rcTran = obj.AddComponent<RectTransform>();
        SpriteRenderer objSR = obj.AddComponent<SpriteRenderer>();
        string pic = info.pic;
        Sprite sprite = LoadTexture.CreateSprieFromAsset(pic);
        sprite.name = info.id;
        objSR.sprite = sprite;
        //rcTran.sizeDelta = new Vector2(objSR.size.x, objSR.size.y);

        obj.transform.position = new Vector3(0, 0, itemPosZ);
        SetItemLock(info, false);

        return obj;
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
    void AddItem(Rect rc, LianLianLeItemInfo info, GameObject obj)
    {
        LianLianLeItemInfo infoItem = new LianLianLeItemInfo();
        infoItem.obj = obj;
        infoItem.category = info.category;

        listItem.Add(infoItem);


        float scale = GetItmeScaleInRect(rc, obj);
        obj.transform.localScale = new Vector3(scale, scale, 1f);


        SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();
        Bounds bd = objSR.bounds;
        float offsetx = bd.size.x / 2;
        //offsetx =0;
        float offsety = bd.size.y / 2;
        //offsety=0;
        Vector2 pt = rc.center;//RandomPointOfRect(rc, offsetx, offsety);

        infoItem.posNormalWorld = new Vector3(pt.x, pt.y, obj.transform.position.z);
        obj.transform.position = infoItem.posNormalWorld;

        if (info.isColor)
        {
            Material mat = new Material(shaderColor);
            objSR.material = mat;
            mat.SetColor("_ColorShape", Color.red);
        }

    }

    void LoadGame()
    {

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

        int level = GameManager.gameLevel;
        LianLianLeItemInfo info = GetItemInfo();
        if (info == null)
        {
            Debug.Log("LoadGame null");
            return;
        }

        int totalRow = 2;
        int totalCol = info.listPic0.Count;
        if (!Device.isLandscape)
        {
            totalRow = info.listPic0.Count;
            totalCol = 2;
        }
        List<object> listPic = info.listPic0;
        for (int k = 0; k < listPic.Count; k++)
        {
            GameObject obj = null;
            LianLianLeItemInfo infopic = listPic[k] as LianLianLeItemInfo;
            obj = CreateItem(infopic);
            int i = k;
            int j = 0;
            if (!Device.isLandscape)
            {
                i = 0;
                j = k;
            }
            Rect rc = GetRectItem(i, j, totalRow, totalCol);
            AddItem(rc, infopic, obj);
        }

        listPic = info.listPic1;
        for (int k = 0; k < listPic.Count; k++)
        {
            GameObject obj = null;
            LianLianLeItemInfo infopic = listPic[k] as LianLianLeItemInfo;
            obj = CreateItem(infopic);
            int i = k;
            int j = 1;
            if (!Device.isLandscape)
            {
                i = 1;
                j = k;
            }
            Rect rc = GetRectItem(i, j, totalRow, totalCol);
            AddItem(rc, infopic, obj);
        }

    }



    bool IsAllItemLock()
    {
        bool isAllItemLock = true;

        foreach (LianLianLeItemInfo info in listItem)
        {

            bool isLock = IsItemLock(info);
            if (!isLock)
            {
                isAllItemLock = false;
            }

        }
        return isAllItemLock;

    }

    void CheckGameWin()
    {
        bool isAllItemLock = IsAllItemLock();

        if (isAllItemLock)
        {
            //show game win
            // gameEndParticle.Play();
            Invoke("OnGameWin", 1f);
            PlayAudioItem(audioClipGuankaFinish);
        }

    }
    public void OnGameWin()
    {
        //show game win
        GameManager.gameLevelFinish = GameManager.gameLevel;
        //gameEndParticle.Play();


        //Invoke("ShowGameWin", 1f);
        ShowGameWin();

        //   ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
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

    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)

    {
        if (STR_KEYNAME_VIEWALERT_GAME_FINISH == alert.keyName)
        {
            if (isYes)
            {
                GameManager.main.GotoNextLevel();
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
        base.OnClickBtnBack();
    }
}
