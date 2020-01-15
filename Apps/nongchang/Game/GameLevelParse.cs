using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class GameLevelParse : LevelParseBase
{

    public const int GUANKA_ITEM_NUM = 5;
    public string strPicBg;
    AutoMakeGuanka autoMakeGuanka;

    public List<object> listAutoGuanka;//item index list
    public List<object> listMapItem;
    public List<object> listPoint;
    List<object> listGuankaItem;//image id
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

    public NongChangItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel;
        return GetGuankaItemInfo(idx) as NongChangItemInfo;
    }

    public override int GetGuankaTotal()
    {
        return ParseGuanka();
    }

    public override void CleanGuankaList()
    {
        Debug.Log("CleanGuankaList nongchang");
        if (listGuanka != null)
        { 
            listGuanka.Clear(); 
        }
        if (listMapItem != null)
        {
            listMapItem.Clear();
        }
        if (listPoint != null)
        {
            listPoint.Clear();
        }
        if (listGuankaItem != null)
        {
            listGuankaItem.Clear();
        }
    }

    public override int ParseGuanka()
    {
        int count = 0;
        CleanGuankaList();
        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            Debug.Log("ParseGuanka nongchang is not null count=" + listGuanka.Count);
            return listGuanka.Count;
        }

        if (autoMakeGuanka == null)
        {
            // autoMakeGuanka = this.gameObject.AddComponent<AutoMakeGuanka>();
            autoMakeGuanka = new AutoMakeGuanka();
            autoMakeGuanka.Init();
        }
        if (listAutoGuanka == null)
        {
            listAutoGuanka = autoMakeGuanka.ParseAutoGuankaJson();
        }


        listGuankaItem = new List<object>();

        listGuanka = new List<object>();
        int idx = LevelManager.main.placeLevel;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);

        string fileName = Common.GAME_RES_DIR + "/guanka/item_" + infoPlace.id + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName); //((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
                                                          // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string type = (string)root["type"];
        int total_map = 6;
        string mapid = "map_" + LevelManager.main.gameLevel % total_map;//(string)root["map_id"];
        string picRoot = Common.GAME_RES_DIR + "/image/" + type + "/";

        //search_items
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            NongChangItemInfo info = new NongChangItemInfo();
            info.id = (string)item["id"];
            info.pic = picRoot + info.id + ".png";
            listGuankaItem.Add(info);
        }


        //让总数是GUANKA_ITEM_NUM的整数倍
        int tmp = (listGuankaItem.Count % GUANKA_ITEM_NUM);
        if (tmp > 0)
        {
            for (int i = 0; i < (GUANKA_ITEM_NUM - tmp); i++)
            {
                NongChangItemInfo infoId = listGuankaItem[i] as NongChangItemInfo;
                NongChangItemInfo info = new NongChangItemInfo();
                info.id = infoId.id;
                info.pic = infoId.pic;
                listGuankaItem.Add(info);
            }
        }

        ParseMapItem(mapid);
        int group = listGuankaItem.Count / GUANKA_ITEM_NUM;

        count = listAutoGuanka.Count;
        // count = listGuankaItem.Count;

        int rdm = Random.Range(0, listPoint.Count);
        NongChangItemInfo infoPoint = listPoint[rdm] as NongChangItemInfo;
        for (int g = 0; g < group; g++)
        {

            for (int i = 0; i < count; i++)
            {
                NongChangItemInfo infoGuanka = new NongChangItemInfo();

                NongChangItemInfo infoAutoGuanka = listAutoGuanka[i] as NongChangItemInfo;
                string strcontent = infoAutoGuanka.id;
                string[] strArray = strcontent.Split(',');
                infoGuanka.listSearchItem = new List<object>();

                int pos_index = 0;
                foreach (string stritem in strArray)
                {
                    idx = Common.String2Int(stritem) + g * GUANKA_ITEM_NUM;
                    NongChangItemInfo infoId = listGuankaItem[idx] as NongChangItemInfo;
                    NongChangItemInfo infoSearchItem = new NongChangItemInfo();
                    infoSearchItem.id = infoId.id;
                    infoSearchItem.pic = infoId.pic;

                    infoGuanka.listSearchItem.Add(infoSearchItem);
                    NongChangItemInfo infoposition = infoPoint.listPosition[pos_index] as NongChangItemInfo;
                    infoSearchItem.x = infoposition.x;
                    infoSearchItem.y = infoposition.y;
                    infoSearchItem.scale = infoposition.scale;
                    infoSearchItem.flipx = infoposition.flipx;
                    infoSearchItem.isHasFound = false;
                    pos_index++;
                }

                listGuanka.Add(infoGuanka);
            }

        }
        Debug.Log("ParseGame::count=" + count + " listGuanka.count=" + listGuanka.Count);
        return listGuanka.Count;
    }

    public void ParseMapItem(string mapid)
    {


        if (listMapItem == null)
        {
            listMapItem = new List<object>();
        }


        if (listPoint == null)
        {
            listPoint = new List<object>();
        }
        listPoint.Clear();

        string filePath = Common.GAME_RES_DIR + "/guanka/map/" + mapid + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filePath); //((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
                                                          // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);

        string strbgname = (string)root["bg"];
        string picRoot = Common.GAME_RES_DIR + "/map/" + mapid + "/";
        strPicBg = picRoot + strbgname;
        Debug.Log("strPicBg1=" + strPicBg);

        //map_item
        JsonData mapitems = root["items"];
        for (int i = 0; i < mapitems.Count; i++)
        {
            JsonData item = mapitems[i];

            NongChangItemInfo findInfo = new NongChangItemInfo();
            findInfo.flipx = (bool)item["flipx"];
            findInfo.pic = picRoot + (string)item["pic"];
            findInfo.x = (int)item["x"];
            findInfo.y = (int)item["y"];
            //double scale = (double)item["scale"];
            //findInfo.scale =(float)scale;

            listMapItem.Add(findInfo);

        }

        JsonData points = root["points"];
        for (int i = 0; i < points.Count; i++)
        {
            JsonData item = points[i];
            NongChangItemInfo info = new NongChangItemInfo();
            info.sound = (string)item["sound"];
            info.listPosition = new List<object>();
            JsonData targets = item["targets"];
            for (int j = 0; j < targets.Count; j++)
            {
                JsonData findItem = targets[j];
                NongChangItemInfo findInfo = new NongChangItemInfo();
                findInfo.flipx = (bool)findItem["flipx"];
                findInfo.pic = picRoot + (string)findItem["pic"];
                findInfo.sound = (string)findItem["sound"];
                findInfo.x = (int)findItem["x"];
                findInfo.y = (int)findItem["y"];
                double scale = 1.0f;
                if (Common.JsonDataContainsKey(findItem, "scale"))
                {
                    scale = (double)findItem["scale"];
                }
                findInfo.scale = (float)scale;
                findInfo.isHasFound = false;
                info.listPosition.Add(findInfo);
            }
            info.icon = "";


            listPoint.Add(info);
        }
    }

}
