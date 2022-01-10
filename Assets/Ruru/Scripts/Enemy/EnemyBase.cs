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

    // �h���b�v�A�C�e���i�[
    [SerializeField] List<GameObject> itemPrefab;
    [SerializeField] Transform dropItemPool;

    // �G�̒e
    [SerializeField] GameObject bulletPrefab;

    // �e�̔��ˈʒu
    GameObject firePoint;

    // �I�u�W�F�N�g�v�[���Ή�
    [SerializeField] Transform enemyBulletPool;

    // �e�̑��x
    [SerializeField] float shotSpeed;

    // �e�̔��ˊԊu
    float shotCurrentTime = 0;
    [SerializeField] float shotTime;

    // �v���C���[�̈ʒu�擾
    private Transform playerPos;

    // Start is called before the first frame update
    public virtual void Start()
    {
        isGate = false;
        EnemyHp = initHp;

        firePoint = this.gameObject;

        dropItemPool = GameObject.FindWithTag("DropItemPool").transform;
    }

    // Update is called once per frame
    public virtual void Update()
    {

        // �G�̔j��
        if (EnemyHp <= 0)
        {
            // �A�C�e�����Z�b�g����Ă���A�h���b�v����
            if (itemPrefab.Count != 0)
            {
                Instantiate(itemPrefab[RandomItemNo()], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity, dropItemPool);
            }
            gameObject.SetActive(false);
        }

        if (isGate)
        {
            // �e�̔���
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

    // �A�C�e�����X�g���烉���_���Ŕz��ԍ���ݒ�
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

    // �e�̔���
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
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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
