using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class GameGuankaParse : GuankaParseBase
{
    static private GameGuankaParse _main = null;
    public static GameGuankaParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameGuankaParse();
            }
            return _main;
        }
    }



    public override int GetGuankaTotal()
    {
        int count = ParseGuanka();
        return count;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }

    }
    public override int ParseGuanka()
    {
        int count = 0;
        return count;

    }







}
