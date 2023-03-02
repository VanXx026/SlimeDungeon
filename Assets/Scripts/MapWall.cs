using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWall : MonoBehaviour
{
    private GameObject mapSprite;

    private void OnEnable()
    {
        mapSprite = transform.parent.GetChild(0).gameObject;

        mapSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            mapSprite.SetActive(true);
        }
    }
}
