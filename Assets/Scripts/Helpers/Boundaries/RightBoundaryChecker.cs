using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightBoundaryChecker : MonoBehaviour
{
    //need to access player to get rigid body in order to add force
    //This should only happen when character is in invicibility mode
    // and  resurrected mode

    GameObject myPlayer;
    Rigidbody2D playerRB;
    PlayerMovement pm;
    public GameObject dustCloudRight;

    float force = 800f;
    float podForce = .65f;

    void Awake()
    {
        myPlayer = GameObject.FindGameObjectWithTag("PlayerHolder");
        pm = myPlayer.GetComponent<PlayerMovement>();
        playerRB = myPlayer.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D right)
    {

        if (right.tag == "RightBourndary")
        {
            playerRB.AddForce(-transform.right * force, ForceMode2D.Force);
            foreach (var pod in pm.podContainer)
            {
                if (pod.tag == "Head")
                {
                    continue;
                }
                else
                {
                    var podRB = pod.gameObject.GetComponent<Rigidbody2D>();
                    podRB.AddForce(-transform.right * podForce, ForceMode2D.Force);
                }
            }
            var dust = Instantiate(dustCloudRight, myPlayer.transform.position, myPlayer.transform.rotation);
            Destroy(dust, 0.5f);
        }

    }
}