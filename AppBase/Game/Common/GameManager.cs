using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System.Text;

public class GameManager
{
    static List<object> listPlace;
    static public int placeLevel;
    HttpRequest httpReqPlaceList;


    static private GameManager _main = null;
    public static GameManager main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameManager();
            }
            return _main;
        }
    }

    static public int placeTotal
    {
        get
        {
            int ret = 0;
            // if (GameScene.gameBase != null)
            {
                GameManager.main.ParsePlaceList();
                ret = GameManager.listPlace.Count;//GameScene.gameBase.GetPlaceTotal();
            }



            return ret;
        }
    }

    static public int gameLevel
    {
        get
        {
            int ret = 0;
            string key = "KEY_GAME_LEVEL_PLACE" + GameManager.placeLevel;
            ret = PlayerPrefs.GetInt(key, 0);
            return ret;
        }

        set
        {
            string key = "KEY_GAME_LEVEL_PLACE" + GameManager.placeLevel;
            PlayerPrefs.SetInt(key, value);

        }

    }
    static public int gameLevelFinish//已经通关 
    {
        get
        {
            int ret = 0;
            string key = "KEY_GAME_LEVEL_PLACE_FINISH" + GameManager.placeLevel;
            ret = PlayerPrefs.GetInt(key, -1);
            return ret;
        }

        set
        {
            string key = "KEY_GAME_LEVEL_PLACE_FINISH" + GameManager.placeLevel;
            PlayerPrefs.SetInt(key, value);
        }

    }
    static public int gameMode
    {
        get
        {
            int ret = 0;
            if (GameViewController.main.gameBase != null)
            {

                ret = UIGameBase.gameMode;
            }

            return ret;
        }

        set
        {
            if (GameViewController.main.gameBase != null)
            {
                Debug.Log("GameScene.gameBase.gameMode = " + value);
                UIGameBase.gameMode = value;

            }

        }

    }
    static public int maxGuankaNum
    {
        get
        {
            int ret = 0;


            if (GameViewController.main.gameBase != null)
            {
                ret = GameViewController.main.gameBase.GetGuankaTotal();
            }
            return ret;
        }
    }
    void StartParsePlaceList()
    {

        if (GameManager.listPlace == null)
        {
            GameManager.listPlace = new List<object>();
        }
        if (GameManager.listPlace.Count > 0)
        {
            //已经解析完成
            return;
        }
        string filepath = Common.GAME_RES_DIR + "/place/place_list.json";
        if (Common.isWeb)
        {
            httpReqPlaceList = new HttpRequest(OnHttpRequestFinished);
            httpReqPlaceList.Get(HttpRequest.GetWebUrlOfAsset(filepath));
        }
        else
        {
            byte[] data = FileUtil.ReadDataAuto(filepath);
            OnGetPlaceListDidFinish(FileUtil.FileIsExistAsset(filepath), data, true);
        }
    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (req == httpReqPlaceList)
        {
            OnGetPlaceListDidFinish(isSuccess, data, false);

        }
    }
    void OnGetPlaceListDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {
        if (isSuccess)
        {
            ParsePlaceList(data);
        }
    }

    void ParsePlaceList(byte[] data)
    {
        if ((GameManager.listPlace != null) && (GameManager.listPlace.Count != 0))
        {
            return;
        }

        string json = Encoding.UTF8.GetString(data);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["places"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["game_type"];
            info.title = (string)item["title"];
            info.pic = Common.GAME_RES_DIR + "/" + (string)item["pic"];
            info.icon = info.pic;
            // info.tag = PlaceScene.PLACE_ITEM_TYPE_GAME;
            info.index = i;
            GameManager.listPlace.Add(info);
        }
    }

    public List<object> ParsePlaceList()
    {
        StartParsePlaceList();
        return GameManager.listPlace;
    }
    public void CleanGuankaList()
    {
        GameViewController.main.gameBase.CleanGuankaList();
    }
    public ItemInfo GetPlaceItemInfo(int idx)
    {
        ParsePlaceList();
        int index = 0;
        foreach (ItemInfo info in GameManager.listPlace)
        {
            //  if (info.tag == PlaceScene.PLACE_ITEM_TYPE_GAME)
            {
                if (index == idx)
                {
                    return info;
                }
                index++;
            }
        }

        return null;
    }

    public void ParseGuanka()
    {
        CleanGuankaList();
        GameViewController.main.gameBase.ParseGuanka();
    }

    public void GotoGame(UIViewController fromController)
    {

        //GameViewController.main.ShowOnController(AppSceneBase.main.rootViewController);
        NaviViewController navi = fromController.naviController;
        if (navi != null)
        {
            navi.Push(GameViewController.main);

        }

    }

    public void GotoPlayAgain()
    {
        GameViewController.main.gameBase.UpdateGuankaLevel(GameManager.gameLevel);
    }

    static public void GotoPreLevel()
    {

        GameManager.gameLevel--;
        if (GameManager.gameLevel < 0)
        {
            GameManager.main.GotoPrePlace();
            return;

        }
        // GameManager.GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(GameManager.gameLevel);

    }
    public void GotoNextLevel()
    {
        Debug.Log("gameLevel=" + GameManager.gameLevel + " maxGuankaNum=" + GameManager.maxGuankaNum);
        GameManager.gameLevel++;
        Debug.Log("gameLevel=" + GameManager.gameLevel + " maxGuankaNum=" + GameManager.maxGuankaNum);
        if (GameManager.gameLevel >= GameManager.maxGuankaNum)
        {
            Debug.Log("GotoNextPlace:gameLevel=" + GameManager.gameLevel + " maxGuankaNum=" + GameManager.maxGuankaNum);
            GotoNextPlace();
            return;

        }
        // GameManager.GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(GameManager.gameLevel);

    }

    //关卡循环
    static public void GotoNextLevelWithoutPlace()
    {
        Debug.Log("gameLevel=" + GameManager.gameLevel + " maxGuankaNum=" + GameManager.maxGuankaNum);
        GameManager.gameLevel++;
        Debug.Log("gameLevel=" + GameManager.gameLevel + " maxGuankaNum=" + GameManager.maxGuankaNum);
        if (GameManager.gameLevel >= GameManager.maxGuankaNum)
        {
            GameManager.gameLevel = 0;

        }
        //GameManager.GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(GameManager.gameLevel);

    }

    public void GotoPrePlace()
    {

        GameManager.placeLevel--;
        if (GameManager.placeLevel < 0)
        {
            GameManager.placeLevel = GameManager.placeTotal - 1;

        }
        //必须在placeLevel设置之后再设置gameLevel
        GameManager.gameLevel = 0;

        ParseGuanka();

        // GameManager.GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(GameManager.gameLevel);

    }
    public void GotoNextPlace()
    {

        GameManager.placeLevel++;

        if (GameManager.placeLevel >= GameManager.placeTotal)
        {
            GameManager.placeLevel = 0;

        }
        //必须在placeLevel设置之后再设置gameLevel
        GameManager.gameLevel = 0;

        ParseGuanka();
        // GameManager.GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(GameManager.gameLevel);

    }

    public List<object> GetGuankaListOfAllPlace()
    {
        List<object> listRet = new List<object>();
        Debug.Log("GetGuankaListOfAllPlace placeTotal=" + placeTotal);
        for (int i = 0; i < placeTotal; i++)
        {
            GameManager.placeLevel = i;
            //必须在placeLevel设置之后再设置gameLevel
            GameManager.gameLevel = 0;
            ParseGuanka();
            if (UIGameBase.listGuanka == null)
            {
                Debug.Log("listGuanka is null");
            }
            else
            {
                foreach (object obj in UIGameBase.listGuanka)
                {
                    listRet.Add(obj);
                }
            }


        }
        return listRet;

    }

    //webgl 异步加载需要提前加载一些配置数据
    public void PreLoadDataForWeb()
    {
        //place list
        StartParsePlaceList();

        //place
        PlaceViewController.main.PreLoadDataForWeb();

        //guanka 
        GuankaViewController.main.PreLoadDataForWeb();

        //game
        UIGameBase game = GameViewController.main.gameBase;
        if (game != null)
        {
            game.PreLoadDataForWeb();
        }

    }
}
