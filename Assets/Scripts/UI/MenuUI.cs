using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI nickname;
    private UserController userController;
    private User user;

    private void OnEnable()
    {
        userController = UserController.Shared;
        print(userController.User.Coins);
        user = userController.User;
        coins.text = $"Монеты\n {user.Coins}";
        nickname.text = user.UserName;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
