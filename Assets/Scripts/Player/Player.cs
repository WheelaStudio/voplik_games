using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{

    [SerializeField] private UserController userController;
    public User User { get => user; }

    [SyncVar(hook = nameof(SyncUserData))]
    private User syncUser;

    [SerializeField] private User user;
    public TextMeshPro nickname;

    public Transform thisTransform;

    public TextMeshPro thisNick;

    public KillChat killChat;

    private Transform mainCameraTransform;
    [SerializeField] private int reward;

    [SerializeField] private int coins;
    [SyncVar(hook = nameof(SyncCoins))] private int syncCoins;
    public int Coins { get => coins; }



    private void SyncCoins(int oldValue, int newValue)
    {
        coins = newValue;
    }

    [Server]
    public void ChangeCoinsValue(int newValue)
    {
        syncCoins = newValue;
    }

    public void AddCoins()
    {
        coins += reward;
        ChangeCoinsValue(coins);
        print("coins added");
        RpcChangeText();
        print(coins);
    }

    [ClientRpc]
    private void RpcChangeText()
    {
        if(isLocalPlayer) 
        {
            MoneyCount.Shared.ChangeText(coins);
            print("Text Chanaged");
        }
    }

    private void SyncUserData(User oldValue, User newValue)
    {
        user = newValue;
    }

    [Server]
    private void ChangeUserData(User newValue)
    {
        syncUser = newValue;
    }

    [Command(requiresAuthority = false)]
    private void CmdChangeUserData(User newValue)
    {
        ChangeUserData(newValue);
    }

    public void SetUser(User u)
    {
        gameObject.name = u.UserName;

        if (isServer)
        {
            ChangeUserData(u);
            user = u;
        }
        else
        {
            CmdChangeUserData(u);        
        }
    }

    public IEnumerator SetText()
    {
        yield return new WaitUntil(() => !String.IsNullOrEmpty(user.UserName));
        thisNick.text = user.UserName;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        }
        
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            userController = UserController.Shared;
        }
    }

    public void MoveNickName(Transform t)
    {
        var position = new Vector3(
            thisTransform.position.x, thisTransform.position.y + 1.3f, 0);
        t.position = position;
        t.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnDestroy()
    {
        if (isLocalPlayer)
        {
            print("Killed");
            SceneManager.LoadScene(0);
            NetworkClient.Disconnect();
            UserController.Shared.AddCoins(Coins);
        }
    }
    public void Kill()
    {
        NetworkServer.Destroy(gameObject);
    }

}
