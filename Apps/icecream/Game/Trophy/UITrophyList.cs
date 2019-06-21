using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//奖杯榜
public class UITrophyList : UIView, ITableViewDataSource
{
    public const int IMAGE_BG_BOARD_LEFT = 72;
    public const int IMAGE_BG_BOARD_RIGHT = 72;
    public const int IMAGE_BG_BOARD_TOP = 248;
    public const int IMAGE_BG_BOARD_BOTTOM = 72;

    public RawImage imageBg;
    public Image imageBoard;
    public Button btnClose;
    public GameObject objContent;
    public TableView tableView;

    public GameObject objTopBar;
    public GameObject objTips;
    public Image imageTitle;
    public Image imageTips;
    public UITrophyGet uiTrophyGet;
    public int numRows;
    int numInstancesCreated = 0;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public List<object> listItem;
    int oneCellNum;
    int heightCell;
    int totalItem;


    void Awake()
    {
        LoadPrefab();
        heightCell = 384;

        //bg
        TextureUtil.UpdateRawImageTexture(imageBg, TrophyRes.IMAGE_TROPHY_BG, true);
        TextureUtil.UpdateImageTexture(imageTitle, Language.main.IsChinese() ? AppRes.IMAGE_TROPHY_ImageTitle_cn : AppRes.IMAGE_TROPHY_ImageTitle_en, true);
        TextureUtil.UpdateImageTexture(imageTips, Language.main.IsChinese() ? AppRes.IMAGE_TROPHY_ImageTips_cn : AppRes.IMAGE_TROPHY_ImageTips_en, true);

        listItem = new List<object>();

        //index 0:奖励星  1：奖牌 2:奖杯
        {
            ItemInfo info = new ItemInfo();
            info.title = "奖励星";
            info.index = 0;
            listItem.Add(info);
        }

        {
            ItemInfo info = new ItemInfo();
            info.title = "奖牌";
            info.index = 1;
            listItem.Add(info);
        }

        {
            ItemInfo info = new ItemInfo();
            info.title = "奖杯";
            info.index = 2;
            listItem.Add(info);
        }

        UpdateTable(false);
        tableView.dataSource = this;



    }
    void Start()
    {
        LayOut();

        uiTrophyGet.gameObject.SetActive(false);

        //显示5s后消失
        Invoke("HideTips", 5.0f);

    }

    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Trophy/UITrophyCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    public override void LayOut()
    {
        float x, y, w, h, ratio, scale;
        RectTransform rctranTopBar = objTopBar.GetComponent<RectTransform>();
        RectTransform rctranTips = objTips.GetComponent<RectTransform>();
        LayOutScale ly = imageBg.GetComponent<LayOutScale>();
        if (ly != null)
        {
            ly.LayOut();
        }

        {
            scale = imageBg.transform.localScale.x;
            // scale = 0.9f;
            w = (imageBg.texture.width) * scale;
            h = IMAGE_BG_BOARD_TOP * scale;
            rctranTopBar.sizeDelta = new Vector2(w, h);
            x = 0;
            y = rctranTopBar.offsetMin.y;
            rctranTopBar.offsetMin = new Vector2(x, y);
            x = 0;
            y = h;
            rctranTopBar.offsetMax = new Vector2(x, y);

            ratio = 0.5f;
            w = rctranTopBar.rect.width * ratio;
            h = rctranTopBar.rect.height * ratio;
            scale = Common.GetBestFitScale(imageTitle.sprite.texture.width, imageTitle.sprite.texture.height, w, h);
            imageTitle.transform.localScale = new Vector3(scale, scale, 1f);
        }

        {
            ratio = 1f;
            w = rctranTips.rect.width * ratio;
            h = rctranTips.rect.height * ratio;
            scale = Common.GetBestFitScale(imageTips.sprite.texture.width, imageTips.sprite.texture.height, w, h);
            imageTips.transform.localScale = new Vector3(scale, scale, 1f);
        }

        {
            scale = imageBg.transform.localScale.x;
            // scale = 0.9f;
            w = (imageBg.texture.width - IMAGE_BG_BOARD_LEFT - IMAGE_BG_BOARD_RIGHT) * scale;
            h = (imageBg.texture.height - IMAGE_BG_BOARD_TOP - IMAGE_BG_BOARD_BOTTOM) * scale;
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(w, h);
            x = (IMAGE_BG_BOARD_LEFT - IMAGE_BG_BOARD_RIGHT) * scale / 2;
            y = (IMAGE_BG_BOARD_BOTTOM - IMAGE_BG_BOARD_TOP) * scale / 2;
            rctran.anchoredPosition = new Vector2(x, y);
        }
    }

    void HideTips()
    {
        objTips.SetActive(false);
    }
    public void OnClickBtnClose()
    {
        PopViewController p = this.controller as PopViewController;
        if (p != null)
        {
            p.Close();
        }
    }

    //position为屏幕坐标
    public Vector2 GetPosOfBtnTrophy(int idxcell)
    {
        Vector2 pos = Vector2.zero;
        TableViewCell cell = tableView.GetCellAtRow(idxcell);
        if (cell != null)
        {
            UITrophyCellItem item = cell.transform.GetComponentInChildren<UITrophyCellItem>();
            if (item != null)
            {
                pos = item.GetPosOfBtnTrophy();
                Debug.Log("UITrophyCellItem pos=" + pos);
            }
            else
            {
                Debug.Log("UITrophyCellItem is null");
            }
        }
        else
        {
            Debug.Log("TableViewCell is null");
        }
        return pos;
    }

    public void UpdateTable(bool isLoad)
    {
        oneCellNum = 1;
        if (Device.isLandscape)
        {
            oneCellNum = oneCellNum * 2;
        }

        int total = listItem.Count;
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
    public void OnCellItemDidClick(UICellItemBase item)
    {

        // if (this.callbackClose != null)
        // {
        //     this.callbackClose(this);
        // }
        OnClickBtnClose();
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
}
