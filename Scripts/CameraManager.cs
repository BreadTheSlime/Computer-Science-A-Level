using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform target;

    Vector3 offset = new Vector3(0f, 0f, -5f);
    Vector3 velocity = Vector3.zero;
    [SerializeField] float smoothTime;

    void Update()
    {
        if (target)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
