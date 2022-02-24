using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSoundAlive : MonoBehaviour
{

    public static KeepSoundAlive soundHolders;
    // Use this for initialization
    void Start()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (soundHolders == null)
        {
            soundHolders = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (soundHolders != null)
        {
            Destroy(gameObject);
        }

    }
}
