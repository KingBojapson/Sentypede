using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager sInstance;

    //The Number Objects
    public GameObject[] nums;
    public GameObject[] destructiveNums;

    GameObject player;
    PlayerStatus playStatus;

    //Score Achievements
    public int starCount;
    public int podCount;
    public int landCount;
    public int spiderKillCount;
    public int scoreValue;
    int newPodCount;
    public int invincibleCount;

    //For The Combo
    bool comboActivated;
    public bool highScoreText;
    int multiplierAmount = 0;
    float comboActivateTime = 1.5f;
    float resetComboATime = 1.5f;

    // Use this for initialization
    private void Awake()
    {
        MakeInstance();
        StartCoroutine(ScoreValue(1));
        StartCoroutine(CheckHighScore());
        player = GameObject.FindGameObjectWithTag("PlayerHolder");
        playStatus = player.GetComponent<PlayerStatus>();
    }

    void MakeInstance()
    {
        if (sInstance == null)
        {
            sInstance = this;
        }
        else if (sInstance != null)
        {
            Destroy(gameObject);
        }

    }
    // Update is called once per frame
    void Update()
    {
        starCount = playStatus.starCounter;
    }

    public IEnumerator ScoreValue(int scored)
    {
        yield return new WaitForSeconds(1.0f);
        if (multiplierAmount < 1)
        {
            scoreValue++;
        }
        else
        {
            scoreValue = scoreValue + scored;
        }
        StartCoroutine(ScoreValue(1));
    }
    public void CheckWhatFruitIAm(int myYum)
    {
        switch (myYum)
        {
            case 2:
                var fNum1 = Instantiate(nums[3], player.transform.position, player.transform.rotation);
                scoreValue = scoreValue + 200;
                GameMenuController.gmmInstance.CheckSpecialMove(200f);
                Destroy(fNum1.gameObject, 0.5f);
                break;
            case 3:
                var fNum2 = Instantiate(nums[2], player.transform.position, player.transform.rotation);
                scoreValue = scoreValue + 100;
                GameMenuController.gmmInstance.CheckSpecialMove(100f);
                Destroy(fNum2.gameObject, 0.5f);
                break;
            case 4:
                var fNum3 = Instantiate(nums[5], player.transform.position, player.transform.rotation);
                scoreValue = scoreValue + 400;
                GameMenuController.gmmInstance.CheckSpecialMove(400f);
                Destroy(fNum3.gameObject, 0.5f);
                break;
        }
    }

    IEnumerator CheckHighScore()
    {
        yield return new WaitForSeconds(.2f);
        if (scoreValue > DataController.instance.highScore)
        {
            highScoreText = true;

            if (highScoreText)
            {
                GameMenuController.gmmInstance.Text();
            }
        }
        else
        {
            StartCoroutine(CheckHighScore());
        }
    }
}
