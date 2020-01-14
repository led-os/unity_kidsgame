
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoveDB : DBLearnWord
{

    static LoveDB _main = null;
    public static LoveDB main
    {
        get
        {
            if (_main==null)
            {
                _main = new LoveDB();
                Debug.Log("LoveDB main init");
                _main.dbFileName = "LoveDB2.sqlite";
                _main.CreateDb();
            }
            return _main;
        }


        //ng:
        //  get
        // {
        //     if (_main==null)
        //     {
        //         _main = new LoveDB();
        //         Debug.Log("LoveDB main init");
        //         _main.CreateDb();
        //     }
        //     return _main;
        // }
    }

  
}

