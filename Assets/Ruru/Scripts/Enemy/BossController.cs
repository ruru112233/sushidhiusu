using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyBase
{
    bool upperFlag = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (isGate)
        {
            // 弾の発射
            if (shotCurrentTime == 0)
            {
                enemyParm.shotTime = 1;
                SumiShot(enemyParm.sumiBullet,
                            new Vector3(firePoint.transform.position.x,
                                        firePoint.transform.position.y,
                                        firePoint.transform.position.z),
                            Quaternion.identity);
            }

            if (transform.position.y >= 3)
            {
                upperFlag = false;
            }
            else if(transform.position.y <= -3)
            {
                upperFlag = true;
            }

            MoveBoss();

        }

    }

    void MoveBoss()
    {
        if (upperFlag)
        {
            transform.position += new Vector3(0, 1 * Time.deltaTime, 0);
        }
        else
        {
            transform.position -= new Vector3(0, 1 * Time.deltaTime, 0);
        }
    }

    void SumiShot(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in enemyParm.sumiBulletPool)
        {
            // 弾が非アクティブなら使いまわし
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                GameObject bullet = t.gameObject;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.AddForce(new Vector3(-1, 0, 0) * enemyParm.shotSpeed * 8);
                return;
            }
        }
        GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, pos, qua, enemyParm.sumiBulletPool);
        Rigidbody2D bulletBody = bullet2.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(new Vector3(-1, 0, 0) * enemyParm.shotSpeed * 8);

    }

}
