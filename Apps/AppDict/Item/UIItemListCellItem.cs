using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;
//public delegate void OnUILoveCellItemDelegate(UILoveCellItem ui);
public class UIItemListCellItem : UICellItemBase
{

    public UIImage imageBg; 
    public UIText textTitle;
    public UIText textPinyin;
    public float itemWidth;
    public float itemHeight;
    public Color colorSel;
    public Color colorUnSel;
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
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        base.LayOut();

    }
    void SetSelect(bool isSel)
    {
        if (isSel)
        {
            textTitle.color = colorSel;
        }
        else
        {
            textTitle.color = colorUnSel;
        }
    }

    public void UpdateInfo(IdiomItemInfo info)
    {
        textTitle.text = info.title;
        textPinyin.text = info.pronunciation;
        LayOut();
    }
 
}
