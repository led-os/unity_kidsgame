using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewControllerManager
{ 
	public UIViewController rootViewController;
    static public Vector2 sizeCanvas;
    
	static private ViewControllerManager _main = null;
    public static ViewControllerManager main
    {
        get
        {
            if (_main == null)
            {
                _main = new ViewControllerManager();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

    }


    static public void ClonePrefabRectTransform(GameObject objPrefab,GameObject obj)
    {

        RectTransform rctranPrefab = objPrefab.GetComponent<RectTransform>();
        //Debug.Log("rctranPrefab.offsetMin=" + rctranPrefab.offsetMin + " rctranPrefab.offsetMax=" + rctranPrefab.offsetMax);
        
        RectTransform rctran = obj.GetComponent<RectTransform>(); 
        rctran.offsetMax = rctranPrefab.offsetMax; 
        rctran.offsetMin = rctranPrefab.offsetMin; 
        
        //Debug.Log("rctran.offsetMin=" + rctran.offsetMin + " rctran.offsetMax=" + rctran.offsetMax);

    }
}
