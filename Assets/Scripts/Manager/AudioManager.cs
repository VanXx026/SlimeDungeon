using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("基本声音")]
    public AudioClip BGMClip;
    public AudioClip attackClip;
    public AudioClip takeBackClip;
    public AudioClip healClip;
    public AudioClip getHitClip;
    public AudioClip getChestClip;
    public AudioClip deadClip;
    public AudioClip winClip;

    [Header("播放器")]
    public AudioSource playerSource;
    public AudioSource enemySource;
    public AudioSource BGMSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);

        playerSource = gameObject.AddComponent<AudioSource>();
        enemySource = gameObject.AddComponent<AudioSource>();
        BGMSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        PlayBGMAudio();
    }

    public void PlayBGMAudio()
    {
        BGMSource.clip = BGMClip;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public void PlayPlayerAttackAudio()
    {
        playerSource.clip = attackClip;
        playerSource.Play();
    }

    public void PlayPlayerTakeBackAudio()
    {
        playerSource.clip = takeBackClip;
        playerSource.Play();
    }

    public void PlayPlayerHealAudio()
    {
        playerSource.clip = healClip;
        playerSource.Play();
    }

    public void PlayPlayerGetHitAudio()
    {
        playerSource.clip = getHitClip;
        playerSource.Play();
    }

    public void PlayPlayerGetChestAudio()
    {
        playerSource.clip = getChestClip;
        playerSource.Play();
    }

    public void PlayPlayerDeadAudio()
    {
        playerSource.clip = deadClip;
        playerSource.Play();
    }

    public void PlayPlayerWinAudio()
    {
        playerSource.clip = winClip;
        playerSource.Play();
    }

    public void PlayEnemyGetHitAudio()
    {
        enemySource.clip = getHitClip;
        enemySource.Play();
    }
}
