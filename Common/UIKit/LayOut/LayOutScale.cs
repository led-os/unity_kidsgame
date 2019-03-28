using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LayOutScale : LayOutBase
{
    public Type _scaleType;
    public float ratio = 1f;

    public Type scaleType
    {
        get
        {
            return _scaleType;
        }

        set
        {
            _scaleType = value;
            LayOut();
        }

    }

    public Vector2 _offsetMin;
    public Vector2 offsetMin
    {
        get
        {
            return _offsetMin;
        }

        set
        {
            _offsetMin = value;
            LayOut();
        }

    }

    public Vector2 _offsetMax;
    public Vector2 offsetMax
    {
        get
        {
            return _offsetMax;
        }

        set
        {
            _offsetMax = value;
            LayOut();
        }

    }

    public enum Type
    {
        MIN = 0,
        MAX,
    }

    void Awake()
    {
        Debug.Log("LayOutScale Awake=");
    }
    void Start()
    {
        // Debug.Log("LayOutScale Start=");
        // this.LayOut();

        //保持和layoutgroup同步
        StartCoroutine(OnLayOutEnumerator());
    }
    IEnumerator OnLayOutEnumerator()
    {
        float time = 0;
        yield return new WaitForSeconds(time);
        this.LayOut();
    }

    public override void LayOut()
    {
        UpdateType(scaleType);
    }


    void UpdateType(Type ty)
    {
        _scaleType = ty;
        switch (this._scaleType)
        {
            case Type.MIN:
                {
                    this.ScaleObj(this.gameObject, false);
                }
                break;
            case Type.MAX:
                {
                    this.ScaleObj(this.gameObject, true);
                }
                break;

        }
    }


    void ScaleObj(GameObject obj, bool isMaxFit)
    {

        float x, y, w = 0, h = 0;
        SpriteRenderer rd = obj.GetComponent<SpriteRenderer>();
        if (rd != null)
        {
            w = rd.sprite.texture.width / 100f;
            h = rd.sprite.texture.height / 100f;
        }
        else
        {
            RawImage rawimage = obj.GetComponent<RawImage>();
            if (rawimage != null)
            {
                w = rawimage.texture.width;
                h = rawimage.texture.height;
            }
            else
            {
                Image image = obj.GetComponent<Image>();
                if (image != null)
                {
                    w = image.sprite.texture.width;
                    h = image.sprite.texture.height;
                }
            }
        }
        RectTransform rctran = this.transform.parent as RectTransform;

        var w_parent = rctran.rect.width;
        var h_parent = rctran.rect.height;
        w_parent -= (this.offsetMin.x + this.offsetMax.x);
        h_parent -= (this.offsetMin.y + this.offsetMax.y);



        float scale = 1f;
        if (w != 0 && h != 0)
        {
            if (isMaxFit == true)
            {
                scale = Common.GetMaxFitScale(w, h, w_parent, h_parent) * ratio;
            }
            else
            {
                scale = Common.GetBestFitScale(w, h, w_parent, h_parent) * ratio;
            }
        }

        Debug.Log("LayOutScale scale=" + scale + " w_parent=" + w_parent + " h_parent=" + h_parent);
        obj.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
