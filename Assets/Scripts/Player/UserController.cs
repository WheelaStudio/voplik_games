using System;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField] private User user;
    public User User
    {
        get
        {
            if(user == null)
            {
                throw new Exception("User not logged in");
            }
            else
            {
                return user;
            }
        }
    }

    public static UserController Shared;
    [SerializeField] private ApiManager apiManager;
    public bool Admin { get => admin; }
    [SerializeField] private bool admin; 

    private void Awake()
    {
        Shared = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddCoins(int c)
    {
        var count = user.Coins + c;
        User.Coins += c;
        apiManager.AddCoins(count, user.Id);
    }

    public void AuthUser(User u)
    {
        user = u;
        if (u.UserName != string.Empty)
        {
            print("User logged in");
            PlayerPrefs.SetInt(PlayerPrefsNames.USER_AUTH, 1);
        }
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey(PlayerPrefsNames.USER_AUTH);
        PlayerPrefs.DeleteKey(PlayerPrefsNames.USER_LOGIN);
        PlayerPrefs.DeleteKey(PlayerPrefsNames.USER_PASSWORD);
    }

}

[Serializable]
public class User
{
    public string UserName;
    public int Coins;
    public int Id;

    public User()
    {

    }
    
    public User(string name, int coins, int id)
    {
        UserName = name; 
        Coins = coins;
        Id = id;
    }

    public User(UserMessage u)
    {
        UserName = u.Name;
        Coins= u.Coins;
        Id = u.Id;
    }
}
