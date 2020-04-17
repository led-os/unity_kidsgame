
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBHistory
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
    string[] item_col = new string[] { KEY_id, KEY_pinyin, KEY_date, KEY_addtime, };
    //string[] item_col = new string[] { KEY_id, KEY_intro, KEY_album, KEY_translation, KEY_author, KEY_year, KEY_style, KEY_pinyin, KEY_appreciation, KEY_head, KEY_end, KEY_tips, KEY_date, KEY_addtime };
    //string[] item_coltype = new string[] { KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text, KEY_text };
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
            Debug.Log("appDBPath=" + appDBPath);

            return appDBPath;
        }
    }
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

    void CreateDb()
    {
        dbTool = new DBToolSqliteKit();
        OpenDB();
        string[] item_coltype = new string[item_col.Length];
        for (int i = 0; i < item_coltype.Length; i++)
        {
            item_coltype[i] = KEY_text;
        }

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
        // foreach (FileInfo NextFile in TheFolder.GetFiles())
        // {
        //     NextFile.Delete();

        // }
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
        //string strsql = "SELECT count(*) FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        SQLiteQuery qr = dbTool.ExecuteQuery(strsql, false);
        int count = 0;//qr.GetCount();
        while (qr.Step())// 循环遍历数据 
        {
            count++;
        }
        qr.Release();
        Debug.Log("IsItemExist count=" + count);
        CloseDB();
        if (count > 0)
        {
            ret = true;
        }
        return ret;
    }

    void ReadInfo(IdiomItemInfo info, SQLiteQuery rd)
    {
        info.id = rd.GetString(KEY_id);
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
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllItem start read");
        while (reader.Step())// 循环遍历数据 
        {
            Debug.Log("GetAllItem reading");
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

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
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            info.date = reader.GetString("date");
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

    public List<IdiomItemInfo> GetItemByDate(string date)
    {
        string strsql = "select * from " + TABLE_NAME + " where date = '" + date + "'" + "order by addtime desc";
        List<IdiomItemInfo> listRet = new List<IdiomItemInfo>();
        OpenDB();

        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }


    public List<IdiomItemInfo> GetItemById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'" + "order by addtime desc";
        List<IdiomItemInfo> listRet = new List<IdiomItemInfo>();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

}

