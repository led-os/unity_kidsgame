using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Moonma.AdKit.AdConfig;

public class UIWordList : UITableViewControllerBase, IUIInputBarDelegate
{
    public RawImage imageBg;
    public UIInputBar uiInputBar; 
    public UIButton btnBack;
    public UIWordSearchResult uiWordSearchResult;


    public enum Type
    {
        LIST = 0,
        SEARCH,
    }

    public Type _type;
    public Type type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
            switch (_type)
            {
                case Type.LIST:
                    {
                        bool isShow = true;
                        tableView.gameObject.SetActive(isShow);
                        uiWordSearchResult.gameObject.SetActive(!isShow);
                        btnBack.gameObject.SetActive(!isShow);
                    }
                    break;
                case Type.SEARCH:
                    {
                        bool isShow = false;
                        tableView.gameObject.SetActive(isShow);
                        uiWordSearchResult.gameObject.SetActive(!isShow);
                        btnBack.gameObject.SetActive(!isShow);
                    }
                    break;
            }
            LayOut();
        }

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        //  textPlaceHold.text = Language.main.GetString("STR_SEARCH");
        uiInputBar.iDelegate = this;
        uiWordSearchResult.gameObject.SetActive(false);
        heightCell = 512;
        oneCellNum = 4;
        LevelManager.main.ParseGuanka();
        listItem = GameLevelParse.main.listGuanka;
        //bg
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_PLACE_BG, true);//IMAGE_GAME_BG


    }
    public void Start()
    {
        base.Start();
        UpdateTable(false);
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }


    public override void LayOut()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
            float w_image = rectTransform.rect.width;
            float h_image = rectTransform.rect.height;
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }


    }
    public override void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        GotoGame(item.index);
    }

    void GotoGame(int idx)
    {
        LevelManager.main.gameLevel = idx;
        GameManager.main.GotoGame(this.controller);
    }

    public void OnClickBtnBack()
    {
        this.type = Type.LIST;
    }

    public void OnClickBtnPre()
    {
    }
    public void OnClickBtnNext()
    {
    }




    public void OnUIInputBarValueChanged(UIInputBar ui)
    {

        string str = ui.text;
        this.type = Type.SEARCH;
        uiWordSearchResult.UpdateList(str);
    }
    public void OnUIInputBarEnd(UIInputBar ui)
    {

    }

}

