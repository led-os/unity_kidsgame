using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class FoodItemInfo
{
    public string pic;
    public string picMultiColor;
    public UITopFoodItem.Type type;
    public int index;
    public bool isLock;
    public string id;
    public bool isSingleColor;
    //卷的下方
    public bool isUnderJuan;

}
public class UIGameIceCream : UIGameBase
{
    public const string STR_KEYNAME_VIEWALERT_SAVE_FINISH = "STR_KEYNAME_VIEWALERT_SAVE_FINISH";

    UIGameTopBar uiGameTopBarPrefab;
    public UIGameTopBar uiGameTopBar;



    // bool isFirstUseStraw
    // {
    //     get
    //     {
    //         if (Common.noad)
    //         {
    //             return false;
    //         }
    //         //   return Common.Int2Bool(PlayerPrefs.GetInt(KEY_STR_FIRST_USE_STRAW, Common.Bool2Int(true)));
    //     }
    //     set
    //     {

    //         //  PlayerPrefs.SetInt(KEY_STR_FIRST_USE_STRAW, Common.Bool2Int(value));
    //     }
    // }

    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {


    }


    public void LoadPrefabBase()
    {

        {
            GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/UIGameTopBar");
            if (obj != null)
            {
                uiGameTopBarPrefab = obj.GetComponent<UIGameTopBar>();
                uiGameTopBar = (UIGameTopBar)GameObject.Instantiate(uiGameTopBarPrefab);

                RectTransform rctranPrefab = uiGameTopBarPrefab.transform as RectTransform;
                //  AppSceneBase.main.AddObjToMainCanvas(uiGameTopBar.gameObject);
                uiGameTopBar.transform.parent = this.transform;
                RectTransform rctran = uiGameTopBar.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;

            }

        }



    }

    public void LayOutBase()
    {
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        float scalex = 0, scaley = 0, scale = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;


    }
 
    public void ShowFirstUseAlert()
    {

        // string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_FIRST_USE_FUNCTION");
        // string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_FIRST_USE_FUNCTION");
        // string yes = Language.main.GetString("STR_UIVIEWALERT_YES_FIRST_USE_FUNCTION");
        // string no = "no";
        // ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION, OnUIViewAlertFinished);

    }

    void DoBtnBack()
    {
        base.OnClickBtnBack();
        ShowAdInsert(1);
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

    public void OnClickBtnStraw()
    {


    }



    // public override void AdVideoDidFail(string str)
    // {
    //     ShowAdVideoFailAlert();
    // }

  
}

