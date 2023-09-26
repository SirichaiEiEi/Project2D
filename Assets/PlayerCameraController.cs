using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public LayerMask hiddenMonsterLayer; // กำหนด Layer ที่กล้องไม่ควรมองเห็น
    public Transform playerTransform; // เปลี่ยนเป็น public
    public Transform spawnerTransform; // เปลี่ยนเป็น public

    private Camera playerCamera;

    private void Start()
    {
        // ค้นหา Main Camera ในฉาก
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("Main Camera not found in the scene!");
            return; // ถ้าไม่พบ Main Camera ในฉาก หยุดทำงานในฟังก์ชันนี้
        }
    }

    private void Update()
    {
        if (playerCamera == null)
        {
            return; // ถ้าไม่มี Main Camera ในฉาก ไม่ต้องทำอะไร
        }

        // ตรวจสอบว่าผู้เล่นอยู่ใกล้ Spawner หรือไม่
        bool playerNearSpawner = Vector3.Distance(playerTransform.position, spawnerTransform.position) < 100f;

        // ปรับการแสดงผลของกล้องตามเงื่อนไข
        if (playerNearSpawner)
        {
            playerCamera.cullingMask &= ~hiddenMonsterLayer.value; // ไม่ให้กล้องมองเห็น Layer มอนสเตอร์
        }
        else
        {
            playerCamera.cullingMask |= hiddenMonsterLayer.value; // ให้กล้องมองเห็น Layer มอนสเตอร์
        }
    }
}
