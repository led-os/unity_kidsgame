﻿using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class GameLevelParse : LevelParseBase
{

    LevelParseBase levelParse;

    static private GameLevelParse _main = null;
    public static GameLevelParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameLevelParse();
            }
            return _main;
        }
    }


    public override ItemInfo GetGuankaItemInfo(int idx)
    {
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ItemInfo info = listGuanka[idx] as ItemInfo;
        return info;
    }

    public WordItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel;
        return GetGuankaItemInfo(idx) as WordItemInfo;
    }

    public override int GetGuankaTotal()
    {
        ParseGuanka();
        if (listGuanka != null)
        {
            return listGuanka.Count;
        }
        return 0;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
    }
    public string GetItemThumb(string id)
    {
        string strDirRoot = Common.GAME_RES_DIR;
        string strDirRootImage = strDirRoot + "/image/" + id;
        //thumb
        string ret = strDirRootImage + "/" + id + "_thumb.png";

        return ret;
    }
    public override int ParseGuanka()
    {
        int count = 0;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        if (infoPlace.id == GameRes.GAME_ID_Xiehanzi)
        {
            levelParse = LevelParseXieHanZi.main;
        }
        if (infoPlace.id == GameRes.GAME_ID_LearnWord)
        {
            levelParse = LevelParseLearnWord.main;
        }

        if (levelParse != null)
        {
            levelParse.ParseGuanka();
            listGuanka = levelParse.listGuanka;
        }
        count = listGuanka.Count;
        return count;
    }
}
