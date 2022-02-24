using UnityEngine;

public class Venom : MonoBehaviour {

    public GameObject venomSplat;

    void OnTriggerEnter2D(Collider2D col) 
    { 
        if(col.tag == "Player" || col.tag == "Barriers") 
        {
            var splat = Instantiate(venomSplat, transform.position, transform.rotation) as GameObject;
            Destroy(splat.gameObject, 0.35f);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject, 0.5f);
        }
    }
}
