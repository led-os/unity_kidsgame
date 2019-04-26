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
    int n;
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
        n = 5;
        total = (int)Mathf.Pow((float)n, 5f);
        Debug.Log("AutoMakeGuanka:total = " + total);

    }


    public void RunAutoMake()
    {
        listGuanka.Clear();
        listGuankaJson.Clear();

        List<object> listTmp = new List<object>();

        while (listTmp.Count < total)
        {
            string str = "";
            for (int i = 0; i < n; i++)
            {
                int rdm = Random.Range(0, n);

                if (i < n - 1)
                {
                    str += rdm.ToString() + strSplit;
                }
                else
                {
                    str += rdm.ToString();
                }
            }
            if (!CheckInList(str, listTmp))
            {
                listTmp.Add(str);
            }
        }

        //重新排列
        for (int num = 1; num <= n; num++)
        {
            foreach (string str in listTmp)
            {
                int count = GetCountOfItem(str);
                if (count == num)
                {
                    listGuanka.Add(str);

                    AutoMakeGuankaInfo info = new AutoMakeGuankaInfo();
                    info.content = str;
                    info.count = count.ToString();
                    listGuankaJson.Add(info);
                }
            }
        }

        SaveJson();
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
        Debug.Log("ParseAutoGuankaJson json = " + json);
        JsonData root = JsonMapper.ToObject(json);
        Debug.Log("ParseAutoGuankaJson ToObject end");
        JsonData items = root["items"];
        Debug.Log("ParseAutoGuankaJson root items end");
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            NongChangItemInfo info = new NongChangItemInfo();
            info.id = JsonUtil.JsonGetString(item, "content", "id");
            list.Add(info);
        }
        return list;

    }

    public void OnClickBtnGuanka()
    {
        RunAutoMake();
    }
}
