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
        var pic = this.GetKeyImage();
        // var board = null;
        // if (cc.ImageRes.main().ContainsBoard(this.keyImage))
        // {
        //     board = cc.ImageRes.main().GetImageBoardSync(this.keyImage);
        // }
        if (!Common.isBlankString(pic))
        {
            UpdateImage(pic);
        }
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
    }

    public void UpdateImage(string pic)
    {
        TextureUtil.UpdateImageTexture(image, pic, true);
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
    }


}
