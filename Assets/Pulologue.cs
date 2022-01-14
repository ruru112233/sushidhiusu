using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pulologue : MonoBehaviour
{
    int textNumber = 0;
    public Text pulologueText;
    public GameObject iMg0;
    public GameObject iMg1;
    public List<string> pulologueTextList = new List<string>()
    {
        "0",
        "うちゅうせいき2022ねん、きょうふのダイオウイカがあらわれた",
        "わくせいすしディウスはたちまちしんりゃくされてしまう",
        "すしやのおやじ「こまったなぁ、さかながとれなくておすしがにぎれないよぉ」",
        "そんなとき、ひとりのゆうしゃがたちあがった！",
        "しゃり「おやじ！おれにまかせとけぃっ！！」",
        "おやじ「おお！たのんだぞ！」",
        "そしてあたらしい「しんわ」がまくをあける、、、",
       
    };

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM(6);
    }



    public　void OnClickNext()
    {
        if(textNumber !=4 && textNumber != 7)
        {
            textNumber++;
            pulologueText.text = pulologueTextList[textNumber];

        }
        else if(textNumber==4)
        {
            iMg0.SetActive(false);
            iMg1.SetActive(true);
            textNumber++;
            pulologueText.text = pulologueTextList[textNumber];

        }

        
        else if(textNumber==7)
        { OnClick.instance.StageSceneButton(); }
        
    }
    
}
