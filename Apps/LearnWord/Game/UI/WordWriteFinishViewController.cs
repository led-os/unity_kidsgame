using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordWriteFinishViewController : PopViewController
{
    public int indexPlace = 0;
    UIView uiPrefab;
    public UIWordWriteFinish ui;


    static private WordWriteFinishViewController _main = null;
    public static WordWriteFinishViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new WordWriteFinishViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

        string strPrefab = "AppCommon/Prefab/Game/UIWordWriteFinish";
        GameObject obj = PrefabCache.main.Load(strPrefab); 
        uiPrefab = obj.GetComponent<UIView>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
 
    public void CreateUI()
    {
        ui = (UIWordWriteFinish)GameObject.Instantiate(uiPrefab); 
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
