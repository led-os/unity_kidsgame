using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIToolBarLearnWord : UIView
{
    public Button btnSound;
    public Button btnDemo;
    public Button btnWrite;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnClickBtnSound()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        AudioPlay.main.PlayUrl(info.dbInfo.audio);

    }

    public void OnClickBtnDemo()
    {
        PopUpManager.main.Show<UIViewPop>("AppCommon/Prefab/Game/UIWordGif", popup =>
     {
         Debug.Log("UIViewAlert Open ");
         UIWordGif ui = popup as UIWordGif;
         WordItemInfo info = GameLevelParse.main.GetItemInfo();
         ui.Updateitem(info);

     }, popup =>
     {


     });

    }
    public void OnClickBtnWrite()
    {


    }
}
