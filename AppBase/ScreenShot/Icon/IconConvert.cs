using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconInfo
{
    public string srcPath;
    public string dstPath;//save file
    public int w;
    public int h;
}

public class IconConvert : MonoBehaviour
{
    List<IconInfo> listItem;
    int[] imageSizeIos = new int[] { 20, 29, 40, 50, 57, 58, 60, 72, 76, 80, 87, 100, 114, 120, 144, 152, 167, 180, 1024 };

    int[] imageSizeAndroid = new int[] { 72, 48, 96, 144, 192 };
    string[] resAndroid = new string[] { "mipmap-hdpi", "mipmap-mdpi", "mipmap-xhdpi", "mipmap-xxhdpi", "mipmap-xxxhdpi" };


    int[] imageSizeQQ = new int[] { 16 };
    int[] imageSizeShare = new int[] { 16, 28, 80, 120, 108, 512 };
    int[] imageSizeXiaomi = new int[] { 90, 136, 168, 192, 224 };
    int[] imageSizeHuawei = new int[] { 216 };

    int[] imageSizeMicrosoft = new int[] { 88,24,24,300,50,48 };
    string[] resMicrosoft = new string[] { "Square44x44Logo.scale-200", "Square44x44Logo.targetsize-24", "Square44x44Logo.targetsize-24_altform-unplated", "Square150x150Logo.scale-200", "StoreLogo","LockScreenLogo.scale-200" };


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<IconInfo>();

        //ios
        InitIos(false);
        InitIos(true);
        //android 
        InitAndroid(false);
        InitAndroid(true);

        InitOther(false, Source.QQ, imageSizeQQ);
        InitOther(true, Source.QQ, imageSizeQQ);

        InitOther(false, "share", imageSizeShare);
        InitOther(true, "share", imageSizeShare);

        InitOther(false, Source.XIAOMI, imageSizeXiaomi);
        InitOther(true, Source.XIAOMI, imageSizeXiaomi);

        InitOther(false, Source.HUAWEI, imageSizeHuawei);
        InitOther(true, Source.HUAWEI, imageSizeHuawei);

        InitMicrosoft(false);
        InitMicrosoft(true);
    }


    void InitIos(bool ishd)
    {
        int[] listsize = imageSizeIos;
        for (int i = 0; i < listsize.Length; i++)
        {
            IconInfo info = new IconInfo();
            info.w = listsize[i];
            info.h = info.w;
            info.srcPath = GetRootDir(ishd) + "/icon.png";
            info.dstPath = GetRootDir(ishd) + "/ios/icon_" + info.w + ".png";
            listItem.Add(info);
        }
    }
    void InitAndroid(bool ishd)
    {
        int[] listsize = imageSizeAndroid;
        for (int i = 0; i < listsize.Length; i++)
        {
            IconInfo info = new IconInfo();
            info.w = listsize[i];
            info.h = info.w;
            info.srcPath = GetRootDir(ishd) + "/icon_android.png";
            info.dstPath = GetRootDir(ishd) + "/android/" + resAndroid[i] + "/ic_launcher.png";
            listItem.Add(info);
        }
    }

     void InitMicrosoft(bool ishd)
    {
        int[] listsize = imageSizeMicrosoft;
        for (int i = 0; i < listsize.Length; i++)
        {
            IconInfo info = new IconInfo();
            info.w = listsize[i];
            info.h = info.w;
            info.srcPath = GetRootDir(ishd) + "/icon_android.png";
            info.dstPath = GetRootDir(ishd) + "/microsoft/" + resMicrosoft[i] + ".png";
            listItem.Add(info);
        }
    }
    void InitOther(bool ishd, string dir, int[] listsize)
    {
        //int[] listsize = imageSizeQQ;
        for (int i = 0; i < listsize.Length; i++)
        {
            {
                IconInfo info = new IconInfo();
                info.w = listsize[i];
                info.h = info.w;
                info.srcPath = GetRootDir(ishd) + "/icon.png";
                info.dstPath = GetRootDir(ishd) + "/" + dir + "/icon_" + info.w + ".png";
                listItem.Add(info);
            }
            {
                IconInfo info = new IconInfo();
                info.w = listsize[i];
                info.h = info.w;
                info.srcPath = GetRootDir(ishd) + "/icon_android.png";
                info.dstPath = GetRootDir(ishd) + "/" + dir + "/icon_android_" + info.w + ".png";
                listItem.Add(info);
            }

        }

    }


    void ConvertMainIcon(bool ishd)
    {
        string icon_path = GetRootDir(ishd) + "/icon.png";
        string icon_path_android = GetRootDir(ishd) + "/icon_android.png";
        Texture2D texIcon = LoadTexture.LoadFromFile(icon_path);
        //保存圆角的android  icon 
        {
            Texture2D texTmp = RoundRectTexture(texIcon);
            TextureUtil.SaveTextureToFile(texTmp, icon_path_android);
        }


        Texture2D texIconAndroid = LoadTexture.LoadFromFile(icon_path_android);
        //512 android
        {
            Texture2D tex512 = TextureUtil.ConvertSize(texIconAndroid, 512, 512);
            string filepath = GetRootDir(ishd) + "/icon_android_512.png";
            TextureUtil.SaveTextureToFile(tex512, filepath);
            filepath = GetRootDir(ishd) + "/icon_android_512.jpg";
            TextureUtil.SaveTextureToFile(tex512, filepath);
        }

        //512 ios
        {
            Texture2D tex512 = TextureUtil.ConvertSize(texIcon, 512, 512);
            string filepath = GetRootDir(ishd) + "/icon_512.png";
            TextureUtil.SaveTextureToFile(tex512, filepath);
            filepath = GetRootDir(ishd) + "/icon_512.jpg";
            TextureUtil.SaveTextureToFile(tex512, filepath);
        }


        //1024 ios
        {
            Texture2D tex = TextureUtil.ConvertSize(texIcon, 1024, 1024);
            string filepath = GetRootDir(ishd) + "/icon_1024.jpg";
            TextureUtil.SaveTextureToFile(tex, filepath);
        }

        //1024 android
        {
            Texture2D tex = TextureUtil.ConvertSize(texIconAndroid, 1024, 1024);
            string filepath = GetRootDir(ishd) + "/icon_android_1024.jpg";
            TextureUtil.SaveTextureToFile(tex, filepath);
        }
    }

    //圆角
    Texture2D RoundRectTexture(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        string strshader = "Custom/RoundRect";
        //string str = FileUtil.ReadStringAsset(ShotBase.STR_DIR_ROOT_SHADER+"/ShaderRoundRect.shader");
        Material mat = new Material(Shader.Find(strshader));//
        float value = (ShotBase.roundRectRadiusIcon * 1f / UIScreenShotController.SCREEN_WIDTH_ICON);
        Debug.Log("RoundRectTexture:value=" + value);
        //value = 0.1f;
        //设置半径 最大0.5f
        mat.SetFloat("_RADIUSBUCE", value);
        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt);
        return texRet;
    }
    string GetRootDir(bool ishd)
    {
        string name = ishd ? "iconhd" : "icon";
        string ret = UIScreenShotController.GetRootDir() + "/" + name;
        return ret;
    }
    void DoConvertIcon(IconInfo info)
    {
        Texture2D texIcon = LoadTexture.LoadFromFile(info.srcPath);

        Texture2D texSave = TextureUtil.ConvertSize(texIcon, info.w, info.h, texIcon.format);
        FileUtil.CreateDir(FileUtil.GetFileDir(info.dstPath));
        // 最后将这些纹理数据，成一个png图片文件  
        TextureUtil.SaveTextureToFile(texSave, info.dstPath);

    }

    public void OnConvertAll()
    {
        ConvertMainIcon(false);
        ConvertMainIcon(true);
        foreach (IconInfo info in listItem)
        {
            DoConvertIcon(info);
        }
    }
}
