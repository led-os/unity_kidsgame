using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class GameGuankaParse : GuankaParseBase
{   
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




   public StickerGameItem GetItemInfo()
    {
        int idx = GameManager.gameLevel;
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        StickerGameItem info = listGuanka[idx] as StickerGameItem;
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

    }
   
    public override int ParseGuanka()
    {
        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        int count = 0;
        listGuanka = new List<object>();
        int idx = GameManager.placeLevel;
        ItemInfo infoPlace = GameManager.main.GetPlaceItemInfo(GameManager.placeLevel); 
        string filepath = Common.GAME_RES_DIR + "/guanka/item_" + infoPlace.id + ".json";
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);

        JsonData root = JsonMapper.ToObject(json);
        string strPlace = infoPlace.id;
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            StickerGameItem info = new StickerGameItem();
            info.id = (string)item["id"];
            //string str = languageGame.GetString(info.id);
            // Debug.Log(i + ":ParseGame:" + str);

            info.isSticked = false;
            info.isDisplayInLeft = false;

            string picdir = Common.GAME_RES_DIR + "/image/" + strPlace;
            info.pic = picdir + "/" + info.id + ".png";
            info.source = "png";
            if (!FileUtil.FileIsExistAsset(info.pic))
            {
                info.source = "jpg";
                info.pic = picdir + "/" + info.id + ".jpg";
            }

            info.icon = info.pic;
            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        Debug.Log("ParseGame::count=" + count);
        return count;
    }
 

}
