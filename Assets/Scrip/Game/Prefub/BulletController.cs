using System.Collections;
using System.Collections.Generic;
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
            if (monsterController != null)
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);
                monsterController.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
    }


    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        damageAmount = Random.Range(1, 11); // สุ่มค่าดาเมทในช่วง 1-10
    }
}




