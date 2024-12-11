using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hyb.Utils;
public class AudioManager : ManualSingletonMono<AudioManager>
{
    [Header("------------Audio------------")]
    public AudioSource audioSource;
    public AudioClip soundBackground;
    public AudioClip successGame;
    public AudioClip collect;
    public AudioClip click;
    public AudioClip coin;
    public override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundBackground;
        audioSource.Play();
    }

    public void PlayAudioSuccessGame()
    {
       StartCoroutine(Croutine());
    }
    IEnumerator Croutine(){
        audioSource.clip = null;
        audioSource.PlayOneShot(successGame,2f);
        yield return new WaitForSeconds(1f);
        audioSource.clip = soundBackground;
        audioSource.Play();
    }
    public void PlayAudioCollect(){
        audioSource.PlayOneShot(collect);
    }
    public void PlayAudioClick(){
        audioSource.PlayOneShot(click);
    }
    public void PlayAudioCoin(){
        audioSource.PlayOneShot(coin);
    }


}
