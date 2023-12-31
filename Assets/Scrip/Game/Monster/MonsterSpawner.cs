﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // โปรเฟปฟรีฟาบทหน้าสร้างมอนสเตอร์
    public GameObject randomBox;
    public GameObject warp;
    private float spawnInterval = 3f; // เวลาที่ระหว่างการเกิดมอนสเตอร์ (วินาที)
    private float spawnRadius = 5f; // รัศมีเกิดมอนสเตอร์ (หน่วย)
    public MonsterManager monsterManager;
    public GameObject Boss;

    private int monstersKilled = 0;
    private int points = 0;
    private Transform playerTransform;
    private bool isPortalOpen = false;
    private bool isBoss = false;
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
        Instantiate(randomBox, spawnPosition, Quaternion.identity);
    }

    public void MonsterKilled()
    {
        monstersKilled++;
        points++;
        if (monstersKilled >= 10)
        {
            // เพิ่มอัตราการเกิดมอนสเตอร์ เช่น อาจเพิ่มเป็นสองเท่า
            spawnInterval /= 2f;

            // รีเซ็ตจำนวนมอนสเตอร์ที่ถูกฆ่าเพื่อนับใหม่ในครั้งถัดไป
            monstersKilled = 0;
        }
        Forest forest = FindObjectOfType<Forest>();
        if (points >= 30 && !isPortalOpen) // เมื่อฆ่ามอนเตอร์ครบ 20 ตัวและยังไม่เปิดวาป
        {
            if( SceneManager.GetActiveScene().name == "Forest")
            {
                forest.TrunOn();
            }
            OpenPortal();
        }
        if(points >= 20 && !isBoss)
        {
            BossGen();
        }
    }

    public void BossGen()
    {
        Boss.SetActive(true);
        isBoss = true;
    }

    public void OpenPortal()
    {
        warp.SetActive(true);
        isPortalOpen = true;
    }
    public void SetPlayerActive(bool isActive)
    {
        isPlayerActive = isActive;
    }
}


