using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyouyuController : MonoBehaviour
{
    [SerializeField] GameObject syouyuPrefab;

    Transform syouyuPool;

    float speed = 0.3f;

    float currentTime = 0;
    float shotTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        syouyuPool = GameObject.FindWithTag("SyouyuPool").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

        currentTime += Time.deltaTime;

        if (shotTime < currentTime)
        {
            currentTime = 0;
            GetNomalBulletObj(syouyuPrefab, 
                              new Vector3(transform.position.x + 0.5f,
                                          transform.position.y,
                                          transform.position.z),
                              Quaternion.identity);
        }

    }

    void GetNomalBulletObj(GameObject bulletPrefab, Vector3 pos, Quaternion qua)
    {
        foreach (Transform t in syouyuPool)
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

        Instantiate(bulletPrefab, pos, qua, syouyuPool);
    }
}
