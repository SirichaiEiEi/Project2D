using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    public GameObject itemSand; // ตัวแปรเก็บ Prefab ของไอเท็มที่จะดรอป
    public int dropChance = 100; // ความน่าจะเป็นในการดรอปไอเท็ม (ในหน่วยเปอร์เซ็นต์)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // ใช้ Random.Range(0, 100) เพื่อตรวจสอบความน่าจะเป็นในการดรอป
            int randomValue = Random.Range(0, 100);
            if (randomValue < dropChance)
            {

                DropItem(); // ถ้าความน่าจะเป็นเป็นไปตามที่กำหนดให้ดรอปไอเท็ม
            }

            Destroy(gameObject); // ทำลายกล่องดรอปหลังจากถูกยิง
            Destroy(collision.gameObject); // ทำลายกระสุนที่ชนกับกล่องดรอป
        }
    }

    private void DropItem()
    {
        // สร้าง Object ของไอเท็มที่จะดรอปที่ตำแหน่งของกล่องดรอป
        Instantiate(itemSand, transform.position, Quaternion.identity);
    }
}

