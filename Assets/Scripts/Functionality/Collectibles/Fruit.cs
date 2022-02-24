using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    public GameObject fruitExplosion;
    Transform internalFruit;

    SpriteRenderer fruitRender;

    void Awake()
    {
        internalFruit = this.transform.GetChild(0);
        fruitRender = internalFruit.GetComponent<SpriteRenderer>();
    }

    public void HandleDeath()
    {
        fruitRender.enabled = false;
        var fruitExplode = Instantiate(fruitExplosion, transform.position, transform.rotation);
        fruitExplode.transform.position = this.transform.position;
        Destroy(fruitExplode, 0.35f);
        Destroy(this.gameObject, 0.35f);
    }
}
