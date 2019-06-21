using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
/* 
3，装顶料 
*/
public class IronIceCreamStep3 : IronIceCreamStepBase
{

    int indexStep = 0;
    int totalStep = 4;

    void Awake()
    {
        stepName = STEP_NAME_TOPFOOD;
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
        LayOutBase();


    }

    void UpdateWanItem(GameObject obj)
    {
        Texture2D tex = TextureCache.main.Load(IronIceCreamStepBase.GetWanJuanPic(GameIronIceCream.indexFood));
        TextureUtil.UpdateSpriteTexture(obj, tex);
        // BoxCollider box = obj.GetComponent<BoxCollider>();
        // box.size = new Vector3(tex.width / 100f, tex.height / 100f);
    }
    void UpdateWan()
    {
        //  objWan.SetActive(true);
        //  Debug.Log("UpdateWan:pic="+pic);
        // string pic = IronIceCreamStep2.strImageWan;
        // if (pic != null)
        // {
        //     TextureUtil.UpdateSpriteTexture(objWanFt, pic);
        // }


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
        objPanzi.SetActive(false);
        //  objWan.SetActive(false);

        uiWanIron.ShowJuan(false);
    }


    public override void UpdateFood(FoodItemInfo info)
    {
        UpdateWan();
        uiWanIron.OnAddTopFood(info);
        LayOut();
    }
}
