
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRes
{
    public const string GAME_XIEHOUYU = "xiehouyu";
    public const string GAME_IDIOM = "Idiom";
    public const string GAME_POEM = "poem";
    public const string GAME_Image = "Image";
    public const string GAME_Guess = "Guess";
    public const string GAME_RIDDLE = "Riddle";
    public const string GAME_IdiomConnect = "IdiomConnect";
    //type 
    public const string GAME_TYPE_IMAGE = "Image";
    public const string GAME_TYPE_TEXT = "Text";
    public const string GAME_TYPE_IMAGE_TEXT = "ImageText";
    public const string GAME_TYPE_CONNECT = "Connect";//接龙

    //image  
    public const string IMAGE_LetterBgNormal = "App/UI/Game/UILetter/LetterBgNormal";
    public const string IMAGE_LetterBgLock = "App/UI/Game/UILetter/LetterBgLock";
    public const string IMAGE_LetterBgRightAnswer = "App/UI/Game/UILetter/LetterBgRightAnswer";
    public const string IMAGE_LetterBgAddWord = "App/UI/Game/UILetter/LetterBgAddWord";


    //color
    //f88816 248,136,22
    public const string KEY_COLOR_TITLE = "title";
    public const string KEY_COLOR_PlaceItemTitle = "PlaceItemTitle";
    public const string KEY_COLOR_BoardTitle = "BoardTitle";
    public const string KEY_COLOR_GameText = "GameText";
    public const string KEY_COLOR_GameWinTitle = "GameWinTitle";
    public const string KEY_COLOR_GameWinTextView = "GameWinTextView"; 
      
    static private GameRes _main = null;
    public static GameRes main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameRes();
            }
            return _main;
        }
    } 

}
