using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// camera follow player
public class FollowCamera : MonoBehaviour
{
    float followCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        followCamera = GameplayManager.instance.camMoveSpeed;
    }
}
