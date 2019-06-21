
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppRes
{
    public const int GOLD_SHARE = 5;
    public const int GOLD_GUANKA = 3;
    public const int GOLD_COMMENT = 3;
    public const int GOLD_INIT_VALUE = 50;
    public const int GOLD_GUANKA_STEP = 4;

    public const int GOLD_CONSUME = 3;//消费

    //game name
    public const string GAME_IronIceCream = "IronIceCream";
    //color
    //f88816 248,136,22
    static public Color colorTitle = new Color(248 / 255f, 136 / 255f, 22 / 255f);

    //audio 
    public const string AUDIO_DIR_ROOT = "AppCommon/Audio/";
    public const string AUDIO_BG = AUDIO_DIR_ROOT + "Bg";
    public const string AUDIO_BTN_CLICK = AUDIO_DIR_ROOT + "BtnClick";
    public const string AUDIO_GAME_camera = AUDIO_DIR_ROOT + "game/camera";
    public const string AUDIO_GAME_liquid = AUDIO_DIR_ROOT + "game/liquid";
    public const string AUDIO_GAME_minus_star = AUDIO_DIR_ROOT + "game/minus_star";
    public const string AUDIO_GAME_move_chanzi = AUDIO_DIR_ROOT + "game/move_chanzi";
    public const string AUDIO_GAME_place_topfood = AUDIO_DIR_ROOT + "game/place_topfood";
    public const string AUDIO_GAME_remove_sugar = AUDIO_DIR_ROOT + "game/remove_sugar";
    public const string AUDIO_GAME_chan = AUDIO_DIR_ROOT + "game/chan";
    public const string AUDIO_GAME_eat = AUDIO_DIR_ROOT + "game/eat";


    //prefab  

    public const string PREFAB_GUANKA_CELL_ITEM = "AppCommon/Prefab/Guanka/UIGuankaCellItem";
    public const string PREFAB_CmdItem = "AppCommon/Prefab/CmdItem/UICmdItem";
    public const string PREFAB_SETTING = "AppCommon/Prefab/Setting/UISettingController";
    public const string PREFAB_MOREAPP_CELL_ITEM = "AppCommon/Prefab/MoreApp/UIMoreAppCellItem";

    //image
    public const string IMAGE_MOREAPP_BG = "App/UI/Setting/SettingBg";
    public const string IMAGE_UIVIEWALERT_BG_BOARD = "App/UI/Setting/SettingCellBgBlue";
    static public Vector4 borderUIViewAlertBgBoard = new Vector4(18f, 18f, 18f, 18f);
    public const string IMAGE_UIVIEWALERT_BG_BTN = "App/UI/Setting/SettingCellBgOringe";
    static public Vector4 borderUIViewAlertBgBtn = new Vector4(18f, 18f, 18f, 18);

    public const string IMAGE_BTN_FREE_cn = "App/UI/Home/BtnFree_cn";
    public const string IMAGE_BTN_FREE_en = "App/UI/Home/BtnFree_en";


    public const string IMAGE_CUP = Common.GAME_RES_DIR + "/image/TopFoodBar/Cup.png";

    public const string IMAGE_BtnMusicOn = "App/UI/Home/BtnMusicOn";
    public const string IMAGE_BtnMusicOff = "App/UI/Home/BtnMusicOff";


    public const string IMAGE_BTN_LANGUAGE_cn = "App/UI/Home/BtnLanguage_cn";
    public const string IMAGE_BTN_LANGUAGE_en = "App/UI/Home/BtnLanguage_en";

    public const string IMAGE_HOME_LOGO = "App/UI/Home/Logo";

    //trophy
    public const string IMAGE_ROOT_DIR_TROPHY = Common.GAME_RES_DIR + "/image/Trophy";
    public const string IMAGE_TROPHY_CELL_BG = IMAGE_ROOT_DIR_TROPHY + "/CellBg.png";
    public const string IMAGE_TROPHY_BG1 = IMAGE_ROOT_DIR_TROPHY + "/Bg1.png";
    public const string IMAGE_TROPHY_BG2 = IMAGE_ROOT_DIR_TROPHY + "/Bg2.png";
    public const string IMAGE_TROPHY_Crown = IMAGE_ROOT_DIR_TROPHY + "/Crown.png";
    public const string IMAGE_TROPHY_Crown_small = IMAGE_ROOT_DIR_TROPHY + "/Crown_small.png";
    public const string IMAGE_TROPHY_ImageTitle_cn = IMAGE_ROOT_DIR_TROPHY + "/ImageTitle_cn.png";
    public const string IMAGE_TROPHY_ImageTitle_en = IMAGE_ROOT_DIR_TROPHY + "/ImageTitle_en.png";
    public const string IMAGE_TROPHY_ImageTips_cn = IMAGE_ROOT_DIR_TROPHY + "/ImageDetail_cn.png";
    public const string IMAGE_TROPHY_ImageTips_en = IMAGE_ROOT_DIR_TROPHY + "/ImageDetail_en.png";

    public const string IMAGE_TROPHY_Rotation_PREFIX = IMAGE_ROOT_DIR_TROPHY + "/Rotation/01_000";
    public const string IMAGE_TROPHY_LEVEL_PREFIX = IMAGE_ROOT_DIR_TROPHY + "/Level/";
    public const string IMAGE_TROPHY_Flower_PREFIX = IMAGE_ROOT_DIR_TROPHY + "/Flower/";
    public const string IMAGE_TROPHY_Star_PREFIX = IMAGE_ROOT_DIR_TROPHY + "/Star/";
    public const string IMAGE_TROPHY_Medal_PREFIX = IMAGE_ROOT_DIR_TROPHY + "/Medal/";
    public const string IMAGE_TROPHY_Cup_PREFIX = IMAGE_ROOT_DIR_TROPHY + "/Cup/";
    public const string IMAGE_ICECREAM_MULTICOLOR_PREFIX = Common.GAME_RES_DIR + "/image/IronIceCream/MultiColor/multi_color_";
    public const string IMAGE_CUP_MULTICOLOR_PREFIX = Common.GAME_RES_DIR + "/image/TopFoodBar/CupFood/cup_multi_color_";
    public const int IMAGE_CUP_MULTICOLOR_TOTAL = 6;
    public const string IMAGE_HOME_BG = Common.GAME_DATA_DIR + "/startup.jpg";
    public const string IMAGE_PLACE_BG = "App/UI/Guanka/GuankaBg";
    public const string IMAGE_GUANKA_BG = "App/UI/Guanka/GuankaBg";
    public const string IMAGE_GUANKA_ITEM_DOT0 = "App/UI/Guanka/dot0";
    public const string IMAGE_GUANKA_ITEM_DOT1 = "App/UI/Guanka/dot1";
    public const string IMAGE_GUANKA_CELL_BG = "App/UI/Guanka/guanka_cell_bg";
    public const string IMAGE_GUANKA_CELL_BG_LOCK = "App/UI/Guanka/guanka_cell_bg_lock";


    public const string IMAGE_COMMON_BG = "App/UI/Guanka/GuankaBg";
    public const string IMAGE_CELL_BG_BLUE = "App/UI/Setting/SettingCellBgBlue";
    public const string IMAGE_CELL_BG_ORINGE = "App/UI/Setting/SettingCellBgOringe";
    public const string IMAGE_CELL_BG_YELLOW = "App/UI/Setting/SettingCellBgYellow";
    static public Vector4 borderCellSettingBg = new Vector4(18f, 18f, 18f, 18f);
    static public Vector4 borderCellTrophyBg = new Vector4(50f, 50f, 50f, 50f);
    public const string IMAGE_GAME_BG =  "App/UI/Game/GameBg";

    public const string IMAGE_HAND = "App/UI/Game/hand";


    //string 
    public const string SOURCE_NAVI_GUANKA = "SOURCE_NAVI_GUANKA";
    public const string SOURCE_NAVI_HISTORY = "SOURCE_NAVI_HISTORY";



}
