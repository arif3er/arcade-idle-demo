using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform newLookRefObj;

    public Transform target;
    public bool lookAtTarget;
    public float speed;

    public Vector3 positionOffset,lookOffset;

    private Vector3 desiredPosition,smoothedPosition;

    void LateUpdate()
    {
        if (ArifHelpers.DistanceTrigger(Player.Instance.gameObject, newLookRefObj.gameObject, 7f))
        {
            positionOffset.x = 5;
            positionOffset.y = 25;
            positionOffset.z = -20;

            lookOffset.x = 5;

            speed = 5;
        }
        else
        {
            positionOffset.x = 0;
            positionOffset.y = 20;
            positionOffset.z = -14;

            lookOffset.x = 0;
        }

        desiredPosition = target.position + positionOffset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
        transform.position = smoothedPosition;
        speed = 10;

        if (lookAtTarget)
            transform.LookAt(target.position + lookOffset);
    }
}
