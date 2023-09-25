using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ArmorGear : MonoBehaviour
{
    private GameObject myObject; // เก็บ Gameobject ที่คุณต้องการควบคุม
    private GameObject item;
    private int kill = 0;
    private bool isGearOpen = false;
    private void Start()
    {
        // กำหนด Gameobject ที่คุณต้องการควบคุม
        myObject = GameObject.Find("Armor");
        item = GameObject.Find("Heater");
        // เรียกใช้ฟังก์ชั่นเพื่อปิด Gameobject ตอนเริ่มเกม
        TurnOffObject();
    }

    // ฟังก์ชั่นเพื่อเปิด Gameobject
    public void TurnOnObject()
    {
        myObject.SetActive(true);
    }

    // ฟังก์ชั่นเพื่อปิด Gameobject
    public void TurnOffObject()
    {
        myObject.SetActive(false);
        item.SetActive(false);
    }

    public void snowGear()
    {
        isGearOpen = true;
        item.SetActive(true);
    }

    public void kills()
    {
        kill++;
        if (kill >= 1 && !isGearOpen && SceneManager.GetActiveScene().name == "Snow")
        {
            snowGear();
        }
    }
}
