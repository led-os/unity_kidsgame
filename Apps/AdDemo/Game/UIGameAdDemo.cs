using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Moonma.AdKit.AdBanner;
public class UIGameAdDemo : UIGameBase
{
    public static string strSourceAdBanner;
    public GameObject objTopBar;
    public Text textTitle;
    public Image imageBar;



    void Awake()
    {
        GameGuankaParse.main.ParseGuanka();


    }


    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel); 
        AdBanner.InitAd(strSourceAdBanner);
        AdBanner.ShowAd(true);
    }

    // Update is called once per frame 
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }


    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);

        AppSceneBase.main.ClearMainWorld();


        LayOut();

        Invoke("OnUIDidFinish", 2f);
    }

    public override void LayOut()
    {

    }

    public override void OnClickBtnBack()
    {
        base.OnClickBtnBack();
    }
}
