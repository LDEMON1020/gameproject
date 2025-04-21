using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;
    // Start is called before the first frame update

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 SmoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = SmoothPosition;

        transform.LookAt(transform.position);
    }
}
