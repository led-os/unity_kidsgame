using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/*铁板冰淇淋步骤：
0，炒冰淇淋
1，铲冰淇淋
2，装淇淋卷到碗里
3，装顶料
4，吃冰淇凌
*/

public class GameIronIceCream : GameIceCream
{
    public const string Prefab_STEP_PREFIX = "AppCommon/Prefab/Game/IronIceCream/IronIceCreamStep";

    public const string IMAGE_DIR_ROOT_SingleColor = Common.GAME_RES_DIR + "/image/IronIceCream/SingleColor";//0  0-1 0-2 0-3 
    public const string IMAGE_DIR_ROOT_MultiColor = Common.GAME_RES_DIR + "/image/IronIceCream/MultiColor";//  0-1 0-4 0-5

    public const string IMAGE_DIR_ROOT_CupLiquid = Common.GAME_RES_DIR + "/image/IronIceCream/CupLiquid";//  1 2 3 4 5

    public const string IMAGE_LOCK = Common.GAME_RES_DIR + "/image/TopFoodBar/Lock.png";

    public const int INDEX_STEP_CHAO = 0;//炒
    public const int INDEX_STEP_CHAN = 1;//铲
    public const int INDEX_STEP_WAN = 2;//碗
    public const int INDEX_STEP_ZHUANG = 3;//装

    public const int INDEX_STEP_CHI = 4;//吃

    IronIceCreamStepBase uiStepPrefab;
    public IronIceCreamStepBase uiStep;

    public int indexStep = 0;
    public int totalStep = 5;
    public static int indexFood = 0;
    static public int status;
    public OnGameIronIceCreamDidUpdateStatusDelegate callBackDidUpdateStatus { get; set; }

    void Awake()
    {


    }
    void Start()
    {
        LayOut();
    }

    public override void LayOut()
    {
        float x, y, w, h;
        float scale = 0;

    }
    //下一个步骤
    public void OnNextStep()
    {
        indexStep++;
        if (indexStep >= totalStep)
        {
            indexStep = 0;
            //return;
        }
        RunSetp(indexStep);
    }

    void DeleteStep()
    {
        if (uiStep != null)
        {
            DestroyImmediate(uiStep.gameObject);
        }

    }

    // 炒冰淇淋
    public void RunSetp(int idx)
    {
        DeleteStep();
        {
            GameObject obj = PrefabCache.main.Load(Prefab_STEP_PREFIX + idx.ToString());
            if (obj != null)
            {
                uiStepPrefab = obj.GetComponent<IronIceCreamStepBase>();
                uiStep = (IronIceCreamStepBase)GameObject.Instantiate(uiStepPrefab);
                uiStep.callBackDidUpdateStatus = OnGameIronIceCreamDidUpdateStatus;
                RectTransform rctranPrefab = uiStepPrefab.transform as RectTransform;
                uiStep.transform.SetParent(this.transform);
                RectTransform rctran = uiStep.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;
                uiStep.transform.localPosition = Vector3.zero;
            }

        }
    }


    public void ResetStep()
    {
        if (uiStep != null)
        {
            uiStep.ResetStep();
        }
    }
    public void UpdateFood(FoodItemInfo info)
    {
        if (uiStep != null)
        {
            uiStep.UpdateFood(info);
        }
    }

    public void OnGameIronIceCreamDidUpdateStatus(UIView ui, int status)
    {
        if (callBackDidUpdateStatus != null)
        {
            callBackDidUpdateStatus(ui, status);
        }
    }

}
