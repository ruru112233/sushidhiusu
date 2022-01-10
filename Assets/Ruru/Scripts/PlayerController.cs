using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;

    // 弾のプレハブ
    [SerializeField] GameObject bulletPrefab;

    // 弾の発射位置
    [SerializeField] GameObject firePoint;

    // 弾の発射間隔
    float shotTime = 0;
    float shotDistanceTime = 0.3f;

    // オブジェクトプール対応
    [SerializeField] Transform normalBulletPool;

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

        GetNomalBulletObj(bulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);
    }

    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in normalBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                return;
            }
        }

        Instantiate(bulletPrefab, pos, qua, normalBulletPool);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (spriteRenderer.sprite.name == "Syari_01")
            {
                Debug.Log("GameOver");
            }
            else
            {
                spriteRenderer.sprite = syariSprite;
            }
        }

        if (collision.gameObject.CompareTag("Gate"))
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

        }

    }
}
