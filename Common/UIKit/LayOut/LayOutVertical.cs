using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//垂直布局
public class LayOutVertical : LayOutGrid
{
    void Awake()
    {
        col = 1;
        row = GetChildCount();
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
