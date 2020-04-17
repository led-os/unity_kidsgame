using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;


public class UISearch : UIView, IUIInputBarDelegate 
{
    public UIInputBar uiInputBar;
    public UIImage imageBg;
    public UIItemList uiItemList;  


    public void Awake()
    {
        base.Awake();
        uiInputBar.iDelegate = this; 
  
       
    }

    // Use this for initialization
    public void Start()
    {
        base.Start(); 
        LayOut();
 
    }

    public override void LayOut()
    {
        base.LayOut();
    }
 
    public void OnClickBtnPlay()
    {
    }

    public void OnUIInputBarValueChanged(UIInputBar ui)
    {
    }

    public void OnUIInputBarEnd(UIInputBar ui)
    {
 
        List<object> ls = DBIdiom.main.Search(ui.text);
        uiItemList.SetList(ls);
        // ui.text = "T";
        Debug.Log("OnUIInputBarEnd text=" + ui.text + " count=" + ls.Count);
        uiItemList.UpdateList();
    }
  



}
