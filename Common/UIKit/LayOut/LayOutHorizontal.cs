using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//水平布局
public class LayOutHorizontal : HorizontalOrVerticalLayoutBase
{
    void Awake()
    {
        row = 1;
        col = GetChildCount();
    }
    void Start()
    {
        LayOut();
    }


    public override void LayOut()
    {
        col = GetChildCount();
        base.LayOut();
    }
}
