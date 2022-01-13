using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //kuwapon tsuika
    public GameObject gameoverText;
    public GameObject retryText;
    public GameObject returnTittleText;

    [SerializeField] float speed;

    // ?e???v???n?u
    [SerializeField] GameObject bulletPrefab, ikuraBulletPrefab, hotateBulletPrefab, maguroBulletPrefab,
                                salmonBulletPrefab, ebiBulletPrefab;

    // ?e?????????u
    [SerializeField] GameObject firePoint, firePoint2, firePoint3;

    // ?e?????????u
    float shotTime = 0;
    float shotDistanceTime = 0.3f;

    // ?I?u?W?F?N?g?v?[??????
    [SerializeField] Transform normalBulletPool, ikuraBulletPool, hotateBulletPool, maguroBulletPool,
                               salmonBulletPool, ebiBulletPool;

    // ?v???C???[???X?v???C?g
    SpriteRenderer spriteRenderer;

    // ?Q?[???I?[?o?[
    bool gameOverFlag = false;

    Animator anime;

    // ?X?v???C?g
    [SerializeField] Sprite maguroSprite, ikuraSprite, salmonSprite, tamagoSprite, tamagoNorinashiSprite, ebiSprite, ikaSprite, takoSprite,
                            hotateSprite, uniSprite, taiSprite, kyuuriSprite, nattoSprite, syariSprite;

    // Start is called before the first frame update
    void Start()
    {
        shotTime = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();

        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            if (shotTime == 0)
            {
                AudioManager.instance.PlaySE(9);
                ShotBullet();
            }

            shotTime += Time.deltaTime;

            if (shotTime > shotDistanceTime)
            {
                shotTime = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            shotTime = 0;
        }

        // ?Q?[???I?[?o?[????????????????
        if (gameOverFlag)
        {
            anime.enabled = true;
            AudioManager.instance.PlayBGM(7);
            GameOver();
            Debug.Log("GameOver");
            Death();
            //Invoke("Death",0.5f);

        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    // ?L?????N?^?[??????
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

    // ?e??????
    void ShotBullet()
    {
        //Instantiate(bulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);

        if (spriteRenderer.sprite.name == ikuraSprite.name)
        {
            // ??????
            GetIkuraBulletObj(ikuraBulletPrefab, 
                              new Vector3(firePoint.transform.position.x, 
                                          firePoint.transform.position.y, 
                                          firePoint.transform.position.z), 
                              Quaternion.identity);
        }
        else if (spriteRenderer.sprite.name == hotateSprite.name)
        {
            // ?z?^?e
            HotateShot();
        }
        else if (spriteRenderer.sprite.name == maguroSprite.name)
        {
            // ?}?O??
            MaguroBulletObj(maguroBulletPrefab, 
                            new Vector3(firePoint2.transform.position.x, 
                                        firePoint2.transform.position.y, 
                                        firePoint2.transform.position.z), 
                            Quaternion.identity);
            MaguroBulletObj(maguroBulletPrefab, 
                            new Vector3(firePoint3.transform.position.x, 
                                        firePoint3.transform.position.y, 
                                        firePoint3.transform.position.z), 
                            Quaternion.identity);

        }
        else if (spriteRenderer.sprite.name == salmonSprite.name)
        {
            // ?T?[????
            SalmonBulletObj(salmonBulletPrefab, 
                            new Vector3(transform.position.x, 
                                        transform.position.y,
                                        transform.position.z),
                            Quaternion.identity,
                            new Vector3(1, 0, 0));
            SalmonBulletObj(salmonBulletPrefab,
                            new Vector3(transform.position.x,
                                        transform.position.y,
                                        transform.position.z),
                            Quaternion.identity,
                            new Vector3(-1, 0, 0));
            SalmonBulletObj(salmonBulletPrefab,
                            new Vector3(transform.position.x,
                                        transform.position.y,
                                        transform.position.z),
                            Quaternion.Euler(0,0,90),
                            new Vector3(0, 1, 0));
            SalmonBulletObj(salmonBulletPrefab,
                            new Vector3(transform.position.x,
                                        transform.position.y,
                                        transform.position.z),
                            Quaternion.Euler(0, 0, 90),
                            new Vector3(0, -1, 0));
        }
        else if (spriteRenderer.sprite.name == ebiSprite.name)
        {
            // ?G?r
            EbiBulletObj(ebiBulletPrefab, 
                         new Vector3(firePoint.transform.position.x,
                                     firePoint.transform.position.y,
                                     firePoint.transform.position.z),
                         Quaternion.identity);
        }
        else
        {
            GetNomalBulletObj(bulletPrefab, 
                              new Vector3(firePoint.transform.position.x, 
                                          firePoint.transform.position.y, 
                                          firePoint.transform.position.z), 
                              Quaternion.identity);
        }
    }

    // ?V?????????U??
    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in normalBulletPool)
        {
            // ?e?????A?N?e?B?u?????g????????
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

    // ???????R???????U??
    void GetIkuraBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in ikuraBulletPool)
        {
            // ?e?????A?N?e?B?u?????g????????
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

    // ?z?^?e???U??
    void HotateShot()
    {
        HotateBulletObj(hotateBulletPrefab, 
                        new Vector3(firePoint.transform.position.x, 
                                    firePoint.transform.position.y, 
                                    firePoint.transform.position.z), 
                        Quaternion.identity, 
                        new Vector3(1,1,0));
        HotateBulletObj(hotateBulletPrefab,
                        new Vector3(firePoint.transform.position.x,
                                    firePoint.transform.position.y,
                                    firePoint.transform.position.z),
                        Quaternion.identity,
                        new Vector3(1.3f, 0, 0));
        HotateBulletObj(hotateBulletPrefab,
                        new Vector3(firePoint.transform.position.x,
                                    firePoint.transform.position.y,
                                    firePoint.transform.position.z),
                        Quaternion.identity,
                        new Vector3(1, -1, 0));
    }

    void HotateBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua, Vector3 shotPos)
    {

        foreach (Transform t in hotateBulletPool)
        {
            // ?e?????A?N?e?B?u?????g????????
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.GetComponent<BulletController>().BulletAtPoint = 1;
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(shotPos * 200);
                return;
            }
        }

        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, hotateBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(shotPos * 200);
    }

    // ?}?O?????U??
    void MaguroBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in maguroBulletPool)
        {
            // ?e?????A?N?e?B?u?????g????????
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.GetComponent<BulletController>().BulletAtPoint = 1;
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                return;
            }
        }

        Instantiate(bulletPrefab, pos, qua, maguroBulletPool);
    }

    // ?T?[???????U??
    void SalmonBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua, Vector3 shotPos)
    {
        foreach (Transform t in salmonBulletPool)
        {
            // ?e?????A?N?e?B?u?????g????????
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.GetComponent<BulletController>().BulletAtPoint = 1;
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(shotPos * 200);
                return;
            }
        }

        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, salmonBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(shotPos * 200);

    }

    // ?G?r???U??
    void EbiBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in ebiBulletPool)
        {
            // ?e?????A?N?e?B?u?????g????????
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.GetComponent<BulletController>().BulletAtPoint = 1;
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                return;
            }
        }

        Instantiate(bulletPrefab, pos, qua, ebiBulletPool);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            AudioManager.instance.PlaySE(6);
            if (spriteRenderer.sprite.name == "Syari_01")
            {
                gameOverFlag = true;
            }
            else if (spriteRenderer.sprite.name == tamagoSprite.name)
            {
                spriteRenderer.sprite = tamagoNorinashiSprite;
            }
            else
            {
                anime.enabled = true;
                anime.SetTrigger("Damege");
                spriteRenderer.sprite = syariSprite;
            }
        }

        if (collision.gameObject.CompareTag("Gate") || collision.gameObject.CompareTag("Iwa"))
        {
            AudioManager.instance.PlaySE(6);
            gameOverFlag = true;
        }

        ChengeSprite(collision.gameObject);
    }

    // Sprite???X
    void ChengeSprite(GameObject obj)
    {
        
        switch (obj.tag)
        {
            case "Maguro":
                CommonSprite(maguroSprite);
                break;
            case "Ikura":
                CommonSprite(ikuraSprite);
                break;
            case "Tamago":
                CommonSprite(tamagoSprite);
                break;
            case "Salmon":
                CommonSprite(salmonSprite);
                break;
            case "Ebi":
                CommonSprite(ebiSprite);
                break;
            case "Ika":
                CommonSprite(ikaSprite);
                break;
            case "Tako":
                CommonSprite(takoSprite);
                break;
            case "Hotate":
                CommonSprite(hotateSprite);
                break;
            case "Uni":
                CommonSprite(uniSprite);
                break;
            case "Tai":
                CommonSprite(taiSprite);
                break;
            case "Kyuuri":
                CommonSprite(kyuuriSprite);
                break;
            case "Natto":
                CommonSprite(nattoSprite);
                break;
            case "Syouyusashi":
                GameManager.instance.syouyusashiCtr.getSyouyuPoint++;
                break;


        }

    }

    void CommonSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        AudioManager.instance.PlaySE(5);
        anime.enabled = false;
    }

    //kuwapon tsuika gameover
    void GameOver()
    {
        gameoverText.SetActive(true);
        retryText.SetActive(true);
        returnTittleText.SetActive(true);
    }
    void Death()
    {
        gameObject.SetActive(false);
    }
}
