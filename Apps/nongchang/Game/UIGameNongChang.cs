using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
农场大发现：
https://itunes.apple.com/cn/app/%E5%84%BF%E7%AB%A5%E5%AD%A6%E4%B9%A0%E4%B9%90%E5%9B%AD-%E5%86%9C%E5%9C%BA%E5%A4%A7%E5%8F%91%E7%8E%B0-%E5%9F%B9%E5%85%BB%E5%AE%9D%E5%AE%9D%E7%9A%84%E8%A7%82%E5%AF%9F%E5%8A%9B%E5%92%8C%E8%A7%86%E8%A7%89%E6%90%9C%E7%B4%A2%E8%83%BD%E5%8A%9B/id842393899?mt=8
 */
public class NongChangItemInfo : ItemInfo
{
    public List<object> listPosition;
    public List<object> listSearchItem;
    public bool flipx;
    public float scale;
    public int x; //基于图片左下角的坐标，自己锚点为(0.5,0.5)
    public int y; //基于图片左下角的坐标，自己锚点为(0.5,0.5)
    public string picBg;
    public bool isHasFound;
    public int count;

}

public class UIGameNongChang : UIGameBase
{

    public const int GUANKA_ITEM_NUM = 5;
    public TopBarSearchItem topBarSearchItem0;
    public TopBarSearchItem topBarSearchItem1;
    public TopBarSearchItem topBarSearchItem2;
    public TopBarSearchItem topBarSearchItem3;
    public TopBarSearchItem topBarSearchItem4;
    public Image imageStar;
    public Image imageStarAnimate;
    public Text textStar;
    GameObject objImageBg;
    public GameObject layoutTopBar;
    public GameObject objScrollView;
    public GameObject objScrollViewContent;
    // public Scro
    static public List<object> listMapItem;
    static public List<object> listPoint;
    List<object> listGuankaItem;//image id

    static public List<object> listAutoGuanka;//item index list
    GameObject objItemClick;
    int idxItemClick;
    int imageBgWidth;
    int imageBgHeight;
    float scaleBg;
    int totalSearchItem;
    int foundSearchItem;
    Vector2 scrollViewOffsetMinNormal;
    Vector2 scrollViewOffsetMaxNormal;

    static public string strPicBg;
    AutoMakeGuanka autoMakeGuanka;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        foundSearchItem = 0;
        // ParseGuanka();
        RectTransform rctran = objScrollView.GetComponent<RectTransform>();
        scrollViewOffsetMinNormal = new Vector2(rctran.offsetMin.x, rctran.offsetMin.y);
        scrollViewOffsetMaxNormal = new Vector2(rctran.offsetMax.x, rctran.offsetMax.y);
        UpdateBtnMusic();

    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(GameManager.gameLevel);
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (!AppVersion.appForPad)
        {
            //Common.SetOrientation((int)UIOrientation.UIORIENTATION_PortraitUp);
        }

    }
    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        DestroyGame();
        LoadGame();
        PlaySoundGameIntro();
        LayOut();

        OnUIDidFinish();
    }
    public override void LayOut()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float offsetLeft = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetLeft);
        float offsetRight = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetRight);
        float offsetTop = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetTop);
        float offsetBottom = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetBottom);



    }

    bool ListContainItem(List<NongChangItemInfo> list, NongChangItemInfo item)
    {
        bool ret = false;
        foreach (NongChangItemInfo info in list)
        {
            if (info.id == item.id)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    //计算相同id数
    int GetTheSameItemById(List<object> list, NongChangItemInfo item)
    {
        int count = 0;
        foreach (NongChangItemInfo info in list)
        {
            if (info.id == item.id)
            {
                count++;
            }
        }
        return count;
    }

    void PlaySoundGameIntro()
    {
        // audioClipBtn = (AudioClip)Resources.Load(AppResAudio.RES_AUDIO_BTN_CLICK);
        NongChangItemInfo info = GetItemInfo();
        // Debug.Log("sound:" + info.sound);
        // string fileaudio = "App/Audio/" + FileUtil.GetFileName(info.sound);
        // AudioUtil.PlayFileResource(fileaudio);
        //统计listSearchItem里的元素个数
        List<NongChangItemInfo> listTmp = new List<NongChangItemInfo>();
        foreach (NongChangItemInfo infosearch in info.listSearchItem)
        {
            if (ListContainItem(listTmp, infosearch))
            {
                continue;
            }

            infosearch.count = GetTheSameItemById(info.listSearchItem, infosearch);
            listTmp.Add(infosearch);
        }

        string strtext = "";
        int idx = 0;
        foreach (NongChangItemInfo item in listTmp)
        {
            string num = item.count.ToString();
            if (Language.main.IsChinese())
            {
                num += "个";
            }
            else
            {
                num += " ";
            }
            strtext += num + languageGame.GetString(item.id);
            if (idx > 0 && idx < (listTmp.Count - 1))
            {
                strtext += ",";
            }
            idx++;
        }

        strtext += Language.main.GetString("STR_FIND_ITEM");

        Debug.Log(strtext);
        TTS.main.Speak(strtext);
    }

    void DestroyGame()
    {
        foreach (Transform child in objScrollViewContent.transform)
        {
            Debug.Log("Destroy Sub Child");
            Destroy(child.gameObject);
        }
    }



    void LoadGame()
    {
        NongChangItemInfo info = GetItemInfo();
        float x, y, w, h;
        RectTransform rctran_content = objScrollViewContent.transform as RectTransform;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        //bg
        {
            objImageBg = new GameObject("ImageBg");
            objImageBg.transform.parent = objScrollViewContent.transform;
            objImageBg.AddComponent<Image>();
            Debug.Log("strPicBg2=" + strPicBg);
            Texture2D tex = LoadTexture.LoadFromAsset(strPicBg);
            if (tex)
            {
                objImageBg.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                RectTransform rctran = objImageBg.transform as RectTransform;
                rctran.sizeDelta = new Vector2(tex.width, tex.height);
                imageBgWidth = tex.width;
                imageBgHeight = tex.height;

                float w_image = rctran.rect.width;
                float h_image = rctran.rect.height;
                float scalex = sizeCanvas.x / w_image;
                int offsety = 0;
                float scaley = (sizeCanvas.y - offsety) / h_image;
                float scale = Mathf.Max(scalex, scaley);
                scaleBg = scale;
                objImageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

                w = scale * rctran.sizeDelta.x;
                h = scale * rctran.sizeDelta.y;
                rctran_content.sizeDelta = new Vector2(w, h);

                rctran.anchoredPosition = Vector2.zero;
            }
        }
        //map item
        foreach (NongChangItemInfo mapiteminfo in listMapItem)
        {
            string name = FileUtil.GetFileName(mapiteminfo.pic);
            Debug.Log("mapitem:" + name);
            GameObject objMapItem = new GameObject(name);
            objMapItem.transform.parent = objScrollViewContent.transform;
            objMapItem.AddComponent<Image>();
            Texture2D tex = LoadTexture.LoadFromAsset(mapiteminfo.pic);
            if (tex)
            {
                objMapItem.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                RectTransform rctran = objMapItem.transform as RectTransform;
                rctran.sizeDelta = new Vector2(tex.width, tex.height);
                float ratio_x = (float)(mapiteminfo.x * 1.0 / imageBgWidth);
                float ratio_y = (float)(mapiteminfo.y * 1.0 / imageBgHeight);
                x = ratio_x * rctran_content.rect.width - rctran_content.rect.width / 2;
                y = ratio_y * rctran_content.rect.height - rctran_content.rect.height / 2;
                rctran.anchoredPosition = new Vector2(x, y);
                objMapItem.transform.localScale = new Vector3(scaleBg, scaleBg, 1.0f);

                // objMapItem.AddComponent<Canvas>();
                // Canvas canvas = objMapItem.GetComponent<Canvas>();
                // canvas.overrideSorting = true;
                // canvas.sortingOrder = 0;
            }
        }

        //search item
        int search_idx = 0;

        foreach (NongChangItemInfo searchiteminfo in info.listSearchItem)
        {
            GameObject objSearchItem = new GameObject(FileUtil.GetFileName(searchiteminfo.pic));
            objSearchItem.transform.parent = objScrollViewContent.transform;
            objSearchItem.AddComponent<Image>();


            objSearchItem.AddComponent<Button>();
            Button btn = objSearchItem.GetComponent<Button>();
            int idxtmp = search_idx;
            btn.onClick.AddListener(delegate ()
                                {
                                    OnSearchItemClick(btn.gameObject, idxtmp);
                                }
                            );



            Texture2D tex = LoadTexture.LoadFromAsset(searchiteminfo.pic);
            if (tex)
            {
                objSearchItem.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                RectTransform rctran = objSearchItem.transform as RectTransform;
                rctran.sizeDelta = new Vector2(tex.width, tex.height);
                float ratio_x = (float)(searchiteminfo.x * 1.0 / imageBgWidth);
                float ratio_y = (float)(searchiteminfo.y * 1.0 / imageBgHeight);
                x = ratio_x * rctran_content.rect.width - rctran_content.rect.width / 2;
                y = ratio_y * rctran_content.rect.height - rctran_content.rect.height / 2;
                rctran.anchoredPosition = new Vector2(x, y);
                float scale_item = searchiteminfo.scale;
                float sc = scaleBg * scale_item;
                objSearchItem.transform.localScale = new Vector3(sc, sc, 1.0f);

            }

            search_idx++;

        }


        totalSearchItem = info.listSearchItem.Count;

        //topbar
        int idx = 0;
        NongChangItemInfo infosearch = (NongChangItemInfo)info.listSearchItem[idx++];
        topBarSearchItem0.UpdateItem(infosearch);
        infosearch = (NongChangItemInfo)info.listSearchItem[idx++];
        topBarSearchItem1.UpdateItem(infosearch);
        infosearch = (NongChangItemInfo)info.listSearchItem[idx++];
        topBarSearchItem2.UpdateItem(infosearch);
        infosearch = (NongChangItemInfo)info.listSearchItem[idx++];
        topBarSearchItem3.UpdateItem(infosearch);
        infosearch = (NongChangItemInfo)info.listSearchItem[idx++];
        topBarSearchItem4.UpdateItem(infosearch);

        UpdateStar();
    }

    void UpdateStar()
    {
        textStar.text = foundSearchItem + "/" + totalSearchItem;
    }
    NongChangItemInfo GetItemInfo()
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
        NongChangItemInfo info = listGuanka[idx] as NongChangItemInfo;
        return info;
    }

    public override int GetGuankaTotal()
    {
        return ParseGuanka();
    }

    public override void CleanGuankaList()
    {
        Debug.Log("CleanGuankaList nongchang");
        if (listGuanka != null)
        {
            Debug.Log("CleanGuankaList nongchang Clear");
            listGuanka.Clear();
            Debug.Log("CleanGuankaList nongchang Clear end=" + UIGameBase.listGuanka.Count);
        }
        if (listMapItem != null)
        {
            listMapItem.Clear();
        }
        if (listPoint != null)
        {
            listPoint.Clear();
        }
        if (listGuankaItem != null)
        {
            listGuankaItem.Clear();
        }
    }

    public override int ParseGuanka()
    {
        int count = 0;
        CleanGuankaList();
        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            Debug.Log("ParseGuanka nongchang is not null count=" + listGuanka.Count);
            return listGuanka.Count;
        }

        if (autoMakeGuanka == null)
        {
            // autoMakeGuanka = this.gameObject.AddComponent<AutoMakeGuanka>();
            autoMakeGuanka = new AutoMakeGuanka();
            autoMakeGuanka.Init();
        }
        if (listAutoGuanka == null)
        {
            listAutoGuanka = autoMakeGuanka.ParseAutoGuankaJson();
        }

        listGuankaItem = new List<object>();

        listGuanka = new List<object>();
        int idx = GameManager.placeLevel;
        string fileName = Common.GAME_RES_DIR + "/guanka/guanka_list_" + idx + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName); //((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string type = (string)root["type"];
        string mapid = (string)root["map_id"];
        string picRoot = Common.GAME_RES_DIR + "/search_items/" + type + "/";

        //search_items
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            NongChangItemInfo info = new NongChangItemInfo();
            info.id = (string)item["id"];
            info.pic = picRoot + info.id + ".png";
            listGuankaItem.Add(info);
        }


        //让总数是GUANKA_ITEM_NUM的整数倍
        int tmp = (listGuankaItem.Count % GUANKA_ITEM_NUM);
        if (tmp > 0)
        {
            for (int i = 0; i < (GUANKA_ITEM_NUM - tmp); i++)
            {
                NongChangItemInfo infoId = listGuankaItem[i] as NongChangItemInfo;
                NongChangItemInfo info = new NongChangItemInfo();
                info.id = infoId.id;
                info.pic = infoId.pic;
                listGuankaItem.Add(info);
            }
        }

        ParseMapItem(mapid);
        int group = listGuankaItem.Count / GUANKA_ITEM_NUM;

        count = listAutoGuanka.Count;
        // count = listGuankaItem.Count;

        int rdm = Random.Range(0, listPoint.Count);
        NongChangItemInfo infoPoint = listPoint[rdm] as NongChangItemInfo;
        for (int g = 0; g < group; g++)
        {

            for (int i = 0; i < count; i++)
            {
                NongChangItemInfo infoGuanka = new NongChangItemInfo();

                NongChangItemInfo infoAutoGuanka = listAutoGuanka[i] as NongChangItemInfo;
                string strcontent = infoAutoGuanka.id;
                string[] strArray = strcontent.Split(',');
                infoGuanka.listSearchItem = new List<object>();

                int pos_index = 0;
                foreach (string stritem in strArray)
                {
                    idx = Common.String2Int(stritem) + g * GUANKA_ITEM_NUM;
                    NongChangItemInfo infoId = listGuankaItem[idx] as NongChangItemInfo;
                    NongChangItemInfo infoSearchItem = new NongChangItemInfo();
                    infoSearchItem.id = infoId.id;
                    infoSearchItem.pic = infoId.pic;

                    infoGuanka.listSearchItem.Add(infoSearchItem);
                    NongChangItemInfo infoposition = infoPoint.listPosition[pos_index] as NongChangItemInfo;
                    infoSearchItem.x = infoposition.x;
                    infoSearchItem.y = infoposition.y;
                    infoSearchItem.scale = infoposition.scale;
                    infoSearchItem.flipx = infoposition.flipx;
                    infoSearchItem.isHasFound = false;
                    pos_index++;
                }

                listGuanka.Add(infoGuanka);
            }

        }
        Debug.Log("ParseGame::count=" + count + " listGuanka.count=" + listGuanka.Count);
        return listGuanka.Count;
    }

    public void ParseMapItem(string mapid)
    {


        if (listMapItem == null)
        {
            listMapItem = new List<object>();
        }


        if (listPoint == null)
        {
            listPoint = new List<object>();
        }
        listPoint.Clear();

        string filePath = Common.GAME_RES_DIR + "/guanka/map/" + mapid + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filePath); //((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);

        string strbgname = (string)root["bg"];
        string picRoot = Common.GAME_RES_DIR + "/map/" + mapid + "/";
        strPicBg = picRoot + strbgname;
        Debug.Log("strPicBg1=" + strPicBg);

        //map_item
        JsonData mapitems = root["items"];
        for (int i = 0; i < mapitems.Count; i++)
        {
            JsonData item = mapitems[i];

            NongChangItemInfo findInfo = new NongChangItemInfo();
            findInfo.flipx = (bool)item["flipx"];
            findInfo.pic = picRoot + (string)item["pic"];
            findInfo.x = (int)item["x"];
            findInfo.y = (int)item["y"];
            //double scale = (double)item["scale"];
            //findInfo.scale =(float)scale;

            listMapItem.Add(findInfo);

        }

        JsonData points = root["points"];
        for (int i = 0; i < points.Count; i++)
        {
            JsonData item = points[i];
            NongChangItemInfo info = new NongChangItemInfo();
            info.sound = (string)item["sound"];
            info.listPosition = new List<object>();
            JsonData targets = item["targets"];
            for (int j = 0; j < targets.Count; j++)
            {
                JsonData findItem = targets[j];
                NongChangItemInfo findInfo = new NongChangItemInfo();
                findInfo.flipx = (bool)findItem["flipx"];
                findInfo.pic = picRoot + (string)findItem["pic"];
                findInfo.sound = (string)findItem["sound"];
                findInfo.x = (int)findItem["x"];
                findInfo.y = (int)findItem["y"];
                double scale = 1.0f;
                if (Common.JsonDataContainsKey(findItem, "scale"))
                {
                    scale = (double)findItem["scale"];
                }
                findInfo.scale = (float)scale;
                findInfo.isHasFound = false;
                info.listPosition.Add(findInfo);
            }
            info.icon = "";


            listPoint.Add(info);
        }
    }

    void MoveAnimationEnd()
    {
        Debug.Log("MoveAnimationEnd");
        if (objItemClick != null)
        {
            objItemClick.SetActive(false);
        }

        TopBarSearchItem topbaritem = GetTopBarSearchItem(idxItemClick);
        topbaritem.DidFind();

        NongChangItemInfo info = GetItemInfo();
        bool isAllItemFound = true;

        foreach (NongChangItemInfo infotmp in info.listSearchItem)
        {
            if (!infotmp.isHasFound)
            {
                isAllItemFound = false;
            }
        }


        if (isAllItemFound)
        {
            ShowGameWin();
        }
    }

    void OnGameWin()
    {
        GameManager.gameLevelFinish = GameManager.gameLevel;
        ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
        //next guanka
        GameManager.main.GotoNextLevel();
    }

    void ShowGameWin()
    {
        Debug.Log("ShowGameWin");
        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_GAME_FINISH);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_GAME_FINISH);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_GAME_FINISH);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_GAME_FINISH);
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, "STR_KEYNAME_VIEWALERT", OnUIViewAlertFinished);
    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (isYes)
        {
            OnGameWin();
        }
        else
        {

        }



    }
    TopBarSearchItem GetTopBarSearchItem(int idx)
    {
        TopBarSearchItem item = topBarSearchItem0;
        if (idx == 0)
        {
            item = topBarSearchItem0;
        }
        if (idx == 1)
        {
            item = topBarSearchItem1;
        }
        if (idx == 2)
        {
            item = topBarSearchItem2;
        }
        if (idx == 3)
        {
            item = topBarSearchItem3;
        }
        if (idx == 4)
        {
            item = topBarSearchItem4;
        }
        return item;
    }

    public void OnSearchItemClick(GameObject sender, int idx)
    {


        TopBarSearchItem topbaritem = GetTopBarSearchItem(idx);
        objItemClick = sender;
        idxItemClick = idx;

        objItemClick.AddComponent<Canvas>();
        Canvas canvas = objItemClick.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1;

        objItemClick.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        Debug.Log("OnSearchItemClick:idx=" + idx + " pos=" + sender.transform.position);
        Debug.Log("Input:" + Common.GetInputPosition());
        //sender.SetActive(false);
        Vector3 pos = new Vector3(0, mainCam.orthographicSize, sender.transform.position.z);

        // imageStarAnimate.gameObject.SetActive(true);
        // imageStarAnimate.transform.position = sender.transform.position;


        //imageStarAnimate.gameObject imageStar.transform.position
        iTween.MoveTo(sender, iTween.Hash("position", topbaritem.transform.position, "time", 1f, "oncompletetarget", this.gameObject, "oncomplete", "MoveAnimationEnd"));
        Vector3[] paths;
        paths = new Vector3[3];
        paths[0] = sender.transform.position;
        paths[2] = imageStar.transform.position;
        //paths[1] = new Vector3((paths[0].x+paths[2].x)/2,(paths[0].y+paths[2].y)/2-200, paths[0].z);

        //iTween.MoveTo(imageStarAnimate.gameObject, iTween.Hash("path", paths, "movetopath", true, "orienttopath", true, "time", 2f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop, "lookahead", 0));



        NongChangItemInfo info = GetItemInfo();
        NongChangItemInfo searchinfo = (NongChangItemInfo)info.listSearchItem[idx];
        searchinfo.isHasFound = true;
        // string fileaudio = "App/Audio/" + FileUtil.GetFileName(searchinfo.sound);
        // AudioUtil.PlayFileResource(fileaudio);



        foundSearchItem = 0;

        foreach (NongChangItemInfo infotmp in info.listSearchItem)
        {
            if (infotmp.isHasFound)
            {
                foundSearchItem++;
            }
        }

        UpdateStar();


    }

    public override void OnClickBtnBack()
    {
        base.OnClickBtnBack();
    }
}