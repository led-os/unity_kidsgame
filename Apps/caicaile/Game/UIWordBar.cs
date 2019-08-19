using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUIWordBarDidGameFinish(UIWordBar bar, bool isFail);
public delegate void OnUIWordBarNotEnoughGold(UIWordBar bar, bool isUpdate);
public class UIWordBar : UIView, IUIWordItemDelegate
{
    public UIWordItem wordItemPrefab;
    public UIWordBoard wordBoard;

    int wordNumMax;//最大字符数
    int wordNumCur;//当前字符数
    bool isAllAnswer = false;
    List<UIWordItem> listItem;
    Color colorNormal = Color.white;
    Color colorFail = Color.red;
    Color colorTips = new Color(107 / 255.0f, 1f, 1.0f, 1.0f);

    Sprite spriteBg;
    public OnUIWordBarDidGameFinish callbackGameFinish { get; set; }
    public OnUIWordBarNotEnoughGold callbackGold { get; set; }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        spriteBg = LoadTexture.CreateSprieFromResource("App/UI/Common/wordbar_item_bg");

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpadteItem(CaiCaiLeItemInfo info)
    {

        string str = UIGameCaiCaiLe.languageWord.GetString(info.id);
        Debug.Log("UIWordBar UpadteItem:" + str);
        int len = str.Length;
        wordNumMax = len;
        wordNumCur = 0;
        //
        if (listItem != null)
        {
            foreach (UIWordItem item in listItem)
            {
                DestroyImmediate(item.gameObject);
            }
        }


        if (listItem == null)
        {
            listItem = new List<UIWordItem>();
        }
        listItem.Clear();
        isAllAnswer = false;
        LayOutHorizontal ly = this.gameObject.GetComponent<LayOutHorizontal>();
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        float oft = 0;
        if (ly != null)
        {
            oft = ly.space.x;
        }
        float w = (rctran.rect.size.x - oft * (len - 1)) / len;
        float limit_max = 160f;
        if (w > limit_max)
        {
            w = limit_max;
        }
        Debug.Log("UpadteItem w = " + w + " len=" + len);
        float h = w;


        for (int i = 0; i < len; i++)
        {
            string word = str.Substring(i, 1);
            Debug.Log(word);
            UIWordItem item = GameObject.Instantiate(wordItemPrefab);
            item.index = i;
            item.iDelegate = this;
            item.strWordAnswer = word;
            rctran = item.gameObject.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(w, h);

            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            listItem.Add(item);
            item.ClearWord();
            item.SetWordColor(colorNormal);
            item.SetFontSize(100);
            item.imageBg.sprite = spriteBg;

            //item.UpdateTitle(word);
        }


        if (ly != null)
        {
            ly.LayOut();
        }

    }

    public void AddWord(string word)
    {
        bool isonlytext = GameGuankaParse.main.OnlyTextGame();
        if (isonlytext)
        {
            CheckAnswerOnlyText(word);
            return;
        }

        foreach (UIWordItem item in listItem)
        {
            if (Common.BlankString(item.strWord))
            {
                item.UpdateTitle(word);
                wordNumCur++;
                break;
            }
        }

        if (IsWordFull())
        {
            CheckAnswer();
        }
    }

    public bool IsWordFull()
    {
        // bool ret = false;
        // if (wordNumCur >= wordNumMax)
        // {
        //     ret = true;
        // }

        bool ret = true;
        foreach (UIWordItem item in listItem)
        {
            if (Common.BlankString(item.strWord))
            {
                ret = false;
            }
        }

        return ret;
    }


    void CheckAnswerOnlyText(string word)
    {

        isAllAnswer = false;

        if (wordBoard.strWordAnswer == word)

        {
            isAllAnswer = true;

        }

        if (isAllAnswer)
        {
            //全部猜对 game win
            OnGameWin();

        }
        else
        {
            //游戏失败
            OnGameFail();
        }
    }

    //判断答案是否正确
    void CheckAnswer()
    {
        isAllAnswer = true;
        foreach (UIWordItem item in listItem)
        {
            if (!item.IsAnswer())
            {
                isAllAnswer = false;
                break;
            }
        }



        if (isAllAnswer)
        {
            //全部猜对 game win
            OnGameWin();

        }
        else
        {
            //游戏失败
            OnGameFail();
        }
    }

    void OnGameFail()
    {
        Debug.Log("UIWordBar OnGameFail");
        foreach (UIWordItem item in listItem)
        {
            if (!item.isWordTips)
            {
                item.SetWordColor(colorFail);
                item.StartAnimateError();
            }

            // const float FADE_ANIMATE_TIME = 1.0f;
            // iTween.FadeTo(item.textTitle.gameObject, iTween.Hash("alpha", 0, "time", FADE_ANIMATE_TIME, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong));
        }
        if (this.callbackGameFinish != null)
        {
            this.callbackGameFinish(this, true);
        }

    }
    void OnGameWin()
    {
        if (this.callbackGameFinish != null)
        {
            this.callbackGameFinish(this, false);
        }

        // GameScene.ShowAdInsert();
    }

    public void WordItemDidClick(UIWordItem item)
    {

        if (!isAllAnswer)
        {
            if (!Common.BlankString(item.strWord))
            {
                Debug.Log("WordItemDidClick 3");
                //字符退回
                wordBoard.BackWord(item.strWord);
                item.ClearWord();
                //item.StopAnimateError();
                wordNumCur--;
                if (wordNumCur < 0)
                {
                    wordNumCur = 0;
                }
                //恢复颜色
                foreach (UIWordItem it in listItem)
                {
                    it.SetWordColor(colorNormal);
                    it.StopAnimateError();
                }
            }
        }
    }

    //没有答但不是空的
    List<UIWordItem> GetNotAnswerList()
    {
        List<UIWordItem> listRet = new List<UIWordItem>();
        foreach (UIWordItem item in listItem)
        {
            if (!item.IsAnswer() && !Common.BlankString(item.strWord))
            {
                listRet.Add(item);
            }
        }
        return listRet;
    }

    //空白列表
    List<UIWordItem> GetBlankList()
    {
        List<UIWordItem> listRet = new List<UIWordItem>();
        foreach (UIWordItem item in listItem)
        {
            if (Common.BlankString(item.strWord))
            {
                listRet.Add(item);
            }
        }
        return listRet;
    }



    public void OnClickBtnTips()
    {
        if (Common.gold <= 0)
        {
            if (this.callbackGold != null)
            {
                this.callbackGold(this, false);
            }
            return;
        }
        //先提示空白的
        List<UIWordItem> listRet = GetBlankList();
        if (listRet.Count == 0)
        {
            listRet = GetNotAnswerList();
        }
        if (listRet.Count != 0)
        {
            int rdx = Random.Range(0, listRet.Count);
            if (rdx >= 0)
            {
                UIWordItem item = listRet[rdx];
                item.SetWordColor(colorTips);
                item.UpdateTitle(item.strWordAnswer);
                item.isWordTips = true;
                Common.gold--;
                if (Common.gold < 0)
                {
                    Common.gold = 0;
                }

                Debug.Log("Common.gold =" + Common.gold);

                if (this.callbackGold != null)
                {
                    this.callbackGold(this, true);
                }

                foreach (UIWordItem it in listItem)
                {
                    it.StopAnimateError();
                }

                if (IsWordFull())
                {
                    CheckAnswer();
                }
            }

        }
    }
}
