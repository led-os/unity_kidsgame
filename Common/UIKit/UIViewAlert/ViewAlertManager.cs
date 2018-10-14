using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewAlertManager
{
    UIViewAlert uiPrefab;
    UIViewAlert ui;

    public OnUIViewAlertFinishedDelegate callback { get; set; }

    static private ViewAlertManager _main = null;
    public static ViewAlertManager main
    {
        get
        {
            if (_main == null)
            {
                _main = new ViewAlertManager();
                _main.Init();
            }
            return _main;
        }
    }

    public string keyName;

    void Init()
    {
        GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIViewAlert/UIViewAlert");
        if (obj != null)
        {
            uiPrefab = obj.GetComponent<UIViewAlert>();
        }

    }
    public void ShowBtnNo(bool isShow)
    {
        if (ui != null)
        {
            ui.ShowBtnNo(isShow);
        }
    }

    public void Show(string title, string msg, string yes, string no)
    {
        if (ui == null)
        {
            ui = (UIViewAlert)GameObject.Instantiate(uiPrefab);
            ui.callback = OnUIViewAlertFinished;
        }
        ui.keyName = keyName;
        ui.SetText(title, msg, yes, no);
        ui.SetViewParent(AppSceneBase.main.canvasMain.gameObject);
        //SetViewParent之后需要初始化位置
        ViewControllerManager.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
        
        ui.transform.SetAsLastSibling();
    }

    public void ShowFull(string title, string msg, string yes, string no, bool isShowBtnNo, string name, OnUIViewAlertFinishedDelegate _callback)
    {
        keyName = name;
        callback = _callback;
        Show(title, msg, yes, no);

        //必须在show之后设置
        ShowBtnNo(isShowBtnNo);
    }

    public void Hide()
    {
        if (ui != null)
        {
            GameObject.DestroyImmediate(ui);
            ui = null;
        }
    }

    public void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {
        if (this.callback != null)
        {
            this.callback(alert, isYes);
        }
    }
}
