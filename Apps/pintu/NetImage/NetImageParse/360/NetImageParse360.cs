
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using LitJson;
//360 图片API
//https://blog.csdn.net/qq_41113081/article/details/87551942

/* 在线格式化JSON工具
http://tool.oschina.net/codeformat/json/
*/

public class NetImageParse360 : NetImageParseBase
{

    HttpRequest httpSortList;
    HttpRequest httpImageList;

    public override void StartParseSortList()
    {

        string filePath = Common.GAME_RES_DIR + "/netimage/image_channel_360.json";

        string json = FileUtil.ReadStringAsset(filePath);
        ParseSortList(json);
    }
    public override void StartParseImageList(ImageItemInfo info)
    {
        httpImageList = new HttpRequest(OnHttpRequestFinished);
        //http://wallpaper.apc.360.cn/index.php?%20c=WallPaper&a=getAppsByCategory&cid=36&start=0&count=24&from=360chrome
        httpImageList.Get("http://wallpaper.apc.360.cn/index.php?%20c=WallPaper&a=getAppsByCategory&cid=" + info.cid + "&start=0&count=24&from=360chrome");
    }


    public void ParseSortList(string str)
    {
        //string str = Encoding.UTF8.GetString(data);
        // Debug.Log(str);
        JsonData jsonRoot = JsonMapper.ToObject(str);
        JsonData items = jsonRoot["items"];
        List<object> list = new List<object>();
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ImageItemInfo info = new ImageItemInfo();
            info.title = (string)item["cat"];
            info.cid = (string)item["cid"];
            list.Add(info);
        }
        if (iDelegate != null)
        {
            iDelegate.OnNetImageParseDidParseSortList(this, true, list);
        }
    }

    public void ParseImageList(byte[] data)
    {
        string str = Encoding.UTF8.GetString(data);
        // Debug.Log(str);
        JsonData jsonRoot = JsonMapper.ToObject(str);
        string str_total = (string)jsonRoot["total"];
        JsonData items = jsonRoot["data"];
        List<object> list = new List<object>();
        for (int i = 0; i < items.Count; i++)
        {
            /*
              "url": "http://p16.qhimg.com/bdr/__85/t01e693524ee8d4d835.jpg", 
            "url_thumb": "http://p16.qhimg.com/t01e693524ee8d4d835.jpg", 
            "url_mid": "http://p16.qhimg.com/t01e693524ee8d4d835.jpg", 
             "img_1600_900": "http://p16.qhimg.com/bdm/1600_900_85/t01e693524ee8d4d835.jpg", 
            "img_1440_900": "http://p16.qhimg.com/bdm/1440_900_85/t01e693524ee8d4d835.jpg", 
            "img_1366_768": "http://p16.qhimg.com/bdm/1366_768_85/t01e693524ee8d4d835.jpg", 
            "img_1280_800": "http://p16.qhimg.com/bdm/1280_800_85/t01e693524ee8d4d835.jpg", 
            "img_1280_1024": "http://p16.qhimg.com/bdm/1280_1024_85/t01e693524ee8d4d835.jpg", 
            "img_1024_768": "http://p16.qhimg.com/bdm/1024_768_85/t01e693524ee8d4d835.jpg"
             */
            JsonData item = items[i];
            ImageItemInfo info = new ImageItemInfo();
            info.title = (string)item["utag"];
            info.id = (string)item["id"];
            info.pic = (string)item["url"];
            // Debug.Log("ParseImageList url = "+info.url);
            list.Add(info);
        }

        if (iDelegate != null)
        {
            iDelegate.OnNetImageParseDidParseImageList(this, true, list);
        }
    }

    public void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("Appversion OnHttpRequestFinished:isSuccess=" + isSuccess);
        if (httpSortList == req)
        {
            if (isSuccess)
            {
                // ParseSortList(data);
            }
            else
            {

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

            }
        }



    }

}

