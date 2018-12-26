using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorWordViewController : UIViewController
{
    UIView uiPrefab;
    UIView ui;
    static private EditorWordViewController _main = null;
    public static EditorWordViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new EditorWordViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        this.title = "Main";
        //this.Push(HomeViewController.main); #endregion
        GameObject obj = PrefabCache.main.Load("App/Prefab/EditorWord/UIEditorWord");
        uiPrefab = obj.GetComponent<UIView>();
    }

    public void CreateUI()
    {
        // if (this.naviController != null)
        // {
        //     this.naviController.HideNavibar(true);
        // }
        ui = (UIView)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);

    }
    public override void ViewDidLoad()
    {
        //必须先调用基类方法以便初始化
        base.ViewDidLoad();
        CreateUI();
    }
    public override void ViewDidUnLoad()
    {
        //必须先调用基类方法
        base.ViewDidUnLoad();
    }

}
