using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotConfig
{
    public ShotDeviceInfo deviceInfo;
    int indexStep = 0;
    public int GetTotalPage()
    {
        return 5;
    }
    public ShotItemInfo GetPage(ShotDeviceInfo dev, int idx)
    {
        GameViewController.gameType = "IronIceCream";
        GameViewController.dirRootPrefab = "AppCommon/Prefab/Game/" + GameViewController.gameType;

        ShotItemInfo info = new ShotItemInfo();
        UIViewController controller = null;
        info.isRealGameUI = true;
        if (dev.name == UIScreenShotController.DEVICE_NAME_ICON)
        {
            controller = IconViewController.main;
            IconViewController.main.deviceInfo = dev;
        }
        else if (dev.name == UIScreenShotController.DEVICE_NAME_AD)
        {
            controller = AdHomeViewController.main;
        }
        else if (dev.name == UIScreenShotController.DEVICE_NAME_COPY_RIGHT_HUAWEI)
        {
            controller = CopyRightViewController.main;
            CopyRightViewController.main.deviceInfo = dev;
        }
        else
        {
            switch (idx)
            {
                case 0:
                    controller = GameViewController.main;
                    indexStep = 0;
                    break;
                case 1:
                    controller = GameViewController.main;
                    indexStep = 1;
                    break;
                case 2:
                    controller = GameViewController.main;
                    indexStep = 2;

                    break;
                case 3:
                    controller = GameViewController.main;
                    indexStep = 3;

                    break;
                case 4:
                    controller = HomeViewController.main;
                    break;

                default:
                    controller = HomeViewController.main;
                    break;


            }
        }
        info.controller = controller;
        UIGameIronIceCream.indexStepScreenShot = indexStep;
        return info;
    }
    void GotoStep()
    {
        UIGameIronIceCream game = GameViewController.main.gameBase as UIGameIronIceCream;
        game.RunSetp(indexStep);
    }
}
