using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{

    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }

}
