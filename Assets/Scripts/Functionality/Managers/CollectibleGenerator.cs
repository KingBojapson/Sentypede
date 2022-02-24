using System.Collections;
using UnityEngine;


// this is my pickup spawner we literally just deploy pickups
public class CollectibleGenerator : MonoBehaviour
{

    public static CollectibleGenerator colInstance;
    public GameObject[] colHolders;
    GameObject mCamera;
    Transform colSpawner;

    Vector2 landPosLeft;
    Vector2 landPosRight;

    public float spawnTimeMin;
    public float spawnTimeMax;

    public float estimatedPos;
    float finalColPos;
    Vector2 colFinalPos;

    //Pod Locations

    //just getting access to the sprite of holders child
    Transform holdersChild;
    // Use this for initialization
    private void Awake()
    {
        MakeInstance();
        mCamera = GameObject.FindWithTag("MainCamera");
        colSpawner = mCamera.transform.GetChild(8);
        StartCoroutine(FirstSpawnObject());
    }

    void MakeInstance()
    {
        if (colInstance == null)
        {
            if (colInstance == null)
            {
                colInstance = this;
            }
            else if (colInstance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        landPosLeft = MapGenerator.instance.clonedLeftPos;
        landPosRight = MapGenerator.instance.clonedRightPos;
    }



    IEnumerator FirstSpawnObject()
    {
        yield return new WaitForSeconds(Random.Range(spawnTimeMin, spawnTimeMax));
        //based on the locations of the land objects will determine where to lay the obstacles and collectibles
        if (estimatedPos <= -2.5f)
        {
            finalColPos = Random.Range(estimatedPos + .6f, 2f);
            colFinalPos = new Vector2(finalColPos, colSpawner.position.y);
            colSpawner.transform.position = colFinalPos;
        }
        else if (estimatedPos >= -2.5)
        {
            finalColPos = Random.Range(estimatedPos + -2.5f, 2.9f);
            colFinalPos = new Vector2(finalColPos, colSpawner.position.y);
            colSpawner.transform.position = colFinalPos;
        }
        var col = Instantiate(colHolders[Random.Range(0, colHolders.Length)], colSpawner.transform.position, colSpawner.transform.rotation);
        StartCoroutine(FirstSpawnObject());
    }
}
