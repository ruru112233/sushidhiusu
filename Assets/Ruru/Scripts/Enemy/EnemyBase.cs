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

    public EnemyParm enemyParm;

    protected bool isGate = false;

    bool destroyFlag = false;

    public enum FishType
    {
        Maguro,
        Ikura,
        Tamago,
        Salmon,
        Ebi,
        Ika,
        Tako,
        Hotate,
        Uni,
        Tai,
        Kyuuri,
        Natto,
        Boss,
    }

    public enum FishDrop
    {
        None,
        Drop,
    }

    public FishType fishtype;
    public FishDrop fishDrop;

    
    // 弾の発射位置
    protected GameObject firePoint;


    // 弾の発射間隔
    protected float shotCurrentTime = 0;

    // プレイヤーの位置取得
    private Transform playerPos;

    Animator anime;

    bool dropFlag = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyParm = GameObject.FindWithTag("EnemyParm").GetComponent<EnemyParm>();

        anime = GetComponent<Animator>();

        isGate = false;
        EnemyHp = GetEnemyHp(fishtype);

        firePoint = this.gameObject;

        if (fishtype == FishType.Ika)
        {
            isGate = true;
        }

    }

    // Update is called once per frame
    public virtual void Update()
    {

        // 敵の破壊
        if (EnemyHp <= 0)
        {
            if (!destroyFlag)
            {
                AudioManager.instance.PlaySE(2);
                destroyFlag = true;
            }

            if (fishDrop == FishDrop.Drop)
            {
                // アイテムがセットされてたら、ドロップする
                if (GetDropItemList(fishtype).Count != 0 && !dropFlag)
                {
                    dropFlag = true;
                    Instantiate(GetDropItemList(fishtype)[RandomItemNo()], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity, enemyParm.dropItemPool);
                }
            }

            if (fishtype == FishType.Ika)
            {
                EnemyHp = GetEnemyHp(fishtype);
            }

            anime.SetTrigger("Destroy");

            StartCoroutine(EnemyDestroy());
        }

        if (isGate)
        {
            if (fishtype == FishType.Boss)
            {
                StartCoroutine(StopBack());
            }
            else if (fishtype == FishType.Ika)
            {
                if (shotCurrentTime == 0)
                { 
                    SumiShot(enemyParm.sumiBullet, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);
                }
            }
            else
            {
                // 弾の発射
                if (shotCurrentTime == 0)
                {
                    ShotBullet();
                }
            }

            shotCurrentTime += Time.deltaTime;

            if (shotCurrentTime >= enemyParm.shotTime)
            {
                shotCurrentTime = 0;
            }
        }

    }

    IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);

    }

    IEnumerator StopBack()
    {
        GameObject[] backGrounds = GameObject.FindGameObjectsWithTag("BackGround");
        
        yield return new WaitForSeconds(2);

        foreach (GameObject back in backGrounds)
        {
            BackGroundMove b = back.GetComponent<BackGroundMove>();
            b.speed = 0;
        }
    }

    // アイテムリストからランダムで配列番号を設定
    int RandomItemNo()
    {
        int randNo = Random.Range(0, GetDropItemList(fishtype).Count);

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

            if (EnemyHp > 0)
            {
                AudioManager.instance.PlaySE(3);
            }
        }
    }

    // 弾の発射
    void ShotBullet()
    {
        if (fishtype != FishType.Hotate)
        {
            GetNomalBulletObj(enemyParm.bulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);
        }
        else
        {
            HotateShot1(enemyParm.bulletPrefab, 
                        new Vector3(firePoint.transform.position.x, 
                                    firePoint.transform.position.y, 
                                    firePoint.transform.position.z), 
                        Quaternion.identity);
            HotateShot2(enemyParm.bulletPrefab, 
                        new Vector3(firePoint.transform.position.x, 
                                    firePoint.transform.position.y, 
                                    firePoint.transform.position.z), 
                        Quaternion.identity);
            HotateShot3(enemyParm.bulletPrefab, 
                        new Vector3(firePoint.transform.position.x, 
                                    firePoint.transform.position.y, 
                                    firePoint.transform.position.z),
                        Quaternion.identity);
        }
    }

    // スミ攻撃
    void SumiShot(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        if (GameObject.FindWithTag("Player"))
        {
            playerPos = GameObject.FindWithTag("Player").transform;
            float dis = Vector3.Distance(playerPos.position, pos);
            Vector2 vec = playerPos.position - pos;


            foreach (Transform t in enemyParm.sumiBulletPool)
            {
                // 弾が非アクティブなら使いまわし
                if (!t.gameObject.activeSelf)
                {
                    t.SetPositionAndRotation(pos, qua);
                    t.gameObject.SetActive(true);
                    GameObject bullet = t.gameObject;
                    Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                    bulletRigidbody.AddForce(new Vector3(vec.x * enemyParm.shotSpeed, vec.y * enemyParm.shotSpeed, 0));
                    return;
                }
            }
            GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, enemyParm.sumiBulletPool);
            Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
            bulletBody.AddForce(new Vector3(vec.x * enemyParm.shotSpeed, vec.y * enemyParm.shotSpeed, 0));

        }

    }

    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        
        if (GameObject.FindWithTag("Player"))
        {
            playerPos = GameObject.FindWithTag("Player").transform;
            float dis = Vector3.Distance(playerPos.position, pos);
            Vector2 vec = playerPos.position - pos;

            foreach (Transform t in enemyParm.enemyBulletPool)
            {
                // 弾が非アクティブなら使いまわし
                if (!t.gameObject.activeSelf)
                {
                    t.SetPositionAndRotation(pos, qua);
                    t.gameObject.SetActive(true);
                    GameObject bullet = t.gameObject;
                    Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                    bulletRigidbody.AddForce(new Vector3(vec.x * enemyParm.shotSpeed, vec.y * enemyParm.shotSpeed, 0));

                    return;
                }
            }

            GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, enemyParm.enemyBulletPool);
            Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
            bulletBody.AddForce(new Vector3(vec.x * enemyParm.shotSpeed, vec.y * enemyParm.shotSpeed, 0));

        }
    }

    // ホタテの攻撃
    void HotateShot1(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in enemyParm.hotateBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(transform.up * enemyParm.shotSpeed * 6);
                return;
            }
        }
        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, enemyParm.hotateBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(transform.up * enemyParm.shotSpeed * 6);

    }

    void HotateShot2(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in enemyParm.hotateBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(new Vector3(1, 1, 0) * enemyParm.shotSpeed * 6);
                return;
            }
        }
        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, enemyParm.hotateBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(new Vector3(1, 1, 0) * enemyParm.shotSpeed * 6);

    }

    void HotateShot3(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in enemyParm.hotateBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(new Vector3(-1, 1, 0) * enemyParm.shotSpeed * 6);
                return;
            }
        }
        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, enemyParm.hotateBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(new Vector3(-1, 1, 0) * enemyParm.shotSpeed * 6);

    }

    // itemListを返す
    int GetEnemyHp(FishType type)
    {
        int hp = 1;

        switch (type)
        {
            case FishType.Maguro:
                hp = enemyParm.maguroInitHp;
                break;
            case FishType.Ikura:
                hp = enemyParm.ikuraInitHp;
                break;
            case FishType.Tamago:
                hp = enemyParm.tamagoInitHp;
                break;
            case FishType.Salmon:
                hp = enemyParm.salmonInitHp;
                break;
            case FishType.Ebi:
                hp = enemyParm.ebiInitHp;
                break;
            case FishType.Ika:
                hp = enemyParm.ikaInitHp;
                break;
            case FishType.Tako:
                hp = enemyParm.takoInitHp;
                break;
            case FishType.Hotate:
                hp = enemyParm.hotateInitHp;
                break;
            case FishType.Uni:
                hp = enemyParm.uniInitHp;
                break;
            case FishType.Tai:
                hp = enemyParm.taiInitHp;
                break;
            case FishType.Kyuuri:
                hp = enemyParm.kyuuriInitHp;
                break;
            case FishType.Natto:
                hp = enemyParm.nattoInitHp;
                break;
            case FishType.Boss:
                hp = enemyParm.bossInitHp;
                break;

        }

        return hp;
    }

    // itemListを返す
    List<GameObject> GetDropItemList(FishType type)
    {
        List<GameObject> objList = new List<GameObject>();

        switch (type)
        {
            case FishType.Maguro:
                objList = enemyParm.maguroItemPrefab;
                break;
            case FishType.Ikura:
                objList = enemyParm.ikuraItemPrefab;
                break;
            case FishType.Tamago:
                objList = enemyParm.tamagoItemPrefab;
                break;
            case FishType.Salmon:
                objList = enemyParm.salmonItemPrefab;
                break;
            case FishType.Ebi:
                objList = enemyParm.ebiItemPrefab;
                break;
            case FishType.Ika:
                objList = enemyParm.ikaItemPrefab;
                break;
            case FishType.Tako:
                objList = enemyParm.takoItemPrefab;
                break;
            case FishType.Hotate:
                objList = enemyParm.hotateItemPrefab;
                break;
            case FishType.Uni:
                objList = enemyParm.uniItemPrefab;
                break;
            case FishType.Tai:
                objList = enemyParm.taiItemPrefab;
                break;
            case FishType.Kyuuri:
                objList = enemyParm.kyuuriItemPrefab;
                break;
            case FishType.Natto:
                objList = enemyParm.nattoItemPrefab;
                break;

        }

        return objList;
    }

    
}
