using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    private int enemyHp = 1;
    
    public int EnemyHp
    {
        get { return enemyHp; }
        set { enemyHp = value; }
    }

    bool isGate = false;

    float countTime = 0;
    [SerializeField] float distanceTime = 2.0f;

    [SerializeField] float xPos, yPos;

    // Start is called before the first frame update
    void Start()
    {
        isGate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGate)
        {
            countTime += Time.deltaTime;
        }

        if (countTime > distanceTime)
        {
            transform.position += new Vector3(xPos * Time.deltaTime, yPos * Time.deltaTime, 0);
        }

        // “G‚Ì”j‰ó
        if (EnemyHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gate"))
        {
            isGate = true;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            int at = collision.gameObject.GetComponent<BulletController>().BulletAtPoint;
            Debug.Log(at);

            EnemyHp -= at;
        }
    }
}
