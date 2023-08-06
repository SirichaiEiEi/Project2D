using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar1;
    public AudioClip _sound;    // this lets you drag in an audio file in the inspector
    private AudioSource audio;
    public float moveSpeed = 5f;
    public TMP_Text hpText;
    public int maxHP = 100; // HP สูงสุดของผู้เล่น
    public float fireRate = 0.001f; // อัตราการยิง (วินาทีต่อกระสุน)
    public GameObject bulletPrefab; // โปรเฟปฟรีฟาบทหน้ากระสุน
    public Transform firePoint; // ตำแหน่งที่กระสุนจะถูกสร้าง
    public Transform muzzle;
    [SerializeField] private GameObject muzzsleFlash;

    public int currentHP; // HP ปัจจุบันของผู้เล่น
    private float nextFireTime; // เวลาถัดไปที่สามารถยิงได้

    private Rigidbody2D rb;
    private Camera mainCamera;

    private void Awake()
    {
        healthBar1 = GetComponentInChildren<FloatingHealthBar>();
    }
    private void Start()
    {
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
            audio.Play();
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        StartCoroutine(StopShootingSound());

        if (currentHP <= 0)
        {
            Die();
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
}
