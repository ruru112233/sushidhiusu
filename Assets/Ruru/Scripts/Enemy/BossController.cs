using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyBase
{

    bool bossMoveFlag = false;

    // ?????????p
    bool upperFlag = false;

    // ?????p
    bool turningUpFlag = false,
         turningRightFlag = false,
         turningDownFlag = false,
         turningLeftFlag = false;

    float bossMoveSpeed = 1.3f;

    float currentActionTime = 0f;
    float actionTime;

    enum BulletType
    {
        Normal,
        Ika,
    }

    BulletType bulletType;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        bulletType = BulletType.Normal;
        actionTime = 10f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (isGate)
        {
            StartCoroutine(BossRoute());

            AudioManager.instance.PlayBGM(8);

            if (bossMoveFlag)
            {
                // ?e??????
                
                if (shotCurrentTime == 0)
                {
                    enemyParm.shotTime = 0.9f;
                    if (bulletType == BulletType.Normal)
                    {
                        
                        SumiShot1(enemyParm.sumiBullet,
                                    new Vector3(firePoint.transform.position.x,
                                                firePoint.transform.position.y,
                                                firePoint.transform.position.z),
                                    Quaternion.identity);
                    }
                    else if (bulletType == BulletType.Ika)
                    {
                        IkaBullet(enemyParm.ikaBullet,
                                  new Vector3(firePoint.transform.position.x - 3,
                                              firePoint.transform.position.y,
                                              firePoint.transform.position.z),
                                  Quaternion.identity);
                    }
                }

                if (transform.position.y >= 3)
                {
                    upperFlag = false;
                }
                else if (transform.position.y <= -3)
                {
                    upperFlag = true;
                }

                ActionChenge();

                //TurningMove();
            }

        }

    }

    // ????????
    void UpDownMoveBoss()
    {
        if (upperFlag)
        {
            transform.position += new Vector3(0, bossMoveSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.position -= new Vector3(0, bossMoveSpeed * Time.deltaTime, 0);
        }
    }

    // ????????????
    void TurningMove()
    {
        

        if (turningUpFlag)
        {
            Debug.Log("??");
            TurningChenge();
            transform.position += new Vector3(0, bossMoveSpeed * Time.deltaTime, 0);
        }
        else if (turningRightFlag)
        {
            Debug.Log("?E");
            TurningChenge();

            transform.position += new Vector3(bossMoveSpeed * Time.deltaTime, 0, 0);
        }
        else if (turningDownFlag)
        {
           // Debug.Log("??");
            TurningChenge();

            transform.position -= new Vector3(0, bossMoveSpeed * Time.deltaTime, 0);
        }
        else if (turningLeftFlag)
        {
            Debug.Log("??");
            TurningChenge();

            transform.position -= new Vector3(bossMoveSpeed * Time.deltaTime, 0, 0);
        }
    }

    // ?^?[?j???O?t???O???????????X
    void TurningChenge()
    {
        //if (transform.position.x < -5)
        //{
        //    turningUpFlag = true;
        //    turningRightFlag = false;
        //    turningDownFlag = false;
        //    turningLeftFlag = false;
        //}
        //else if(transform.position.x < -5f && transform.position.y > 3.5f)
        //{
        //    turningUpFlag = false;
        //    turningRightFlag = true;
        //    turningDownFlag = false;
        //    turningLeftFlag = false;
        //}
        //else if (transform.position.x < 5)
        //{
        //    turningUpFlag = false;
        //    turningRightFlag = false;
        //    turningDownFlag = true;
        //    turningLeftFlag = false;
        //}
        //else if (transform.position.y < -3f)
        //{
        //    turningUpFlag = false;
        //    turningRightFlag = false;
        //    turningDownFlag = false;
        //    turningLeftFlag = true;
        //}

        if (transform.position.x > 12 && transform.position.y > -3)
        {
            turningUpFlag = false;
            turningRightFlag = false;
            turningDownFlag = true;
            turningLeftFlag = false;
        }
        else if (transform.position.x > 3 && transform.position.y <= -3)
        {
            Debug.Log(transform.position.x);
            turningUpFlag = false;
            turningRightFlag = false;
            turningDownFlag = false;
            turningLeftFlag = true;
        }
        //else if (transform.position.x <= 3)
        //{
            
        //    turningUpFlag = true;
        //    turningRightFlag = false;
        //    turningDownFlag = false;
        //    turningLeftFlag = false;
        //}

    }

    // ???F?????n???????????B
    void SumiShot1(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in enemyParm.sumiBulletPool)
        {
            // ?e?????A?N?e?B?u?????g????????
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

    // ?C?J?I?u?W?F???o??
    void IkaBullet(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in enemyParm.ikaBulletPool)
        {
            // ?e?????A?N?e?B?u?????g????????
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, qua);
                t.gameObject.SetActive(true);
                return;
            }
        }

        Instantiate(bulletPrefab, pos, qua, enemyParm.ikaBulletPool);
        
    }

    IEnumerator BossRoute()
    {


        yield return new WaitForSeconds(3.0f);

        bossMoveFlag = true;


        yield return new WaitForSeconds(0.1f);
        
    }

    void ActionChenge()
    {
        currentActionTime += Time.deltaTime;

        if (currentActionTime > actionTime)
        {
            currentActionTime = 0;

            if (bulletType == BulletType.Normal)
            {
                bulletType = BulletType.Ika;
                actionTime = 3.1f;

            }
            else
            {
                bulletType = BulletType.Normal;
                actionTime = 10.0f;
            }
        }

        UpDownMoveBoss();
    }

}
