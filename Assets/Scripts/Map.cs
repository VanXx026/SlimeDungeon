using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public bool isOpen;
    public GameObject map;

    private void Awake()
    {
        isOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isOpen)
            {
                isOpen = true;
                OpenMap(true);

            }
            else
            {
                isOpen = false;
                OpenMap(false);
            }
        }

    }

    private void OpenMap(bool isOpen)
    {
        map.SetActive(isOpen);
    }
}
