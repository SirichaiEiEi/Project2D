using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntidoteItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.CollectAntidote(); // กันพิษ
                Destroy(gameObject); // ทำลายไอเท็มหลังจากใช้งาน
            }
        }
    }
}
