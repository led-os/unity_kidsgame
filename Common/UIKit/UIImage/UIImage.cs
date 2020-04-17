using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIView
{
    public Image image;
    public string keyImage2;



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
            UpdateImage(pic,key);
        }
    }

    public void UpdateImage(string pic,string key)
    {
        if (Common.isBlankString(pic))
        {
            return;
        }
        bool isBoard = ImageRes.main.IsHasBoard(key);
        Vector4 board = Vector4.zero;
        if (isBoard)
        {
            board = ImageRes.main.GetImageBoard(key);
        }
        if(board!=Vector4.zero){
          //  image.imagety
        }
        RectTransform rctranOrigin = this.GetComponent<RectTransform>();
        Vector2 offsetMin = rctranOrigin.offsetMin;
        Vector2 offsetMax = rctranOrigin.offsetMax;
        Debug.Log("UpdateImage pic=" + pic+"isBoard="+isBoard+" keyImage="+key);
        Texture2D tex = TextureCache.main.Load(pic);
        TextureUtil.UpdateImageTexture(image, tex, true, board);
        RectTransform rctan = this.GetComponent<RectTransform>();
        rctan.sizeDelta = new Vector2(tex.width, tex.height);

        if ((rctan.anchorMin == new Vector2(0.5f, 0.5f)) && (rctan.anchorMax == new Vector2(0.5f, 0.5f)))
        {
        }
        else
        {
            //sizeDelta 会自动修改offsetMin和offsetMax 所以需要还原
            rctan.offsetMin = offsetMin;
            rctan.offsetMax = offsetMax;
        }
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
    }


}
