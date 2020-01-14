
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleSQL;


public class DBWordInfo
{
    // The WeaponID field is set as the primary key in the SQLite database,
    // so we reflect that here with the PrimaryKey attribute
    //   [PrimaryKey]
    public string id { get; set; }
    public string word { get; set; }
    public string pinyin { get; set; }//拼音
    public string zuci { get; set; }//组词
    public string bushou { get; set; }//部首
    public string bihua { get; set; }//笔画
    public string audio { get; set; }
    public string gif { get; set; }
    public string mean { get; set; }
    public string date { get; set; }
    public string addtime { get; set; }

}
public class DBLearnWord
{
    public SimpleSQLManager2 dbManager;
    public const string TABLE_NAME = "TableItem";
    DBToolSqliteKit dbTool;
    public string dbFileName;

    public const string KEY_id = "id";
    public const string KEY_word = "word";
    public const string KEY_pinyin = "pinyin";
    public const string KEY_zuci = "zuci";
    public const string KEY_bushou = "bushou";
    public const string KEY_bihua = "bihua";
    public const string KEY_audio = "audio";
    public const string KEY_gif = "gif";
    public const string KEY_mean = "mean";

    public const string KEY_date = "date";
    public const string KEY_addtime = "addtime";


    public const string KEY_text = "text";

    string[] item_col = new string[] { KEY_id, KEY_word, KEY_pinyin, KEY_zuci, KEY_bushou, KEY_bihua, KEY_audio, KEY_gif, KEY_mean, KEY_date, KEY_addtime };
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
    static DBLearnWord _main = null;
    static bool isInited = false;
    public static DBLearnWord main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBLearnWord();
                Debug.Log("DBLearnWord main init");
                _main.dbFileName = "Word2_" + Common.appKeyName + ".db";
                _main.CopyDbFileFromResource();
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

    void CopyDbFileFromResource()
    {
        string src = Common.GAME_RES_DIR + "/Item.db";
        string dst = dbFilePath;
        if (!FileUtil.FileIsExist(dst))
        {
            byte[] data = FileUtil.ReadDataAsset(src);
            System.IO.File.WriteAllBytes(dst, data);
        }

    }
    public void CreateDb()
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
    public void AddItem(DBWordInfo info)
    {
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        values[0] = info.id;
        values[1] = info.word;
        values[2] = info.pinyin;
        values[3] = info.zuci;
        values[4] = info.bushou;
        values[5] = info.bihua;
        values[6] = info.audio;
        values[7] = info.gif;
        values[8] = info.mean;
        values[9] = info.date;
        values[10] = info.addtime;

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


    public void DeleteItem(DBWordInfo info)
    {
        OpenDB();
        // string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        dbTool.ExecuteQuery(strsql, true);
        CloseDB();
    }


    public bool IsItemExist(DBWordInfo info)
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

    void ReadInfo(DBWordInfo info, SQLiteQuery rd)
    {
        info.id = rd.GetString(KEY_id);
        info.word = rd.GetString(KEY_word);
        info.pinyin = rd.GetString(KEY_pinyin);
        info.zuci = rd.GetString(KEY_zuci);
        info.bushou = rd.GetString(KEY_bushou);
        info.bihua = rd.GetString(KEY_bihua);
        info.audio = rd.GetString(KEY_audio);
        info.gif = rd.GetString(KEY_gif);
        info.mean = rd.GetString(KEY_mean);
        info.date = rd.GetString(KEY_date);
        info.addtime = rd.GetString(KEY_addtime);
        // info.pinyin = rd.GetString(KEY_pinyin);
        // Debug.Log("ReadInfo info.pinyin=" + info.pinyin);
        /* 

        */
        //  info.addtime = rd.GetString(KEY_addtime);
        // info.date = rd.GetString(KEY_date);
    }


    public List<DBWordInfo> GetAllItem()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT id from " + TABLE_NAME + " order by addtime desc";

        List<DBWordInfo> listRet = new List<DBWordInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllItem start read");
        while (reader.Step())// 循环遍历数据 
        {
            Debug.Log("GetAllItem reading");
            DBWordInfo info = new DBWordInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        Debug.Log("GetAllItem:" + listRet.Count);
        return listRet;
    }

    public List<DBWordInfo> GetAllDate()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT date from " + TABLE_NAME + " order by addtime desc";

        List<DBWordInfo> listRet = new List<DBWordInfo>();
        OpenDB();
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            DBWordInfo info = new DBWordInfo();
            info.date = reader.GetString("date");
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

    public List<DBWordInfo> GetItemByDate(string date)
    {
        string strsql = "select * from " + TABLE_NAME + " where date = '" + date + "'" + "order by addtime desc";
        List<DBWordInfo> listRet = new List<DBWordInfo>();
        OpenDB();

        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            DBWordInfo info = new DBWordInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }


    public List<DBWordInfo> GetItemById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'" + "order by addtime desc";
        List<DBWordInfo> listRet = new List<DBWordInfo>();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            DBWordInfo info = new DBWordInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }



    public DBWordInfo GetItem2(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'";
        dbManager = new SimpleSQLManager2();
        dbManager.databaseFile = (TextAsset)Resources.Load("Item");
        dbManager.Awake();
        List<DBWordInfo> listRet = dbManager.Query<DBWordInfo>(strsql);
        DBWordInfo info = new DBWordInfo();

        foreach (DBWordInfo dbinfo in listRet)
        {
            info.id = dbinfo.id;
            info.word = dbinfo.word;
            info.pinyin = dbinfo.pinyin;
            info.zuci = dbinfo.zuci;
            info.bushou = dbinfo.bushou;
            info.bihua = dbinfo.bihua;
            info.audio = dbinfo.audio;
            info.gif = dbinfo.gif;
            info.mean = dbinfo.mean;
            break;
        }
        return info;
    }

    public DBWordInfo GetItem(string id)
    {
        //where 查询中文会异常
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'";
        //List<DBWordInfo> listRet = new List<DBWordInfo>();
        DBWordInfo info = new DBWordInfo();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        int count = 0;
        while (reader.Step())// 循环遍历数据 
        {
            ReadInfo(info, reader);
            count++;
            break;
            //listRet.Add(info);
        }
        if (count == 0)
        {
            Debug.Log(" not find id=" + id);
        }
        reader.Release();

        CloseDB();
        return info;
    }
}

