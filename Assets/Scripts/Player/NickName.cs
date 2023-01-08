using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NickName : MonoBehaviour
{
    public UnityEvent<Transform> Move = new UnityEvent<Transform>();
    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Update()
    {
        Move?.Invoke(thisTransform);
    }
}
