using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;

    // 弾のプレハブ
    [SerializeField] GameObject bulletPrefab;

    // 弾の発射位置
    [SerializeField] GameObject firePoint;

    // 弾の発射間隔
    float shotTime = 0;
    float shotDistanceTime = 0.1f;

    // オブジェクトプール対応
    [SerializeField] Transform normalBulletPool;

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
}
