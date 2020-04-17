using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayOutBase : MonoBehaviour
{
    public enum DispLayVertical
    {
        TOP_TO_BOTTOM = 0,
        BOTTOM_TO_TOP,
    }

    public enum DispLayHorizontal
    {
        LEFT_TO_RIGHT = 0,
        RIGHT_TO_LEFT,
    }


    public enum Align
    {
        UP = 0,
        DOWN,
        LEFT,
        RIGHT,
        CENTER,
        UP_LEFT,
        UP_RIGHT,
        DOWN_LEFT,
        DOWN_RIGHT,
        Horizontal,
        Vertical,
        SAME_POSTION,
    }


    public DispLayVertical dispLayVertical;
    public DispLayHorizontal dispLayHorizontal;
    public bool enableLayout = true;
    public bool enableHide = true;//是否过虑Hide

    public bool enableOffsetAdBanner=false;
    public bool enableOffsetScreen=false;//全面屏 四周的偏移
    public Vector2 space = Vector2.zero;

    protected TextAnchor childAlignment;
    public Align align = Align.CENTER;


    public virtual void LayOut()
    {

    }

    public int GetChildCount(bool includeHide = true)
    {
        return LayoutUtil.main.GetChildCount(this.gameObject, includeHide);
    }
}
