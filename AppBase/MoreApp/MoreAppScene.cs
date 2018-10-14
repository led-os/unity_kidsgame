using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tacticsoft;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoreAppScene : ScriptBase, ITableViewDataSource
{


    //public const string APPCENTER_HTTP_URL_HOME = "http://42.96.196.180/moonma/app_center/applist_home.json";
    public const string APPCENTER_HTTP_URL_HOME_KIDS_GAME = "http://www.mooncore.cn/moonma/app_center/applist_moreapp_kids.json";
    public const string APPCENTER_HTTP_URL_HOME_SMALL_GAME = "http://www.mooncore.cn/moonma/app_center/applist_moreapp_smallgame.json";
    public const string APPCENTER_HTTP_URL_SORT = "http://www.mooncore.cn/moonma/app_center/applist_sort.json";
    public GameObject objTopBar;
    public GameObject objTableViewTemplate;
    public Image imageBg;
    public Image imageBarBg;
    public Button btnBack;
    public MoreAppCell cellPrefab;//GuankaItemCell GameObject 
    public MoreAppCellItem cellItemPrefab;
    public TableView tableView;
    public Text textTitle;
    public int numRows;
    private int numInstancesCreated = 0;

    private int oneCellNum;
    private MoreAppParser moreAppParser;
    //appcenter
    private List<ItemInfo> listAppMore;
    private AudioClip audioClipBtn;

 

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        InitScalerMatch();
    }
    // Use this for initialization
    void Start()
    {
        InitUiScaler(); 
        audioClipBtn = AudioCache.main.Load(AppCommon.AUDIO_BTN_CLICK);

        oneCellNum = 1;
        if (Screen.width > Screen.height)
        {
            oneCellNum = 2;
        }
        Rect rccell = (cellPrefab.transform as RectTransform).rect;
        Rect rctable = (tableView.transform as RectTransform).rect;
        //oneCellNum = (int)(rctable.width / rccell.height);
        int total = 0;//cene.MAX_GUANKA_NUM;
        if (listAppMore != null)
        {
            total = listAppMore.Count;
        }
        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }

        //.Log("MoreApp Start 1");
        tableView.dataSource = this;


        //Debug.Log("MoreApp Start 2");
        {
            string str = Language.main.GetString(AppString.STR_APPCENTER);
            textTitle.text = str;
        }

        StartParse();
        Debug.Log("MoreApp Start screen:w=" + Screen.width + " h=" + Screen.height);
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }

 

    void LayOut()
    {
        {
            RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
            float w_image = rectTransform.rect.width;
            float h_image = rectTransform.rect.height;
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }

        {
            //imageBarBg
            if (!Device.isLandscape)
            {
                RectTransform rctran = imageBarBg.GetComponent<RectTransform>();
                Vector2 sizeDelta = rctran.sizeDelta;
                float ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemTopBar);

                // rctran.offsetMax = new Vector2(rctran.offsetMax.x, rctran.offsetMax.y - ofty);
                // rctran.sizeDelta = sizeDelta;

                //增大显示
                rctran.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + ofty);

            }

        }
        {
            //btnBack
            // if (!Device.isLandscape)
            // {
            //     RectTransform rctran = btnBack.GetComponent<RectTransform>();
            //     Vector2 sizeDelta = rctran.sizeDelta;
            //     float ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemTopBar);
            //     rctran.offsetMax = new Vector2(rctran.offsetMax.x, rctran.offsetMax.y - ofty / 2);
            //     //offsetMax 修改之后sizeDelta也会跟着变化，需要还原
            //     rctran.sizeDelta = sizeDelta;
                
            // }

        }

        {
            //topbar
            if (!Device.isLandscape)
            {
                RectTransform rctran = objTopBar.GetComponent<RectTransform>();
                Vector2 sizeDelta = rctran.sizeDelta;
                float ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemTopBar);
                rctran.offsetMax = new Vector2(rctran.offsetMax.x, rctran.offsetMax.y - ofty);
                //offsetMax 修改之后sizeDelta也会跟着变化，需要还原
                rctran.sizeDelta = sizeDelta;
                //Debug.Log("objTopBar rctran=" + rctran.rect + " offsetMin=" + rctran.offsetMin + " offsetMax=" + rctran.offsetMax + " sizeDelta=" + rctran.sizeDelta);
            }

        }

        {
            //
            RectTransform rctran = objTableViewTemplate.GetComponent<RectTransform>();
            Vector2 sizeDelta = rctran.sizeDelta;
            //Debug.Log("TableViewTemplate rc=" + rctran.rect + " offsetMin=" + rctran.offsetMin + " offsetMax=" + rctran.offsetMax + " sizeDelta=" + rctran.sizeDelta);
            if (Device.isLandscape)
            {
                float ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemTopBar);
                //rctran.offsetMax = new Vector2(rctran.offsetMax.x, rctran.offsetMax.y - ofty);
                ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemHomeBar);
                rctran.offsetMin = new Vector2(rctran.offsetMin.x, rctran.offsetMin.y + ofty);
            }
            else
            {
                float ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemTopBar);
                rctran.offsetMax = new Vector2(rctran.offsetMax.x, rctran.offsetMax.y - ofty);
                ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemHomeBar);
                rctran.offsetMin = new Vector2(rctran.offsetMin.x, rctran.offsetMin.y + ofty);
            }

            //offsetMax 修改之后sizeDelta也会跟着变化，需要还原
            //rctran.sizeDelta = sizeDelta;
            //Debug.Log("TableViewTemplate rc=" + rctran.rect + " offsetMin=" + rctran.offsetMin + " offsetMax=" + rctran.offsetMax + " sizeDelta=" + rctran.sizeDelta);
        }
    }
    public void OnClickBtnBack()
    {


        {
            //AudioPlayer对象在场景切换后可能从当前scene移除了
            GameObject audioPlayer = GameObject.Find("AudioPlayer");
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClipBtn);
        }



        SceneManager.LoadScene(AppCommon.NAME_SCENE_MAIN);
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
        //return 256;
        return (cellPrefab.transform as RectTransform).rect.height;
    }

    //Will be called by the TableView when a cell needs to be created for display
    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        MoreAppCell cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as MoreAppCell;
        if (cell == null)
        {
            cell = (MoreAppCell)GameObject.Instantiate(cellPrefab);
            cell.name = "GuankaItemCellInstance_" + (++numInstancesCreated).ToString();
            Rect rccell = (cellPrefab.transform as RectTransform).rect;
            Rect rctable = (tableView.transform as RectTransform).rect;
            Vector2 sizeCell = (cellPrefab.transform as RectTransform).sizeDelta;
            Vector2 sizeTable = (tableView.transform as RectTransform).sizeDelta;
            Vector2 sizeCellNew = sizeCell;
            sizeCellNew.x = rctable.width;
            //(cell.transform as RectTransform).sizeDelta = sizeCellNew;
            cell.SetCellSize(sizeCellNew);

            //Debug.LogFormat("TableView Cell Add Item:rcell:{0}, sizeCell:{1},rctable:{2},sizeTable:{3}", rccell, sizeCell, rctable, sizeTable);
            //oneCellNum = (int)(rctable.width / rccell.height);
            //int i =0;

            for (int i = 0; i < oneCellNum; i++)
            {
                // GameObject obj = (GameObject)Resources.Load("Prefab/MoreApp/MoreAppCellItem");
                // MoreAppCellItem item = obj.GetComponent<MoreAppCellItem>();
                MoreAppCellItem item = cellItemPrefab;

                item = (MoreAppCellItem)GameObject.Instantiate(item);
                Rect rcItem = (item.transform as RectTransform).rect;

                item.transform.SetParent(cell.transform, false);
                item.index = row * oneCellNum + i;
                // LayoutElement layoutElement = item.GetComponent<LayoutElement>();
                // if (layoutElement == null) {
                //     layoutElement = item.gameObject.AddComponent<LayoutElement>();
                // }
                // layoutElement.preferredWidth =160;// GetHeightForRowInTableView(tableView,row);
                //  item.transform.SetSiblingIndex(1);


                RectTransform rectTransform = item.GetComponent<RectTransform>();
                //RectTransform rectTransform = item.transform as RectTransform;
                // Vector2 size = rectTransform.sizeDelta * scaleUI;
                Vector3 pos = new Vector3(rcItem.width * i, 0, 0);

                // rectTransform.position = pos;
                rectTransform.anchoredPosition = pos;
                cell.AddItem(i, item);


                Vector2 sizeItem = (item.transform as RectTransform).sizeDelta;
                //Debug.LogFormat("TableView Item:rcItem:{0}, sizeItem:{1},rectTransform.position:{2}", rcItem, sizeItem, rectTransform.position);
                rcItem.x = 0;
                rcItem.y = 0;
                // (item.transform as RectTransform).rect = rcItem;

            }

        }

        int cellNumCur = oneCellNum;
        if (row == GetNumberOfRowsForTableView(tableView) - 1)
        {

            cellNumCur = listAppMore.Count - (GetNumberOfRowsForTableView(tableView) - 1) * oneCellNum;
        }

        cell.SetRowNumber(row, oneCellNum, cellNumCur);
        cell.UpdateItem(listAppMore);
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
            MoreAppCell cell = (MoreAppCell)tableView.GetCellAtRow(row);
            cell.NotifyBecameVisible();
        }
    }

    #endregion

    #region  Parse
    void StartParse()
    {
        //Debug.Log("MoreApp StartParse 0");
        moreAppParser = new MoreAppParser();
        //Debug.Log("MoreApp StartParse 1");
        moreAppParser.callback = OnMoreAppParserFinished;
        //Debug.Log("MoreApp StartParse 2");
        string url = APPCENTER_HTTP_URL_HOME_KIDS_GAME;
        if (!Config.main.APP_FOR_KIDS)
        {
            url = APPCENTER_HTTP_URL_HOME_SMALL_GAME;
        }
        moreAppParser.startParserAppList(url);
        // Debug.Log("MoreApp StartParse 3");
    }

    void OnMoreAppParserFinished(MoreAppParser parser, List<ItemInfo> listApp)
    {
        listAppMore = listApp;

        int total = listAppMore.Count;//cene.MAX_GUANKA_NUM;
        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }
        //numRows = 0;
        Debug.Log("OnMoreAppParserFinished:numRows=" + numRows + " oneCellNum=" + oneCellNum + " total=" + total);
        tableView.ReloadData();
        //tableView.scrollY = 0;
    }

    #endregion
}
