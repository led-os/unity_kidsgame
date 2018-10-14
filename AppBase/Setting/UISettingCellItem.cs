
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class UISettingCellItem : UICellItemBase
{
    public Text textTitle;
    public Image imageBg;
    public Image imageArrow;
    public Button btnSwitch;
    public const string IMAGE_BTN_SWITCH_UNSEL = "App/UI/Common/BtnSwitchUnsel";
    public const string IMAGE_BTN_SWITCH_SEL = "App/UI/Common/BtnSwitchsel";
    string[] strImageBg = { AppRes.IMAGE_CELL_BG_BLUE, AppRes.IMAGE_CELL_BG_ORINGE, AppRes.IMAGE_CELL_BG_YELLOW };

    public override void UpdateItem(List<object> list)
    {
        if (index < list.Count)
        {
            ItemInfo info = list[index] as ItemInfo;
            textTitle.text = info.title;
            tagValue = info.tag;
            ShowSwitchButton(IsShowSwitchButton(info));
            if (info.tag == (int)SettingItemTag.TAG_SETTING_BACKGROUND_MUSIC)
            {
                bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
                UpdateBtnSwitch(ret);
            }
            Vector4 border = AppRes.borderCellSettingBg;
            TextureUtil.UpdateImageTexture(imageBg, strImageBg[index % 3], false, border);
        }
    }
    bool IsShowSwitchButton(ItemInfo info)
    {
        bool ret = false;
        if (info.tag == (int)SettingItemTag.TAG_SETTING_BACKGROUND_MUSIC)
        {
            ret = true;
        }
        return ret;
    }

    void ShowSwitchButton(bool isShow)
    {
        btnSwitch.gameObject.SetActive(isShow);
    }
    void UpdateBtnSwitch(bool isSel)
    {
        Image img = btnSwitch.GetComponent<Image>();
        if (isSel)
        {
            img.sprite = LoadTexture.CreateSprieFromTex(TextureCache.main.Load(IMAGE_BTN_SWITCH_SEL));
        }
        else
        {
            img.sprite = LoadTexture.CreateSprieFromTex(TextureCache.main.Load(IMAGE_BTN_SWITCH_UNSEL));
        }
    }
    public void CellSwitchDidClick()
    {
        Debug.Log("CellSwitchDidClick");
        if (tagValue == (int)SettingItemTag.TAG_SETTING_BACKGROUND_MUSIC)
        {
            bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
            bool value = !ret;
            Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, value);
            UpdateBtnSwitch(value);
            if (value)
            {
                AudioPlay.main.Play();
            }
            else
            {
                AudioPlay.main.Stop();
            }


        }
    }

}

