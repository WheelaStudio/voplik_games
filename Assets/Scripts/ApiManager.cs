using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    [SerializeField] private string host;

    [SerializeField] private TMP_InputField registerUsername;
    [SerializeField] private TMP_InputField registerPassword;
    [SerializeField] private TMP_InputField registerPasswordRepeat;

    [SerializeField] private TMP_InputField loginUsername;
    [SerializeField] private TMP_InputField loginPassword;

    public UnityEvent<User> LoggedIn = new UnityEvent<User>();
    public UnityEvent<string> LoginError = new UnityEvent<string>();
    public UnityEvent<string> RegisterMessage = new UnityEvent<string>();
    public UnityEvent<SendTransactionMessage> SendCoinsMessage = new UnityEvent<SendTransactionMessage>();
    public UnityEvent<TransactionHistory> GetTransactions = new UnityEvent<TransactionHistory>();
    public UnityEvent<int> GetActivePlayersSuccess = new UnityEvent<int>();
    public static ApiManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private bool IsValidPassword(string p)
    {
        string error = "";
        if(p != registerPasswordRepeat.text)
            error = "Пароли не совпадают.";

        RegisterMessage?.Invoke(error);
        return p == registerPasswordRepeat.text; 
    }

    private bool IsValidNickName(string n)
    {
        string error = "";
        if(n.Length > 6)
        {
            error = "Максимум 6 знаков.";
        }

        RegisterMessage?.Invoke(error);

        return n.Length <= 6 && !string.IsNullOrEmpty(n);
    }

    public void Register()
    {
        StartCoroutine(RegisterI(registerUsername.text, registerPassword.text));

    }

    public void Login()
    {
        StartCoroutine(LoginI(loginUsername.text, loginPassword.text));
    }

    public void Login(string username, string password)
    {
        StartCoroutine(LoginI(username, password));
    }

    public void AddCoins(int coins, int id)
    {
        StartCoroutine(IAddCoins(coins, id));
    }

    public void SendCoins(string from, string to, int coins)
    {
        StartCoroutine(ISendCoins(from, to, coins));
    }

    public void GetHistory(int min, int max, string username)
    {
        StartCoroutine(IGetHistory(min, max, username));
    }

    public void GetActivePlayers()
    {
        StartCoroutine(IGetActivePlayers());
    }

    public void SetActivePlayers(int count)
    {
        StartCoroutine(ISetActivePlayers(count));
    }

    private IEnumerator IGetActivePlayers()
    {
        var www = UnityWebRequest.Get($"{host}/getactiveplayers.php");
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success) print(www.result);
        else
        {
            print(www.downloadHandler.text);
            GetActivePlayersSuccess?.Invoke(Convert.ToInt32(www.downloadHandler.text));
        }
    }

    private IEnumerator ISetActivePlayers(int count)
    {
        var form = new WWWForm();
        form.AddField("count", count);

        var www = UnityWebRequest.Post($"{host}/setactiveplayers.php", form);
        yield return www.SendWebRequest();
        if(www.result != UnityWebRequest.Result.Success) print (www.result);
        else
        {
            print(www.downloadHandler.text);
        }
    }

    private IEnumerator IGetHistory(int min, int max, string userName)
    {
        var form = new WWWForm();
        form.AddField("max", max);
        form.AddField("min", min);
        form.AddField("name", userName);

        var www = UnityWebRequest.Post($"{host}/transactionhistory.php", form);
        yield return www.SendWebRequest();
        if(www.result != UnityWebRequest.Result.Success) print(www.result);
        else
        {
            try
            {
                var json = $"{"{"}{"\"TransactionMessages\""}:{www.downloadHandler.text} {"}"} ";
                print(json);
                GetTransactions?.Invoke(JsonUtility.FromJson<TransactionHistory>(json));
            }
            catch (Exception e) 
            {
                print(e);
                print(www.downloadHandler.text);
            }
        }
    }

    private IEnumerator ISendCoins(string from, string to, int coins)
    {
        var form = new WWWForm();
        form.AddField("coins", coins);
        form.AddField("s_name", from);
        form.AddField("r_name", to);

        var www = UnityWebRequest.Post($"{host}/sendmoney.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) print(www.result);
        else
        {
            try
            {
                print(www.downloadHandler.text);
                SendCoinsMessage?.Invoke(JsonUtility.FromJson<SendTransactionMessage>(www.downloadHandler.text));
            }
            catch
            {
                print(www.downloadHandler.text);
            }
        }
    }

    public void WithdrawCoins(int curCoins, int withdraw, int id)
    {
        StartCoroutine(IAddCoins(curCoins - withdraw, id));
    }



    private IEnumerator IAddCoins(int coins, int id)
    {
        var form = new WWWForm();
        form.AddField("coins", coins);
        form.AddField("id", id);

        var www = UnityWebRequest.Post($"{host}/addcoins.php", form);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success) print (www.result);
        else
        {
            print(www.downloadHandler.text);
        }
    }

    private IEnumerator LoginI(string username, string password)
    {
        var form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post($"{host}/login.php", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) print(www.result);
            else
            {
                try
                {
                    LoggedIn?.Invoke(JsonUtility.FromJson<User>(www.downloadHandler.text));
                    PlayerPrefs.SetString(PlayerPrefsNames.USER_LOGIN, username);
                    PlayerPrefs.SetString(PlayerPrefsNames.USER_PASSWORD, password);

                }
                catch (Exception e)
                {
                    print(www.downloadHandler.text);
                    LoginError?.Invoke(www.downloadHandler.text);
                    print(e.ToString());
                }
            };
        }



    }

    private IEnumerator RegisterI(string username, string password)
    {
        if (IsValidPassword(password) && IsValidNickName(username))
        {
            var form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);

            var www = UnityWebRequest.Post($"{host}/register.php", form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                throw new Exception(www.downloadHandler.text);
            }
            RegisterMessage?.Invoke(www.downloadHandler.text);

        }

    }


    private string GetResult(UnityWebRequest w)
    {
        return w.downloadHandler.text;
    }

}
