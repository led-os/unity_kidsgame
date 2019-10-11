
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRes
{
    public const string GAME_XIEHOUYU = "xiehouyu";
    public const string GAME_IDIOM = "chengyu";
    public const string GAME_POEM = "poem";

    public const string GAME_RIDDLE = "Riddle";
    public const string GAME_IdiomConnect = "IdiomConnect";
    //type 
    public const string GAME_TYPE_IMAGE = "Image";
    public const string GAME_TYPE_TEXT = "Text";
    public const string GAME_TYPE_IMAGE_TEXT = "ImageText";
    public const string GAME_TYPE_CONNECT = "Connect";//接龙
    //color
    //f88816 248,136,22
    static private GameRes _main = null;
    public static GameRes main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameRes();
            }
            return _main;
        }
    }
    public Color32 colorTitle
    {
        get
        {
            Color32 cr = new Color32(255, 255, 255, 255);
            if (Common.appKeyName == GAME_RIDDLE)
            {
                cr = new Color32(89, 45, 6, 255);
            }
            return cr;
        }
    }

    public Color32 colorGameText
    {
        get
        {
            Color32 cr = new Color32(0, 0, 0, 255);
            if (Common.appKeyName == GAME_RIDDLE)
            {
                cr = new Color32(255, 255, 255, 255);
            }
            return cr;
        }
    }

    public Color32 colorGameWinTitle
    {
        get
        {
            Color32 cr = new Color32(192, 90, 59, 255);
            if (Common.appKeyName == GAME_RIDDLE)
            {
                cr = new Color32(255, 255, 255, 255);
            }
            return cr;
        }
    }
    public Color32 colorGameWinTextView
    {
        get
        {
            Color32 cr = new Color32(192, 90, 59, 255);
            if (Common.appKeyName == GAME_RIDDLE)
            {
                cr = new Color32(89, 45, 6, 255);
            }
            return cr;
        }
    }

}
