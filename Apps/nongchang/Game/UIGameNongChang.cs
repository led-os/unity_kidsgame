using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
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


    GameObject objItemClick;
    int idxItemClick;
    int imageBgWidth;
    int imageBgHeight;
    float scaleBg;
    int totalSearchItem;
    int foundSearchItem;
    Vector2 scrollViewOffsetMinNormal;
    Vector2 scrollViewOffsetMaxNormal;


    GameBase gameBase;


    int rowPointTotal;
    int colPointTotal;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        foundSearchItem = 0;
        int row = 8;
        int col = 10;
        if (Device.isLandscape)
        {
            rowPointTotal = Mathf.Min(row, col);
            colPointTotal = Mathf.Max(row, col);
        }
        else
        {
            rowPointTotal = Mathf.Max(row, col);
            colPointTotal = Mathf.Min(row, col);
        }
        //ParseGuanka();
        if (objScrollView != null)
        {
            RectTransform rctran = objScrollView.GetComponent<RectTransform>();
            scrollViewOffsetMinNormal = new Vector2(rctran.offsetMin.x, rctran.offsetMin.y);
            scrollViewOffsetMaxNormal = new Vector2(rctran.offsetMax.x, rctran.offsetMax.y);
        }
        UpdateBtnMusic();
        gameBase = new GameBase();

    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
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
        float x, y, w, h;
        NongChangItemInfo info = GameLevelParse.main.GetItemInfo();
        if (info == null)
        {
            return;
        }
        foreach (NongChangItemInfo searchiteminfo in info.listSearchItem)
        {
            GameObject objSearchItem = searchiteminfo.obj;
            RectTransform rctranParent = objSearchItem.transform.parent.transform as RectTransform;
            float scale = objSearchItem.transform.localScale.x;
            RectTransform rctran = objSearchItem.transform as RectTransform;
            w = rctran.rect.width * scale;
            h = rctran.rect.height * scale;
            x = rctran.anchoredPosition.x - w / 2;
            y = rctran.anchoredPosition.y - h / 2;
            //GameManager.main.heightAdCanvas = 160f;

            float y_bottom = (-rctranParent.rect.height / 2 + GameManager.main.heightAdCanvas);
            if (y < y_bottom)
            {
                y = y_bottom;
                rctran.anchoredPosition = new Vector2(rctran.anchoredPosition.x, y + h / 2);
            }
        }




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
        NongChangItemInfo info = GameLevelParse.main.GetItemInfo();
        // Debug.Log("sound:" + info.sound);
        // string fileaudio = "AppCommon/Audio/" + FileUtil.GetFileName(info.sound);
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
        NongChangItemInfo info = GameLevelParse.main.GetItemInfo();
        float x, y, w, h;
        RectTransform rctran_content = objScrollViewContent.transform as RectTransform;
        PointManager.main.width = rctran_content.rect.width;
        PointManager.main.height = rctran_content.rect.height;
        PointManager.main.offsetX = 0;
        PointManager.main.offsetY = 160f;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        //bg
        {
            objImageBg = new GameObject("ImageBg");
            objImageBg.transform.parent = objScrollViewContent.transform;
            RawImage imageBg = objImageBg.AddComponent<RawImage>();
            Texture2D tex = TextureCache.main.Load(GameLevelParse.main.strPicBg);// = LoadTexture.LoadFromAsset(strPicBg);
            if (tex)
            {
                TextureUtil.UpdateRawImageTexture(imageBg, tex, true);
                RectTransform rctran = objImageBg.transform as RectTransform;
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
        foreach (NongChangItemInfo mapiteminfo in GameLevelParse.main.listMapItem)
        {
            string name = FileUtil.GetFileName(mapiteminfo.pic);
            Debug.Log("mapitem:" + name);
            GameObject objMapItem = new GameObject(name);
            objMapItem.transform.parent = objScrollViewContent.transform;
            RawImage imageMapItem = objMapItem.AddComponent<RawImage>();
            Texture2D tex = TextureCache.main.Load(mapiteminfo.pic);//LoadTexture.LoadFromAsset(mapiteminfo.pic);
            if (tex)
            {
                TextureUtil.UpdateRawImageTexture(imageMapItem, tex, true);
                RectTransform rctran = objMapItem.transform as RectTransform;
                float ratio_x = (float)(mapiteminfo.x * 1.0 / imageBgWidth);
                float ratio_y = (float)(mapiteminfo.y * 1.0 / imageBgHeight);
                x = ratio_x * rctran_content.rect.width - rctran_content.rect.width / 2;
                y = ratio_y * rctran_content.rect.height - rctran_content.rect.height / 2;
                rctran.anchoredPosition = new Vector2(x, y);
                objMapItem.transform.localScale = new Vector3(scaleBg, scaleBg, 1.0f);
                mapiteminfo.obj = objMapItem;
                // objMapItem.AddComponent<Canvas>();
                // Canvas canvas = objMapItem.GetComponent<Canvas>();
                // canvas.overrideSorting = true;
                // canvas.sortingOrder = 0;
            }
        }

        //search item
        int search_idx = 0;
        int m = 5;
        int[] arrIndexRow = PointManager.main.GetNumList(rowPointTotal, m);
        int[] arrIndexCol = PointManager.main.GetNumList(colPointTotal, m);

        foreach (NongChangItemInfo searchiteminfo in info.listSearchItem)
        {
            GameObject objSearchItem = new GameObject(FileUtil.GetFileName(searchiteminfo.pic));
            objSearchItem.transform.parent = objScrollViewContent.transform;
            RawImage imageItem = objSearchItem.AddComponent<RawImage>();
            searchiteminfo.obj = objSearchItem;

            Button btn = objSearchItem.AddComponent<Button>();
            int idxtmp = search_idx;
            btn.onClick.AddListener(delegate ()
                                {
                                    OnSearchItemClick(btn.gameObject, idxtmp);
                                }
                            );



            Texture2D tex = TextureCache.main.Load(searchiteminfo.pic);//LoadTexture.LoadFromAsset(searchiteminfo.pic);


            if (tex)
            {
                TextureUtil.UpdateRawImageTexture(imageItem, tex, true);
                RectTransform rctran = objSearchItem.transform as RectTransform;


                //从json中读取位置
                float ratio_x = (float)(searchiteminfo.x * 1.0 / imageBgWidth);
                float ratio_y = (float)(searchiteminfo.y * 1.0 / imageBgHeight);
                x = ratio_x * rctran_content.rect.width - rctran_content.rect.width / 2;
                y = ratio_y * rctran_content.rect.height - rctran_content.rect.height / 2;

                //随机生成位置
                int row, col;
                //row = Random.Range(0, rowPointTotal);
                //col = Random.Range(0, colPointTotal);
                row = arrIndexRow[search_idx];
                col = arrIndexCol[search_idx];
                Rect rc = PointManager.main.GetRectItem(row, col, rowPointTotal, colPointTotal);
                Vector2 pt = PointManager.main.RandomPointOfRect(rc, 0, 0);

                //判断和map item是否有显示重叠
                foreach (NongChangItemInfo mapiteminfo in GameLevelParse.main.listMapItem)
                {
                    RectTransform rctranMap = mapiteminfo.obj.GetComponent<RectTransform>();
                    w = rctranMap.rect.width * mapiteminfo.obj.transform.localScale.x;
                    h = rctranMap.rect.height * mapiteminfo.obj.transform.localScale.y;
                    x = rctranMap.anchoredPosition.x - w / 2;
                    y = rctranMap.anchoredPosition.y - h / 2;
                    Rect rcMap = new Rect(x, y, w, h);
                    if (rcMap.Contains(pt))
                    {
                        //有显示重叠
                        //  pt.x = (pt.x > x) ? (x + w / 2) : (x - w / 2);
                        //   pt.y = (pt.y > y) ? (y + h / 2) : (y - h / 2);
                        //置顶
                        objSearchItem.transform.SetAsLastSibling();
                        break;
                    }

                }

                rctran.anchoredPosition = new Vector2(pt.x, pt.y);
                float scale_item = searchiteminfo.scale;
                float sc = scaleBg * scale_item;
                if (LevelManager.main.placeLevel >= 6)
                {
                    sc = sc / 2;
                }
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


    void MoveAnimationEnd()
    {
        Debug.Log("MoveAnimationEnd");
        if (objItemClick != null)
        {
            objItemClick.SetActive(false);
        }

        TopBarSearchItem topbaritem = GetTopBarSearchItem(idxItemClick);
        topbaritem.DidFind();

        NongChangItemInfo info = GameLevelParse.main.GetItemInfo();
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
        LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;
        //ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
        //next guanka
        LevelManager.main.GotoNextLevel();

    }

    void ShowGameWin()
    {
        Debug.Log("ShowGameWin");
        OnGameWinBase();
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
        //  iTween.MoveTo(sender, iTween.Hash("position", topbaritem.transform.position, "time", 1f, "oncompletetarget", this.gameObject, "oncomplete", "MoveAnimationEnd"));


        sender.transform.DOMove(topbaritem.transform.position, 1f).OnComplete(() =>
                 {
                     this.MoveAnimationEnd();
                 });

        Vector3[] paths;
        paths = new Vector3[3];
        paths[0] = sender.transform.position;
        paths[2] = imageStar.transform.position;
        //paths[1] = new Vector3((paths[0].x+paths[2].x)/2,(paths[0].y+paths[2].y)/2-200, paths[0].z);

        //iTween.MoveTo(imageStarAnimate.gameObject, iTween.Hash("path", paths, "movetopath", true, "orienttopath", true, "time", 2f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop, "lookahead", 0));



        NongChangItemInfo info = GameLevelParse.main.GetItemInfo();
        NongChangItemInfo searchinfo = (NongChangItemInfo)info.listSearchItem[idx];
        searchinfo.isHasFound = true;
        // string fileaudio = "AppCommon/Audio/" + FileUtil.GetFileName(searchinfo.sound);
        // AudioUtil.PlayFileResource(fileaudio);

        gameBase.SetGameItemStatus(info, GameBase.GAME_STATUS_FINISH);

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