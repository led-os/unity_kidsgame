
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBLearnWord : DBBase
{
    public const string KEY_word = "word";
    public const string KEY_pinyin = "pinyin";
    public const string KEY_zuci = "zuci";
    public const string KEY_bushou = "bushou";
    public const string KEY_bihua = "bihua";
    public const string KEY_audio = "audio";
    public const string KEY_gif = "gif";
    public const string KEY_mean = "mean";


    string[] item_col = new string[] { KEY_id, KEY_word, KEY_pinyin, KEY_zuci, KEY_bushou, KEY_bihua, KEY_audio, KEY_gif, KEY_mean, };
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
                Debug.Log("DBWord main init");
                _main.TABLE_NAME = "table_word";
                _main.dbFileName = "Word_" + Common.appKeyName + ".db";
                _main.Init();
            }
            return _main;
        }

    }

    public void Init()
    {
        isNeedCopyFromAsset = false;
        if (Application.isEditor || Common.isiOS)
        {
            // CopyDbFileFromResource();
        }
        CreateDb();
        CreateTable(item_col);
    }

    //{ "id", "intro", "album", "translation", "author", "year", "style", "pinyin", "appreciation", "head", "end", "tips", "date", "addtime" };
    public override void AddItem(DBWordItemInfo info)
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

    public override void ReadInfo(DBWordItemInfo info, SqlInfo infosql)
    {
        info.id = dbTool.GetString(infosql, KEY_id);
        info.word = dbTool.GetString(infosql, KEY_word);
        info.pinyin = dbTool.GetString(infosql, KEY_pinyin);
        info.zuci = dbTool.GetString(infosql, KEY_zuci);
        info.bushou = dbTool.GetString(infosql, KEY_bushou);
        info.bihua = dbTool.GetString(infosql, KEY_bihua);
        info.audio = dbTool.GetString(infosql, KEY_audio);
        info.gif = dbTool.GetString(infosql, KEY_gif);
        info.mean = dbTool.GetString(infosql, KEY_mean);



        // info.pinyin = rd.GetString(KEY_pinyin);
        // Debug.Log("ReadInfo info.pinyin=" + info.pinyin);
        /* 

        */
        //  info.addtime = rd.GetString(KEY_addtime);
        // info.date = rd.GetString(KEY_date);
    }


}

