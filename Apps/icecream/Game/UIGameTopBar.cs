using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIGameTopBar : UIView, IPopViewControllerDelegate
{

    public Button btnShop;
    public Button btnRestore;
    public Button btnStar;
    public Text textGold;
    public Button btnFree;

    public Button btnHome;
    public Button btnMusic;
    public Image imageTrophyBg;
    public Image imageTrophy;
    Tween tweenTrophy;
    void Awake()
    {
        UpdateGold();
        TextureUtil.UpdateButtonTexture(btnFree, Language.main.IsChinese() ? AppRes.IMAGE_BTN_FREE_cn : AppRes.IMAGE_BTN_FREE_en, true);
        UpdateBtnMusic();
    }

    void Start()
    {
        RunActionTrophy();
    }


    //position为屏幕坐标
    public Vector2 GetPosOfBtnTrophy()
    {
        return imageTrophy.transform.position;
    }
    //当得到奖牌或者奖杯时，奖杯镑按钮上的奖杯会一大一小的闪动
    void RunActionTrophy()
    {
        if (tweenTrophy == null)
        {
            float scale = 0.6f;
            tweenTrophy = imageTrophy.transform.DOScale(new Vector3(scale, scale, 2f), 1f).OnComplete(() =>
          {

          }).SetLoops(-1, LoopType.Yoyo);

        }
        tweenTrophy.Play();
    }

    public void UpdateGold()
    {

        string str = Common.gold.ToString();
        textGold.text = str;
        // int fontsize = textGold.fontSize;
        // float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        // RectTransform rctran = imageGoldBg.transform as RectTransform;
        // Vector2 sizeDelta = rctran.sizeDelta;

        // sizeDelta.x = str_w + fontsize;
        // rctran.sizeDelta = sizeDelta; 
    }

    public void UpdateBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        TextureUtil.UpdateButtonTexture(btnMusic, ret ? AppRes.IMAGE_BtnMusicOn : AppRes.IMAGE_BtnMusicOff, true);
    }
    public void OnPopViewControllerDidClose(PopViewController controller)
    {

    }
    public void OnClickBtnShop()
    {
        StarViewController p = StarViewController.main;
        p.SetType(StarViewController.TYPE_STAR_BUY);
        p.Show(null, null);
    }
    public void OnClickBtnRestore()
    {
        StarViewController p = StarViewController.main;
        p.SetType(StarViewController.TYPE_STAR_RESTORE);
        p.Show(null, null);
    }
    public void OnClickBtnStar()
    {

    }
    public void OnClickBtnFree()
    {

    }
    public void OnClickBtnHome()
    {
        GameViewController.main.gameBase.OnClickBtnBack();
    }
    public void OnClickBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        bool value = !ret;
        Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, value);
        if (value)
        {
            AudioPlay.main.Play();
        }
        else
        {
            AudioPlay.main.Stop();
        }
        UpdateBtnMusic();
    }
    public void OnClickBtnTrophy()
    {
        TrophyViewController.main.Show(null, null);
    }
}
