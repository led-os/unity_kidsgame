using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LayOutSize : LayOutBase
{
    public float ratioW = 1f;
    public float ratioH = 1f;
    public GameObject target;

    //左下角偏移量
    public Vector2 _offsetMin;

    public Vector2 offsetMin
    {
        get
        {
            return _offsetMin;
        }

        set
        {
            _offsetMin = value;
            LayOut();
        }

    }

    //右上角角偏移量
    public Vector2 _offsetMax;
    public Vector2 offsetMax
    {
        get
        {
            return _offsetMax;
        }

        set
        {
            _offsetMax = value;
            LayOut();
        }

    }

    public Type _typeX;
    public Type typeX
    {
        get
        {
            return _typeX;
        }

        set
        {
            _typeX = value;
            LayOut();
        }

    }

    public Type _typeY;
    public Type typeY
    {
        get
        {
            return _typeY;
        }

        set
        {
            _typeY = value;
            LayOut();
        }

    }

    public enum Type
    {
        MATCH_CONTENT = 0,//按内容设置
        MATCH_PARENT,//与父窗口等大或者按比例 
        MATCH_TARGET,//与目标等大或者按比例 
        MATCH_PARENT_MIN,//父窗口width 和 height 的 min
        MATCH_PARENT_MAX,//父窗口width 和 height 的 max
    }
    void Awake()
    {
        this.LayOut();
    }
    void Start()
    {
        this.LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
        UpdateSize();
    }


    void UpdateSize()
    {
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranParent = this.transform.parent as RectTransform;
        if (rctranParent == null)
        {
            return;
        }
        RectTransform rctran = this.transform as RectTransform;
        var w_parent = rctranParent.rect.width;
        var h_parent = rctranParent.rect.height;
        w = rctran.rect.width;
        h = rctran.rect.height;
        switch (this.typeX)
        {
            case Type.MATCH_CONTENT:
                {
                    w = rctran.rect.width;
                    x = rctran.anchoredPosition.x;
                }
                break;


            case Type.MATCH_PARENT:
                {
                    w = w_parent * ratioW;
                    x = rctran.anchoredPosition.x;
                }
                break;
            case Type.MATCH_PARENT_MIN:
                {
                    w = Mathf.Min(w_parent, h_parent) * ratioW;
                    x = rctran.anchoredPosition.x;
                }
                break;
            case Type.MATCH_PARENT_MAX:
                {
                    w = Mathf.Max(w_parent, h_parent) * ratioW;
                    x = rctran.anchoredPosition.x;
                }
                break;
            case Type.MATCH_TARGET:
                {
                    if (this.target != null)
                    {
                        RectTransform rctranTarget = this.target.GetComponent<RectTransform>();
                        Vector2 ptTarget = rctranTarget.anchoredPosition;//this.target.getPosition();
                        w = rctranTarget.rect.width * ratioW;
                        x = rctran.anchoredPosition.x;
                    }

                }
                break;

        }


        switch (this.typeY)
        {
            case Type.MATCH_CONTENT:
                {
                    h = rctran.rect.height;
                    y = rctran.anchoredPosition.y;
                }
                break;

            case Type.MATCH_PARENT:
                {
                    h = h_parent * ratioH;
                    y = rctran.anchoredPosition.y;
                }
                break;
            case Type.MATCH_PARENT_MIN:
                {
                    h = Mathf.Min(w_parent, h_parent) * ratioH;
                    y = rctran.anchoredPosition.y;
                }
                break;
            case Type.MATCH_PARENT_MAX:
                {
                    h = Mathf.Max(w_parent, h_parent) * ratioH;
                    y = rctran.anchoredPosition.y;
                }
                break;
            case Type.MATCH_TARGET:
                {
                    if (this.target != null)
                    {
                        RectTransform rctranTarget = this.target.GetComponent<RectTransform>();
                        Vector2 ptTarget = rctranTarget.anchoredPosition;//this.target.getPosition();
                        h = rctranTarget.rect.height * ratioH;
                        y = rctran.anchoredPosition.y;
                    }

                }
                break;

        }

        w -= (this.offsetMin.x + this.offsetMax.x);
        h -= (this.offsetMin.y + this.offsetMax.y);
        rctran.sizeDelta = new Vector2(w, h);
        //rctran.anchoredPosition = new Vector2(x, y);
    }

}
