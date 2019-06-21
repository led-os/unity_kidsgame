using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
/*铁板冰淇淋步骤：
0，炒冰淇淋
*/
public class IronIceCreamStep0 : IronIceCreamStepBase
{
    public const int CHANZI_STATUS_NONE = 0;
    public const int CHANZI_STATUS_START = 1;
    public const int CHANZI_STATUS_MOVE = 2;
    public const int CHANZI_STATUS_END = 3;
    public GameObject objChanzi;//铲子
    public GameObject objIcecreemBlock;//冰淇凌块
    public GameObject objMultiColor;//

    public GameObject objIcecreemPiece;//冰淇凌片
    public GameObject objIcecreemLiquid;//冰淇凌液体倾倒动画

    int indexFood = 0;
    int indexStep = 0;
    int totalStep = 4;

    Tween tweenAlpha;
    int chanziStatus;
    float scaleBlockNormal;
    float scaleMultiNormal;
    Tweener twHandMove;
    FoodItemInfo infoItem;

    UIDragEventChanzi dragEvChanzi;

    void Awake()
    {
        stepName = STEP_NAME_CHAO;
        TextureUtil.UpdateSpriteTexture(objHand, AppRes.IMAGE_HAND);
        ResetStep();
        objHand.SetActive(false);
        objChanzi.SetActive(true);
        objIcecreemPiece.SetActive(false);

        UITouchEventWithMove ev = objChanzi.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;
        BoxCollider box = objChanzi.AddComponent<BoxCollider>();
        box.size = objChanzi.GetComponent<SpriteRenderer>().bounds.size;

        dragEvChanzi = objChanzi.GetComponent<UIDragEventChanzi>();
        dragEvChanzi.enableDrag = false;
        dragEvChanzi.callBackALpha = OnAlphaChange;
        // LayOut(); 

    }
    void Start()
    {
        LayOut();
    }

    public override void LayOut()
    {
        float x, y, z, w, h;
        float scale = 0;
        LayOutBase();
        RectTransform rectMainWorld = AppSceneBase.main.GetRectMainWorld();
        float ratio = 0.8f;
        SpriteRenderer rdpanzi = objPanzi.GetComponent<SpriteRenderer>();
        {
            SpriteRenderer rd = objChanzi.GetComponent<SpriteRenderer>();
            w = rd.sprite.texture.width / 100f;
            h = rd.sprite.texture.height / 100f;



            z = objChanzi.transform.localPosition.z;
            float w_rect = (rectMainWorld.rect.width - rdpanzi.bounds.size.x) / 2;
            x = -rdpanzi.bounds.size.x / 2 - w_rect / 2;
            y = 0;
            objChanzi.transform.localPosition = new Vector3(x, y, z);
            ratio = 0.8f;
            scale = Common.GetBestFitScale(w, h, w_rect, rdpanzi.bounds.size.y) * ratio;
            objChanzi.transform.localScale = new Vector3(scale, scale, 1f);
        }

        {
            z = objHand.transform.localPosition.z;
            Vector3 pos = objChanzi.transform.localPosition;
            SpriteRenderer rd = objChanzi.GetComponent<SpriteRenderer>();
            x = pos.x + rd.bounds.size.x / 2;
            y = pos.y + rd.bounds.size.y / 2;
            objHand.transform.localPosition = new Vector3(x, y, z);

        }

        {
            SpriteRenderer rd_chanzi = objChanzi.GetComponent<SpriteRenderer>();
            SpriteRenderer rd = objHand.GetComponent<SpriteRenderer>();
            w = rd.sprite.texture.width / 100f;
            h = rd.sprite.texture.height / 100f;
            ratio = 0.6f;
            scale = Common.GetBestFitScale(w, h, rd_chanzi.bounds.size.x, rd_chanzi.bounds.size.y) * ratio;
            objHand.transform.localScale = new Vector3(scale, scale, 1f);
        }

        float ratio_block = 0.7f;
        {
            SpriteRenderer rd = objIcecreemBlock.GetComponent<SpriteRenderer>();
            if (rd.sprite != null && rd.sprite.texture != null)
            {
                w = rd.sprite.texture.width / 100f;
                h = rd.sprite.texture.height / 100f;
                scale = Common.GetBestFitScale(w, h, rdpanzi.bounds.size.x, rdpanzi.bounds.size.y) * ratio_block;
                scaleBlockNormal = scale;
                objIcecreemBlock.transform.localScale = new Vector3(scale, scale, 1f);
            }

        }

        {
            SpriteRenderer rd = objMultiColor.GetComponent<SpriteRenderer>();
            if (rd.sprite != null && rd.sprite.texture != null)
            {
                w = rd.sprite.texture.width / 100f;
                h = rd.sprite.texture.height / 100f;
                scale = Common.GetBestFitScale(w, h, rdpanzi.bounds.size.x, rdpanzi.bounds.size.y) * ratio_block;
                scaleMultiNormal = scale;
                objMultiColor.transform.localScale = new Vector3(scale, scale, 1f);
            }

        }

        {
            SpriteRenderer rd = objIcecreemPiece.GetComponent<SpriteRenderer>();
            if (rd.sprite != null && rd.sprite.texture != null)
            {
                w = rd.sprite.texture.width / 100f;
                h = rd.sprite.texture.height / 100f;
                scale = Common.GetBestFitScale(w, h, rdpanzi.bounds.size.x, rdpanzi.bounds.size.y) * ratio_block;
                objIcecreemPiece.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }


        {
            SpriteRenderer rd = objIcecreemLiquid.GetComponent<SpriteRenderer>();
            if (rd.sprite != null && rd.sprite.texture != null)
            {
                w = rd.sprite.texture.width / 100f;
                h = rd.sprite.texture.height / 100f;
                ratio = 0.8f;
                scale = Common.GetBestFitScale(w, h, rdpanzi.bounds.size.x, rdpanzi.bounds.size.y) * ratio;
                objIcecreemLiquid.transform.localScale = new Vector3(scale, scale, 1f);
                z = objIcecreemLiquid.transform.localPosition.z;
                y = h * scale / 2;
                objIcecreemLiquid.transform.localPosition = new Vector3(0, y, z);
            }
        }
    }



    // 炒冰淇淋
    public void OnDoStep(int idx)
    {
        indexFood = idx;
        GameIronIceCream.status = STATUS_STEP_START;
        GameIronIceCream.indexFood = idx;
        objHand.SetActive(false);
        objMultiColor.SetActive(false);
        //objChanzi.SetActive(false);
        TextureUtil.UpdateSpriteTexture(objIcecreemBlock, IronIceCreamStepBase.GetImageOfIcecreemLiquid(indexFood));

        int idx_multi = ((indexFood - 1) / 2) % UITopFoodBar.TOTAL_IMAGE_MultiColor;
        TextureUtil.UpdateSpriteTexture(objMultiColor, IronIceCreamStepBase.GetImageOfIceCreamBlockMultiColor(idx_multi));

        TextureUtil.UpdateSpriteTexture(objIcecreemPiece, IronIceCreamStepBase.GetImageOfIcecreemPiece(indexFood));

        string pic = GetLiquidImage(0);
        //LayOut 前先初始化
        TextureUtil.UpdateSpriteTexture(objIcecreemLiquid, pic);

        LayOut();

        Invoke("RunActionLiquid", 1f);
    }

    string GetLiquidImage(int idx)
    {
        return GameIronIceCream.IMAGE_DIR_ROOT_CupLiquid + "/" + (indexFood / 2).ToString() + "/" + (idx + 1).ToString() + ".png";

    }

    //倾倒冰淇淋液体动画
    void RunActionLiquid()
    {
        objIcecreemBlock.SetActive(true);
        objMultiColor.SetActive(!infoItem.isSingleColor);

        objIcecreemLiquid.SetActive(true);
        ActionImage acImage = objIcecreemLiquid.AddComponent<ActionImage>();
        acImage.duration = 3f;
        acImage.isLoop = false;
        for (int i = 0; i < 5; i++)
        {
            string pic = GetLiquidImage(i);
            acImage.AddPic(pic);
        }
        acImage.Run();
        acImage.callbackComplete = OnIcecreemLiquidActionFinish;
        AudioPlay.main.PlayFile(AppRes.AUDIO_GAME_liquid);
        LayOut();

        float scale0 = 0.1f;
        float scale1 = scaleBlockNormal;
        objIcecreemBlock.transform.localScale = new Vector3(scale0, scale0, 1f);
        objIcecreemBlock.transform.DOScale(new Vector3(scale1, scale1, 1f), acImage.duration).OnComplete(() =>
        {
            objChanzi.SetActive(true);
            ShowHand(true, true);
            if (callBackDidUpdateStatus != null)
            {
                callBackDidUpdateStatus(this, STATUS_Liquid_Finish);
            }
            chanziStatus = CHANZI_STATUS_START;
        });

        scale0 = 0.1f;
        scale1 = scaleMultiNormal;
        objMultiColor.transform.localScale = new Vector3(scale0, scale0, 1f);
        objMultiColor.transform.DOScale(new Vector3(scale1, scale1, 1f), acImage.duration).OnComplete(() =>
        {
        });

    }
    public void OnIcecreemLiquidActionFinish(GameObject obj)
    {
        //倒冰淇凌夜结束
        objIcecreemLiquid.SetActive(false);
    }
    public override void ResetStep()
    {
        GameIronIceCream.status = STATUS_STEP_NONE;
        objIcecreemBlock.SetActive(false);
        objMultiColor.SetActive(false);
        objIcecreemPiece.SetActive(false);
        chanziStatus = CHANZI_STATUS_NONE;

        //恢复透明度为100%
        SpriteRenderer rd = objIcecreemBlock.GetComponent<SpriteRenderer>();
        rd.color = Color.white;

        rd = objMultiColor.GetComponent<SpriteRenderer>();
        rd.color = Color.white;

        if (dragEvChanzi != null)
        {
            dragEvChanzi.enableDrag = false;
            dragEvChanzi.OnReset();
        }

    }

    public override void UpdateFood(FoodItemInfo info)
    {
        infoItem = info;
        if (twHandMove != null)
        {
            twHandMove.Pause();
        }
        OnDoStep(info.index);
    }


    void OnAlphaChange(float alpha)
    {
        objIcecreemPiece.SetActive(true);
        {
            SpriteRenderer rd = objIcecreemBlock.GetComponent<SpriteRenderer>();
            Color cr = rd.color;
            cr.a = 1 - alpha;
            rd.color = cr;
            //  DOTween.ToAlpha(() => rd.color, x => rd.color = x, 0f, duration);
        }
        {
            SpriteRenderer rd = objIcecreemPiece.GetComponent<SpriteRenderer>();
            Color cr = rd.color;
            cr.a = alpha;
            rd.color = cr;
        }
        // {
        //     SpriteRenderer rd = objIcecreemPiece.GetComponent<SpriteRenderer>();
        //     Color cr = rd.color;
        //     cr.a = 0f;
        //     rd.color = cr;
        //     objIcecreemPiece.SetActive(true);
        //     //objIcecreemBlock.SetActive(false);
        //     DOTween.ToAlpha(() => rd.color, x => rd.color = x, 1f, duration).OnComplete(() =>
        // {
        //     GameIronIceCream.status = STATUS_STEP_END;
        //     //制作淇淋片结束
        //     if (callBackDidUpdateStatus != null)
        //     {
        //         callBackDidUpdateStatus(this, STATUS_STEP_END);
        //     }

        // });

        // }
        if (alpha >= 1f)
        {
            GameIronIceCream.status = STATUS_STEP_END;
            //制作淇淋片结束
            if (callBackDidUpdateStatus != null)
            {
                callBackDidUpdateStatus(this, STATUS_STEP_END);
            }
            chanziStatus = CHANZI_STATUS_NONE;
            objHand.SetActive(false);
            objChanzi.SetActive(false);
        }




    }
    //淇淋液变淇淋片
    void MakeIceCreamBlock()
    {
        float duration = 8f;
        {
            SpriteRenderer rd = objIcecreemBlock.GetComponent<SpriteRenderer>();
            Color cr = rd.color;
            cr.a = 1f;
            rd.color = cr;
            DOTween.ToAlpha(() => rd.color, x => rd.color = x, 0f, duration);
        }
        {
            SpriteRenderer rd = objIcecreemPiece.GetComponent<SpriteRenderer>();
            Color cr = rd.color;
            cr.a = 0f;
            rd.color = cr;
            objIcecreemPiece.SetActive(true);
            //objIcecreemBlock.SetActive(false);
            DOTween.ToAlpha(() => rd.color, x => rd.color = x, 1f, duration).OnComplete(() =>
        {
            GameIronIceCream.status = STATUS_STEP_END;
            //制作淇淋片结束
            if (callBackDidUpdateStatus != null)
            {
                callBackDidUpdateStatus(this, STATUS_STEP_END);
            }

        });

        }

        chanziStatus = CHANZI_STATUS_NONE;
        objHand.SetActive(false);
        objChanzi.SetActive(false);


    }
    public void ShowHand(bool isShow, bool isAnimation)
    {
        objHand.SetActive(isShow);
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

            //闪烁动画
            if (tweenAlpha == null)
            {
                SpriteRenderer rd = objHand.GetComponent<SpriteRenderer>();
                tweenAlpha = DOTween.ToAlpha(() => rd.color, x => rd.color = x, 0f, duration).SetLoops(-1, LoopType.Yoyo);
            }
            tweenAlpha.Play();

        }
        else
        {

            if (tweenAlpha != null)
            {
                tweenAlpha.Pause();
            }

        }
    }

    //进入上下滑动炒冰淇凌操作
    public void GotoStatusMove()
    {
        float x, y, z, w, h;
        z = objChanzi.transform.localPosition.z;
        x = 0;
        y = 0;
        objChanzi.transform.localPosition = new Vector3(x, y, z);

        z = objHand.transform.localPosition.z;
        Vector3 pos = objChanzi.transform.localPosition;
        SpriteRenderer rd = objChanzi.GetComponent<SpriteRenderer>();
        x = pos.x;
        y = pos.y + rd.bounds.size.y / 2;
        objHand.transform.localPosition = new Vector3(x, y, z);
        y = pos.y - rd.bounds.size.y / 2;
        Vector3 posEnd = new Vector3(x, y, z);
        twHandMove = objHand.transform.DOLocalMove(posEnd, 2f).SetLoops(-1, LoopType.Yoyo);
        tweenAlpha.Pause();

        rd = objHand.GetComponent<SpriteRenderer>();
        rd.color = Color.white;
        chanziStatus = CHANZI_STATUS_MOVE;

        if (dragEvChanzi != null)
        {
            dragEvChanzi.enableDrag = true;
        }

    }

    //上下滑动炒冰淇凌操作
    public void OnUITouchEventStatusMove(UITouchEvent ev, PointerEventData eventData, int status)
    {
        // Debug.Log("OnUITouchEventStatusMove");
        if (status == UITouchEvent.STATUS_TOUCH_DOWN)
        {
            objHand.SetActive(false);
            twHandMove.Pause();
        }

        if (dragEvChanzi != null)
        {
            dragEvChanzi.OnUIDragEvent(eventData, status);
        }
    }
    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {

        if (chanziStatus == CHANZI_STATUS_MOVE)
        {
            OnUITouchEventStatusMove(ev, eventData, status);
            return;
        }

        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    //铲子移动到盘子上 竖向滑动指导动作
                    if (chanziStatus == CHANZI_STATUS_START)
                    {
                        GotoStatusMove();
                    }
                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {
                    Vector3 pos = Common.GetInputPositionWorld(mainCam);
                    // Vector3 poslocal = this.transform.InverseTransformPoint(pos);
                    // poslocal.z = objChanzi.transform.localPosition.z;
                    // objChanzi.transform.localPosition = poslocal;
                    if (chanziStatus == CHANZI_STATUS_MOVE)
                    {
                        // chanziStatus = CHANZI_STATUS_END;
                    }
                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                if (chanziStatus == CHANZI_STATUS_END)
                {
                    // MakeIceCreamBlock();

                }
                break;

        }
    }
}
