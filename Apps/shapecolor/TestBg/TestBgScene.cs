using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class TestBgScene : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject objSpriteBg;
    public Text textTitle;

    List<object> listBg;
    List<object> listShape;
    List<object> listColor;
    List<object> listItem;
    Shader shaderColor;

    int indexBg;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string strshader = "Custom/ShapeColor";
        shaderColor = Shader.Find(strshader);

        ParseBgList();
        ParseShape();
        ParseColor();
        indexBg = 0;
        LoadBg();
    }
    // Use this for initialization
    void Start()
    {

        ShowAllItem();
        LayOutChild();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void LayOutChild()
    {
        {
            SpriteRenderer spRender = objSpriteBg.GetComponent<SpriteRenderer>();
            Vector2 world_size = Common.ScreenToWorldSize(mainCamera, new Vector2(Screen.width, Screen.height));
            float scalex = world_size.x / spRender.size.x;
            float scaley = world_size.y / spRender.size.y;
            float scale = Mathf.Max(scalex, scaley);
            Debug.Log("spRender.size=" + spRender.size);
            objSpriteBg.transform.localScale = new Vector3(scale, scale, 1f);

        }


    }
    void LoadBg()
    {
        SpriteRenderer spRender = objSpriteBg.GetComponent<SpriteRenderer>();
        ShapeColorItemInfo info = GetItemInfo(indexBg);
        Texture2D tex = LoadTexture.LoadFromAsset(info.pic);
        Sprite sp = LoadTexture.CreateSprieFromTex(tex);
        spRender.size = new Vector2(tex.width / 100, tex.height / 100);
        spRender.sprite = sp;
        textTitle.text = FileUtil.GetFileName(info.pic);
        LayOutChild();
    }
    ShapeColorItemInfo GetItemInfo(int idx)
    {
        if (listBg == null)
        {
            return null;
        }
        if (idx >= listBg.Count)
        {
            return null;
        }
        ShapeColorItemInfo info = listBg[idx] as ShapeColorItemInfo;
        return info;
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
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            string strdir = Common.GAME_RES_DIR + "/image/bg";
            info.pic = strdir + "/" + (string)item["pic"];

            listBg.Add(info);
        }
    }

    public void ParseShape()
    {
        if ((listShape != null) && (listShape.Count != 0))
        {
            return;
        }

        listShape = new List<object>();
        int idx = GameManager.placeLevel;
        string fileName = Common.GAME_RES_DIR + "/guanka/shape.json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
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

    public void ParseColor()
    {
        if ((listColor != null) && (listColor.Count != 0))
        {
            return;
        }

        listColor = new List<object>();
        int idx = GameManager.placeLevel;
        string fileName = Common.GAME_RES_DIR + "/guanka/color.json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
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


    Rect GetRectItem(int i, int j, int totalRow, int totalCol)
    {
        Rect rc = Rect.zero;
        float x, y, w, h;
        float w_world = Common.GetCameraWorldSizeWidth(mainCamera) * 2;
        float h_world = mainCamera.orthographicSize * 2;
        w = w_world / totalCol;
        h = h_world / totalRow;
        float oftx = -w_world / 2;
        float ofty = -h_world / 2;
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



    Texture2D CreateTexTureBg(int w, int h)
    {
        Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
       // return tex;
        ColorImage crImage = new ColorImage();
        crImage.Init(tex);
        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                Vector2 pttmp = new Vector2(i, j);
                Color colorpic = new Color(0f, 0f, 0f, 0f);
                crImage.SetImageColor(pttmp, colorpic);
            }
        }

        crImage.UpdateTexture();

        return tex;
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

        obj.AddComponent<RectTransform>();
        RectTransform rcTran = obj.GetComponent<RectTransform>();
        obj.AddComponent<SpriteRenderer>();
        SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();
        string pic = info.picOuter;
        if (isInner)
        {
            pic = info.picInner;
        }

        
        Sprite sprite = LoadTexture.CreateSprieFromAsset(pic);
        sprite.name = info.id;
        objSR.sprite = sprite;
 
        
        
        //objSR.size = new Vector2(sprite.texture.width/100,sprite.texture.height/100);
        //rcTran.sizeDelta = new Vector2(objSR.size.x, objSR.size.y);
        float itemPosZ = -5f;
        obj.transform.position = new Vector3(0, 0, itemPosZ);

        //color

        objSR.material = new Material(shaderColor);
        Material mat = objSR.material;
        mat.SetColor("_ColorShape", color);
        //mat.SetTexture("_MainTex", texPic);


        return obj;
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
    void ShowAllItem()
    {

        int totalRow = 0;
        int totalCol = 0;
        int totalItem = listShape.Count;
        int sqr = (int)Mathf.Sqrt(totalItem);
        if (totalItem > sqr * sqr)
        {
            sqr++;
        }
        totalRow = sqr;
        totalCol = sqr;
        Debug.Log("totalItem=" + totalItem + " row=" + totalRow + " col=" + totalCol);

        int[] indexRectList = RandomIndex(totalRow * totalCol, totalItem);

        for (int k = 0; k < listShape.Count; k++)
        {
            int indexRect = indexRectList[k];
            int i = indexRect % totalCol;
            int j = indexRect / totalRow;

            ShapeColorItemInfo infoshape = listShape[k] as ShapeColorItemInfo;
            ShapeColorItemInfo infocolor = listColor[k] as ShapeColorItemInfo;

            GameObject obj = null;
            bool isInner = false;

            obj = CreateItem(infoshape, isInner, infocolor.color);
            Rect rc = GetRectItem(i, j, totalRow, totalCol);
            SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();
            Bounds bd = objSR.bounds;
            float offsetx = bd.size.x / 2;
            //offsetx =0;
            float offsety = bd.size.y / 2;
            //offsety=0;
            Vector2 pt = RandomPointOfRect(rc, offsetx, offsety);
            Debug.Log("CreateItem:i=" + i + " j=" + j + " rc=" + rc + " pt=" + pt + " bd=" + bd.size);
            obj.transform.position = new Vector3(pt.x, pt.y, obj.transform.position.z);


        }


    }

    public void OnClickBtnNext()
    {
        indexBg++;
        if (indexBg >= listBg.Count)
        {
            indexBg = 0;
        }
        LoadBg();
    }
    public void OnClickBtnPre()
    {
        indexBg--;
        if (indexBg < 0)
        {
            indexBg = listBg.Count - 1;
        }
        LoadBg();
    }
}
