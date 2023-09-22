using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar1;
    public AudioClip _sound;    // this lets you drag in an audio file in the inspector
    public static PlayerController Instance;
    private AudioSource audio;
    public float moveSpeed = 5f;
    public TMP_Text hpText;
    public int maxHP = 100; // HP สูงสุดของผู้เล่น
    public float fireRate = 0.001f; // อัตราการยิง (วินาทีต่อกระสุน)
    public GameObject bulletPrefab; // โปรเฟปฟรีฟาบทหน้ากระสุน
    public GameObject bulletWater; // กระสุนน้ำ
    public GameObject bulletFire; // กระสุนน้ำ
    public Transform firePoint; // ตำแหน่งที่กระสุนจะถูกสร้าง
    public Transform muzzle;
    [SerializeField] private GameObject muzzsleFlash;

    public int currentHP; // HP ปัจจุบันของผู้เล่น

    private float nextFireTime; // เวลาถัดไปที่สามารถยิงได้
    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool isShooting = false; // ตัวแปรสำหรับตรวจสอบว่ากำลังยิงปืนหรือไม่
    private float cameraShakeMagnitude = 0.1f; // ระดับความสั่นของกล้อง
    private float cameraShakeDuration = 0.1f; // ระยะเวลาที่กล้องจะสั่น
    private Vector3 originalCameraPosition; // ตำแหน่งเริ่มต้นของกล้อง
    private bool isPoisoned = false;
    private float poisonTimer = 0f;
    private float poisonInterval = 1f; // ระยะเวลาที่จะลดเลือดเนื่องจากพิษ
    private float poisonSlowMultiplier = 0.5f; // 80% ของความเร็วเดิม
    private float originalMoveSpeed; // ความเร็วเดิมของผู้เล่น
    private bool hasPoisonImmunity = false; // เก็บสถานะว่าผู้เล่นมีการกันพิษหรือไม่
    private bool hasWaterBulletItem = false;
    private bool hasFireBulletItem = false;
    private bool isReducingSpeed = false; // เพิ่มตัวแปรเพื่อตรวจสอบว่ากำลังลดความเร็วอย่างต่อเนื่องหรือไม่
    private float slowMoveSpeedMultiplier = 0.5f; // 50% ของความเร็วเดิม
    private float timeToReachHalfSpeed = 10.0f; // เวลาที่ใช้ในการลดความเร็วไปยัง 50%
    private float slowMoveSpeedDamageInterval = 1.0f; // ระยะเวลาที่ลดเลือดเนื่องจากความเร็วช้า
    private float slowMoveSpeedDamageTimer = 0f; // ตัวจับเวลาเมื่อลดเลือดเนื่องจากความเร็วช้า
    private int slowMoveSpeedDamageAmount = 1; // จำนวนเลือดที่ลดเมื่อความเร็วช้า
    private float currentMoveSpeed; // ความเร็วปัจจุบัน
    private float startReducingSpeedTime; // เวลาที่เริ่มลดความเร็ว
    private float slowMoveSpeedInterval; // ระยะเวลาระหว่างการลดความเร็ว
    private float slowMoveSpeedTimer = 0f; // ตัวจับเวลาเมื่อลดความเร็ว
    private bool hasSnowGear = false;
    private bool hasHeatArmor = false; // เก็บสถานะว่าผู้เล่นมีเกราะกันความร้อนหรือไม่
    private float sandDamageInterval = 1.0f; // ระยะเวลาที่จะลดเลือดเนื่องจากแมพ "Sand"
    private float sandDamageTimer = 0f; // ตัวจับเวลาเมื่อลดเลือดเนื่องจากแมพ "Sand"

    private void Awake()
    {
        healthBar1 = GetComponentInChildren<FloatingHealthBar>();
    }
    private void Start()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        GameObject.DontDestroyOnLoad(this.gameObject);
        originalMoveSpeed = moveSpeed;
        if (_sound == null)
        {
            Debug.Log("You haven't specified _sound through the inspector");
            this.enabled = false; //disables this script cause there is no sound loaded to play
        }

        audio = gameObject.AddComponent<AudioSource>(); //adds an AudioSource to the game object this script is attached to
        audio.playOnAwake = false;
        audio.clip = _sound;
        audio.Stop();
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        healthBar1.UpdateHealthBar(currentHP, maxHP);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = moveDirection * moveSpeed;

        LookAtMouse(); // เพิ่มฟังก์ชัน LookAtMouse() เพื่อหมุนตามทิศที่เคลื่อนที่
        healthBar1.UpdateHealthBar(currentHP, maxHP);

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (currentHP <= 0)
        {
            Die();
        }
        if (isPoisoned)
        {
            if (Time.time >= poisonTimer)
            {
                poisonTimer = Time.time + poisonInterval;
                TakeDamage(1); // ลดเลือดทีละ 1 เนื่องจากพิษ
            }

            moveSpeed = originalMoveSpeed * poisonSlowMultiplier; // ลดความเร็วเมื่อติดพิษ
        }
        else
        {
            moveSpeed = originalMoveSpeed; // คืนความเร็วเดิมเมื่อไม่ติดพิษ
        }
        if (SceneManager.GetActiveScene().name == "Snow" && !hasSnowGear)
        {
            if (!isReducingSpeed)
            {
                // หากยังไม่ได้เริ่มการลดความเร็ว ให้เริ่มต้นการลดความเร็ว
                isReducingSpeed = true;
                currentMoveSpeed = moveSpeed; // เซ็ตความเร็วปัจจุบันเป็นความเร็วปัจจุบัน
                startReducingSpeedTime = Time.time;
            }

            // คำนวณความเร็วใหม่
            float elapsedTime = Time.time - startReducingSpeedTime;
            currentMoveSpeed = Mathf.Lerp(moveSpeed, moveSpeed * slowMoveSpeedMultiplier, elapsedTime / timeToReachHalfSpeed);
            moveSpeed = currentMoveSpeed;

            // เริ่มต้นตัวจับเวลาลดเลือดเนื่องจากความเร็วช้า
            if (Time.time >= slowMoveSpeedDamageTimer)
            {
                slowMoveSpeedDamageTimer = Time.time + slowMoveSpeedDamageInterval;
                TakeDamage(slowMoveSpeedDamageAmount); // ลดเลือดเนื่องจากความเร็วช้า
            }
        }
        else if (SceneManager.GetActiveScene().name == "Sand")
        {
            // อยู่ในแมพ "Sand"
            if (!hasHeatArmor) // ถ้าไม่มีเกราะกันความร้อน
            {
                // ลดเลือดทีละ 1 หน่วย
                if (Time.time >= sandDamageTimer)
                {
                    sandDamageTimer = Time.time + sandDamageInterval;
                    TakeDamage(1); // ลดเลือดทีละ 1 หน่วยเมื่ออยู่ในแมพ "Sand"
                }
            }
        }
        else
        {
            // ไม่ใช่แมพ "Snow" หรือ "Sand" หรือมีชุดกันความเย็น ให้ใช้ความเร็วเดิม
            moveSpeed = originalMoveSpeed;

            // ถ้าเคยเริ่มการลดความเร็ว ก็ควรเริ่มต้นการลดความเร็วใหม่
            if (isReducingSpeed)
            {
                isReducingSpeed = false;
                startReducingSpeedTime = 0f;
            }

            // ไม่ต้องลดเลือด
            sandDamageTimer = Time.time;
        }
    }

    // ฟังก์ชัน LookAtMouse() ที่ใช้ในการหมุนตามเม้าส์
    private void LookAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector2 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    private void Shoot()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector2 muzzlePos = new Vector2(muzzle.position.x, muzzle.position.y);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.SetTarget(mousePosition);
        }
        Instantiate(muzzsleFlash, muzzle.position, muzzle.rotation);
        audio.Play();
        StartCoroutine(StopShootingSound());
        if (!isShooting)
        {
            StartCoroutine(CameraShake());
        }

    }

    private IEnumerator CameraShake()
    {
        isShooting = true;
        originalCameraPosition = mainCamera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < cameraShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * cameraShakeMagnitude;
            float y = Random.Range(-1f, 1f) * cameraShakeMagnitude;

            mainCamera.transform.localPosition = new Vector3(originalCameraPosition.x + x, originalCameraPosition.y + y, originalCameraPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.localPosition = originalCameraPosition;
        isShooting = false;
    }

    public void IncreaseHealth(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    public void FillHealth()
    {
        currentHP = maxHP; // เพิ่มเลือดให้เต็ม
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        healthBar1.UpdateHealthBar(currentHP, maxHP);
        hpText.text = currentHP + " / " + maxHP;
    }

    private IEnumerator StopShootingSound()
    {
        yield return new WaitForSeconds(1f);
        audio.Stop();
    }

    private void Die()
    {
        SceneManager.LoadSceneAsync(2);
        // สามารถเพิ่มโค้ดเพื่อจัดการกับการสิ้นสุดเกมตามต้องการ
    }
    public void firespeed()
    {
        fireRate = 0.2f;
    }
    public void ApplyPoison()
    {
        if (!hasPoisonImmunity)
        {
            isPoisoned = true;
            poisonTimer = Time.time + poisonInterval;
        }
    }
    public void GainPoisonImmunity()
    {
        hasPoisonImmunity = true;
        isPoisoned = false;
    }

    public void waterarmor()
    {
        bulletPrefab = bulletWater;
        hasWaterBulletItem = true;
    }
    public void firearmor()
    {
        bulletPrefab = bulletFire;
        hasFireBulletItem = true;
        hasWaterBulletItem = false;
    }

    public bool HasWaterBulletItem()
    {
        return hasWaterBulletItem;
    }

    public bool HasFireBulletItem()
    {
        return hasFireBulletItem;
    }
    public void EquipSnowGear()
    {
        // เรียกเมื่อผู้เล่นได้รับชุดกันความเย็น
        hasSnowGear = true;
    }
    public void GainHeatArmor()
    {
        hasHeatArmor = true;
        // สามารถเรียกฟังก์ชันนี้เมื่อผู้เล่นได้รับเกราะกันความร้อน
    }
    public void LoseHeatArmor()
    {
        hasHeatArmor = false;
    }
}
