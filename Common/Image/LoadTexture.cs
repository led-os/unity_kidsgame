using UnityEngine;
using System.Collections;
using System.IO;

public class LoadTexture : MonoBehaviour
{


    //filePath 为绝对路径
    static public Texture2D LoadFromFile(string filePath)
    {
        Texture2D tex = null;
        byte[] data = FileUtil.ReadData(filePath);
        if (data != null)
        {
            tex = LoadFromData(data);
        }
        return tex;
    }

    //file 为相对路径
    static public Texture2D LoadFromAsset(string file)
    {
        Texture2D tex = null;
        byte[] data = FileUtil.ReadDataAsset(file);
        if (data != null)
        {
            tex = LoadFromData(data);
        }
        return tex;
    }


    static public Texture2D LoadFromData(byte[] data)
    {
        Texture2D tex = null;
        tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        tex.LoadImage(data);
        return tex;
    }

    static public Texture2D LoadFromDataWithFormat(byte[] data, TextureFormat format)
    {
        Texture2D tex = null;
        tex = new Texture2D(0, 0, format, false);
        Debug.Log("LoadFromDataWithFormat 1 format=" + tex.format);
        tex.LoadImage(data);
        Debug.Log("LoadFromDataWithFormat 2 format=" + tex.format);
        return tex;
    }

    static public Texture2D LoadFromResource(string file)
    {
        Texture2D tex = (Texture2D)Resources.Load(file);
        return tex;
    }

    static public Sprite CreateSprieFromResource(string file)
    {
        Texture2D tex = LoadFromResource(file);
        Sprite sp = CreateSprieFromTex(tex);
        return sp;
    }

    static public Sprite CreateSprieFromResource(string file, Vector4 border)
    {
        Texture2D tex = LoadFromResource(file);
        Sprite sp = CreateSprieFromTex(tex, border);
        return sp;
    }
    static public Sprite CreateSprieFromTex(Texture2D tex)
    {
        Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return sp;
    }

    static public Sprite CreateSprieFromTex(Texture2D tex, Vector4 border)
    {
        Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.Tight, border);
        return sp;
    }

    static public Sprite CreateSprieFromAsset(string file)
    {
        Texture2D tex = LoadFromAsset(file);
        Sprite sp = CreateSprieFromTex(tex);
        return sp;
    } 
 
}