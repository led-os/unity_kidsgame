using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;

public class GameManager
{
    static List<object> listPlace;
    static public int placeLevel;
    static public int placeTotal
    {
        get
        {
            int ret = 0;
            // if (GameScene.gameBase != null)
            {
                GameManager.ParsePlaceList();
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

    static public List<object> ParsePlaceList()
    {
        int count = 0;
        if ((GameManager.listPlace != null) && (GameManager.listPlace.Count != 0))
        {
            return GameManager.listPlace;
        }
        GameManager.listPlace = new List<object>();
        //string fileName = "Place/PlaceList";
        string fileName = Common.GAME_RES_DIR + "/place/place_list.json";
        if (!FileUtil.FileIsExistAsset(fileName))
        {
            return GameManager.listPlace;
        }
        //FILE_PATH
        //string json = ((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        string json = FileUtil.ReadStringAsset(fileName);
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

        return GameManager.listPlace;
    }
    static public void CleanGuankaList()
    {
        GameViewController.main.gameBase.CleanGuankaList();
    }
    static public ItemInfo GetPlaceItemInfo(int idx)
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

    static public void ParseGuanka()
    {
        CleanGuankaList();
        GameViewController.main.gameBase.ParseGuanka();
    }

    static public void GotoGame(UIViewController fromController)
    {

        //GameViewController.main.ShowOnController(AppSceneBase.main.rootViewController);
        NaviViewController navi = fromController.naviController;
        if (navi != null)
        {
            navi.Push(GameViewController.main);

        }

    }

    static public void GotoPreLevel()
    {

        GameManager.gameLevel--;
        if (GameManager.gameLevel < 0)
        {
            GameManager.GotoPrePlace();
            return;

        }
        // GameManager.GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(GameManager.gameLevel);

    }
    static public void GotoNextLevel()
    {
        Debug.Log("gameLevel=" + GameManager.gameLevel + " maxGuankaNum=" + GameManager.maxGuankaNum);
        GameManager.gameLevel++;
        Debug.Log("gameLevel=" + GameManager.gameLevel + " maxGuankaNum=" + GameManager.maxGuankaNum);
        if (GameManager.gameLevel >= GameManager.maxGuankaNum)
        {
            Debug.Log("GotoNextPlace:gameLevel=" + GameManager.gameLevel + " maxGuankaNum=" + GameManager.maxGuankaNum);
            GameManager.GotoNextPlace();
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

    static public void GotoPrePlace()
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
    static public void GotoNextPlace()
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

    static public List<object> GetGuankaListOfAllPlace()
    {
        List<object> listRet = new List<object>();
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
}
