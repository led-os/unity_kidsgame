using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

/*  
4，吃冰淇凌
*/
public class IronIceCreamStep4 : IronIceCreamStepBase
{

    int indexStep = 0;
    int totalStep = 4;


    void Awake()
    {
        stepName = STEP_NAME_EAT;
        TextureUtil.UpdateSpriteTexture(objHand, AppRes.IMAGE_HAND);

        ResetStep();



    }
    void Start()
    {

        OnDoStep(GameIronIceCream.indexFood);


        LayOut();
    }

    public override void LayOut()
    {
        float x, y, w, h;
        float scale = 0;
        LayOutBase();

    }

    void UpdateWanItem(GameObject obj)
    {
        Texture2D tex = TextureCache.main.Load(IronIceCreamStepBase.GetWanJuanPic(GameIronIceCream.indexFood));
        TextureUtil.UpdateSpriteTexture(obj, tex);
        // BoxCollider box = obj.GetComponent<BoxCollider>();
        // box.size = new Vector3(tex.width / 100f, tex.height / 100f);
    }
    void UpdateWan(string pic)
    {
        //   objWan.SetActive(true);



    }


    void UpdateItem()
    {
    }
    public void OnDoStep(int idx)
    {

        objHand.SetActive(false);
        UpdateItem();

    }


    public override void ResetStep()
    {
        int index = 0;
        string pic = IronIceCreamStepBase.GetImageOfWan(index);
        UpdateWan(pic);

    }


    public override void UpdateFood(FoodItemInfo info)
    {
        UpdateWan(info.pic);
        LayOut();
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = this.transform.InverseTransformPoint(posworld);
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    //   uiWanIron.StartEat();
                    // poslocal.z = objErase.transform.localPosition.z;
                    // objErase.transform.localPosition = poslocal;
                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {
                    // poslocal.z = objErase.transform.localPosition.z;
                    // objErase.transform.localPosition = poslocal;
                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {

                    // poslocal.z = objErase.transform.localPosition.z;
                    // objErase.transform.localPosition = poslocal;

                }
                break;
        }
    }
}