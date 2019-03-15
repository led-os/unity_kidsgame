using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//方格布局
public class LayOutGrid : LayOutBase
{
    public int row = 1;//行
    public int col = 1;//列  

    private void Awake()
    {

    }

    private void Start()
    {
        LayOut();
    }
    // r 行 ; c 列  返回中心位置
    public Vector2 GetItemPostion(int r, int c)
    {
        float x, y, w, h;
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        w = rctran.rect.width;
        h = rctran.rect.height;
        float item_w = w / col;
        float item_h = h / row;

        x = -w / 2 + item_w * c + item_w / 2;
        y = -h / 2 + item_h * r + item_h / 2;

        return new Vector2(x, y);

    }

    public override void LayOut()
    {
        int idx = 0;
        int r = 0, c = 0;
        /* 
        foreach (Transform child in objMainWorld.transform)这种方式遍历子元素会漏掉部分子元素
        */

        //GetComponentsInChildren寻找的子对象也包括父对象自己本身和子对象的子对象
        foreach (Transform child in this.gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child == null)
            {
                // 过滤已经销毁的嵌套子对象
                Debug.Log("LayOut child is null idx=" + idx);
                continue;
            }
            GameObject objtmp = child.gameObject;
            if (this.gameObject == objtmp)
            {
                continue;
            }

            if (objtmp.transform.parent != this.gameObject.transform)
            {
                //只找第一层子物体
                continue;
            }

            r = idx / col;
            c = idx - r * col;

            //从顶部往底部显示
            r = row - 1 - r;

            Vector2 pt = GetItemPostion(r, c);
            RectTransform rctran = child.gameObject.GetComponent<RectTransform>();
            if (rctran != null)
            {
                rctran.anchoredPosition = pt;
                Debug.Log("GetItemPostion:idx=" + idx + " r=" + r + " c=" + c + " pt=" + pt);
            }
            idx++;
        }
    }
}
