using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public class JsonUtil : MonoBehaviour
{

    static public string JsonGetString(JsonData data, string key, string strdefault)
    {
        string ret = strdefault;
        if (ContainsKey(data, key))
        {
            ret = (string)data[key];
        }
        return ret;
    }

    static public bool JsonGetBool(JsonData data, string key, bool _default)
    {
        bool ret = _default;
        if (ContainsKey(data, key))
        {
            ret = (bool)data[key];
        }
        return ret;
    }

    static public bool ContainsKey(JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
        {
            return result;
        }
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key))
        {
            result = true;
        }
        return result;
    }

    



}
