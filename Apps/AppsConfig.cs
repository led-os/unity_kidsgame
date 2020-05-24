using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsConfig
{

    static public string ROOT_DIR_PC
    {
        get
        {
            string ret = ROOT_DIR_PC_MAC;


            ret = Application.dataPath;//assets
            ret = FileUtil.GetLastDir(ret);
            ret = FileUtil.GetLastDir(ret);
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                ret = ROOT_DIR_PC_WIN;
            }
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                ret = ROOT_DIR_PC_MAC;
            }

            // Debug.Log("ROOT_DIR_PC=" + ret);
            return ret;
        }
    }
    public const string ROOT_DIR_PC_MAC = "/Users/moon/sourcecode/unity/product/kidsgame";
    public const string ROOT_DIR_PC_WIN = "F:/sourcecode/unity/product/kidsgame";
}
