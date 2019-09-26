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
    public bool skip;
}

public class AnswerInfo
{
    public int index;
    public bool isFinish;//是否答对
    public string word;//答案
    public bool isFillWord;//是否填了字
    public string wordFill;//实际填充的字
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
    public string pinyin;
    public string head;
    public string end;
    public string tips;

    public List<PoemContentInfo> listPoemContent;
}
public class GameGuankaParse : GuankaParseBase
{

    public const string STR_APPKEYNAME_POEM = "poem";
    public const string STR_APPKEYNAME_CHENGYU = "chengyu";
    public const string STR_APPKEYNAME_XIEHOUYU = "xiehouyu";
    public const string STR_APPKEYNAME_RIDDLE = "Riddle";
    public string strWord3500;
    string[] arrayPunctuation = { "。", "？", "！", "，", "、", "；", "：" };
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
        if (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_POEM)
        {
            ret = true;
        }
        if (Common.appKeyName == STR_APPKEYNAME_RIDDLE)
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
            info.id = JsonUtil.JsonGetString(item, "id", "");
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

    //过滤标点符号 点号：句号（ 。）、问号（ ？）、感叹号（ ！）、逗号（ ，）顿号（、）、分号（；）和冒号（：）。
    public string FilterPunctuation(string str)
    {
        string ret = str;

        foreach (string item in arrayPunctuation)
        {
            ret = ret.Replace(item, "");
        }
        return ret;
    }

    public bool IsPunctuation(string str)
    {
        bool ret = false;

        foreach (string item in arrayPunctuation)
        {
            if (str == item)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    //非标点符号文字
    public List<int> IndexListNotPunctuation(string str)
    {
        List<int> listRet = new List<int>();

        int len = str.Length;
        for (int i = 0; i < len; i++)
        {
            string word = str.Substring(i, 1);
            if (!IsPunctuation(word))
            {
                listRet.Add(i);
            }

        }
        return listRet;
    }

    public void ParseItem(CaiCaiLeItemInfo info)
    {

        if (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_CHENGYU)
        {
            ParseChengyuItem(info);
        }
        if (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_POEM)
        {
            ParsePoemItem(info);
        }
        if (Common.appKeyName == STR_APPKEYNAME_RIDDLE)
        {
            ParseRiddleItem(info);
        }

    }

    public void ParseChengyuItem(CaiCaiLeItemInfo info)
    {
        string filepath = Common.GAME_RES_DIR + "/guanka/data/" + LanguageManager.main.languageGame.GetString(info.id) + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            return;
        }
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        info.title = (string)root["title"];
        info.album = (string)root["album"];
        info.translation = (string)root["translation"];
        info.pinyin = (string)root["pinyin"];
    }

    //诗词
    public void ParsePoemItem(CaiCaiLeItemInfo info)
    {
        string filepath = Common.GAME_RES_DIR + "/guanka/poem/" + info.id + ".json";
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
            bool isSkip = JsonUtil.JsonGetBool(item, "skip", false);
            if (!isSkip)
            {
                info.listPoemContent.Add(infoPoem);
            }
        }
    }

    //谜语
    public void ParseRiddleItem(CaiCaiLeItemInfo info)
    {
    }
}
