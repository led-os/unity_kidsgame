
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using LitJson;
//必应美图
//https://bing.lylares.com/
//code:https://github.com/lylares/bingima

public interface INetImageParseDelegate
{
    void OnNetImageParseDidParseSortList(NetImageParse parse, bool isFail,List<object> list);
    void OnNetImageParseDidParseImageList(NetImageParse parse, bool isFail,List<object> list);
}
public class NetImageParse
{

    public INetImageParseDelegate iDelegate;
    HttpRequest httpSortList;
    HttpRequest httpImageList;

    // static private NetImageParse _main = null;
    // public static NetImageParse main
    // {
    //     get
    //     {
    //         if (_main == null)
    //         {
    //             _main = new NetImageParse();
    //         }
    //         return _main;
    //     }
    // }

    public void StartParseSortList()
    {
        httpSortList = new HttpRequest(OnHttpRequestFinished);
        httpSortList.Get("https://itunes.apple.com/lookup?id=914391781");
    }

    public void StartParseImageList(ItemInfo info)
    {
        httpImageList = new HttpRequest(OnHttpRequestFinished);
        httpImageList.Get("https://itunes.apple.com/lookup?id=914391781");
    }

    public void ParseSortList(byte[] data)
    {
        string str = Encoding.UTF8.GetString(data);
        Debug.Log(str);
        JsonData jsonRoot = JsonMapper.ToObject(str);
        JsonData jsonResult = jsonRoot["results"];
        //Debug.Log("Appversion ParseAppstore 1");
        JsonData jsonItem = jsonResult[0];
        string url = (string)jsonItem["trackViewUrl"];

        if (iDelegate != null)
        {
            iDelegate.OnNetImageParseDidParseSortList(this, false);
        }
    }

    public void ParseImageList(byte[] data)
    {
        if (iDelegate != null)
        {
            iDelegate.OnNetImageParseDidParseImageList(this, false);
        }
    }

    public void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("Appversion OnHttpRequestFinished:isSuccess=" + isSuccess);
        if (httpSortList == req)
        {
            if (isSuccess)
            {
                ParseSortList(data);
            }
            else
            {
                if (iDelegate != null)
                {
                    iDelegate.OnNetImageParseDidParseSortList(this, true);
                }
            }

        }
        if (httpImageList == req)
        {
            if (isSuccess)
            {
                ParseImageList(data);
            }
            else
            {
                if (iDelegate != null)
                {
                    iDelegate.OnNetImageParseDidParseImageList(this, true);
                }
            }
        }



    }

}

