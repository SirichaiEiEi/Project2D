using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject Resure;
    public GameObject Exit;
    public GameObject Setting;
    public GameObject Restart;

    private bool isGamePaused = false; // เก็บสถานะการพักเกม

    private void Start()
    {
        Resure.SetActive(false);
        Setting.SetActive(false);
        Exit.SetActive(false);
        Restart.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Resure.SetActive(true);
        Setting.SetActive(true);
        Exit.SetActive(true);
        Restart.SetActive(true);
        Time.timeScale = 0f; // หยุดเวลาเกม
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        Resure.SetActive(false);
        Setting.SetActive(false);
        Exit.SetActive(false);
        Restart.SetActive(false);
        Time.timeScale = 1f; // เริ่มเวลาเกมอีกครั้ง
        isGamePaused = false;
    }

    public void ExitGame()
    {
        Application.Quit(); // ออกจากเกม
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu2"); // โหลดหน้าเมนูหลักหรือหน้าเริ่มเกม
    }

    public void RestartGame()
    {
        // เริ่มเกมใหม่ โดยโหลดฉากที่คุณต้องการ (เช่น "GameplayScene")
        SceneManager.LoadScene("Madow");
    }
}
