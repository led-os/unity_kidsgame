using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHowToPlayPage3 : UIHowToPlayPage, IUIWordBoardDelegate
{

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // textTitle.color = AppRes.colorTitle;
        textDetail.color = AppRes.colorTitle;
        //textTitle.text = Language.main.GetString("STR_HOWTOPLAY_TITLE");
        textDetail.text = Language.main.GetString("STR_HOWTOPLAY_DETAIL3");
        LoadPrefab();
    }
    void Start()
    {
        LayOut();
        // Vector2 pt0 = imageBg.GetComponent<RectTransform>().anchoredPosition;
        // Vector2 pt1 = image1.GetComponent<RectTransform>().anchoredPosition;
        // Vector2 pt2 = image2.GetComponent<RectTransform>().anchoredPosition;
        // Debug.Log("pt0=" + pt0 + " pt1=" + pt1 + " pt2" + pt2);
        // Sequence seq = DOTween.Sequence();
        // RectTransform rctran = imageGuide.GetComponent<RectTransform>();
        // rctran.anchoredPosition = pt0;
        // float t_animation = 2f;
        // Tweener action0 = rctran.DOLocalMove(pt1, t_animation).SetEase(Ease.InSine);
        // Tweener action1 = rctran.DOLocalMove(pt2, t_animation).SetEase(Ease.InSine);
        // seq.Append(action0).Append(action1).AppendInterval(t_animation / 2).OnComplete(
        //     () =>
        //     {
        //         rctran.anchoredPosition = pt0;
        //     }
        // ).SetLoops(-1);
        LevelManager.main.gameLevel = 3;
        UpdateGuankaLevel(LevelManager.main.gameLevel);
    }


    public void UpdateGuankaLevel(int level)
    {
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        GameGuankaParse.main.ParseItem(info);
        uiWordFillBox = (UIWordFillBox)GameObject.Instantiate(uiWordFillBoxPrefab);
        uiWordFillBox.transform.SetParent(objTop.transform);
        uiWordFillBox.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        UIViewController.ClonePrefabRectTransform(uiWordFillBoxPrefab.gameObject, uiWordFillBox.gameObject);
        //   uiWordFillBox.iDelegate = this;
        uiWordFillBox.infoItem = info;
        uiWordFillBox.UpdateGuankaLevel(level);

        UpdateWord();

    }

    public override void LayOut()
    {
        float x, y, w, h;
        {
            RectTransform rctranPage = this.GetComponent<RectTransform>();
            Debug.Log(" page.rect=" + rctranPage.rect + " width=" + width + " heigt=" + heigt);
            x = width / 4;
            y = 0;

        }

        Rect rectImage = Rect.zero;
        //game pic
        {
            float ratio = 0.9f;
            w = this.frame.width * ratio;
            h = (this.frame.height / 2);
            x = 0;
            y = 0;
            if (uiWordFillBox != null)
            {
                RectTransform rctran = uiWordFillBox.GetComponent<RectTransform>();
                w = Mathf.Min(w, h);
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = new Vector2(x, y);
                uiWordFillBox.LayOut();
            }
        }



        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        string strBoard = GameAnswer.main.GetGuankaAnswer(info);
        uiWordBoard.row = 1;
        uiWordBoard.col = strBoard.Length;
    }
    void UpdateWord()
    {
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        //先计算行列数
        LayOut();
        uiWordBoard.iDelegate = this;
        uiWordBoard.InitItem();
        string strBoard = GameAnswer.main.GetGuankaAnswer(info);
        uiWordBoard.UpadteItem(info, strBoard);
    }


    public void UIWordBoardDidClick(UIWordBoard bd, UIWordItem item)
    {
        Debug.Log("UIWordBoardDidClick");
        CaiCaiLeItemInfo infoGuanka = GameGuankaParse.main.GetItemInfo();
        if (uiWordFillBox != null)
        {
            uiWordFillBox.OnAddWord(item.wordDisplay);
            item.ShowContent(false);
        }
    }
}
