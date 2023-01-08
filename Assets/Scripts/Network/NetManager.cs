using Mirror;
using TMPro;
using UnityEngine;

public class NetManager : NetworkManager
{
    private UserController controller;
    public TextMeshPro nickname;
    private Map map;

    public override void Start()
    {
        base.Start();
        controller = UserController.Shared;
        map = Map.instance;
    }

    private float RandomCoordinate(float max)
    {
        return Random.Range(0, max);
    }

    private void OnCreateCharacter(NetworkConnectionToClient conn, UserMessage message)
    {

        var position = new Vector2(
            RandomCoordinate(map.mapSize.x), RandomCoordinate(map.mapSize.y));
        var go = Instantiate(playerPrefab, map.transform);
        go.transform.localPosition = position;
        go.transform.parent = null;
        var user = new User(message);
        var tank = go.GetComponent<TankController>();
        tank.thisNick.text = user.UserName;
        
        NetworkServer.AddPlayerForConnection(conn, go);
        tank.SetUser(user);
        print(user.UserName);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<UserMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ActivatePLayerSpawn();

    }



    private void ActivatePLayerSpawn()
    {
        var user = controller.User;
        var message = new UserMessage(user);
        NetworkClient.Send(message);
    } 
}

public struct UserMessage: NetworkMessage
{
    public string Name;
    public int Coins;
    public int Id;
    public UserMessage(User u)
    {
        Name = u.UserName; 
        Coins = u.Coins;
        Id = u.Id;
    }
}
