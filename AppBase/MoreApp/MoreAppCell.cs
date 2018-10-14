using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;


public class MoreAppCell : TableViewCell
{

    private Dictionary<int, MoreAppCellItem> dicItems;
    private int cellItemNum;
    private int cellIndex;
    private int cellDisplayItemNum;
    void Awake()
    {
        Debug.Log("MoreAppCell Awake");
        dicItems = new Dictionary<int, MoreAppCellItem>();
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("MoreAppCell Start");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRowNumber(int row, int cellNum,int dispNum)
    {
        cellItemNum = cellNum;
        cellIndex = row;
        cellDisplayItemNum = dispNum;
        // if(dicItems==null){
        // 	dicItems = new Dictionary<int, MoreAppCellItem>();

        // }
        if (dicItems.Count == 0)
        {
            return;
        }

        // m_rowNumberText.text = "Row " + rowNumber.ToString();
        // m_background.color = GetColorForRow(rowNumber);
        for (int i = 0; i < dicItems.Count; i++)
        {
            MoreAppCellItem item = dicItems[i];
            // item.itemDelegate = this;
            item.SetItemIndex(cellIndex * cellItemNum + i);
            if(i<cellDisplayItemNum){
                item.Hide(false);
            }else{
                item.Hide(true);
            }
        }
    }
    public void SetItem(MoreAppCellItem item)
    {

    }

    public void SetCellSize(Vector2 size)
    {
        Vector2 imagesize = size;
       // print("imagesize");
       // print(imagesize);
        // imagesize.x = 1630;
        // (m_background.transform as RectTransform).sizeDelta = imagesize;
    }

    public void AddItem(int idx, MoreAppCellItem item)
    {
        if (dicItems == null)
        {
           // Debug.Log("MoreAppCell AddItem null");
            //dicItems = new Dictionary<int, MoreAppCellItem>();

        }
        dicItems[idx] = item;
    }


    #region update
   public void UpdateItem(List<ItemInfo> list)
    {
        for (int i = 0; i < cellDisplayItemNum; i++)
        {
            MoreAppCellItem item = dicItems[i];
            int idx = cellIndex * cellItemNum + i;
            if (idx < list.Count)
            {
                ItemInfo info = list[idx];
                item.UpdateInfo(info);
            }

        }
    }
    #endregion

    private int m_numTimesBecameVisible;
    public void NotifyBecameVisible()
    {
        //m_numTimesBecameVisible++;
        //m_visibleCountText.text = "# rows this cell showed : " + m_numTimesBecameVisible.ToString();
    }
}
