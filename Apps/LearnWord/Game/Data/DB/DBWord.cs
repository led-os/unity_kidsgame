using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class DBWord 
{
    public const string WORD_TABLE_NAME = "table_word";
    DBToolSqliteKit dbTool;
    public string dbFileName;
    public string dbFilePath
    {
        get
        {

            string appDBPath = Application.temporaryCachePath + "/" + dbFileName;
            Debug.Log(appDBPath);
            //temporaryCachePath streamingAssetsPath
            // if (Application.platform == RuntimePlatform.WindowsEditor)//如果是windows编辑器中

            // {

            //     appDBPath = Application.dataPath + "//" + dbfilename;

            // }
            // else if (Application.platform == RuntimePlatform.Android)//--如果是安卓平台

            // {

            //     appDBPath = Application.persistentDataPath + "/" + dbfilename;

            //     // if (!File.Exists(appDBPath))

            //     // {

            //     //     WWW loader = new WWW("jar:file://" + Application.dataPath + "/" + connectionString);//把数据库复制到安卓可写路径中，注：sqlite不能在安装包中读取数据

            //     //     File.WriteAllBytes(appDBPath, loader.bytes);

            //     // }

            // }
            return appDBPath;
        }
    }
    static DBWord _main = null;
    static bool isInited = false;
    public static DBWord main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBWord();
                Debug.Log("DBWord main init");
                _main.dbFileName = "DBWord.sqlite";
                _main.CreateDb();
            }
            return _main;
        }


        //ng:
        //  get
        // {
        //     if (_main==null)
        //     {
        //         _main = new DBWord();
        //         Debug.Log("DBWord main init");
        //         _main.CreateDb();
        //     }
        //     return _main;
        // }
    }


    static DBWord _mainFreeWrite = null;
    public static DBWord mainFreeWrite//自由写字
    {
        get
        {
            if (_mainFreeWrite == null)
            {
                _mainFreeWrite = new DBWord();
                _mainFreeWrite.dbFileName = "DBWord_freewrite.sqlite";
                Debug.Log("DBWord _mainFreeWrite init");
                _mainFreeWrite.CreateDb();
            }
            return _mainFreeWrite;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void CreateDb()
    {
        dbTool = new DBToolSqliteKit();
        OpenDB();
        string[] item_col = new string[] { "id", "filesave", "date", "addtime" };
        string[] item_coltype = new string[] { "text", "text", "text", "text" };

        if (!dbTool.IsExitTable(WORD_TABLE_NAME))
        {
            dbTool.CreateTable(WORD_TABLE_NAME, item_col, item_coltype);
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
        string strsql = "select id from " + WORD_TABLE_NAME + " order by addtime desc";
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(WORD_TABLE_NAME);//
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
        string dir = UIGameXieHanzi.strSaveWordShotDir;
        //Directory.Delete(dir);
        DirectoryInfo TheFolder = new DirectoryInfo(dir);

        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            NextFile.Delete();

        }
        OpenDB();
        dbTool.DeleteContents(WORD_TABLE_NAME);
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
    public void AddItem(WordItemInfo info)
    {
        OpenDB();
        string[] values = new string[4];
        //id,filesave,date,addtime
        values[0] = info.id;
        values[1] = info.fileSaveWord;
        //values[2] = info.id;
        //values[3] = info.id;
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        string str = year + "." + month + "." + day;
        Debug.Log("date:" + str);
        values[2] = str;
        long time_ms = Common.GetCurrentTimeMs();//GetCurrentTimeSecond
        values[3] = time_ms.ToString();
        dbTool.InsertInto(WORD_TABLE_NAME, values);

        CloseDB();
        //  GetItemsByWord();


    }

    void ReadInfo(WordItemInfo info, SQLiteQuery rd)
    {
        Debug.Log("ReadInfo");
        string id = rd.GetString("id");
        Debug.Log(id);
        string filesave = rd.GetString("filesave");
        // Debug.Log(filesave);
        string date = rd.GetString("date");
        //  Debug.Log(date);
        string addtime = rd.GetString("addtime");
        //  Debug.Log(addtime);
        info.id = id;
        info.fileSaveWord = filesave;
        info.addtime = addtime;
        info.date = date;
    }

    public void DeleteItem(WordItemInfo info)
    {
        OpenDB();
        string strsql = "DELETE FROM " + WORD_TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        dbTool.ExecuteQuery(strsql, true);
        CloseDB();
    }

    public List<WordItemInfo> GetAllWord()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT id from " + WORD_TABLE_NAME + " order by addtime desc";

        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(WORD_TABLE_NAME);//
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllWord start read");
        while (reader.Step())// 循环遍历数据 
        {
            Debug.Log("GetAllWord reading");
            WordItemInfo info = new WordItemInfo();
            info.id = reader.GetString("id");
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        Debug.Log("GetAllWord:" + listRet.Count);
        return listRet;
    }

    public List<WordItemInfo> GetAllDate()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT date from " + WORD_TABLE_NAME + " order by addtime desc";

        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            WordItemInfo info = new WordItemInfo();
            info.date = reader.GetString("date");
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

    public List<WordItemInfo> GetItemsOfDate(string date)
    {
        string strsql = "select * from " + WORD_TABLE_NAME + " where date = '" + date + "'" + "order by addtime desc";
        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();

        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            WordItemInfo info = new WordItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }


    public List<WordItemInfo> GetItemsOfWord(string word)
    {
        string strsql = "select * from " + WORD_TABLE_NAME + " where id = '" + word + "'" + "order by addtime desc";
        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            WordItemInfo info = new WordItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

}
