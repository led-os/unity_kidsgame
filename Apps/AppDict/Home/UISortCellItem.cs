using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;
//public delegate void OnUILoveCellItemDelegate(UILoveCellItem ui);
public class UISortCellItem : UICellItemBase
{
    public UIImage imageBg; 
    public UIText textTitle;  
    IdiomItemInfo infoItem;
    // public OnUILoveCellItemDelegate callbackClickDelete { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }

    void Start()
    {
        LayOut();
    }

    public override void UpdateItem(List<object> list)
    {
        infoItem = list[index] as IdiomItemInfo;
        UpdateInfo(infoItem);
        //  LayOut();
        Invoke("LayOut", 0.2f);
    } 
    public override void LayOut()
    {
        base.LayOut();

    } 
    public void UpdateInfo(IdiomItemInfo info)
    {
        textTitle.text = info.title; 
        LayOut();
    }

    public void OnClickBtnDelete()
    {
        // if (this.callbackClickDelete != null)
        // {
        //     this.callbackClickDelete(this);
        // }
    }

}
