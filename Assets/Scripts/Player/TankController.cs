using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class TankController : Player
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float maxBulletDistance;
    [SerializeField] private float fireRate;
    [SerializeField] private float noKillTime;
    [SerializeField] private bool canShoot = true;
    private Animator animator;
    private Rigidbody2D rb;
    private Map map;
    private Vector2 mapGlobalSize;


    private void Start()
    {
        StartCoroutine(ChangeKill());
        if (isServer)
        {
            ApiManager.instance.SetActivePlayers(NetworkServer.connections.Count);
        }
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
        animator = GetComponent<Animator>();
    }

    private IEnumerator ChangeKill()
    {
        yield return new WaitForSeconds(noKillTime);
        CanBeKilled = true;
        GetComponent<SpriteRenderer>().color = Color.white;
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
        activeBullets.Add(clone);
        NetworkServer.Spawn(clone);
    }


    public void HandleShoot()
    {
        if (canShoot && CanBeKilled)
        {
            StartCoroutine(ShootI());
        }
    }

    private IEnumerator ShootI()
    {
        canShoot = false;
        if (!isLocalPlayer)
        {
            CmdSpawnBullet();
        }
        else
        {
            SpawnBullet();
        }
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    public void HandleMove(Vector2 dir)
    {
        
        animator.SetBool("move", dir != Vector2.zero );

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
            activeBullets.Remove(t.gameObject);
            Destroy(t.gameObject);
        }
    }

}
