using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayViewController : PopViewController
{

    UIHowToPlayController uiControllerPrefab;
    UIHowToPlayController uiController;


    static private HowToPlayViewController _main = null;
    public static HowToPlayViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new HowToPlayViewController(); 
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    { 
        GameObject obj = PrefabCache.main.Load(AppRes.PREFAB_UIHowToPlayController);
        uiControllerPrefab = obj.GetComponent<UIHowToPlayController>();
    }

    public override void ViewDidLoad()
    { 
        base.ViewDidLoad();
        CreateUI();
    } 
    public void CreateUI()
    {
        uiController = (UIHowToPlayController)GameObject.Instantiate(uiControllerPrefab);
        uiController.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiControllerPrefab.gameObject, uiController.gameObject);
    }
}
