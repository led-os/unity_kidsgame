using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*相对布局 
在targetMain和target2的中心位置 Horizontal  Vertical
在targetMain和屏幕边界的中心位置cc.Align.LEFT cc.Align.RIGHT cc.Align.UP cc.Align.DOWN

*/
public class LayOutBetween : LayOutBase
{
    public GameObject targetMain;
    public GameObject target2;
    public Vector2 _offset;
    public Vector2 offset
    {
        get
        {
            return this._offset;
        }
        set
        {
            this._offset = value;
            this.LayOut();
        }
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
        float x, y, w, h;

        if (this.targetMain == null)
        {
            return;
        }

        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        if (rctran == null)
        {
            return;
        }

        Vector2 pt = rctran.anchoredPosition;//this.transform.position;//getPosition
        x = pt.x;
        y = pt.y;

        //左右
        if (this.align == LayOutBase.Align.Horizontal)
        {
            x = LayoutUtil.main.GetBetweenCenterX(this.targetMain, this.target2) + this.offset.x;
        }
        if (this.align == LayOutBase.Align.Vertical)
        {
            y = LayoutUtil.main.GetBetweenCenterY(this.targetMain, this.target2) + this.offset.y;
        }

        //屏幕边界
        if ((this.align == LayOutBase.Align.LEFT) || (this.align == LayOutBase.Align.RIGHT))
        {
            x = LayoutUtil.main.GetBetweenScreenCenter(this.targetMain, this.align) + this.offset.x;
        }
        if ((this.align == LayOutBase.Align.UP) || (this.align == LayOutBase.Align.DOWN)) 
        {
            y = LayoutUtil.main.GetBetweenScreenCenter(this.targetMain, this.align) + this.offset.y;
        }
        Debug.Log("LayOutBetween x=" + x + " y=" + y + " align=" + this.align);
        // this.node.setPosition(x, y);
        rctran.anchoredPosition = new Vector2(x, y);
    }
}
