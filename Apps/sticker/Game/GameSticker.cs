using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moonma.AdKit.AdBanner;
using Moonma.AdKit.AdInsert;
using UnityEngine.EventSystems;
using LitJson;
using System;

public class StickerGameItem : ItemInfo
{
    public bool isSticked;//贴纸完成
    public bool isDisplayInLeft;
}
// using UnityEngine.SceneManagement;
//sprite 描边：http://m.blog.csdn.net/article/details?id=57082556
public class GameSticker : GameBase
{
    static public int MAX_GUANKA_NUM = 100;

    static public bool isPauseGame = false;
    public const int GAME_END_MODE_PAUSE = 0;
    public const int GAME_END_MODE_FAIL = 1;
    public const int GAME_END_MODE_FINISH = 2;

    public const float GAME_ITEM_SCREEN_WIDTH = 256;//
    public const float GAME_ITEM_SCREEN_STEP = 40;
    public const int GAME_MODE_NORMAL = 0;
    public const int GAME_MODE_RANDOM = 1;
    public GameObject objSpriteLeft;
    private Vector2 ptDown;
    //private bool startDraw = false;

    private float gameTimeStart;



    private bool isShowBirdCircle = false;
    private float birdCircleWidth = 0.2f;
    private float birdCirclInnerRadiusMax = 2.0f;

    private LayOut layoutLeft;
    private LayOut layoutRight;
    private List<StickerGameItem> listItemLeft;
    private List<StickerGameItem> listItemRight;

    private List<GameObject> listDisplayGameItem;
    private int DisplayItemNumLeft = 3;
    private int DisplayItemNumRight = 9;
    //touch
    private int indexTouchSel = -1;
    private Vector2 posWorldTouchDown = Vector2.zero;
    private Vector2 itemPosWorldTouchDown = Vector2.zero;

    private int gameGroupTotal;
    public int gameGroupIndx;


    List<object> listGuanka;
    int gameMode;


    private float image_canvas_w;
    private float topbar_canvas_h;
    UITouchEventWithMove uiTouchEvent;
    BoxCollider boxCollider;
    public StickerGameItem infoGuanka;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listGuanka = GameGuankaParse.main.listGuanka;

        gameGroupIndx = 0;

        uiTouchEvent = this.gameObject.AddComponent<UITouchEventWithMove>();
        uiTouchEvent.callBackTouch = OnUITouchEvent;
        Vector2 worldsize = Common.GetWorldSize(mainCam);
        boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.size = worldsize;
    }
    // Use this for initialization
    void Start()
    {

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
        ClearTexture();
    }


    public void LoadGame()
    {
        gameGroupIndx = 0;


        // if (Screen.height > Screen.width)
        // {
        //     image_canvas_w = sizeCanvas.y * image_canvas_w / sizeRef.x;
        //     //image_canvas_w = image_canvas_w * rectRef.y / rectRef.x;

        //     topbar_canvas_h = sizeCanvas.x * topbar_canvas_h / sizeRef.x;
        //     //topbar_canvas_h = topbar_canvas_h * rectRef.y / rectRef.x;

        // }

        gameTimeStart = Time.time;

        Debug.LogFormat("game Start:{0}{1}", GameManager.gameLevel, gameTimeStart);


        objSpriteLeft = new GameObject("bg_left");
        SpriteRenderer rd = objSpriteLeft.AddComponent<SpriteRenderer>();
        Texture2D tex = TextureCache.main.Load("AppCommon/UI/Home/XuxianKang");
        Vector4 border = new Vector4(40, 40, 40, 40);
        rd.sprite = LoadTexture.CreateSprieFromTex(tex, border);
        rd.drawMode = SpriteDrawMode.Sliced;
        objSpriteLeft.transform.SetParent(this.gameObject.transform);


        gameGroupTotal = listGuanka.Count / DisplayItemNumRight;
        if ((listGuanka.Count % DisplayItemNumRight) != 0)
        {
            gameGroupTotal++;
        }

        listDisplayGameItem = new List<GameObject>();

        listItemLeft = new List<StickerGameItem>();
        listItemRight = new List<StickerGameItem>();

        layoutLeft = new LayOut();
        layoutLeft.isAutoFitSize = true;
        layoutLeft.itemScaleRatio = 0.8f;
        layoutLeft.row = 3;
        layoutLeft.col = 1;
        layoutRight = new LayOut();
        layoutRight.isAutoFitSize = true;
        layoutRight.itemScaleRatio = layoutLeft.itemScaleRatio;
        layoutRight.row = 3;
        layoutRight.col = 3;
        if (gameMode == GAME_MODE_NORMAL)
        {
            GotoGameGroup(gameGroupIndx);
        }
        if (gameMode == GAME_MODE_RANDOM)
        {
            GotoGameModeRandom();
        }

        OnUIDidFinish();
        // Invoke("OnUIDidFinish",1f);
    }


    public override void LayOut()
    {
        if (boxCollider != null)
        {
            Vector2 worldsize = Common.GetWorldSize(mainCam);
            boxCollider.size = worldsize;
        }

        float x = 0, y = 0, w = 0, h = 0;
        Debug.Log("uigamesticker layout");
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        if ((sizeCanvas.x == 0) || (sizeCanvas.y == 0))
        {
            return;
        }
        {
            // RectTransform rctran = objTopBar.transform as RectTransform;
            topbar_canvas_h = 160;
        }

        float h_topbar_canvas = topbar_canvas_h;// (textTitle.transform as RectTransform).sizeDelta.y;
        float screen_world_w_half = Common.GetCameraWorldSizeWidth(mainCam);
        float adbaner_h_canvas = GameManager.main.heightAdCanvas;
        float adbaner_h_world = GameManager.main.heightAdWorld;
        float h_topbar_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, h_topbar_canvas);

        if (!Device.isLandscape)
        {
            layoutLeft.row = 1;
            layoutLeft.col = 3;
        }
        else
        {
            layoutLeft.row = 3;
            layoutLeft.col = 1;
        }

        Debug.Log("adbaner_h_canvas=" + adbaner_h_canvas);
        Debug.Log("h_topbar_canvas=" + h_topbar_canvas);

        float imageLeft_world_w = 0;
        float offset_left = 0;
        float offset_top = 0;
        float offset_bottom = 0;
        if (Device.isLandscape)
        {
            offset_left = Common.ScreenToWorldWidth(mainCam, Device.heightSystemTopBar);
            offset_top = Common.CanvasToWorldHeight(mainCam, sizeCanvas, h_topbar_canvas);
            offset_bottom = adbaner_h_world + Common.ScreenToWorldHeight(mainCam, Device.heightSystemHomeBar);
            imageLeft_world_w = mainCam.orthographicSize / 2;
            h = mainCam.orthographicSize * 2 - offset_top - offset_bottom;
            y = -mainCam.orthographicSize + offset_bottom;
            w = imageLeft_world_w;
            Debug.Log("Device.isLandscapeLeft=" + Device.isLandscapeLeft);

            //windowsp'tai不分LandscapeLeft和LandscapeRight
            x = -screen_world_w_half;
            if (Device.isLandscapeLeft)
            {
                x = -screen_world_w_half + offset_left;
            }
            if (Device.isLandscapeRight)
            {
                x = -screen_world_w_half;
            }

            layoutLeft.rect = new Rect(x, y, w, h);
            Debug.Log("layoutLeft.rect=" + layoutLeft.rect + " offset_left=" + offset_left + " offset_top=" + offset_top + " offset_bottom=" + offset_bottom);
            x += imageLeft_world_w;
            w = screen_world_w_half - x;
            if (Device.isLandscapeLeft)
            {
                w = screen_world_w_half - x;
            }
            if (Device.isLandscapeRight)
            {
                w = screen_world_w_half - offset_left - x;
            }

            layoutRight.rect = new Rect(x, y, w, h);
        }

        else
        {
            offset_left = 0;
            offset_top = Common.CanvasToWorldHeight(mainCam, sizeCanvas, h_topbar_canvas) + Common.ScreenToWorldHeight(mainCam, Device.heightSystemTopBar);
            offset_bottom = adbaner_h_world + Common.ScreenToWorldHeight(mainCam, Device.heightSystemHomeBar);
            imageLeft_world_w = screen_world_w_half / 2;
            h = imageLeft_world_w;
            y = -mainCam.orthographicSize + offset_bottom;
            x = -screen_world_w_half + offset_left;
            w = screen_world_w_half - x;
            layoutLeft.rect = new Rect(x, y, w, h);
            Debug.Log("layoutLeft.rect=" + layoutLeft.rect + " sizeCanvas=" + sizeCanvas);

            y = y + h;
            h = (mainCam.orthographicSize - offset_top) - y;
            layoutRight.rect = new Rect(x, y, w, h);

        }





        if (gameMode == GAME_MODE_NORMAL)
        {
            layoutLeft.LayOutItem();
            layoutRight.LayOutItem();
        }


        {
            Vector3 pos = objSpriteLeft.transform.position;
            pos.x = layoutLeft.rect.center.x;
            pos.y = layoutLeft.rect.center.y;
            //pos.y = 0;
            pos.z = -1;
            objSpriteLeft.transform.localPosition = pos;
            SpriteRenderer spRender = objSpriteLeft.GetComponent<SpriteRenderer>();
            spRender.size = layoutLeft.rect.size;

        }






    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    onTouchDown();
                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {
                    onTouchMove();
                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {
                    onTouchUp();
                }
                break;

        }
    }

    void onTouchDown()
    {
        indexTouchSel = -1;
        if (layoutLeft == null)
        {
            return;
        }
        for (int i = 0; i < layoutLeft.count; i++)
        {
            GameObject obj = layoutLeft.GetItem(i);
            //Input.touches[0].position
            Vector2 inputPos = Common.GetInputPosition();
            Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);

            Vector3 size = Common.GetObjBoundSize(obj);
            Vector3 center = Common.GetObjBoundCenter(obj);
            //RectTransform rctran = obj.transform as RectTransform;
            //Debug.Log("onTouchDown,i="+i+": size:"+size+" center:"+center+" anchorpos:"+rctran.anchoredPosition);
            if ((posTouchWorld.x > (center.x - size.x / 2)) && (posTouchWorld.x < (center.x + size.x / 2))
             && (posTouchWorld.y > (center.y - size.y / 2)) && (posTouchWorld.y < (center.y + size.y / 2)))
            {
                //in range
                Debug.Log("item onTouchDown i=" + i);
                StickerGameItem infoLeft = GetItemInfoLeftById(obj.name);
                if (!infoLeft.isSticked)
                {
                    indexTouchSel = i;
                    posWorldTouchDown = posTouchWorld;
                    itemPosWorldTouchDown = center;
                    UpdateTitle(infoLeft);
                }


            }
        }
    }
    void onTouchMove()
    {

        if (indexTouchSel >= 0)
        {
            GameObject obj = layoutLeft.GetItem(indexTouchSel);

            Vector2 inputPos = Common.GetInputPosition();
            Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
            float step_x = posTouchWorld.x - posWorldTouchDown.x;
            float step_y = posTouchWorld.y - posWorldTouchDown.y;
            Vector2 itempos = new Vector2((itemPosWorldTouchDown.x + step_x), (itemPosWorldTouchDown.y + step_y));
            //limit
            Vector3 size = Common.GetObjBoundSize(obj);
            Vector3 center = Common.GetObjBoundCenter(obj);
            if (itempos.x < -(Common.GetCameraWorldSizeWidth(mainCam) - size.x / 2))
            {
                itempos.x = -(Common.GetCameraWorldSizeWidth(mainCam) - size.x / 2);
            }
            if (itempos.x > (Common.GetCameraWorldSizeWidth(mainCam) - size.x / 2))
            {
                itempos.x = (Common.GetCameraWorldSizeWidth(mainCam) - size.x / 2);
            }
            if (itempos.y < -(mainCam.orthographicSize - size.y / 2))
            {
                itempos.y = -(mainCam.orthographicSize - size.y / 2);
            }
            if (itempos.y > (mainCam.orthographicSize - size.y / 2))
            {
                itempos.y = (mainCam.orthographicSize - size.y / 2);
            }

            obj.transform.position = new Vector3(itempos.x, itempos.y, obj.transform.position.z);//anchoredPosition
        }


    }
    void onTouchUp()
    {
        if (indexTouchSel >= 0)
        {
            GameObject objLeft = layoutLeft.GetItem(indexTouchSel);
            string nameLeft = objLeft.name;
            Debug.Log("nameLeft:" + nameLeft);
            StickerGameItem infoLeft = GetItemInfoLeftById(nameLeft);

            Vector2 itempos = (objLeft.transform as RectTransform).anchoredPosition;
            float step_screen = GAME_ITEM_SCREEN_STEP * AppCommon.scaleBase;
            float step = Common.ScreenToWorldWidth(mainCam, step_screen);

            bool isAllFinished = true;
            for (int i = 0; i < layoutRight.count; i++)
            {
                StickerGameItem infoRight = listItemRight[i];
                GameObject item = layoutRight.GetItem(i);
                Vector2 itemposright = (item.transform as RectTransform).anchoredPosition;
                Vector3 size = Common.GetObjBoundSize(item);
                Vector3 center = Common.GetObjBoundCenter(item);
                if ((itempos.x > (center.x - size.x / 2)) && (itempos.x < (center.x + size.x / 2))
             && (itempos.y > (center.y - size.y / 2)) && (itempos.y < (center.y + size.y / 2)))
                {
                    Debug.Log("left:" + infoLeft.id);
                    Debug.Log("right:" + infoRight.id);
                    //in range
                    if (infoLeft.id == infoRight.id)
                    {
                        //贴图正确
                        infoLeft.isSticked = infoRight.isSticked = true;
                        //锁定位置
                        (objLeft.transform as RectTransform).anchoredPosition = itemposright;
                        item.SetActive(false);

                        if ((listItemLeft.Count != 0) && (gameMode == GAME_MODE_NORMAL))
                        {
                            StickerGameItem itemFirst = GetItemInfoLeftOfFirstNotDisplayInLeft();
                            if (itemFirst != null)
                            {
                                GameObject objNew = CreateSpriteFormRes(itemFirst, false);
                                layoutLeft.ReplaceItem(indexTouchSel, objNew);
                                layoutLeft.LayOutItem();
                                listDisplayGameItem.Add(objNew);
                                itemFirst.isDisplayInLeft = true;

                            }

                        }


                        Debug.Log("item onTouchUp i= ,stick ok" + i);
                        indexTouchSel = -1;
                        if (gameMode == GAME_MODE_RANDOM)
                        {
                            ShowGameWin();
                            return;
                        }
                    }



                }

                if (!infoRight.isSticked)
                {
                    isAllFinished = false;
                }
            }

            if (isAllFinished)
            {
                // game group finish
                gameGroupIndx++;
                //game win
                //  gameEndParticle.Play();
                ShowGameWin();


            }


        }

    }

    void ShowTitle(bool isShow)
    {
        // textTitle.gameObject.SetActive(isShow);
        // imageBgTopBar.gameObject.SetActive(isShow);
        if (iDelegate != null)
        {
            iDelegate.OnGameUpdateTitle(this, null, isShow);
        }
    }

    void UpdateTitle(StickerGameItem info)
    {
        if (iDelegate != null)
        {
            iDelegate.OnGameUpdateTitle(this, info, true);
        }

    }
    StickerGameItem GetItemInfoLeftOfFirstNotDisplayInLeft()
    {
        for (int i = 0; i < listItemLeft.Count; i++)
        {
            StickerGameItem info = listItemLeft[i];
            if (!info.isDisplayInLeft)
            {
                return info;
            }

        }

        return null;
    }
    StickerGameItem GetItemInfoLeftById(string id)
    {
        for (int i = 0; i < listItemLeft.Count; i++)
        {
            StickerGameItem info = listItemLeft[i];
            if (info.id == id)
            {
                return info;
            }
        }

        return null;
    }


    void ClearTexture()
    {
        int idx = 0;
        // foreach (Texture2D tex in listTexTure)
        // {
        //     //   Debug.Log("ClearTexture:" + idx);
        //     idx++;
        //     //Resources.UnloadAsset(tex);
        //     // GC.Collect();
        // }
        // Resources.UnloadUnusedAssets();
        // listTexTure.Clear();
    }

    GameObject CreateSpriteFormRes(StickerGameItem info, bool isRight)
    {
        //InitAd();
        GameObject obj = new GameObject(info.id);//"GameItem" + 
        obj.transform.parent = AppSceneBase.main.objMainWorld.transform;
        float z = -10f;
        if (!isRight)
        {
            z = z - 1;
        }
        obj.transform.localPosition = new Vector3(0, 0, z);
        obj.AddComponent<RectTransform>();
        RectTransform rcTran = obj.GetComponent<RectTransform>();
        obj.AddComponent<SpriteRenderer>();
        SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();
        if (isRight)
        {
            objSR.material = new Material(Shader.Find("Custom/Sticker"));
        }

        // obj = (GameObject)GameObject.Instantiate(obj);
        //Debug.Log(info.pic);
        Texture2D tex = LoadTexture.LoadFromAsset(info.icon);
        // listTexTure.Add(tex);
        //Debug.Log("tex,w:" + tex.width + " h:" + tex.height);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        sprite.name = "DaXiang";
        objSR.sprite = sprite;
        float pixsel_w = GAME_ITEM_SCREEN_WIDTH * AppCommon.scaleBase;
        float pixsel_h = pixsel_w;
        Vector2 sizeWorld = Common.ScreenToWorldSize(mainCam, new Vector2(pixsel_w, pixsel_h));
        float pixsel_per_unit = 100;
        float tex_world_w = tex.width / pixsel_per_unit;
        float tex_world_h = tex.height / pixsel_per_unit;
        float scale_x = sizeWorld.x / tex_world_w;
        float scale_y = sizeWorld.y / tex_world_h;
        obj.transform.localScale = new Vector3(scale_x, scale_y, 1f);
        rcTran.sizeDelta = new Vector2(tex_world_w, tex_world_h);
        return obj;
    }

    public void GotoGameModeRandom()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float x = 0, y = 0, w = 0, h = 0;
        Vector2 worldSize = Common.GetWorldSize(mainCam);
        Rect rect0 = Rect.zero, rect1 = Rect.zero, rect2 = Rect.zero;
        float world_w = worldSize.x;
        float world_h = worldSize.y;

        float z_left = 0;
        int count = 0;
        float r_display = 0;

        float angle_step = 0;
        Vector2 center = Vector2.zero;
        int level = GameManager.gameLevel;
        float adbaner_h_world = GameManager.main.heightAdWorld;
        float h_topbar_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, topbar_canvas_h);
        float oft_y = Mathf.Max(h_topbar_world, adbaner_h_world);
        r_display = Mathf.Min(world_w, world_h - oft_y * 2) * 0.4f;
        count = 6;
        angle_step = Mathf.PI * 2 / (count);

        objSpriteLeft.SetActive(false);

        layoutLeft.DestroyAllItem();
        layoutRight.DestroyAllItem();
        listItemLeft.Clear();
        listItemRight.Clear();
        layoutLeft.Clear();
        layoutRight.Clear();
        for (int i = 0; i < listDisplayGameItem.Count; i++)
        {
            GameObject objItem = listDisplayGameItem[i];
            Destroy(objItem);
        }
        listDisplayGameItem.Clear();

        StickerGameItem infoGuanka = GameGuankaParse.main.GetGuankaItemInfo(level) as StickerGameItem;
        float angle = 0;

        int[] indexList = Common.RandomIndex(listGuanka.Count, count);
        bool isHave = false;
        for (int i = 0; i < count; i++)
        {
            if (indexList[i] == level)
            {
                isHave = true;
                break;
            }
        }
        if (!isHave)
        {
            int rdm = UnityEngine.Random.Range(0, count);
            indexList[rdm] = level;
        }


        for (int i = 0; i < count; i++)
        {
            angle = angle_step * i;
            StickerGameItem info = listGuanka[indexList[i]] as StickerGameItem;
            GameObject obj = CreateSpriteFormRes(info, true);


            w = r_display * 0.5f;
            h = w;
            x = center.x + (r_display * Mathf.Cos(angle));
            y = center.y + (r_display * Mathf.Sin(angle));

            Rect rc = new Rect(x, y, w, h);
            info.index = i;

            obj.transform.position = new Vector3(x, y, -10f);
            z_left = obj.transform.position.z;
            listItemRight.Add(info);
            layoutRight.AddItem(obj);
        }
        //centerga

        {
            StickerGameItem info = infoGuanka;
            w = r_display * 0.7f;
            h = w;
            x = center.x;
            y = center.y;
            info.index = count;
            GameObject obj = CreateSpriteFormRes(info, false);
            Vector3 pos = obj.transform.position;
            pos.z = z_left - 2;
            pos.x = x;
            pos.y = y;
            obj.transform.position = pos;
            listItemLeft.Add(info);
            layoutLeft.AddItem(obj);
        }

        LayOut();
    }
    public void GotoGameGroup(int idx)
    {
        // ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
        objSpriteLeft.SetActive(true);

        gameGroupIndx = idx;
        if (gameGroupIndx >= gameGroupTotal)
        {
            StickerGameItem itemFirst = GetItemInfoLeftOfFirstNotDisplayInLeft();
            if (itemFirst == null)
            {
                GotoNextGamePlace();
            }
            return;
        }

        ShowTitle(false);

        for (int i = 0; i < listDisplayGameItem.Count; i++)
        {
            GameObject objItem = listDisplayGameItem[i];
            Destroy(objItem);
        }
        listDisplayGameItem.Clear();

        //必须在对象Destroy之后执行清理
        //  ClearTexture();

        //layout
        {

            //left

            int indexstart = gameGroupIndx * DisplayItemNumRight;

            listItemLeft.Clear();
            for (int i = indexstart; i < (indexstart + DisplayItemNumRight); i++)
            {
                if (i < listGuanka.Count)
                {
                    StickerGameItem info = (StickerGameItem)listGuanka[i];
                    info.isDisplayInLeft = false;
                    listItemLeft.Add(info);
                }


            }



            layoutLeft.Clear();

            for (int i = 0; i < DisplayItemNumLeft; i++)
            {
                if (i >= listItemLeft.Count)
                {
                    continue;
                }
                StickerGameItem info = listItemLeft[i];
                info.isDisplayInLeft = true;
                GameObject obj = CreateSpriteFormRes(info, false);
                layoutLeft.AddItem(obj);
                listDisplayGameItem.Add(obj);


            }




            //right 
            indexstart = gameGroupIndx * DisplayItemNumRight;
            listItemRight.Clear();
            layoutRight.Clear();
            for (int i = indexstart; i < (indexstart + DisplayItemNumRight); i++)
            {
                if (i < listGuanka.Count)
                {
                    StickerGameItem info = (StickerGameItem)listGuanka[i];
                    GameObject obj = CreateSpriteFormRes(info, true);
                    layoutRight.AddItem(obj);
                    listItemRight.Add(info);
                    listDisplayGameItem.Add(obj);
                }


            }


        }

        LayOut();
    }

    void GotoNextGamePlace()
    {
        GameManager.main.GotoNextPlace();
    }
    void StartGame()
    {

    }

    void GotoNextPlace()
    {



    }
    void ShowGameWin()
    {
        OnGameWin();
    }

    void PauseGame(bool isPause)
    {
        isPauseGame = isPause;
        if (isPauseGame)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }


    public void OnClickGameEndBtnPlay()
    {
        PauseGame(false);

    }



}
