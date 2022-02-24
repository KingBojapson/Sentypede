using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : MonoBehaviour
{
    Vector2 pos;

    public GameObject podExplode;

    public float amplitude = 0.25f;
    public float frequency = 1.5f;
    GameObject player;
    PlayerStatus playerStats;
    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerHolder");
        playerStats = player.GetComponent<PlayerStatus>();
        posOffset = transform.position;
        if (playerStats.hasPod)
        {
            FixPosition();
        }
    }

    private void FixPosition()
    {
        pos = new Vector2(this.transform.position.x, .15f);
        this.transform.position = pos;
    }

    private void Update()
    {
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

    public void HandleMe()
    {
        var podExplosion = Instantiate(podExplode.gameObject, transform.position, transform.rotation) as GameObject;
        podExplode.transform.position = this.transform.position;
        Destroy(podExplosion, 0.4f);
        Destroy(this.gameObject, 0.5f);
    }
}
