using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyouyusashiCtr : MonoBehaviour
{
    [SerializeField] GameObject syouyusashi01, syouyusashi02;

    public int getSyouyuPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        syouyusashi01.SetActive(false);
        syouyusashi02.SetActive(false);
    }

    private void Update()
    {
        if (getSyouyuPoint >= 2)
        {
            syouyusashi02.SetActive(true);
        }
        else if (getSyouyuPoint == 1)
        {
            syouyusashi01.SetActive(true);
        }
    }

}
