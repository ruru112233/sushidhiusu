using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : MonoBehaviour
{
    public static OnClick instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }

    }

    public void TitleSceneButton()
    {
        StartCoroutine(PushButton(0));
    }

    public void StageSceneButton()
    {

        StartCoroutine(PushButton(1));
    }

    public void ReTryButton()
    {
        StartCoroutine(PushButton(2));
    }

    public void PulologueButton()
    {
        StartCoroutine(PushButton(3));
    }

    IEnumerator PushButton(int num)
    {
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlaySE(12);

        yield return new WaitForSeconds(1f);

        SceneSeni(num);
    }


    void SceneSeni(int num)
    {
        switch (num)
        {
            case 0:
                // ?^?C?g??
                SceneManager.LoadScene("TitleScene");
                break;
            case 1:
                // ?X?e?[?W?V?[??
                SceneManager.LoadScene("StageScene");
                break;
            case 2:
                // ???X?^?[?g
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case 3:
                
                SceneManager.LoadScene("Pulologue");
                break;
        }
    }
}
