using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private int enemyHp = 1;

    public int EnemyHp
    {
        get { return enemyHp; }
        set { enemyHp = value; }
    }

    public int initHp = 1;

    protected bool isGate = false;

    // ドロップアイテム格納
    [SerializeField] List<GameObject> itemPrefab;

    // Start is called before the first frame update
    public virtual void Start()
    {
        isGate = false;
        EnemyHp = initHp;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // 敵の破壊
        if (EnemyHp <= 0)
        {
            // アイテムがセットされてたら、ドロップする
            if (itemPrefab.Count != 0)
            {
                Instantiate(itemPrefab[RandomItemNo()], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            }
            gameObject.SetActive(false);
        }
    }

    // アイテムリストからランダムで配列番号を設定
    int RandomItemNo()
    {
        int randNo = Random.Range(0, itemPrefab.Count);

        return randNo;
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
