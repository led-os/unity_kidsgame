using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button; 
public delegate void OnUIHistoryCellItemDelegate(UIHistoryCellItem ui);
public class UIHistoryCellItem : UICellItemBase
{

    public UIImage imageBg;
    public UIButton btnDelete;
    public UIText textTitle;
    public UIText textPinyin;
    public float itemWidth;
    public float itemHeight;
    public Color colorSel;
    public Color colorUnSel;
    IdiomItemInfo infoItem;
    public OnUIHistoryCellItemDelegate callbackClickDelete { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LevelManager.main.ParseGuanka();
        // Common.SetButtonText(btnDelete, Language.main.GetString("STR_IdiomDetail_DELETE_LOVE"), 0, false);
    }

    private void Start()
    {
        LayOut();
    }
    public override void UpdateItem(List<object> list)
    {
        infoItem = list[index] as IdiomItemInfo;
        IdiomParser.main.ParseIdiomItem(infoItem);

        // infoItem = DBIdiom.main.GetItemById(infoItem.id);

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
        Debug.Log("UpdateInfo info.title=" + info.title + " info.pronunciation=" + info.pronunciation);
        LayOut();

    }

    public void OnClickBtnDelete()
    {
        if (this.callbackClickDelete != null)
        {
            this.callbackClickDelete(this);
        }
    }

}
