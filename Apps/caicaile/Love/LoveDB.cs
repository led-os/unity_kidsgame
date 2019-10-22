
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoveDB
{
    public const string TABLE_NAME = "table_items";
    DBToolSqliteKit dbTool;
    public string dbFileName;

    public const string KEY_id = "id";
    public const string KEY_intro = "intro";
    public const string KEY_album = "album";
    public const string KEY_translation = "translation";
    public const string KEY_author = "author";
    public const string KEY_year = "year";
    public const string KEY_style = "style";
    public const string KEY_pinyin = "pinyin";
    public const string KEY_appreciation = "appreciation";
    public const string KEY_head = "head";
    public const string KEY_end = "end";
    public const string KEY_tips = "tips";

    public const string KEY_date = "date";
    public const string KEY_addtime = "addtime";


    public const string KEY_text = "text";
    string[] item_col = new string[] { KEY_id, KEY_intro, KEY_album, KEY_translation, KEY_author, KEY_year, KEY_style, KEY_pinyin, KEY_appreciation, KEY_head, KEY_end, KEY_tips, KEY_date, KEY_addtime };
    string[] item_coltype = new string[] { KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text };
    static public string strSaveWordShotDir//字截图保存目录
    {
        get
        {
            return Application.temporaryCachePath + "/SaveItem";
        }
    }

    public string dbFilePath
    {
        get
        {

            string appDBPath = Application.temporaryCachePath + "/" + dbFileName;
            Debug.Log(appDBPath);

            return appDBPath;
        }
    }
    static LoveDB _main = null;
    static bool isInited = false;
    public static LoveDB main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new LoveDB();
                Debug.Log("LoveDB main init");
                _main.dbFileName = "LoveDB.sqlite";
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

    void CreateDb()
    {
        dbTool = new DBToolSqliteKit();
        OpenDB();


        if (item_col.Length != item_coltype.Length)
        {
            Debug.Log("DB Table Error");
        }
        if (!dbTool.IsExitTable(TABLE_NAME))
        {
            dbTool.CreateTable(TABLE_NAME, item_col, item_coltype);
        }

        CloseDB();
    }

    void OpenDB()
    {
        dbTool.OpenDB(dbFilePath);
        //   string[] item_col = new string[] { "id,filesave,date,addtime" };
        //   string[] item_coltype = new string[] { "string,string,string,string" };

    }
    //判断是否空
    public bool DBEmpty()
    {
        bool ret = true;
        string strsql = "select id from " + TABLE_NAME + " order by addtime desc";
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            ret = false;
            break;
        }

        reader.Release();

        CloseDB();
        return ret;
    }

    void CloseDB()
    {
        dbTool.CloseDB();
    }

    public void ClearDB()
    {
        //
        string dir = strSaveWordShotDir;
        //Directory.Delete(dir);
        DirectoryInfo TheFolder = new DirectoryInfo(dir);

        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            NextFile.Delete();

        }
        OpenDB();
        dbTool.DeleteContents(TABLE_NAME);
        CloseDB();
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
    public void AddItem(CaiCaiLeItemInfo info)
    {
        OpenDB();
        string[] values = new string[item_col.Length];
        //id,filesave,date,addtime 

        values[0] = info.id;
        values[1] = info.intro;
        values[2] = info.album;
        values[3] = info.translation;
        values[4] = info.author;
        values[5] = info.year;
        values[6] = info.style;
        values[7] = info.pinyin;
        values[8] = info.appreciation;
        values[9] = info.head;
        values[10] = info.end;
        values[11] = info.tips;

        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        string str = year + "." + month + "." + day;
        Debug.Log("date:" + str);
        values[item_col.Length - 2] = str;
        long time_ms = Common.GetCurrentTimeMs();//GetCurrentTimeSecond
        values[item_col.Length - 1] = time_ms.ToString();
        dbTool.InsertInto(TABLE_NAME, values);

        CloseDB();
        //  GetItemsByWord();


    }

    void ReadInfo(CaiCaiLeItemInfo info, SQLiteQuery rd)
    {
        info.id = rd.GetString(KEY_id);
        info.intro = rd.GetString(KEY_intro);
        info.album = rd.GetString(KEY_album);
        info.translation = rd.GetString(KEY_translation);
        info.author = rd.GetString(KEY_author);
        info.year = rd.GetString(KEY_year);
        info.style = rd.GetString(KEY_style);
        info.pinyin = rd.GetString(KEY_pinyin);
        info.appreciation = rd.GetString(KEY_appreciation);
        info.head = rd.GetString(KEY_head);
        info.end = rd.GetString(KEY_end);
        info.tips = rd.GetString(KEY_tips);
        info.addtime = rd.GetString(KEY_addtime);
        info.date = rd.GetString(KEY_date);
    }

    public void DeleteItem(CaiCaiLeItemInfo info)
    {
        OpenDB();
        string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        dbTool.ExecuteQuery(strsql, true);
        CloseDB();
    }

    public List<CaiCaiLeItemInfo> GetAllItem()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT id from " + TABLE_NAME + " order by addtime desc";

        List<CaiCaiLeItemInfo> listRet = new List<CaiCaiLeItemInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllItem start read");
        while (reader.Step())// 循环遍历数据 
        {
            Debug.Log("GetAllItem reading");
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            info.id = reader.GetString(KEY_id);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        Debug.Log("GetAllItem:" + listRet.Count);
        return listRet;
    }

    public List<CaiCaiLeItemInfo> GetAllDate()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT date from " + TABLE_NAME + " order by addtime desc";

        List<CaiCaiLeItemInfo> listRet = new List<CaiCaiLeItemInfo>();
        OpenDB();
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            info.date = reader.GetString("date");
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

    public List<CaiCaiLeItemInfo> GetItemByDate(string date)
    {
        string strsql = "select * from " + TABLE_NAME + " where date = '" + date + "'" + "order by addtime desc";
        List<CaiCaiLeItemInfo> listRet = new List<CaiCaiLeItemInfo>();
        OpenDB();

        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }


    public List<CaiCaiLeItemInfo> GetItemById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'" + "order by addtime desc";
        List<CaiCaiLeItemInfo> listRet = new List<CaiCaiLeItemInfo>();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

}

