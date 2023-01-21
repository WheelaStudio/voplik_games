using TMPro;
using UnityEditor;
using UnityEngine;

public class MenuMessages : MonoBehaviour
{
    [SerializeField] private GameObject messages;
    [SerializeField] private GameObject messagePrefab;
    public static MenuMessages instance;

    private void Awake()
    {
        instance = this;
    }

    public void CreateMessage(Message message)
    {
        var msg = Instantiate(messagePrefab, messages.transform);
        msg.GetComponent<MessageObject>().Setup(message);
    }
}

public struct MessageTypes
{
    public static MessageType Message = new MessageType("Message", Color.white);
    public static MessageType Error = new MessageType("Error", Color.red);
    public static MessageType Warning = new MessageType("Warning", Color.yellow);
}

public struct MessageType
{
    public Color Color;
    public string Type;

    public MessageType(string type, Color color)
    {
        Color = color;
        Type = type;
    }
}

public class Message
{
    public MessageType Type;
    public string MessageText;

    public Message(string message)
    {
        this.MessageText = message;
        Type = MessageTypes.Message;
    }

    public Message(string message, MessageType type)
    {
        MessageText = message;
        Type = type;
    }
}
