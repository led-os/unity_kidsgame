using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class DBColor
{
    public const string WORD_TABLE_NAME = "table_color";
    DBToolSqliteKit dbTool;
    static public string strSaveColorDir//字截图保存目录
    {
        get
        {
            return Application.temporaryCachePath + "/color";
        }
    }
    public string dbFilePath
    {
        get
        {
            string dbfilename = "DBColor.sqlite";
            string appDBPath = Application.temporaryCachePath + "/" + dbfilename;
            Debug.Log(appDBPath);
            return appDBPath;
        }
    }
    static DBColor _main = null;
    static bool isInited = false;
    public static DBColor main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBColor();
                Debug.Log("DBColor main init");
                _main.CreateDb();
            }
            return _main;
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
        string[] item_col = new string[] { "id", "pic", "filesave", "date", "addtime" };
        string[] item_coltype = new string[] { "text", "text", "text", "text", "text" };

        if (!dbTool.IsExitTable(WORD_TABLE_NAME))
        {
            dbTool.CreateTable(WORD_TABLE_NAME, item_col, item_coltype);
        }

        CloseDB();
    }

    void OpenDB()
    {
        dbTool.OpenDB(dbFilePath);

    }

    void CloseDB()
    {
        dbTool.CloseDB();
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
    public void ClearDB()
    {
        //
        string dir = DBColor.strSaveColorDir;
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

    public void DeleteItem(ColorItemInfo info)
    {
        OpenDB();
        string strsql = "DELETE FROM " + WORD_TABLE_NAME + " WHERE id = '" + info.id + "'";
        dbTool.ExecuteQuery(strsql, true);
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

    //更新时间信息
    public void UpdateItemTime(ColorItemInfo info)
    {
        OpenDB();
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        string strDate = year + "." + month + "." + day;
        long time_sconde = Common.GetCurrentTimeSecond();
        string strTime = time_sconde.ToString();

        string strsql = "UPDATE " + WORD_TABLE_NAME + " SET date = '" + strDate + "', " + "addtime = '" + strTime + "'" + " where id  = '" + info.id + "'";
        //string strsql = "update " + WORD_TABLE_NAME + " set addtime = '" + strTime + "'"+ " where id  = '" + info.id + "'";
        dbTool.ExecuteQuery(strsql, true);

        CloseDB();
    }
    public void AddItem(ColorItemInfo info)
    {
        OpenDB();
        string[] values = new string[5];
        //id,filesave,date,addtime
        values[0] = info.id;
        values[1] = info.pic;
        values[2] = info.fileSave;
        //values[2] = info.id;
        //values[3] = info.id;
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        string str = year + "." + month + "." + day;
        Debug.Log("date:" + str);
        values[3] = str;
        long time_sconde = Common.GetCurrentTimeSecond();
        values[4] = time_sconde.ToString();
        dbTool.InsertInto(WORD_TABLE_NAME, values);

        CloseDB();
        //  GetItemsByWord();


    }

    void ReadInfo(ColorItemInfo info, SQLiteQuery rd)
    {
        Debug.Log("ReadInfo");
        string id = rd.GetString("id");
        Debug.Log(id);
        info.pic = rd.GetString("pic");
        string filesave = rd.GetString("filesave");
        // Debug.Log(filesave);
        string date = rd.GetString("date");
        //  Debug.Log(date);
        string addtime = rd.GetString("addtime");
        //  Debug.Log(addtime);
        info.id = id;
        info.fileSave = filesave;
        info.addtime = addtime;
        info.date = date;
    }


    public bool IsItemExist(ColorItemInfo info)
    {
        string strsql = "select * from " + WORD_TABLE_NAME + " where id  = '" + info.id + "'";
        Debug.Log(strsql);
        int count = 0;
        OpenDB();

        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            count++;
        }

        reader.Release();

        CloseDB();
        bool ret = false;
        if (count > 0)
        {
            ret = true;
        }
        return ret;

    }


    public List<ColorItemInfo> GetAllItems()
    {
        string strsql = "select * from " + WORD_TABLE_NAME + " order by addtime desc";
        List<ColorItemInfo> listRet = new List<ColorItemInfo>();
        OpenDB();

        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            ColorItemInfo info = new ColorItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }



}
