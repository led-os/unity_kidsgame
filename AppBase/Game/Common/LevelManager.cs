using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System.Text;

public class LevelManager
{


    public int placeLevel;
    static private LevelManager _main = null;
    public static LevelManager main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelManager();
            }
            return _main;
        }
    }

    public int placeTotal
    {
        get
        {
            int ret = 0;
            ret = GameGuankaParse.main.listPlace.Count;//GameScene.gameBase.GetPlaceTotal();
            return ret;
        }
    }

    public int gameLevel
    {
        get
        {
            int ret = 0;
            string key = "KEY_GAME_LEVEL_PLACE" + placeLevel;
            ret = PlayerPrefs.GetInt(key, 0);
            return ret;
        }

        set
        {
            string key = "KEY_GAME_LEVEL_PLACE" + placeLevel;
            PlayerPrefs.SetInt(key, value);

        }

    }
    public int gameLevelFinish//已经通关 
    {
        get
        {
            int ret = 0;
            string key = "KEY_GAME_LEVEL_PLACE_FINISH" + placeLevel;
            ret = PlayerPrefs.GetInt(key, -1);
            return ret;
        }

        set
        {
            string key = "KEY_GAME_LEVEL_PLACE_FINISH" + placeLevel;
            PlayerPrefs.SetInt(key, value);
        }

    }
    public int maxGuankaNum
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

    public void GotoPreLevel()
    {

        gameLevel--;
        if (gameLevel < 0)
        {
            GotoPrePlace();
            return;

        }
        // GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(gameLevel);

    }
    public void GotoNextLevel()
    {
        Debug.Log("gameLevel=" + gameLevel + " maxGuankaNum=" + maxGuankaNum);
        gameLevel++;
        //Debug.Log("gameLevel=" + gameLevel + " maxGuankaNum=" + maxGuankaNum);
        if (gameLevel >= maxGuankaNum)
        {
           // Debug.Log("GotoNextPlace:gameLevel=" + .gameLevel + " maxGuankaNum=" + maxGuankaNum);
            GotoNextPlace();
            return;

        }
        // GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(gameLevel);

    }

    //关卡循环
    public void GotoNextLevelWithoutPlace()
    {
        Debug.Log("gameLevel=" + gameLevel + " maxGuankaNum=" + maxGuankaNum);
        gameLevel++;
        Debug.Log("gameLevel=" + gameLevel + " maxGuankaNum=" + maxGuankaNum);
        if (gameLevel >= maxGuankaNum)
        {
            gameLevel = 0;

        }
        //GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(gameLevel);

    }

    public void GotoPrePlace()
    {

        placeLevel--;
        if ( placeLevel < 0)
        {
            placeLevel = placeTotal - 1;

        }
        //必须在placeLevel设置之后再设置gameLevel
        gameLevel = 0;

        ParseGuanka();

        // GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(gameLevel);

    }
    public void GotoNextPlace()
    {

        placeLevel++;

        if (placeLevel >= placeTotal)
        {
            placeLevel = 0;

        }
        //必须在placeLevel设置之后再设置gameLevel
        gameLevel = 0;

        ParseGuanka();
        // GotoGame();
        GameViewController.main.gameBase.UpdateGuankaLevel(gameLevel);

    }

    public List<object> GetGuankaListOfAllPlace()
    {
        List<object> listRet = new List<object>();
        Debug.Log("GetGuankaListOfAllPlace placeTotal=" + placeTotal);
        for (int i = 0; i < placeTotal; i++)
        {
            placeLevel = i;
            //必须在placeLevel设置之后再设置gameLevel
            gameLevel = 0;
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
            placeLevel = idx;
            //必须在placeLevel设置之后再设置gameLevel
            gameLevel = 0;
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

}
