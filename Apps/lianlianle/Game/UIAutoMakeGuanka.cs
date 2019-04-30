using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class UIAutoMakeGuanka : MonoBehaviour
{
    AutoMakeGuanka autoMakeGuanka;
    void Awake()
    {
        autoMakeGuanka = new AutoMakeGuanka();

    }
    public void OnClickBtnGuanka()
    {
        autoMakeGuanka.RunAutoMake();
    }
}
