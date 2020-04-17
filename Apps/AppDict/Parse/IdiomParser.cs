using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
 
public class IdiomParser
{ 
    public List<object> listPosition;
    public List<object> listCategory;
 

    static private IdiomParser _main = null;
    public static IdiomParser main
    {
        get
        {
            if (_main == null)
            {
                _main = new IdiomParser(); 
            }
            return _main;
        }
    }
 
      public List<object> ParseIdiomList(string category,string sort)
    {
        int count = 1; 

      List<object> list = new List<object>();

        int idx = LevelManager.main.placeLevel;

        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);

        string filepath = Common.GAME_RES_DIR + "/guanka/" + category + "/" + sort + ".json";
        // Debug.Log("ParseGuanka filepath=" + filepath);
        //string filepath = Common.GAME_RES_DIR + "/guanka/first.json";
        //
        //FILE_PATH
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            return list;
        }
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);

        string strPlace = infoPlace.id;
        JsonData items = root["items"];
        count = items.Count;
        for (int i = 0; i < count; i++)
        {
            JsonData item = items[i];
            IdiomItemInfo info = new IdiomItemInfo();
            info.id = JsonUtil.GetString(item, "id", "");
            info.title = JsonUtil.GetString(item, "title", "");
            if (info.title.Length != 4)
            {
                continue;
            }
            info.pronunciation = JsonUtil.GetString(item, "pronunciation", "");
            info.translation = JsonUtil.GetString(item, "translation", "");
            info.album = JsonUtil.GetString(item, "album", "");
            list.Add(info);
        }
     

        return list;
    }

    public void ParseIdiomItem(IdiomItemInfo info)
    {
        IdiomItemInfo idiom = null;
        Debug.Log("Flower UpdateItem ParseItem info.id=" + info.id + " info.pinyin=" + info.pronunciation + " info.title=" + info.title);

        if (Common.BlankString(info.id))
        {
            idiom = DBIdiom.main.GetItemByTitle(info.title);
        }
        else
        {
            idiom = DBIdiom.main.GetItemById(info.id);
        }

        if (Common.BlankString(info.pronunciation))
        {
            info.title = idiom.title;
            info.album = idiom.album;
            info.translation = idiom.translation;
            info.pronunciation = idiom.pronunciation;
            info.id = idiom.id;
        }

    }
    public List<object> ParseCategory()
    {
        int count = 1;
        if ((listCategory != null) && (listCategory.Count != 0))
        {
            return listCategory;
        }
        listCategory = new List<object>();
        string filepath = Common.GAME_RES_DIR + "/guanka/category.json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        count = items.Count;
        for (int i = 0; i < count; i++)
        {
            JsonData item = items[i];
            IdiomItemInfo info = new IdiomItemInfo();
            info.title = JsonUtil.GetString(item, "title", "");
            listCategory.Add(info);
        }
        return listCategory;
    }

    public List<object> ParseSort(string category)
    {
            Debug.Log("ParseSort category=" + category);

        int count = 1;
        List<object> listSort = new List<object>();
        string filepath = Common.GAME_RES_DIR + "/guanka/" + category + "/sort.json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        count = items.Count;
        for (int i = 0; i < count; i++)
        {
            JsonData item = items[i];
            IdiomItemInfo info = new IdiomItemInfo();
            info.title = JsonUtil.GetString(item, "title", "");
            listSort.Add(info);
        }
        return listSort;
    }


}
