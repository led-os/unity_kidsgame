using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
public class GuankaJsonItemInfo
{
    //public string pic;//0,0,100,100
    public string id;
}
public class AutoGuankaJson : ScriptBase
{
    List<GuankaJsonItemInfo> listGuankaJson;

    List<string> listImage;

    // Use this for initialization
    void Start()
    {
        listImage = new List<string>();

        string dirRoot = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR + "/image/";

        listImage.Add(dirRoot + "Livingroom");
        listImage.Add(dirRoot + "Kitchen");
        listImage.Add(dirRoot + "School");



    }

    // Update is called once per frame
    void Update()
    {

    }

    void ConvertImage(string pathImgae, string dirname, GuankaJsonItemInfo info, string ext, int w, int h)
    {
        Texture2D tex = LoadTexture.LoadFromFile(pathImgae);
        float scale = 1f;
        string dir = dirname + "_" + w;
        {
            scale = Common.GetBestFitScale(tex.width, tex.height, w, h);
            w = (int)(tex.width * scale);
            h = (int)(tex.height * scale);
            Debug.Log("autoguankajson scale=" + scale + " tex.width=" + tex.width);
            Texture2D texNew = TextureUtil.ConvertSize(tex, w, h);
            FileUtil.CreateDir(dir);
            string filepath_new = dir + "/" + info.id + "." + ext;
            TextureUtil.SaveTextureToFile(texNew, filepath_new);
        }
    }

    void CreateGuankaJsonFile(string path)
    {
        string strPlace = FileUtil.GetFileName(path);
        //  string path = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR + "/image/" + strPlace;
        string path_new = path + "_new";
        // int width_save = 1024;
        // int height_save = 768;
        //创建文件夹
        // Directory.CreateDirectory(path_new);
        string strLanguageContent = "KEY,CN,EN\n";
        listGuankaJson = new List<GuankaJsonItemInfo>();
        // C#遍历指定文件夹中的所有文件 
        DirectoryInfo TheFolder = new DirectoryInfo(path);
        int idx = 0;
        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            string fullpath = NextFile.ToString();
            //1.jpg
            // Debug.Log(NextFile.Name);
            string ext = FileUtil.GetFileExt(fullpath);
            if ((ext == "png") || (ext == "jpg"))
            {
                string name = idx.ToString() + "." + ext;
                string filepath_new = NextFile.ToString();
                GuankaJsonItemInfo info = new GuankaJsonItemInfo();
                //info.pic = NextFile.Name;
                info.id = FileUtil.GetFileName(NextFile.Name);

                ConvertImage(fullpath, path, info, ext, 1024, 1024);
                ConvertImage(fullpath, path, info, ext, 256, 256);


                //重命名
                //filepath_new = path + "/" + name;
                // NextFile.MoveTo(filepath_new);


                listGuankaJson.Add(info);
                strLanguageContent += info.id + ",,\n";

                idx++;
            }

        }

        //save guanka json
        {

            Hashtable data = new Hashtable();
            data["type"] = strPlace;
            data["items"] = listGuankaJson;
            string strJson = JsonMapper.ToJson(data);
            //Debug.Log(strJson);
            string filepath = path_new + "/guanka.json";
            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }

        //save language
        {
            string filepath = path_new + "/" + strPlace + ".csv";
            SaveString(filepath, strLanguageContent);
        }

        Debug.Log("CreateGuankaJsonFile Finished");

    }

    public void SaveString(string Path, string content)
    {
        FileStream aFile = new FileStream(Path, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(aFile);
        sw.Write(content);
        sw.Close();
        sw.Dispose();
    }
    public void OnClickBtnGuanka()
    {

        foreach (string pic in listImage)
        {
            CreateGuankaJsonFile(pic);
        }
    }
}
