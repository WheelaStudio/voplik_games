using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    [SerializeField] private string host;
    [SerializeField] private string mySqlUser;
    [SerializeField] private string mySqlPassword;

    [SerializeField] private TMP_InputField registerUsername;
    [SerializeField] private TMP_InputField registerPassword;
    [SerializeField] private TMP_InputField registerPasswordRepeat;

    [SerializeField] private TMP_InputField loginUsername;
    [SerializeField] private TMP_InputField loginPassword;

    public UnityEvent<User> LoggedIn = new UnityEvent<User>();
    public UnityEvent<string> LoginError = new UnityEvent<string>();
    public static ApiManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private bool IsValidPassword(string p)
    {
        if (p.Length < 6)
            print("6 or more symbols");
        else if(p != registerPasswordRepeat.text)
            print("Passwords don't match");

        return p.Length >= 6 && p == registerPasswordRepeat.text; 
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

        var www = UnityWebRequest.Post($"{host}/login.php", form);
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
                print(e.ToString());
            }
        };

    }

    private IEnumerator RegisterI(string username, string password)
    {
        if (IsValidPassword(password))
        {
            var form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);

            var www = UnityWebRequest.Post($"{host}/register.php", form);
            yield return www.SendWebRequest();
            GetResult(www);

        }
    }



    private void GetResult(UnityWebRequest w)
    {
        if (w.result != UnityWebRequest.Result.Success) throw new Exception(w.downloadHandler.text);
        else print(w.downloadHandler.text);

    }

}
