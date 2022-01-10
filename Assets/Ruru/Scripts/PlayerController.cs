using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;

    // �e�̃v���n�u
    [SerializeField] GameObject bulletPrefab;

    // �e�̔��ˈʒu
    [SerializeField] GameObject firePoint;

    // �e�̔��ˊԊu
    float shotTime = 0;
    float shotDistanceTime = 0.1f;

    // �I�u�W�F�N�g�v�[���Ή�
    [SerializeField] Transform normalBulletPool;

    // �Q�[���I�[�o�[
    bool gameOverFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        shotTime = 0;
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

        GetNomalBulletObj(bulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z), Quaternion.identity);
    }

    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in normalBulletPool)
        {
            // �e����A�N�e�B�u�Ȃ�g���܂킵
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
        }

        if (collision.gameObject.CompareTag("Gate"))
        {
            gameOverFlag = true;
        }
    }
}
