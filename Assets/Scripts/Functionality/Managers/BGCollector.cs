using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is just the background collector to destroy old screens
public class BGCollector : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D bgCollide)
    {
        if (bgCollide.tag == "Background")
        {
            Destroy(bgCollide.gameObject);
            BGSpawner.instance.bgCount--;
        }
    }
}
