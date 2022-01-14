using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject clearText;

    public static Ending instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }

    }
    public GameObject player;


    public void End()
    {
        Debug.Log("GameClear");
        clearText.SetActive(true);
        
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<BoxCollider2D>().enabled = false;
        Invoke("PlayEnd",3);
        Invoke("ToEndScene", 11);


    }
    void PlayEnd()
    {
        AudioManager.instance.PlayBGM(3);
    }
    void ToEndScene()
    {
        SceneManager.LoadScene("End");
    }
}

