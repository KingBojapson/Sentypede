using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public GameObject landExplode;
    public void HandleMe()
    {

        var landExplosion = Instantiate(landExplode.gameObject, transform.position, transform.rotation);
        landExplode.transform.position = this.transform.position;
        CameraShake.cmInstance.Shake(.15f, .12f);
        Destroy(landExplosion, 0.2f);
        Destroy(this.gameObject, 0.35f);
    }
}
