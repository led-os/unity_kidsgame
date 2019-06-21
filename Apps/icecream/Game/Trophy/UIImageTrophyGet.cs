using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

//获得奖励
public class UIImageTrophyGet : UIView
{
    public Image imageTrophy;


    void Awake()
    {
        imageTrophy.gameObject.SetActive(false);
        // UpdateItem(TrophyRes.GetImageOfIcon(TrophyRes.TYPE_Medal, 1));
        //  string pic = TrophyRes.GetImageOfIcon(TrophyRes.TYPE_Medal, 1);

        //  TextureUtil.UpdateImageTexture(imageTrophy, pic, true);
    }

    void Start()
    {

    }

    public override void LayOut()
    {
        float x, y, w, h, ratio, scale;
        RectTransform rctran = this.GetComponent<RectTransform>();
        RectTransform rctranImage = imageTrophy.GetComponent<RectTransform>();
        scale = Common.GetBestFitScale(rctranImage.rect.width, rctranImage.rect.height, rctran.rect.width, rctran.rect.height);
        imageTrophy.transform.localScale = new Vector3(scale, scale, 1f);
    }

    public void UpdateItem(string pic)
    {
        imageTrophy.gameObject.SetActive(true);
        TextureUtil.UpdateImageTexture(imageTrophy, pic, true);
        LayOut();
    }

}



