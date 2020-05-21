using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUILoveControllerDidCloseDelegate(UILoveController ui);
public class UILoveController : UIView, ITableViewDataSource
{
    public OnUILoveControllerDidCloseDelegate callbackClose { get; set; }
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public Button btnDeleteAll;
    public int numRows;
    private int numInstancesCreated = 0;

    int totalItem;
    private int oneCellNum;
    private int heightCell;
    public TableView tableView;

    List<object> listItem;
    public UIText textTitle;
    public UIText textDetail;

    Color colorSel;
    Color colorUnSel;
    int indexSegment;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        colorSel = new Color(1f, 0f, 0f, 1f);
        colorUnSel = new Color(1f, 1f, 1f, 1f);
        //bg

        oneCellNum = 2;
        if (!Device.isLandscape)
        {
            oneCellNum = oneCellNum / 2;
        }
        heightCell = 256;
        LevelManager.main.ParseGuanka();
        tableView.dataSource = this;

        indexSegment = LevelManager.main.placeLevel;

        listItem = new List<object>();
        LanguageManager.main.UpdateLanguagePlace();
        LanguageManager.main.UpdateLanguage(indexSegment);
    }

    // Use this for initialization
    void Start()
    {

        UpdateTitle();
        UpdateList();

        LayOut();
        OnUIDidFinish(0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnBtnClickBack();
        }
    }
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Love/UILoveCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }


    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;


    }
    void UpdateTitle()
    {
        // string str = Language.main.GetString("STR_LOVE");
        // textTitle.text = str; 
        // textDetail.text = Language.main.GetString("STR_TITLE_LOVE_DBEmpty");
        // Common.SetButtonText(btnDeleteAll, Language.main.GetString("STR_TITLE_LOVE_DELETE_ALL"), 64, true);
    }
    void UpdateList()
    {
        List<IdiomItemInfo> ls = DBLove.main.GetAllItem();
        listItem.Clear();
        listItem.AddRange(ls);
        totalItem = listItem.Count;
        numRows = totalItem / oneCellNum;
        if (totalItem % oneCellNum != 0)
        {
            numRows++;
        }
        textDetail.gameObject.SetActive(DBLove.main.DBEmpty());
        btnDeleteAll.gameObject.SetActive(!DBLove.main.DBEmpty());
        tableView.ReloadData();
    }

    public void OnBtnClickBack()
    {
        // NaviViewController navi = this.controller.naviController;
        // if (navi != null)
        // {
        //     navi.Pop();
        // }
    }

    public void OnBtnClickDeleteAll()
    {
        DBLove.main.ClearDB();
        UpdateList();
    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        IdiomItemInfo info = listItem[item.index] as IdiomItemInfo;
        PopUpManager.main.Show<UIItemDetail>("App/Prefab/Item/UIItemDetail", popup =>
           {
               Debug.Log("UIItemDetail Open ");
               popup.UpdateItem(info);

           }, popup =>
           {
               Debug.Log("UIItemDetail Close ");

           });
    }

    public void OnCellItemDidClickDelete(UILoveCellItem ui)
    {
        IdiomItemInfo info = listItem[ui.index] as IdiomItemInfo;
        if (info != null)
        {
            DBLove.main.DeleteItem(info);
        }
        UpdateList();
    }

    #region ITableViewDataSource

    //Will be called by the TableView to know how many rows are in this table
    public int GetNumberOfRowsForTableView(TableView tableView)
    {
        return numRows;
    }

    //Will be called by the TableView to know what is the height of each row
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
        return heightCell;
        //return (cellPrefab.transform as RectTransform).rect.height;
    }

    //Will be called by the TableView when a cell needs to be created for display
    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        UICellBase cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as UICellBase;
        if (cell == null)
        {
            cell = (UICellBase)GameObject.Instantiate(cellPrefab);
            cell.name = "UICellBase" + (++numInstancesCreated).ToString();
            Rect rccell = (cellPrefab.transform as RectTransform).rect;
            Rect rctable = (tableView.transform as RectTransform).rect;
            Vector2 sizeCell = (cellPrefab.transform as RectTransform).sizeDelta;
            Vector2 sizeTable = (tableView.transform as RectTransform).sizeDelta;
            Vector2 sizeCellNew = sizeCell;
            sizeCellNew.x = rctable.width;

            for (int i = 0; i < oneCellNum; i++)
            {
                int itemIndex = row * oneCellNum + i;
                float cell_space = 10;
                UICellItemBase item = (UICellItemBase)GameObject.Instantiate(cellItemPrefab);
                //item.itemDelegate = this;
                Rect rcItem = (item.transform as RectTransform).rect;
                item.width = (rctable.width - cell_space * (oneCellNum - 1)) / oneCellNum;
                item.height = heightCell;
                item.transform.SetParent(cell.transform, false);
                item.index = itemIndex;
                item.totalItem = totalItem;
                item.callbackClick = OnCellItemDidClick;
                UILoveCellItem loveitem = item as UILoveCellItem;
                loveitem.callbackClickDelete = OnCellItemDidClickDelete;
                cell.AddItem(item);

            }

        }
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        foreach (UICellItemBase it_base in cell.listItem)
        {
            UILoveCellItem ui = it_base as UILoveCellItem;
            ui.colorSel = colorSel;
            ui.colorUnSel = colorUnSel;
        }
        cell.UpdateItem(listItem);
        return cell;
    }

    #endregion



    #region Table View event handlers

    //Will be called by the TableView when a cell's visibility changed
    public void TableViewCellVisibilityChanged(int row, bool isVisible)
    {
        //Debug.Log(string.Format("Row {0} visibility changed to {1}", row, isVisible));
        if (isVisible)
        {

        }
    }

    #endregion

}
