using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotConfig
{
    public ShotDeviceInfo deviceInfo;
    public int GetTotalPage()
    {
        return 5;
    }
    public ShotItemInfo GetPage(ShotDeviceInfo dev, int idx)
    {
        ShotItemInfo info = new ShotItemInfo();
        UIViewController controller = null;
        info.isRealGameUI = true;
        if (dev.name == ScreenDeviceInfo.DEVICE_NAME_ICON)
        {
            controller = IconViewController.main;
            IconViewController.main.deviceInfo = dev;
        }
        else if (dev.name == ScreenDeviceInfo.DEVICE_NAME_AD)
        {
            controller = AdHomeViewController.main;
        }
        else if (dev.name == ScreenDeviceInfo.DEVICE_NAME_COPY_RIGHT_HUAWEI)
        {
            controller = CopyRightViewController.main;
            CopyRightViewController.main.deviceInfo = dev;
        }
        else
        {
            switch (idx)
            {
                case 0:
                    {
                        controller = HomeViewControllerDict.main;
                    }
                    break;
                case 1:
                    {
                        controller = SearchViewController.main;
                    }
                    break;
                case 2:
                    {
                        controller = HistoryViewController.main;
                    }
                    break;
                case 3:
                    {
                        controller = LoveViewController.main;
                    }
                    break;
                case 4:
                    {
                        controller = SettingViewController.main;
                    }
                    break;

                default:
                    {
                        controller = HomeViewControllerDict.main;
                    }
                    break;


            }
        }
        info.controller = controller;

        return info;
    }

}
