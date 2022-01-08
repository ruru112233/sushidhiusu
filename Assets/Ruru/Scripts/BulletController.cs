using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    private int bulletAtPoint = 1;

    public int BulletAtPoint
    {
        get { return bulletAtPoint; }
        set { bulletAtPoint = value; }
    }

    [SerializeField] float speed;

    // ’e‚ÌÁ–ÅŽžŠÔ
    float countTime = 0;
    float delTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        countTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;

        if (countTime > delTime)
        {
            countTime = 0;
            gameObject.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
