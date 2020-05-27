
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class ImageRes
{
    //color
    //f88816 248,136,22
    JsonData rootJson;

    ImageResInternal imageResApp;
    ImageResInternal imageResCommon;
    static private ImageRes _main = null;
    public static ImageRes main
    {
        get
        {
            if (_main == null)
            {
                _main = new ImageRes();

                _main.imageResCommon = new ImageResInternal();
                _main.imageResCommon.Init(Common.RES_CONFIG_DATA_COMMON + "/Image/ImageRes.json");

                _main.imageResApp = new ImageResInternal();
                _main.imageResApp.Init(Common.RES_CONFIG_DATA + "/Image/ImageRes.json");

            }
            return _main;
        }
    }

    public bool IsHasBoard(string key)
    {
        bool ret = false;
        if (imageResApp.IsHasKey(key))
        {
            ret = imageResApp.IsHasBoard(key);
        }
        else
        {
            ret = imageResCommon.IsHasBoard(key);
        }

        return ret;
    }
    public string GetImage(string key)
    {
        string ret = "";
        if (imageResApp.IsHasKey(key))
        {
            ret = imageResApp.GetImage(key);
        }
        else
        {
            ret = imageResCommon.GetImage(key);
        }
        return ret;
    }



    public Vector4 GetImageBoard(string key)
    {
        Vector4 ret = Vector4.zero;
        if (imageResApp.IsHasKey(key))
        {
            ret = imageResApp.GetImageBoard(key);
        }
        else
        {
            ret = imageResCommon.GetImageBoard(key);
        }

        return ret;
    }
}
