using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class WordXmlPoint2Json : MonoBehaviour
{
    public string GAME_RES_DIR = Common.GAME_RES_DIR + "/image";

    public string GAME_RES_DIR_NEW;
    public string GAME_RES_DIR_OLD;

    public GameObject objSpriteWord;
    float scaleWorld;
    List<EditorItemPoint> listJsonPoint;
    float letterImageZ = -20f;
    int indexBihua;
    float widthImage;
    float heightImage;
    float guideImageOffsetX;
    float guideImageOffsetY;
    Vector2 sizeGuideImage;
    Bounds boundsLetter;

    Hashtable dataJsonRoot;
    Texture2D texWord;
    UIGameBase gameBase;
    UIGameXieHanzi uiGameXieHanzi;

    //xml 
    Dictionary<string, object> dicRoot;
    float imageScaleFactor = 2;//@2x png
    int gameLevel;
    List<object> listGuanka;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GAME_RES_DIR_NEW = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR + "_New";
        GAME_RES_DIR_OLD = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR + "/image";
        dataJsonRoot = new Hashtable();
        listJsonPoint = new List<EditorItemPoint>();
        indexBihua = 0;
        gameLevel = 0;

        gameBase = GameViewController.main.gameBase;
        uiGameXieHanzi = gameBase as UIGameXieHanzi;
        //gameXieHanzi.ParseGuanka2();
        GameManager.placeLevel = 0;

        ParseGuankaXml();
        //listGuanka = gameBase.listGuanka;

    }
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {


    }



    string GetImageGuidePic(int type)
    {
        string strPic = "";
        string strDirRoot = GAME_RES_DIR + "/common";
        switch (type)
        {
            case GuideItemInfo.IMAGE_TYPE_START:
                strPic = strDirRoot + "/writing_guide_point_start@2x~ipad.png";
                //
                break;
            case GuideItemInfo.IMAGE_TYPE_MIDDLE_ANIMATE:
                strPic = strDirRoot + "/writing_guide_point_animating@2x~ipad.png";
                break;
            case GuideItemInfo.IMAGE_TYPE_MIDDLE:
                strPic = strDirRoot + "/writing_guide_point_middle@2x~ipad.png";

                break;


            case GuideItemInfo.IMAGE_TYPE_END:
                strPic = strDirRoot + "/writing_guide_point_end@2x~ipad.png";

                break;

            default:
                strPic = strDirRoot + "/writing_guide_point_animating@2x~ipad.png";
                break;

        }
        return strPic;
    }


    WordItemInfo GetItemInfo(int idx)
    {
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        WordItemInfo info = listGuanka[idx] as WordItemInfo;
        return info;
    }

    Texture2D GetTextureOfLetter(string pic) //tex坐标
    {
        Texture2D tex = LoadTexture.LoadFromFile(pic);
        if (tex == null)
        {
            tex = LoadTexture.LoadFromAsset(pic);
        }
        return tex;
    }

    void SaveJson(WordItemInfo info, int count)
    {
        dataJsonRoot["count"] = count;
        // dataJsonRoot["width"] = texWord.width;
        // dataJsonRoot["height"] = texWord.height;

        string strJson = JsonMapper.ToJson(dataJsonRoot);
        //Debug.Log(strJson);
        string strDir = GAME_RES_DIR_NEW + "/image/" + info.id;
        //创建文件夹
        if (!Directory.Exists(strDir))
        {
            Directory.CreateDirectory(strDir);
        }


        string filepath = strDir + "/" + info.id + ".json";


        //string filepath = Application.streamingAssetsPath + "/" + info.id + ".json";
        byte[] bytes = Encoding.UTF8.GetBytes(strJson);
        System.IO.File.WriteAllBytes(filepath, bytes);
    }

    void CreateGuankaJsonFile()
    {
        List<GuankaJsonItemInfo> listGuankaJson = new List<GuankaJsonItemInfo>();
        foreach (WordItemInfo info in listGuanka)
        {
            GuankaJsonItemInfo infoJson = new GuankaJsonItemInfo();

            infoJson.id = info.id;
            listGuankaJson.Add(infoJson);
        }

        // //save guanka json
        int idx = GameManager.placeLevel;
        string fileName = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        string filepath = Application.streamingAssetsPath + "/" + fileName;
        {

            Hashtable data = new Hashtable();
            data["place"] = "place";
            data["items"] = listGuankaJson;
            string strJson = JsonMapper.ToJson(data);
            //Debug.Log(strJson);

            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }


    }
    void CopyFile(string src, string dst)
    {
        string dir = FileUtil.GetFileDir(dst);
        //创建文件夹
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.Copy(src, dst, true);
    }
    void CopyImageFiles(WordItemInfo infoFrom, WordItemInfo infoTo, Texture2D texOfLetter, Rect rcOfLetter)
    {
        //Directory.Move


        //笔顺图片 
        for (int i = 0; i < infoFrom.listImageBihua0.Count; i++)
        {
            string strfrom = (string)infoFrom.listImageBihua0[i];
            string strto = (string)infoTo.listImageBihua0[i];
            Texture2D tex = GetTextureOfLetter(strfrom);
            Texture2D texSUb = TextureUtil.GetSubTexture(tex, rcOfLetter);
            //CopyFile(strfrom, strto);
            TextureUtil.SaveTextureToFile(texSUb, strto);
        }
        //pic
        {
            Texture2D tex = texOfLetter;
            Texture2D texSUb = TextureUtil.GetSubTexture(tex, rcOfLetter);
            // CopyFile(infoFrom.pic, infoTo.pic);
            TextureUtil.SaveTextureToFile(texSUb, infoTo.pic);
        }
        //thumb
        CopyFile(infoFrom.thumbLetter, infoTo.thumbLetter);
        //image  
        CopyFile(infoFrom.imageLetter, infoTo.imageLetter);

        //笔顺示意图

        CopyFile(infoFrom.imageBihua, infoTo.imageBihua);

        //普通话
        CopyFile(infoFrom.soundPutonghua, infoTo.soundPutonghua);

        //广东话
        CopyFile(infoFrom.soundGuangdonghua, infoTo.soundGuangdonghua);

    }

    void Xml2JsonItem(WordItemInfo info, Texture2D texOfLetter, Rect rcOfLetter)
    {
        ParserGuankaItemXml(info);

        if (dataJsonRoot == null)
        {
            dataJsonRoot = new Hashtable();
        }
        int pic_w = texOfLetter.width;
        int pic_h = texOfLetter.height;//1536;
        indexBihua = 0;

        //demo point
        Hashtable dataDemoPoint = new Hashtable();
        List<object> listDemoPoint = info.listDemoPoint;
        foreach (List<object> listPoint in listDemoPoint)
        {
            string key = "bihua_" + indexBihua;
            // if (!dataDemoPoint.Contains(key))
            {
                List<JsonItemPoint> listtmp = new List<JsonItemPoint>();
                foreach (Vector2 pt in listPoint)
                {
                    JsonItemPoint jsonitem = new JsonItemPoint();
                    int x = (int)(pt.x - rcOfLetter.x);
                    int y = (int)((pic_h - 1 - pt.y) - rcOfLetter.y);
                    jsonitem.x = x.ToString();
                    jsonitem.y = y.ToString();
                    listtmp.Add(jsonitem);
                }
                dataDemoPoint[key] = listtmp;
                indexBihua++;
            }

        }
        string keyDemoPoint = "demo_point";
        dataJsonRoot[keyDemoPoint] = dataDemoPoint;

        //guide point
        int idx = 0;
        Hashtable dataGuidePoint = new Hashtable();
        foreach (List<object> listPoint in info.listGuidePoint)
        {
            string key = "bihua_" + idx;
            List<JsonGuideItemInfo> listJson = new List<JsonGuideItemInfo>();
            foreach (GuideItemInfo guideInfo in listPoint)
            {
                JsonGuideItemInfo jsonInfo = new JsonGuideItemInfo();
                jsonInfo.angle = guideInfo.angle.ToString();
                jsonInfo.type = guideInfo.type.ToString();
                jsonInfo.direction = guideInfo.direction.ToString();

                string strPic = GetImageGuidePic(guideInfo.type);
                Texture2D tex = LoadTexture.LoadFromAsset(strPic);
                //偏移量
                guideInfo.point.x += (tex.width / 2);
                guideInfo.point.y += (tex.height / 2);

                int x = (int)(guideInfo.point.x - rcOfLetter.x);
                int y = (int)((pic_h - 1 - guideInfo.point.y) - rcOfLetter.y);
                jsonInfo.x = x.ToString();
                jsonInfo.y = y.ToString();
                listJson.Add(jsonInfo);
            }
            //GuideItemInfo
            dataGuidePoint[key] = listJson;
            idx++;
        }
        string keyGuidePoint = "guide_point";
        dataJsonRoot[keyGuidePoint] = dataGuidePoint;

        //save  json 
        SaveJson(info, indexBihua);
    }

    public void ParserGuankaItemXml(WordItemInfo info)
    {

        Dictionary<string, object> dic = dicRoot[info.id] as Dictionary<string, object>;
        Dictionary<string, object> dicWord = dic["iPad"] as Dictionary<string, object>;

        string strDirRoot = GAME_RES_DIR_OLD;

        string strBg = dicWord["bg_image_name"] as string;
        info.pic = strDirRoot + "/letter/" + "letter_" + info.id + "@2x~ipad.png";

        //笔顺示意图
        info.imageBihua = strDirRoot + "/letter/" + "letter_stroke_" + info.id + "@2x~ipad.png";

        Debug.Log(strBg);

        string strWidth = dicWord["stroke_width"] as string;
        info.lineWidth = Common.String2Int(strWidth);

        //去掉_cn 如 green_cn 变成 green
        string strkey = info.id;
        int idx = info.id.IndexOf("_cn");
        if (idx >= 0)
        {
            strkey = info.id.Substring(0, idx);
        }



        //thumb
        info.thumbLetter = strDirRoot + "/letter_thumb/L_" + info.id + "@2x~ipad.png";
        //image 
        info.imageLetter = strDirRoot + "/letter_image/element_" + strkey + "@2x~ipad.png";
        //sound
        string strDirRootSound = "App/Game/hanziyuan";
        //普通话
        info.soundPutonghua = strDirRootSound + "/sound/pronunciation_pth_" + strkey;

        //广东话
        info.soundGuangdonghua = strDirRootSound + "/sound/pronunciation_canton_" + strkey;


        //示例笔画
        {
            List<object> listDemo = dicWord["demo_writing_points"] as List<object>;
            Debug.Log("Demo Count:" + listDemo.Count);
            info.listDemoPoint = new List<object>();
            foreach (List<object> listBihua in listDemo)
            {
                //Debug.Log("listBihua Count:" + listBihua.Count);
                List<object> listPoint = new List<object>();
                foreach (Dictionary<string, object> dicItem in listBihua)
                {
                    //start_point 文件读取的点坐标是居于1024x768图片的坐标
                    string direction = dicItem["direction"] as string;
                    string start_point = dicItem["start_point"] as string;
                    Vector2 ptStart = uiGameXieHanzi.gameXieHanzi.WordString2Point(start_point);
                    string total_points = dicItem["total_points"] as string;
                    int total = Common.String2Int(total_points);
                    for (int i = 0; i < total; i++)
                    {
                        //分割成多个点
                        int direc = Common.String2Int(direction);
                        Vector2 pt = new Vector2(0, 0);
                        switch (direc)
                        {
                            case WordWrite.DIRECTION_BISHUN_DOWN://向下
                                pt.x = ptStart.x;
                                pt.y = ptStart.y + i;
                                break;
                            case WordWrite.DIRECTION_BISHUN_RIGHT://向右
                                pt.x = ptStart.x + i;
                                pt.y = ptStart.y;
                                break;
                            default:
                                break;
                        }
                        pt *= imageScaleFactor;
                        listPoint.Add(pt);
                    }

                    //  Debug.Log("direction:" + direction + " start_point:" + start_point + " total_points:" + total_points + " point" + point);
                }

                info.listDemoPoint.Add(listPoint);
            }
        }

        info.countBihua = info.listDemoPoint.Count;

        //提示图片 guide
        {
            info.listGuidePoint = new List<object>();
            List<object> listGuide = dicWord["guiding_point_images"] as List<object>;
            foreach (List<object> listBihua in listGuide)
            {
                //Debug.Log("listBihua Count:" + listBihua.Count);
                List<object> listPoint = new List<object>();
                foreach (Dictionary<string, object> dicItem in listBihua)
                {
                    string direction = "0";
                    string count = "1";
                    //start_point 文件读取的点坐标是居于1024x768图片的坐标
                    if (dicItem.ContainsKey("direction"))
                    {
                        direction = dicItem["direction"] as string;
                    }
                    if (dicItem.ContainsKey("count"))
                    {
                        count = dicItem["count"] as string;
                    }

                    string angle = dicItem["angle"] as string;
                    string type = dicItem["type"] as string;
                    string position = dicItem["position"] as string;

                    Vector2 ptStart = uiGameXieHanzi.gameXieHanzi.WordString2Point(position);

                    int total = Common.String2Int(count);
                    for (int i = 0; i < total; i++)
                    {
                        GuideItemInfo guideItemInfo = new GuideItemInfo();
                        guideItemInfo.angle = -Common.String2Int(angle);
                        guideItemInfo.direction = Common.String2Int(direction);
                        guideItemInfo.type = Common.String2Int(type);

                        //分割成多个点
                        int direc = Common.String2Int(direction);
                        Vector2 pt = new Vector2(0, 0);
                        switch (direc)
                        {
                            case WordWrite.DIRECTION_BISHUN_DOWN://向下
                                pt.x = ptStart.x;
                                pt.y = ptStart.y + i;
                                break;
                            case WordWrite.DIRECTION_BISHUN_RIGHT://向右
                                pt.x = ptStart.x + i;
                                pt.y = ptStart.y;
                                break;
                            default:
                                break;
                        }
                        pt *= imageScaleFactor;
                        guideItemInfo.point = pt;
                        listPoint.Add(guideItemInfo);
                    }

                    //  Debug.Log("direction:" + direction + " start_point:" + start_point + " total_points:" + total_points + " point" + point);
                }

                info.listGuidePoint.Add(listPoint);
            }
        }



        //笔顺图片

        info.listImageBihua0 = new List<string>();
        info.listImageBihua1 = new List<string>();
        for (int i = 0; i < info.countBihua; i++)
        {
            string strtmp0 = strDirRoot + "/letter/letter_" + info.id + "_" + (i + 1) + "@2x~ipad.png";
            string strtmp1 = strDirRoot + "/letter/letter_" + info.id + (i + 1) + "@2x~ipad.png";

            //info.listImageBihua1.Add(strtmp1);
            if (FileUtil.FileIsExistAsset(strtmp0))
            {
                info.listImageBihua0.Add(strtmp0);
            }
            else
            {
                info.listImageBihua0.Add(strtmp1);
            }

        }



    }

    int ParseGuankaXml()
    {
        int count = 0;
        long tickGuanka = Common.GetCurrentTimeMs();
        string fileName = GAME_RES_DIR + "/LetterStructure~ipad_bak.xml";

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();
        int idx = GameManager.placeLevel;
        long tickPlist = Common.GetCurrentTimeMs();
        Plist2Dictionary plist = new Plist2Dictionary();
        plist.LoadFileAsset(fileName);
        tickPlist = Common.GetCurrentTimeMs() - tickPlist;
        dicRoot = plist.dicRoot;

        count = dicRoot.Count;
        Debug.Log("ParseGuanka:count=" + count);

        long tickParse = Common.GetCurrentTimeMs();
        foreach (string key in dicRoot.Keys)
        {
            WordItemInfo info = new WordItemInfo();
            info.id = key;

            if ((info.id == "boat") || (info.id == "whole") || (info.id == "father2"))
            {
                //过滤错别字
                continue;
            }

            //_tw 过滤繁体字
            int idxtmp = info.id.IndexOf("_tw");
            if (idxtmp >= 0)
            {
                continue;
            }

            //thumb
            info.icon = GetItemThumb(info.id);//strDirRoot + "/letter_thumb/L_" + info.id + "@2x~ipad.png";


            listGuanka.Add(info);
            //Debug.Log("ParseGuanka:key="+key); 
        }
        tickParse = Common.GetCurrentTimeMs() - tickParse;
        WordItemInfo infonow = GetItemInfo(gameLevel);
        long tickItem = Common.GetCurrentTimeMs();
        ParserGuankaItemXml(infonow);
        tickItem = Common.GetCurrentTimeMs() - tickItem;

        count = listGuanka.Count;
        tickGuanka = Common.GetCurrentTimeMs() - tickGuanka;
        Debug.Log("ParseGame::count=" + count + " tickGuanka=" + tickGuanka + " tickItem=" + tickItem + " tickPlist=" + tickPlist + " tickParse=" + tickParse);
        return count;
    }

    string GetItemThumb(string id)
    {
        string strDirRoot = GAME_RES_DIR;
        string ret = strDirRoot + "/letter_thumb/L_" + id + "@2x~ipad.png";

        return ret;
    }

    void UpdateWordImageInfoXml(WordItemInfo info)
    {

        // xml 
        string strDirRoot = GAME_RES_DIR_OLD;
        //去掉_cn 如 green_cn 变成 green
        string strkey = info.id;
        int idx = info.id.IndexOf("_cn");
        if (idx >= 0)
        {
            strkey = info.id.Substring(0, idx);
        }
        info.pic = strDirRoot + "/letter/" + "letter_" + info.id + "@2x~ipad.png";


        info.imageBihua = strDirRoot + "/letter/" + "letter_stroke_" + info.id + "@2x~ipad.png";

        info.thumbLetter = strDirRoot + "/letter_thumb/L_" + info.id + "@2x~ipad.png";
        //image 
        info.imageLetter = strDirRoot + "/letter_image/element_" + strkey + "@2x~ipad.png";

        //sound
        string strDirRootSound = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR;
        //普通话
        info.soundPutonghua = strDirRootSound + "/sound/pronunciation_pth_" + strkey + ".mp3";

        //广东话
        info.soundGuangdonghua = strDirRootSound + "/sound/pronunciation_canton_" + strkey + ".mp3";

        //笔顺图片

        if (info.listImageBihua0 == null)
        {
            info.listImageBihua0 = new List<string>();
        }
        info.listImageBihua0.Clear();

        for (int i = 0; i < info.countBihua; i++)
        {
            string strtmp0 = strDirRoot + "/letter/letter_" + info.id + "_" + (i + 1) + "@2x~ipad.png";
            string strtmp1 = strDirRoot + "/letter/letter_" + info.id + (i + 1) + "@2x~ipad.png";

            //info.listImageBihua1.Add(strtmp1);
            if (FileUtil.FileIsExist(strtmp0))
            {
                info.listImageBihua0.Add(strtmp0);
            }
            else
            {
                info.listImageBihua0.Add(strtmp1);
            }

        }

    }
    void UpdateWordImageInfoJson(WordItemInfo info)
    {
        string strDirRoot = GAME_RES_DIR_NEW;
        //创建文件夹
        if (!Directory.Exists(strDirRoot))
        {
            Directory.CreateDirectory(strDirRoot);
        }
        string strDirRootImage = strDirRoot + "/image/" + info.id;
        info.pic = strDirRootImage + "/" + info.id + ".png";

        //thumb
        info.thumbLetter = strDirRootImage + "/" + info.id + "_thumb.png";
        //image 
        info.imageLetter = strDirRootImage + "/" + info.id + "_image.png";
        //笔顺示意图
        info.imageBihua = strDirRootImage + "/" + info.id + "_stroke.png";


        //笔顺图片
        if (info.listImageBihua0 == null)
        {
            info.listImageBihua0 = new List<string>();
        }
        info.listImageBihua0.Clear();

        for (int i = 0; i < info.countBihua; i++)
        {
            string strtmp0 = strDirRootImage + "/bihua/" + info.id + "_" + i + ".png";
            info.listImageBihua0.Add(strtmp0);
        }

        //sound 
        //普通话
        info.soundPutonghua = strDirRoot + "/sound/" + info.id + "_cn.mp3";

        //广东话
        info.soundGuangdonghua = strDirRoot + "/sound/" + info.id + "_gd.mp3";


    }


    public void OnClickBtnGuankaJson()
    {
        Debug.Log("OnClickBtnGuankaJson start");
        foreach (WordItemInfo info in listGuanka)
        {
            //copy
            WordItemInfo infoFrom = new WordItemInfo();
            WordItemInfo infoTo = new WordItemInfo();
            infoFrom.id = info.id;
            infoFrom.countBihua = info.countBihua;
            infoTo.id = info.id;
            infoTo.countBihua = info.countBihua;
            UpdateWordImageInfoXml(infoFrom);
            UpdateWordImageInfoJson(infoTo);

            Texture2D texLetter = GetTextureOfLetter(infoFrom.pic);


            //计算笔划区域
            Rect rectLetter = TextureUtil.GetRectNotAlpha(texLetter);
            float oft = 8f;
            rectLetter.width += oft;
            rectLetter.x -= oft / 2;
            if (rectLetter.x < 0)
            {
                rectLetter.x = 0;
            }
            if (rectLetter.width > texLetter.width)
            {
                rectLetter.width = texLetter.width;
                rectLetter.x = 0;
            }
            rectLetter.height += oft;
            rectLetter.y -= oft / 2;
            if (rectLetter.y < 0)
            {
                rectLetter.y = 0;
            }
            if (rectLetter.height > texLetter.height)
            {
                rectLetter.height = texLetter.height;
                rectLetter.y = 0;
            }


            CopyImageFiles(infoFrom, infoTo, texLetter, rectLetter);

            Xml2JsonItem(info, texLetter, rectLetter);
        }
        Debug.Log("OnClickBtnGuankaJson end");
    }
}
