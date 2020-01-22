using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class GameLevelParse : LevelParseBase
{

    public const string PLACE_MATH = "Math";
    public List<object> listBg;
    public List<object> listShape;
    public List<object> listColor;

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




    public ShapeColorItemInfo GetItemInfo()
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
        ShapeColorItemInfo info = listGuanka[idx] as ShapeColorItemInfo;
        return info;
    }



    public override int GetGuankaTotal()
    {
        int count = ParseGuanka();
        return count;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
        if (listShape != null)
        {
            listShape.Clear();
        }
        if (listColor != null)
        {
            //listColor.Clear();
        }
    }

    void ParseBgList(byte[] data)
    {
        if ((listBg != null) && (listBg.Count != 0))
        {
            return;
        }
        listBg = new List<object>();
        string json = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["list"];
        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            string strdir = Common.GAME_RES_DIR + "/image_bg";
            info.pic = strdir + "/" + (string)item["pic"];
            info.listColorFilter = new List<object>();
            JsonData colorFilter = item["color_filter"];
            for (int j = 0; j < colorFilter.Count; j++)
            {
                JsonData itemtmp = colorFilter[j];
                ShapeColorItemInfo infotmp = new ShapeColorItemInfo();
                infotmp.id = (string)itemtmp["color_id"];
                info.listColorFilter.Add(infotmp);

            }
            listBg.Add(info);
        }
    }
    public void ParseShape(byte[] data)
    {
        if ((listShape != null) && (listShape.Count != 0))
        {
            return;
        }
        if (listGuanka == null)
        {
            listGuanka = new List<object>();
        }
        listShape = new List<object>();
        int idx = LevelManager.main.placeLevel;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);
        string json = Encoding.UTF8.GetString(data);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string strPlace = infoPlace.id;
        JsonData items = null;
        string key = "list";
        if (Common.JsonDataContainsKey(root, key))
        {
            items = root[key];
        }
        else
        {
            items = root["items"];
        }

        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            info.id = (string)item["id"];
            //string picdir = Common.GAME_RES_DIR + "/image/" + info.id;
            string picdir = Common.GAME_RES_DIR + "/image/" + strPlace + "/" + info.id;
            info.isMath = true;
            if (strPlace != PLACE_MATH)
            {
                info.isMath = false;
                picdir = Common.GAME_RES_DIR + "/image/" + strPlace;
            }
            info.pic = picdir + "/" + info.id + ".png";
            info.picInner = picdir + "/" + info.id + "_inner.png";
            info.picOuter = picdir + "/" + info.id + "_outer.png";
            if (strPlace != PLACE_MATH)
            {
                info.picInner = info.pic;
                info.picOuter = info.pic;
            }
            listShape.Add(info);
            listGuanka.Add(info);
        }

    }

    public void ParseColor(byte[] data)
    {
        if ((listColor != null) && (listColor.Count != 0))
        {
            return;
        }

        listColor = new List<object>();
        int idx = LevelManager.main.placeLevel;
        string json = Encoding.UTF8.GetString(data);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["list"];
        for (int i = 0; i < items.Count; i++)
        {
            ShapeColorItemInfo info = new ShapeColorItemInfo();
            JsonData item = items[i];
            info.id = (string)item["id"];
            info.color = Common.RGBString2Color((string)item["color"]);
            listColor.Add(info);
        }

    }


    public override int ParseGuanka()
    {
        Debug.Log("ParseGuanka UIGameShapeColor");

        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        //ParseBgList();
        {
            string filepath = Common.GAME_RES_DIR + "/image_bg/bg.json";
            byte[] data = FileUtil.ReadDataAuto(filepath);
            ParseBgList(data);

        }

        //  ParseShape();
        {
            int idx = LevelManager.main.placeLevel;
            string filepath = Common.GAME_RES_DIR + "/guanka/item_" + infoPlace.id + ".json";
            byte[] data = FileUtil.ReadDataAuto(filepath);
            ParseShape(data);
        }
        // ParseColor();
        {

            string filepath = Common.GAME_RES_DIR + "/guanka/color.json";
            byte[] data = FileUtil.ReadDataAuto(filepath);
            ParseColor(data);

        }
        int count = 0;
        if (listShape != null)
        {
            count = GameShapeColor.GUANKA_NUM_PER_ITEM * listShape.Count;
        }
        return count;

    }


}
