using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : NetworkBehaviour
{
    public UnityEvent OnShoot = new UnityEvent();
    public UnityEvent<float> OnHorizontalMove = new UnityEvent<float>();
    public UnityEvent<float> OnVerticalMove = new UnityEvent<float>();
    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>();

    private void Update()
    {
        if (isLocalPlayer)
        {
            GetShootingInput();
        }
    }

    private void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            GetMoveInput();
        }
    }

    private void GetShootingInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnShoot?.Invoke();
        }
    }

    private void GetMoveInput()
    {
        OnMove?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }

    private float GetAxisValueOnce(string axis)
    {
        if (Input.anyKeyDown)
        {
            return Input.GetAxisRaw(axis);  
        }
        return 0;
    }
    
}
