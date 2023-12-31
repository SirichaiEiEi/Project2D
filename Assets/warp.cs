﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class warp : MonoBehaviour
{
    private GameObject Armor;
    // Start is called before the first frame update
    private void Start()
    {
        Armor = GameObject.Find("Armor");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                LoadNextLevel();
                if (Armor != null)
                {
                    Armor.SetActive(true);
                }
            }
        }
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // หากเป็นด่านสุดท้ายแล้ว โหลดด่านแรกใหม่
            SceneManager.LoadScene(0);
        }
    }

    // โค้ดอื่น ๆ ที่ไม่เกี่ยวข้อง
}


