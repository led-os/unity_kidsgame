

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCache
{
    Dictionary<string, Texture2D> dicItem;

    static private TextureCache _main = null;
    public static TextureCache main
    {
        get
        {
            if (_main == null)
            {
                _main = new TextureCache();
                _main.Init();
            }
            return _main;
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Init()
    {
        dicItem = new Dictionary<string, Texture2D>();
    }

    public Texture2D Load(string filepath)
    {
        Texture2D tex = null;
        string key = filepath;
        if (dicItem.ContainsKey(key))
        {
            tex = dicItem[key];
        }
        else
        {
            if (FileUtil.FileIsExistAsset(key))
            {
                tex = LoadTexture.LoadFromAsset(key);
            }
            else
            {
                if (tex == null)
                {
                    tex = LoadTexture.LoadFromResource(key);
                }

                if (tex == null)
                {
                    tex = LoadTexture.LoadFromFile(key);
                }

            }

            if (tex != null)
            {
                dicItem.Add(key, tex);
            }

        }
        return tex;
    }

    public void DestoryAllItem()
    {
        foreach (KeyValuePair<string, Texture2D> item in dicItem)
        {
            string key = item.Key;
            Texture2D tex = item.Value;
            if (tex != null)
            {
                GameObject.DestroyImmediate(tex);
                tex = null;
            }
        }
        dicItem.Clear();
    }

    public void DeleteItem(string key)
    {
        if (!dicItem.ContainsKey(key))
        {
            return;
        }
        Texture2D tex = dicItem[key];
        if (tex != null)
        {
            GameObject.DestroyImmediate(tex);
            tex = null;
        }
        dicItem.Remove(key);
    }
}
