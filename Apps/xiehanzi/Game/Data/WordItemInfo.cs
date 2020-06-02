using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordItemInfo : ItemInfo
{
    public List<object> listDemoPoint;//演示笔画的坐标点
    public List<object> listGuidePoint;//笔画提示图片坐标

    public List<string> listImageBihua0;//笔画图片0
    public List<string> listImageBihua1;//笔画图片1

    public string soundPutonghua;
    public string soundGuangdonghua;
    public int countBihua;
    public string imageLetter;
    public string thumbLetter;
    public string imageBihua;
    public int lineWidth;//基于图片大小 如 45
 
    public string date;
    public string addtime;

    public DBWordItemInfo dbInfo;

}


public class GuideItemInfo
{
    public const int IMAGE_TYPE_START = 0;
    public const int IMAGE_TYPE_MIDDLE_ANIMATE = 3;
    public const int IMAGE_TYPE_MIDDLE = 1;
    public const int IMAGE_TYPE_END = 2;

    public float angle;
    public Vector2 point;
    public int type;
    public int count;
    public int direction;
}
