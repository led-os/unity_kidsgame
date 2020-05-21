using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
 
 

public class GameLevelParse : LevelParseBase
{
  
    static private GameLevelParse _main = null;
    public static GameLevelParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameLevelParse();
            }
            return _main;
        }
    }
 
    
 
}
