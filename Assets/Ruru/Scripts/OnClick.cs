using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : MonoBehaviour
{
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

    IEnumerator PushButton(int num)
    {
        AudioManager.instance.PlaySE(12);

        yield return new WaitForSeconds(0.5f);

        SceneSeni(num);
    }


    void SceneSeni(int num)
    {
        switch (num)
        {
            case 0:
                // タイトル
                SceneManager.LoadScene("TitleScene");
                break;
            case 1:
                // ステージシーン
                SceneManager.LoadScene("StageScene");
                break;
            case 2:
                // リスタート
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }
}
