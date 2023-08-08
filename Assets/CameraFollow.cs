using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ตำแหน่งที่กล้องจะตาม (ผู้เล่น)

    public float smoothSpeed = 0.125f; // ความราบรื่นในการเคลื่อนที่ของกล้อง
    public Vector3 offset; // ระยะห่างระหว่างกล้องและผู้เล่น

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);

        transform.LookAt(target);
    }
}






