using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntidoteItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // เรียกใช้ฟังก์ชันที่เพิ่มสถานะการกันพิษให้ผู้เล่น
                playerController.GainPoisonImmunity();

                // ทำให้ไอเท็มหายไป
                Destroy(gameObject);
            }
        }
    }
}
