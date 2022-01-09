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

    // �h���b�v�A�C�e���i�[
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
        // �G�̔j��
        if (EnemyHp <= 0)
        {
            // �A�C�e�����Z�b�g����Ă���A�h���b�v����
            if (itemPrefab.Count != 0)
            {
                Instantiate(itemPrefab[RandomItemNo()], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            }
            gameObject.SetActive(false);
        }
    }

    // �A�C�e�����X�g���烉���_���Ŕz��ԍ���ݒ�
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
