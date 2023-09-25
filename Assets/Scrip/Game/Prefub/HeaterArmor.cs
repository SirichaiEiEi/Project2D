using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaterArmor : MonoBehaviour
{
    private GameObject heater;
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            ArmorGear armorGear = FindObjectOfType<ArmorGear>();
            if (playerController != null)
            {
                armorGear.TurnOnObject();
                playerController.EquipSnowGear(); // เพิ่มเลือดให้เต็ม
                Destroy(gameObject); // ทำลายไอเท็มหลังจากใช้งาน
            }

        }
    }
}
