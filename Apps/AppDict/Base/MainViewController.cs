using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainViewController : TabBarViewController //TabBarViewController NaviViewController
{
    static private MainViewController _main = null;
    public static MainViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new MainViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        this.title = "Main";
        // this.Push(HomeViewController.main);
    }

    public override void ViewDidLoad()
    {
        //必须先调用基类方法以便初始化
        base.ViewDidLoad();

        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.title = Language.main.GetString("STR_HOME");
            // info.pic = ImageRes.main.GetImage("IMAGE_BtnBg");
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            navi.Push(HomeViewControllerDict.main);
            info.controller = navi;
            AddItem(info);
        }


        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.title = Language.main.GetString("STR_LOVE");
            // info.pic = ImageRes.main.GetImage("IMAGE_BtnBg");
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            navi.Push(LoveViewController.main);
            info.controller = navi;
            AddItem(info);
        }
        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.title = Language.main.GetString("STR_HISTORY");
            // info.pic = ImageRes.main.GetImage("IMAGE_BtnBg");
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            navi.Push(HistoryViewController.main);
            info.controller = navi;
            AddItem(info);
        }
        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.title = Language.main.GetString("STR_SETTING");
            // info.pic = ImageRes.main.GetImage("IMAGE_BtnBg");
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            navi.Push(SettingViewController.main);
            info.controller = navi;
            AddItem(info);
        }
        this.ShowImageBg(false);
        this.SelectItem(0);

        Debug.Log("MainViewController ViewDidLoad");
    }
    public override void ViewDidUnLoad()
    {
        //必须先调用基类方法
        base.ViewDidUnLoad();
    }

}
