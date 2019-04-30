using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class AutoMakeGuankaInfo
{
    //public string pic;//0,0,100,100
    public string content;
    public string count;
}
public class AutoMakeGuanka
{
    public List<object> listGuanka;//string "0,1,2,3,4"
    List<AutoMakeGuankaInfo> listGuankaJson;
    int n_one_group;
    int total;
    string strSplit = ",";


    static public string filepathAutoGuankaJson
    {
        get
        {
            string dirRoot = Common.GAME_RES_DIR + "/guanka";
            return dirRoot + "/auto_guanka.json";
        }

    }
    public void Init()
    {
        listGuanka = new List<object>();
        listGuankaJson = new List<AutoMakeGuankaInfo>();
        n_one_group = 5;

    }

    //组合 C(n,m)
    void GetIndex(int n, int m)
    {

    }

    public void RunAutoMake()
    {
        GetNumList(2);
        GetNumList(3);
        GetNumList(4);
        GetNumList(5);
        SaveJson();
    }
    public void GetNumList(int m)
    {
        int[] IntArr = new int[] { 0, 1, 2, 3, 4 }; //整型数组
        List<int[]> ListCombination = PermutationCombination<int>.GetCombination(IntArr, m); //求全部的3-3组合
        Debug.Log("count =" + ListCombination.Count);
        foreach (int[] arr in ListCombination)
        {
            string str = "";
            int idx = 0;
            foreach (int item in arr)
            {
                str += item.ToString() + strSplit;
                if (idx == arr.Length - 1)
                {
                    str += item.ToString();
                }
                idx++;
            }
            AutoMakeGuankaInfo info = new AutoMakeGuankaInfo();
            info.content = str;
            info.count = m.ToString();
            listGuankaJson.Add(info);
            Debug.Log(str);
        }

    }

    public bool CheckInList(string str, List<object> list)
    {
        foreach (string tmp in list)
        {
            if (tmp == str)
            {
                return true;
            }
        }
        return false;
    }

    //去掉重复的元素
    void RemoveRepeatItems(List<string> list)
    {
        if (list.Count != 0)
        {
            //去掉重复的元素
            for (int i = 0; i < list.Count; i++)
            {
                string stritem = list[i];
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (stritem == list[j])
                    {
                        //remove
                        list.RemoveAt(i);
                        RemoveRepeatItems(list);
                    }
                }
            }
        }
    }

    //统计不同序号的个数
    public int GetCountOfItem(string str)
    {
        List<string> listStr = new List<string>();
        string[] strArray = str.Split(',');
        foreach (string stritem in strArray)
        {
            listStr.Add(stritem);
        }
        RemoveRepeatItems(listStr);
        return listStr.Count;
    }

    void SaveJson()
    {
        string filepath = filepathAutoGuankaJson;
        if (Application.isEditor)
        {
            filepath = Application.streamingAssetsPath + "/" + filepathAutoGuankaJson;
        }
        //save guanka json
        Hashtable data = new Hashtable();
        data["total"] = listGuankaJson.Count;
        data["items"] = listGuankaJson;
        string strJson = JsonMapper.ToJson(data);
        byte[] bytes = Encoding.UTF8.GetBytes(strJson);
        System.IO.File.WriteAllBytes(filepath, bytes);
    }



    public List<object> ParseAutoGuankaJson()
    {
        List<object> list = new List<object>();
        string json = FileUtil.ReadStringAsset(filepathAutoGuankaJson);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            LianLianLeItemInfo info = new LianLianLeItemInfo();
            info.id = (string)item["content"];
            info.count = Common.String2Int((string)item["count"]);
            list.Add(info);
        }
        return list;

    }

}
