using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MosterRanger : MonoBehaviour
{
    public string hiddenLayerName = "HiddenMonster"; // ชื่อ Layer ที่ใช้ซ่อนมอนเตอร์
    private int defaultLayer; // Layer ที่ใช้ในสถานการณ์ปกติ
    [SerializeField] FloatingHealthBar healthBar;
    private float moveSpeed = 3f;
    public int damageAmount = 10;
    public int maxHP = 100;
    public int currentHP;
    public GameObject projectilePrefab; // โปรเจกไทล์ของกระสุน
    public Transform firePoint; // จุดที่จะยิงจาก
    public float fireRate = 2f; // อัตราการยิง (ยิงทุกๆ 2 วินาที)
    private float nextFireTime; // เวลาที่จะยิงครั้งถัดไป


    private Transform playerTransform;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isPlayerActive = true;
    public float rotationSpeed = 5f;
    private bool isAttacking = false;
    public MonsterManager monsterManager;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        defaultLayer = gameObject.layer;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        healthBar.UpdateHealthBar(currentHP, maxHP);
    }

    private void Update()
    {
        if (CanFire())
        {
            Fire();
        }
    }

    private bool CanFire()
    {
        return Time.time >= nextFireTime && playerTransform != null; // เพิ่มตรวจสอบว่า playerTransform ไม่เป็น null
    }

    private void Fire()
    {
        if (projectilePrefab != null && playerTransform != null)
        {
            // คำนวณทิศทางและมุมการยิง
            Vector2 direction = playerTransform.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // สร้างกระสุน
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));

            // เซ็ตความเสียหายของกระสุน
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(playerTransform.position); // ให้กระสุนเป็นเป้าหมายที่ตำแหน่งของผู้เล่น
            }
        }

        // ปรับเปลี่ยนเวลาถัดไปที่จะยิง
        nextFireTime = Time.time + 1f / fireRate;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        healthBar.UpdateHealthBar(currentHP, maxHP);

        Debug.Log("มอนสเตอร์ได้รับดาเมท: " + damageAmount);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // ส่งข้อมูลจำนวนมอนสเตอร์ที่ถูกฆ่าไปยัง MonsterSpawner
        MonsterSpawner monsterSpawner = FindObjectOfType<MonsterSpawner>();
        if (monsterSpawner != null)
        {
            monsterSpawner.MonsterKilled();
        }
        MonsterManager monsterManager = FindObjectOfType<MonsterManager>();
        if (monsterManager != null)
        {
            monsterManager.MonsterKilled1();
        }
        Sand sand = FindObjectOfType<Sand>();
        if (sand != null)
        {
            sand.kills();
        }
        ArmorGear armorGear = FindObjectOfType<ArmorGear>();
        if (armorGear != null)
        {
            armorGear.kills();
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // ทำการโจมตีผู้เล่น
                if (!isAttacking)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // หยุดเล่น Animation และกำหนดให้ไม่ทำการโจมตีอีก
            animator.SetTrigger("Idle");
            isAttacking = false;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        // เริ่มโจมตี
        isAttacking = true;

        // เล่น Animation Attack
        animator.SetTrigger("Attack");

        // รอจนกว่า animation โจมตีจะเสร็จสิ้น
        yield return new WaitForSeconds(1f); // รอเป็นเวลา 1 วินาที

        DealDamageToPlayer();

        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // ตรวจสอบว่าอยู่ในฉาก 'Forest' หรือไม่
            if (SceneManager.GetActiveScene().name == "Forest")
            {
                playerController.ApplyPoison();
            }
        }

        // ตรวจสอบระยะห่างระหว่างมอนสเตอร์และผู้เล่นอีกครั้ง
        // หากยังคงอยู่ใกล้กันให้ทำดาเมทต่อเนื่อง โดยเริ่ม Coroutine AttackCoroutine ใหม่
        Vector2 direction = playerTransform.position - (Vector3)rb.position;
        if (direction.sqrMagnitude < 1f && isPlayerActive)
        {
            StartCoroutine(AttackCoroutine());
        }
        else
        {
            // ถ้าผู้เล่นได้ห่างออกไปแล้วให้หยุดการโจมตีและเล่น Animation Idle
            isAttacking = false;
            animator.SetTrigger("Idle");
        }
    }

    public void DealDamageToPlayer()
    {
        // โจมตีผู้เล่น
        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damageAmount);
        }

    }

    public void SetPlayerActive(bool isActive)
    {
        isPlayerActive = isActive;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            // เมื่อมอนเตอร์เข้าสู่พื้นที่ที่คุณต้องการให้ซ่อน
            gameObject.layer = LayerMask.NameToLayer(hiddenLayerName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            // เมื่อมอนเตอร์ออกจากพื้นที่ที่คุณต้องการให้ซ่อน
            gameObject.layer = defaultLayer; // คืนสถานะ Layer กลับไปเป็นปกติ
        }
    }

}
