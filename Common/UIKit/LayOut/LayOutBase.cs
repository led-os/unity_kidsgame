using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayOutBase : MonoBehaviour
{
    public virtual void LayOut()
    {

    }

    public int GetChildCount()
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
