using Mirror;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class TankController : Player
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float maxBulletDistance;
    private Rigidbody2D rb;
    private Map map;
    private Vector2 mapGlobalSize;

    private void Start()
    {
        if(isLocalPlayer)
        {
            map = Map.instance;
            mapGlobalSize = map.mapSize / 2;
            rb = GetComponent<Rigidbody2D>();
            killChat = KillChat.instance;
        }
        if (!isServer)
        {
            StartCoroutine(SetText());
        }
    }

    private void Awake()
    {
        thisTransform = transform;
    }

    [Client]
    private void SpawnBullet()
    {
        CmdSpawnBullet();
    }

    [Command]
    private void CmdSpawnBullet()
    {
        var clone = Instantiate(bullet, thisTransform.position, thisTransform.rotation);
        var controller = clone.GetComponent<BulletController>();
        controller.SetPlayer(this);
        controller.OnMove.AddListener(HandleBulletMove);
        clone.SetActive(true);
        print("Shoot");
        NetworkServer.Spawn(clone);
    }


    public void HandleShoot()
    {
        if (!isLocalPlayer)
        {
            CmdSpawnBullet();
        }
        else
        {
            SpawnBullet();
        }
    }

    public void HandleMove(Vector2 dir)
    {
        var step = Vector2.zero;
        if (dir.x != 0)
        {
            rotation.z = 90 * -dir.x;
            step = speed * Time.deltaTime * transform.up;
        }
        else if (dir.y != 0) 
        {
            if(dir.y > 0)
            {
                rotation.z = 0;
            }
            else
            {
                rotation.z = 180;
            }
            
            step = speed * Time.deltaTime * transform.up;
        }

        thisTransform.rotation = Quaternion.Euler(rotation);


        rb.MovePosition(transform.position + (Vector3)step);
        transform.position = ClampPosition(transform.position);

    }


    private Vector2 ClampPosition(Vector2 pos)
    {
        return new Vector2(Mathf.Clamp(pos.x, -mapGlobalSize.x, mapGlobalSize.x), 
            Mathf.Clamp(pos.y, -mapGlobalSize.y, mapGlobalSize.y));
    }

    public void HandleBulletMove(Rigidbody2D t, Vector3 startPos)
    {
        var step = bulletSpeed * Time.deltaTime * t.transform.up;
        t.MovePosition((Vector3)t.position + step);

        if(Vector3.Distance(startPos, t.position) >= maxBulletDistance)
        {
            Destroy(t.gameObject);
        }
    }

}