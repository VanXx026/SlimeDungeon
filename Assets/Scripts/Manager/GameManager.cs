using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public RoomGenerator roomGenerator;
    public bool isGameOver;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);

        // player = FindObjectOfType<PlayerController>().gameObject;
        // roomGenerator = FindObjectOfType<RoomGenerator>().gameObject.GetComponent<RoomGenerator>();

        isGameOver = false;
    }

    public void GameOver(bool isPlayerDead)
    {
        if(isPlayerDead)
        {
            isGameOver = true;
            AudioManager.instance.PlayPlayerDeadAudio();
            ScreenCanvasManager.instance.OpenGameOverPanel();
        }
    }


}
