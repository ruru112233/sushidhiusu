using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;

    // �e�̃v���n�u
    [SerializeField] GameObject bulletPrefab, ikuraBulletPrefab;

    // �e�̔��ˈʒu
    [SerializeField] GameObject firePoint;

    // �e�̔��ˊԊu
    float shotTime = 0;
    float shotDistanceTime = 0.3f;

    // �I�u�W�F�N�g�v�[���Ή�
    [SerializeField] Transform normalBulletPool, ikuraBulletPool;

    // �v���C���[�̃X�v���C�g
    SpriteRenderer spriteRenderer;

    // �Q�[���I�[�o�[
    bool gameOverFlag = false;

    // �X�v���C�g
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

        // �Q�[���I�[�o�[�ɂȂ������̏���
        if (gameOverFlag)
        {
            Debug.Log("GameOver");
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    // �L�����N�^�[�̈ړ�
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

    // �e�̔���
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

    // �V�������̍U��
    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in normalBulletPool)
        {
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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

    // ������R�͎��̍U��
    void GetIkuraBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in ikuraBulletPool)
        {
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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

    // �z�^�e�̍U��
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
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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

    // Sprite�ύX
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
