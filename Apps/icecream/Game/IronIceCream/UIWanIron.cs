using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HighlightingSystem;

/*铁板冰淇淋 碗
*/
public class UIWanIron : UIView
{
    public const string IMAGE_EatErase = "App/UI/Game/EatErase";
    public List<TopFoodItemInfo> listItem;
    public GameObject objWanFt;//碗 
    public GameObject objWanBg;//碗bg Wan_bg
    public GameObject objWanItemRoot;//顶料
    public GameObject objJuan;//冰淇凌券
    public GameObject objJuanitem0;
    public GameObject objJuanitem1;
    public GameObject objJuanitem2;
    public GameObject objJuanitem3;
    public GameObject objJuanitem4;
    public GameObject objJuanitem5;
    public GameObject objErase;//碗  

    GameObject objItemSelect;//选中的顶料

    public MeshTexture meshTex;
    Rect rectMain;//local 
    RenderTexture rtMain;
    public Camera camWan;
    Vector3 posLocalTouchDown;
    Vector3 posInputTouchDown;
    Material matErase;

    Material matEat;
    Texture2D texBrush;

    public GameObject[] listJuan = new GameObject[6];
    int indexLayer = 8;//Layer8

    int indexStep = 0;

    //选中描边高亮

    protected struct Preset
    {
        public string name;
        public int downsampleFactor;
        public int iterations;
        public float blurMinSpread;
        public float blurSpread;
        public float blurIntensity;
    }

    List<Preset> presets = new List<Preset>()
    {
        new Preset() { name = "Default",    downsampleFactor = 4,   iterations = 2, blurMinSpread = 0.65f,  blurSpread = 0.25f, blurIntensity = 0.3f },
        new Preset() { name = "Strong",     downsampleFactor = 4,   iterations = 2, blurMinSpread = 0.5f,   blurSpread = 0.15f, blurIntensity = 0.325f },
        new Preset() { name = "Wide",       downsampleFactor = 4,   iterations = 4, blurMinSpread = 0.5f,   blurSpread = 0.15f, blurIntensity = 0.325f },
        new Preset() { name = "Speed",      downsampleFactor = 4,   iterations = 1, blurMinSpread = 0.75f,  blurSpread = 0f,    blurIntensity = 0.35f },
        new Preset() { name = "Quality",    downsampleFactor = 2,   iterations = 3, blurMinSpread = 0.5f,   blurSpread = 0.5f,  blurIntensity = 0.28f },
        new Preset() { name = "Solid 1px",  downsampleFactor = 1,   iterations = 1, blurMinSpread = 1f,     blurSpread = 0f,    blurIntensity = 1f },
        new Preset() { name = "Solid 2px",  downsampleFactor = 1,   iterations = 2, blurMinSpread = 1f,     blurSpread = 0f,    blurIntensity = 1f }
    };
    //
    void Awake()
    {
        listItem = new List<TopFoodItemInfo>();
        GameObject[] listJuanTmp = { objJuanitem0, objJuanitem1, objJuanitem2, objJuanitem3, objJuanitem4, objJuanitem5 };
        for (int i = 0; i < listJuanTmp.Length; i++)
        {
            listJuan[i] = listJuanTmp[i];
        }
        texBrush = TextureCache.main.Load("App/UI/Brush/brush_dot");
        TextureUtil.UpdateSpriteTexture(objWanBg, UITopFoodItem.IMAGE_WAN_BG);
        TextureUtil.UpdateSpriteTexture(objErase, texBrush);
        matErase = new Material(Shader.Find("Custom/Erase"));
        matEat = new Material(Shader.Find("Custom/IceCreamEat"));
        rtMain = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        camWan.targetTexture = rtMain;
        SpriteRenderer rd = objErase.GetComponent<SpriteRenderer>();
        rd.material = matErase;

        int layer = indexLayer;
        objErase.layer = layer;
        mainCam.cullingMask &= ~(1 << layer); // 关闭层x
                                              // mainCam.cullingMask |= (1 << layer);  // 打开层x

        objErase.SetActive(false);
        if (mainCam.GetComponent<HighlightingRenderer>() == null)
            mainCam.gameObject.AddComponent<HighlightingRenderer>();

    }
    void Start()
    {
        meshTex.EnableTouch(false);
        meshTex.UpdateMaterial(matEat);
        meshTex.UpdateTexture(rtMain);
        Vector2 worldsize = Common.GetWorldSize(mainCam);
        meshTex.UpdateSize(worldsize.x, worldsize.y);
        UITouchEventWithMove ev = meshTex.gameObject.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;

        LayOut();
    }

    public override void LayOut()
    {
        float x, y, z, w, h;
        float scale = 0;
        float ratio = 0.95f;



        {
            SpriteRenderer rd = objWanBg.GetComponent<SpriteRenderer>();
            if (rd.sprite != null)
            {
                w = rd.sprite.texture.width / 100f;
                h = rd.sprite.texture.height / 100f;
                scale = Common.GetBestFitScale(w, h, rectMain.width, rectMain.height) * ratio;
                objWanBg.transform.localScale = new Vector3(scale, scale, 1f);
                z = objWanBg.transform.localPosition.z;
                objWanBg.transform.localPosition = new Vector3(rectMain.center.x, rectMain.center.y, z);
            }
        }
        {
            SpriteRenderer rd = objWanFt.GetComponent<SpriteRenderer>();
            if (rd.sprite != null)
            {
                w = rd.sprite.texture.width / 100f;
                h = rd.sprite.texture.height / 100f;
                scale = Common.GetBestFitScale(w, h, rectMain.width, rectMain.height) * ratio;
                objWanFt.transform.localScale = new Vector3(scale, scale, 1f);
                z = objWanFt.transform.localPosition.z;
                objWanFt.transform.localPosition = new Vector3(rectMain.center.x, rectMain.center.y, z);
            }
        }
        {
            SpriteRenderer render = objErase.GetComponent<SpriteRenderer>();
            if (render.sprite && render.sprite.texture)
            {
                w = render.sprite.texture.width / 100f;
                h = render.sprite.texture.height / 100f;
                if ((w != 0) && (h != 0))
                {
                    scale = (rectMain.width / 10) / w;
                    objErase.transform.localScale = new Vector3(scale, scale, 1f);
                }


            }

        }


        {
            z = objJuan.transform.localPosition.z;
            objJuan.transform.localPosition = new Vector3(0, 0.15f, z);
        }

        {
            for (int i = 0; i < listJuan.Length; i++)
            {
                GameObject obj = listJuan[i];
                SpriteRenderer rd = obj.GetComponent<SpriteRenderer>();
                if ((rd != null) && (rd.sprite != null))
                {
                    Vector2 sizeRc = GetJuanRectSize();
                    w = sizeRc.x / 3;
                    h = sizeRc.y / 2;
                    ratio = 2f;// 1.8f;
                    float w_tex = rd.sprite.texture.width / 100f;
                    float h_tex = rd.sprite.texture.height / 100f;
                    scale = Common.GetMaxFitScale(w_tex, h_tex, w, h) * ratio;
                    //scale = Common.GetBestFitScale(w_tex, h_tex, w, h);
                    obj.transform.localScale = new Vector3(scale, scale, 1f);
                    int r = i / 3;
                    int c = i % 3;
                    Vector2 pt = GetJuanItemPostion(r, c);
                    z = obj.transform.localPosition.z;
                    obj.transform.localPosition = new Vector3(pt.x, pt.y, z);
                }
            }
        }
    }

    void RemoveHighLight()
    {
        if (objItemSelect != null)
        {
            //remove
            TopFoodHighlighterController sc = objItemSelect.GetComponent<TopFoodHighlighterController>();
            if (sc != null)
            {
                DestroyImmediate(sc);
            }

            Highlighter hl = objItemSelect.GetComponent<Highlighter>();
            if (hl != null)
            {
                DestroyImmediate(hl);
            }

        }
    }
    void SetPresetSettings(Preset p)
    {
        HighlightingBase hb = mainCam.GetComponent<HighlightingBase>();//FindObjectOfType<HighlightingBase>();
        Debug.Log("SetPresetSettings");
        if (hb == null)
        {
            Debug.Log("SetPresetSettings is null");
            return;
        }

        hb.downsampleFactor = p.downsampleFactor;
        hb.iterations = p.iterations;
        hb.blurMinSpread = p.blurMinSpread;
        hb.blurSpread = p.blurSpread;
        hb.blurIntensity = p.blurIntensity;
    }
    public void UpdateRect(Rect rc)
    {
        rectMain = rc;
        LayOut();


    }

    public void UpdateStep(int idx)
    {
        indexStep = idx;
        meshTex.EnableTouch(false);
        if (indexStep == GameIronIceCream.INDEX_STEP_CHI)
        {
            meshTex.EnableTouch(true);

            //显示到meshtex
            foreach (TopFoodItemInfo info in listItem)
            {
                info.obj.layer = indexLayer;
            }
        }
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        if (indexStep == GameIronIceCream.INDEX_STEP_ZHUANG)
        {
            OnUITouchEventTopFood(ev, eventData, status);
        }
        if (indexStep == GameIronIceCream.INDEX_STEP_CHI)
        {
            OnUITouchEventEat(ev, eventData, status);

        }
    }
    public void OnUITouchEventTopFood(UITouchEvent ev, PointerEventData eventData, int status)
    {
        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = this.transform.InverseTransformPoint(posworld);
        poslocal.z = ev.gameObject.transform.localPosition.z;

        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {

                    RemoveHighLight();
                    objItemSelect = ev.gameObject;
                    objItemSelect.AddComponent<TopFoodHighlighterController>();

                    posInputTouchDown = posworld;
                    posLocalTouchDown = poslocal;
                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {
                    Vector3 step = posworld - posInputTouchDown;

                    Vector3 posnow = posLocalTouchDown + step;
                    posnow.z = poslocal.z;
                    ev.gameObject.transform.localPosition = posnow;
                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {

                }
                break;
        }
    }

    public void OnUITouchEventEat(UITouchEvent ev, PointerEventData eventData, int status)
    {
        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = this.transform.InverseTransformPoint(posworld);
        poslocal.z = objErase.transform.localPosition.z;

        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    StartEat();

                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {

                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {

                }
                break;
        }

        objErase.transform.localPosition = poslocal;
    }

    //添加顶料
    public void OnAddTopFood(FoodItemInfo info)
    {
        float x, y, w, h;
        string pic = info.pic;
        TopFoodItemInfo infoTop = new TopFoodItemInfo();
        infoTop.name = info.id + "_" + FileUtil.GetFileName(pic);

        GameObject obj = new GameObject(infoTop.name);
        SpriteRenderer rd = obj.AddComponent<SpriteRenderer>();
        Texture2D tex = TextureCache.main.Load(pic);
        rd.sprite = LoadTexture.CreateSprieFromTex(tex);

        BoxCollider box = obj.AddComponent<BoxCollider>();
        box.size = new Vector2(tex.width / 100f, tex.height / 100f);

        UITouchEventWithMove ev = obj.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;

        //AppSceneBase.main.AddObjToMainWorld(obj);
        obj.transform.SetParent(objWanItemRoot.transform);

        SpriteRenderer rdbg = objWanBg.GetComponent<SpriteRenderer>();
        float scale = 0f;
        float ratio = 0.33f;//0.25f;
        scale = Common.GetBestFitScale(tex.width / 100f, tex.height / 100f, rdbg.bounds.size.x * ratio, rdbg.bounds.size.y * ratio);
        obj.transform.localScale = new Vector3(scale, scale, 1);

        infoTop.obj = obj;
        infoTop.pt = Vector3.zero;
        RemoveHighLight();
        objItemSelect = obj;
        objItemSelect.AddComponent<TopFoodHighlighterController>();
        SetPresetSettings(presets[5]);
        listItem.Add(infoTop);
        float oft_y = 0.3f;
        float z_juan = objJuan.transform.localPosition.z;
        if (info.isUnderJuan)
        {
            //统一放在碗的右上角 
            w = rd.bounds.size.x;
            h = rd.bounds.size.y;
            x = rdbg.bounds.size.x / 2 - w;
            y = rdbg.bounds.size.y / 2 - h/2;
            obj.transform.localPosition = new Vector3(x, y, 1f);
        }
        else
        {
            obj.transform.localPosition = new Vector3(0, oft_y, -1 * listItem.Count);
        }
    }

    //删除选中顶料
    public void OnDeleteTopFood()
    {
        if (objItemSelect != null)
        {
            TopFoodItemInfo infoSel = null;
            foreach (TopFoodItemInfo info in listItem)
            {
                if (info.obj == objItemSelect)
                {
                    infoSel = info;
                    break;
                }
            }
            if (infoSel != null)
            {
                listItem.Remove(infoSel);
            }
            DestroyImmediate(objItemSelect);
            objItemSelect = null;
        }
    }
    //逆时针旋转选中顶料
    public void OnRotationAddTopFood()
    {
        float step_angle = 15f;
        if (objItemSelect != null)
        {
            float angle_z = objItemSelect.transform.localRotation.eulerAngles.z;
            objItemSelect.transform.localRotation = Quaternion.Euler(0, 0, angle_z + step_angle);
        }
    }

    //顺时针旋转选中顶料
    public void OnRotationMinusTopFood()
    {
        float step_angle = 15f;
        if (objItemSelect != null)
        {
            float angle_z = objItemSelect.transform.localRotation.eulerAngles.z;
            objItemSelect.transform.localRotation = Quaternion.Euler(0, 0, angle_z - step_angle);
        }
    }

    //缩小选中顶料
    public void OnScaleMinusTopFood()
    {
        float step = 0.1f;
        if (objItemSelect != null)
        {
            float scale = objItemSelect.transform.localScale.x - step;
            if (scale <= 0)
            {
                scale = step;
            }
            objItemSelect.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
    //放大选中顶料
    public void OnScaleAddTopFood()
    {
        float step = 0.1f;
        float scale_max = 3f;
        if (objItemSelect != null)
        {
            float scale = objItemSelect.transform.localScale.x + step;
            if (scale > scale_max)
            {
                scale = scale_max;
            }
            objItemSelect.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
    void UpdateJuanItem(GameObject obj, float w, float h)
    {
        Texture2D tex = TextureCache.main.Load(IronIceCreamStepBase.GetWanJuanPic(GameIronIceCream.indexFood));
        TextureUtil.UpdateSpriteTexture(obj, tex);
        // BoxCollider box = obj.GetComponent<BoxCollider>();
        // box.size = new Vector3(tex.width / 100f, tex.height / 100f);


    }


    // r 行 ; c 列  返回中心位置
    public Vector2 GetJuanRectSize()
    {
        float w, h;
        SpriteRenderer rd = objWanBg.GetComponent<SpriteRenderer>();
        float ratio = 0.5f;
        w = rd.bounds.size.x * ratio;
        h = rd.bounds.size.y * ratio;
        return new Vector2(w, h);
    }

    public Vector2 GetJuanItemPostion(int r, int c)
    {
        float x, y, w, h;
        int row = 2;
        int col = 3;
        Vector2 sizeRc = GetJuanRectSize();
        float item_w = sizeRc.x / col;
        float item_h = sizeRc.y / row;

        x = -sizeRc.x / 2 + item_w * c + item_w / 2;
        y = -sizeRc.y / 2 + item_h * r + item_h / 2;

        return new Vector2(x, y);

    }
    //放置冰淇凌卷
    public void UpdateJuan()
    {

        SpriteRenderer rd = objWanBg.GetComponent<SpriteRenderer>();
        float x, y, w, h;
        float ratio = 0.5f;
        w = rd.bounds.size.x * ratio;
        h = rd.bounds.size.y * ratio;
        //  GridLayoutGroup gridLayout = objJuan.GetComponent<GridLayoutGroup>();

        ///  gridLayout.cellSize = new Vector2(w / 3, h / 2);

        RectTransform rctran = objJuan.GetComponent<RectTransform>();
        rctran.sizeDelta = new Vector2(w, h);

        foreach (GameObject obj in listJuan)
        {
            UpdateJuanItem(obj, w / 3, h / 2);
        }
        LayOut();
    }
    public void ShowJuan(bool isShow)
    {
        for (int i = 0; i < listJuan.Length; i++)
        {
            ShowJuanItem(isShow, i);
        }

    }
    public void ShowJuanItem(bool isShow, int idx)
    {
        GameObject obj = listJuan[idx];
        obj.SetActive(isShow);
    }

    public void UpdateWan(string pic)
    {
        // objWan.SetActive(true);
        TextureUtil.UpdateSpriteTexture(objWanFt, pic);
        //  strImageWan = pic;
        LayOut();

    }

    public void StartEat()
    {
        objErase.SetActive(true);
        objWanItemRoot.SetActive(false);
    }

}
