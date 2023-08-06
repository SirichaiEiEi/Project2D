using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deatime : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float timer = 0.4f;
    void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
}
