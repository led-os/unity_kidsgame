using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorItemPoint
{
    public string x;//(x,y)
    public string y;
    public string angle;
    public string guide_type;
    public bool isKey;
}

public class JsonItemPoint
{
    public string x;//(x,y)
    public string y;

}

public class JsonGuideItemInfo
{
    public string angle;
    public string x;
    public string y;
    public string type;
    public string direction;
}
public class EditorWordPoint : UIGameBase
{ 
    public Camera cameraBihuaShow;
    public Canvas canvasWord;
    public LineRenderer lineRenderer;
    public Text textTitle;
    public Button btnXml2Json;
    public Button btnKeyPoint;
    public InputField inputFieldWord;
    public Text textWord;
    public GameObject objWordPoint;
    public GameObject objWordWrite;
    public GameObject objWordBihuaShow;//笔划示意图 

    List<GameObject> listObjBihua;
    List<GameObject> listObjBihuaGuide;
    List<GameObject> listObjBihuaWordWrite;
    List<EditorItemPoint> listJsonPoint;//current bihua
    List<Vector3> listTouchPoint;
    List<object> listDemoPointAll;
    List<object> listGuidePointAll;

    float letterImageZ = -20f;
    int indexBihua;
    int indexBihuaPoint;
    int widthImage;
    int heightImage;
    float guideImageOffsetX;
    float guideImageOffsetY;
    Vector2 sizeGuideImage;

    Hashtable dataJsonRoot;
    Hashtable dataJsonDemopoint;
    Hashtable dataJsonGuidepoint;
    UIGameBase gameBase;
    UIGameXieHanzi gameXieHanzi;
    bool isKeyPoint = false;
    GameObject objBihuaCur;

    WordBihuaShow wordBihuaShow;

    WordWrite wordWrite;
    WordWrite wordWriteAjustLine;
    WordWrite wordWritePrefab;
    int indexWordWrite;
    float drawLineWidth = 0.5f;

    Rect rectWordWrite;
    Color colorWord;
    //xml 
    WordXmlPoint2Json wordXmlPoint2Json;

    int textWordWidth;//canvas
    int textWordHeight;//canvas

    Vector3 posTouchDown;
    bool isTouchDown = false;

    bool enableAjustline = false;

    WordItemInfo infoWord;
    string rootDirImage;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        infoWord = new WordItemInfo();
        listDemoPointAll = new List<object>();
        listGuidePointAll = new List<object>();

        listObjBihua = new List<GameObject>();
        listObjBihuaWordWrite = new List<GameObject>();
        listTouchPoint = new List<Vector3>();
        listJsonPoint = new List<EditorItemPoint>();
        indexBihua = 0;
        enableAjustline = false;

        rootDirImage = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR + "/image_new";


        dataJsonRoot = new Hashtable();
        dataJsonDemopoint = new Hashtable();
        dataJsonGuidepoint = new Hashtable();
        dataJsonRoot["demo_point"] = dataJsonDemopoint;
        dataJsonRoot["guide_point"] = dataJsonGuidepoint;

        GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/WordWrite");
        wordWritePrefab = obj.GetComponent<WordWrite>();



        InitWord();
        InitAjustLine();
        wordBihuaShow = objWordBihuaShow.AddComponent<WordBihuaShow>();
        wordBihuaShow.editorPoint = this;
        wordBihuaShow.gameObject.SetActive(false);
        wordBihuaShow.wordWritePrefab = wordWritePrefab;

        gameBase = GameViewController.main.gameBase;
        gameXieHanzi = gameBase as UIGameXieHanzi;
        LevelManager.main.placeLevel = 0;
        //gameXieHanzi.ParseGuanka();


        //wordXmlPoint2Json = this.gameObject.AddComponent<WordXmlPoint2Json>();


        UITouchEvent touch_ev = textWord.gameObject.AddComponent<UITouchEventWithMove>();
        touch_ev.callBackTouch = OnWordTouchEvent;



    }
    // Use this for initialization
    void Start()
    {
        float x, y, w, h;
        CreateLine();
        // WordItemInfo info = GetItemInfo(gameLevel);
        // gameXieHanzi.ParseGuankaItem(info);


        Debug.Log("textWord fontSize=" + textWord.fontSize + " rectWordWrite=" + rectWordWrite);
        UpdateTitle();

        UpdateWordInfo(textWord.text);
        //DrawAjustLine(Vector3.zero);

        //TestLineRender();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {

            //  Debug.Log("compositionCursorPos=" + Input.compositionCursorPos + " mousePosition=" + Input.mousePosition);
            // if (Input.GetMouseButton(0))
            {
                // Debug.Log("GetMouseButton");
                onTouchMove();
            }

        }
    }

    void InitWord()
    {
        float x, y, w, h;
        Rect rc = (canvasWord.transform as RectTransform).rect;
        Vector2 sizeWordCanvas = new Vector2(rc.width, rc.height);
        int topbar_h = (int)(160 * AppCommon.scaleBase);
        textWordWidth = (int)(Mathf.Min(sizeWordCanvas.x, sizeWordCanvas.y - topbar_h * 2));
        textWordHeight = textWordWidth;

        widthImage = (int)Common.CanvasToScreenWidth(sizeWordCanvas, textWordWidth);
        heightImage = (int)Common.CanvasToScreenHeight(sizeWordCanvas, textWordHeight);
        Debug.Log("widthImage=" + widthImage + " heightImage=" + heightImage);

        w = Common.ScreenToWorldWidth(mainCam, widthImage);
        h = Common.ScreenToWorldHeight(mainCam, heightImage);
        x = -w / 2;
        y = -h / 2;
        rectWordWrite = new Rect(x, y, w, h);

        RectTransform rctran = textWord.GetComponent<RectTransform>();
        rctran.sizeDelta = new Vector2(textWordWidth, textWordHeight);

        colorWord = Color.red;
    }
    void UpdateWordInfo(string word)
    {
        string key = "";
        if (Common.BlankString(word))
        {
            key = "word";
        }
        else
        {
            key = word;
        }
        infoWord.id = "id_" + key;
    }
    public void OnWordTouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
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
        //ClearLinePoint();
        Vector2 pos = Common.GetInputPosition();
        posTouchDown = mainCam.ScreenToWorldPoint(pos);
        posTouchDown.z = letterImageZ - 1;
        isTouchDown = true;
        ClearAjustLine();
    }
    void onTouchMove()
    {

        Vector2 pos = Common.GetInputPosition();
        Vector3 posword = mainCam.ScreenToWorldPoint(pos);
        posword.z = letterImageZ - 1;
        //Debug.Log("pos=" + pos + " mousePosition=" + Input.mousePosition);
        DrawAjustLine(posword);

        if (!isTouchDown)
        {
            return;
        }

    }
    void onTouchUp()
    {
        isTouchDown = false;
        Vector2 pos = Common.GetInputPosition();
        Vector3 posword = mainCam.ScreenToWorldPoint(pos);
        Vector2 postmp = posword;
        if (rectWordWrite.Contains(postmp))
        {
            posword.z = letterImageZ - 1;
            AddLinePoint(posword);
            enableAjustline = true;
        }


    }

    public void LoadGame()
    {
        // gameBaseRun = (GameBase)GameObject.Instantiate(gameBase);
        // gameBase.mainCam = mainCam;
        // gameBaseRun.mainCam = mainCam; 
    }
    void UpdateTitle()
    {
        textTitle.text = "笔划:" + indexBihua;
    }
    void ClearLinePoint()
    {
        listTouchPoint.Clear();
        listJsonPoint.Clear();
        lineRenderer.positionCount = 0;
        listJsonPoint = new List<EditorItemPoint>();
        DestroyLetterBihuaGuide();
    }
    void AddLinePoint(Vector3 pt)
    {
        listTouchPoint.Add(pt);
        EditorItemPoint jsonitem = new EditorItemPoint();
        float ratio_x = (pt.x - rectWordWrite.x) / rectWordWrite.width;
        float ratio_y = (pt.y - rectWordWrite.y) / rectWordWrite.height;
        int x = (int)(ratio_x * widthImage);
        int y = (int)(ratio_y * heightImage);
        jsonitem.x = x.ToString();
        jsonitem.y = y.ToString();
        jsonitem.angle = "0";
        jsonitem.isKey = isKeyPoint;
        listJsonPoint.Add(jsonitem);

        lineRenderer.positionCount = listTouchPoint.Count;

        int idx = 0;
        foreach (Vector3 pos in listTouchPoint)
        {
            lineRenderer.SetPosition(idx, pos);
            idx++;
        }
        int type = GuideItemInfo.IMAGE_TYPE_END;
        if (listTouchPoint.Count == 1)
        {
            type = GuideItemInfo.IMAGE_TYPE_START;
        }
        jsonitem.guide_type = type.ToString();


        AddImageGuide(pt, type);
        if (listTouchPoint.Count >= 3)
        {
            for (int i = 1; i < listTouchPoint.Count - 1; i++)
            {
                EditorItemPoint item = listJsonPoint[i];
                if (item.isKey)
                {
                    type = GuideItemInfo.IMAGE_TYPE_MIDDLE;
                }
                else
                {
                    type = GuideItemInfo.IMAGE_TYPE_MIDDLE_ANIMATE;
                }
                UpdateImageGuide(i, type);

                item.guide_type = type.ToString();
            }
        }

        //angle
        if (listTouchPoint.Count >= 2)
        {
            Vector3 ptStart = listTouchPoint[listTouchPoint.Count - 2];
            Vector3 ptEnd = listTouchPoint[listTouchPoint.Count - 1];
            Vector3 ptimage = ptEnd - ptStart;

            //guide image +90 
            float angle = GetAngleDegreelOfVector(ptimage) + 90;

            UpdateImageGuideAngle(listTouchPoint.Count - 2, angle);

            EditorItemPoint jsonitemlast = listJsonPoint[listJsonPoint.Count - 2];
            jsonitemlast.angle = angle.ToString();
        }


    }

    void CreateLine()
    {
        GameObject obj = new GameObject("point" + "_bihua_" + indexBihua);
        obj.transform.parent = objWordPoint.transform;
        lineRenderer = obj.AddComponent<LineRenderer>();
        listObjBihua.Add(obj);
        ClearLinePoint();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        objBihuaCur = obj;
    }

    void TestLineRender()
    {
        CreateLine();

        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        int idx = 0;
        Vector3 pos;
        lineRenderer.positionCount = 3;

        pos = new Vector3(0f, 0f, -50f);
        lineRenderer.SetPosition(idx, pos);
        idx++;

        pos = new Vector3(5f, 0f, -50f);
        lineRenderer.SetPosition(idx, pos);
        idx++;


        pos = new Vector3(0f, -5f, -50f);
        lineRenderer.SetPosition(idx, pos);
        idx++;
        // wordWriteAjustLine.AddPoint();
        // wordWriteAjustLine.AddPoint(new Vector3(5f, 0f, -50f));
        // wordWriteAjustLine.AddPoint(new Vector3(2.5f, 1f, -50f));


    }
    public Vector2 ImagePoint2World(Vector2 pt)
    {

        Vector2 point = pt;
        //文件读取的点坐标是居于1024x768图片的坐标

        float pic_w = widthImage;
        float pic_h = heightImage;

        float ratio_x = (point.x) * 1.0f / pic_w;//范围0-1f
        float x = ratio_x * rectWordWrite.width;

        //Debug.Log("boundsLetter.center:"+boundsLetter.center+" boundsLetter.size:"+boundsLetter.size);
        //字的左边界坐标
        float letter_x_left = rectWordWrite.center.x - rectWordWrite.width / 2;//(Common.GetCameraWorldSizeWidth(mainCam) * 2 - boundSizeLetter.x) / 2
        x = letter_x_left + x;

        float ratio_y = (point.y) * 1.0f / pic_h;
        float y = ratio_y * rectWordWrite.height;

        //字的底边界坐标
        float letter_y_bottom = rectWordWrite.center.y - rectWordWrite.height / 2;//(mainCam.orthographicSize * 2 - boundSizeLetter.y) / 2;
        y = letter_y_bottom + y;
        return new Vector2(x, y);
    }
    //提示图片
    void DestroyLetterBihuaGuide()
    {
        if (listObjBihuaGuide == null)
        {
            return;
        }
        foreach (GameObject obj in listObjBihuaGuide)
        {
           // iTween.Stop(obj); Dotween
            GameObject.DestroyImmediate(obj);
        }
        listObjBihuaGuide.Clear();

    }
    //return 0-360度
    public float GetAngleDegreelOfVector(Vector2 pt)
    {
        float angle = 0f;
        float len = Mathf.Sqrt(Mathf.Pow(pt.x, 2) + Mathf.Pow(pt.y, 2));
        if (len != 0f)
        {
            //第一 第二象限
            angle = Mathf.Acos(pt.x / len);
        }
        //第三 第四象限
        if (pt.y < 0)
        {
            angle = Mathf.PI * 2 - angle;
        }

        //转化成360度
        angle = angle * 360 / (Mathf.PI * 2);
        return angle;
    }
    string GetImageGuidePic(int type)
    {
        string strPic = "";
        string strDirRoot = Common.GAME_RES_DIR + "/image_common";

        switch (type)
        {
            case GuideItemInfo.IMAGE_TYPE_START:
                strPic = strDirRoot + "/writing_guide_point_start@2x~ipad.png";
                //
                break;
            case GuideItemInfo.IMAGE_TYPE_MIDDLE_ANIMATE:
                strPic = strDirRoot + "/writing_guide_point_animating@2x~ipad.png";
                break;
            case GuideItemInfo.IMAGE_TYPE_MIDDLE:

                strPic = strDirRoot + "/writing_guide_point_middle@2x~ipad.png";
                break;


            case GuideItemInfo.IMAGE_TYPE_END:
                strPic = strDirRoot + "/writing_guide_point_end@2x~ipad.png";

                break;

            default:
                strPic = strDirRoot + "/writing_guide_point_animating@2x~ipad.png";
                break;

        }
        return strPic;
    }

    void UpdateImageGuide(int idx, int type)
    {
        string strPic = GetImageGuidePic(type);
        GameObject obj = listObjBihuaGuide[idx];
        obj.name = "guide_" + idx + "_type_" + type;
        Texture2D tex = LoadTexture.LoadFromAsset(strPic);

        SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();
        objSR.sprite = TextureUtil.CreateSpriteFromTex(tex);
    }

    void UpdateImageGuideAngle(int idx, float angle)
    {
        GameObject obj = listObjBihuaGuide[idx];
        obj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void AddImageGuide(Vector3 pt, int type)
    {
        float x, y, w, h;
        // List<object> listBihua = info.listGuidePoint[indexBihua] as List<object>;
        float z = letterImageZ - 2;
        int idx = 0;

        {
            string strPic = GetImageGuidePic(type);
            Texture2D tex = LoadTexture.LoadFromAsset(strPic);
            w = tex.width / 2;
            h = tex.height / 2;

            guideImageOffsetX = w / 2;
            guideImageOffsetY = h / 2;

            GameObject obj = new GameObject("guide_type_" + type);
            if (objBihuaCur != null)
            {
                obj.transform.parent = objBihuaCur.transform;
            }
            if (listObjBihuaGuide == null)
            {
                listObjBihuaGuide = new List<GameObject>();
            }
            listObjBihuaGuide.Add(obj);
            RectTransform rcTran = obj.AddComponent<RectTransform>();
            SpriteRenderer objSR = obj.AddComponent<SpriteRenderer>();

            w = tex.width;
            h = tex.height;
            x = 0;
            y = 0;

            Sprite sprite = Sprite.Create(tex, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));
            sprite.name = "item" + type;
            objSR.sprite = sprite;
            float scale = Common.ScreenToWorldWidth(mainCam, 64 * AppCommon.scaleBase) / (tex.width / 100f);

            obj.transform.localScale = new Vector3(scale, scale, 1f);
            // obj.transform.rotation = Quaternion.Euler(0, 0, -itemInfo.angle);
            obj.transform.position = new Vector3(pt.x, pt.y, z);
            //obj.transform.position = new Vector3(pt.x+objSR.bounds.size.x/2, pt.y-objSR.bounds.size.y/2, -20f);
            sizeGuideImage = objSR.bounds.size;
            idx++;
        }
    }

    void AddBiHua2Json()
    {
        List<EditorItemPoint> listtmp = new List<EditorItemPoint>();
        foreach (EditorItemPoint item in listJsonPoint)
        {
            listtmp.Add(item);
        }
        listDemoPointAll.Add(listtmp);
        listGuidePointAll.Add(listtmp);

    }
 

    void CreateObjectWordWrite()
    {

        wordWrite = (WordWrite)GameObject.Instantiate(wordWritePrefab);
        wordWrite.gameObject.name = "WordWrite" + indexWordWrite;
        wordWrite.gameObject.transform.parent = objWordWrite.transform;
        wordWrite.mainCamera = mainCam;
        wordWrite.rectDraw = rectWordWrite;
        wordWrite.setDrawLineWidth(drawLineWidth);
        wordWrite.setColor(colorWord);
        indexWordWrite++;
        wordWrite.transform.localPosition = new Vector3(0, 0, 0);
        listObjBihuaWordWrite.Add(wordWrite.gameObject);
    }


    void InitAjustLine()
    {
        wordWriteAjustLine = (WordWrite)GameObject.Instantiate(wordWritePrefab);
        wordWriteAjustLine.gameObject.name = "WordWriteAjustLine";
        //wordWrite.gameObject.transform.parent = objWordWrite.transform;
        wordWriteAjustLine.mainCamera = mainCam;
        wordWriteAjustLine.rectDraw = rectWordWrite;
        wordWriteAjustLine.setDrawLineWidth(0.1f);
        wordWriteAjustLine.setColor(Color.red);
        wordWriteAjustLine.transform.localPosition = new Vector3(0, 0, -50F);

    }

    void ClearAjustLine()
    {
        wordWriteAjustLine.ClearDraw();
    }

    void DrawAjustLine(Vector3 pos)
    {
        if (!enableAjustline)
        {
            return;
        }
        ClearAjustLine();
        wordWriteAjustLine.AddPoint(posTouchDown);
        wordWriteAjustLine.AddPoint(pos);

        //NG Test
        // wordWriteAjustLine.AddPoint(new Vector3(0f, 0f, -50f));
        // wordWriteAjustLine.AddPoint(new Vector3(5f, 0f, -50f));
        // wordWriteAjustLine.AddPoint(new Vector3(2.5f, 1f, -50f));
        wordWriteAjustLine.DrawLine();
    }

    void DestroyObjectWordWrite()
    {
        StopWordWriteDemo();
        indexWordWrite = 0;
        foreach (GameObject obj in listObjBihuaWordWrite)
        {
            GameObject.DestroyImmediate(obj);
        }
        listObjBihuaWordWrite.Clear();

    }
    void StopWordWriteDemo()
    {
        if (IsInvoking("OnTimerDemo"))
        {
            CancelInvoke("OnTimerDemo");
        }
    }
    void OnTimerDemo()
    {
        List<EditorItemPoint> listBihua = listDemoPointAll[indexBihua] as List<EditorItemPoint>;
        if (indexBihuaPoint == 0)
        {
            //一个笔画
            CreateObjectWordWrite();
        }


        //保证画每个笔画的总时间一致
        int one_bihua_draw_count = 5;

        int point_step_min = 1;
        int point_step = listBihua.Count / one_bihua_draw_count;
        if (point_step < point_step_min)
        {
            point_step = point_step_min;
        }
        //foreach (Vector2 point in listBihua)
        for (int i = 0; i < point_step; i++)
        {
            Debug.Log("i=" + i + " listBihua.count=" + listBihua.Count + " indexBihuaPoint=" + indexBihuaPoint);
            Vector2 point = wordBihuaShow.GetBihuaPoint(listBihua, indexBihuaPoint);
            //文件读取的点坐标是居于1024x768图片的坐标
            float z;
            z = letterImageZ;

            Vector2 pt_world = ImagePoint2World(point);

            Vector3 pt = new Vector3(pt_world.x, pt_world.y, z);//mainCam.ScreenToWorldPoint(ptscreen);
            pt.z = z;

            //Debug.Log("AddPoint:" + pt + " ptscreen=" + ptscreen + " point" + point + " screen:" + Screen.width + " " + Screen.height);
            // pt.y -=17;
            // pt.x -=8f;
            wordWrite.AddPoint(pt);
            indexBihuaPoint++;
            if (indexBihuaPoint >= listBihua.Count)
            {
                //下一笔画
                indexBihuaPoint = 0;
                indexBihua++;
                break;
            }
        }

        wordWrite.DrawLine();



        // wordWrite.transform.localScale = new Vector3(scaleLetter,scaleLetter,1f);

        if (indexBihua >= listDemoPointAll.Count)
        {
            indexBihua = 0;
            indexBihuaPoint = 0;
            //整个字写完
            CancelInvoke("OnTimerDemo");
        }

    }

    public void SaveTextWord()
    {
        DestroyLetterBihuaGuide();
        Vector2 size = new Vector2(Screen.width, Screen.height);
        float x, y, w, h;
        w = widthImage;
        h = heightImage;
        x = (Screen.width - w) / 2;
        y = (Screen.height - h) / 2;
        Rect rc = new Rect(x, y, w, h);
        WordItemInfo info = infoWord;
        string filepath = rootDirImage + "/" + info.id + "/" + info.id + ".png";
        Common.CaptureCamera(mainCam, rc, filepath, size);//
    }

    public void OnInputValueChangedWord()
    {

    }

    public void OnInputEndWord()
    {
        textWord.text = inputFieldWord.text;
        UpdateWordInfo(textWord.text);

        OnClickBtnClearAll();
    }
    public void OnClickBtnClearBihua()
    {
        ClearAjustLine();
        enableAjustline = false;
        ClearLinePoint();
    }
    public void OnClickBtnClearAll()
    {
        listDemoPointAll.Clear();
        listGuidePointAll.Clear();

        ClearAjustLine();
        enableAjustline = false;

        indexBihua = 0;
        DestroyLetterBihuaGuide();
        foreach (GameObject obj in listObjBihua)
        {
            GameObject.DestroyImmediate(obj);
        }
        listObjBihua.Clear();
        CreateLine();
    }

    public void OnClickBtnNextBihua()
    {
        ClearAjustLine();
        enableAjustline = false;

        //先更新json
        AddBiHua2Json();
        indexBihua++;
        CreateLine();
        UpdateTitle();
    }

    public void OnClickBtnGuankaJson()
    {
        btnXml2Json.gameObject.SetActive(false);
        wordXmlPoint2Json.OnClickBtnGuankaJson();
    }

    public void OnClickBtnKeyPoint()
    {
        isKeyPoint = !isKeyPoint;
        string str = "NotKey";
        if (isKeyPoint)
        {
            str = "Key";
        }
        Text btnText = Common.GetButtonText(btnKeyPoint);
        btnText.text = str;
    }

    public void OnClickBtnSaveBihua()
    {
        ClearAjustLine();
        enableAjustline = false;

        if (textWord.gameObject.activeSelf)
        {
            //先更新json
            AddBiHua2Json();

            //save  json 
            SaveJson(infoWord);

            //
            SaveTextWord();
        }
        else
        {
            OnClickBtnSaveBihuaShow();
        }

    }

    public void OnClickBtnSaveBihuaShow()
    {
        Vector2 size = new Vector2(Screen.width, Screen.height);
        Rect rc = Common.WorldToScreenRect(mainCam, rectWordWrite);

        Debug.Log("rectWordWrite=:" + rectWordWrite + " rc=" + rc + " size=" + size);
        WordItemInfo info = infoWord;
        string filepath = rootDirImage + "/" + info.id + "/" + info.id + "_stroke.png";
        Common.CaptureCamera(cameraBihuaShow, rc, filepath, size);//
    }


    // 书写动画演示
    public void OnClickBtndDemo()
    {
        ClearAjustLine();
        DestroyObjectWordWrite();
        float t = 0.2f;
        indexBihua = 0;
        indexBihuaPoint = 0;
        enableAjustline = false;
        InvokeRepeating("OnTimerDemo", t, t);
    }

    //笔划示意图
    public void OnClickBtndBihuaShow()
    {

        DestroyObjectWordWrite();
        enableAjustline = true;
        if (!wordBihuaShow.gameObject.activeSelf)
        {
            wordBihuaShow.mainCamera = mainCam;
            wordBihuaShow.rectWordWrite = rectWordWrite;
            wordBihuaShow.UpdateItem(listDemoPointAll);
            enableAjustline = false;
        }
        wordBihuaShow.gameObject.SetActive(!wordBihuaShow.gameObject.activeSelf);
        textWord.gameObject.SetActive(!textWord.gameObject.activeSelf);
        objWordPoint.SetActive(!objWordPoint.activeSelf);
        wordWriteAjustLine.gameObject.SetActive(!wordWriteAjustLine.gameObject.activeSelf);

    }

    void SaveJson(WordItemInfo info)
    {
        int count = listDemoPointAll.Count;
        dataJsonRoot["count"] = count;
        dataJsonRoot["width"] = widthImage;
        dataJsonRoot["height"] = heightImage;
        //save  json 

        /*
          "demo_point": {
                "bihua_0": [
                    {
                        "x": "374",
                        "y": "990"
                    }
                ]
                 }
         */



        //demo_point 
        int idx = 0;
        foreach (List<EditorItemPoint> listBihua in listDemoPointAll)
        {
            string key = "bihua_" + idx;
            if (!dataJsonDemopoint.Contains(key))
            {
                List<JsonItemPoint> listtmp = new List<JsonItemPoint>();
                foreach (EditorItemPoint item in listBihua)
                {
                    JsonItemPoint jsonitem = new JsonItemPoint();
                    jsonitem.x = item.x;
                    jsonitem.y = item.y;
                    listtmp.Add(jsonitem);
                }
                dataJsonDemopoint[key] = listtmp;
            }
            idx++;
        }


        /* 
             "guide_point": {
                "bihua_0": [
                    {
                        "angle": "-33",
                        "x": "369",
                        "y": "982",
                        "type": "0",
                        "direction": "0"
                    }
                ]
                }
                */

        //guide_point


        idx = 0;
        foreach (List<EditorItemPoint> listBihua in listGuidePointAll)
        {
            string key = "bihua_" + idx;
            if (!dataJsonGuidepoint.Contains(key))
            {
                List<JsonGuideItemInfo> listtmp = new List<JsonGuideItemInfo>();
                foreach (EditorItemPoint item in listBihua)
                {
                    JsonGuideItemInfo jsonitem = new JsonGuideItemInfo();
                    jsonitem.x = item.x;
                    jsonitem.y = item.y;
                    jsonitem.angle = item.angle;
                    jsonitem.type = item.guide_type;
                    jsonitem.direction = WordWrite.DIRECTION_BISHUN_DOWN.ToString();
                    listtmp.Add(jsonitem);
                }
                dataJsonGuidepoint[key] = listtmp;
            }
            idx++;
        }


        string strJson = JsonMapper.ToJson(dataJsonRoot);

        string filepath = rootDirImage + "/" + info.id + "/" + info.id + ".json";

        string dir = FileUtil.GetFileDir(filepath);
        FileUtil.CreateDir(dir);
        byte[] bytes = Encoding.UTF8.GetBytes(strJson);
        System.IO.File.WriteAllBytes(filepath, bytes);
    }



}
