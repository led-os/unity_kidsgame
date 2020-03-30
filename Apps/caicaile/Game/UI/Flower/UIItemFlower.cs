
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IUIItemFlowerDelegate
{
    void OnUIItemFlowerTouchDown(UIItemFlower ui);
    void OnUIItemFlowerTouchMove(UIItemFlower ui);
    void OnUIItemFlowerTouchUp(UIItemFlower ui);
}
public class UIItemFlower : UIView
{
    public enum Status
    {
        NORMAL = 0,
        SELECT,
    }
    public UIImage imageBg;
    public UIText textTitle;
    public int indexRow;
    public int indexCol;
    public int index;
    public bool isAnswerItem;
    public string word;



    private IUIItemFlowerDelegate _delegate;

    public IUIItemFlowerDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    private Status _status;
    public Status status
    {
        get { return _status; }
        set
        {
            _status = value;
            // imageSel.gameObject.SetActive(false);

            if (_status == Status.SELECT)
            {
                imageBg.UpdateImageByKey("LetterBgNormal");
                // imageBg.gameObject.SetActive(true);
                // TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgLock, true);
                // textTitle.gameObject.SetActive(false);
                // imageIcon.gameObject.SetActive(false);
                // imageSel.gameObject.SetActive(true);


            }


            if (_status == Status.NORMAL)
            {
                imageBg.UpdateImageByKey("LetterBgSel");

                // textTitle.color = Color.black;
                // imageBg.gameObject.SetActive(true);
                // TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgNormal, true);
                // textTitle.gameObject.SetActive(true);
                // imageIcon.gameObject.SetActive(false);
            }

        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
     
        status = Status.NORMAL;
        LayOut();
    }


    public override void LayOut()
    {
        base.LayOut();
    }


    public void UpdateItem(string letter)
    {
        word = letter;
        textTitle.text = letter;
    }

    public void OnClickItem()
    {
        Debug.Log("OnUILetterItemDidClick OnClickItem");

    }
}
