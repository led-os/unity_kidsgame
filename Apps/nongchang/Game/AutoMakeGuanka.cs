using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoMakeGuanka
{
    public List<object> listGuanka;//string "0,1,2,3,4"
    int n;
    int total;
    string strSplit = ",";
    public void Init()
    {
        listGuanka = new List<object>();
        n = 5;
        total = (int)Mathf.Pow((float)n, 5f);
        Debug.Log("AutoMakeGuanka:total = " + total);

    }


    public void RunAutoMake()
    {
        listGuanka.Clear();
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
                }
            }
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
}
