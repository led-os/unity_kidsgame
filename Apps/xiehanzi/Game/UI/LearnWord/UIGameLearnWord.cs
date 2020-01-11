﻿using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Moonma.Share;
//笔画顺序查询网： https://bihua.51240.com/e68891__bihuachaxun/



public class UIGameLearnWord : UIGameBase
{

    //top
    public UIImage imageBgWord;
    public UIImage imageWord;

    //detail
    public UIImage imageBgDetail;
    public UIText textWord;
    public UIText textPinyin;//拼音
    public UIText textZuci;//组词
    public UIText textBushou;//部首
    public UIText textBihua;//笔画
    public UIText textContentPinyin;
    public UIText textContentZuci;
    public UIText textContentBushou;
    public UIText textContentBihua;

    void Awake()
    {
        LoadPrefab();
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);
    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }

    }

    void LoadPrefab()
    {


    }



    void UpdateItem(WordItemInfo info)
    {

        textPinyin.text = info.pinyin;
        textZuci.text = info.zuci;
        textBushou.text = info.bushou;
        textBihua.text = info.bihua;
    }


    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        AppSceneBase.main.ClearMainWorld();
        WordItemInfo infonow = GameLevelParse.main.GetItemInfo();
        long tickItem = Common.GetCurrentTimeMs();
        tickItem = Common.GetCurrentTimeMs() - tickItem;
        Debug.Log("ParserGuankaItem: tickItem=" + tickItem);
        UpdateItem(infonow);
    }
    public override void LayOut()
    {
        base.LayOut();
        // base.LayOut();
    }
    public void OnClickBtnWrite()
    {

    }


}
