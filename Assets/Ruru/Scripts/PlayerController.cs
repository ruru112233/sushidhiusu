using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;

    // 弾のプレハブ
    [SerializeField] GameObject bulletPrefab, ikuraBulletPrefab;

    // 弾の発射位置
    [SerializeField] GameObject firePoint;

    // 弾の発射間隔
    float shotTime = 0;
    float shotDistanceTime = 0.3f;

    // オブジェクトプール対応
    [SerializeField] Transform normalBulletPool, ikuraBulletPool;

    // プレイヤーのスプライト
    SpriteRenderer spriteRenderer;

    // ゲームオーバー
    bool gameOverFlag = false;

    // スプライト
    [SerializeField] Sprite maguroSprite, ikuraSprite, salmonSprite, tamagoSprite, ebiSprite, ikaSprite, takoSprite,
                            hotateSprite, uniSprite, taiSprite, kyuuriSprite, nattoSprite, syariSprite;

    // Start is called before the first frame update
    void Start()
    {
        shotTime = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            if (shotTime == 0)
            {
                ShotBullet();
            }

            shotTime += Time.deltaTime;

            if (shotTime > shotDistanceTime)
            {
                shotTime = 0;
            }
        }

        // ゲームオーバーになった時の処理
        if (gameOverFlag)
        {
            Debug.Log("GameOver");
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    // キャラクターの移動
    void PlayerMove()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    // 弾の発射
    void ShotBullet()
    {
        //Instantiate(bulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);

        if (spriteRenderer.sprite.name == ikuraSprite.name)
        {
            GetIkuraBulletObj(ikuraBulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);
        }
        else if (spriteRenderer.sprite.name == hotateSprite.name)
        {
            HotateShot();
        }
        else
        {
            GetNomalBulletObj(bulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);
        }
    }

    // シャリ時の攻撃
    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in normalBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.GetComponent<BulletController>().BulletAtPoint = 1;
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                return;
            }
        }

        Instantiate(bulletPrefab, pos, qua, normalBulletPool);
    }

    // いくら軍艦時の攻撃
    void GetIkuraBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in ikuraBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.GetComponent<BulletController>().BulletAtPoint = 3;
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                return;
            }
        }

        Instantiate(bulletPrefab, pos, qua, ikuraBulletPool);
    }

    // ホタテの攻撃
    void HotateShot()
    {
        HotateBulletObj(bulletPrefab, 
                        new Vector3(firePoint.transform.position.x, 
                                    firePoint.transform.position.y, 
                                    firePoint.transform.position.z), 
                        Quaternion.identity, 
                        new Vector3(1,1,0));
        HotateBulletObj(bulletPrefab,
                        new Vector3(firePoint.transform.position.x,
                                    firePoint.transform.position.y,
                                    firePoint.transform.position.z),
                        Quaternion.identity,
                        new Vector3(1.3f, 0, 0));
        HotateBulletObj(bulletPrefab,
                        new Vector3(firePoint.transform.position.x,
                                    firePoint.transform.position.y,
                                    firePoint.transform.position.z),
                        Quaternion.identity,
                        new Vector3(1, -1, 0));
    }

    void HotateBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua, Vector3 shotPos)
    {

        foreach (Transform t in normalBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.GetComponent<BulletController>().BulletAtPoint = 1;
                t.gameObject.GetComponent<BulletController>().Speed = 0;
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(shotPos * 200);
                return;
            }
        }

        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, normalBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(shotPos * 200);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (spriteRenderer.sprite.name == "Syari_01")
            {

                gameOverFlag = true;
            }
            else
            {
                spriteRenderer.sprite = syariSprite;
            }
        }

        if (collision.gameObject.CompareTag("Gate") || collision.gameObject.CompareTag("Iwa"))
        {
            gameOverFlag = true;
        }

        ChengeSprite(collision.gameObject);
    }

    // Sprite変更
    void ChengeSprite(GameObject obj)
    {
        
        switch (obj.tag)
        {
            case "Maguro":
                spriteRenderer.sprite = maguroSprite;
                break;
            case "Ikura":
                spriteRenderer.sprite = ikuraSprite;
                break;
            case "Tamago":
                spriteRenderer.sprite = tamagoSprite;
                break;
            case "Salmon":
                spriteRenderer.sprite = salmonSprite;
                break;
            case "Ebi":
                spriteRenderer.sprite = ebiSprite;
                break;
            case "Ika":
                spriteRenderer.sprite = ikaSprite;
                break;
            case "Tako":
                spriteRenderer.sprite = takoSprite;
                break;
            case "Hotate":
                spriteRenderer.sprite = hotateSprite;
                break;
            case "Uni":
                spriteRenderer.sprite = uniSprite;
                break;
            case "Tai":
                spriteRenderer.sprite = taiSprite;
                break;
            case "Kyuuri":
                spriteRenderer.sprite = kyuuriSprite;
                break;
            case "Natto":
                spriteRenderer.sprite = nattoSprite;
                break;
            case "Syouyusashi":
                GameManager.instance.syouyusashiCtr.getSyouyuPoint++;
                break;


        }

    }
}
