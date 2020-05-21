using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;
public delegate void OnUILoveCellItemDelegate(UILoveCellItem ui);
public class UILoveCellItem : UICellItemBase
{
    /*
    纤夫的爱
    男：妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠
    妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠
    女：小妹妹我坐船头，哥哥你在岸上走，我俩的情我俩的爱
    在纤绳上荡悠悠，噢荡悠悠
    你一步一叩首啊，没有别的乞求
    只盼拉住我妹妹的手哇，跟你并肩走，噢……
    男：妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠
    妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠


    女：小妹妹我坐船头，哥哥你在岸上走，我俩的情我俩的爱
    在纤绳上荡悠悠，噢荡悠悠
    你汗水洒一路啊，泪水在我心里流
    只盼日头它落西山沟哇，让你亲个够，噢……
    男：妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠
    妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠



    女：小妹妹我坐船头，哥哥你在岸上走，我俩的情我俩的爱
    在纤绳上荡悠悠，噢荡悠悠
    你汗水洒一路啊，泪水在我心里流
    只盼日头它落西山沟哇，让你亲个够，噢……
    男：妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠
    妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠
    妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠
    妹妹你坐船头，哥哥在岸上走，恩恩爱爱纤绳荡悠悠

    */

  /*
                滑板的爱
    男： 小姐姐滑板头，小哥哥沙滩跑，开开心心滑板飞溜溜
    小姐姐滑板头，小哥哥沙滩跑，开开心心滑板飞溜溜
    女：小姐姐我滑板头，小哥哥在沙滩跑，我们的情我们的爱
    在滑板上飞溜溜，呀飞溜溜
    你一步一跨越啊，没有别的目的
    只愿牵着我妹妹的手哇，跟你一起飞，噢……
    男：小姐姐滑板头，小哥哥沙滩跑，开开心心滑板飞溜溜
    小姐姐滑板头，小哥哥沙滩跑，开开心心滑板飞溜溜


    女：小姐姐滑板头，小哥哥沙滩跑，我们的情我们的爱
    在滑板上飞溜溜，呀飞溜溜
    你热情洒一滩啊，感动在我心里乐
    只等太阳它滑山崖下呀，让你吻个嗨，噢……
    小姐姐滑板头，小哥哥沙滩跑，开开心心滑板飞溜溜
    小姐姐滑板头，小哥哥沙滩跑，开开心心滑板飞溜溜

    */

    public UIImage imageBg;
    public UIButton btnDelete;
    public UIText textTitle;
    public UIText textPinyin;
    public float itemWidth;
    public float itemHeight;
    public Color colorSel;
    public Color colorUnSel;
    IdiomItemInfo infoItem;
    public OnUILoveCellItemDelegate callbackClickDelete { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LevelManager.main.ParseGuanka();
        // Common.SetButtonText(btnDelete, Language.main.GetString("STR_IdiomDetail_DELETE_LOVE"), 0, false);
    }

    private void Start()
    {
        LayOut();
    }
    public override void UpdateItem(List<object> list)
    {
        infoItem = list[index] as IdiomItemInfo;
        IdiomParser.main.ParseIdiomItem(infoItem);
        // infoItem = DBIdiom.main.GetItemById(infoItem.id);
        UpdateInfo(infoItem);  //  LayOut();
        Invoke("LayOut", 0.2f);

    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        base.LayOut();

    }
    void SetSelect(bool isSel)
    {
        if (isSel)
        {
            textTitle.color = colorSel;
        }
        else
        {
            textTitle.color = colorUnSel;
        }
    }

    public void UpdateInfo(IdiomItemInfo info)
    {
        textTitle.text = info.title;
        textPinyin.text = info.pronunciation;
        LayOut();

    }

    public void OnClickBtnDelete()
    {
        if (this.callbackClickDelete != null)
        {
            this.callbackClickDelete(this);
        }
    }

}
