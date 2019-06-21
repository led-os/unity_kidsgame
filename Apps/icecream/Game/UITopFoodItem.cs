using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public delegate void OnUITopFoodItemDidClickDelegate(UITopFoodItem item);


public class UITopFoodItem : UIView
{
    public const string SORT_chocolate = "chocolate";
    public const string SORT_egg = "egg";
    public const string SORT_wire = "wire";
    public const string SORT_sugar = "sugar";
    public const string SORT_cream = "cream";
    public const string SORT_fruit = "fruit";
    public const string SORT_scoop = "scoop";
    public enum Type
    {
        CUP,//杯子
        WAN,//碗
        FOOD,//顶料
        SUB_FOOD,//顶料子项 popselect bar
    }
    public const string PREFAB_TopFoodItem = "AppCommon/Prefab/Game/UITopFoodItem";
    public RawImage imageCup;
    public RawImage imageMultiColor;
    public RawImage imageFood;
    public RawImage imageWanBg;
    public RawImage imageLock;
    public RawImage imageHand;
    public int index;
    public float width;
    public float height;
    public bool enableLock = true;

   

    public const string IMAGE_WAN_BG = Common.GAME_RES_DIR + "/image/TopFoodBar/Wan/WanBg.png";
    public string strImageWan;
    public string strPic;
    UITouchEvent uITouchEvent;
    Tween tweenAlpha;
    public FoodItemInfo infoFood;
    public Type type;
    float scaleRatio = 0.65f;
    public OnUITopFoodItemDidClickDelegate callBackDidClick { get; set; }
    private void Awake()
    {
        uITouchEvent = this.gameObject.AddComponent<UITouchEvent>();
        // uITouchEvent.callBackTouch = OnUITouchEvent;
        TextureUtil.UpdateRawImageTexture(imageHand, AppRes.IMAGE_HAND, true);
        imageHand.gameObject.SetActive(false);

        TextureUtil.UpdateRawImageTexture(imageWanBg, IMAGE_WAN_BG, true);
    }
    public override void LayOut()
    {
        float x = 0, y = 0, w, h;
        RectTransform rctranCup = imageCup.GetComponent<RectTransform>();

        {
            w = imageCup.texture.width;
            h = imageCup.texture.height;
            float scale = Common.GetBestFitScale(w, h, width, height * scaleRatio);
            imageCup.transform.localScale = new Vector3(scale, scale, 1f);
        }
        if (imageMultiColor.texture != null)
        {
            w = imageMultiColor.texture.width;
            h = imageMultiColor.texture.height;
            float scale = Common.GetBestFitScale(w, h, width, height * scaleRatio);
            imageMultiColor.transform.localScale = new Vector3(scale, scale, 1f);
        }
        if (imageFood.texture != null)
        {
            w = imageFood.texture.width;
            h = imageFood.texture.height;
            float scale = Common.GetBestFitScale(w, h, width, height * scaleRatio);
            imageFood.transform.localScale = new Vector3(scale, scale, 1f);
        }

        if (imageWanBg.texture != null)
        {
            w = imageWanBg.texture.width;
            h = imageWanBg.texture.height;
            float scale = Common.GetBestFitScale(w, h, width, height * scaleRatio);
            imageWanBg.transform.localScale = new Vector3(scale, scale, 1f);
        }
        {
            w = imageLock.texture.width;
            h = imageLock.texture.height;
            float w_cup = rctranCup.rect.width * imageCup.transform.localScale.x;
            float h_cup = rctranCup.rect.height * imageCup.transform.localScale.y;
            float w_rect = w_cup / 2;
            float h_rect = h_cup / 2;

            float scale = Common.GetBestFitScale(w, h, w_rect, h_rect);
            imageLock.transform.localScale = new Vector3(scale, scale, 1f);

            RectTransform rctran = imageLock.GetComponent<RectTransform>();
            x = w_cup / 2 - w_rect / 2;
            y = -h_cup / 2 + h_rect / 2;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        {
            w = imageHand.texture.width;
            h = imageHand.texture.height;
            float scale = Common.GetBestFitScale(w, h, width, height * scaleRatio) * 0.7f;
            imageHand.transform.localScale = new Vector3(scale, scale, 1f);
            x = -width / 2;
            y = height * scaleRatio / 2;
            RectTransform rctran = imageHand.GetComponent<RectTransform>();
            rctran.anchoredPosition = new Vector2(x, y);
        }

    }
    static public string GetLockKey(string id, int idx)
    {
        return "KEY_LOCK_ITEM2_" + id + "_" + idx.ToString();
    }
    public void OnUnLockItem()
    {
        imageLock.gameObject.SetActive(false);
        if (infoFood != null)
        {
            string key = GetLockKey(infoFood.id, index);
            Common.SetBool(key, false);
            int num_star = PlayerPrefs.GetInt(TrophyRes.KEY_TROPHY_NUM_STAR, 0);
            num_star++;
            //记录解锁道具数量
            PlayerPrefs.SetInt(TrophyRes.KEY_TROPHY_NUM_STAR, num_star);
            //   

            int num_medal = num_star / TrophyRes.ONE_CELL_NUM_STAR;
            PlayerPrefs.SetInt(TrophyRes.KEY_TROPHY_NUM_MEDAL, num_medal);

            int num_cup = num_medal / TrophyRes.ONE_CELL_NUM_MEDAL;
            PlayerPrefs.SetInt(TrophyRes.KEY_TROPHY_NUM_CUP, num_cup);

            int num_crown = num_cup / TrophyRes.ONE_CELL_NUM_CUP;
            PlayerPrefs.SetInt(TrophyRes.KEY_TROPHY_NUM_CROWN, num_crown);

        }
    }

    public void UpdateItem(FoodItemInfo info)
    {
        infoFood = info;
        float x = 0, y = 0, w, h;
        {
            string pic = AppRes.IMAGE_CUP;
            TextureUtil.UpdateRawImageTexture(imageCup, pic, true);

        }
        imageWanBg.gameObject.SetActive(false);
        {
            string pic = "";
            switch (type)
            {
                case Type.CUP:
                    pic = info.pic;
                    imageMultiColor.gameObject.SetActive(false);
                    if (info.isSingleColor == false)
                    {
                        imageMultiColor.gameObject.SetActive(true);

                        if (!Common.BlankString(info.picMultiColor))
                        {
                            // Debug.Log("info.picMultiColor=" + info.picMultiColor);
                            TextureUtil.UpdateRawImageTexture(imageMultiColor, info.picMultiColor, true);
                        }

                    }
                    break;
                case Type.WAN:
                    imageWanBg.gameObject.SetActive(true);
                    imageCup.gameObject.SetActive(false);
                    imageMultiColor.gameObject.SetActive(false);
                    pic = IronIceCreamStepBase.GetImageOfWan(index);
                    strImageWan = pic;
                    break;
                case Type.FOOD:
                    imageCup.gameObject.SetActive(false);
                    imageMultiColor.gameObject.SetActive(false);
                    //横选择条里的分类是不用加锁的，只有竖选择条里的道具才按规则加锁
                    info.isLock = false;
                    //  pic = IronIceCreamStepBase.GetImageOfTopFood(index);
                    pic = info.pic;
                    break;
                case Type.SUB_FOOD:
                    imageCup.gameObject.SetActive(false);
                    imageMultiColor.gameObject.SetActive(false);
                    pic = IronIceCreamStepBase.GetImageOfTopFoodSubFood(index);
                    break;
            }
            strPic = pic;
            if (!Common.BlankString(info.pic))
            {
                TextureUtil.UpdateRawImageTexture(imageFood, pic, true);
            }

        }

        {

            string pic = GameIronIceCream.IMAGE_LOCK;
            TextureUtil.UpdateRawImageTexture(imageLock, pic, true);

            imageLock.gameObject.SetActive(false);
            if (info.isLock && enableLock)
            {
                imageLock.gameObject.SetActive(true);
            }
        }


        LayOut();
    }

    public void ShowHand(bool isShow, bool isAnimation)
    {
        imageHand.gameObject.SetActive(isShow);
        if (isAnimation)
        {

            // ActionBlink ac = this.gameObject.AddComponent<ActionBlink>();
            // ac.duration = 3f;
            // ac.count = 75;
            // ac.target = imageHand.gameObject;
            // ac.isLoop = true;
            // ac.Run(); 
            float duration = 1f;

            //ng：这种方法淡入淡出会改变整个ui的alpha
            // imageHand.material.DOFade(0, duration).SetLoops(-1, LoopType.Yoyo);
            //imageHand.color = Color.white;
            if (tweenAlpha == null)
            {
                tweenAlpha = DOTween.ToAlpha(() => imageHand.color, x => imageHand.color = x, 0f, duration).SetLoops(-1, LoopType.Yoyo);
            }
            tweenAlpha.Play();

        }
        else
        {

            if (tweenAlpha != null)
            {
                tweenAlpha.Pause();
            }
            //imageHand.color = Color.white;
        }
    }

    public void OnClickItem()
    {
        if (type == Type.FOOD)
        {
            UIPopSelectBar.indexFoodSort = index;
            UIPopSelectBar.countFoodSort = IronIceCreamStepBase.countTopFoodSort[index];
        }
        if (callBackDidClick != null)
        {
            callBackDidClick(this);
        }
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {

                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {

                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {

                }
                break;
        }
    }
}
