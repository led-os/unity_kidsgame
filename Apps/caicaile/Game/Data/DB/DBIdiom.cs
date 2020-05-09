
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DBIdiom : DBBase
{
    // public const string TABLE_NAME = "TableIdiom"; 
    public string[] item_col = new string[] { KEY_id, KEY_title, KEY_pinyin, KEY_album, KEY_translation, KEY_year, KEY_usage,  KEY_common_use, KEY_emotional, KEY_structure,
        KEY_near_synonym, KEY_antonym, KEY_example, KEY_correct_pronunciation
 };
    static DBIdiom _main = null;
    static bool isInited = false;
    public static DBIdiom main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBIdiom();
                Debug.Log("DBIdiom main init");
                _main.TABLE_NAME = "TableIdiom";
                _main.dbFileName = "Idiom.db";
                _main.Init();
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
    public void Init()
    {
        isNeedCopyFromAsset = true;
        if (Application.isEditor)
        {
            CopyDbFileFromResource();
        }
        CreateDb();
        CreateTable(item_col);
    }


    //{ "id", "intro", "album", "translation", "author", "year", "style", "pinyin", "appreciation", "head", "end", "tips", "date", "addtime" };
    public override void AddItem(IdiomItemInfo info)
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





    public override void ReadInfo(IdiomItemInfo info, SqlInfo infosql)
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
        while (true)// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, infosql);
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }
        //   reader.Release();

        CloseDB();
        return listRet;
    }
}

