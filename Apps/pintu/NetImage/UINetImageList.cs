using System.Collections;
using System.Collections.Generic;
using Moonma.SysImageLib;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UINetImageList : UIView, ITableViewDataSource, INetImageParseDelegate
{
    public int index;
    public GameObject objLayoutBtn;
    public Button btnPlay;
    public Button btnNetImage;

    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;//GuankaItemCell GameObject 
    public TableView tableView;
    public int numRows;
    private int numInstancesCreated = 0;
    int oneCellNum;
    int heightCell;
    int totalItem;
    List<object> listItem;

    NetImageParse netImageParse;
    void Awake()
    {

        listItem = new List<object>();
        heightCell = 256 + 128;
        UpdateTable(false);
        tableView.dataSource = this;

        netImageParse = new NetImageParse();
        netImageParse.iDelegate = this;
    }

    // Use this for initialization
    void Start()
    {
        ItemInfo info = listItem[index] as ItemInfo;
        netImageParse.StartParseImageList(info);

        LayOut();
        OnUIDidFinish();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickBtnPlay()
    {


    }

    public override void LayOut()
    {

    }

    #region NetImageParse_Delegate 
    void OnNetImageParseDidParseSortList(NetImageParse parse, bool isFail, List<object> list)
    {

    }
    void OnNetImageParseDidParseImageList(NetImageParse parse, bool isFail, List<object> list)
    {
        if ((!isFail) && (list != null))
        {
            listItem.Clear();
            foreach (Object obj in list)
            {
                listItem.Add(obj);
            }
            UpdateTable(true);
        }
    }
    #endregion

    #region GuankaItem_Delegate 
    void GotoGame(int idx)
    {
        GameManager.gameLevel = idx;
        GameManager.main.GotoGame(this.controller);
    }
    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        GotoGame(item.index);

    }

    #endregion


    void UpdateTable(bool isLoad)
    {
        oneCellNum = 3;
        if (Device.isLandscape)
        {
            oneCellNum = oneCellNum * 2;
        }

        int total = GameManager.maxGuankaNum;
        totalItem = total;
        Debug.Log("total:" + total);
        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }

        if (isLoad)
        {
            tableView.ReloadData();
        }

    }

    void AddCellItem(UICellBase cell, TableView tableView, int row)
    {
        Rect rctable = (tableView.transform as RectTransform).rect;

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

            cell.AddItem(item);

        }
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

            AddCellItem(cell, tableView, row);

        }
        cell.totalItem = totalItem;
        if (oneCellNum != cell.oneCellNum)
        {
            //relayout
            cell.ClearAllItem();
            AddCellItem(cell, tableView, row);
        }
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
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



    public void TableViewCellOnClik()
    {
        print("TableViewCellOnClik1111");
    }

}
