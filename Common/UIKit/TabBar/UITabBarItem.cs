using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUITabBarItemClickDelegate(UITabBarItem ui);
public class UITabBarItem : UIView
{
    public UIImage imageBg;
    public UIText textTitle;
    public int index;

    public OnUITabBarItemClickDelegate callbackClick { get; set; }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void LayOut()
    {
        base.LayOut();
    }
    public void UpdateItem(TabBarItemInfo info)
    {
        textTitle.text = info.title;
        // textTitle.color = GetColorOfKey("TabBarTitle");
        if (!Common.isBlankString(info.pic))
        {
            imageBg.UpdateImage(info.pic, imageBg.keyImage);
        }
        this.LayOut();
    }

    public void OnClickBtnItem()
    {
        Debug.Log("UITabBarItem OnClickBtnItem ");
        if (callbackClick != null)
        {
            Debug.Log("UITabBarItem OnClickBtnItem 2");
            callbackClick(this);
        }
    }
}
