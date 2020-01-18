using UnityEngine;
using UnityEngine.UI;

public class LayoutUtil
{
    static private LayoutUtil _main = null;
    public static LayoutUtil main
    {
        get
        {
            if (_main == null)
            {
                _main = new LayoutUtil();
            }
            return _main;
        }
    }

    //两个node之间的中心位置x
    public float GetBetweenCenterX(GameObject obj1, GameObject obj2)
    {
        GameObject objleft, objright;
        RectTransform rctran1 = obj1.GetComponent<RectTransform>();
        RectTransform rctran2 = obj2.GetComponent<RectTransform>();
        if (rctran1.anchoredPosition.x < rctran2.anchoredPosition.x)
        {
            objleft = obj1;
            objright = obj2;
        }
        else
        {
            objleft = obj2;
            objright = obj1;
        }
        RectTransform rctran = objleft.GetComponent<RectTransform>();
        float v1 = rctran.anchoredPosition.x + rctran.rect.width / 2;
        rctran = objright.GetComponent<RectTransform>();
        float v2 = rctran.anchoredPosition.x - rctran.rect.width / 2;
        return (v1 + v2) / 2;
    }


    //两个node之间的中心位置y

    public float GetBetweenCenterY(GameObject obj1, GameObject obj2)
    {
        GameObject objDown, objUp;
        RectTransform rctran1 = obj1.GetComponent<RectTransform>();
        RectTransform rctran2 = obj2.GetComponent<RectTransform>();
        if (rctran1.anchoredPosition.y < rctran2.anchoredPosition.y)
        {
            objDown = obj1;
            objUp = obj2;
        }
        else
        {
            objDown = obj2;
            objUp = obj1;
        }
        RectTransform rctran = objDown.GetComponent<RectTransform>();
        float v1 = rctran.anchoredPosition.y + rctran.rect.height / 2;
        rctran = objUp.GetComponent<RectTransform>();
        float v2 = rctran.anchoredPosition.y - rctran.rect.height / 2;
        return (v1 + v2) / 2;
    }


    //node和屏幕边界之间的中心位置x或者y
    public float GetBetweenScreenCenter(GameObject obj, LayOutBase.Align align)
    {
        float v1 = 0, v2 = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        RectTransform rctran = obj.GetComponent<RectTransform>();
        switch (align)
        {
            case LayOutBase.Align.LEFT:
                {
                    //左边界
                    v1 = -sizeCanvas.x / 2;
                    v2 = rctran.anchoredPosition.x - rctran.rect.width / 2;
                }
                break;
            case LayOutBase.Align.RIGHT:
                {
                    //右边界
                    v1 = sizeCanvas.x / 2;
                    v2 = rctran.anchoredPosition.x + rctran.rect.width / 2;
                }
                break;
            case LayOutBase.Align.UP:
                {
                    //上边界
                    v1 = sizeCanvas.y / 2;
                    v2 = rctran.anchoredPosition.y + rctran.rect.height / 2;
                }
                break;
            case LayOutBase.Align.DOWN:
                {
                    //下边界
                    v1 = -sizeCanvas.y / 2;
                    v2 = rctran.anchoredPosition.y - rctran.rect.height / 2;
                }
                break;
        }

        return (v1 + v2) / 2;
    }


    public int GetChildCount(GameObject parent, bool includeHide = true)
    {
        int count = 0;
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child == null)
            {
                // 过滤已经销毁的嵌套子对象 
                continue;
            }
            GameObject objtmp = child.gameObject;
            if (parent == objtmp)
            {
                continue;
            }

            if (!includeHide)
            {
                if (!objtmp.activeSelf)
                {
                    //过虑隐藏的
                    continue;
                }
            }

            {
                LayoutElement le = objtmp.GetComponent<LayoutElement>();
                if (le != null && le.ignoreLayout)
                {
                    continue;
                }
            }

            {
                LayOutElement le = objtmp.GetComponent<LayOutElement>();
                if (le != null && le.ignoreLayout)
                {
                    continue;
                }

            }


            if (objtmp.transform.parent != parent.transform)
            {
                //只找第一层子物体
                continue;
            }
            count++;
        }
        return count;
    }
}
