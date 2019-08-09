using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWordBoard : UIView, IUIWordItemDelegate
{
    public UIWordItem wordItemPrefab;
    public UIWordBar wordBar;
    List<object> listItem;
    public int row = 6;
    public int col = 4;
    Sprite spriteBg;
    List<string> listWord3500;
    List<string> listWordSel;//从3500汉字中随机选出的字
                             /// <summary>
                             /// Awake is called when the script instance is being loaded.
                             /// </summary>

    public string strWordAnswer = "";

    void Awake()
    {
        spriteBg = LoadTexture.CreateSprieFromResource("AppCommon/UI/Common/word");
        InitItem();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    bool IsWordInString(string word, string strAnswer)
    {
        bool ret = false;
        int len = strAnswer.Length;
        for (int i = 0; i < len; i++)
        {
            string wordAnswer = strAnswer.Substring(i, 1);
            if (word == wordAnswer)
            {
                ret = true;
                Debug.Log("word in answer:" + word);
                break;
            }

        }

        return ret;
    }
    void InitWord(string strAnswer)
    {
        if (listWord3500 == null)
        {
            listWord3500 = new List<string>();
            listWordSel = new List<string>();
        }

        string strAllWord = GameGuankaParse.main.strWord3500;
        int len = strAllWord.Length;
        for (int i = 0; i < len; i++)
        {
            string word = strAllWord.Substring(i, 1);
            if (!IsWordInString(word, strAnswer))
            {
                listWord3500.Add(word);
            }

        }
    }
    public void InitItem()
    {
        if (listItem == null)
        {
            listItem = new List<object>();
        }
        else
        {
            foreach (UIWordItem item in listItem)
            {
                Destroy(item.gameObject);
            }
            listItem.Clear();
        }

        int len = row * col;
        for (int i = 0; i < len; i++)
        {
            string word = i.ToString();
            //Debug.Log(word);
            UIWordItem item = GameObject.Instantiate(wordItemPrefab);
            item.index = i;
            item.iDelegate = this;
            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            item.UpdateTitle(word);
            item.imageBg.sprite = spriteBg;
            item.SetWordColor(Color.white);
            item.SetFontSize(80);
            listItem.Add(item);
        }
    }
    void UpdateWord(int count, string strAnswer)
    {
        listWordSel.Clear();

        //随机生成汉字
        for (int i = 0; i < count; i++)
        {
            int size = listWord3500.Count;
            int rdm = UnityEngine.Random.Range(0, size);
            string str = listWord3500[rdm] as string;
            listWordSel.Add(str);
            listWord3500.RemoveAt(rdm);
        }

        //恢复3500汉字列表
        foreach (string str in listWordSel)
        {
            listWord3500.Add(str);
        }

        //插入答案 
        int len = strAnswer.Length;
        for (int i = 0; i < len; i++)
        {
            string str = strAnswer.Substring(i, 1);
            int size = listWordSel.Count;
            int rdm = UnityEngine.Random.Range(0, size);
            listWordSel.Insert(rdm, str);
        }
    }

    string GetStringAnswer(CaiCaiLeItemInfo info)
    {
        //真正的答案
        string str = UIGameCaiCaiLe.languageWord.GetString(info.id);
        //随机抽取其他关卡的答案
        int gamelevel = LevelManager.main.gameLevel;
        int total = LevelManager.main.maxGuankaNum;
        if (total > 1)
        {

            int size = total - 1;
            int[] idxTmp = new int[size];

            int idx = 0;
            for (int i = 0; i < total; i++)
            {
                if (i != gamelevel)
                {
                    idxTmp[idx++] = i;
                }
            }

            int rdm = Random.Range(0, size);
            if (rdm >= size)
            {
                rdm = size - 1;
            }
            idx = idxTmp[rdm];
            ItemInfo infoOther = GameGuankaParse.main.GetGuankaItemInfo(idx);
            if (infoOther != null)
            {
                string strOther = UIGameCaiCaiLe.languageWord.GetString(infoOther.id);
                string strtmp = RemoveSameWord(str, strOther);
                str += strtmp;
                Debug.Log("other guanka item:" + strOther + " RemoveSameWord:" + strtmp);
            }

        }
        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        if (isonlytext)
        {
            str = strWordAnswer;
        }
        return str;
    }

    //从str2中过滤在str1重复的字
    string RemoveSameWord(string str1, string str2)
    {
        string ret = "";
        int len = str2.Length;
        for (int i = 0; i < len; i++)
        {
            string word = str2.Substring(i, 1);
            if (!IsWordInString(word, str1))
            {
                ret += word;
            }

        }
        return ret;
    }
    public void UpadteItem(CaiCaiLeItemInfo info)
    {
        foreach (UIWordItem item in listItem)
        {
            item.ShowContent(true);
        }

        string str = GetStringAnswer(info);
        Debug.Log("UIWordBoard GetStringAnswer:" + str);
        int len = str.Length;
        int total = row * col;
        InitWord(str);
        UpdateWord(total - len, str);


        int idx = 0;
        foreach (string word in listWordSel)
        {
            UIWordItem item = listItem[idx] as UIWordItem;
            Debug.Log("UIWordBoard UpdateTitle:" + word);
            item.UpdateTitle(word);
            idx++;
        }
    }

    //退回字符
    public void BackWord(string word)
    {
        foreach (UIWordItem item in listItem)
        {
            if (word == item.strWord)
            {
                item.ShowContent(true);
            }
        }
    }


    public void WordItemDidClick(UIWordItem item)
    {
        if (!item.isShowContent)
        {
            return;
        }
        if (wordBar.IsWordFull())
        {
            return;
        }

        wordBar.AddWord(item.strWord);
        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        if (!isonlytext)
        {
            item.ShowContent(false);
        }


    }
}
