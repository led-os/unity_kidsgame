using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class PoemContentInfo
{
    public string content;
    public string pinyin;
}


public class CaiCaiLeItemInfo : ItemInfo
{
    public string author;
    public string year;
    public string style;
    public string album;
    public string intro;
    public string translation;
    public string appreciation;
    public List<PoemContentInfo> listPoemContent;
}
public class GameGuankaParse : GuankaParseBase
{

    public string strWord3500;
    static private GameGuankaParse _main = null;
    public static GameGuankaParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameGuankaParse();
            }
            return _main;
        }
    }

    public bool OnlyTextGame()
    {
        bool ret = false;
        if (Common.appKeyName == "poem")
        {
            ret = true;
        }
        return ret;
    }
    public CaiCaiLeItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel;
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        CaiCaiLeItemInfo info = listGuanka[idx] as CaiCaiLeItemInfo;
        return info;
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
            info.id = (string)item["id"];
            //string str = "aa";// = languageGame.GetString(info.id);
            //Debug.Log(i + ":ParseGame:" + str);
            info.pic = Common.GAME_RES_DIR + "/image/" + strPlace + "/" + info.id + ".png";
            info.icon = Common.GAME_RES_DIR + "/image_thumb/" + strPlace + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = info.pic;
            }
            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        //word3500
        filepath = Common.GAME_DATA_DIR + "/words_3500.json";
        json = FileUtil.ReadStringAsset(filepath);
        root = JsonMapper.ToObject(json);
        strWord3500 = (string)root["words"];
        Debug.Log(strWord3500);

        Debug.Log("ParseGame::count=" + count);
        return count;
    }

    //诗词
    public void ParsePoemItem(CaiCaiLeItemInfo info)
    {
        string filepath = Common.GAME_RES_DIR + "/guanka/" + info.id + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            return;
        }
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath); 
        JsonData root = JsonMapper.ToObject(json);
        info.title = (string)root["title"];
        info.author = (string)root["author"];
        info.year = (string)root["year"];
        info.style = (string)root["style"];
        info.album = (string)root["album"];
        info.url = (string)root["url"];
        info.intro = (string)root["intro"];
        info.translation = (string)root["translation"];
        info.appreciation = (string)root["appreciation"];

        JsonData itemPoem = root["poem"];
        info.listPoemContent = new List<PoemContentInfo>();
        for (int i = 0; i < itemPoem.Count; i++)
        {
            JsonData item = itemPoem[i];
            PoemContentInfo infoPoem = new PoemContentInfo();
            infoPoem.content = (string)item["content"];
            infoPoem.pinyin = (string)item["pinyin"];
            info.listPoemContent.Add(infoPoem);
        }
    }
}
