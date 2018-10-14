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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void CreateGuankaJsonFile()
    {
        string strPlace = "树木";
        string path = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR + "/image/" + strPlace;
        string path_new = path + "_new";
        int width_save = 1024;
        int height_save = 768;
        //创建文件夹
        Directory.CreateDirectory(path_new);

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
                //重命名
                //filepath_new = path + "/" + name;
                // NextFile.MoveTo(filepath_new);

                GuankaJsonItemInfo info = new GuankaJsonItemInfo();
                //info.pic = NextFile.Name;
                info.id = FileUtil.GetFileName(NextFile.Name);
                listGuankaJson.Add(info);

                idx++;
            }

        }

        //save guanka json
        {

            Hashtable data = new Hashtable();
            data["place"] = strPlace;
            data["items"] = listGuankaJson;
            string strJson = JsonMapper.ToJson(data);
            //Debug.Log(strJson);
            string filepath = path_new + "/guanka.json";
            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }

        Debug.Log("CreateGuankaJsonFile Finished");

    }
    public void OnClickBtnGuanka()
    {
        CreateGuankaJsonFile();
    }
}
