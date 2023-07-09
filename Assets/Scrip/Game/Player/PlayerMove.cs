using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public int maxHP = 100; // HP สูงสุดของผู้เล่น
    private int currentHP; // HP ปัจจุบันของผู้เล่น

    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่ของผู้เล่น

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("ตุยแล้ว"); // แสดงข้อความในคอนโซลว่า "ตุยแล้ว"
        // สามารถเพิ่มโค้ดเพื่อจัดการกับการสิ้นสุดเกมตามต้องการ
    }
}

