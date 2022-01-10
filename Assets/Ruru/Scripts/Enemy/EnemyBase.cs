using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private int enemyHp = 1;

    public int EnemyHp
    {
        get { return enemyHp; }
        set { enemyHp = value; }
    }

    public int initHp = 1;

    protected bool isGate = false;

    // ドロップアイテム格納
    [SerializeField] List<GameObject> itemPrefab;

    // 敵の弾
    [SerializeField] GameObject bulletPrefab;

    // 弾の発射位置
    GameObject firePoint;

    // オブジェクトプール対応
    [SerializeField] Transform enemyBulletPool;

    // 弾の速度
    [SerializeField] float shotSpeed;

    // 弾の発射間隔
    float shotCurrentTime = 0;
    [SerializeField] float shotTime;

    // プレイヤーの位置取得
    private Transform playerPos;

    // Start is called before the first frame update
    public virtual void Start()
    {
        isGate = false;
        EnemyHp = initHp;

        firePoint = this.gameObject;

    }

    // Update is called once per frame
    public virtual void Update()
    {

        // 敵の破壊
        if (EnemyHp <= 0)
        {
            // アイテムがセットされてたら、ドロップする
            if (itemPrefab.Count != 0)
            {
                Instantiate(itemPrefab[RandomItemNo()], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            }
            gameObject.SetActive(false);
        }

        if (isGate)
        {
            // 弾の発射
            if (shotCurrentTime == 0)
            {
                ShotBullet();
            }

            shotCurrentTime += Time.deltaTime;

            if (shotCurrentTime >= shotTime)
            {
                shotCurrentTime = 0;
            }
        }
        
    }

    // アイテムリストからランダムで配列番号を設定
    int RandomItemNo()
    {
        int randNo = Random.Range(0, itemPrefab.Count);

        return randNo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gate"))
        {
            // isGate = true;
            isGate = !isGate;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            int at = collision.gameObject.GetComponent<BulletController>().BulletAtPoint;
            Debug.Log(at);

            EnemyHp -= at;
        }
    }

    // 弾の発射
    void ShotBullet()
    {
        GetNomalBulletObj(bulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);
    }

    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        playerPos = GameObject.FindWithTag("Player").transform;
        float dis = Vector3.Distance(playerPos.position, pos);
        Debug.Log(dis);
        Vector2 vec = playerPos.position - pos;

        foreach (Transform t in enemyBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(new Vector3(vec.x * shotSpeed, vec.y * shotSpeed, 0));
//                bulletRigidbody.AddForce(transform.up * shotSpeed);

                //t.GetComponent<Rigidbody2D>().velocity = vec;
                //t.GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, 2.0f);
                return;
            }
        }

        //Instantiate(bulletPrefab, pos, qua, enemyBulletPool);

        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, enemyBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(new Vector3(vec.x * shotSpeed, vec.y * shotSpeed, 0));


        //bulletBody.AddForce(transform.up * shotSpeed);

    }

    
}
