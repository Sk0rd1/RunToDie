using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Vector3 positionOffset = Vector3.zero;

    void Update()
    {
        Vector3 targetPosition = target.position + positionOffset;
        targetPosition.y = positionOffset.y;
        targetPosition.x = transform.position.x;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1f);
    }
}
