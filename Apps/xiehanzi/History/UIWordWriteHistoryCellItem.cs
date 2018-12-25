using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;

public class UIWordWriteHistoryCellItem : UICellItemBase
{

    public const int ITEM_TYPE_SORT = 0;
    public const int ITEM_TYPE_WORD = 1;
    public Image imageBg;
    public Text textTitle;
    public int itemType;
    public TableView tableView;

    Color colorSel;
    Color colorUnSel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetItemType(int type)
    {
        itemType = type;
        switch (itemType)
        {
            case ITEM_TYPE_SORT:
                //textTitle.
                break;
            case ITEM_TYPE_WORD:
                textTitle.text = "";
                break;
        }
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

    void UpdateInfo(WordItemInfo info)
    {
        //Sprite sp = Resources.Load(info.pic, typeof(Sprite)) as Sprite;
        //imageBg.sprite = sp;
        switch (itemType)
        {
            case ITEM_TYPE_SORT:
                {
                    if (Common.BlankString(info.id))
                    {
                        textTitle.text = DBWord.getDateDisplay(info.date);

                        TextureUtil.UpdateImageTexture(imageBg, UIGameXieHanzi.GetHistorySortCellItemBg(), true);
                    }
                    else
                    {
                        textTitle.text = "";
                        string str = UIGameXieHanzi.GetItemThumb(info.id);
                        if (FileUtil.FileIsExistAsset(str))
                        {
                            TextureUtil.UpdateImageTexture(imageBg, str, true);
                        }
                    }
                }

                break;
            case ITEM_TYPE_WORD:
                {

                    textTitle.text = "";//
                    if (FileUtil.FileIsExist(info.fileSaveWord))
                    {
                        TextureUtil.UpdateImageTexture(imageBg, info.fileSaveWord, true);
                    }


                }
                break;
        }
    }

    public override void UpdateItem(List<object> list)
    {
        WordItemInfo info = list[index] as WordItemInfo;
        UpdateInfo(info);

        LayOut();
    }
    public override bool IsLock()
    {

        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        RectTransform rctran = imageBg.GetComponent<RectTransform>();
        float ratio = 1f;
        if (Common.appType == AppType.SHAPECOLOR)
        {
            ratio = 0.9f;
        }
        float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, width, height) * ratio;
        imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

    }
}
