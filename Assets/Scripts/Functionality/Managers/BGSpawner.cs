using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the background spawner it literally spawns background images
public class BGSpawner : MonoBehaviour
{

    public static BGSpawner instance;
    public int bgCount;
    public GameObject[] tutBacks;
    public GameObject[] subBacks;
    GameObject myBG;
    GameObject nextBG;
    GameObject bgHolder;
    private BoxCollider2D boxC;
    private float lastY;

    private void Awake()
    {
        MakeInstance();

        myBG = Instantiate(tutBacks[Random.Range(0, tutBacks.Length)], new Vector2(this.transform.position.x, this.transform.position.y + 10), this.transform.rotation);
        bgCount++;
        boxC = myBG.GetComponent<BoxCollider2D>();

        float height = boxC.size.y;
        lastY = height + 3;

        nextBG = Instantiate(tutBacks[Random.Range(0, tutBacks.Length)], new Vector2(myBG.transform.position.x, myBG.transform.position.y - lastY), myBG.transform.rotation);
        bgCount++;
        boxC = nextBG.GetComponent<BoxCollider2D>();
        height = boxC.size.y;

        lastY = height + 3;
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

    }//Instance

    private void Update()
    {
        if (bgCount <= 1)
        {
            if (GameplayManager.instance.suburbs)
            {
                nextBG = Instantiate(subBacks[Random.Range(0, subBacks.Length)], new Vector2(nextBG.transform.position.x, nextBG.transform.position.y - lastY + .1f), nextBG.transform.rotation);
            }
            else
            {
                nextBG = Instantiate(tutBacks[Random.Range(0, tutBacks.Length)], new Vector2(nextBG.transform.position.x, nextBG.transform.position.y - lastY), nextBG.transform.rotation);
            }
            bgCount++;
            boxC = nextBG.GetComponent<BoxCollider2D>();
            lastY = boxC.size.y + 3;
        }
    }
}
