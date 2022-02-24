using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public GameObject spikeParticle;
    ParticleSystem spikeParticleSystem;
    // Start is called before the first frame update

    void Awake()
    {
        spikeParticleSystem = spikeParticle.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule pMain = spikeParticleSystem.main;
        ChangeParticleColor(pMain);
    }

    //THis will change particle color based on selected choice of character
    public void ChangeParticleColor(ParticleSystem.MainModule pMain)
    {
        int myChoice = DataController.instance.selectedIndex;

        switch (myChoice)
        {
            case 0:
                pMain.startColor = Color.white;
                break;
            case 1:
                pMain.startColor = Color.blue;
                break;
            case 2:
                pMain.startColor = Color.blue;
                break;
            case 3:
                pMain.startColor = new Color(253, 194, 194, 255);
                break;
            case 4:
                pMain.startColor = new Color(142, 172, 25, 255);
                break;
            case 5:
                pMain.startColor = new Color(217, 150, 214, 255);
                break;
            case 6:
                pMain.startColor = new Color(185, 242, 255, 255);
                break;
        }
    }
    // Update is called once per frame
    public void HandleMe()
    {
        SoundManager.soundInstance.SpikeHit();
        var spikePop = Instantiate(spikeParticle.gameObject, this.transform.position, this.transform.rotation) as GameObject;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        CameraShake.cmInstance.Shake(.15f, .12f);
        Destroy(spikePop.gameObject, 0.6f);
        Destroy(this.gameObject, 0.7f);
    }
}
