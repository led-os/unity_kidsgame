
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppRes
{
    public const int GOLD_SHARE = 5;
    public const int GOLD_GUANKA = 3;
    public const int GOLD_COMMENT = 3;
    public const int GOLD_INIT_VALUE = 10;
    public const int GOLD_GUANKA_STEP = 4;
    //color
    //f88816 248,136,22
    static public Color colorTitle = new Color(248 / 255f, 136 / 255f, 22 / 255f);

    //audio 
    public const string AUDIO_BG = "App/Audio/Bg";
    public const string AUDIO_BTN_CLICK = "AppCommon/Audio/BtnClick";
    
    public const string AUDIO_PINTU_BLOCK_FINISH = "Audio/PintuBlockFinish";
    public const string AUDIO_WORD_OK = "AppCommon/Audio/word_ok";
    public const string AUDIO_WORD_FAIL = "AppCommon/Audio/word-failed";
    public const string AUDIO_LETTER_DRAG_1 = "AppCommon/Audio/letter-drag-1";
    public const string AUDIO_LETTER_DRAG_2 = "AppCommon/Audio/letter-drag-2";
    public const string AUDIO_LETTER_DRAG_3 = "AppCommon/Audio/letter-drag-3";
    public const string AUDIO_LETTER_DRAG_4 = "AppCommon/Audio/letter-drag-4";
    public const string AUDIO_SELECT = "AppCommon/Audio/select";
    public const string AUDIO_SUCCESS_1 = "AppCommon/Audio/success-1";
    public const string AUDIO_SUCCESS_2 = "AppCommon/Audio/success-2";

    public const string Audio_PopupOpen = "AppCommon/Audio/PopUp/PopupOpen";
    public const string Audio_PopupClose = "AppCommon/Audio/PopUp/PopupClose";
    //prefab  
 
    //image
 
 
    public const string IMAGE_UIVIEWALERT_BG_BOARD = "App/UI/Setting/SettingCellBgBlue";
    static public Vector4 borderUIViewAlertBgBoard = new Vector4(18f, 18f, 18f, 18f);
    public const string IMAGE_UIVIEWALERT_BG_BTN = "App/UI/Setting/SettingCellBgOringe";
    static public Vector4 borderUIViewAlertBgBtn = new Vector4(18f, 18f, 18f, 18);

 
    //
    public const string IMAGE_Game_Bomb = "AppCommon/UI/Game/Bomb";
    

    //bg
 
    public const string IMAGE_HOME_BG = Common.GAME_DATA_DIR + "/startup.jpg";
 
    public const string IMAGE_LEARN_BG = "App/UI/Bg/LearnBg";   
 


}
