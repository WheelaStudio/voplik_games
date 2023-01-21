using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageObject : MonoBehaviour
{
    [SerializeField] private Message message;
    [SerializeField] private float lifeTime;
    public void Setup(Message msg)
    {
        message = msg;
        var text = GetComponent<TextMeshProUGUI>();
        text.text = message.MessageText;
        text.color = message.Type.Color;
        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
