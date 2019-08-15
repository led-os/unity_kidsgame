using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayOutBase : MonoBehaviour
{
    public bool enableLayout = true;
    public bool enableHide = true;//是否过虑Hide
    public Vector2 space = Vector2.zero;
    public virtual void LayOut()
    {

    }

    public int GetChildCount(bool includeHide = true)
    {
        int count = 0;
        foreach (Transform child in this.gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child == null)
            {
                // 过滤已经销毁的嵌套子对象 
                continue;
            }
            GameObject objtmp = child.gameObject;
            if (this.gameObject == objtmp)
            {
                continue;
            }

            if (!includeHide)
            {
                if (!objtmp.activeSelf)
                {
                    //过虑隐藏的
                    continue;
                }
            }

            if (objtmp.transform.parent != this.gameObject.transform)
            {
                //只找第一层子物体
                continue;
            }
            count++;
        }
        return count;
    }
}
