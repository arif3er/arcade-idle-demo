using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public bool lookAtTarget;
    public float speed;

    public Vector3 positionOffset,lookOffset;

    private Vector3 desiredPosition,smoothedPosition;

    void LateUpdate()
    {
        desiredPosition = target.position + positionOffset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
        transform.position = smoothedPosition;

        if (lookAtTarget)
            transform.LookAt(target.position + lookOffset);
    }
}
