using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public interface IGameXieHanziDelegate
{
    void OnGameXieHanziDidUpdateMode(GameXieHanzi game, WordWriteMode mode);

}

public enum WordWriteMode
{
    None,
    WriteDemo,//演示自动书写
    WriteWithOneWord,//显示整个字形
    WriteWithOneBihua,//显示一个笔画
    WriteWithNone,//不显示字和笔画
    ShowBihua
}
public class ColorItemInfo : ItemInfo
{
    public List<Color> listColor;
    public string name;
    public string picmask;
    public string colorJson;
    public Vector2 pt;
    public Color colorOrigin;//填充前原来颜色
    public Color colorFill;//当前填充颜色
    public Color colorMask;
    public Color32 color32Fill;
    public string fileSave;
    public string fileSaveWord;
    public string addtime;
    public string date;
    public Rect rectFill;
}
public class GameXieHanzi : UIView
{

    public const int GAME_MODE_NORMAL = 0;
    public const int GAME_MODE_FREE_WRITE = 1;
    public const float FADE_ANIMATE_TIME = 1.0f;
    public GameObject objSpriteWordWriteBg;
    PaintLine paintLinePrefab;
    PaintLine paintLine;

    public IGameXieHanziDelegate iDelegate;
    public WordItemInfo infoWord;
    public Color colorWord;
    WordWrite2 wordWritePrefab;
    WordWrite2 wordWrite;
    int indexWordWrite = 0;
    WordWriteMode writeModeCur;
    WordWriteMode writeModePre;//
    GameObject objLetter;
    GameObject objLetterBihua;

    public float letterImageZ = -20f;
    UITouchEventWithMove uiTouchEvent;
    BoxCollider boxCollider;



    public WordItemInfo infoFreeWrite;
    bool isTouchDownForWrite;
    int gameMode;

    Rect rectWordWrite;//local world rect

    List<GameObject> listObjBihua;
    List<GameObject> listObjBihuaGuide;
    int indexBihua;
    int indexBihuaPoint;

    float widthImage;
    float heightImage;
    float drawLineWidth = 0.5f;

    float guideImageOffsetX;
    float guideImageOffsetY;
    Vector2 sizeGuideImage;
    float scaleLetter;
    Vector2 texSizeLetter;
    Bounds boundsLetter;
    bool isOnTimerWriteFail = false;

    bool isShowBihuaImage = false;

    Rect _imageRectOfLetter = Rect.zero;
    Rect imageRectOfLetter //tex坐标
    {
        get
        {
            if (_imageRectOfLetter.Equals(Rect.zero))
            {
                SpriteRenderer objSR = objLetter.GetComponent<SpriteRenderer>();
                Texture2D tex = objSR.sprite.texture;
                //_imageRectOfLetter = TextureUtil.GetRectNotAlpha(tex);
                _imageRectOfLetter = new Rect(0, 0, tex.width, tex.height);

            }
            return _imageRectOfLetter;
        }
    }

    public bool isHasPaint
    {
        get
        {
            if (!paintLine)
            {
                return false;
            }
            return paintLine.isHasPaint;
        }
    }

    public bool isHasSave
    {
        get
        {
            if (!paintLine)
            {
                return false;
            }
            return paintLine.isHasSave;
        }
    }
    void Awake()
    {
        uiTouchEvent = this.gameObject.AddComponent<UITouchEventWithMove>();
        uiTouchEvent.callBackTouch = OnUITouchEvent;
        boxCollider = this.gameObject.AddComponent<BoxCollider>();
        LoadPrefab();


    }
    // Use this for initialization
    void Start()
    {
        paintLine.gameObject.transform.localPosition = new Vector3(0, 0, letterImageZ - 4);
        paintLine.gameObject.SetActive(false);
        if (gameMode == GAME_MODE_FREE_WRITE)
        {
            paintLine.gameObject.SetActive(true);
        }
        paintLine.UpdateColor(colorWord);
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitValue()
    {
        gameMode = GameManager.gameMode;
        listObjBihua = new List<GameObject>();
        isShowBihuaImage = false;
        isOnTimerWriteFail = false;
    }

    void LoadPrefab()
    {
        {
            GameObject obj = (GameObject)Resources.Load("App/Prefab/Game/PaintLine");
            if (obj != null)
            {
                paintLinePrefab = obj.GetComponent<PaintLine>();
                paintLine = (PaintLine)GameObject.Instantiate(paintLinePrefab);
                paintLine.gameObject.transform.SetParent(this.transform);
                UIViewController.ClonePrefabRectTransform(paintLinePrefab.gameObject, paintLine.gameObject);
            }
        }

        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Game/WordWrite2");
            wordWritePrefab = obj.GetComponent<WordWrite2>();
        }

    }

    public override void LayOut()
    {
        float x, y, w, h, z;
        float scale = 0; float scalex = 0; float scaley = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        Vector2 worldSize = Common.GetWorldSize(mainCam);
        boxCollider.size = worldSize;

        //objLetter
        if (objLetter != null)
        {
            SpriteRenderer rd = objLetter.GetComponent<SpriteRenderer>();
            if ((rd.sprite != null) && (rd.sprite.texture.width != 0))
            {
                scale = Common.GetBestFitScale(rd.sprite.texture.width / 100f, rd.sprite.texture.height / 100f, rectWordWrite.width, rectWordWrite.height);
                objLetter.transform.localScale = new Vector3(scale, scale, 1f);
                objLetter.transform.localPosition = new Vector3(rectWordWrite.center.x, rectWordWrite.center.y, letterImageZ);
            }
        }

        {
            SpriteRenderer rd = objSpriteWordWriteBg.GetComponent<SpriteRenderer>();
            scale = Common.GetBestFitScale(rd.sprite.texture.width / 100f, rd.sprite.texture.height / 100f, rectWordWrite.width, rectWordWrite.height);
            objSpriteWordWriteBg.transform.localScale = new Vector3(scale, scale, 1f);
            objSpriteWordWriteBg.transform.localPosition = new Vector3(rectWordWrite.center.x, rectWordWrite.center.y, -1f);

        }
    }

    public void UpdateColor(Color cr)
    {
        colorWord = cr;
        if (paintLine != null)
        {
            paintLine.UpdateColor(colorWord);
        }

    }
    Rect GetImageRect()
    {
        float x, y, w, h;
        float pt_image_left = widthImage;//BIHUA_POINT_PIC_BASE_WIDTH;
        float pt_image_right = 0;
        float pt_image_top = heightImage;//BIHUA_POINT_PIC_BASE_HEIGHT;
        float pt_image_bottom = 0;

        WordItemInfo info = infoWord;

        foreach (List<object> listPoint in info.listDemoPoint)
        {

            foreach (Vector2 pt in listPoint)
            {
                if (pt.x < pt_image_left)
                {
                    pt_image_left = pt.x;
                }
                if (pt.x > pt_image_right)
                {
                    pt_image_right = pt.x;
                }

                if (pt.y < pt_image_top)
                {
                    pt_image_top = pt.y;
                }

                if (pt.y > pt_image_bottom)
                {

                    pt_image_bottom = pt.y;
                }

            }

        }

        x = pt_image_left;
        w = pt_image_right - pt_image_left;
        y = heightImage - pt_image_bottom;//BIHUA_POINT_PIC_BASE_HEIGHT
        h = pt_image_bottom - pt_image_top;

        return new Rect(x, y, w, h);
    }


    // {621.14, 146.24} to Vector2
    public Vector2 WordString2Point(string str)
    {
        float x, y;
        string strtmp = str.Substring(str.IndexOf("{") + 1);
        int idx = strtmp.IndexOf(",");
        string strx = strtmp.Substring(0, idx);
        float.TryParse(strx, out x);

        string strtmpy = strtmp.Substring(idx + 1);
        idx = strtmpy.IndexOf("}");
        string stry = strtmpy.Substring(0, idx);
        float.TryParse(stry, out y);

        return new Vector2(x, y);
    }
    public void SetLineWidthPixsel(int w)
    {
        if (paintLine != null)
        {
            paintLine.SetLineWidthPixsel(w);
        }
    }
    public void UpdateRect(Rect rc)
    {
        rectWordWrite = rc;
        if (paintLine != null)
        {
            paintLine.UpdateRect(rc);
        }
        LayOut();

    }
    void DestroyObjectWordWrite()
    {
        indexWordWrite = 0;
        foreach (GameObject obj in listObjBihua)
        {
            GameObject.DestroyImmediate(obj);
        }
        listObjBihua.Clear();

    }
    void CreateObjectWordWrite()
    {

        wordWrite = (WordWrite2)GameObject.Instantiate(wordWritePrefab);
        wordWrite.gameObject.name = "WordWrite" + indexWordWrite;
        //AppSceneBase.main.AddObjToMainWorld(wordWrite.gameObject);
        wordWrite.gameObject.transform.SetParent(this.transform);
        wordWrite.mainCamera = mainCam;
        wordWrite.rectDraw = rectWordWrite;
        wordWrite.setDrawLineWidth(drawLineWidth);
        wordWrite.setColor(colorWord);
        indexWordWrite++;
        wordWrite.transform.localPosition = new Vector3(0, 0, letterImageZ - 1);
        listObjBihua.Add(wordWrite.gameObject);
    }



    void GotoNextWordWriteMode()
    {
        if (writeModeCur == WordWriteMode.WriteWithOneWord)
        {
            GotoWordWriteMode(WordWriteMode.WriteWithOneBihua);
        }
        else if (writeModeCur == WordWriteMode.WriteWithOneBihua)
        {
            GotoWordWriteMode(WordWriteMode.WriteWithNone);
        }
        else if (writeModeCur == WordWriteMode.WriteWithNone)
        {
            //练字完成 准备写下一个汉字
            ShowGameWin();
        }
    }
    void CheckBihuaWrite()
    {
        WordItemInfo info = infoWord;
        if (info == null)
        {
            return;
        }
        if (wordWrite == null)
        {
            return;
        }

        List<object> listBihua = info.listGuidePoint[indexBihua] as List<object>;
        GuideItemInfo infoStart = listBihua[0] as GuideItemInfo;
        GuideItemInfo infoEnd = listBihua[listBihua.Count - 1] as GuideItemInfo;

        bool isWriteFinish = false;
        bool isWriteStart = false;
        bool isWriteEnd = false;
        foreach (Vector3 pt in wordWrite.listPoint)
        {

            {
                Vector2 point = new Vector2(0, 0);
                point.x = infoStart.point.x + guideImageOffsetX;
                point.y = infoStart.point.y + guideImageOffsetY;
                Vector2 pt_world = ImagePoint2LocalWorld(point);
                float oft = Common.ScreenToWorldWidth(mainCam, 60 * AppCommon.scaleBase);
                float w = sizeGuideImage.x + oft;
                float h = sizeGuideImage.y + oft;
                Rect rc = new Rect(pt_world.x - w / 2, pt_world.y - h / 2, w, h);
                Vector2 ptwrite = new Vector2(pt.x, pt.y);
                if (rc.Contains(ptwrite))
                {
                    isWriteStart = true;
                }
            }

            {
                Vector2 point = new Vector2(0, 0);
                point.x = infoEnd.point.x + guideImageOffsetX;
                point.y = infoEnd.point.y + guideImageOffsetY;
                Vector2 pt_world = ImagePoint2World(point);
                float oft = Common.ScreenToWorldWidth(mainCam, 60 * AppCommon.scaleBase);
                float w = sizeGuideImage.x + oft;
                float h = sizeGuideImage.y + oft;
                Rect rc = new Rect(pt_world.x - w / 2, pt_world.y - h / 2, w, h);
                Vector2 ptwrite = new Vector2(pt.x, pt.y);
                if (rc.Contains(ptwrite))
                {
                    isWriteEnd = true;
                }
            }
        }

        if (isWriteStart && isWriteEnd)
        {
            isWriteFinish = true;

        }

        if (isWriteFinish)
        {
            //书写正确 
            indexBihua++;
            Debug.Log("WriteFinish:indexBihua=" + indexBihua + " total=" + info.listImageBihua0.Count);
            if (indexBihua >= info.listImageBihua0.Count)
            {
                //所有笔画写完
                indexBihua = 0;
                Debug.Log("next mode");
                GotoNextWordWriteMode();
            }
            else
            {
                //写下一笔
                Debug.Log("next bihua");
                UpdateLetterBihuaGuide();
            }

        }
        else
        {
            //书写失败
            float t = 0.15f;
            wordWrite.totalPoint = wordWrite.listPoint.Count;
            InvokeRepeating("OnTimerWriteFail", t, t);
            isOnTimerWriteFail = true;
        }
    }



    void DestroyLetterBihuaGuide()
    {
        if (listObjBihuaGuide == null)
        {
            return;
        }

        StopCoroutine("ShowGuideImageFadeAnimate");

        foreach (GameObject obj in listObjBihuaGuide)
        {
            iTween.Stop(obj);
            GameObject.DestroyImmediate(obj);
        }
        listObjBihuaGuide.Clear();

    }
    //提示图片
    void UpdateLetterBihuaGuide()
    {
        if (gameMode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            return;
        }
        float x, y, w, h;
        WordItemInfo info = infoWord;
        if (info == null)
        {
            return;
        }
        DestroyLetterBihuaGuide();
        List<object> listBihua = info.listGuidePoint[indexBihua] as List<object>;
        float z = letterImageZ - 2;
        int idx = 0;
        int animate_count = 0;
        foreach (GuideItemInfo itemInfo in listBihua)
        {
            string strPic = "";
            string strDirRoot = Common.GAME_RES_DIR + "/image_common";
            switch (itemInfo.type)
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
                    z--;
                    //writing_guide_point_end@2x~ipad
                    break;

                default:
                    strPic = strDirRoot + "/writing_guide_point_animating@2x~ipad.png";
                    break;

            }

            Debug.Log("guide image type:" + itemInfo.type);
            //Texture2D tex = (Texture2D)Resources.Load(strPic);
            Texture2D tex = TextureCache.main.Load(strPic);
            w = tex.width / 2;
            h = tex.height / 2;
            // if(itemInfo.type==GuideItemInfo.IMAGE_TYPE_MIDDLE)
            // {
            //     //偏移量按writing_guide_point_middle 大小计算
            //     w = 90/2;
            //     h = 90/2;
            // }
            //   listTexTure.Add(tex);
            guideImageOffsetX = w / 2;
            guideImageOffsetY = h / 2;
            Vector2 point = new Vector2(0, 0);
            point.x = itemInfo.point.x + guideImageOffsetX;
            point.y = itemInfo.point.y + guideImageOffsetY;
            Vector2 pt_world = ImagePoint2World(point);
            Vector3 pt = this.transform.InverseTransformPoint(pt_world);
            pt.z = z;

            if (!Device.isLandscape)
            {

            }
            GameObject obj = new GameObject("guide" + idx);
            //AppSceneBase.main.AddObjToMainWorld(obj);
            obj.transform.SetParent(this.transform);
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
            sprite.name = "item";
            objSR.sprite = sprite;
            float scale = objLetter.transform.localScale.x;//0.8f;
            if (!Device.isLandscape)
            {
                //scale = scale*1536/2048;
            }
            obj.transform.localScale = new Vector3(scale, scale, 1f);
            obj.transform.rotation = Quaternion.Euler(0, 0, itemInfo.angle);

            obj.transform.localPosition = new Vector3(pt.x, pt.y, z);

            Debug.Log("guideimage idx=" + idx + " pt=" + pt + " scale=" + scale);

            //obj.transform.position = new Vector3(pt.x+objSR.bounds.size.x/2, pt.y-objSR.bounds.size.y/2, -20f);
            sizeGuideImage = objSR.bounds.size;


            if (itemInfo.type == GuideItemInfo.IMAGE_TYPE_MIDDLE_ANIMATE)
            {
                obj.SetActive(false);
                //Invoke("ShowGuideImageFadeAnimate",FADE_ANIMATE_TIME*animate_count);
                //StartCoroutine(ShowGuideImageFadeAnimate(obj,FADE_ANIMATE_TIME*animate_count));
                ItemInfo info_coroutine = new ItemInfo();
                info_coroutine.obj = obj;
                info_coroutine.time = FADE_ANIMATE_TIME * animate_count;
                StartCoroutine("ShowGuideImageFadeAnimate", info_coroutine);
                animate_count++;

            }

            idx++;
        }
    }

    //如果想要传递参数，并且实现延迟调用，可以考虑采用Coroutine
    IEnumerator ShowGuideImageFadeAnimate(ItemInfo info)

    {
        float delaySeconds = info.time;
        yield return new WaitForSeconds(delaySeconds);
        if ((listObjBihuaGuide != null) && (listObjBihuaGuide.Count > 0))
        {
            GameObject obj = info.obj;
            if (obj != null)
            {
                obj.SetActive(true);
                //淡入淡出动画
                iTween.FadeTo(obj, iTween.Hash("alpha", 0, "time", FADE_ANIMATE_TIME, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong));
            }

        }

    }

    void ShowLetterBihuaImage(bool isShow)
    {

        isShowBihuaImage = isShow;

        float x, y, w, h, z;

        WordItemInfo info = infoWord;
        if (info == null)
        {
            return;
        }

        if (objLetterBihua == null)
        {
            objLetterBihua = new GameObject("letterBihua");
            objLetterBihua.AddComponent<RectTransform>();
            objLetterBihua.AddComponent<SpriteRenderer>();
        }
        RectTransform rcTran = objLetterBihua.GetComponent<RectTransform>();
        SpriteRenderer objSR = objLetterBihua.GetComponent<SpriteRenderer>();
        string strPic = info.imageBihua;
        objLetterBihua.SetActive(isShow);

        if (isShow)
        {
            //Texture2D tex = (Texture2D)Resources.Load(strPic);
            Texture2D tex = LoadTexture.LoadFromAsset(strPic);
            // listTexTure.Add(tex);

            w = tex.width;
            h = tex.height;
            //  w  = tex.width;
            // h =  tex.height;
            Debug.Log("tex,w:" + w + " h:" + h);
            x = 0;
            y = 0;

            Sprite sprite = Sprite.Create(tex, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));
            sprite.name = "item";
            objSR.sprite = sprite;


            Vector2 spsize = new Vector2(tex.width / 100f, tex.height / 100f);
            Vector2 world_size = rectWordWrite.size;
            float scalex = world_size.x / spsize.x;
            float scaley = world_size.y / spsize.y;
            float scale = Mathf.Min(scalex, scaley);
            objLetterBihua.transform.localScale = new Vector3(scale, scale, 1f);

            x = rectWordWrite.position.x + rectWordWrite.size.x / 2;
            y = rectWordWrite.position.y + rectWordWrite.size.y / 2;
            z = letterImageZ - 10f;

            objLetterBihua.transform.position = new Vector3(x, y, z);
        }

    }
    void UpdateLetterImage()
    {
        int x, y, w, h;
        if (gameMode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            return;
        }
        WordItemInfo info = infoWord;
        if (info == null)
        {
            return;
        }

        if (objLetter == null)
        {
            objLetter = new GameObject("letter");
            //AppSceneBase.main.AddObjToMainWorld(objLetter);
            objLetter.transform.SetParent(this.transform);
            objLetter.AddComponent<RectTransform>();
            objLetter.AddComponent<SpriteRenderer>();
        }
        RectTransform rcTran = objLetter.GetComponent<RectTransform>();
        SpriteRenderer objSR = objLetter.GetComponent<SpriteRenderer>();
        string strPic = info.pic;
        objLetter.SetActive(true);
        if (gameMode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            objLetter.SetActive(false);
        }

        switch (writeModeCur)
        {
            case WordWriteMode.WriteWithOneWord:
                {
                    strPic = info.pic;
                }
                break;
            case WordWriteMode.WriteWithOneBihua:
                {
                    strPic = info.listImageBihua0[indexBihua];
                }
                break;
            case WordWriteMode.WriteWithNone:
                {
                    objLetter.SetActive(false);
                    //  return;
                }

                break;


        }
        Debug.Log(strPic);
        //Texture2D tex = (Texture2D)Resources.Load(strPic);
        Texture2D tex = TextureCache.main.Load(strPic);
        if (writeModeCur == WordWriteMode.WriteWithOneWord)
        {
            widthImage = tex.width;
            heightImage = tex.height;

        }
        if (writeModeCur == WordWriteMode.WriteWithOneBihua)
        {
            if (tex == null)
            {
                strPic = info.listImageBihua1[indexBihua];
                Debug.Log(strPic);
                tex = TextureCache.main.Load(strPic);
            }
        }
        // listTexTure.Add(tex);

        w = tex.width;
        h = tex.height;
        //  w  = tex.width;
        // h =  tex.height;
        Debug.Log("tex,w:" + w + " h:" + h);
        x = 0;
        y = 0;
        texSizeLetter = new Vector2(w, h);
        Sprite sprite = Sprite.Create(tex, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));
        sprite.name = "item";
        objSR.sprite = sprite;

        LayOut();

    }

    public void GotoWordWriteMode(WordWriteMode mode)
    {
        writeModeCur = mode;

        WordItemInfo info = infoWord;
        if (info == null)
        {
            return;
        }
        switch (mode)
        {
            case WordWriteMode.WriteDemo:
                {
                    // ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    // ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    //  ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                    StopWordWriteDemo();
                    DestroyObjectWordWrite();
                    DestroyLetterBihuaGuide();
                    if (isShowBihuaImage)
                    {
                        ShowLetterBihuaImage(false);
                    }
                    float t = 0.2f;
                    indexBihua = 0;
                    indexBihuaPoint = 0;
                    InvokeRepeating("OnTimerDemo", t, t);

                }
                break;

            case WordWriteMode.WriteWithOneWord:
                {
                    //  ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_SEL);
                    //   ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    //  ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                    indexBihua = 0;
                    indexBihuaPoint = 0;
                    DestroyObjectWordWrite();
                    StopWordWriteDemo();
                    UpdateLetterImage();
                    UpdateLetterBihuaGuide();
                    if (isShowBihuaImage)
                    {
                        ShowLetterBihuaImage(false);
                    }
                }
                break;
            case WordWriteMode.WriteWithOneBihua:
                {
                    //  ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    //  ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_SEL);
                    //  ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                    indexBihua = 0;
                    indexBihuaPoint = 0;
                    DestroyObjectWordWrite();
                    StopWordWriteDemo();
                    UpdateLetterImage();
                    UpdateLetterBihuaGuide();
                    if (isShowBihuaImage)
                    {
                        ShowLetterBihuaImage(false);
                    }
                }
                break;
            case WordWriteMode.WriteWithNone:
                {
                    //  ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    //   ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    //   ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_SEL);
                    indexBihua = 0;
                    indexBihuaPoint = 0;
                    DestroyObjectWordWrite();
                    StopWordWriteDemo();
                    UpdateLetterImage();
                    UpdateLetterBihuaGuide();
                    if (isShowBihuaImage)
                    {
                        ShowLetterBihuaImage(false);
                    }
                }
                break;

            case WordWriteMode.ShowBihua:
                {
                    //   ButtonSetImage(uiToolBar.btnModeAll, AppXieHanzi.RES_WORDWRITE_MODE_ALL_UNSEL);
                    //   ButtonSetImage(uiToolBar.btnModeOne, AppXieHanzi.RES_WORDWRITE_MODE_ONE_UNSEL);
                    //   ButtonSetImage(uiToolBar.btnModeNone, AppXieHanzi.RES_WORDWRITE_MODE_NONE_UNSEL);
                    //DestroyObjectWordWrite();
                    //StopWordWriteDemo();
                    //UpdateLetterImage();
                    //UpdateLetterBihuaGuide();
                    ShowLetterBihuaImage(!isShowBihuaImage);

                }
                break;

            default:
                break;


        }


        LayOut();

        if (iDelegate != null)
        {
            iDelegate.OnGameXieHanziDidUpdateMode(this, mode);
        }
    }

    void StopWordWriteDemo()
    {
        if (IsInvoking("OnTimerDemo"))
        {
            CancelInvoke("OnTimerDemo");
        }
    }
    Vector2 ImagePoint2LocalWorld(Vector2 pt)
    {
        return this.transform.InverseTransformPoint(ImagePoint2World(pt));
    }
    Vector2 ImagePoint2World(Vector2 pt)
    {
        if (objLetter != null)
        {
            boundsLetter = objLetter.GetComponent<SpriteRenderer>().bounds;
        }
        Vector2 point = pt;
        //文件读取的点坐标是居于1024x768图片的坐标

        float pic_w = widthImage;
        float pic_h = heightImage;

        float ratio_x = (point.x) * 1.0f / pic_w;//范围0-1f
        float x = ratio_x * boundsLetter.size.x;

        //Debug.Log("boundsLetter.center:"+boundsLetter.center+" boundsLetter.size:"+boundsLetter.size);
        //字的左边界坐标
        float letter_x_left = boundsLetter.center.x - boundsLetter.size.x / 2;//(Common.GetCameraWorldSizeWidth(mainCam) * 2 - boundSizeLetter.x) / 2
        x = letter_x_left + x;

        float ratio_y = (pic_h - point.y) * 1.0f / pic_h;//范围0-1f
        //if (imageScaleFactor == 1f)
        {
            //坐标原点在底部
            ratio_y = (point.y) * 1.0f / pic_h;
        }

        float y = ratio_y * boundsLetter.size.y;

        //字的底边界坐标
        float letter_y_bottom = boundsLetter.center.y - boundsLetter.size.y / 2;//(mainCam.orthographicSize * 2 - boundSizeLetter.y) / 2;
        y = letter_y_bottom + y;
        return new Vector2(x, y);
    }

    void OnTimerWriteFail()
    {
        if (wordWrite)
        {
            wordWrite.OnDrawFail();
            if (wordWrite.listPoint.Count == 0)
            {
                CancelInvoke("OnTimerWriteFail");
                isOnTimerWriteFail = false;
                if (wordWrite != null)
                {
                    Destroy(wordWrite.gameObject);
                    indexWordWrite--;
                    if (indexWordWrite < 0)
                    {
                        indexWordWrite = 0;
                    }
                }

            }
        }
    }

    void OnTimerDemo()
    {

        WordItemInfo info = infoWord;
        if (info == null)
        {
            return;
        }


        List<object> listBihua = info.listDemoPoint[indexBihua] as List<object>;
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
            Vector2 point = (Vector2)listBihua[indexBihuaPoint];
            //文件读取的点坐标是居于1024x768图片的坐标
            float z;
            z = letterImageZ;

            Vector2 pt_world = ImagePoint2World(point);
            Vector3 pt = this.transform.InverseTransformPoint(pt_world);
            pt.z = 0;

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

        if (indexBihua >= info.listDemoPoint.Count)
        {
            indexBihua = 0;
            indexBihuaPoint = 0;
            //整个字写完
            CancelInvoke("OnTimerDemo");
        }

    }

    public void SaveImage(string filePath)
    {
        if (paintLine != null)
        {
            paintLine.SaveImage(filePath);
        }
    }

    public void ClearAll()
    {
        if (paintLine != null)
        {
            paintLine.ClearAll();
        }
    }

    void CheckGameWin()
    {
        bool isAllItemLock = true;
        Debug.Log("CheckGameWin");
    }

    void ShowGameWin()
    {
        Debug.Log("ShowGameWin");
        WordItemInfo info = infoWord;
        if (info == null)
        {
            return;
        }
        /* 
        //GameScene.ShowAdInsert(100);


        //remove guide image
        DestroyLetterBihuaGuide();

        Debug.Log("ShowGameWin 1");
        CaptureWordWrite();
        Debug.Log("ShowGameWin 2");
        DBWord.main.AddItem(info);

        WordWriteFinishViewController controller = WordWriteFinishViewController.main;
        controller.Show(null, null);
        controller.ui.callbackClose = OnUIWordWriteFinish;
        controller.ui.gameLevelNow = GameManager.gameLevel;
        controller.ui.UpdateItem(GetItemInfo());
        Debug.Log("ShowGameWin end");

        */
    }
    void onTouchDown()
    {
        // Debug.Log("onTouchDown gameMode=" + gameMode + " writeModeCur=" + writeModeCur);
        isTouchDownForWrite = false;
        if (gameMode == GAME_MODE_NORMAL)
        {
            if ((writeModeCur == WordWriteMode.WriteWithNone) || (writeModeCur == WordWriteMode.WriteWithOneWord) || (writeModeCur == WordWriteMode.WriteWithOneBihua))
            {
                if (!isOnTimerWriteFail)
                {
                    Vector2 posword = Common.GetInputPositionWorld(mainCam);
                    Vector2 ptlocal = this.transform.InverseTransformPoint(posword);
                    // Debug.Log("onTouchDown ptlocal=" + ptlocal + " rectWordWrite=" + rectWordWrite);
                    if (!rectWordWrite.Contains(ptlocal))
                    {
                        return;
                    }
                    isTouchDownForWrite = true;
                    CreateObjectWordWrite();
                    wordWrite.ClearDraw();
                }

            }

        }

    }
    void onTouchMove()
    {
        //  Debug.Log("onTouchMove");
        if (!isTouchDownForWrite)
        {
            return;
        }
        if (gameMode == GAME_MODE_NORMAL)
        {
            wordWrite.OnDraw();
        }

    }
    void onTouchUp()
    {
        if (!isTouchDownForWrite)
        {
            return;
        }
        isTouchDownForWrite = false;
        if (gameMode == GAME_MODE_NORMAL)
        {
            CheckBihuaWrite();
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
}
