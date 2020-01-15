using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
public class PointManager
{
    public List<object> listGuanka;
    int rowTotal;
    int colTotal;
    public float width;
    public float height;
    public float offsetX;
    public float offsetY;
    static private PointManager _main = null;
    public static PointManager main
    {
        get
        {
            if (_main == null)
            {
                _main = new PointManager();
            }
            return _main;
        }
    }
    Rect GetRectDisplay()
    {
        Rect rc = Rect.zero;
        rc.width = width - offsetX * 2;
        rc.height = height - offsetY * 2;
        rc.x = -width / 2;
        rc.y = -height / 2;
        return rc;
    }
    public float GetItmeScaleInRect(Rect rc, GameObject obj)
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

    public Rect GetRectItem(int row, int col, int row_total, int col_total)
    {
        float x, y, w, h;
        Rect rcDisplay = GetRectDisplay();
        w = rcDisplay.width / col_total;
        h = rcDisplay.height / row_total;
        x = rcDisplay.x + w * col;
        y = rcDisplay.y + h * row;
        Rect rc = new Rect(x, y, w, h);
        return rc;
    }
    public Vector2 RandomPointOfRect(Rect rc, float offsetx, float offsety)
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


    //n中取m个数字的组合
    public int[] GetNumList(int n, int m)
    {
        int[] arrRet = new int[m];
        int[] IntArr = new int[n]; //整型数组
        for (int i = 0; i < n; i++)
        {
            IntArr[i] = i;
        }
        List<int[]> ListCombination = PermutationCombination<int>.GetCombination(IntArr, m); //求全部的3-3组合
        Debug.Log("count =" + ListCombination.Count);
        // foreach (int[] arr in ListCombination)
        // {
        //     string str = "";
        //     int idx = 0;
        //     foreach (int item in arr)
        //     {
        //         str += item.ToString() + ",";
        //         if (idx == arr.Length - 1)
        //         {
        //             str += item.ToString();
        //         }
        //         idx++;
        //     }

        //     Debug.Log(str);
        // }
        if (ListCombination.Count != 0)
        {
            int rdm = Random.Range(0, ListCombination.Count);
            arrRet = ListCombination[rdm];
        }
        return arrRet;
    }
}
