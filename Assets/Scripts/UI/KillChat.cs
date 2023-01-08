using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class KillChat : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject msgPrefab;
    [SerializeField] private List<TextMeshProUGUI> messages;

    public static KillChat instance;

    private void Awake()
    {
        instance = this;
    }

    public void CreateKillMsg(string msg)
    {
        if (messages.Count >= 10)
        {
            Destroy(messages[0].gameObject);
            messages.Remove(messages[0]);
        }
        var newMsg = Instantiate(msgPrefab, content);
        var msgText = newMsg.GetComponent<TextMeshProUGUI>();
        msgText.text = msg;
        messages.Add(msgText);
    }
}
