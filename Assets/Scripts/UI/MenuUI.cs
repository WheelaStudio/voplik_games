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
    [SerializeField] private MenuMessages messages;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        messages = MenuMessages.instance;
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
            var msg = new Message("Сервер перегружен, зайдите позже.", MessageTypes.Error);
            messages.CreateMessage(msg);

        }
        playerCountText.text = $"В игре: {playerCount}";
    }

    public void Play()
    {
        if (user.Coins < 10)
        {
            var msg = new Message("Недостаточно монет.", MessageTypes.Error);
            MenuMessages.instance.CreateMessage(msg);
        }
        if (playerCount < 50 && user.Coins >= 10)
        {
            UserController.Shared.AddCoins(-10);
            SceneManager.LoadScene(1);
        }

    }
}
