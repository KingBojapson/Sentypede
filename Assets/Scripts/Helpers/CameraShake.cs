using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static CameraShake cmInstance;
    public Camera mainCam;
    Transform myParent;

    float shakeAmount = 0;

    private void Awake()
    {
        MakeInstance();
        myParent = this.transform.parent;
        if(mainCam == null) 
        {
            mainCam = Camera.main; 
        }
    }

    void MakeInstance() 
    {
      if (cmInstance == null)
      {
        cmInstance = this;
      }
       else if (cmInstance != null)
      {
        Destroy(gameObject);
      }
    }
    public void Shake(float amt, float length) 
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("EndShake", length);
    }

    void BeginShake() 
    { 
        if(shakeAmount > 0) 
        {
            Vector3 camPos = mainCam.transform.position;
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
        }
    }

    void EndShake() 
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = myParent.transform.localPosition;
    }
}
