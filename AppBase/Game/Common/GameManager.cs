using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System.Text;

public class GameManager
{


    static public int placeLevel;

    public UIViewController fromUIViewController;//来源

    public float heightAdWorld;
    public float heightAdScreen;
    public float heightAdCanvas;

    public bool isShowGameAdInsert;

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
            ret = GameGuankaParse.main.listPlace.Count;//GameScene.gameBase.GetPlaceTotal();
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
            int ret = GameGuankaParse.main.GetGuankaTotal();
            return ret;
        }
    }
    public List<object> ParsePlaceList()
    {
        return GameGuankaParse.main.listPlace;
    }
    public void CleanGuankaList()
    {
        GameGuankaParse.main.CleanGuankaList();
    }
    public ItemInfo GetPlaceItemInfo(int idx)
    {
        return GameGuankaParse.main.GetPlaceItemInfo(idx);
    }

    public void ParseGuanka()
    {
        CleanGuankaList();
        //GameViewController.main.gameBase.ParseGuanka();
        GameGuankaParse.main.ParseGuanka();
    }

    public void GotoGame(UIViewController fromController)
    {
        fromUIViewController = fromController;
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
            if (GameGuankaParse.main.listGuanka == null)
            {
                Debug.Log("listGuanka is null");
            }
            else
            {
                foreach (object obj in GameGuankaParse.main.listGuanka)
                {
                    listRet.Add(obj);
                }
            }


        }
        return listRet;

    }


    public List<object> GetGuankaListOfPlace(int idx)
    {
        List<object> listRet = new List<object>();
        {
            GameManager.placeLevel = idx;
            //必须在placeLevel设置之后再设置gameLevel
            GameManager.gameLevel = 0;
            ParseGuanka();
            if (GameGuankaParse.main.listGuanka == null)
            {
                Debug.Log("listGuanka is null");
            }
            else
            {
                foreach (object obj in GameGuankaParse.main.listGuanka)
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
        ParsePlaceList();

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
