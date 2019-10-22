
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnswer
{


    List<string> listWord3500;
    List<string> listWordSel;//从3500汉字中随机选出的字
                             /// <summary>
                             /// Awake is called when the script instance is being loaded.
                             /// </summary>
    public string strWordAnswer = "";

    static private GameAnswer _main = null;
    public static GameAnswer main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameAnswer();
            }
            return _main;
        }
    }


    public string GetGuankaAnswer(CaiCaiLeItemInfo info, bool isRandom = true)
    {
        string str = "";
        //真正的答案
        if ((info.gameType == GameRes.GAME_TYPE_IMAGE) || (info.gameType == GameRes.GAME_TYPE_IMAGE_TEXT))
        {
            str = UIGameCaiCaiLe.languageWord.GetString(info.id);
            //歇后语
            if ((!Common.BlankString(info.head)) && (!Common.BlankString(info.end)))
            {
                return info.end;
            }
        }
        if (info.gameType == GameRes.GAME_TYPE_TEXT)
        {
            str = GameAnswer.main.strWordAnswer;
        }
        if (info.gameType == GameRes.GAME_TYPE_CONNECT)
        {
            for (int i = 0; i < info.listWordAnswer.Count; i++)
            {
                int idx = info.listWordAnswer[i];
                string word = info.listWord[idx];
                int rdm = Random.Range(0, str.Length);
                //是否打乱
                if (!isRandom)
                {
                    rdm = str.Length;
                    if (rdm < 0)
                    {
                        rdm = 0;
                    }
                }

                str = str.Insert(rdm, word);
                Debug.Log("GetGuankaAnswer rdm=" + rdm + " word=" + word + " str=" + str);
            }
        }
        return str;
    }

    public string GetWordBoardString(CaiCaiLeItemInfo info, int row, int col)
    {
        string ret = "";
        string str = GetInsertToBoardAnswer(info);

        int len = str.Length;
        int total = row * col;
        Debug.Log("UIWordBoard GetWordBoardString:" + str + " str.len=" + len);
        InitWord(str);
        UpdateWord(total - len, str);
        foreach (string strtmp in listWordSel)
        {
            ret += strtmp;
        }
        Debug.Log("UIWordBoard GetWordBoardString:" + str + " str.len=" + len + " total=" + total + " ret=" + ret + " ret.count=" + ret.Length);
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

    //插入答案
    public string GetInsertToBoardAnswer(CaiCaiLeItemInfo info)
    {
        //真正的答案
        string str = GameAnswer.main.GetGuankaAnswer(info);
        Debug.Log("UIWordBoard GetGuankaAnswer:" + str);
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
            CaiCaiLeItemInfo infoOther = GameGuankaParse.main.GetGuankaItemInfo(idx) as CaiCaiLeItemInfo;
            if (infoOther != null)
            {
                string strOther = GameAnswer.main.GetGuankaAnswer(infoOther);
                string strtmp = RemoveSameWord(str, strOther);
                str += strtmp;
                Debug.Log("UIWordBoard other strOther=:" + strOther + " RemoveSameWord:" + strtmp);
            }

        }

        if (info.gameType == GameRes.GAME_TYPE_TEXT)
        {
            str = GameAnswer.main.strWordAnswer;
        }
        if (info.gameType == GameRes.GAME_TYPE_CONNECT)
        {
            str = GameAnswer.main.GetGuankaAnswer(info);
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




}
