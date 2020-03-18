using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEditor;
using System.Text;


public class FlowerEditorJsonItemInfo
{
    public string rowcol;
}

public class FlowerEditorItemInfo
{
    public int index;
    public int row;
    public int col;
}
public class EditorIdiomFlower : Editor
{
    static public int rowTotal;
    static public int colTotal;
    static public int totalIdiom;
    static List<FlowerEditorJsonItemInfo> listJson = new List<FlowerEditorJsonItemInfo>();
    static public FlowerEditorItemInfo infoSeed;
    static public List<FlowerEditorItemInfo> listItemSel;//选中 
    [MenuItem("CaiCaiLe/MakeIdiomFlowerData")]
    static void OnMakeIdiomFlowerData()
    {
        rowTotal = 4;
        colTotal = 4;
        totalIdiom = rowTotal * colTotal / 4;
        listItemSel = new List<FlowerEditorItemInfo>();

        int total = 1000;//1000
        int count = 0;
        while (count < total)
        {
            bool ret = MakeIdiomFlowerData();
            if (ret)
            {
                AddJson();
                count++;
            }
        }
        SaveJson();
        Debug.Log("OnMakeIdiomFlowerData Finish");

    }

    static void AddJson()
    {
        //save guanka json 
        string strrowcol = "";
        FlowerEditorJsonItemInfo infojson = new FlowerEditorJsonItemInfo();
        int idx = 0;
        foreach (FlowerEditorItemInfo info in listItemSel)
        {

            string str = info.row.ToString() + "-" + info.col.ToString();
            strrowcol += str;
            if (idx < listItemSel.Count - 1)
            {
                strrowcol += ",";
            }
            idx++;
        }
        infojson.rowcol = strrowcol;
        listJson.Add(infojson);

    }

    static void SaveJson()
    {
        //save guanka json  
        Hashtable data = new Hashtable();
        data["items"] = listJson;
        string strJson = JsonMapper.ToJson(data);
        string dir = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR + "/guanka/data";
        FileUtil.CreateDir(dir);
        string filepath = dir + "/item_position.json";
        byte[] bytes = Encoding.UTF8.GetBytes(strJson);
        System.IO.File.WriteAllBytes(filepath, bytes);

    }

    static bool MakeIdiomFlowerData()
    {
        bool ret = true;
        FlowerEditorItemInfo infoSeed = null;
        bool isError = false;
        listItemSel.Clear();
        for (int i = 0; i < rowTotal * colTotal; i++)
        {
            if (listItemSel.Count % 4 == 0)
            {
                //成语的开头
                infoSeed = GetSeed();
                if (infoSeed != null)
                {
                    if (infoSeed.col < 0)
                    {
                        // Debug.Log("GetSeed  row=" + infoSeed.row + " col=" + infoSeed.col);
                    }
                }


            }
            if (infoSeed == null)
            {
                //ng
                isError = true;
                break;
            }


            List<FlowerEditorItemInfo> listSide = GetSideItems(infoSeed.row, infoSeed.col);
            if (listSide.Count == 0)
            {
                //ng
                isError = true;
                break;
            }

            int idx = Random.Range(0, listSide.Count);
            FlowerEditorItemInfo info = listSide[idx];
            listItemSel.Add(info);
            infoSeed = info;
            if (infoSeed.col < 0)
            {
                Debug.Log("GetSeed  row=" + infoSeed.row + " col=" + infoSeed.col);
            }

        }

        if (isError)
        {
            if (listItemSel.Count < totalIdiom * 4)
            {
                // fail
                ret = false;
            }
        }
        return ret;
    }
    static FlowerEditorItemInfo GetSeed()
    {
        List<FlowerEditorItemInfo> list = new List<FlowerEditorItemInfo>();
        for (int i = 0; i < rowTotal; i++)
        {
            for (int j = 0; j < colTotal; j++)
            {
                if (!IsItemHasSel(i, j))
                {
                    FlowerEditorItemInfo info = new FlowerEditorItemInfo();
                    info.row = i;
                    info.col = j;
                    if (info.col < 0)
                    {
                        // Debug.Log("GetSeed inner  row=" + info.row + " col=" + info.col);
                    }
                    list.Add(info);
                }
            }
        }
        if (list.Count > 0)
        {
            int idx = Random.Range(0, list.Count);
            FlowerEditorItemInfo infoRet = list[idx];
            return infoRet;
        }
        Debug.Log("GetSeed ret null=");
        return null;
    }
    static bool IsItemHasSel(int row, int col)
    {
        bool ret = false;
        foreach (FlowerEditorItemInfo info in listItemSel)
        {
            if ((info.row == row) && (info.col == col))
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    static void AddItem(List<FlowerEditorItemInfo> list, int rtmp, int ctmp, int direct)
    {
        if (!IsItemHasSel(rtmp, ctmp))
        {
            FlowerEditorItemInfo info = new FlowerEditorItemInfo();
            info.row = rtmp;
            info.col = ctmp;
            if (ctmp < 0)
            {
                Debug.Log("AddItem  row=" + rtmp + " col=" + ctmp + " direct=" + direct);
            }
            list.Add(info);
        }
    }
    //获取相邻的item
    static List<FlowerEditorItemInfo> GetSideItems(int row, int col)
    {
        int r, c;
        List<FlowerEditorItemInfo> list = new List<FlowerEditorItemInfo>();
        //左
        {
            r = row;
            c = col;
            c--;
            if (c >= 0)
            {
                AddItem(list, r, c, 0);
            }

        }
        //右
        {
            r = row;
            c = col;
            c++;
            if (c < colTotal)
            {
                AddItem(list, r, c, 1);
            }
        }
        //上
        {
            r = row;
            c = col;
            r++;
            if (r < rowTotal)
            {
                AddItem(list, r, c, 2);
            }
        }
        //下
        {
            r = row;
            c = col;
            r--;
            if (r >= 0)
            {
                AddItem(list, r, c, 3);
            }
        }



        //左上
        {
            r = row;
            c = col;
            c--;
            r++;
            if ((c >= 0) && (r < rowTotal))
            {
                AddItem(list, r, c, 4);
            }
        }
        //左下
        {
            r = row;
            c = col;
            c--;
            r--;
            if ((c >= 0) && (r >= 0))
            {
                AddItem(list, r, c, 5);
            }
        }

        //右上
        {
            r = row;
            c = col;
            c++;
            r++;
            if ((c < colTotal) && (r < rowTotal))
            {
                AddItem(list, r, c, 6);
            }
        }
        //右下 
        {
            r = row;
            c = col;
            c++;
            r--;
            if ((r >= 0) && (c < colTotal))
            {
                AddItem(list, r, c, 7);
            }
        }

        return list;
    }
}
