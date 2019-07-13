using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
using Moonma.AdKit.AdVideo;
public class UIAdVideoController : UIView, ITableViewDataSource
{
    public RawImage imageBg;
    public Text textTitle;

    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public TableView tableView;
    public int numRows;
    private int numInstancesCreated = 0;
    public List<object> listItem;
    int oneCellNum;
    int heightCell;
    int totalItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        heightCell = 320;
        //bg
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);


    }

    // Use this for initialization
    void Start()
    {

        UpdateTitle();
        listItem = new List<object>();
        UpdateItem();
        oneCellNum = 1;
        totalItem = listItem.Count;
        numRows = totalItem;
        tableView.dataSource = this;
        LayOut();

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
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/AdVideo/UIAdVideoCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }


    public override void LayOut()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w = imageBg.texture.width;//rectTransform.rect.width;
            float h = imageBg.texture.height;//rectTransform.rect.height;
            print("imageBg size:w=" + w + " h=" + h);
            rctran.sizeDelta = new Vector2(w, h);
            float scalex = sizeCanvas.x / w;
            float scaley = sizeCanvas.y / h;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }


    }
    void UpdateTitle()
    {
        textTitle.text = "AdVideo";
    }

    public void UpdateItem()
    {
        listItem.Clear();
        {
            ItemInfo info = new ItemInfo();
            info.source = Source.UNITY;
            info.title = info.source;
            listItem.Add(info);
        }
        {
            ItemInfo info = new ItemInfo();
            info.source = Source.MOBVISTA;
            info.title = info.source;
            listItem.Add(info);
        }

        {
            ItemInfo info = new ItemInfo();
            info.source = Source.VUNGLE;
            info.title = info.source;
            listItem.Add(info);
        }

        {
            ItemInfo info = new ItemInfo();
            info.source = Source.ADMOB;
            info.title = info.source;
            listItem.Add(info);
        }


    }

    public void OnBtnClickBack()
    {
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        Debug.Log("AdVideo OnCellItemDidClick");
        ItemInfo info = listItem[item.index] as ItemInfo;
        AdVideo.SetType(AdVideo.ADVIDEO_TYPE_REWARD);
        AdVideo.InitAd(info.source);
        AdVideo.ShowAd();

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

            //  cell.SetCellSize(sizeCellNew);

            // Debug.LogFormat("TableView Cell Add Item:rcell:{0}, sizeCell:{1},rctable:{2},sizeTable:{3}", rccell, sizeCell, rctable, sizeTable);
            // oneCellNum = (int)(rctable.width / heightCell);
            //int i =0;
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
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        cell.UpdateItem(listItem);
        return cell;
    }

    #endregion 
}
