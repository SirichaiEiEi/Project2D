using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectail : MonoBehaviour
{
    public float speed = 10f; // ความเร็วของกระสุน
    public int damage = 10; // ความเสียหายที่กระสุนจะส่งให้ผู้เล่น
    public GameObject impactEffect; // อ็อบเจกต์เอฟเฟกต์ที่เกิดขึ้นเมื่อกระสุนชนเป้าหมาย

    private void Start()
    {
        // กำหนดความเร็วให้กระสุนเคลื่อนที่
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // เมื่อกระสุนชนเป้าหมาย
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            // ถ้าเป้าหมายเป็นผู้เล่น
            player.TakeDamage(damage); // ให้ผู้เล่นรับความเสียหาย
        }

        // เล่นเอฟเฟกต์ที่เกิดขึ้นเมื่อกระสุนชนเป้าหมาย
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        // ทำลายกระสุน
        Destroy(gameObject);
    }
}
