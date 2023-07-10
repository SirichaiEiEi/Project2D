using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // อ้างอิงถึง Slider ของหลอดเลือด

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth; // ตั้งค่าค่าสูงสุดของ Slider
        slider.value = maxHealth; // ตั้งค่าค่าปัจจุบันของ Slider
    }

    public void SetHealth(int health)
    {
        slider.value = health; // ตั้งค่าค่าปัจจุบันของ Slider
    }
}


