using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//index 0:奖励星  1：奖牌 2:奖杯
public class UITrophyCellItem : UICellItemBase
{
    public Image imageBg;

    public GameObject objLeft;
    public Image imageIconLeftBg;
    public Image imageIconLeft;
    public Image imageLevel;


    public GameObject objRight;
    public Image imageRight0;
    public Image imageRight1;
    public Image imageRight2;
    public Image imageRight3;
    public Image imageRight4;
    public Image imageRight5;
    public Image imageRight6;
    public Image imageRight7;
    public Image imageRight8;
    public Image imageRight9;

    Image[] listImage = new Image[10];
    Material matGrey;
    int type;
    private void Awake()
    {
        //  base.Awake();
        matGrey = new Material(Shader.Find("Custom/Grey"));
        Image[] listTmp = { imageRight0, imageRight1, imageRight2, imageRight3, imageRight4, imageRight5, imageRight6, imageRight7, imageRight8, imageRight9 };
        for (int i = 0; i < listTmp.Length; i++)
        {
            listImage[i] = listTmp[i];
        }


        Vector4 border = AppRes.borderCellTrophyBg;
        //TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_TROPHY_CELL_BG, false, border);
        TextureUtil.UpdateImageTexture(imageIconLeftBg, AppRes.IMAGE_ROOT_DIR_TROPHY + "/IconBoard.png", true);

    }



    public override void UpdateItem(List<object> list)
    {
        type = GetTrophyType();
        int level = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_STAR_DISPLAY, 0) / TrophyRes.ONE_CELL_NUM_STAR + 1;
        int group = level / TrophyRes.ONE_CELL_NUM_STAR + 1;//1-5
        if (index < list.Count)
        {
            ItemInfo info = list[index] as ItemInfo;
            tagValue = info.tag;


        }
        //level
        {
            //begain with 1
            int idx = level;
            string pic = AppRes.IMAGE_TROPHY_LEVEL_PREFIX + idx.ToString() + ".png";
            TextureUtil.UpdateImageTexture(imageLevel, pic, true);
        }

        //icon left
        {
            //begain with 1 

            string pic = TrophyRes.GetImageOfIcon(type, group);
            int num = 0;
            if (type == TrophyRes.TYPE_Star)
            {
                num = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_MEDAL_DISPLAY, 0);

            }
            if (type == TrophyRes.TYPE_Medal)
            {
                num = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_CUP_DISPLAY, 0);
            }
            if (type == TrophyRes.TYPE_Cup)
            {
                num = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_CROWN_DISPLAY, 0);
            }
            SetImageGrey(imageIconLeft, (num >= 1) ? false : true, pic);
        }


        //icon right 

        int num_hightlight = -1;
        if (type == TrophyRes.TYPE_Star)
        {
            num_hightlight = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_STAR_DISPLAY, 0) % TrophyRes.ONE_CELL_NUM_STAR;

        }
        if (type == TrophyRes.TYPE_Medal)
        {
            num_hightlight = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_MEDAL_DISPLAY, 0) % TrophyRes.ONE_CELL_NUM_MEDAL;
        }
        if (type == TrophyRes.TYPE_Cup)
        {
            num_hightlight = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_CUP_DISPLAY, 0) % TrophyRes.ONE_CELL_NUM_CUP;
        }
        Debug.Log("num_hightlight=" + num_hightlight);
        for (int i = 0; i < listImage.Length; i++)
        {
            Image image = listImage[i];
            //begain with 1 
            string pic = TrophyRes.GetImageOfIcon(type, group);
            SetImageGrey(image, i < num_hightlight ? false : true, pic);
        }


        if (type == TrophyRes.TYPE_Cup)
        {
            //2:奖杯
            for (int i = 5; i < listImage.Length; i++)
            {
                Image image = listImage[i];
                image.gameObject.SetActive(false);
            }

        }

        imageLevel.gameObject.SetActive(false);
        if (type == TrophyRes.TYPE_Star)
        {
            imageLevel.gameObject.SetActive(true);
        }
        LayOut();
    }

    int GetTrophyType()
    {
        int type = 0;
        switch (index)
        {
            case 0:
                {
                    type = TrophyRes.TYPE_Star;
                }
                break;
            case 1:
                {
                    type = TrophyRes.TYPE_Medal;
                }
                break;
            case 2:
                {
                    type = TrophyRes.TYPE_Cup;
                }
                break;

        }

        return type;
    }
    //变灰
    void SetImageGrey(Image image, bool enable, string pic)
    {
        //Shader "Custom/Grey" 
        // if (enable)
        // {
        //     image.material = matGrey;
        // }
        // else
        // {
        //     image.material = null;
        // }
        Texture2D tex = null;
        if (enable)
        {
            tex = TextureCache.main.Load(pic, matGrey);
        }
        else
        {
            tex = TextureCache.main.Load(pic);
        }
        TextureUtil.UpdateImageTexture(image, tex, true);
    }

    public override void LayOut()
    {
        float x, y, w, h, ratio, scale;
        RectTransform rctranLeft = objLeft.GetComponent<RectTransform>();
        RectTransform rctranRight = objRight.GetComponent<RectTransform>();
        GridLayoutGroup grid = objRight.GetComponent<GridLayoutGroup>();
        float height_level = 32;
        float oft_border_x = 48;
        {
            ratio = 0.8f;
            w = height * ratio - height_level * 2;
            h = w;

            rctranLeft.sizeDelta = new Vector2(w, h);
            x = -width / 2 + w / 2 + oft_border_x;
            y = 0;
            rctranLeft.anchoredPosition = new Vector2(x, y);

            scale = Common.GetBestFitScale(imageIconLeftBg.sprite.texture.width, imageIconLeftBg.sprite.texture.height, w, h);
            imageIconLeftBg.transform.localScale = new Vector3(scale, scale, 1f);
            scale = Common.GetBestFitScale(imageIconLeft.sprite.texture.width, imageIconLeft.sprite.texture.height, w, h);
            imageIconLeft.transform.localScale = new Vector3(scale, scale, 1f);
        }

        {
            RectTransform rctran = imageLevel.GetComponent<RectTransform>();
            w = rctranLeft.rect.width;
            h = height_level;
            x = 0;
            y = -rctranLeft.rect.height / 2 - h / 2;
            scale = Common.GetBestFitScale(imageLevel.sprite.texture.width, imageLevel.sprite.texture.height, w, h);
            imageLevel.transform.localScale = new Vector3(scale, scale, 1f);
            rctran.anchoredPosition = new Vector2(x, y);

        }
        {
            ratio = 0.8f;

            w = (width / 2 - oft_border_x) - (rctranLeft.anchoredPosition.x + rctranLeft.rect.width / 2);
            h = height * ratio;

            rctranRight.sizeDelta = new Vector2(w, h);
            x = (width / 2 - oft_border_x) - w / 2;
            y = 0;
            rctranRight.anchoredPosition = new Vector2(x, y);


            float cell_w = w / 5;
            float cell_h = h / 2;
            if (index == 2)
            {
                //2:奖杯 显示一行
                cell_h = h;
            }
            grid.cellSize = new Vector2(cell_w, cell_h);
        }



        //icon right 
        for (int i = 0; i < listImage.Length; i++)
        {
            Image image = listImage[i];
            scale = Common.GetBestFitScale(image.sprite.texture.width, image.sprite.texture.height, grid.cellSize.x, grid.cellSize.y);
            image.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    public Vector2 GetPosOfBtnTrophy()
    {
        Vector2 pos = imageIconLeft.transform.position;
        return pos;
    }
}



