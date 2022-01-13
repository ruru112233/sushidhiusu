using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] se;
    public AudioSource[] bgm;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (this != instance)
        {
            Destroy(this.gameObject);

            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySE(int soundToPlay)
    {
        if (soundToPlay < se.Length)
        {
            se[soundToPlay].Play();
        }
    }

    public void PlayBGM(int musicToPlay)
    {
        if (!bgm[musicToPlay].isPlaying)
        {
            StopMusic();

            if (musicToPlay < bgm.Length)
            {
                bgm[musicToPlay].Play();
            }
        }
    }

    public void StopMusic()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }



    // SEの音量調整
    /*
     * volumeSize ボリュームの大きさ
    */
    public void SeVolume(float volumeSize)
    {
        if (se[0] != null)
        {
            for (int i = 0; i < se.Length; i++)
            {
                se[i].volume += volumeSize;
            }
        }
    }

    // SEのスライダー
    public void SeSliderVolume(float volume)
    {
        if (se[0] != null)
        {
            for (int i = 0; i < se.Length; i++)
            {
                se[i].volume = volume;
            }
        }
    }

    // SEの音量を取得
    public float GetSeVolume()
    {
        float vol = 0;

        if (se[0] != null)
        {
            vol = se[0].volume;
        }

        return vol;
    }

    // BGMの音量を調整する
    /* 
     * volumeSize ボリュームの大きさ
    */
    public void BgmVolume(float volumeSize)
    {
        if (bgm[0] != null)
        {
            for (int i = 0; i < bgm.Length; i++)
            {
                bgm[i].volume += volumeSize;
            }
        }
    }

    // BGMのスライダー
    public void BgmSliderVolume(float volume)
    {

        if (bgm[0] != null)
        {
            for (int i = 0; i < bgm.Length; i++)
            {
                bgm[i].volume = volume;
            }
        }

    }

    // BGMの音量を取得
    public float GetBgmVolume()
    {
        float vol = 0;

        if (se[0] != null)
        {
            vol = bgm[0].volume;
        }

        return vol;
    }
}
