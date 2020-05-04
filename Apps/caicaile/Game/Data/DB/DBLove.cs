
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBLove : DBBase
{
 
 
    static DBLove _main = null;
    static bool isInited = false;
    public static DBLove main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBLove();
                Debug.Log("DBLove main init");
                _main.TABLE_NAME = "table_items";
                _main.dbFileName = "LoveIdiom.db";
                // _main.CopyDbFileFromResource();
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
        values[1] = info.title;
        values[2] = info.pronunciation;
        values[3] = info.album;
        values[4] = info.translation;

        values[5] = info.year;
        values[6] = info.usage;
        values[7] = info.common_use;
        values[8] = info.emotional;

        values[9] = info.structure;
        values[10] = info.near_synonym;
        values[11] = info.antonym;
        values[12] = info.example;
        values[13] = info.correct_pronunciation;

        // int year = System.DateTime.Now.Year;
        // int month = System.DateTime.Now.Month;
        // int day = System.DateTime.Now.Day;
        // string str = year + "." + month + "." + day;
        // Debug.Log("date:" + str);
        // values[lengh - 2] = str;
        // long time_ms = Common.GetCurrentTimeMs();//GetCurrentTimeSecond
        // values[lengh - 1] = time_ms.ToString();


        dbTool.InsertInto(TABLE_NAME, values);

        CloseDB();
        //  GetItemsByWord();


    }


    public void DeleteItem(IdiomItemInfo info)
    {
        OpenDB();
        // string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = "" + info.id + "'" + " and addtime = "" + info.addtime + "'";
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
        info.title = dbTool.GetString(infosql, KEY_title);
        info.pronunciation = dbTool.GetString(infosql, KEY_pinyin);
        info.album = dbTool.GetString(infosql, KEY_album);
        info.translation = dbTool.GetString(infosql, KEY_translation);
        info.year = dbTool.GetString(infosql, KEY_year);
        info.usage = dbTool.GetString(infosql, KEY_usage);
        info.common_use = dbTool.GetString(infosql, KEY_common_use);
        info.emotional = dbTool.GetString(infosql, KEY_emotional);
        info.structure = dbTool.GetString(infosql, KEY_structure);
        info.near_synonym = dbTool.GetString(infosql, KEY_near_synonym);
        info.antonym = dbTool.GetString(infosql, KEY_antonym);
        info.example = dbTool.GetString(infosql, KEY_example);
        info.correct_pronunciation = dbTool.GetString(infosql, KEY_correct_pronunciation);



        // Debug.Log("ReadInfo info.pinyin=" + info.pinyin);
        /* 
     
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


    public List<IdiomItemInfo> GetItemListById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'" + "order by addtime desc";
        List<IdiomItemInfo> listRet = new List<IdiomItemInfo>();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        dbTool.MoveToFirst(infosql);
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, infosql);
            listRet.Add(info);
        }

        //   reader.Release();

        CloseDB();
        return listRet;
    }



    public IdiomItemInfo GetItemByTitle(string title)
    {
        string strsql = "select * from " + TABLE_NAME + " where title = '" + title + "'";
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

        // reader.Release();

        CloseDB();
        return info;
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
    public List<object> Search(string word)
    {
        List<object> listRet = new List<object>();
        if (Common.isBlankString(word))
        {
            return listRet;
        }
        string strsearch = word;
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE title LIKE '%" + strsearch + "%'" + " OR id LIKE '%" + strsearch + "%'";
        //  strsql = "SELECT * FROM " + TABLE_NAME;
        // strsql = "SELECT rowid , * FROM " + TABLE_NAME;//SELECT rowid, * FROM "TableIdiom"
        //  strsql = "select * from TableIdiom where title like '%一%'";
        OpenDB();

        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        dbTool.MoveToFirst(infosql);
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, infosql);
            listRet.Add(info);
        }
        OpenDB();
        //   reader.Release();

        CloseDB();
        return listRet;
    }
}

