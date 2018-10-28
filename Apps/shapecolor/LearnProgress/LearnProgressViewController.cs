using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnProgressViewController : PopViewController
{

    UILearnProgress uiPrefab;
    UILearnProgress ui;

    static private LearnProgressViewController _main = null;
    public static LearnProgressViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new LearnProgressViewController();
                _main.Init();
            }
            return _main;
        }
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    void Init()
    {
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Home/UILearnProgress");
            uiPrefab = obj.GetComponent<UILearnProgress>();
        }
    }

    public void CreateUI()
    {
        ui = (UILearnProgress)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
