using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void Start()
    {
        Vector3 setPosition = transform.position;
        //setPosition.x = target.transform.position.x;
        setPosition.x = 0;
        setPosition.y = target.transform.position.y;
        transform.position = setPosition;
    }

    void FixedUpdate()
    {
        Vector3 setPosition = transform.position;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        setPosition.y = smoothedPosition.y;
        transform.position = setPosition;
    }
}
