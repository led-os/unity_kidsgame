using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUITabBarClickDelegate(UITabBar bar, UITabBarItem item);

public class UITabBar : UIView
{
    public UIImage imageBg;
    UITabBarItem uiTabBarItem;
    UITabBarItem uiTabBarItemPrefab;
    public OnUITabBarClickDelegate callbackClick { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string strPrefab = "App/Prefab/TabBar/UITabBarItem";
        string strPrefabDefault = "Common/Prefab/TabBar/UITabBarItem";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        if (obj == null)
        {
            obj = PrefabCache.main.Load(strPrefabDefault);
        }
        if (obj != null)
        {
            uiTabBarItemPrefab = obj.GetComponent<UITabBarItem>();
        } 
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CreateTabItem()
    {
        uiTabBarItem = (UITabBarItem)GameObject.Instantiate(uiTabBarItemPrefab);
        uiTabBarItem.transform.parent = this.transform;
        uiTabBarItem.callbackClick = OnUITabBarItemClick;
        UIViewController.ClonePrefabRectTransform(uiTabBarItemPrefab.gameObject, uiTabBarItem.gameObject);
    }

    public void AddItem(TabBarItemInfo info, int idx)
    {
        CreateTabItem();
        uiTabBarItem.index = idx;
        uiTabBarItem.UpdateItem(info);
    }

    public void OnUITabBarItemClick(UITabBarItem ui)
    {
        if (callbackClick != null)
        {
            callbackClick(this, ui);
        }
    }
      public float GetBarHeight()
    { 
         RectTransform rctran = this.transform.GetComponent<RectTransform>();
         return rctran.rect.size.y;
    }
}
