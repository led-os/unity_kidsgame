using UnityEngine;
using System.Collections;

public class Sql
{
    static private Sql _main = null;
    public static Sql main
    {
        get
        {
            if (_main == null)
            {
                _main = new Sql();
                _main.Init();
            }
            return _main;
        }
    }

    public SqlBasePlatformWrapper platform
    {
        get
        {
#if UNITY_ANDROID && !UNITY_EDITOR
				return new SqlAndroidWrapper();
#elif UNITY_IPHONE && !UNITY_EDITOR
				return new SqliOSWrapper();
#else
            return new SqlBasePlatformWrapper();
#endif
        }
    }

    public void Init()
    {

    }

    public void OpenDB(string dbfile)
    {
        this.platform.OpenDB(dbfile);
    }

    public void CloseDB()//关闭数据库连接

    {
        this.platform.CloseDB();
    }

    //执行查询
    public void Query(string sql)
    {
        this.platform.Query(sql);
    }



    public void ReadFullTable(string tableName)//读取整个表

    {
 this.platform.ReadFullTable(tableName);
    }



    public void Insert(string tableName, string[] values)//在表中插入数据

    {
this.platform.Insert(tableName,values);

    }



    public void Update(string tableName, string[] cols, string colsValues, string selectKey, string selectValue)//替换表中数据

    {


    }



    public void Delete(string tableName, string[] cols, string[] colsvalues)//删除表中数据

    {
// this.platform.Delete(tableName,values);

    }




    public void DeleteTable(string tableName)//删除表

    {
      this.platform.DeleteTable(tableName);

    }



    //select count(*)  from sqlite_master where type='table' and name = 'yourtablename';
    public bool IsExitTable(string name)//创建表

    {
        bool ret = false;
        // //string query = "select count(*)  from sqlite_master where type='table' and name = '" + name + "'";
        // string query = "select * from sqlite_master where type='table' and name = '" + name + "'";
        // SQLiteQuery rd = Query(query);
        // //int count = rd.GetCount();//rd.GetInteger("0");
        // int count = 0;
        // while (rd.Step())
        // {
        //     count++;
        // }

        // Debug.Log("IsExitTable:count=" + count);
        // if (count > 0)
        // {
        //     ret = true;
        // }
        // rd.Release();
        return ret;
    }
    public void CreateTable(string name, string[] col, string[] colType)//创建表

    {
        if (col.Length != colType.Length)
        {
            // return null;
            //throw new SqliteException("columns.Length != colType.Length")

        }

        string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i] + " " + colType[i];

        }

        query += ")";

        Query(query);



    }



    public void SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)//集成所有操作后执行

    {
        if (col.Length != operation.Length || operation.Length != values.Length)
        {

        }

        string query = "SELECT " + items[0];
        for (int i = 1; i < items.Length; ++i)
        {
            query += ", " + items[i];

        }
        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
        for (int i = 1; i < col.Length; ++i)
        {
            query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";

        }
        Query(query);

    }


}

