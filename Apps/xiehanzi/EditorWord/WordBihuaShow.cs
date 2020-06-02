using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WordBihuaShow : ScriptBase
{

    public Rect rectWordWrite;
    public EditorWordPoint editorPoint;

    WordWrite wordWrite;
    public WordWrite wordWritePrefab;
    int indexWordWrite;
    List<GameObject> listObjBihuaWordWrite;
    List<GameObject> listObjBihuaTips;//数字和箭头
    Color colorWord = new Color(32 / 255f, 173 / 255f, 222 / 255f, 1f);//32,173,222
    float drawLineWidth = 0.1f;
    float letterImageZ = 0f;
    int indexBihua;
    int indexBihuaPoint;
    List<object> listDemoPoint;
    GameObject objNum;
    GameObject objArrow;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listObjBihuaWordWrite = new List<GameObject>();
        listObjBihuaTips = new List<GameObject>();

    }
    public void UpdateItem(List<object> demoPoint)
    {
        listDemoPoint = demoPoint;
        indexBihua = 0;
        indexBihuaPoint = 0;
        DestroyObjects();
        DrawBihua();
    }

    void SetTextMeshFontSize(GameObject obj, int fontSize)
    {
        TextMesh textMesh = obj.GetComponent<TextMesh>();
        if (textMesh != null)
        {
            int old = textMesh.fontSize;
            float scale = fontSize * 1f / old;
            obj.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
    //
    GameObject CreateBihuaNumObject(int idx, Vector2 pos)
    {
        GameObject obj = new GameObject("bihua_" + (idx + 1));
        obj.layer = this.gameObject.layer;
        obj.transform.parent = this.transform;
        SpriteRenderer spRd = obj.AddComponent<SpriteRenderer>();
        spRd.sprite = TextureUtil.CreateSpriteFromResource("UI/EditorPoint/BihuaNumBg");
        obj.transform.localPosition = new Vector3(pos.x, pos.y, letterImageZ - 1);
        float scale = Common.ScreenToWorldWidth(mainCamera, 64 * AppCommon.scaleBase) / (spRd.sprite.texture.width / 100f);
        obj.transform.localScale = new Vector3(scale, scale, 1f);
        {
            /* 
			@moon 3D Text 字体显示模糊问题 ：
			将frontsize 调大点，如果字体大小不合适，调整scale缩放
			http://tieba.baidu.com/p/3023091682
			*/
            GameObject objTitle = new GameObject("Title");
            objTitle.layer = this.gameObject.layer;
            objTitle.transform.parent = obj.transform;
            TextMesh textMesh = objTitle.AddComponent<TextMesh>();
            textMesh.text = (idx + 1).ToString();
            textMesh.fontSize = 128;
            textMesh.alignment = TextAlignment.Center;
            //textMesh.lineSpacing = 2;
            textMesh.anchor = TextAnchor.MiddleCenter;
            SetTextMeshFontSize(objTitle, 6);

            objTitle.transform.localPosition = new Vector3(0, 0, 0);

        }
        return obj;
    }


    GameObject CreateBihuaArrowObject(int idx, Vector2 pos, float angelZ)
    {
        GameObject obj = new GameObject("arrow_" + (idx + 1));
        obj.layer = this.gameObject.layer;
        obj.transform.parent = this.transform;
        SpriteRenderer spRd = obj.AddComponent<SpriteRenderer>();
        spRd.sprite = TextureUtil.CreateSpriteFromResource("UI/EditorPoint/BihuaArrow");
        obj.transform.localPosition = new Vector3(pos.x, pos.y, letterImageZ - 1);


        float scale = Common.ScreenToWorldWidth(mainCamera, 64 * AppCommon.scaleBase) / (spRd.sprite.texture.width / 100f);

        obj.transform.localScale = new Vector3(scale, scale, 1f);
        obj.transform.rotation = Quaternion.Euler(0, 0, angelZ);

        return obj;
    }

    void CreateObjectWordWrite()
    {

        wordWrite = (WordWrite)GameObject.Instantiate(wordWritePrefab);
        wordWrite.gameObject.name = "WordWrite" + indexWordWrite;
        wordWrite.gameObject.transform.parent = this.transform;
        wordWrite.gameObject.layer = this.gameObject.layer;
        wordWrite.mainCamera = mainCamera;
        wordWrite.rectDraw = rectWordWrite;
        wordWrite.setDrawLineWidth(drawLineWidth);
        wordWrite.setColor(colorWord);
        indexWordWrite++;
        wordWrite.transform.localPosition = new Vector3(0, 0, 0);
        listObjBihuaWordWrite.Add(wordWrite.gameObject);
    }

    void DestroyObjects()
    {
        indexWordWrite = 0;
        foreach (GameObject obj in listObjBihuaWordWrite)
        {
            GameObject.DestroyImmediate(obj);
        }
        listObjBihuaWordWrite.Clear();


        foreach (GameObject obj in listObjBihuaTips)
        {
            GameObject.DestroyImmediate(obj);
        }
        listObjBihuaTips.Clear();
    }


    bool IsObjInclude(GameObject objNow)
    {
        bool ret = false;
        Renderer rdNow = objNow.GetComponent<Renderer>();

        Rect rc1 = Common.GetRectOfBounds(rdNow.bounds);
        foreach (GameObject obj in listObjBihuaTips)
        {
            Renderer rd = obj.GetComponent<Renderer>();
            Rect rc2 = Common.GetRectOfBounds(rd.bounds);
            ret = IsTowRectInclude(rc1, rc2);
            if (ret)
            {
                break;
            }
        }
        return ret;
    }

    //判断两个rect区域是否重叠
    bool IsTowRectInclude(Rect rc1, Rect rc2)
    {
        bool ret = true;
        Vector2 ptLeftTop1 = new Vector2(rc1.x, rc1.y + rc1.height);
        Vector2 ptLeftBottom1 = new Vector2(rc1.x, rc1.y);
        Vector2 ptRightTop1 = new Vector2(rc1.x + rc1.width, rc1.y + rc1.height);
        Vector2 ptRightBottmom1 = new Vector2(rc1.x + rc1.width, rc1.y);

        Vector2 ptLeftTop2 = new Vector2(rc2.x, rc2.y + rc2.height);
        Vector2 ptLeftBottom2 = new Vector2(rc2.x, rc2.y);
        Vector2 ptRightTop2 = new Vector2(rc2.x + rc2.width, rc2.y + rc2.height);
        Vector2 ptRightBottmom2 = new Vector2(rc2.x + rc2.width, rc2.y);

        //四个顶点都不在对方区域中则true
        if (rc2.Contains(ptLeftTop1) || rc2.Contains(ptLeftBottom1) || rc2.Contains(ptRightTop1) || rc2.Contains(ptRightBottmom1))
        {
            ret = false;
        }

        if (rc1.Contains(ptLeftTop2) || rc1.Contains(ptLeftBottom2) || rc1.Contains(ptRightTop2) || rc1.Contains(ptRightBottmom2))
        {
            ret = false;
        }

        return ret;
    }

    float GetNumAngle(List<EditorItemPoint> listBihua)
    {
        if (listBihua.Count < 2)
        {
            return 0f;
        }
        // Vector2 pt_world = editorPoint.ImagePoint2World(pt);

        Vector2 ptStart = GetBihuaPoint(listBihua, 0);
        Vector2 ptEnd = GetBihuaPoint(listBihua, 1);
        Vector2 ptimage = ptEnd - ptStart;
        float angle = editorPoint.GetAngleDegreelOfVector(ptimage);

        return angle;

    }

    float GetArrowAngle(List<EditorItemPoint> listBihua)
    {
        if (listBihua.Count < 2)
        {
            return 0f;
        }
        // Vector2 pt_world = editorPoint.ImagePoint2World(pt);

        Vector2 ptStart = GetBihuaPoint(listBihua, listBihua.Count - 2);
        Vector2 ptEnd = GetBihuaPoint(listBihua, listBihua.Count - 1);
        Vector2 ptimage = ptEnd - ptStart;
        float angle = editorPoint.GetAngleDegreelOfVector(ptimage);

        return angle;

    }

    public Vector2 GetBihuaPoint(List<EditorItemPoint> listBihua, int idx)
    {
        EditorItemPoint item = listBihua[idx];
        return new Vector2(Common.String2Float(item.x), Common.String2Float(item.y));
    }

    void DrawBihua()
    {
        if (listDemoPoint.Count < 2)
        {
            return;
        }

        List<EditorItemPoint> listBihua = listDemoPoint[indexBihua] as List<EditorItemPoint>;
        if (indexBihuaPoint == 0)
        {
            //一个笔画
            CreateObjectWordWrite();
            Vector2 pt = GetBihuaPoint(listBihua, indexBihuaPoint);
            Vector2 pt_world = editorPoint.ImagePoint2World(pt);
            objNum = CreateBihuaNumObject(indexBihua, pt_world);
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

            Vector2 point = GetBihuaPoint(listBihua, indexBihuaPoint);
            //文件读取的点坐标是居于1024x768图片的坐标
            float z;
            z = letterImageZ;

            Vector2 pt_world = editorPoint.ImagePoint2World(point);

            Vector3 pt = new Vector3(pt_world.x, pt_world.y, z);//mainCamera.ScreenToWorldPoint(ptscreen);
            pt.z = z;

            //Debug.Log("AddPoint:" + pt + " ptscreen=" + ptscreen + " point" + point + " screen:" + Screen.width + " " + Screen.height);
            // pt.y -=17;
            // pt.x -=8f;
            wordWrite.AddPoint(pt);
            indexBihuaPoint++;
            if (indexBihuaPoint >= listBihua.Count)
            {
                float angelZ = GetArrowAngle(listBihua);
                objArrow = CreateBihuaArrowObject(listBihua.Count - 1, pt_world, angelZ);
                //下一笔画
                indexBihuaPoint = 0;
                indexBihua++;
                AjustNumArrowPosition(listBihua);
                break;
            }
        }

        wordWrite.DrawLine();





        // wordWrite.transform.localScale = new Vector3(scaleLetter,scaleLetter,1f);

        if (indexBihua >= listDemoPoint.Count)
        {
            indexBihua = 0;
            indexBihuaPoint = 0;
            //整个字写完
            Debug.Log("Write word bihua end");
        }
        else
        {
            DrawBihua();
        }

    }

    //调整数字和箭头的偏移量
    void AjustNumArrowPosition(List<EditorItemPoint> listBihua)
    {

        if (objNum != null)
        {
            bool ret = IsObjInclude(objNum);
            float angelZ = GetNumAngle(listBihua);
            angelZ = angelZ * (Mathf.PI * 2) / 360;
            Renderer rd = objNum.GetComponent<Renderer>();
            if (ret)
            {
                //调整偏移
                float oftx = rd.bounds.size.x * Mathf.Cos(angelZ) / 2;
                float ofty = rd.bounds.size.y * Mathf.Sin(angelZ) / 2;
                Vector3 pos = objNum.transform.localPosition;
                pos.x += oftx;
                pos.y += ofty;
                objNum.transform.localPosition = pos;

            }
            listObjBihuaTips.Add(objNum);
        }

        if (objArrow != null)
        {
            bool ret = IsObjInclude(objArrow);
            float angelZ = GetArrowAngle(listBihua);
            angelZ = angelZ * (Mathf.PI * 2) / 360;
            Renderer rd = objArrow.GetComponent<Renderer>();
            if (ret)
            {
                //调整偏移
                float oftx = rd.bounds.size.x * Mathf.Cos(angelZ) / 2;
                float ofty = rd.bounds.size.y * Mathf.Sin(angelZ) / 2;
                Vector3 pos = objArrow.transform.localPosition;
                pos.x -= oftx;
                pos.y -= ofty;
                objArrow.transform.localPosition = pos;
            }
            listObjBihuaTips.Add(objArrow);
        }
    }
}
