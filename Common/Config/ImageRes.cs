
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class ImageRes
{
    //color
    //f88816 248,136,22
    JsonData rootJson;
    static private ImageRes _main = null;
    public static ImageRes main
    {
        get
        {
            if (_main == null)
            {
                _main = new ImageRes();
                string filePath = Common.RES_CONFIG_DATA + "/Image/ImageRes.json";
                _main.Init(filePath);
            }
            return _main;
        }
    }

    void Init(string filePath)
    {
        string json = FileUtil.ReadStringAuto(filePath);
        rootJson = JsonMapper.ToObject(json);
    }

    // 255,100,200,255 to color return Vector4 47,47,47,255
    //Vector4 (left,right,top,bottom)
    Vector4 String2Vec4(string str)
    {
        float x, y, z, w;
        string[] rgb = str.Split(',');
        x = Common.String2Int(rgb[0]);
        y = Common.String2Int(rgb[1]);
        z = Common.String2Int(rgb[2]);
        w = Common.String2Int(rgb[3]);
        return new Vector4(x, y, z, w);
    }
    string GetBoardKey(string key)
    {
        return key + "_BOARD";
    }

    //9宫格图片边框参数 (left,right,top,bottom)
    //cc.Vec4 (left,right,top,bottom)
    Vector4 GetImageBoard(string key)
    {
        var str = JsonUtil.JsonGetString(rootJson, GetBoardKey(key), "");
        return String2Vec4(str);
    }

    public string GetImage(string key)
    {
        return JsonUtil.JsonGetString(rootJson, key, "");
    }
}
