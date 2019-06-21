using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class TrophyRes
{
    //audio
    public const string AUDIO_TROPHY_GET_STAR = "App/Audio/TrophyGetStar";
    public const string AUDIO_TROPHY_GET_MEDAL_CUP = "AppCommon/Audio/TrophyGetMedalCup";
    public const string AUDIO_TROPHY = "AppCommon/Audio/Trophy";
    //image
    public const string IMAGE_TROPHY_BG = "App/UI/Game/Trophy/Bg";

    //idx :奖励星,奖牌,奖杯 
    public const string KEY_TROPHY_NUM_STAR = "KEY_TROPHY_NUM_STAR";
    public const string KEY_TROPHY_NUM_MEDAL = "KEY_TROPHY_NUM_MEDAL";
    public const string KEY_TROPHY_NUM_CUP = "KEY_TROPHY_NUM_CUP";
    public const string KEY_TROPHY_NUM_CROWN = "KEY_TROPHY_NUM_CROWN";

    public const string KEY_TROPHY_NUM_STAR_DISPLAY = "KEY_TROPHY_NUM_STAR_DISPLAY";
    public const string KEY_TROPHY_NUM_MEDAL_DISPLAY = "KEY_TROPHY_NUM_MEDAL_DISPLAY";
    public const string KEY_TROPHY_NUM_CUP_DISPLAY = "KEY_TROPHY_NUM_CUP_DISPLAY";
    public const string KEY_TROPHY_NUM_CROWN_DISPLAY = "KEY_TROPHY_NUM_CROWN_DISPLAY";

    public const int TYPE_Star = 0;   //奖励星
    public const int TYPE_Medal = 1; //奖牌
    public const int TYPE_Cup = 2; //奖杯  
    public const int TYPE_Crown = 3;// //皇冠

    public const int ONE_CELL_NUM_STAR = 10;
    public const int ONE_CELL_NUM_MEDAL = 10;
    public const int ONE_CELL_NUM_CUP = 5;

    //group begain with 1
    static public string GetImageOfIcon(int ty, int group)
    {
        if (ty == TYPE_Star)
        {
            return GetImageOfStar(group);
        }
        if (ty == TYPE_Medal)
        {
            return GetImageOfMedal(group);
        }
        if (ty == TYPE_Cup)
        {
            return GetImageOfCup(group);
        }

        if (ty == TYPE_Crown)
        {
            //皇冠
            return AppRes.IMAGE_TROPHY_Crown_small;
        }
        return "";
    }
    //奖励星
    static public string GetImageOfStar(int group)
    {
        //1 - 1 - 1
        return AppRes.IMAGE_TROPHY_Star_PREFIX + group.ToString() + "-1-1" + ".png";
    }
    //奖牌
    static public string GetImageOfMedal(int group)
    {
        //1 - 2 - 1
        return AppRes.IMAGE_TROPHY_Medal_PREFIX + group.ToString() + "-2-1" + ".png";
    }
    //奖杯  
    static public string GetImageOfCup(int group)
    {
        //1-3-big
        return AppRes.IMAGE_TROPHY_Cup_PREFIX + group.ToString() + "-3-small" + ".png";
    }


}



