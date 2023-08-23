using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;
    public float moveSpeed = 1f;
    public int damageAmount = 10;
    public int maxHP = 100;
    public int currentHP;

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
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        healthBar.UpdateHealthBar(currentHP, maxHP);
    }

    private void Update()
    {
        if (playerTransform != null && isPlayerActive)
        {
            Vector2 direction = playerTransform.position - (Vector3)rb.position;
            rb.velocity = direction.normalized * moveSpeed;

            // หมุนให้มอนสเตอร์หันไปทางผู้เล่น
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle - 90f, Time.deltaTime * rotationSpeed);

            // ตรวจสอบสถานะของการเคลื่อนที่ หากไม่มีการเคลื่อนที่ให้หยุดการเคลื่อนที่
            if (direction.sqrMagnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
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
}
