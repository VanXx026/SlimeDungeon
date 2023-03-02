using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            AudioManager.instance.PlayPlayerWinAudio();
            ScreenCanvasManager.instance.OpenWinPanel();
        }
    }
}
