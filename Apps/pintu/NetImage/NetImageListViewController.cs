using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetImageListViewController : NaviViewController
{
    static private NetImageListViewController _main = null;
    public static NetImageListViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new NetImageListViewController();
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
