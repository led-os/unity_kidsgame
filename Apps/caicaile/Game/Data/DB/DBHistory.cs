
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBHistory:DBBase
{ 
    
    static DBHistory _main = null;
    static bool isInited = false;
    public static DBHistory main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBHistory();
                Debug.Log("DBHistory main init");
                _main.dbFileName = "DBHistory.sqlite";
                _main.CreateDb();
            }
            return _main;
        }


        //ng:
        //  get
        // {
        //     if (_main==null)
        //     {
        //         _main = new LoveDB();
        //         Debug.Log("LoveDB main init");
        //         _main.CreateDb();
        //     }
        //     return _main;
        // }
    }

       

    //2017.1.10 to 1.10
    static public string getDateDisplay(string date)
    {
        int idx = date.IndexOf(".");
        if (idx >= 0)
        {
            string str = date.Substring(idx + 1);
            return str;
        }
        return date;
    }

    //{ "id", "intro", "album", "translation", "author", "year", "style", "pinyin", "appreciation", "head", "end", "tips", "date", "addtime" };
    public void AddItem(IdiomItemInfo info)
    {
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        values[0] = info.id;
        //values[0] = "性";//ng

        values[1] = info.pronunciation;

        /* 
        values[1] = info.intro;
        values[2] = info.album;
        Debug.Log("translation=" + info.translation);
        //values[3] = "成千上万匹马在奔跑腾跃。形容群众性的活动声势浩大或场面热烈。";
        //values[3] = "成千上万匹马在奔跑腾跃。形容群众性";//ng
        // values[3] = "性";//ng
        // values[3] = "性";//ng

        values[3] = "u6027";//\u6027

        values[4] = info.author;
        values[5] = info.year;
        values[6] = info.style;
       
        values[8] = info.appreciation;
        values[9] = info.head;
        values[10] = info.end;
        values[11] = info.tips;
        */

        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        string str = year + "." + month + "." + day;
        Debug.Log("date:" + str);
        values[lengh - 2] = str;
        long time_ms = Common.GetCurrentTimeMs();//GetCurrentTimeSecond
        values[lengh - 1] = time_ms.ToString();
        dbTool.InsertInto(TABLE_NAME, values);

        CloseDB();
        //  GetItemsByWord();


    }


    public void DeleteItem(IdiomItemInfo info)
    {
        OpenDB();
        // string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        dbTool.ExecuteQuery(strsql, true);
        CloseDB();
    }


     public bool IsItemExist(IdiomItemInfo info)
    {
        bool ret = false;
        OpenDB();
        //string strsql = "SELECT count(*) FROM " + TABLE_NAME + " WHERE id = "" + info.id + "'";
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        int count = 0;//qr.GetCount();
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            count++;
        }
        // qr.Release();
        Debug.Log("IsItemExist count=" + count);
        CloseDB();
        if (count > 0)
        {
            ret = true;
        }
        return ret;
    }

     void ReadInfo(IdiomItemInfo info, SqlInfo infosql)
    {
        info.id = dbTool.GetString(infosql, KEY_id);
        // info.pinyin = rd.GetString(KEY_pinyin);
        // Debug.Log("ReadInfo info.pinyin=" + info.pinyin);
        /* 
        info.intro = rd.GetString(KEY_intro);
        info.album = rd.GetString(KEY_album);
        info.translation = rd.GetString(KEY_translation);
        info.author = rd.GetString(KEY_author);
        info.year = rd.GetString(KEY_year);
        info.style = rd.GetString(KEY_style);
    
        info.appreciation = rd.GetString(KEY_appreciation);
        info.head = rd.GetString(KEY_head);
        info.end = rd
        .GetString(KEY_end);
        info.tips = rd.GetString(KEY_tips);
        */
        //  info.addtime = rd.GetString(KEY_addtime);
        // info.date = rd.GetString(KEY_date);
    }


    public List<IdiomItemInfo> GetAllItem()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT id from " + TABLE_NAME + " order by addtime desc";

        List<IdiomItemInfo> listRet = new List<IdiomItemInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllItem start read");
        dbTool.MoveToFirst(infosql);
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            Debug.Log("GetAllItem reading");
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, infosql);
            listRet.Add(info);
        }

        // reader.Release();

        CloseDB();
        Debug.Log("GetAllItem:" + listRet.Count);
        return listRet;
    }

    public List<IdiomItemInfo> GetAllDate()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT date from " + TABLE_NAME + " order by addtime desc";

        List<IdiomItemInfo> listRet = new List<IdiomItemInfo>();
        OpenDB();
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        dbTool.MoveToFirst(infosql);
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            info.date = dbTool.GetString(infosql, "date");
            listRet.Add(info);
        }

        // reader.Release();

        CloseDB();
        return listRet;
    }


    public List<IdiomItemInfo> GetItemByDate(string date)
    {
        string strsql = "select * from " + TABLE_NAME + " where date = '" + date + "'" + "order by addtime desc";
        List<IdiomItemInfo> listRet = new List<IdiomItemInfo>();
        OpenDB();

        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        dbTool.MoveToFirst(infosql);
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, infosql);
            listRet.Add(info);
        }

        // reader.Release();

        CloseDB();
        return listRet;
    }



    public IdiomItemInfo GetItemById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'";
        //List<IdiomItemInfo> listRet = new List<IdiomItemInfo>();
        IdiomItemInfo info = new IdiomItemInfo();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        dbTool.MoveToFirst(infosql);
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            ReadInfo(info, infosql);
            break;
            //listRet.Add(info);
        }

        //  reader.Release();

        CloseDB();
        return info;
    }


}

