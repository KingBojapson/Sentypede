using UnityEngine;

public class TNT : MonoBehaviour
{
    public GameObject tntExplosion;

    public void Explosive()
    {
        SoundManager.soundInstance.TNTSound();
        var explosion = Instantiate(tntExplosion.gameObject, this.transform.position, this.transform.rotation) as GameObject;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        CameraShake.cmInstance.Shake(.25f, .22f);
        Destroy(explosion.gameObject, 0.5f);
        Destroy(this.gameObject, 0.5f);
    }
}
