using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;


public class UIHome : UIView, IUIInputBarDelegate, ISegmentDelegate
{
    public UIInputBar uiInputBar; 
    public UISortList uiSortList;
    public UISegment uiSegment;
    int indexSegment;


    public void Awake()
    {
        base.Awake();
        uiInputBar.iDelegate = this;

        indexSegment = 0;
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;

        // DBHistory db = DBHistory.main;
        // DBLove db1 = DBLove.main;
    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
        UpdateSegment();
        LayOut();
        Common.UnityStartUpFinish(); 
        HomeViewControllerDict.main.SetAdBannerTop();
        OnUIDidFinish(0.5f);
    }

    public override void LayOut()
    {
        base.LayOut();
        HomeViewControllerDict.main.UpdateAdBanner();
    }

    public void UpdateSegment()
    {

        IdiomParser.main.ParseCategory();
        foreach (IdiomItemInfo info in IdiomParser.main.listCategory)
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.title = info.title;
            uiSegment.AddItem(infoSeg);
        }
        if (uiSegment != null)
        {
            uiSegment.UpdateList();
        }
        uiSegment.Select(indexSegment, true);
        uiSegment.ShowItemImageBg(false);
    }
    public void OnClickBtnPlay()
    {
    }
    public void OnTest()
    {
        // string str = "一";
        // str = "ren";
        // List<object> ls = DBIdiom.main.Search(str);

        // Debug.Log("OnUIInputBarEnd text=" + str + " count=" + ls.Count);
        // // uiIdiomList.UpdateList(ls);

        // SearchViewController p = SearchViewController.main;
        // p.litItem = ls;
        // p.Show(null, null);




    }
    public void OnUIInputBarValueChanged(UIInputBar ui)
    {
    }

    public void OnUIInputBarEnd(UIInputBar ui)
    {
        if(Common.BlankString(ui.text))
        {
            return;
        }
        List<object> ls = DBIdiom.main.Search(ui.text);
        // ui.text = "T";
        Debug.Log("OnUIInputBarEnd text=" + ui.text + " count=" + ls.Count);
        // uiIdiomList.UpdateList(ls);

        SearchViewController p = SearchViewController.main;
        p.litItem = ls;
        p.Show(null, null);

    }
    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        IdiomItemInfo info = IdiomParser.main.listCategory[item.index] as IdiomItemInfo;
        Debug.Log("SegmentDidClick title=" + info.title + " item=" + item.index);
        uiSortList.UpdateList(info.title);
    }



}
