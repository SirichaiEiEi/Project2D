using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterManager : MonoBehaviour
{
    public GameObject spawner1;
    public GameObject spawner2;
    private int monstersKilled1 = 0;


    // ...

    public void MonsterKilled1()
    {
        monstersKilled1++;

        if (monstersKilled1 >= 20)
        {
            spawner1.SetActive(false);
            spawner2.SetActive(true);
        }
    }

    // ...
}