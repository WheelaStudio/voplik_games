using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public static MenuUI instance;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI nickname;
    private UserController userController;
    private User user;

    private void Awake()
    {
        instance = this;
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
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
