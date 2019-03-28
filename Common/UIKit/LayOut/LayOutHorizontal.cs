using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//水平布局
public class LayOutHorizontal : LayOutGrid
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
        base.LayOut();
    }
}
