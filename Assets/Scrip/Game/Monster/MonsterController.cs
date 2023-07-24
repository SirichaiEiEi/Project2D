using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;
    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่ของมอนสเตอร์
    public int damageAmount = 1; // จำนวนดาเมทที่มอนสเตอร์จะโจมตีผู้เล่น
    public int maxHP = 100; // HP สูงสุดของมอนสเตอร์
    public int currentHP; // HP ปัจจุบันของมอนสเตอร์

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isPlayerActive = true; // ตัวแปรสำหรับตรวจสอบว่าผู้เล่นยังใช้งานอยู่หรือไม่

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
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
            Vector2 direction = playerTransform.position - transform.position;
            rb.velocity = direction.normalized * moveSpeed;

            // หมุนให้มอนสเตอร์หันไปทางทิศที่เคลื่อนที่
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            rb.velocity = Vector2.zero; // หยุดเคลื่อนที่เมื่อผู้เล่นหายไป
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
        // โค้ดสำหรับกระทำเมื่อมอนสเตอร์ตาย
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damageAmount);
            }
        }
    }

    public void SetPlayerActive(bool isActive)
    {
        isPlayerActive = isActive;
    }
}
