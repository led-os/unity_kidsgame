using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//跟随紧靠在某个对象旁边
public class LayOutFollow : LayOutBase
{
    public GameObject target;
    public enum Direction
    {
        TOP = 0,
        BOTTOM,
        LEFT,
        RIGHT,

    }
    void Awake()
    {

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
