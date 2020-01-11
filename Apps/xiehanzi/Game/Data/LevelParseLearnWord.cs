using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class LevelParseLearnWord : LevelParseBase
{
    static private LevelParseLearnWord _main = null;
    public static LevelParseLearnWord main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseLearnWord();
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
        long tickJson = Common.GetCurrentTimeMs();
        string fileName = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            WordItemInfo info = new WordItemInfo();
            info.id = (string)item["id"];
            info.pic = GetWordImage(info.id);
            info.word = (string)item["word"];
            info.pinyin = (string)item["pinyin"];
            info.zuci = (string)item["zuci"];
            info.bushou = (string)item["bushou"];
            info.bihua = (string)item["bihua"];
            //info.icon = GameLevelParse.main.GetItemThumb(info.id);
            listGuanka.Add(info);
        }
        count = listGuanka.Count;

        return count;
    }
    public string GetWordImage(string id)
    { 
        string strDirRoot = Common.GAME_RES_DIR + "/image"; 
        string ret = strDirRoot + "/" + id + ".png";
        return ret;
    }
}
