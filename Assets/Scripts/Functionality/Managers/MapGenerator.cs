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


    float distanceBetweenLands, distanceBetweenObjects;
    public GameObject[] tutlands;
    public GameObject[] tutbabyLands;

    //Suburb spawn section
    public GameObject[] subLands;
    public GameObject[] subBabyLands;

    public float minLandSpawnTimeLeft, maxLandSpawnTimeLeft, minLandSpawnTimeRight, maxLandSpawnTimeRight;
    public float miniLandSpawnLeft, miniLandSpawnRight;

    float minLeftLandMoveX;
    float minLandMoveY;

    float leftXPos;
    float rightXPos;
    float midXPos;

    int orderInLayerLeft;
    int orderInLayerRight;
    Vector2 desiredPosLeft;
    Vector2 desiredPosMid;
    Vector2 desiredPosRight;
    float heightBetween;

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
        if (GameplayManager.instance.suburbs == false)
        {
            if (minLeftLandMoveX >= -0.7 || minLeftLandMoveX >= -.6f)
            {
                var mid = Instantiate(tutbabyLands[Random.Range(0, tutbabyLands.Length)], babySpawn.transform.position, babySpawn.transform.rotation);
                midXPos = Random.Range(-2.0f, 2.5f);
                desiredPosMid = new Vector2(midXPos, babySpawn.position.y);
                babySpawn.transform.position = desiredPosMid;
                StartCoroutine(MiddleLandSpawner());
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
    }

    IEnumerator EarlySpawnLeft()
    {

        if (GameplayManager.instance.suburbs)
        {
            minLandSpawnTimeRight = .5f;
            maxLandSpawnTimeRight = 6f;

            yield return new WaitForSeconds(Random.Range(minLandSpawnTimeLeft, maxLandSpawnTimeLeft));
            var leftSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnLeft.transform.position, spawnLeft.transform.rotation) as GameObject;
            minLeftLandMoveX = leftSide.transform.position.x;
            minLandMoveY = leftSide.transform.position.y;
            Debug.Log("Left Side " + leftSide.name + minLeftLandMoveX);
            //This is telling spawn location to float to the right
            leftXPos = Random.Range(-4.9f, -.87f);
            desiredPosLeft = new Vector2(leftXPos, spawnLeft.position.y);
            spawnLeft.transform.position = desiredPosLeft;
        }
        else
        {
            minLandSpawnTimeLeft = 1f;
            maxLandSpawnTimeLeft = 8f;
            var leftSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnLeft.transform.position, spawnLeft.transform.rotation);
            minLeftLandMoveX = leftSide.transform.position.x;
            minLandMoveY = leftSide.transform.position.y;
            Debug.Log("Left Side " + leftSide.name + minLeftLandMoveX);
            leftXPos = Random.Range(-4.50f, -0.35f);
            desiredPosLeft = new Vector2(leftXPos, spawnLeft.position.y);
            spawnLeft.transform.position = desiredPosLeft;
        }
        yield return new WaitForSeconds(Random.Range(minLandSpawnTimeLeft, maxLandSpawnTimeLeft));
        StartCoroutine(EarlySpawnLeft());

    }//EarlySpawn

    IEnumerator EarlySpawnRight()
    {
        if (GameplayManager.instance.suburbs)
        {
            minLandSpawnTimeRight = .5f;
            maxLandSpawnTimeRight = 6f;

            if (minLeftLandMoveX >= -0.7 || minLeftLandMoveX >= -.6f)
            {
                rightXPos = Random.Range(5.3f, 5.8f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                //we will check the height difference between height of left to right
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }

            }
            else if (minLeftLandMoveX >= -1f)
            {
                rightXPos = Random.Range(5.3f, 5.8f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else if (minLeftLandMoveX >= -1.9)
            {
                rightXPos = Random.Range(4.55f, 5.48f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else if (minLeftLandMoveX >= -2.75f)
            {
                rightXPos = Random.Range(3.57f, 5.48f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else if (minLeftLandMoveX >= -3.5f)
            {
                rightXPos = Random.Range(3.2f, 5.48f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else if (minLeftLandMoveX >= -3.77f)
            {
                rightXPos = Random.Range(2.7f, 5.48f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else
            {
                rightXPos = Random.Range(.65f, 5f);
                var rightSide = Instantiate(subLands[Random.Range(0, subLands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                spawnRight.position = new Vector2(spawnRight.position.x, spawnRight.position.y - 5);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y - 2);
                spawnRight.transform.position = desiredPosRight;
            }
        }
        else
        {
            minLandSpawnTimeRight = 1f;
            maxLandSpawnTimeRight = 8f;

            if (minLeftLandMoveX >= -0.7 || minLeftLandMoveX >= -.6f)
            {
                rightXPos = Random.Range(5.3f, 5.8f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else if (minLeftLandMoveX >= -1.9)
            {
                rightXPos = Random.Range(4.55f, 5.48f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else if (minLeftLandMoveX >= -2.75f)
            {
                rightXPos = Random.Range(3.57f, 5.48f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else if (minLeftLandMoveX >= -3.5f)
            {
                rightXPos = Random.Range(3.2f, 5.48f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else if (minLeftLandMoveX >= -3.77f)
            {
                rightXPos = Random.Range(2.7f, 5.48f);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y);
                heightBetween = desiredPosRight.y + minLandMoveY;
                if (heightBetween <= 2.3f)
                {
                    spawnRight.transform.position = new Vector2(rightXPos, heightBetween - 1f);
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
                else
                {
                    var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                }
            }
            else
            {
                rightXPos = Random.Range(.65f, 5f);
                var rightSide = Instantiate(tutlands[Random.Range(0, tutlands.Length)], spawnRight.transform.position, spawnRight.transform.rotation);
                spawnRight.position = new Vector2(spawnRight.position.x, spawnRight.position.y - 5);
                desiredPosRight = new Vector2(rightXPos, spawnRight.position.y - 2);
                spawnRight.transform.position = desiredPosRight;
            }

        }

        yield return new WaitForSeconds(Random.Range(minLandSpawnTimeRight, maxLandSpawnTimeRight));
        StartCoroutine(EarlySpawnRight());
    }
}//Class
