using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MonsterManager : MonoBehaviour
{
    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    private int monstersKilled1 = 0;
    public TMP_Text point;

    private int points = 0; 


    // ...

    public void MonsterKilled1()
    {
        monstersKilled1++;
        points++;

        if (monstersKilled1 >= 20)
        {
            spawner1.SetActive(false);
            spawner2.SetActive(true);
        }
        if (monstersKilled1 >= 30)
        {
            spawner2.SetActive(false);
            spawner3.SetActive(true);
        }
    }

    public int GetKilledMonstersCount()
    {
        return monstersKilled1;
    }

    private void Update()
    {
        point.text = points + "";
    }

    // ...
}