using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Transactions : MonoBehaviour
{
    [SerializeField] private ApiManager api;
    [SerializeField] private TMP_InputField nick;
    [SerializeField] private TMP_InputField sum;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TransactionHistory history;

    [SerializeField] private int max;
    [SerializeField] private int min;
    [SerializeField] private string username;
    [SerializeField] private GameObject text;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private Button btn;
    private Message currentMsg;
    private int step;
    private int factor = 1;

    private void Start()
    {
        texts = text.GetComponentsInChildren<TextMeshProUGUI>();
        step = texts.Length;
        GetHisotry();
    }

    public void Slide(int f)
    {
        var tempFactor = factor + f;
        if (tempFactor >= 1 && (step == history.TransactionMessages.Count || f < 0))
        {
            factor += f;
            GetHisotry();
        }
    }

    private void OnEnable()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => gameObject.SetActive(true));
    }

    private void SetTexts()
    {
        for (var i = 0; i < step; i++)
        {
            texts[i].text = "";
            if (i < history.TransactionMessages.Count)
            {
                var h = history.TransactionMessages[i];
                if (h.To == UserController.Shared.User.UserName)
                {
                    texts[i].text = $"{h.Sum} �� {h.From} {h.Date}";
                    texts[i].color = Color.green;
                }
                else
                {
                    texts[i].text = $"{h.Sum} ������ {h.To} {h.Date}";
                    texts[i].color = Color.red;
                }
            }
        }
    }

    public void MakeTransaction()
    {
        var u = UserController.Shared.User;

        var coins = u.Coins;
        var thisNickname = u.UserName;

        var sendCoins = Convert.ToInt32(sum.text);



        if (coins >= sendCoins && nick.text != thisNickname)
        {
            api.SendCoins(thisNickname, nick.text, sendCoins);
            currentMsg = new Message($"���������� {sendCoins} ����� {nick.text}");
        }
        else if(coins < sendCoins)
        {
            errorText.text = "������������ �����";
        }
        else if(nick.text == thisNickname) 
        {
            var msg = new Message("������ ��������� ������ ������ ����", MessageTypes.Error);
            MenuMessages.instance.CreateMessage(msg);
        }
    }

    public void GetHisotry()
    {
        var min = step * factor - step;
        api.GetHistory(min, step, UserController.Shared.User.UserName);
        print(min);
    }

    public void HandleGetHistory(TransactionHistory h)
    {
        history = h;
        SetTexts();
    }

    public void GetMessage(SendTransactionMessage msg)
    {

        print(msg.LogType);
        print(msg.Message);
        switch (msg.LogType)
        {
            case SendTransactionMessage.Log.Success:
                var login = PlayerPrefs.GetString(PlayerPrefsNames.USER_LOGIN);
                var password = PlayerPrefs.GetString(PlayerPrefsNames.USER_PASSWORD);
                api.Login(login, password);
                MenuMessages.instance.CreateMessage(currentMsg);
                break;

            case SendTransactionMessage.Log.Warning:
                var m = msg.Message;
                errorText.text = m;
                break;
        }

    }
}

[Serializable]
public class TransactionHistory
{
    public List<TransactionMessage> TransactionMessages;
}
[Serializable]
public class TransactionMessage
{
    public string From;
    public string To;
    public int Sum;
    public int Id;
    public string Date;
}

public class SendTransactionMessage
{
    public enum Log { Warning, Success }
    public string Type;
    public Log LogType
    {
        get => (Log) Enum.Parse( typeof(Log), Type);
    }
    public string Message;
}
