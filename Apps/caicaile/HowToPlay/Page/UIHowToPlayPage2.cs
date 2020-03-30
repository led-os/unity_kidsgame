using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class HowPlayWordInfo
{
    public UIWordItem ui;
    public Tweener action;
    public Vector2 pt;

}

public class UIHowToPlayPage2 : UIHowToPlayPage, IUIWordBoardDelegate, IUIWordContentBaseDelegate
{
    Sequence seqAnimate;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        // textTitle.color = AppRes.colorTitle;
        textDetail.color = AppRes.colorTitle;
        //textTitle.text = Language.main.GetString("STR_HOWTOPLAY_TITLE");
        textDetail.text = Language.main.GetString("STR_HOWTOPLAY_DETAIL2");
        LoadPrefab();
    }
    void Start()
    {
        gameLevel = index;
        infoItem = GameLevelParse.main.GetGuankaItemInfo(gameLevel) as CaiCaiLeItemInfo;
        LayOut();
        // LevelManager.main.gameLevel = 0;
        UpdateGuankaLevel(gameLevel);
        //Invoke("RunAnimate", 1.0f);
    }


    public void UpdateGuankaLevel(int level)
    {
        GameLevelParse.main.ParseItem(infoItem);
        uiWordFillBox = (UIWordFillBox)GameObject.Instantiate(uiWordFillBoxPrefab);
        uiWordFillBox.transform.SetParent(objTop.transform);
        uiWordFillBox.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        UIViewController.ClonePrefabRectTransform(uiWordFillBoxPrefab.gameObject, uiWordFillBox.gameObject);
        uiWordFillBox.iDelegate = this;
        uiWordFillBox.infoItem = infoItem;
        uiWordFillBox.UpdateGuankaLevel(level);
        UpdateWord();

    }

    public override void LayOut()
    {
        float x, y, w, h;
        RectTransform rctranPage = this.GetComponent<RectTransform>();
        {

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

        //imageHand
        {
            RectTransform rctran = imageHand.GetComponent<RectTransform>();
            x = rctranPage.rect.size.x / 2 - rctran.rect.size.x;
            y = -rctranPage.rect.size.y / 2 + rctran.rect.size.y;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        if (infoItem != null)
        {
            string strBoard = GameAnswer.main.GetGuankaAnswer(infoItem);
            uiWordBoard.row = 1;
            uiWordBoard.col = strBoard.Length;
            uiWordBoard.LayOut();
        }
    }

    void RunAnimate()
    {
        //position 屏幕坐标
        Vector2 pt0 = imageHand.transform.position;

        seqAnimate = DOTween.Sequence();
        RectTransform rctran = imageHand.GetComponent<RectTransform>();
        float t_animation = 1f;
        float t_rotate = t_animation / 2;
        float anglez = 30;

        Tweener action_prestart = imageHand.transform.DOMove(pt0, 0.01f);
        Sequence seqWordRight = DOTween.Sequence();
        Sequence seqWordError = DOTween.Sequence();
        Tweener actionInitRight = imageHand.transform.DOMove(pt0, t_animation).SetEase(Ease.InSine);
        Tweener actionInitError = imageHand.transform.DOMove(pt0, t_animation).SetEase(Ease.InSine);

        {

            List<HowPlayWordInfo> listWord = new List<HowPlayWordInfo>();
            for (int i = 0; i < uiWordBoard.GetItemCount(); i++)
            {
                Tweener acRotate = imageHand.transform.DORotate(new Vector3(0, 0, 0), t_rotate);
                HowPlayWordInfo info = new HowPlayWordInfo();
                UIWordItem ui = uiWordBoard.GetItem(i);
                info.pt = ui.transform.position;
                info.action = imageHand.transform.DOMove(info.pt, t_animation).SetEase(Ease.InSine);
                info.ui = ui;
                listWord.Add(info);
                seqWordRight
                .Append(info.action.OnComplete(
                () =>
                {
                    Debug.Log("RunAnimate  move end 0");
                    imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -anglez));
                }
                 ))
                .Append(acRotate.OnComplete(
                () =>
                {
                    Debug.Log("RunAnimate  acRotate end 0");
                    imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    info.ui.OnClickItem();
                }
            ));

            }

        }


        {

            List<HowPlayWordInfo> listWord = new List<HowPlayWordInfo>();
            for (int i = uiWordBoard.GetItemCount() - 1; i >= 0; i--)
            {

                Tweener acRotate = imageHand.transform.DORotate(new Vector3(0, 0, 0), t_rotate);
                HowPlayWordInfo info = new HowPlayWordInfo();
                UIWordItem ui = uiWordBoard.GetItem(i);
                info.pt = ui.transform.position;
                info.action = imageHand.transform.DOMove(info.pt, t_animation).SetEase(Ease.InSine);
                info.ui = ui;
                listWord.Add(info);
                seqWordError
                .Append(info.action.OnComplete(
                () =>
                {
                    Debug.Log("RunAnimate  move end 0");
                    imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -anglez));
                }
                 ))
                .Append(acRotate.OnComplete(
                () =>
                {
                    Debug.Log("RunAnimate  acRotate end 0");
                    imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    info.ui.OnClickItem();
                    Debug.Log("RunAnimate  uiUnsel");
                    UILetterItem uiUnsel = uiWordFillBox.GetFistUnSelItem();
                    if (uiUnsel != null)
                    {
                        Debug.Log("RunAnimate  uiUnsel OnClickItem");
                        uiUnsel.OnClickItem();
                    }
                }
            ));

            }

        }



        seqAnimate
         //prestart
         .Append(action_prestart.OnComplete(
            () =>
            {

                //解决动画循环播放时 第一次介绍后imageHand角度被修改成-anglez)的问题
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        ))
        //right 
          .Append(seqWordRight)  //delay
        .AppendInterval(t_animation / 2)
        .Append(actionInitRight.OnComplete(
            () =>
            {
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                imageHand.transform.position = pt0;
                uiWordBoard.OnReset();
                uiWordFillBox.OnReset();
                Debug.Log("RunAnimate  OnReset");
            }
        ))

        //error
        .Append(seqWordError)  //delay
        .AppendInterval(t_animation / 2)
        .Append(actionInitError.OnComplete(
            () =>
            {
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                imageHand.transform.position = pt0;
                uiWordBoard.OnReset();
                uiWordFillBox.OnReset();
            }
        ))
      .SetLoops(-1);
        seqAnimate.Play();

    }

    void StopAnimate()
    {
        seqAnimate.Pause();
        LayOut();
    }
    void UpdateWord()
    {
        //先计算行列数
        LayOut();
        uiWordBoard.iDelegate = this;
        uiWordBoard.InitItem();
        string strBoard = GameAnswer.main.GetGuankaAnswer(infoItem, false);
        Debug.Log(" strBoard=" + strBoard);
        uiWordBoard.UpadteItem(infoItem, strBoard);
    }


    public void UIWordBoardDidClick(UIWordBoard bd, UIWordItem item)
    {
        Debug.Log("UIWordBoardDidClick");
        if (uiWordFillBox != null)
        {
            uiWordFillBox.OnAddWord(item.wordDisplay);
            item.ShowContent(false);
        }
    }

      public void UIWordContentBaseDidBackWord(UIWordContentBase ui, string word)
    {
        uiWordBoard.BackWord(word);
    }
    public void UIWordContentBaseDidTipsWord(UIWordContentBase ui, string word)
    {
        uiWordBoard.HideWord(word);
    }
    public void UIWordContentBaseDidAdd(UIWordContentBase ui, string word)
    {

    }

      public void UIWordContentBaseDidGameFinish(UIWordContentBase ui, bool isFail)
    {

    }
    public override void OnPageExit()
    {
        StopAnimate();
    }
    public override void OnPageEnter()
    {
        RunAnimate();
    }
}
