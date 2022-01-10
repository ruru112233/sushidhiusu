using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParm : MonoBehaviour
{
    public int maguroInitHp = 1;
    public int ikuraInitHp = 1;
    public int tamagoInitHp = 1;
    public int salmonInitHp = 1;
    public int ebiInitHp = 1;
    public int ikaInitHp = 1;
    public int takoInitHp = 1;
    public int hotateInitHp = 1;
    public int uniInitHp = 1;
    public int taiInitHp = 1;
    public int kyuuriInitHp = 1;
    public int nattoInitHp = 1;


    // �h���b�v�A�C�e���i�[
    public List<GameObject> maguroItemPrefab,ikuraItemPrefab,
                                      tamagoItemPrefab,salmonItemPrefab,
                                      ebiItemPrefab, ikaItemPrefab,
                                      takoItemPrefab, hotateItemPrefab,
                                      uniItemPrefab, taiItemPrefab,
                                      kyuuriItemPrefab, nattoItemPrefab;
    public Transform dropItemPool;

    // �G�̒e
    public GameObject bulletPrefab;

    // �I�u�W�F�N�g�v�[���Ή�
    public Transform enemyBulletPool, hotateBulletPool;

    // �e�̑��x
    public float shotSpeed;

    public float shotTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
