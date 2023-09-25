using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    private GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        item = GameObject.Find("Armor");
        item.SetActive(false);
    }
    // Update is called once per frame
    public void TrunOn()
    {
        item.SetActive(true);
    }
    void Update()
    {

    }
}