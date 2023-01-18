using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckServerBuild : MonoBehaviour
{
    public static bool ServerBuild()
    {
        if (Application.platform == RuntimePlatform.LinuxServer ||
            Application.platform == RuntimePlatform.OSXServer || 
            Application.platform == RuntimePlatform.WindowsServer)
        {
            return true;
        }
        return false;
    }

    private void Awake()
    {
        if(ServerBuild())
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            ApiManager.instance.GetActivePlayers();
        }
    }
}
