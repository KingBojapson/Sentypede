using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



// This spawns many obstacles in the game
public class ObstacleGenerator : MonoBehaviour
{

    //This is where we check
    public static ObstacleGenerator instance;
    public GameObject[] obsHolders;
    GameObject mCamera;
    Transform obstacleSpawner;

    [SerializeField]
    Vector2 landPosLeft;
    [SerializeField]
    Vector2 landPosRight;

    public float spawnTimeMin;
    public float spawnTimeMax;

    public float estimatedPos;
    float finalObstaclePos;



    Vector2 obstacleFinalPos;

    private void Awake()
    {
        MakeInstance();
        mCamera = GameObject.FindWithTag("MainCamera");
        obstacleSpawner = mCamera.transform.GetChild(7);
        StartCoroutine(FirstSpawnObject());
    }
    // Update is called once per frame
    void Update()
    {
        // This is checking the location of the the land objects
        landPosLeft = MapGenerator.instance.clonedLeftPos;
        landPosRight = MapGenerator.instance.clonedRightPos;

        estimatedPos = landPosLeft.x + landPosRight.x;
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

    IEnumerator FirstSpawnObject()
    {
        yield return new WaitForSeconds(Random.Range(spawnTimeMin, spawnTimeMax));

        //based on the locations of the land objects will determine where to lay the obstacles and collectibles
        if (estimatedPos <= -2.5f)
        {
            finalObstaclePos = Random.Range(estimatedPos + .6f, 2f);
            obstacleFinalPos = new Vector2(finalObstaclePos, obstacleSpawner.position.y);
            obstacleSpawner.transform.position = obstacleFinalPos;
        }
        else if (estimatedPos >= -2.5)
        {
            finalObstaclePos = Random.Range(estimatedPos + -2.5f, 2.9f);
            obstacleFinalPos = new Vector2(finalObstaclePos, obstacleSpawner.position.y);
            obstacleSpawner.transform.position = obstacleFinalPos;
        }
        var obstacle = Instantiate(obsHolders[Random.Range(0, obsHolders.Length)], obstacleSpawner.transform.position, obstacleSpawner.transform.rotation);
        StartCoroutine(FirstSpawnObject());
    }

    public void StopFirstSpawn()
    {

    }
}
