using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WelcomeScreenUI : MonoBehaviour
{
    [SerializeField] private Panel[] panels;
    [SerializeField] private GameObject menu;
    private bool userAuth = false;
    public static WelcomeScreenUI Shared;
    private ApiManager api;

    private void Awake()
    {
        Shared = this;
    }

    private void Start()
    {
        userAuth = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsNames.USER_AUTH));
        api = ApiManager.instance;
        if (userAuth)
        {
            var login = PlayerPrefs.GetString(PlayerPrefsNames.USER_LOGIN);
            var password = PlayerPrefs.GetString(PlayerPrefsNames.USER_PASSWORD);
            api.Login(login, password);
        }
    }

    public void OpenPanel(int p)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            if (p == i)
            {
                panels[i].OpenPanel();
            }
            else
            {
                panels[i].ClosePanel();
            }
        }
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        gameObject.SetActive(true);
    }
}

[Serializable]
public class Panel
{
    public Button panelBtn;
    public GameObject panelContent;

    public void OpenPanel()
    {
        panelBtn.GetComponent<Image>().color = UIColors.ActiveBtn;
        panelBtn.GetComponentInChildren<TextMeshProUGUI>().color = UIColors.DisableBtn;
        panelContent.SetActive(true);
    }

    public void ClosePanel()
    {
        panelBtn.GetComponent<Image>().color = UIColors.DisableBtn;
        panelBtn.GetComponentInChildren<TextMeshProUGUI>().color = UIColors.ActiveBtn;
        panelContent.SetActive(false);
    }

}

public struct UIColors
{
    public static Color ActiveBtn = Color.white;
    public static Color DisableBtn = new Color(0.2f, 0.2f, 0.2f);
}
