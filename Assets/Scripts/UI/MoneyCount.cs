using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MoneyCount : MonoBehaviour
{
    private TextMeshProUGUI text;
    public static MoneyCount Shared;

    private void Awake()
    {
        Shared = this;
    }

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeText(int coins)
    {
        text.text = coins.ToString();
    }

}
