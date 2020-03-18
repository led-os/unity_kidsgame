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
            // if ((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.WindowsPlayer))
            // {
            //     ret = ROOT_DIR_PC_WIN;
            // }

            ret = Application.dataPath;//assets
            ret = FileUtil.GetLastDir(ret);
            ret = FileUtil.GetLastDir(ret);
            Debug.Log("ROOT_DIR_PC=" + ret);
            return ret;
        }
    }
    public const string ROOT_DIR_PC_MAC = "/Users/moon/sourcecode/unity/product/kidsgame";
    public const string ROOT_DIR_PC_WIN = "F:/sourcecode/unity/product/kidsgame";
}
