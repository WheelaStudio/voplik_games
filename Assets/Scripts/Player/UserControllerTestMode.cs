using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControllerTestMode : UserController
{
    [SerializeField] private bool testMode;

    private void Awake()
    {
        if (!testMode)
        {
            Destroy(gameObject);
        }
    }
}
