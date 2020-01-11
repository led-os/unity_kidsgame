using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//相对布局 位于target的左边 右边 下边 上边
public class LayOutRelation : LayOutBase
{
    public GameObject target;
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

        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        if (rctran == null)
        {
            return;
        }

        Vector2 pt = rctran.anchoredPosition;//this.transform.position;//getPosition
        x = pt.x;
        y = pt.y;

        w = rctran.rect.width;
        h = rctran.rect.height;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        if (this.target == null)
        {
            //相对屏幕
            if (this.align == LayOutBase.Align.LEFT)
            {
                x = -sizeCanvas.x / 2 + w / 2 + this.offset.x;
            }
            if (this.align == LayOutBase.Align.RIGHT)
            {
                x = sizeCanvas.x / 2 - w / 2 - this.offset.x;
            }
            if (this.align == LayOutBase.Align.UP)
            {
                y = sizeCanvas.y / 2 - h / 2 - this.offset.y;
            }
            if (this.align == LayOutBase.Align.DOWN)
            {
                y = -sizeCanvas.y / 2 + h / 2 + this.offset.y;
            }
        }
        else
        {
            //相对目标

            RectTransform rctranTarget = this.target.GetComponent<RectTransform>();
            Vector2 ptTarget = rctranTarget.anchoredPosition;//this.target.getPosition();
                                                             //位于target的左边
            if (this.align == LayOutBase.Align.LEFT)
            {
                x = ptTarget.x - rctranTarget.rect.width / 2 - w / 2 - this.offset.x;
            }
            if (this.align == LayOutBase.Align.RIGHT)
            {
                x = ptTarget.x + rctranTarget.rect.width / 2 + w / 2 + this.offset.x;
            }
            if (this.align == LayOutBase.Align.UP)
            {
                y = ptTarget.y + rctranTarget.rect.height / 2 + h / 2 + this.offset.y;
            }
            if (this.align == LayOutBase.Align.DOWN)
            {
                y = ptTarget.y - rctranTarget.rect.height / 2 - h / 2 - this.offset.y;
            }

            //相同位置
            if (this.align == LayOutBase.Align.SAME_POSTION)
            {
                x = ptTarget.x;
                y = ptTarget.y;
            }
        }

        //this.node.setPosition(x, y);
        rctran.anchoredPosition = new Vector2(x, y);
    }

}
