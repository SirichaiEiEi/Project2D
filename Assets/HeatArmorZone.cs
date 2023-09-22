using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatArmorZone : MonoBehaviour
{
    private bool isInHeatZone = false;
    public GameObject heater;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ผู้เล่นอยู่ในพื้นที่ของเกราะ
            isInHeatZone = true;
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                heater.SetActive(true);
                player.GainHeatArmor(); // เปิดเกราะกันความร้อน
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ผู้เล่นออกจากพื้นที่ของเกราะ
            isInHeatZone = false;
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                heater.SetActive(false);
                player.LoseHeatArmor(); // ปิดเกราะกันความร้อน
            }
        }
    }
}
