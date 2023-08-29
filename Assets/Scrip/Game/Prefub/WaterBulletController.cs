using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterBulletController : MonoBehaviour
{
    public float speed = 10f;
    public float destroyDelay = 5f;
    public GameObject hitEffect;
    private Vector3 targetPosition;
    private int damageAmount;
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

                monsterController.TakeDamage(damageAmount); // Regular damage

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
