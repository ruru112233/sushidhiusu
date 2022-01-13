using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
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
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayBGM(3);
        player.GetComponent<PlayerController>().enabled = false;
        Invoke("ToEndScene", 8);


    }
    void ToEndScene()
    {
        SceneManager.LoadScene("End");
    }
}

