using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    float smoothTime = 0.3f;
    [SerializeField]Vector3 offset;
    Vector3 velocity = Vector3.zero;
    [SerializeField]int xMin = 10;
    [SerializeField]int xMax = 20;
    [SerializeField]int zMin = -2;
    [SerializeField]int zMax = 8;

    void Update()
    {
        if(target != null)
        {
            Vector3 targetPosition = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            LimitCameraPosition();
        }

    }

    public void LimitCameraPosition()
    {
        if (transform.position.x > xMax) transform.position = new Vector3(xMax, transform.position.y, transform.position.z);
        if (transform.position.x < xMin) transform.position = new Vector3(xMin, transform.position.y, transform.position.z);
        if (transform.position.z > zMax) transform.position = new Vector3(transform.position.x, transform.position.y, zMax);
        if (transform.position.z < zMin) transform.position = new Vector3(transform.position.x, transform.position.y, zMin);
    }
}
