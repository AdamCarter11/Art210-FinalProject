using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smoothSpeed = 0.125f;
    private Vector3 offset;
    private void Start()
    {
        offset = transform.position - player.position;
    }
    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(player);
    }
}
 