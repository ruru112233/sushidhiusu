using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Controller : EnemyBase
{
    // ‹O“¹
    // *     *
    //  *   * *
    //   * *   *
    //    *     *
    //   

    bool isUpperFlag = false;

    float countTime = 0;
    [SerializeField] float distanceTime = 2.0f;

    public float waveSpeed = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        countTime += Time.deltaTime;

        if (countTime > distanceTime)
        {
            countTime = 0;
            isUpperFlag = !isUpperFlag;
        }

        if (isUpperFlag)
        {
            transform.position += new Vector3(0, waveSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.position -= new Vector3(0, waveSpeed * Time.deltaTime, 0);
        }
    }
}
