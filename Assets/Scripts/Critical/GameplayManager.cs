using System.Collections;
using UnityEngine;

//this is primarily used to control the flow of the game make sure what levels youre on and difficulty levels
public class GameplayManager : MonoBehaviour
{

    public static GameplayManager instance;
    GameObject camHolder;
    GameObject player;
    PlayerStatus playerStats;
    public float camMoveSpeed, distanceFactor = 1f;
    public int nDifficulty, nOldDifficulty;
    bool hasGameStarted;
    public bool gameStop;
    public float deathCount;


    public bool suburbs, newLevel, firstPlay;
    public enum LevelState
    {
        Country = 0,
        Suburbs = 1,
        Mall = 2,
    }

    public LevelState levelState = LevelState.Country;

    private void Awake()
    {
        MakeInstance();
        camHolder = GameObject.FindGameObjectWithTag("CameraHolder");
        player = GameObject.FindGameObjectWithTag("PlayerHolder");
        playerStats = player.GetComponent<PlayerStatus>();
        nOldDifficulty = 0;
    }

    // Use this for initialization
    void Start()
    {
        hasGameStarted = true;
        StartCoroutine(CheckLevel());
    }

    private void Update()
    {

        nDifficulty = CheckDifficulty();
        if (nDifficulty != nOldDifficulty)
        {
            nOldDifficulty = nDifficulty;
            if (nDifficulty == 1)
            {
                camMoveSpeed += Time.deltaTime * 65f;
                MapGenerator.instance.minLandSpawnTimeLeft = .7f;
                MapGenerator.instance.maxLandSpawnTimeLeft = 3f;
                MapGenerator.instance.minLandSpawnTimeRight = 1.25f;
                MapGenerator.instance.maxLandSpawnTimeRight = 3.5f;

                ObstacleGenerator.instance.spawnTimeMin = 2f;
                ObstacleGenerator.instance.spawnTimeMax = 4.5f;

            }
            else if (nDifficulty == 2)
            {
                camMoveSpeed += Time.deltaTime * 125f;
                MapGenerator.instance.minLandSpawnTimeLeft = .5f;
                MapGenerator.instance.maxLandSpawnTimeLeft = 1.5f;
                MapGenerator.instance.minLandSpawnTimeRight = .5f;
                MapGenerator.instance.maxLandSpawnTimeRight = 2.5f;

                ObstacleGenerator.instance.spawnTimeMin = 0.5f;
                ObstacleGenerator.instance.spawnTimeMax = 3.2f;

            }
            else if (nDifficulty == 3)
            {
                camMoveSpeed += Time.deltaTime * 150f;
                MapGenerator.instance.minLandSpawnTimeLeft = 0.25f;
                MapGenerator.instance.maxLandSpawnTimeLeft = 1f;
                MapGenerator.instance.minLandSpawnTimeRight = 0.2f;
                MapGenerator.instance.maxLandSpawnTimeRight = 2f;

                ObstacleGenerator.instance.spawnTimeMin = 0.25f;
                ObstacleGenerator.instance.spawnTimeMax = 1.6f;
            }
        }
    }
    void FixedUpdate()
    {
        MoveCamera();
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

    }

    public void GameOver()
    {
        deathCount++;
        if (playerStats.hasDied || playerStats.explosionDeath || playerStats.stabbed && playerStats.lives < 1)
        {
            StartCoroutine(WaitForGameOverCall());
        }
    }

    IEnumerator WaitForGameOverCall()
    {
        yield return new WaitForSeconds(0.6f);
        GameMenuController.gmmInstance.GameOver();
        SavePlayerScore();
    }

    public void HandleEnd()
    {
        StopCoroutine(WaitForGameOverCall());
    }

    void MoveCamera()
    {
        if (hasGameStarted)
        {
            if (camMoveSpeed <= 3.5f)
            {
                camMoveSpeed += Time.deltaTime * 3.5f;
            }
            else
            {
                camMoveSpeed = 2f;
                hasGameStarted = false;
            }
        }
        camHolder.transform.position += new Vector3(0f, -camMoveSpeed * Time.deltaTime, 0f);
    }

    //check difficulty
    int CheckDifficulty()
    {
        int difficulty = 0;
        int podCount = ScoreManager.sInstance.podCount;
        int score = ScoreManager.sInstance.scoreValue;
        if (podCount > 10 && podCount < 20 || score >= 1500)
        {
            difficulty = 1;
        }
        else if (podCount > 25 && podCount < 5 || score >= 2500)
        {
            difficulty = 2;
        }
        else if (podCount > 75 || score >= 5000)
        {
            difficulty = 3;
        }

        return difficulty;
    }

    public void SavePlayerScore()
    {
        DataController.instance.firstPlay = false;
        if (DataController.instance.highScore < ScoreManager.sInstance.scoreValue)
        {
            DataController.instance.highScore = ScoreManager.sInstance.scoreValue;
        }

        DataController.instance.highPod += ScoreManager.sInstance.podCount;
        DataController.instance.SaveGameData();
    }

    public void CheckLeadersEnd()
    {
    }
    public void CheckAchievements()
    {

    }

    IEnumerator CheckLevel()
    {
        yield return new WaitForSeconds(.2f);
        if (ScoreManager.sInstance.scoreValue >= 1500)
        {
            if (((int)levelState) == 0)
            {
                newLevel = true;
                levelState = LevelState.Suburbs;
                CheckLevelChanged(1);
            }

            GameMenuController.gmmInstance.Text();
        }
        else
        {
            StartCoroutine(CheckLevel());
        }
    }

    void CheckLevelChanged(int level)
    {
        if (level == 1)
        {
            suburbs = true;
        }
    }
}//Class
