using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System.Text;

public class LanguageManager
{

    public Language languageGame;

    static private LanguageManager _main = null;
    public static LanguageManager main
    {
        get
        {
            if (_main == null)
            {
                _main = new LanguageManager();
            }
            return _main;
        }
    }


    public void UpdateLanguage(int indexPlace)
    {
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(indexPlace);
        string filepath = Common.GAME_RES_DIR + "/language/" + infoPlace.language + ".csv";
        byte[] data = FileUtil.ReadDataAuto(filepath);
        if (languageGame == null)
        {
            languageGame = new Language();
        }
        languageGame.Init(data);
        languageGame.SetLanguage(Language.main.GetLanguage());
    }

}
