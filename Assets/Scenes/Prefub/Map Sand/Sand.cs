using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Sand : MonoBehaviour
{
    public GameObject FireBullet1;
    private bool isFireOpen = false;
    private int kill = 0;
    // Start is called before the first frame update
    void Start()
    {
        FireBullet1 = GameObject.Find("FireBullet");
        TurnOffObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnOffObject()
    {
        FireBullet1.SetActive(false);
    }
    public void FireBullet()
    {
        FireBullet1.SetActive(true);
        isFireOpen = true;
    }
    public void kills()
    {
        kill++;
        if (kill >= 50 && !isFireOpen && SceneManager.GetActiveScene().name == "Sand")
        {
            FireBullet();
        }
    }
}
