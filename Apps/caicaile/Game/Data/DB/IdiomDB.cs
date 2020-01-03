
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IdiomItemInfo
{
    public string title; //成语名字：
    public string album; //成语出处： 
    public string url;
    public string translation; //成语解释：
    public string pronunciation; //成语发音： 
    public string id; //简拼 ZWJY
    public string year;
    public string usage; //成语用法
    public string common_use;  //常用程度
    public string emotional;  //感情色彩
    public string structure; //成语结构：
    public string near_synonym; //近义词 near-synonym
    public string antonym; //反义词：Antonym:
    public string example; //成语例句：
    public string correct_pronunciation;  //成语正音：别，不能读作“biè”

    public string date;

}

public class IdiomDB
{
    public const string TABLE_NAME = "TableIdiom";
    DBToolSqliteKit dbTool;
    public string dbFileName;

    public const string KEY_id = "id";
    public const string KEY_title = "title";
    public const string KEY_translation = "translation";
    public const string KEY_date = "date";
    public const string KEY_addtime = "addtime";



    public const string KEY_album = "album";
    public const string KEY_pinyin = "pronunciation";
    public const string KEY_year = "year";

    public const string KEY_usage = "usage";
    public const string KEY_common_use = "common_use";

    public const string KEY_emotional = "emotional";
    public const string KEY_structure = "structure";
    public const string KEY_near_synonym = "near_synonym";  //近义词
    public const string KEY_antonym = "antonym"; //反义词  
    public const string KEY_example = "example";
    public const string KEY_correct_pronunciation = "correct_pronunciation"; //成语正音：别，不能读作“biè”


    public const string KEY_text = "text";


    string[] item_col = new string[] { KEY_id, KEY_title, KEY_pinyin, KEY_album, KEY_translation, KEY_year, KEY_usage,  KEY_common_use, KEY_emotional, KEY_structure,
        KEY_near_synonym, KEY_antonym, KEY_example, KEY_correct_pronunciation
 };
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
    static IdiomDB _main = null;
    static bool isInited = false;
    public static IdiomDB main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new IdiomDB();
                Debug.Log("IdiomDB main init");
                _main.dbFileName = "Idiom.db";
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
        string src = Common.GAME_RES_DIR + "/Idiom.db";
        string dst = dbFilePath;
        if (!FileUtil.FileIsExist(dst))
        {
            byte[] data = FileUtil.ReadDataAsset(src);
            System.IO.File.WriteAllBytes(dst, data);
        }

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
        info.title = rd.GetString(KEY_title);
        info.pronunciation = rd.GetString(KEY_pinyin);
        info.album = rd.GetString(KEY_album);
        info.translation = rd.GetString(KEY_translation);

        info.year = rd.GetString(KEY_year);
        info.usage = rd.GetString(KEY_usage);
        info.common_use = rd.GetString(KEY_common_use);
        info.emotional = rd.GetString(KEY_emotional);
        info.structure = rd.GetString(KEY_structure);

        info.near_synonym = rd.GetString(KEY_near_synonym);
        info.antonym = rd.GetString(KEY_antonym);
        info.example = rd.GetString(KEY_example);
        info.correct_pronunciation = rd.GetString(KEY_correct_pronunciation);


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



    public IdiomItemInfo GetItemByTitle(string title)
    {
        string strsql = "select * from " + TABLE_NAME + " where title = '" + title + "'";
        //List<IdiomItemInfo> listRet = new List<IdiomItemInfo>();
        IdiomItemInfo info = new IdiomItemInfo();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            ReadInfo(info, reader);
            break;
            //listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return info;
    }
}

