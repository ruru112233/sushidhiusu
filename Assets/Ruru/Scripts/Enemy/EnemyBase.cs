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

    
    // �e�̔��ˈʒu
    protected GameObject firePoint;


    // �e�̔��ˊԊu
    protected float shotCurrentTime = 0;

    // �v���C���[�̈ʒu�擾
    private Transform playerPos;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyParm = GameObject.FindWithTag("EnemyParm").GetComponent<EnemyParm>();

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

        // �G�̔j��
        if (EnemyHp <= 0)
        {
            if (fishDrop == FishDrop.Drop)
            {
                // �A�C�e�����Z�b�g����Ă���A�h���b�v����
                if (GetDropItemList(fishtype).Count != 0)
                {
                    Instantiate(GetDropItemList(fishtype)[RandomItemNo()], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity, enemyParm.dropItemPool);
                }
            }

            if (fishtype == FishType.Ika)
            {
                EnemyHp = GetEnemyHp(fishtype);
            }

            gameObject.SetActive(false);
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
                // �e�̔���
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

    // �A�C�e�����X�g���烉���_���Ŕz��ԍ���ݒ�
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
        }
    }

    // �e�̔���
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

    // �X�~�U��
    void SumiShot(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        playerPos = GameObject.FindWithTag("Player").transform;
        float dis = Vector3.Distance(playerPos.position, pos);
        Vector2 vec = playerPos.position - pos;


        foreach (Transform t in enemyParm.sumiBulletPool)
        {
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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

    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        playerPos = GameObject.FindWithTag("Player").transform;
        float dis = Vector3.Distance(playerPos.position, pos);
        Vector2 vec = playerPos.position - pos;

        foreach (Transform t in enemyParm.enemyBulletPool)
        {
            // �e����A�N�e�B�u�Ȃ�g���܂킵
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(new Vector3(vec.x * enemyParm.shotSpeed, vec.y * enemyParm.shotSpeed, 0));
//                bulletRigidbody.AddForce(transform.up * shotSpeed);

                //t.GetComponent<Rigidbody2D>().velocity = vec;
                //t.GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, 2.0f);
                return;
            }
        }

        //Instantiate(bulletPrefab, pos, qua, enemyBulletPool);

        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, enemyParm.enemyBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(new Vector3(vec.x * enemyParm.shotSpeed, vec.y * enemyParm.shotSpeed, 0));


        //bulletBody.AddForce(transform.up * shotSpeed);

    }

    // �z�^�e�̍U��
    void HotateShot1(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in enemyParm.hotateBulletPool)
        {
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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

    // itemList��Ԃ�
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

    // itemList��Ԃ�
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
