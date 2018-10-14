
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class Language
{


    private LTLocalization ltLocalization;
    static private Language _common = null;
    static private Language _main = null;
    public static Language main//GameData
    {
        get
        {
            if (_main == null)
            {


                _common = new Language();
                string fileName = Common.GAME_DATA_DIR_COMMON + "/language/language.csv";
                _common.Init(fileName);
                //_common.SetLanguage(SystemLanguage.Chinese);

                _main = new Language();
                // _main.Init("Language/Language");
                fileName = Common.GAME_DATA_DIR + "/language/language.csv";
                _main.Init(fileName);
                _main.SetLanguage(SystemLanguage.Chinese);


            }
            return _main;
        }
    }


    static private Language _game = null;
    public static Language game//GameRes
    {
        get
        {
            if (_game == null)
            {
                _game = new Language();
                string filepath = Common.GAME_RES_DIR + "/language/language.csv";
                _game.Init(filepath);
                _game.SetLanguage(main.GetLanguage());
            }
            return _game;
        }
    }
    // void setLanguageType(LanguageType languageType);
    public void Init(string file)
    {
        // if(instance!=null){
        //     return;
        // }
        // instance = new Language();

        ltLocalization = new LTLocalization();
        //csv需要在pc上先转换成utf8格式
        ltLocalization.csvFilePath = file;//Resources 不要后缀
                                          // mInstance.SetLanguage(SystemLanguage.Chinese);
                                          // mInstance.ReadData();

    }

    public void SetLanguage(SystemLanguage lan)
    {
        // Init();
        ltLocalization.SetLanguage(lan);
        _common.ltLocalization.SetLanguage(lan);
        if (_game != null)
        {
            _game.ltLocalization.SetLanguage(lan);
        }
    }

    public SystemLanguage GetLanguage()
    {
        return ltLocalization.GetLanguage();

    }
    public string GetString(string key)
    {
        // Init();
        string str = "0";
        if (IsContainsKey(key))
        {
            str = ltLocalization.GetText(key);
        }
        else
        {
            str = _common.ltLocalization.GetText(key);
        }
        return str;

    }


//
    public string GetReplaceString(string key, string replace, string strnew)
    {
        string str = GetString(key);
        str = str.Replace(replace, strnew);
        return str;
    }

    public bool IsContainsKey(string key)
    {
        return ltLocalization.IsContainsKey(key);
    }


}
