using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUITabBarClickDelegate(UITabBar bar,UITabBarItem item);

public class UITabBar : UIView
{
    public Image imageBg;
    UITabBarItem uiTabBarItem;
    UITabBarItem uiTabBarItemPrefab;
	public GameObject objLayoutItem;
    public OnUITabBarClickDelegate callbackClick { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string strPrefab = "Common/Prefab/TabBar/UITabBarItem";
        GameObject obj = (GameObject)Resources.Load(strPrefab);
        uiTabBarItemPrefab = obj.GetComponent<UITabBarItem>();
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
        uiTabBarItem.transform.parent = objLayoutItem.transform;
        uiTabBarItem.callbackClick = OnUITabBarItemClick;
        ViewControllerManager.ClonePrefabRectTransform(uiTabBarItemPrefab.gameObject,uiTabBarItem.gameObject);
    }

    public void AddItem(TabBarItemInfo info,int idx)
    {
		CreateTabItem();
		uiTabBarItem.index = idx;
		uiTabBarItem.UpdateItem(info);
    }

    public void OnUITabBarItemClick(UITabBarItem ui)
    {
		 if (callbackClick != null)
        {
            callbackClick(this,ui);
        }
    } 
}
