using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotConfig
{
    public ShotDeviceInfo deviceInfo;
    public int GetTotalPage()
    {
        return 4;
    }
    public ShotItemInfo GetPage(ShotDeviceInfo dev, int idx)
    {
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
                    controller = PlaceViewController.main;
                    break;
                case 1:
                    controller = GameViewController.main;
                    break;
                case 2:
                    controller = GuankaViewController.main;
                    break;
                case 3:
                    controller = HomeViewController.main;
                    break;

                default:
                    controller = HomeViewController.main;
                    break;


            }
        }
        info.controller = controller;

        return info;
    }

}
