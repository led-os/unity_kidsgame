using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUITabBarItemClickDelegate(UITabBarItem ui);
public class UITabBarItem : MonoBehaviour
{
    public Button btnItem;
	public Text textTitle;
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

     public void UpdateItem(TabBarItemInfo info)
    { 
        textTitle.text = info.title;
    }

    public void OnClickBtnItem()
    {
        if (callbackClick != null)
        {
            callbackClick(this);
        }
    }
}
