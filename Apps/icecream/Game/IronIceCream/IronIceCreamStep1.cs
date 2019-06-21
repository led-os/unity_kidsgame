using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

/* 
1，铲冰淇淋 
*/
public class IronIceCreamStep1 : IronIceCreamStepBase
{

    public GameObject objChanzi;//铲子
    public GameObject objBlock;
    public GameObject objBlock0;
    public GameObject objBlock1;
    public GameObject objBlock2;
    public GameObject objBlock3;
    public GameObject objBlock4;
    public GameObject objBlock5;


    GameObject[] listJuan = new GameObject[6];
    public GameObject objJuan;
    public GameObject objJuan0;
    public GameObject objJuan1;
    public GameObject objJuan2;
    public GameObject objJuan3;
    public GameObject objJuan4;
    public GameObject objJuan5;
    Tween tweenMoveHand;
    Texture2D texBlock;
    int numBlock = 6;
    int indexBlock;
    bool isTouchItem;
    void Awake()
    {
        float x, y, z, w, h;
        stepName = STEP_NAME_CHAN;
        GameObject[] listJuanTmp = { objJuan0, objJuan1, objJuan2, objJuan3, objJuan4, objJuan5 };
        for (int i = 0; i < listJuanTmp.Length; i++)
        {
            listJuan[i] = listJuanTmp[i];
        }
        TextureUtil.UpdateSpriteTexture(objHand, AppRes.IMAGE_HAND);

        ResetStep();

        UITouchEventWithMove ev = objChanzi.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;
        BoxCollider box = objChanzi.AddComponent<BoxCollider>();
        box.size = objChanzi.GetComponent<SpriteRenderer>().bounds.size;
        indexBlock = numBlock - 1;


    }
    void Start()
    {
        LayOut();
    }
    public override void LayOut()
    {
        float x, y, z, w, h;
        float ratio = 0.8f;
        float scale = 0;
        RectTransform rectMainWorld = AppSceneBase.main.GetRectMainWorld();
        SpriteRenderer rdpanzi = objPanzi.GetComponent<SpriteRenderer>();
        LayOutBase();
        RectTransform rctranBlock = objBlock.GetComponent<RectTransform>();
        if (texBlock != null)
        {
            w = texBlock.width / 100f;
            h = texBlock.height / 100f;
            ratio = 0.65f;
            scale = Common.GetBestFitScale(w, h, rectMain.width, rectMain.height) * ratio;
            objBlock.transform.localScale = new Vector3(scale, scale, 1f);

            rctranBlock.sizeDelta = new Vector2(texBlock.width / 100f, texBlock.height / 100f);
        }

        {
            SpriteRenderer rd = objChanzi.GetComponent<SpriteRenderer>();
            w = rd.sprite.texture.width / 100f;
            h = rd.sprite.texture.height / 100f;



            z = objChanzi.transform.localPosition.z;
            float w_rect = (rectMainWorld.rect.width - rdpanzi.bounds.size.x) / 2;
            x = -rdpanzi.bounds.size.x / 2 - w_rect / 2;
            y = 0;
            ratio = 0.7f;
            scale = Common.GetBestFitScale(w, h, w_rect, rdpanzi.bounds.size.y) * ratio;
            objChanzi.transform.localScale = new Vector3(scale, scale, 1f);
        }


        {
            z = objHand.transform.localPosition.z;
            Vector3 pos = objChanzi.transform.localPosition;
            SpriteRenderer rd = objChanzi.GetComponent<SpriteRenderer>();
            x = pos.x;
            y = pos.y - rd.bounds.size.y / 2;
            objHand.transform.localPosition = new Vector3(x, y, z);

        }

        {
            SpriteRenderer rd_chanzi = objChanzi.GetComponent<SpriteRenderer>();
            SpriteRenderer rd = objHand.GetComponent<SpriteRenderer>();
            w = rd.sprite.texture.width / 100f;
            h = rd.sprite.texture.height / 100f;
            ratio = 0.8f;
            scale = Common.GetBestFitScale(w, h, rd_chanzi.bounds.size.x, rd_chanzi.bounds.size.y) * ratio;
            objHand.transform.localScale = new Vector3(scale, scale, 1f);
        }



        {
            RectTransform rctran = objJuan.GetComponent<RectTransform>();
            // w = rectMain.width;
            w = objBlock.transform.localScale.x * rctranBlock.rect.width;
            h = rectMain.height / 4;
            rctran.sizeDelta = new Vector2(w, h);
            x = 0;
            y = rectMain.height / 2 - h / 2;
            rctran.anchoredPosition = new Vector2(x, y);
        }


        {
            ratio = 0.9f;
            SpriteRenderer rd = objPanzi.GetComponent<SpriteRenderer>();
            w = rd.sprite.texture.width / 100f;
            h = rd.sprite.texture.height / 100f;
            scale = Common.GetBestFitScale(w, h, rectMain.width, rectMain.height) * ratio;
            objPanzi.transform.localScale = new Vector3(scale, scale, 1f);
        }

    }

    public override void ResetStep()
    {
        indexBlock = numBlock - 1;
        isTouchItem = false;
        for (int i = 0; i < listJuan.Length; i++)
        {
            GameObject obj = listJuan[i];
            obj.SetActive(false);
        }
    }

    public override void UpdateFood(FoodItemInfo info)
    {
        float x, y, z, w, h;
        string pic = "APP/UI/Game/test";
        pic = IronIceCreamStepBase.GetImageOfIcecreemPiece(info.index);
        texBlock = TextureCache.main.Load(pic);

        UpdateItem();


        LayOut();
        float scale = objBlock.transform.localScale.x;
        //初始化铲子位置
        {
            RectTransform rctran = objBlock.GetComponent<RectTransform>();
            int idx = 0;
            GameObject objItem = GetBlock(idx);
            float w_block = rctran.rect.width * scale;
            float h_block = rctran.rect.height * scale;
            z = objChanzi.transform.localPosition.z;
            w = w_block / numBlock;
            SpriteRenderer rd = objChanzi.GetComponent<SpriteRenderer>();
            x = objBlock.transform.localPosition.x + (w_block / 2 - (w / 2) * (idx + 1));
            y = objBlock.transform.localPosition.y - h_block / 2 - rd.bounds.size.y / 2;

            objChanzi.transform.localPosition = new Vector3(x, y, z);
        }
        //重新更新hand位置
        LayOut();
        ShowHand(true, true);
    }

    public void ShowHand(bool isShow, bool isAnimation)
    {
        float x, y, z, w, h;
        //Debug.Log("showhand isShow=" + isShow + " isAnimation=" + isAnimation);
        objHand.SetActive(isShow);
        if (isAnimation)
        {
            if (tweenMoveHand == null)
            {
                //  SpriteRenderer rd = objHand.GetComponent<SpriteRenderer>(); 
                z = objHand.transform.localPosition.z;
                Vector3 pos = objChanzi.transform.localPosition;
                SpriteRenderer rd = objChanzi.GetComponent<SpriteRenderer>();
                x = pos.x;
                y = pos.y + rd.bounds.size.y / 2;
                Vector3 posEnd = new Vector3(x, y, z);
                objHand.transform.DOLocalMove(posEnd, 2f).SetLoops(-1, LoopType.Yoyo);

            }
            tweenMoveHand.Play();

        }
        else
        {

            if (tweenMoveHand != null)
            {
                tweenMoveHand.Pause();
            }

        }
    }

    void UpdateJuanItem(GameObject obj)
    {
        Texture2D tex = TextureCache.main.Load(IronIceCreamStepBase.GetBlockItemPic(GameIronIceCream.indexFood));
        TextureUtil.UpdateSpriteTexture(obj, tex);
    }
    void UpdateBlockItem(GameObject obj, int idx)
    {
        BlockItemChan it = obj.GetComponent<BlockItemChan>();
        if (it != null)
        {
            it.UpdateSize((texBlock.width / 100f) / numBlock, (texBlock.height / 100f));
            it.indexCol = idx;
            it.row = 1;
            it.col = numBlock;
            it.UpdateTexture(texBlock);
            //it.UpdatePercent(50);
        }
    }
    void UpdateItem()
    {
        UpdateJuanItem(objJuan0);
        UpdateJuanItem(objJuan1);
        UpdateJuanItem(objJuan2);
        UpdateJuanItem(objJuan3);
        UpdateJuanItem(objJuan4);
        UpdateJuanItem(objJuan5);

        UpdateBlockItem(objBlock0, 0);
        UpdateBlockItem(objBlock1, 1);
        UpdateBlockItem(objBlock2, 2);
        UpdateBlockItem(objBlock3, 3);
        UpdateBlockItem(objBlock4, 4);
        UpdateBlockItem(objBlock5, 5);

    }

    int GetPercent(Vector3 pos)
    {
        int percent = 0;

        //判断铲子顶部接触块
        Renderer rdChanzi = objChanzi.GetComponent<Renderer>();
        float h = rdChanzi.bounds.size.y;
        Vector3 posnew = pos;
        posnew.y = pos.y + h / 2;
        
        Vector3 poslocal = objBlock.transform.InverseTransformPoint(posnew);
        RectTransform rctran = objBlock.GetComponent<RectTransform>();
        float h_block = rctran.rect.height * objBlock.transform.localScale.y;
        percent = (int)(100 * (h_block / 2 - poslocal.y) / h_block);
        if (percent < 0)
        {
            percent = 0;
        }
        if (percent > 100)
        {
            percent = 100;
        }

        return percent;
    }
    GameObject GetBlock(int idx)
    {
        GameObject obj = objBlock0;
        if (idx == 0)
        {
            obj = objBlock0;
        }
        if (idx == 1)
        {
            obj = objBlock1;
        }
        if (idx == 2)
        {
            obj = objBlock2;
        }
        if (idx == 3)
        {
            obj = objBlock3;
        }
        if (idx == 4)
        {
            obj = objBlock4;
        }
        if (idx == 5)
        {
            obj = objBlock5;
        }
        return obj;
    }

    //判断铲子是否碰到块
    void ChcekIsTouchItem()
    {
        float x, y, w, h;

        Vector3 pos = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = this.transform.InverseTransformPoint(pos);
        //判断铲子顶部接触块
        Renderer rdChanzi = objChanzi.GetComponent<Renderer>();
        h = rdChanzi.bounds.size.y;

        poslocal.y += h / 2;

        GameObject objItem = GetBlock(indexBlock);
        Renderer rd = objItem.GetComponent<Renderer>();
        w = rd.bounds.size.x;
        h = rd.bounds.size.y;
        x = rd.bounds.center.x - w / 2;
        y = rd.bounds.center.y - h / 2;
        Rect rc = new Rect(x, y, w, h);
        //Debug.Log("rc=" + rc + " poslocal=" + poslocal);
        ShowHand(false, false);

        Debug.Log("ChcekIsTouchItem:poslocal=" + poslocal + " rc" + rc + " sz=" + rdChanzi.bounds.size);
        if (rc.Contains(poslocal))
        {
            isTouchItem = true;

        }
    }

    void OnBlockItemFinish(GameObject objItem, bool isNext)
    {
        BlockItemChan it = objItem.GetComponent<BlockItemChan>();
        if (it.percent <= 0)
        {
            GameObject obj = listJuan[indexBlock];
            if (obj.activeSelf)
            {
                // return;
            }
            obj.SetActive(true);
            if (isNext)
            {
                indexBlock--;
                if (indexBlock < 0)
                {
                    indexBlock = 0;
                    objChanzi.SetActive(false);
                    if (callBackDidUpdateStatus != null)
                    {
                        callBackDidUpdateStatus(this, STATUS_STEP_END);
                    }
                }
            }

        }
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        Vector3 pos = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = this.transform.InverseTransformPoint(pos);
        GameObject objItem = GetBlock(indexBlock);
        float x, y, w, h;
        //  Debug.Log("OnUITouchEvent status=" + status);
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    isTouchItem = false;
                    ChcekIsTouchItem();
                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {
                    poslocal.z = objChanzi.transform.localPosition.z;
                    objChanzi.transform.localPosition = poslocal;
                    if (isTouchItem == false)
                    {
                        ChcekIsTouchItem();
                    }

                    //Debug.Log("isTouchItem=" + isTouchItem);
                    if (isTouchItem)
                    {
                        BlockItemChan it = objItem.GetComponent<BlockItemChan>();
                        it.UpdatePercent(GetPercent(pos));
                        OnBlockItemFinish(objItem, false);
                    }

                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                if (isTouchItem)
                {
                    OnBlockItemFinish(objItem, true);
                }
                break;

        }
    }
}
