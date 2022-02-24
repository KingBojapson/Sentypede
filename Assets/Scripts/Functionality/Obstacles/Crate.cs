using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject crateExplosion;


    public void DestroyCrate()
    {
        SoundManager.soundInstance.TNTSound();
        var explosion = Instantiate(crateExplosion.gameObject, this.transform.position, this.transform.rotation) as GameObject;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        CameraShake.cmInstance.Shake(.25f, .22f);
        Destroy(explosion.gameObject, 0.4f);
        Destroy(this.gameObject, 0.4f);
    }
}
