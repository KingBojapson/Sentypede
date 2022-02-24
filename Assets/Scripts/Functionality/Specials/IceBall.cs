using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    SpriteRenderer iceballRender;
    Transform iceBlast;
    // Start is called before the first frame update
    void Awake()
    {
        iceballRender = this.GetComponent<SpriteRenderer>();
        iceBlast = this.transform.GetChild(1);
    }

    void OnTriggerEnter2D(Collider2D handle)
    {
        if (handle.tag == "Obstacle" ||
        handle.tag == "spike" ||
        handle.tag == "Cone" || handle.tag == "Crate" || handle.tag == "Land" ||
        handle.tag == "skull" || handle.tag == "explosive-obstacle" || handle.tag == "SpiderLand" ||
        handle.tag == "grassland")
        {
            Destroy(handle.gameObject);
            iceballRender.enabled = false;
            iceBlast.gameObject.SetActive(true);

            Destroy(this.gameObject, 0.35f);
        }
    }
}
