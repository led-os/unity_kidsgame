using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public interface IGameDelegate
{
    void OnGameDidWin(GameBase g);
    void OnGameDidFail(GameBase g);
    void OnGameUpdateTitle(GameBase g,ItemInfo info,bool isshow);

}

public class GameBase : UIView
{
    public List<object> listItem;
    private IGameDelegate _delegate;

    public IGameDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }


    public void OnGameWin()
    {
        if (iDelegate != null)
        {
            iDelegate.OnGameDidWin(this);
        }
    }
    public void OnGameFail()
    {
        if (iDelegate != null)
        {
            iDelegate.OnGameDidFail(this);
        }
    }
}
