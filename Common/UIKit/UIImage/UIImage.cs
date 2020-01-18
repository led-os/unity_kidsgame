using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIView
{
    public Image image;


    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        UpdateImageByKey(keyImage);
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
    }

    public void UpdateImageByKey(string key)
    {
        string pic = "";
        if (!Common.isBlankString(key))
        {
            pic = ImageRes.main.GetImage(key);
        }

        if (!Common.isBlankString(pic))
        {
            UpdateImage(pic);
        }
    }

    public void UpdateImage(string pic)
    {
        bool isBoard = ImageRes.main.IsHasBoard(keyImage);
        Vector4 board = Vector4.zero;
        if (isBoard)
        {
            board = ImageRes.main.GetImageBoard(keyImage);
        }
        Texture2D tex = TextureCache.main.Load(pic);
        TextureUtil.UpdateImageTexture(image, tex, true, board); 
        RectTransform rctan = this.GetComponent<RectTransform>();
        rctan.sizeDelta = new Vector2(tex.width, tex.height);
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
    }


}
