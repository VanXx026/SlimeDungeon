using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    public float smoothTime = 0.1f;
    private Transform target;
    private Vector3 currentVelocity;

    private void Start()
    {
        target = GameManager.instance.player.transform;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 smoothMove = Vector3.SmoothDamp(transform.position, new Vector3(target.transform.position.x, target.position.y, transform.position.z), ref currentVelocity, smoothTime);
            transform.position = smoothMove;
        }
    }

}
