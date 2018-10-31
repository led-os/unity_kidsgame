using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUILearnProgressDidCloseDelegate(UILearnProgress ui);
public class UILearnProgress : UIView, ITableViewDataSource
{
    public OnUILearnProgressDidCloseDelegate callbackClose { get; set; }
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public Button btnShape;
    public Button btnColor;
    public Image imageBg;
    public int numRows;
    private int numInstancesCreated = 0;

    int totalItem;
    private int oneCellNum;
    private int heightCell;
    public TableView tableView;

    int itemType;
    List<object> listItem;

    public Text textTitle;

    Color colorSel;
    Color colorUnSel;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        // listItem = new List<ShapeColorItemInfo>();
        colorSel = new Color(1f, 0f, 0f, 1f);
        colorUnSel = new Color(1f, 1f, 1f, 1f);

        oneCellNum = 2;
        if (!Device.isLandscape)
        {
            oneCellNum = oneCellNum / 2;
        }
        heightCell = 256;
        UIGameShapeColor game = GameViewController.main.gameBase as UIGameShapeColor;
        game.ParseGuanka();
        tableView.dataSource = this;
    }

    // Use this for initialization
    void Start()
    {

        UpdateTitle();
        OnBtnClickShape();

        {
            Transform tr = btnShape.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            string str = Language.main.GetString("STR_TITLE_SHAPE");
            btnText.text = str;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, btnText.fontSize);
            RectTransform rctran = btnShape.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + oft * 2;
            rctran.sizeDelta = sizeDelta;
        }

        {
            Transform tr = btnColor.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            string str = Language.main.GetString("STR_TITLE_COLOR");
            btnText.text = str;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, btnText.fontSize);
            RectTransform rctran = btnShape.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + oft * 2;
            rctran.sizeDelta = sizeDelta;
        }

        LayoutChild();

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
            GameObject obj = PrefabCache.main.Load("App/Prefab/Home/UILearnProgressCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }


    void LayoutChild()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w = imageBg.sprite.texture.width;//rectTransform.rect.width;
            float h = imageBg.sprite.texture.height;//rectTransform.rect.height;
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
        string str = Language.main.GetString("STR_TITLE_LEARN_PROGRESS");
        textTitle.text = str;
    }
    void UpdateList(int type)
    {
        itemType = type;
        if (type == UILearnProgressCellItem.ITEM_TYPE_SHAPE)
        {
            listItem = UIGameShapeColor.listShape;
        }
        if (type == UILearnProgressCellItem.ITEM_TYPE_COLOR)
        {
            listItem = UIGameShapeColor.listColor;
        }

        totalItem = listItem.Count;
        numRows = totalItem / oneCellNum;
        if (totalItem % oneCellNum != 0)
        {
            numRows++;
        }

        tableView.ReloadData();
    }
    public void OnBtnClickBack()
    {
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
    }

    public void OnBtnClickShape()
    {
        {
            Transform tr = btnShape.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            btnText.color = colorSel;
        }
        {
            Transform tr = btnColor.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            btnText.color = colorUnSel;
        }


        UpdateList(UILearnProgressCellItem.ITEM_TYPE_SHAPE);
    }

    public void OnBtnClickColor()
    {
        {
            Transform tr = btnColor.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            btnText.color = colorSel;
        }
        {
            Transform tr = btnShape.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            btnText.color = colorUnSel;
        }
        UpdateList(UILearnProgressCellItem.ITEM_TYPE_COLOR);
    }


    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
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
        foreach (UICellItemBase it_base in cell.listItem)
        {
            UILearnProgressCellItem ui = it_base as UILearnProgressCellItem;
            ui.SetItemType(itemType);
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
