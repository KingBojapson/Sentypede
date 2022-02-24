using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlick : MonoBehaviour {

    AreaEffector2D area;
    GameObject player;
    Transform getPlayer;
    PlayerMovement playMove;

    private void Awake()
    {
        area = this.GetComponent<AreaEffector2D>();
        player = GameObject.FindGameObjectWithTag("PlayerHolder");
        playMove = player.GetComponent<PlayerMovement>();
    }

	// Update is called once per frame
	void Update () 
    {
        if (playMove.havePressedButton) 
        {
            area.forceMagnitude = -95;
        }
        else 
        {
            area.forceMagnitude = 95;
        }
	}
}
