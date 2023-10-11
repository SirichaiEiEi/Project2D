using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f; // ความเร็วของกระสุน
    public float destroyDelay = 5f; // เวลาที่เกิดกระสุน (วินาที)
    public GameObject hitEffect;
    private Vector3 targetPosition;
    private int damageAmount; // จำนวนดาเมทที่กระสุนจะส่งให้มอนสเตอร์
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = (targetPosition - transform.position).normalized * speed;

        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            MonsterController monsterController = collision.GetComponent<MonsterController>();
            MosterRanger mosterRanger = collision.GetComponent<MosterRanger>();
            if (monsterController != null || mosterRanger != null)
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);

                int modifiedDamageAmount = damageAmount;

                PlayerController playerController = FindObjectOfType<PlayerController>();
                if (playerController != null && playerController.HasWaterBulletItem() && SceneManager.GetActiveScene().name == "Sand")
                {
                    modifiedDamageAmount *= 2; // Double damage
                }
                if (playerController != null && playerController.HasFireBulletItem() && SceneManager.GetActiveScene().name == "Snow")
                {
                    modifiedDamageAmount *= 2;
                }

                if (monsterController != null)
                {
                    monsterController.TakeDamage(modifiedDamageAmount);
                }
                if (mosterRanger != null)
                {
                    mosterRanger.TakeDamage(modifiedDamageAmount);
                }
            }

            Destroy(gameObject);
        }
    }



    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        damageAmount = 10; // ดาเมท 10
    }
}





