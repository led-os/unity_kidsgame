using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetImageViewController : NaviViewController
{
    static private NetImageViewController _main = null;
    public static NetImageViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new NetImageViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        this.title = "Main"; 
    }

    public override void ViewDidLoad()
    {
        //必须先调用基类方法以便初始化
        base.ViewDidLoad();

        Debug.Log("MainViewController ViewDidLoad");
    }
    public override void ViewDidUnLoad()
    {
        //必须先调用基类方法
        base.ViewDidUnLoad();
    }

}
