using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LevelParseIdiom : GameLevelParseBase
{ 
    static private LevelParseIdiom _main = null;
    public static LevelParseIdiom main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseIdiom();
            }
            return _main;
        }
    }

    public override int ParseGuanka()
    {
        int count = 0;

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();

        int idx = LevelManager.main.placeLevel;

        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string filepath = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            filepath = Common.GAME_RES_DIR + "/guanka/item_" + infoPlace.id + ".json";
        }

        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);

        JsonData root = JsonMapper.ToObject(json);
        string strPlace = infoPlace.id;
        JsonData items = root["items"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            info.id = JsonUtil.JsonGetString(item, "id", "");
            info.title = JsonUtil.JsonGetString(item, "title", "");
            info.pinyin = JsonUtil.JsonGetString(item, "pronunciation", "");
            info.translation = JsonUtil.JsonGetString(item, "translation", "");
            info.album = JsonUtil.JsonGetString(item, "album", "");
            //string str = "aa";// = languageGame.GetString(info.id);
            //Debug.Log(i + ":ParseGame:" + str);
            info.pic = Common.GAME_RES_DIR + "/image/" + strPlace + "/" + info.id + ".png";
            info.icon = Common.GAME_RES_DIR + "/image_thumb/" + strPlace + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = info.pic;
            }
            string key = "xiehouyu";
            if (JsonUtil.ContainsKey(item, key))
            {
                JsonData xiehouyu = item[key];
                for (int j = 0; j < xiehouyu.Count; j++)
                {
                    JsonData item_xhy = xiehouyu[j];
                    if (j == 0)
                    {
                        info.head = (string)item_xhy["content"];
                    }
                    if (j == 1)
                    {
                        info.end = (string)item_xhy["content"];
                    }
                }

            }

            key = "head";
            if (JsonUtil.ContainsKey(item, key))
            {
                //Riddle
                info.head = (string)item["head"];
                info.end = (string)item["end"];
                info.tips = (string)item["tips"];
                info.type = (string)item["type"];
            }
            info.gameType = GameRes.GAME_TYPE_IMAGE;


            listGuanka.Add(info);
        }

        count = listGuanka.Count;


        Debug.Log("ParseGame::count=" + count);
        return count;
    }


 
    public override void ParseItem(CaiCaiLeItemInfo info)
    {

        
    }
 

     
}
