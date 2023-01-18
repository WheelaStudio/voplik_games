using TMPro;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public static MenuUI instance;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI nickname;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI serverError;

    private UserController userController;
    private User user;

    [SerializeField] private int playerCount;

    private void Awake()
    {
        instance = this;
    }

    public void SetActivePlayers(int p)
    {
        playerCount = p;
    }

    private void OnEnable()
    {
        UpdateUserData();
    }

    public void UpdateUserData()
    {
        userController = UserController.Shared;
        user = userController.User;
        coins.text = $"Монеты\n {user.Coins}";
        nickname.text = user.UserName;
        if(playerCount >= 50)
        {
            serverError.text = "Сервер перегружен";

        }
        playerCountText.text = $"В игре: {playerCount}";
    }

    public void Play()
    {
        if (user.Coins < 10)
        {
            print("Not enough coins");
        }
        else if (playerCount < 50) { }
        {
            UserController.Shared.AddCoins(-10);
            SceneManager.LoadScene(1);
        }

    }
}
