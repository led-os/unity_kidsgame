using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

//获得奖励
public class UITrophyGet : UIView
{
    public Image imageBg;
    public RawImage imageAnimate;

    public Image imageNumBg;
    public Text textNum;
    public UITrophyList uiTrophyList;
    List<UIImageTrophyGet> listImageTrophy;
    UIImageTrophyGet uiImageTrophyGetPrefab;
    float timeShow = 3f;
    ActionImage acImage;

    int numNewAddStar;//新增加的星星
    int numNewAddMedal;//新增加的奖牌
    int numNewAddCup;//新增加的奖杯

    void Awake()
    {
        listImageTrophy = new List<UIImageTrophyGet>();
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Trophy/UIImageTrophyGet");
            uiImageTrophyGetPrefab = obj.GetComponent<UIImageTrophyGet>();
        }
        for (int i = 0; i < 2; i++)
        {
            // CreateImageTrophyItem();
        }
        string pic = GetPicOfAnimate(0);
        TextureUtil.UpdateRawImageTexture(imageAnimate, pic, true);

        imageAnimate.gameObject.SetActive(false);
    }

    void Start()
    {
        numNewAddStar = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_STAR, 0) - PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_STAR_DISPLAY, 0);
        numNewAddMedal = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_MEDAL, 0) - PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_MEDAL_DISPLAY, 0);
        numNewAddCup = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_CUP, 0) - PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_CUP_DISPLAY, 0);

        LayOut();
        // RunAction();
        LayOut();
        //  Invoke("RunActionMoveTrophy", 0.5f);
    }

    void CreateImageTrophyItem()
    {
        int idx = listImageTrophy.Count;
        UIImageTrophyGet ui = (UIImageTrophyGet)GameObject.Instantiate(uiImageTrophyGetPrefab);
        ui.transform.SetParent(this.transform);
        RectTransform rctran = ui.GetComponent<RectTransform>();
        rctran.anchoredPosition = Vector2.zero;
        listImageTrophy.Add(ui);
        textNum.text = listImageTrophy.Count.ToString();
    }

    string GetPicOfAnimate(int idx)
    {
        return AppRes.IMAGE_TROPHY_Rotation_PREFIX + idx.ToString("D2") + ".png";
    }
    void RunAction()
    {
        imageAnimate.gameObject.SetActive(true);
        acImage = imageAnimate.gameObject.AddComponent<ActionImage>();
        acImage.duration = 10f;
        acImage.isLoop = true;
        for (int i = 0; i < 25; i++)
        {
            string pic = GetPicOfAnimate(i);
            //Debug.Log(pic);
            acImage.AddPic(pic);
        }

        acImage.Run();
    }
    public override void LayOut()
    {
        float x, y, w, h, ratio, scale;
        if (listImageTrophy.Count != 0)
        {
            UIImageTrophyGet ui = listImageTrophy[0];
            RectTransform rctran = ui.GetComponent<RectTransform>();
            RectTransform rctranImageNum = imageNumBg.GetComponent<RectTransform>();
            x = rctran.rect.width / 2;
            y = rctran.rect.height / 2;
            rctranImageNum.anchoredPosition = new Vector2(x, y);
        }
    }


    void RunActionMoveTrophy()
    {
        float x, y;
        int idx = 0;
        foreach (UIImageTrophyGet ui in listImageTrophy)
        {
            string pic = TrophyRes.GetImageOfIcon(TrophyRes.TYPE_Medal, 1);
            Texture2D tex = TextureCache.main.Load(pic);
            RectTransform rctran = ui.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(tex.width, tex.height);

            ui.UpdateItem(pic);
            // TextureUtil.UpdateImageTexture(imageTrophy, pic, true);
            Vector2 posNormal = ui.transform.position;
            //置顶
            ui.transform.SetAsLastSibling();
            // RectTransform rctran = imageTrophy.GetComponent<RectTransform>();
            Vector2 posEnd = uiTrophyList.GetPosOfBtnTrophy(idx);
            if (posEnd != Vector2.zero)
            {
                ui.transform.DOMove(posEnd, timeShow).OnComplete(() =>
                          {
                              // imageTrophy.transform.position = posNormal;
                              OnActionMoveTrophyFinish();
                          });
            }
            idx++;

        }

        LayOut();

    }

    void OnAddTrophyValue(string key, string key_display)
    {
        int v = PlayerPrefs.GetInt(key_display, 0) + 1;
        int v_limit = PlayerPrefs.GetInt(key, 0);
        if (v > v_limit)
        {
            v = v_limit;
        }
        PlayerPrefs.SetInt(key_display, v);

    }
    void OnActionMoveTrophyFinish()
    {
        OnAddTrophyValue(TrophyRes.KEY_TROPHY_NUM_STAR, TrophyRes.KEY_TROPHY_NUM_STAR_DISPLAY);
        OnAddTrophyValue(TrophyRes.KEY_TROPHY_NUM_MEDAL, TrophyRes.KEY_TROPHY_NUM_MEDAL_DISPLAY);
        OnAddTrophyValue(TrophyRes.KEY_TROPHY_NUM_CUP, TrophyRes.KEY_TROPHY_NUM_CUP_DISPLAY);
        OnAddTrophyValue(TrophyRes.KEY_TROPHY_NUM_CROWN, TrophyRes.KEY_TROPHY_NUM_CROWN_DISPLAY);

        imageNumBg.gameObject.SetActive(false);
        acImage.Pause();
        this.gameObject.SetActive(false);
        uiTrophyList.UpdateTable(true);
        AudioPlay.main.PlayFile(TrophyRes.AUDIO_TROPHY_GET_MEDAL_CUP);
    }

}



