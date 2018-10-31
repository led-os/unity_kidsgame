using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//图片帧动画
public class ActionImage : ActionBase
{
    public int count = 20;
    int count_step = 0;
    List<string> listPic;

    float deltaTime = 0;
    float animationTimeDelay = 0;
    public void AddPic(string pic)
    {
        listPic.Add(pic);
    }

    void UpdateImage()
    {
        if (index < listPic.Count)
        {
            Debug.Log("UpdateImage:index=" + index + " listPic.Count=" + listPic.Count);
            string pic = listPic[index];
            Texture2D tex = TextureCache.main.Load(pic);
            SpriteRenderer rd = this.gameObject.GetComponent<SpriteRenderer>();
            if ((rd != null) && (tex != null))
            {
                rd.sprite = LoadTexture.CreateSprieFromTex(tex);
            }
        }
    }
    public override void InitAction()
    {
        Debug.Log("ActionImage:InitAction");
        listPic = new List<string>();
        index = 0;
        isUpdateByPercent = false;
        percentage = 0;
    }
    public override void UpdateAction()
    {

        if (listPic.Count == 0)
        {
            return;
        }
        animationTimeDelay = duration / listPic.Count;
        deltaTime += Time.deltaTime;
        // Debug.Log(animationDeltaTime);
        if (deltaTime >= animationTimeDelay)
        {
            deltaTime = 0;
            UpdateImage();
            index++;
            if (index >= listPic.Count)
            {
                OnFinish();
                index = 0;
            }
        }


    }

    public override void OnActionComplete()
    {
        if (target != null)
        {
            target.gameObject.SetActive(true);
        }

    }
}
