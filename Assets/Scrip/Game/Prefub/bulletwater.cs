﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletwater : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.waterarmor(); // เพิ่มเลือดให้เต็ม
                Destroy(gameObject); // ทำลายไอเท็มหลังจากใช้งาน
            }

        }
    }
}
