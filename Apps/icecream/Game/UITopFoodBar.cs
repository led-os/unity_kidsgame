using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public delegate void OnUITopFoodBarDidClickDelegate(UITopFoodBar bar, UITopFoodItem item);
// 顶料
public class UITopFoodBar : UIView
{
    public const int TOTAL_CUP_SingleColor = 42;
    public const int TOTAL_CUP_MultiColor = 42;
    public const int TOTAL_IMAGE_MultiColor = 6;
    public const int TOTAL_CUP = TOTAL_CUP_SingleColor + TOTAL_CUP_MultiColor;
    public const int TOTAL_WAN = 10;
    public int TOTAL_FOOD = IronIceCreamStepBase.strTopFoodSort.Length;//顶料分类
    public GameObject objScrollView;
    public GameObject objScrollViewContent;
    public Image imageBg;
    public Image imageHand;
    public List<object> listItem;
    public List<UITopFoodItem> listTopItem;
    ScrollRect scrollRect;
    UITopFoodItem uiTopFoodItemPrefab;
    float widthItem;
    Tweener twHand;
    public UITopFoodItem.Type type;

    public OnUITopFoodBarDidClickDelegate callBackDidClick { get; set; }

    void Awake()
    {
        listItem = new List<object>();
        listTopItem = new List<UITopFoodItem>();
        GameObject obj = PrefabCache.main.Load(UITopFoodItem.PREFAB_TopFoodItem);
        uiTopFoodItemPrefab = obj.GetComponent<UITopFoodItem>();
        scrollRect = objScrollView.GetComponent<ScrollRect>();

        UpdateType(UITopFoodItem.Type.CUP);
        TextureUtil.UpdateImageTexture(imageHand, AppRes.IMAGE_HAND, true);
        Vector3 posStart = Vector3.zero;
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        float x, y;
        x = rctran.rect.width / 2;
        y = -rctran.rect.height / 2;
        Vector3 posEnd = new Vector3(x, y, 0);

        RectTransform rctranHand = imageHand.GetComponent<RectTransform>();
        rctranHand.anchoredPosition = new Vector2(-x, 0);
        float scale = Common.GetBestFitScale(rctranHand.rect.width, rctranHand.rect.height, widthItem, widthItem) * 0.8f;
        imageHand.transform.localScale = new Vector3(scale, scale, 1);
        twHand = rctranHand.DOLocalMove(posEnd, 5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void UpdateType(UITopFoodItem.Type ty)
    {
        ClearItems();
        int total = TOTAL_CUP;
        if (ty == UITopFoodItem.Type.CUP)
        {
            total = TOTAL_CUP;
        }

        if (ty == UITopFoodItem.Type.WAN)
        {
            total = TOTAL_WAN;
        }
        if (ty == UITopFoodItem.Type.FOOD)
        {
            total = TOTAL_FOOD;
        }

        for (int i = 0; i < total; i++)
        {
            AddItem();
        }
    }


    void ClearItems()
    {
        foreach (UITopFoodItem item in listTopItem)
        {
            DestroyImmediate(item.gameObject);
        }
        listTopItem.Clear();
        listItem.Clear();
    }

    public void AddItemInfo()
    {
        int idx = listItem.Count;
        FoodItemInfo info = new FoodItemInfo();
        info.type = type;
        info.index = idx;
        info.isLock = false;
        info.isSingleColor = true;


        string pic = "";
        string key = "";

        switch (type)
        {
            case UITopFoodItem.Type.CUP:
                if (idx % 2 != 0)
                {
                    info.isSingleColor = false;
                    int idx_multi = ((idx - 1) / 2) % TOTAL_IMAGE_MultiColor;
                    //idx_multi = 0;
                    info.picMultiColor = IronIceCreamStepBase.GetImageOfCupMultiColor(idx_multi);

                }
                pic = IronIceCreamStepBase.GetImageOfCupFood(idx / 2);
                info.id = IronIceCreamStepBase.FOOD_ITEM_ID_liquit;
                key = UITopFoodItem.GetLockKey(info.id, idx);
                info.isLock = Common.GetBool(key, true);
                if (idx < 7)
                {
                    if (idx % 2 == 0)
                    {
                        //开放
                        info.isLock = false;
                    }
                }
                break;
            case UITopFoodItem.Type.WAN:
                info.id = IronIceCreamStepBase.FOOD_ITEM_ID_wan;
                pic = IronIceCreamStepBase.GetImageOfWan(idx);
                key = UITopFoodItem.GetLockKey(info.id, idx);
                info.isLock = Common.GetBool(key, true);
                if (idx < 7)
                {
                    if (idx % 2 == 0)
                    {
                        //开放
                        info.isLock = false;
                    }
                }
                break;
            case UITopFoodItem.Type.FOOD:
                pic = IronIceCreamStepBase.GetImageOfTopFood(idx);
                info.id = IronIceCreamStepBase.strTopFoodSort[idx];
                key = UITopFoodItem.GetLockKey(info.id, idx);
                info.isLock = Common.GetBool(key, true);
                //  info.isUnderJuan = CheckUnderJuan(info);
                if (idx < 5)
                {
                    if (idx % 2 == 0)
                    {
                        //开放
                        info.isLock = false;
                    }
                }

                break;
            case UITopFoodItem.Type.SUB_FOOD:
                //  info.id = IronIceCreamStepBase.strTopFoodSort[idx];
                pic = IronIceCreamStepBase.GetImageOfTopFoodSubFood(idx);
                break;
        }
        info.pic = pic;

        listItem.Add(info);
    }


    public void AddItem()
    {
        AddItemInfo();
        int idx = listTopItem.Count;
        UITopFoodItem item = (UITopFoodItem)GameObject.Instantiate(uiTopFoodItemPrefab);
        item.transform.SetParent(objScrollViewContent.transform);
        item.callBackDidClick = OnUITopFoodItemDidClick;
        //this.transform;
        item.transform.localScale = new Vector3(1, 1, 1);
        item.transform.localPosition = new Vector3(0, 0, 0);
        item.index = idx;
        // cmdItem.callBackTouch = OnUITouchEvent;
        item.type = type;

        //更新scrollview 内容的长度
        RectTransform rctranItem = item.GetComponent<RectTransform>();
        RectTransform rctran = objScrollViewContent.GetComponent<RectTransform>();
        RectTransform rctranScroll = objScrollView.GetComponent<RectTransform>();
        Vector2 size = rctran.sizeDelta;
        size.y = rctranScroll.rect.height;

        widthItem = size.y;
        // Debug.Log("widthItem=" + widthItem);
        size.x = widthItem * (idx + 1);

        item.width = widthItem;
        item.height = size.y;
        rctran.sizeDelta = size;
        FoodItemInfo info = listItem[idx] as FoodItemInfo;
        item.UpdateItem(info);
        listTopItem.Add(item);
    }

    public FoodItemInfo GetItem(int idx)
    {
        FoodItemInfo item = null;
        if ((listItem != null) && (listItem.Count != 0))
        {
            item = listItem[idx] as FoodItemInfo;
        }
        return item;
    }
    public void OnUITopFoodItemDidClick(UITopFoodItem item)
    {

        if (callBackDidClick != null)
        {
            Debug.Log("UITopFoodBar: OnUITopFoodItemDidClick callBackDidClick");
            imageHand.gameObject.SetActive(false);
            twHand.Pause();
            callBackDidClick(this, item);
        }
    }
}
