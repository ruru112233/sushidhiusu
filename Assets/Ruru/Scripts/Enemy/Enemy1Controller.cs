using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gate"))
        {
            isGate = true;
        }
    }
}
