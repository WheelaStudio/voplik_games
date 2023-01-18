using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : NetworkBehaviour
{
    public UnityEvent<Rigidbody2D, Vector3> OnMove = new UnityEvent<Rigidbody2D, Vector3>();
    public Player Player { get => player; }
    [SerializeField] private Player player;
    private Transform thisTransform;
    private Vector3 startPos;
    private Rigidbody2D thisRigidbody;
    [SerializeField] private KillChat killChat;
    [SerializeField] private bool canKill;

    private void Start()
    {
        thisTransform = transform;
        thisRigidbody = GetComponent<Rigidbody2D>();
        startPos = transform.position;

    }

    private void FixedUpdate()
    {
        OnMove?.Invoke(thisRigidbody, startPos);
    }

    private void Update()
    {
        if(Player == null && isServer)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    public void SetPlayer(Player p)
    {
        player = p;
        killChat = p.killChat;
    }


    [ClientRpc]
    private void RpcKillChat(string t)
    {
        NetworkClient.localPlayer.GetComponent<Player>().killChat.CreateKillMsg(t);
        canKill = true;
    }

    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out var p) && p != player)
        {
            if (isServer && p.CanBeKilled)
            {
                var text = $"{player.User.UserName} killed {p.User.UserName}";
                RpcKillChat(text);
                player.AddCoins();
                p.Kill();
            }
        }
    }
}


