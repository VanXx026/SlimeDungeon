using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float smoothTime = 0.1f;
    public Transform target;
    private Vector3 currentVelocity;

    private void Awake()
    {

    }

    void Update()
    {
        if(target != null)
        {
            Vector3 smoothMove = Vector3.SmoothDamp(transform.position, new Vector3(target.transform.position.x, target.position.y, transform.position.z), ref currentVelocity, smoothTime);
            transform.position = smoothMove;
        }

    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
