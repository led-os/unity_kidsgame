
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vectrosity;
public class UIBoard : UIView
{
    public Vector2 sizeRect;
    public int mapSizeY;//行
    public int mapSizeX;//列
    public float lineWidth = 2f;//屏幕像素大小

    List<object> listLine;
    bool isHasDraw = false;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }


    void Start()
    {
        // DrawGrid(mapSizeX, mapSizeY);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (isHasDraw)
        {
            //   float w = GetRealLineWidth();
        }

    }

    public void Init()
    {
        if (listLine == null)
        {
            listLine = new List<object>();
            VectorLine.SetCamera3D(mainCam);
            isHasDraw = false;
        }

    }
    public void Draw()
    {
        foreach (VectorLine li in listLine)
        {
            GameObject obj = li.GetObj();
            DestroyImmediate(obj);
        }
        listLine.Clear();
        DrawGrid(1, 1);
    }
    //画网格
    void DrawGrid(int sizex, int sizey)
    {
        mapSizeX = sizex;
        mapSizeY = sizey;
        float x, y, z, step;
        z = 0;
        Vector2 worldsize = sizeRect;//Common.GetWorldSize(mainCam);
        float w_line_world = GetWorldLineWidth();
        //横线
        for (int i = 0; i < (mapSizeY + 1); i++)
        {
            step = worldsize.y / mapSizeY;
            x = -worldsize.x / 2;
            y = -worldsize.y / 2 + step * i;

            if (i == 0)
            {
                //最底部 偏移量
                y += w_line_world / 2;
            }
            if (i == mapSizeY)
            {
                //最顶部 偏移量
                y -= w_line_world / 2;
            }

            Vector3 posstart = new Vector3(x, y, z);
            x = worldsize.x / 2;
            Vector3 posend = new Vector3(x, y, z);
            string str = "line_row_" + i;
            DrawGridLine(str, posstart, posend);
        }

        //竖线
        for (int j = 0; j < mapSizeX + 1; j++)
        {
            step = worldsize.x / mapSizeX;
            y = -worldsize.y / 2;
            x = -worldsize.x / 2 + step * j;

            if (j == 0)
            {
                //最左 偏移量
                x += w_line_world / 2;
            }
            if (j == mapSizeX)
            {
                //最右 偏移量
                x -= w_line_world / 2;
            }

            Vector3 posstart = new Vector3(x, y, z);
            y = worldsize.y / 2;
            Vector3 posend = new Vector3(x, y, z);
            string str = "line_col_" + j;
            DrawGridLine(str, posstart, posend);
        }
        isHasDraw = true;

        Invoke("SetLineCollider", 0.2f);
    }
    //画网格线
    void DrawGridLine(string name, Vector3 posStart, Vector3 posEnd)
    {
        // Make Vector2 array; in this case we just use 2 elements...
        List<Vector3> linePoints = new List<Vector3>();
        linePoints.Add(posStart);
        linePoints.Add(posEnd);
        // Make a VectorLine object using the above points and the default material, with a width of 2 pixels
        VectorLine line = new VectorLine(name, linePoints, lineWidth);
        line.Draw3D();
        GameObject objLine = line.GetObj();
        objLine.transform.parent = this.gameObject.transform;
        objLine.transform.localPosition = Vector3.zero;
        // Renderer rd = objLine.GetComponent<Renderer>();
        // if (rd != null)
        // {
        //     Debug.Log("line bound size= " + rd.bounds.size);
        // }
        Rigidbody2D bd = objLine.AddComponent<Rigidbody2D>();
        bd.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bd.bodyType = RigidbodyType2D.Static;

        listLine.Add(line);
        // bd.useGravity = false;
    }
    public void SetLineCollider()
    {
        foreach (VectorLine li in listLine)
        {
            GameObject obj = li.GetObj();
            Renderer rd = obj.GetComponent<Renderer>();
            if (rd != null)
            {
                if (rd.bounds.size.x == 0)
                {
                    continue;
                }
                BoxCollider2D box = obj.GetComponent<BoxCollider2D>();
                if (box == null)
                {
                    box = obj.AddComponent<BoxCollider2D>();
                    box.size = rd.bounds.size;
                }
            }
        }

    }

    //实际显示大小
    public float GetWorldLineWidth()
    {
        float ret = Common.ScreenToWorldWidth(mainCam, lineWidth);
        return ret;

        if (listLine.Count != 0)
        {
            VectorLine line = listLine[0] as VectorLine;
            GameObject obj = line.GetObj();
            Renderer rd = obj.GetComponent<Renderer>();
            if (rd != null)
            {
                ret = Mathf.Min(rd.bounds.size.x, rd.bounds.size.y);
                Debug.Log("line real = " + ret + " w=" + Common.ScreenToWorldWidth(mainCam, lineWidth) + " h=" + Common.ScreenToWorldHeight(mainCam, lineWidth));
            }
        }

        return ret;
    }

}

