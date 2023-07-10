using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // โปรเฟปฟรีฟาบทหน้าสร้างมอนสเตอร์
    public float spawnInterval = 5f; // เวลาที่ระหว่างการเกิดมอนสเตอร์ (วินาที)
    public float spawnRadius = 10f; // รัศมีเกิดมอนสเตอร์ (หน่วย)

    private Transform playerTransform;
    private bool isPlayerActive = true; // ตัวแปรสำหรับตรวจสอบว่าผู้เล่นยังใช้งานอยู่หรือไม่

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("SpawnMonster", spawnInterval, spawnInterval);
    }

    private void SpawnMonster()
    {
        if (!isPlayerActive)
        {
            return; // เลิกสร้างมอนสเตอร์เมื่อผู้เล่นหายไป
        }

        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomPosition.x, randomPosition.y, 0f);
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }

    public void SetPlayerActive(bool isActive)
    {
        isPlayerActive = isActive;
    }
}


