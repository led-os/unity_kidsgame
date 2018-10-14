using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
public class FileUtil : MonoBehaviour
{
    public const string JAVA_CLASS_FILEUTIL = "com.moonma.common.FileUtil";

    //filePath 为绝对路径
    static public byte[] ReadData(string filePath)
    {

        //FileStream fs = new FileStream(filePath, FileMode.Open);
        //win10 访问 app 目录下文件需要加 FileAccess.Read 权限
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (fs != null)
        {
            int len = (int)fs.Length;
            if (len > 0)
            {
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                return data;
            }
            else
            {
                fs.Close();
            }
        }
        return null;
    }
    /* 
        ios下边的目录可以使用FIle.Read 直接读取，
        但是StreamAsseting目录文件流只能使用myStream = File.OpenRead(myPath); 来读。原因是FileMode.Open有读写权限 而StreamAsseting只有读的权限 
        所以 用FIle.Open会出错:Access to the path "/..Raw/4.page" is denied.
    */
    static public byte[] ReadDataAssetIos(string filePath)
    {
        FileStream fs = File.OpenRead(filePath);
        if (fs != null)
        {
            int len = (int)fs.Length;
            if (len > 0)
            {
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                return data;
            }
            else
            {
                fs.Close();
            }
        }
        return null;
    }

    static public string ReadString(string filePath)
    {
        byte[] data = ReadData(filePath);
        if (data == null)
        {
            return null;
        }
        string str = Encoding.UTF8.GetString(data);
        return str;
    }

    //http://blog.csdn.net/ynnmnm/article/details/52253674
    /*
        我们常用的是以下四个路径：

    Application.dataPath 
    Application.streamingAssetsPath 
    Application.persistentDataPath 
    Application.temporaryCachePath 
    根据测试，详细情况如下：

    iOS:

    Application.dataPath            /var/containers/Bundle/Application/app sandbox/xxx.app/Data 
    Application.streamingAssetsPath /var/containers/Bundle/Application/app sandbox/test.app/Data/Raw 
    Application.temporaryCachePath /var/mobile/Containers/Data/Application/app sandbox/Library/Caches 
    Application.persistentDataPath  /var/mobile/Containers/Data/Application/app sandbox/Documents


     Android:

    Application.dataPath            /data/app/package name-1/base.apk 
    Application.streamingAssetsPath jar:file:///data/app/package name-1/base.apk!/assets 
    Application.temporaryCachePath /storage/emulated/0/android/data/package name/cache 
    Application.persistentDataPath   /storage/emulated/0/Android/data/package name/files

     */
    //从streamingasset 读取，android为apk的asset目录下
    // file 为相对路径
    static public byte[] ReadDataAsset(string file)
    {
        //string fileDir = Application.dataPath + "/StreamingAssets";
        string fileDir = Application.streamingAssetsPath;
        if (Common.isAndroid)
        {
            byte[] data = null;
            using (var javaClass = new AndroidJavaClass(JAVA_CLASS_FILEUTIL))
            {

                data = javaClass.CallStatic<byte[]>("ReadDataAsset", file);
            }
            return data;
        }
        if (Common.isiOS)
        {
            //fileDir = Application.streamingAssetsPath;

        }
        string filePath = fileDir + "/" + file;
        if (Common.isiOS)
        {
            //StreamAsseting目录 ios真机只读 不可写
            return ReadDataAssetIos(filePath);
        }
        return ReadData(filePath);
    }


    static public string ReadStringAsset(string file)
    {
        byte[] data = ReadDataAsset(file);
        if (data == null)
        {
            return null;
        }
        string str = Encoding.UTF8.GetString(data);
        return str;
    }

    static public bool FileIsExist(string file)
    {
        return File.Exists(file);
    }


    //file 为相对路径
    static public bool FileIsExistAsset(string file)
    {
        if (Common.isAndroid)
        {

            bool ret = true;
            using (var javaClass = new AndroidJavaClass(JAVA_CLASS_FILEUTIL))
            {

                ret = javaClass.CallStatic<bool>("FileIsExistAsset", file);
            }



            return ret;
        }

        string fileDir = Application.streamingAssetsPath;

        string filePath = fileDir + "/" + file;
        return File.Exists(filePath);
    }
    //文件名
    static public string GetFileName(string filepath)
    {
        string ret = "";
        int idx = filepath.LastIndexOf("/");
        if (idx >= 0)
        {
            string str = filepath.Substring(idx + 1);
            idx = str.LastIndexOf(".");
            if (idx >= 0)
            {
                ret = str.Substring(0, idx);
            }
        }
        else
        {
            idx = filepath.LastIndexOf(".");
            if (idx >= 0)
            {
                ret = filepath.Substring(0, idx);
            }
        }
        return ret;
    }

    //文件后缀
    static public string GetFileExt(string filepath)
    {
        string ret = "";
        int idx = filepath.LastIndexOf(".");
        if (idx >= 0)
        {
            ret = filepath.Substring(idx + 1);
        }
        return ret;
    }

    //除去文件后缀
    static public string GetFileBeforeExt(string filepath)
    {
        string ret = filepath;
        int idx = filepath.LastIndexOf(".");
        if (idx >= 0)
        {
            ret = filepath.Substring(0, idx + 1);
        }
        return ret;
    }

    //文件目录
    static public string GetFileDir(string filepath)
    {
        string ret = filepath;
        int idx = filepath.LastIndexOf("/");
        if (idx >= 0)
        {
            ret = filepath.Substring(0, idx);
        }
        return ret;
    }

    static public void CreateDir(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }


}