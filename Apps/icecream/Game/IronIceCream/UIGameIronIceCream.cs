using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DG.Tweening;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameIronIceCream : UIGameIceCream
{
    public static int indexStepScreenShot;
    public UITopFoodToolBar uiTopFoodToolBar;//
    UIPopSelectBar uiPopSelectBarPrefab;
    UIPopSelectBar uiPopSelectBar;
    UITopFoodBar uiTopFoodBarPrefab;
    UITopFoodBar uiTopFoodBar;
    UITopFoodItem uiTopFoodItemPrefab;
    GameIronIceCream gameIronIceCreamPrefab;
    GameIronIceCream gameIronIceCream;

    UITopFoodItem uiCup;//倒液体的杯子
    public Button btnNext;

    public Image imageTrophy;//获得奖励星动画
    void Awake()
    {
        LoadPrefab();

        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);
        uiPopSelectBar.gameObject.SetActive(false);
        uiTopFoodToolBar.gameObject.SetActive(false);
        //  btnNext.gameObject.SetActive(false);

        if (uiCup == null)
        {

            Texture2D tex = TextureCache.main.Load(AppRes.IMAGE_CUP);

            uiCup = (UITopFoodItem)GameObject.Instantiate(uiTopFoodItemPrefab);
            uiCup.transform.parent = this.transform;
            uiCup.transform.localScale = new Vector3(1, 1, 1);
            uiCup.transform.localPosition = new Vector3(0, 0, 0);
            //uiCup.callBackDidClick = OnUITopFoodItemDidClick;
            uiCup.gameObject.SetActive(false);
            uiCup.width = tex.width;
            uiCup.height = tex.height;
            uiCup.enableLock = false;
            RectTransform rctran = uiCup.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(uiCup.width, uiCup.height);
        }

        UpdateCup(0);
        //ShowFPS();
        imageTrophy.gameObject.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);

        if (!GameManager.main.isLoadGameScreenShot)
        {
            // Invoke("ShowTrophy", 2.0f);
        }


    }
    // Update is called once per frame
    void Update()
    {


    }
    void LoadPrefab()
    {
        base.LoadPrefabBase();
        {
            GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/IronIceCream/WanIron");
            if (obj != null)
            {
                IronIceCreamStepBase.uiWanIronPrefab = obj.GetComponent<UIWanIron>();
                IronIceCreamStepBase.uiWanIron = (UIWanIron)GameObject.Instantiate(IronIceCreamStepBase.uiWanIronPrefab);
                RectTransform rctranPrefab = IronIceCreamStepBase.uiWanIronPrefab.transform as RectTransform;
                AppSceneBase.main.AddObjToMainWorld(IronIceCreamStepBase.uiWanIron.gameObject);
                RectTransform rctran = IronIceCreamStepBase.uiWanIron.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;
                IronIceCreamStepBase.uiWanIron.gameObject.SetActive(false);
                IronIceCreamStepBase.uiWanIron.transform.localPosition = new Vector3(0, 0, -10f);

            }

        }


        {
            GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/UITopFoodBar");
            if (obj != null)
            {
                uiTopFoodBarPrefab = obj.GetComponent<UITopFoodBar>();
                uiTopFoodBar = (UITopFoodBar)GameObject.Instantiate(uiTopFoodBarPrefab);
                uiTopFoodBar.callBackDidClick = OnUITopFoodBarDidClick;
                RectTransform rctranPrefab = uiTopFoodBarPrefab.transform as RectTransform;
                //  AppSceneBase.main.AddObjToMainCanvas(uiGameTopBar.gameObject);
                uiTopFoodBar.transform.parent = this.transform;
                RectTransform rctran = uiTopFoodBar.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;

            }

        }


        {
            GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/UIPopSelectBar/UIPopSelectBar");
            if (obj != null)
            {
                uiPopSelectBarPrefab = obj.GetComponent<UIPopSelectBar>();
                uiPopSelectBar = (UIPopSelectBar)GameObject.Instantiate(uiPopSelectBarPrefab);
                uiPopSelectBar.callBackDidClick = OnUIPopSelectBarDidClick;
                RectTransform rctranPrefab = uiPopSelectBarPrefab.transform as RectTransform;
                //  AppSceneBase.main.AddObjToMainCanvas(uiGameTopBar.gameObject);
                uiPopSelectBar.transform.parent = this.transform;
                RectTransform rctran = uiPopSelectBar.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;

            }

        }

        {
            GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/IronIceCream/GameIronIceCream");
            if (obj != null)
            {
                gameIronIceCreamPrefab = obj.GetComponent<GameIronIceCream>();
                gameIronIceCream = (GameIronIceCream)GameObject.Instantiate(gameIronIceCreamPrefab);
                gameIronIceCream.callBackDidUpdateStatus = OnGameIronIceCreamDidUpdateStatus;
                RectTransform rctranPrefab = gameIronIceCreamPrefab.transform as RectTransform;
                gameIronIceCream.transform.parent = GameViewController.main.objController.transform;
                AppSceneBase.main.AddObjToMainWorld(gameIronIceCream.gameObject);
                gameIronIceCream.transform.localPosition = new Vector3(0f, 0f, -1f);
                RectTransform rctran = gameIronIceCream.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;

            }

        }
        //cup
        {

            GameObject obj = PrefabCache.main.Load(UITopFoodItem.PREFAB_TopFoodItem);
            uiTopFoodItemPrefab = obj.GetComponent<UITopFoodItem>();
        }
    }

    public override void UpdateGuankaLevel(int level)
    {
        InitUI();
    }
    public void ShowTrophy()
    {
        TrophyViewController.main.Show(null, null);
    }

    //每解锁一个道具，会从这个道具飞出一个彩色奖励星到奖杯榜按钮处
    void RunActionMoveTrophy(UITopFoodItem item)
    {
        float x, y;
        string pic = TrophyRes.GetImageOfIcon(TrophyRes.TYPE_Star, 1);
        TextureUtil.UpdateImageTexture(imageTrophy, pic, true);
        imageTrophy.gameObject.SetActive(true);
        Vector2 posNormal = imageTrophy.transform.position;
        UIGameIceCream game = GameViewController.main.gameBase as UIGameIceCream;
        //置顶
        imageTrophy.transform.SetAsLastSibling();
        RectTransform rctran = imageTrophy.GetComponent<RectTransform>();
        Vector2 posEnd = game.uiGameTopBar.GetPosOfBtnTrophy();
        rctran.DOMove(posEnd, 1f).OnComplete(() =>
          {
              imageTrophy.transform.position = posNormal;
              imageTrophy.gameObject.SetActive(false);
              DoClickFoodItem(item);
          });

        AudioPlay.main.PlayFile(TrophyRes.AUDIO_TROPHY_GET_STAR);
    }
    void UpdateCup(int idx)
    {
        uiCup.index = idx;
        FoodItemInfo info = new FoodItemInfo();
        info.type = uiCup.type;
        info.pic = IronIceCreamStepBase.GetImageOfCupFood(idx / 2);
        info.isSingleColor = true;
        if (idx % 2 != 0)
        {
            info.isSingleColor = false;
            int idx_multi = ((idx - 1) / 2) % UITopFoodBar.TOTAL_IMAGE_MultiColor;
            info.picMultiColor = IronIceCreamStepBase.GetImageOfCupMultiColor(idx_multi);

        }
        uiCup.UpdateItem(info);
        uiCup.ShowHand(true, true);
        //直接倾倒液体
        OnUITopFoodItemDidClick(uiCup);
        LayOut();

    }
    void InitUI()
    {
        if (GameManager.main.isLoadGameScreenShot)
        {
            // for (int i = 0; i < indexStepScreenShot; i++)
            {
                Invoke("RunSetpScreenShot", 1f);
            }

        }
        else
        {
            RunSetpInternal(0);
        }
    }

    public void RunSetpScreenShot()
    {
        RunSetp(indexStepScreenShot);
    }
    public void RunSetp(int idx)
    {
        RunSetpInternal(idx);

        uiTopFoodBar.type = UITopFoodItem.Type.CUP;

        uiPopSelectBar.gameObject.SetActive(false);
        uiTopFoodToolBar.gameObject.SetActive(false);
        uiTopFoodBar.gameObject.SetActive(true);
        if (IronIceCreamStepBase.uiWanIron != null)
        {
            IronIceCreamStepBase.uiWanIron.gameObject.SetActive(false);
        }
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_CHAN)
        {
            uiTopFoodBar.type = UITopFoodItem.Type.CUP;
            FoodItemInfo info = uiTopFoodBar.GetItem(GameIronIceCream.indexFood);
            gameIronIceCream.UpdateFood(info);
        }
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_WAN)
        {
            if (IronIceCreamStepBase.uiWanIron != null)
            {
                IronIceCreamStepBase.uiWanIron.gameObject.SetActive(true);
            }
            uiTopFoodBar.type = UITopFoodItem.Type.WAN;
            FoodItemInfo info = uiTopFoodBar.GetItem(GameIronIceCream.indexFood);
            gameIronIceCream.UpdateFood(info);
        }
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_ZHUANG)
        {
            if (IronIceCreamStepBase.uiWanIron != null)
            {
                IronIceCreamStepBase.uiWanIron.gameObject.SetActive(true);
            }
            uiTopFoodBar.type = UITopFoodItem.Type.FOOD;
            uiPopSelectBar.gameObject.SetActive(true);
            uiTopFoodToolBar.gameObject.SetActive(true);
            UIPopSelectBar.indexFoodSort = 0;
            UIPopSelectBar.countFoodSort = IronIceCreamStepBase.countTopFoodSort[0];
            uiPopSelectBar.UpdateItem(uiTopFoodBar.GetItem(0));
            // FoodItemInfo info = uiPopSelectBar.GetItem(0);
            // if (info != null)
            // { 
            //     OnUIPopSelectBarDidClick(uiPopSelectBar, info);
            // }

        }

        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_CHI)
        {

            if (IronIceCreamStepBase.uiWanIron != null)
            {
                IronIceCreamStepBase.uiWanIron.gameObject.SetActive(true);
            }
            uiTopFoodBar.gameObject.SetActive(false);
        }

        uiTopFoodBar.UpdateType(uiTopFoodBar.type);
        if (IronIceCreamStepBase.uiWanIron != null)
        {
            IronIceCreamStepBase.uiWanIron.UpdateStep(gameIronIceCream.indexStep);
        }


        OnUIDidFinish();
        //  Invoke("OnUIDidFinish", 1.0f);
    }
    public void RunSetpInternal(int idx)
    {
        if (gameIronIceCream != null)
        {
            gameIronIceCream.RunSetp(idx);
        }
        LayOut();
    }


    public override void LayOut()
    {
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        float scalex = 0, scaley = 0, scale = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        //
        // Debug.Log("LayOut 1");
        if ((gameIronIceCream != null) && (gameIronIceCream.uiStep != null))
        {
            //  Debug.Log("LayOut 2");
            IronIceCreamStep0 ui = gameIronIceCream.uiStep as IronIceCreamStep0;
            if (ui != null)
            {
                // Debug.Log("LayOut 3");
                GameObject obj = ui.objIcecreemLiquid;
                x = obj.transform.position.x;
                y = obj.transform.position.y + obj.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                Vector2 pt = Common.WorldToCanvasPoint(mainCam, sizeCanvas, new Vector2(x, y));
                RectTransform rctran = uiCup.GetComponent<RectTransform>();
                float oft_bottom = 160;
                //  Debug.Log("LayOut 3 y="+y+" pt="+pt+" sizeCanvas="+sizeCanvas);
                w = rctran.rect.width;
                h = rctran.rect.height;
                x = pt.x - sizeCanvas.x / 2 - w / 2;
                y = pt.y - sizeCanvas.y / 2 - oft_bottom / 2 - 32;
                rctran.anchoredPosition = new Vector2(x, y);
            }
        }

    }

    public void OnGameIronIceCreamDidUpdateStatus(UIView ui, int status)
    {
        if (status == IronIceCreamStepBase.STATUS_Liquid_Finish)
        {
            if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_CHAO)
            {
                uiCup.gameObject.SetActive(false);
            }
        }

        if (status == IronIceCreamStepBase.STATUS_STEP_END)
        {
            btnNext.gameObject.SetActive(true);
        }
    }

    void DoClickFoodItem(UITopFoodItem item)
    {
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_CHAO)
        {
            // if (GameIronIceCream.status == IronIceCreamStepBase.STATUS_STEP_NONE)
            {
                gameIronIceCream.ResetStep();
                uiCup.transform.localRotation = Quaternion.Euler(0, 0, 0);
                UpdateCup(item.index);
                uiCup.gameObject.SetActive(true);

            }

        }
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_CHAN)
        {

        }
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_WAN)
        {
            FoodItemInfo info = uiTopFoodBar.GetItem(item.index);
            gameIronIceCream.UpdateFood(info);
        }
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_ZHUANG)
        {
            uiPopSelectBar.UpdateItem(item.infoFood);
        }

    }
    public void OnUITopFoodBarDidClick(UITopFoodBar bar, UITopFoodItem item)
    {
        Debug.Log("OnUITopFoodBarDidClick  indexStep=" + gameIronIceCream.indexStep);
        if ((item.infoFood != null) && (item.infoFood.isLock))
        {

            if (Common.gold < AppRes.GOLD_CONSUME)
            {
                //星星不足
                StarViewController p = StarViewController.main;
                p.SetType(StarViewController.TYPE_STAR_NOTENOUGHT);
                p.Show(null, null);
            }
            else
            {
                Common.gold -= AppRes.GOLD_CONSUME;
                if (Common.gold < 0)
                {
                    Common.gold = 0;
                }
                //执行解锁
                item.OnUnLockItem();
                RunActionMoveTrophy(item);
            }
        }
        else
        {
            DoClickFoodItem(item);
        }

        LayOut();
    }

    public void OnUIPopSelectBarDidClick(UIPopSelectBar bar, FoodItemInfo info)
    {
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_ZHUANG)
        {
            gameIronIceCream.UpdateFood(info);
        }
    }

    public void OnUITopFoodItemDidClick(UITopFoodItem item)
    {
        Debug.Log("OnUITopFoodItemDidClick gameIronIceCream.indexStep=" + gameIronIceCream.indexStep);
        if (gameIronIceCream.indexStep == GameIronIceCream.INDEX_STEP_CHAO)
        {
            uiCup.transform.localRotation = Quaternion.Euler(0, 0, -45);
            uiCup.ShowHand(false, false);
            FoodItemInfo info = uiTopFoodBar.GetItem(item.index);
            gameIronIceCream.UpdateFood(info);
        }
        LayOut();
    }

    void DoBtnBack()
    {
        base.OnClickBtnBack();

    }

    public override void OnClickBtnBack()
    {
        DoBtnBack();
    }

    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)

    {

        // if (STR_KEYNAME_VIEWALERT_SAVE == alert.keyName)
        // {
        //     if (isYes)
        //     {
        //         DoBtnSave();
        //     }
        // }

    }

    void DoClickBtnStrawAlert()
    {
        // if (AppVersion.appCheckHasFinished && !Application.isEditor)
        // {
        //     if (isFirstUseStraw)
        //     {
        //         //show ad video
        //         AdKitCommon.main.ShowAdVideo();
        //     }
        //     else
        //     {
        //         DoClickBtnStraw();
        //     }
        // }
        // else
        // {
        //     DoClickBtnStraw();
        // }
    }
    public void OnClickBtnNext()
    {
        gameIronIceCream.indexStep++;
        if (gameIronIceCream.indexStep >= gameIronIceCream.totalStep)
        {
            gameIronIceCream.indexStep = 0;
            //return;
        }
        RunSetp(gameIronIceCream.indexStep);

    }



    // public override void AdVideoDidFail(string str)
    // {
    //     ShowAdVideoFailAlert();
    // }

}

