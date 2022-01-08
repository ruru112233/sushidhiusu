using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Contoller : EnemyBase
{

    // ‹O“¹
    //*****
    //    *
    //   *
    //  *
    // *
    //**************

    float countTime = 0;
    [SerializeField] float distanceTime = 2.0f;
    [SerializeField] float distanceTime2 = 2.0f;

    [SerializeField] float xPos, yPos;



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
            countTime += Time.deltaTime;
        }

        if (countTime > distanceTime && countTime < distanceTime2)
        {
            transform.position += new Vector3(xPos * Time.deltaTime, yPos * Time.deltaTime, 0);
        }
    }
}
