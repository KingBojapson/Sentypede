using System.Collections;
using UnityEngine;

// Probably should change this to landGenerator this literally spawns and places the land objects in different patterns
public class MapGenerator : MonoBehaviour
{

    public static MapGenerator instance;

    SpriteRenderer[] getSprites;

    [SerializeField]
    GameObject camera;

    [SerializeField]
    Transform spawnLeft, spawnRight, babySpawn;

    public GameObject[] tutlands;
    public GameObject[] tutbabyLands;

    //Suburb spawn section
    public GameObject[] subLands;
    public GameObject[] subBabyLands;

    public float minLandSpawnTimeLeft, maxLandSpawnTimeLeft, minLandSpawnTimeRight, maxLandSpawnTimeRight;
    public float miniLandSpawnLeft, miniLandSpawnRight;

    float minLandMoveX, maxLandMoveX;
    float minLandMoveRightX, maxLandMoveRightX;

    float leftXPos;
    float rightXPos;
    float midXPos;

    int orderInLayerLeft;
    int orderInLayerRight;
    Vector2 desiredPosLeft;
    Vector2 desiredPosMid;
    Vector2 desiredPosRight;

    //This will get the current Vector2 position of cloned object
    public Vector2 clonedLeftPos;
    public Vector2 clonedRightPos;
    // Use this for initialization

    void Awake()
    {
        MakeInstance();
        camera = GameObject.FindWithTag("MainCamera");
        spawnLeft = camera.transform.GetChild(4);
        spawnRight = camera.transform.GetChild(5);
        babySpawn = camera.transform.GetChild(8);
    }
    void Start()
    {
        StartCoroutine(EarlySpawnLeft());
        StartCoroutine(EarlySpawnRight());
        StartCoroutine(MiddleLandSpawner());
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

    IEnumerator MiddleLandSpawner()
    {
        yield return new WaitForSeconds(Random.Range(miniLandSpawnLeft, miniLandSpawnRight));
        if (GameplayManager.instance.suburbs == true)
        {

        }
        else
        {
            var mid = Instantiate(tutbabyLands[Random.Range(0, tutbabyLands.Length)], babySpawn.transform.position, babySpawn.transform.rotation);
            midXPos = Random.Range(-2.0f, 2.5f);
            desiredPosMid = new Vector2(midXPos, babySpawn.position.y);
            babySpawn.transform.position = desiredPosMid;
            StartCoroutine(MiddleLandSpawner());
        }

    }

    IEnumerator EarlySpawnLeft()
    {
        if (GameplayManager.instance.suburbs)
        {
            minLandSpawnTimeLeft = 0f;
            maxLandSpawnTimeLeft = 7f;
            yield return new WaitForSeconds(Random.Range(minLandSpawnTimeLeft, maxLandSpawnTimeLeft));
            var leftSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnLeft.transform.position, spawnLeft.transform.rotation);

            leftXPos = Random.Range(-4.90f, -1.75f);
            if (spawnRight.position.x <= 2.90)
            {
                leftXPos = Random.Range(-4.90f, -3.87f);
                desiredPosLeft = new Vector2(leftXPos, spawnLeft.position.y);
                spawnLeft.transform.position = desiredPosLeft;
            }
            desiredPosLeft = new Vector2(leftXPos, spawnLeft.position.y);
            spawnLeft.transform.position = desiredPosLeft;
        }
        else
        {
            minLandSpawnTimeLeft = 0f;
            maxLandSpawnTimeLeft = 7f;
            yield return new WaitForSeconds(Random.Range(minLandSpawnTimeLeft, maxLandSpawnTimeLeft));
            var leftSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnLeft.transform.position, spawnLeft.transform.rotation);
            leftXPos = Random.Range(-4.90f, -1.75f);
            if (spawnRight.position.x <= 2.90)
            {
                leftXPos = Random.Range(-4.90f, -3.87f);
                desiredPosLeft = new Vector2(leftXPos, spawnLeft.position.y);
                spawnLeft.transform.position = desiredPosLeft;
            }
            desiredPosLeft = new Vector2(leftXPos, spawnLeft.position.y);
            spawnLeft.transform.position = desiredPosLeft;
        }
        StartCoroutine(EarlySpawnLeft());

    }//EarlySpawn

    IEnumerator EarlySpawnRight()
    {
        if (GameplayManager.instance.suburbs)
        {
            minLandSpawnTimeRight = 2.50f;
            maxLandSpawnTimeRight = 9.50f;
            yield return new WaitForSeconds(Random.Range(minLandSpawnTimeRight, maxLandSpawnTimeRight));
            var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
            rightXPos = Random.Range(3.90f, 6.5f);
            if (spawnLeft.position.x <= -1.3f)
            {
                rightXPos = Random.Range(2.79f, 6.00f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                spawnRight.transform.position = desiredPosRight;
            }
            desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
            spawnLeft.transform.position = desiredPosRight;
        }
        else
        {
            minLandSpawnTimeRight = 2.50f;
            maxLandSpawnTimeRight = 9.5f;
            yield return new WaitForSeconds(Random.Range(minLandSpawnTimeRight, maxLandSpawnTimeRight));
            var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
            rightXPos = Random.Range(3.90f, 6.5f);
            if (spawnLeft.position.x <= -1.3f)
            {
                rightXPos = Random.Range(2.79f, 6.00f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                spawnRight.transform.position = desiredPosRight;
            }
            //try to avoid lands being too close
            if (spawnRight.position.y - spawnLeft.position.y <= 5)
            {
                if (spawnLeft.position.x >= -3)
                {
                    rightXPos = Random.Range(2.71f, 5.58f);
                    spawnRight.position = new Vector2(spawnRight.position.x, spawnRight.position.y - 5);
                    desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                    spawnRight.transform.position = desiredPosRight;
                }
                else if (spawnLeft.position.x >= -1.32)
                {
                    rightXPos = Random.Range(2.95f, 5.58f);
                    spawnRight.position = new Vector2(spawnRight.position.x, spawnRight.position.y - 5);
                    desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                    spawnRight.transform.position = desiredPosRight;
                }
                else if (spawnLeft.position.x >= -1.76)
                {
                    rightXPos = Random.Range(4.3f, 5.58f);
                    spawnRight.position = new Vector2(spawnRight.position.x, spawnRight.position.y - 5);
                    desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                    spawnRight.transform.position = desiredPosRight;
                }
                spawnRight.position = new Vector2(spawnRight.position.x, spawnRight.position.y - 5);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y - 2);
                spawnRight.transform.position = desiredPosRight;
            }
        }
        StartCoroutine(EarlySpawnRight());
    }
}//Class
