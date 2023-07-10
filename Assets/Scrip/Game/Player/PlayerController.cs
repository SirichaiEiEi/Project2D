using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHP = 100; // HP สูงสุดของผู้เล่น
    public float fireRate = 0.2f; // อัตราการยิง (วินาทีต่อกระสุน)
    public GameObject bulletPrefab; // โปรเฟปฟรีฟาบทหน้ากระสุน
    public Transform firePoint; // ตำแหน่งที่กระสุนจะถูกสร้าง

    public int currentHP; // HP ปัจจุบันของผู้เล่น
    private float nextFireTime; // เวลาถัดไปที่สามารถยิงได้

    private Rigidbody2D rb;
    private Camera mainCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = moveDirection * moveSpeed;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        // สามารถเพิ่มโค้ดเพื่อจัดการกับการสิ้นสุดเกมตามต้องการ
    }

    private void Shoot()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.SetTarget(mousePosition);
        }
    }
}


