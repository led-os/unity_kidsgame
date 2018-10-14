using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
public class TextureUtil : MonoBehaviour
{
    static public Rect GetRectNotAlpha(Texture2D tex)
    {
        int fillXLeft = tex.width;
        int fillXRight = 0;
        int fillYBottom = tex.height;
        int fillYTop = 0;
        int x, y, w, h;
        ColorImage colorImage = new ColorImage();
        colorImage.Init(tex);
        for (int j = 0; j < tex.height; j++)
        {
            for (int i = 0; i < tex.width; i++)
            {
                Vector2 pt = new Vector2(i, j);
                Color color = colorImage.GetImageColorOrigin(pt);
                if (color.a >= 1f)
                {
                    x = i;
                    y = j;
                    if (x < fillXLeft)
                    {
                        fillXLeft = x;
                    }
                    if (x > fillXRight)
                    {
                        fillXRight = x;
                    }


                    if (y < fillYBottom)
                    {
                        fillYBottom = y;
                    }
                    if (y > fillYTop)
                    {
                        fillYTop = y;
                    }
                }

            }
        }
        Debug.Log("tex.width=" + tex.width + " fillXLeft=" + fillXLeft + " fillXRight=" + fillXRight);
        return new Rect(fillXLeft, fillYBottom, (fillXRight - fillXLeft), (fillYTop - fillYBottom));

    }


    //jpg转为透明png
    static public Texture2D ConvertJpg2AlphaPng(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;
        Texture2D texRet = new Texture2D(w, h, TextureFormat.ARGB32, false);
        ColorImage colorImageRet = new ColorImage();
        colorImageRet.Init(texRet);

        ColorImage colorImage = new ColorImage();
        colorImage.Init(tex);


        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Vector2 pt = new Vector2(i, j);
                Color color = colorImage.GetImageColorOrigin(pt);
                color.a = 1f;
                colorImageRet.SetImageColor(pt, color);
            }
        }
        colorImageRet.UpdateTexture();

        return texRet;
    }


    static public Texture2D RenderTexture2Texture2D(RenderTexture rt)
    {
        return RenderTexture2Texture2D(rt, TextureFormat.ARGB32, new Rect(0, 0, rt.width, rt.height));
    }

    static public Texture2D RenderTexture2Texture2D(RenderTexture rt, Rect rc)
    {
        return RenderTexture2Texture2D(rt, TextureFormat.ARGB32, rc);
    }
    static public Texture2D RenderTexture2Texture2D(RenderTexture rt, TextureFormat format, Rect rc)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D((int)rc.width, (int)rc.height, format, false);
        //new Rect(0, 0, rt.width, rt.height)
        tex.ReadPixels(rc, 0, 0);
        tex.Apply();
        RenderTexture.active = prev;
        return tex;
    }

    static public Texture2D ConvertSize(Texture2D tex, int w_new, int h_new)
    {
        return ConvertSize(tex, w_new, h_new, TextureFormat.ARGB32);
    }
    static public Texture2D ConvertSize(Texture2D tex, int w_new, int h_new, TextureFormat format)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w_new, h_new, 0);
        Graphics.Blit(tex, rt);
        Texture2D texRet = RenderTexture2Texture2D(rt, format, new Rect(0, 0, rt.width, rt.height));
        return texRet;
    }


    static public Texture2D GetSubTexture(Texture2D tex, Rect rc)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        Graphics.Blit(tex, rt);
        Texture2D texRet = RenderTexture2Texture2D(rt, rc);
        return texRet;
    }

    static public void SaveTextureToFile(Texture2D tex, string filepath)
    {
        string strExt = FileUtil.GetFileExt(filepath);
        byte[] bytes = null;
        if (strExt == "png")
        {
            bytes = tex.EncodeToPNG();
        }
        else if (strExt == "jpg")
        {
            bytes = tex.EncodeToJPG();
        }
        else
        {
            bytes = tex.EncodeToPNG();
        }
        System.IO.File.WriteAllBytes(filepath, bytes);
    }


    static public void UpdateImageTexture(Image image, string filepath, bool isUpdateSize)
    {
        UpdateImageTexture(image, filepath, isUpdateSize, Vector4.zero);
    }

    static public void UpdateImageTexture(Image image, string filepath, bool isUpdateSize, Vector4 border)
    {
        Texture2D tex = TextureCache.main.Load(filepath);
        if (tex)
        {
            Sprite sp = LoadTexture.CreateSprieFromTex(tex, border);
            image.sprite = sp;
            if (isUpdateSize)
            {
                RectTransform rctan = image.GetComponent<RectTransform>();
                rctan.sizeDelta = new Vector2(tex.width, tex.height);
            }

        }
    }




}