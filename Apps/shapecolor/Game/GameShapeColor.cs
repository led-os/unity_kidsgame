
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HighlightingSystem;
using DG.Tweening;
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
    public GameObject objTrail;
    public bool isMath;

}
public interface IGameShapeColorDelegate
{
    void OnGameShapeColorDidWin(GameShapeColor g);
    void OnGameShapeColorDidError(GameShapeColor g, int error, ShapeColorItemInfo info);
    void OnGameShapeColorDidBomb(GameShapeColor g);
}
public class GameShapeColor : GameBase
{

    public const int GUANKA_NUM_PER_ITEM = 5;
    public const int GAME_MODE_SHAPE = 0;
    public const int GAME_MODE_COLOR = 1;
    public const int GAME_MODE_SHAPE_COLOR = 2;

    public const string STR_KEY_GAME_STATUS_SHAPE = "KEY_GAME_STATUS_SHAPE_";
    public const string STR_KEY_GAME_STATUS_COLOR = "KEY_GAME_STATUS_COLOR_";


    public const int TAG_ITEM_LOCK = -1;
    public const int TAG_ITEM_UNLOCK = 0;

    public const string ID_BOMB = "id_bomb";

    public UIBoard uiBoard;
    Shader shaderColor;
    Shader shaderOutLine;//描边
    public IGameShapeColorDelegate iDelegateShapeColor;


    int totalRow = 0;
    int totalCol = 0;

    int gameMode;
    // public GameEndParticle gameEndParticle;
    static public string strWord3500;
    string strPlace;

    List<object> listBg;
    public List<object> listShape;
    public List<object> listColor;
    public List<object> listColorShow;
    List<object> listItem;
    public List<object> listGuanka;
    bool isItemHasSel;
    Vector2 ptDownScreen;
    Vector3 posItemWorld;
    ShapeColorItemInfo itemInfoSel;
    float itemPosZ = -20f;

    AudioClip audioClipItemFinish;

    GameObject objBomb;//炸弹

    bool isSelectTheBomb;
    bool isShowAnimalFinish;
    bool isError = false;
    float height_topbar_canvas = 160f;
    void Awake()
    {
        height_topbar_canvas = 160f;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float height_ad_canvas = 128f;
        shaderColor = Shader.Find("Custom/ShapeColor");
        shaderOutLine = Shader.Find("Custom/OutLine");

        UpdateBoard();

        Vector2 worldsize = Common.GetWorldSize(mainCam);
        BoxCollider box = this.gameObject.AddComponent<BoxCollider>();
        box.size = worldsize;

        UITouchEventWithMove ev = this.gameObject.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;
        InitHighLight();
    }


    // Use this for initialization
    void Start()
    {
        //  LayOut();
    }
    // Update is called once per frame 
    void Update()
    {
    }
    public override void LayOut()
    {
        Debug.Log(" GameShapeColor LayOut ");

        Vector2 worldsize = Common.GetWorldSize(mainCam);
        BoxCollider box = this.gameObject.GetComponent<BoxCollider>();
        if (box != null)
        {
            box.size = worldsize;
        }
        UpdateBoard();
        if (isShowAnimalFinish)
        {
            LayOutItems(false);
        }

    }

    public void LayOutItems(bool isAnimate)
    {
        float x, y, w, h;


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
            Debug.Log("LayOut:i=" + info.i + " j=" + info.j + " rc=" + rc + " pt=" + pt + " bd=" + bd.size + " localScale=" + scale);
            float z = obj.transform.position.z;
            obj.transform.position = new Vector3(pt.x, pt.y, z);

            Rect rcDisp = GetRectDisplay();
            Vector3 ptNew = LimitObjPos(obj, rcDisp);
            if (isAnimate)
            {
                obj.transform.localPosition = new Vector3(0, 0, z);
                this.RunActionItemShow(obj, ptNew);
            }
        }
    }

    //防止超出Rect范围
    Vector3 LimitObjPos(GameObject obj, Rect rc)
    {
        Bounds bd = obj.GetComponent<SpriteRenderer>().bounds;
        Vector3 pt = obj.transform.position;
        float w, h;
        w = bd.size.x;
        h = bd.size.y;
        if ((pt.x + w / 2) > (rc.x + rc.width))
        {
            pt.x = rc.x + rc.width - w / 2;
        }
        if ((pt.x - w / 2) < rc.x)
        {
            pt.x = rc.x + w / 2;
        }

        if ((pt.y + h / 2) > (rc.y + rc.height))
        {
            pt.y = rc.y + rc.height - h / 2;
        }
        if ((pt.y - h / 2) < rc.y)
        {
            pt.y = rc.y + h / 2;
        }
        obj.transform.position = pt;
        return pt;
    }

    //item 出现动画
    void RunActionItemShow(GameObject obj, Vector3 toPos)
    {
        float duration = 0.5f;
        obj.transform.DOLocalMove(toPos, duration).OnComplete(() =>
                          {
                              isShowAnimalFinish = true;
                          });

        // var action = cc.moveTo(duration, toPos).easing(cc.easeOut(3.0));
        // //delay延时
        // var time = cc.delayTime(0.1);
        // cc.Debug.Log("RunActionItemShow:" + " toPos=" + toPos);
        // var fun = cc.callFunc(function() {
        //     //node.setPosition(toPos);
        //     //  cc.Debug.Log("RunActionItemShow end:"+" toPos="+toPos);
        // }.bind(this));
        // var seq = cc.sequence([action, fun]);
        // node.runAction(seq);
    }
    void UpdateBoard()
    {
        Vector2 worldsize = Common.GetWorldSize(mainCam);
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;


        float height_topbar_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, height_topbar_canvas) + Device.offsetTopWorld;
        worldsize.y -= (Device.offsetBottomWithAdBannerWorld + height_topbar_world);

        uiBoard.sizeRect = worldsize;
        uiBoard.lineWidth = 10f * AppCommon.scaleBase;
        uiBoard.Init();
        uiBoard.Draw();
        Vector3 pos = uiBoard.transform.position;
        pos.x = 0;
        pos.y = Device.offsetBottomWithAdBannerWorld / 2 - height_topbar_world / 2;
        uiBoard.transform.position = pos;
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                onTouchDown();
                break;

            case UITouchEvent.STATUS_TOUCH_MOVE:
                onTouchMove();
                break;

            case UITouchEvent.STATUS_TOUCH_UP:
                onTouchUp();
                break;
        }


    }
    void onTouchDown()
    {


        isItemHasSel = false;
        Vector2 pos = Common.GetInputPosition();
        ptDownScreen = pos;
        isError = false;
        Vector3 posword = mainCam.ScreenToWorldPoint(pos);
        Debug.Log("onTouchDown: " + posword);
        foreach (ShapeColorItemInfo info in listItem)
        {
            bool isLock = IsItemLock(info);
            if (isLock)
            {
                continue;
            }

            if (iDelegateShapeColor != null)
            {
                iDelegateShapeColor.OnGameShapeColorDidError(this, UIGameShapeColor.ERROR_STATUS_HIDE, null);
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

                // ShapeHighlighterController hlc = AddHighLight(info.obj);
                //   hlc.UpdateColor(color);

                if (info.objTrail != null)
                {
                    ShapeTrail trail = info.objTrail.GetComponent<ShapeTrail>();
                    if (trail != null)
                    {
                        trail.setColor(Color.red);
                        trail.ClearDraw();
                    }
                }

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
        Vector2 ptStepWorld = Common.ScreenToWorldSize(mainCam, ptStep);
        Vector3 posStepWorld = new Vector3(ptStepWorld.x, ptStepWorld.y, 0);
        Vector3 posword = posItemWorld + posStepWorld;

        //将选中item暂时置顶
        posword.z = itemPosZ - 2;
        //itemInfoSel.obj.transform.position = posword;
        Rigidbody2D body = itemInfoSel.obj.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            body.MovePosition(posword);
        }

        //尾巴添加节点
        if (itemInfoSel.objTrail != null)
        {
            Debug.Log("itemInfoSel.objTrail");
            ShapeTrail trail = itemInfoSel.objTrail.GetComponent<ShapeTrail>();
            if (trail != null)
            {
                // trail.AddPoint(posword);
                trail.OnDraw();
            }
        }


        foreach (ShapeColorItemInfo info in listItem)
        {
            isLock = IsItemLock(info);
            if (isLock)
            {
                //  Debug.Log("onTouchMove ng 3");
                // continue;
            }
            Bounds bd = info.obj.GetComponent<SpriteRenderer>().bounds;

            posword.z = bd.center.z;
            w = bd.size.x / 4;
            h = bd.size.y / 4;
            Rect rc = new Rect(info.obj.transform.position.x - w / 2, info.obj.transform.position.y - h / 2, w, h);
            if (info == itemInfoSel)
            {
                continue;
            }

            //  Debug.Log("onTouchMove:posword=" + posword + "rc=" + rc);


            if (rc.Contains(posword))
            {
                if (!isTheSamePairItems(this.itemInfoSel, info))
                {
                    if ((iDelegateShapeColor != null) && (!isError))
                    {
                        isError = true;
                        int error = 0;
                        if ((itemInfoSel.id != info.id) && (itemInfoSel.color != info.color))
                        {
                            error = UIGameShapeColor.ERROR_STATUS_SHAPE_COLOR;
                        }
                        else
                        {
                            if (itemInfoSel.id != info.id)
                            {
                                error = UIGameShapeColor.ERROR_STATUS_SHAPE;
                            }
                            if (itemInfoSel.color != info.color)
                            {
                                error = UIGameShapeColor.ERROR_STATUS_COLOR;
                            }
                        }

                        iDelegateShapeColor.OnGameShapeColorDidError(this, error, null);
                        Invoke("OnGameFail", 0.5f);
                    }
                    continue;
                }
                isSelectTheBomb = false;
                if (itemInfoSel.id == ID_BOMB)
                {
                    isSelectTheBomb = true;
                }
                Debug.Log("合并正确 isSelectTheBomb=" + isSelectTheBomb);

                //合并正确
                SetItemLock(info, true);
                SetItemLock(itemInfoSel, true);

                itemInfoSel.obj.transform.position = info.obj.transform.position;
                RunDisapperAnimation(itemInfoSel);
                PlayAudioItemFinish(itemInfoSel);


                //记录游戏开始进行中
                int level = LevelManager.main.gameLevel;
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

        Rect rcDisp = GetRectDisplay();
        LimitObjPos(itemInfoSel.obj, rcDisp);

    }

    //是否同一对
    bool isTheSamePairItems(ShapeColorItemInfo infoSel, ShapeColorItemInfo info)
    {
        bool ret = false;
        Debug.Log("isTheSamePairItems:infoSel.id=" + infoSel.id + " info.isInner=" + info.isInner);

        if (infoSel.id == ID_BOMB)
        {
            if ((!info.isInner) && (info.isMain))
            {
                ret = true;
            }
        }
        else
        {
            if ((infoSel.id == info.id) && (infoSel.color == info.color))
            {
                ret = true;
            }
        }
        return ret;
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
    Rect GetRectDisplay()
    {
        Rect rc = Rect.zero;
        float x, y, w, h;

        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        float height_topbar_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, height_topbar_canvas) + Device.offsetTopWorld;

        Vector2 szWorld = Common.GetWorldSize(mainCam);
        float ratio = 0.9f;
        w = (szWorld.x - uiBoard.GetWorldLineWidth() * 2) * ratio;
        h = (szWorld.y - height_topbar_world - Device.offsetBottomWithAdBannerWorld);
        rc.width = w;
        rc.height = h;
        rc.x = -w / 2;
        rc.y = -szWorld.y / 2 + Device.offsetBottomWithAdBannerWorld;
        return rc;
    }
    float GetItmeScaleInRect(Rect rc, GameObject obj)
    {
        float scale = 1f, w, h;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        w = sr.sprite.texture.width / 100f;
        h = sr.sprite.texture.height / 100f;

        float ratio = 0.7f;
        float scalex = rc.width * ratio / w;
        float scaley = rc.height * ratio / h;
        scale = Mathf.Min(scalex, scaley);
        if (scale > 1f)
        {
            //scale = 1f;
        }
        return scale;
    }
    // Rect GetRectItem(int i, int j, int totalRow, int totalCol)
    // {
    //     Rect rc = Rect.zero;
    //     float x, y, w, h;
    //     Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
    //     float w_world = Common.GetCameraWorldSizeWidth(mainCam) * 2;
    //     float height_topbar_canvas = 160f;
    //     //  float height_ad_canvas = 128f;
    //     float height_topbar_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, height_topbar_canvas);
    //     // float height_ad_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, height_ad_canvas);
    //     //   height_ad_world += Common.ScreenToWorldHeight(mainCam, Device.heightSystemHomeBar);
    //     if (!Device.isLandscape)
    //     {
    //         height_topbar_world += Common.ScreenToWorldHeight(mainCam, Device.heightSystemTopBar);
    //     }
    //     float h_world = mainCam.orthographicSize * 2;
    //     w = w_world / totalCol;
    //     h = (h_world - height_topbar_world - height_ad_world) / totalRow;
    //     float oftx = -w_world / 2;
    //     float ofty = -h_world / 2 + height_ad_world;
    //     x = oftx + w * i;
    //     y = ofty + h * j;
    //     rc = new Rect(x, y, w, h);
    //     return rc;
    // }

    Rect GetRectItem(int i, int j, int row, int col)
    {
        float x, y, w, h;
        Rect rcDisplay = GetRectDisplay();
        w = rcDisplay.width / col;
        h = rcDisplay.height / row;
        x = rcDisplay.x + w * i;
        y = rcDisplay.y + h * j;
        Rect rc = new Rect(x, y, w, h);
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

    //创建炸弹
    void CreateBomb(Color color)
    {
        if (objBomb != null)
        {
            return;
        }
        objBomb = new GameObject("Bomb");
        objBomb.transform.parent = this.transform;

        float z = itemPosZ - 5;
        objBomb.transform.position = new Vector3(0, 0, z);

        SpriteRenderer objSR = objBomb.AddComponent<SpriteRenderer>();
        TextureUtil.UpdateSpriteTexture(objBomb, AppRes.IMAGE_Game_Bomb);
        // 高亮颜色
        {
            ShapeHighlighterController hlc = AddHighLight(objBomb);
            hlc.UpdateColor(color);
        }


        //添加物理特性 
        {
            Rigidbody2D bd = objBomb.AddComponent<Rigidbody2D>();
            bd.gravityScale = 0;
            // bd.useGravity = false;
            bd.freezeRotation = true;
            bd.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            PolygonCollider2D collider = objBomb.AddComponent<PolygonCollider2D>();
        }

        ShapeColorItemInfo infoItem = new ShapeColorItemInfo();
        infoItem.obj = objBomb;
        // infoItem.objTrail = infoshape.objTrail;
        infoItem.id = ID_BOMB;
        //   infoItem.color = infocolor.color;
        infoItem.isMain = false;
        infoItem.isInner = false;
        SetItemLock(infoItem, false);

        listItem.Add(infoItem);
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
        obj.transform.parent = this.transform;

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

        float z = itemPosZ;
        if (isInner)
        {
            z = itemPosZ - 1;
        }
        obj.transform.position = new Vector3(0, 0, z);

        bool is_add_shader = true;
        //color
        // if (Common.appKeyName != AppType.SHAPECOLOR)
        if (!info.isMath)
        {
            if (isInner)
            {
                is_add_shader = false;
                // objSR.material = new Material(shaderOutLine);
                // objSR.material.SetColor("_OutlineColor", color);
                ShapeHighlighterController hlc = AddHighLight(obj);
                hlc.UpdateColor(color);
            }
        }
        if (is_add_shader)
        {
            objSR.material = new Material(shaderColor);
            Material mat = objSR.material;
            mat.SetColor("_ColorShape", color);
        }


        //添加物理特性
        if (isInner)
        {
            Rigidbody2D bd = obj.AddComponent<Rigidbody2D>();
            bd.gravityScale = 0;
            // bd.useGravity = false;
            bd.freezeRotation = true;
            bd.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            PolygonCollider2D collider = obj.AddComponent<PolygonCollider2D>();
        }

        //添加尾巴 ShapeTrail
        // if (isInner)
        // {
        //     info.objTrail = new GameObject("trail");
        //     info.objTrail.transform.parent = obj.transform;
        //     info.objTrail.transform.localPosition = Vector3.zero;
        //     ShapeTrail trail = info.objTrail.AddComponent<ShapeTrail>();

        // }

        return obj;
    }

    public void LoadGame(int mode)
    {
        Debug.Log("LoadGame:mode=" + mode);

        isShowAnimalFinish = false;
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

        if (AppVersion.appCheckHasFinished)
        {
            if (listColorShow.Count != 0)
            {
                int idx = Random.Range(0, listColorShow.Count);
                ShapeColorItemInfo infocolor = listColor[idx] as ShapeColorItemInfo;
                // CreateBomb(infocolor.color);
            }
        }

        LayOutItems(true);
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

    void InitHighLight()
    {
        if (mainCam.GetComponent<HighlightingRenderer>() == null)
        {
            mainCam.gameObject.AddComponent<HighlightingRenderer>();
        }
    }
    void RemoveHighLight(GameObject obj)
    {
        if (obj != null)
        {
            //remove
            ShapeHighlighterController sc = obj.GetComponent<ShapeHighlighterController>();
            if (sc != null)
            {
                DestroyImmediate(sc);
            }

            Highlighter hl = obj.GetComponent<Highlighter>();
            if (hl != null)
            {
                DestroyImmediate(hl);
            }

        }
    }
    ShapeHighlighterController AddHighLight(GameObject obj)
    {

        return obj.AddComponent<ShapeHighlighterController>();

    }

    ShapeColorItemInfo AddItem(Rect rc, ShapeColorItemInfo infoshape, ShapeColorItemInfo infocolor, GameObject obj, bool isInner, bool isMain)
    {
        ShapeColorItemInfo infoItem = new ShapeColorItemInfo();
        infoItem.obj = obj;
        infoItem.objTrail = infoshape.objTrail;
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
        int level = LevelManager.main.gameLevel;
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


        int level = LevelManager.main.gameLevel;
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
        int level = LevelManager.main.gameLevel;
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
        if (isSelectTheBomb)
        {
            //显示踩到炸弹了 游戏结束
            OnGameBomb();
            return;
        }
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
            OnGameWin();

        }

    }

    void OnGameBomb()
    {
        if (iDelegateShapeColor != null)
        {
            iDelegateShapeColor.OnGameShapeColorDidBomb(this);
        }
    }
    public void OnGameWin()
    {
        //show game win
        LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;
        //gameEndParticle.Play();

        //记录游戏完成
        int level = LevelManager.main.gameLevel;
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
        if (iDelegateShapeColor != null)
        {
            iDelegateShapeColor.OnGameShapeColorDidWin(this);
        }

    }


    public ShapeColorItemInfo GetItemInfoShapeColor(int idx, List<object> list)
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

    void PlayAudioItemFinish(ShapeColorItemInfo info)
    {
        if (info.id == ID_BOMB)
        {
            return;
        }
        //AudioPlayer对象在场景切换后可能从当前scene移除了
        // GameObject audioPlayer = GameObject.Find("AudioPlayer");
        // if (audioPlayer != null)
        // {
        //     AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
        //     audioSource.PlayOneShot(audioClipItemFinish);
        // }


        if (iDelegateShapeColor != null)
        {
            iDelegateShapeColor.OnGameShapeColorDidError(this, UIGameShapeColor.ERROR_STATUS_NONE, info);
        }
    }
    void RunDisapperAnimation(ShapeColorItemInfo info)
    {
        // iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);

        // 10 秒内 比例变化到  （5，5，5）  
        info.obj.transform.DOScale(new Vector3(0f, 0f, 0f), 1.5f).OnComplete(() =>
          {
              CheckGameWin();
          });
    }



    public void OnTextureHttpRequestFinished(bool isSuccess, Texture2D tex, object data)
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
        //  LayOut();
    }

}
