using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingViewController : PopViewController
{

    UISettingControllerBase uiSettingPrefab;
    UISettingControllerBase uiSetting;

    static private SettingViewController _main = null;
    public static SettingViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new SettingViewController();
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
            GameObject obj = PrefabCache.main.Load(AppRes.PREFAB_SETTING);
            if (obj == null)
            {
                obj = PrefabCache.main.Load(AppCommon.PREFAB_SETTING);
            }
            uiSettingPrefab = obj.GetComponent<UISettingControllerBase>();
        }
    }

    public void CreateUI()
    {
        uiSetting = (UISettingControllerBase)GameObject.Instantiate(uiSettingPrefab);
        uiSetting.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiSettingPrefab.gameObject, uiSetting.gameObject);
    }

}
