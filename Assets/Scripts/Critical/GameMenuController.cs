using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//This is game menu portion like the pause button and activates text in the menu
public class GameMenuController : MonoBehaviour
{

    public static GameMenuController gmmInstance;

    public GameObject rewardFlash;
    public GameObject[] splats;
    public Transform[] splatLocations;
    public GameObject[] numImages;
    [Header("In Game Score Board")]
    public Text podScore;
    public Text score;

    public bool reset;

    public GameObject[] lives;

    public GameObject reverseText;
    public GameObject poisonText;
    public GameObject invincibleText;
    public GameObject highScoreText;
    public GameObject specialMoveText;
    public GameObject levelChanged;

    [Header("The Menu's")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject secondPanel;
    public GameObject adGamePanel;
    public GameObject tutorialPanel;

    [Header("The Menu Scores")]
    public Text pauseHighscore;
    public Text pauseCurrentScore;

    [Header("Game Over Stuff")]
    public Text gameOverHighscore;
    public Text gameOverCurrentScore;
    public Text gameOverPods;

    int myLoc;
    GameObject player;
    Transform internalP;
    //Tutorial Segments
    Transform tutorialReminders;
    Transform tutorialPops;
    Transform ObjectivePops;
    Transform WelcomePops;
    Transform PodPops;
    Transform CollectiblePops;
    Transform DeathPops;
    Transform FingerPops;
    Transform specialMoveReminder;

    [Header("Star Score")]
    public GameObject[] stars;
    int starCount;

    public bool hasSpecial, secondChanceActive;

    public bool cancelGameOver;
    Animator splatAnim;
    GameObject camera;
    //Menu Animators
    Animator pauseAnim;
    Animator gameOverAnim;
    Animator adGameAnim;
    Animator tutorialPopAnim;
    public bool pausedGame, gameStarting;


    //Scripts To Access
    PlayerStatus ps;
    PlayerProperties pp;
    PlayerMovement pm;
    PlayerAnimations pa;

    public GameObject adButton;
    Animator adBAnim;

    public GameObject countDown;

    //This handles press me button pop up and Slider filler
    [Header("Slider Animations")]
    public Slider specialBar;
    public Button sliderButton;
    float resetSliderValue = 0f;

    public GameObject specialBarParticle;

    Transform textChild1;
    Transform textChild2;

    float myScore = 1499f;

    private void Awake()
    {
        MakeInstance();
        specialBar.minValue = 0f;
        specialBar.maxValue = 1500f;
        player = GameObject.FindGameObjectWithTag("PlayerHolder");
        ps = player.GetComponent<PlayerStatus>();
        pp = player.GetComponent<PlayerProperties>();
        pm = player.GetComponent<PlayerMovement>();
        internalP = player.transform.GetChild(0);
        pa = internalP.GetComponent<PlayerAnimations>();
        pauseAnim = pausePanel.GetComponent<Animator>();
        gameOverAnim = gameOverPanel.GetComponent<Animator>();
        adGameAnim = adGamePanel.GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("CameraHolder");
        adBAnim = adButton.GetComponent<Animator>();
        adBAnim.enabled = false;

        //Tutorial Stuff
        tutorialPops = tutorialPanel.transform.GetChild(0);
        ObjectivePops = tutorialPanel.transform.GetChild(2);
        PodPops = tutorialPanel.transform.GetChild(3);
        CollectiblePops = tutorialPanel.transform.GetChild(4);
        DeathPops = tutorialPanel.transform.GetChild(5);
        FingerPops = tutorialPanel.transform.GetChild(6);
        tutorialReminders = tutorialPanel.transform.GetChild(1);
        specialMoveReminder = tutorialReminders.GetChild(1);
        tutorialPopAnim = tutorialPanel.GetComponent<Animator>();

        //SLider Stuff
        specialBar.enabled = false;
        textChild1 = sliderButton.transform.GetChild(0);
        textChild2 = sliderButton.transform.GetChild(1);

        if (DataController.instance.firstPlay)
        {
            StopCoroutine(DelayStart());
            TutorialPlayThrough();
            Time.timeScale = 0f;
        }
        else
        {
            countDown.SetActive(true);
            StartCoroutine(DelayStart());
        }
    }

    void Start()
    {
        AdController.instance.RequestBanner();
    }
    // Update is called once per frame
    void Update()
    {
        CheckForPoints();
    }

    void MakeInstance()
    {
        if (gmmInstance == null)
        {
            gmmInstance = this;
        }
        else if (gmmInstance != null)
        {
            Destroy(gameObject);
        }

    }
    //This Checks the points
    void CheckForPoints()
    {
        starCount = ScoreManager.sInstance.starCount;
        podScore.text = ScoreManager.sInstance.podCount.ToString();
        score.text = ScoreManager.sInstance.scoreValue.ToString();

        for (int i = 0; i <= starCount; i++)
        {
            if (i >= 10)
            {
                starCount = 0;
            }
            else
            {
                stars[i].SetActive(true);
            }

        }

        if (starCount == 0)
        {
            stars[0].SetActive(false);
            stars[1].SetActive(false);
            stars[2].SetActive(false);
            stars[3].SetActive(false);
            stars[4].SetActive(false);
            stars[5].SetActive(false);
            stars[6].SetActive(false);
            stars[7].SetActive(false);
            stars[8].SetActive(false);
            stars[9].SetActive(false);
        }

        if (specialBar.value >= myScore)
        {
            CheckMySpecial();
        }
    }

    public void activateReminder()
    {

    }
    // This takes care of the ability checker and allow special ability to happen
    void CheckMySpecial()
    {
        int mySelection = DataController.instance.selectedIndex;
        switch (mySelection)
        {
            case 0:
                textChild2.gameObject.SetActive(true);
                hasSpecial = false;
                break;
            case 1:
                textChild2.gameObject.SetActive(true);
                hasSpecial = false;
                break;
            case 2:
                textChild2.gameObject.SetActive(true);
                hasSpecial = false;
                break;
            case 3:
                textChild1.gameObject.SetActive(true);
                hasSpecial = true;
                break;
            case 4:
                textChild1.gameObject.SetActive(true);
                hasSpecial = true;
                break;
            case 5:
                textChild1.gameObject.SetActive(true);
                hasSpecial = true;
                break;
            case 6:
                textChild1.gameObject.SetActive(true);
                hasSpecial = true;
                break;
            case 7:
                textChild1.gameObject.SetActive(true);
                hasSpecial = true;
                break;
            case 8:
                textChild1.gameObject.SetActive(true);
                hasSpecial = true;
                break;
            case 9:
                textChild1.gameObject.SetActive(true);
                hasSpecial = true;
                break;
        }
    }
    //These Are in Game Functions
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseAnim.Play("FadeAway");
        pausePanel.SetActive(false);
        if (pausedGame)
        {
            pausedGame = false;
            StartCoroutine(DelayStart());
            countDown.SetActive(true);
        }
    }

    public void PauseGame()
    {

        //Disable the countdown so this will stop the game no matter what
        pausedGame = true;
        if (pausedGame)
        {
            StopCoroutine(DelayStart());
            countDown.SetActive(false);
        }
        else
        {
            gameStarting = false;
            pausedGame = false;
        }
        pauseHighscore.text = DataController.instance.highScore.ToString();
        pauseCurrentScore.text = ScoreManager.sInstance.scoreValue.ToString();
        pausePanel.SetActive(true);
        pauseAnim.Play("SlideDown");
        Time.timeScale = 0f;
    }

    public void CallForRewardVideo()
    {
        AdController.instance.ShowRewarded();
    }
    public void AdStartGame()
    {
        adGamePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeAfterAd()
    {
        StartCoroutine(SlideDown());
        Time.timeScale = 1f;
    }

    IEnumerator SlideDown()
    {
        yield return new WaitForSeconds(1.0f);
        adGamePanel.SetActive(false);

    }

    public void GameOver()
    {
        poisonText.SetActive(false);
        invincibleText.SetActive(false);
        reverseText.SetActive(false);
        highScoreText.SetActive(false);
        gameOverHighscore.text = DataController.instance.highScore.ToString();
        gameOverCurrentScore.text = ScoreManager.sInstance.scoreValue.ToString();
        gameOverPods.text = DataController.instance.highPod.ToString();
        DataController.instance.firstPlay = false;
        DataController.instance.finishedFirstPlay = true;
        SoundManager.soundInstance.gameMusicSource.Stop();
        SoundManager.soundInstance.EndGame();
        gameOverPanel.SetActive(true);
        gameOverAnim.Play("SlideIn");
        StartCoroutine(AdCountDown(5f));
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SoundManager.soundInstance.gameMusicSource.Stop();
        SoundManager.soundInstance.PlayBackgroundMusic();
        SceneManager.LoadScene("MainLevel");
        Time.timeScale = 1f;
        DataController.instance.adCount++;
        DataController.instance.SaveGameData();
        if (DataController.instance.adCount >= 4 && !DataController.instance.removeAd)
        {
            AdController.instance.ShowInterstitial();
            PauseGame();
            DataController.instance.adCount = 0;
            DataController.instance.SaveGameData();
        }
    }
    public void RemoveLife(int life)
    {
        if (life == 1)
        {
            lives[1].SetActive(false);
        }
        else if (life < 1)
        {
            lives[0].SetActive(false);
        }
        StartCoroutine(ResetMyLife());
    }

    public void AwardResurrection()
    {
        cancelGameOver = true;
        rewardFlash.SetActive(true);
    }
    //This is only called from Ad Animation After Reward Flash
    public void AfterAdPlay()
    {
        gameOverPanel.SetActive(false);
        SoundManager.soundInstance.gameMusicSource.Stop();
        SoundManager.soundInstance.PlayBackgroundMusic();
        pp.Continue();
    }
    public void Resurrection()
    {
        StartCoroutine(DelayStart());
        countDown.SetActive(true);
    }
    public void QuitGame()
    {
        DataController.instance.firstPlay = false;
        DataController.instance.finishedFirstPlay = true;
        SoundManager.soundInstance.gameMusicSource.Stop();
        SceneManager.LoadScene("MainMenu");
        pauseHighscore.text = DataController.instance.highScore.ToString();
        pauseCurrentScore.text = ScoreManager.sInstance.scoreValue.ToString();
        SoundManager.soundInstance.PlayMenuMusic();
        DataController.instance.SaveGameData();

        if (DataController.instance.adCount >= 4 && !DataController.instance.removeAd)
        {
            AdController.instance.ShowInterstitial();
            DataController.instance.adCount = 0;
            DataController.instance.SaveGameData();
        }
        Time.timeScale = 1f;
    }

    public void CallMyTrap(int glueSplats)
    {
        SoundManager.soundInstance.Splatters();
        for (int glues = 0; glues <= glueSplats; glues++)
        {
            myLoc = Random.Range(0, splatLocations.Length);
            var splatC2 = Instantiate(splats[Random.Range(0, splats.Length)], splatLocations[myLoc].position, splatLocations[myLoc].rotation);
            splatC2.transform.SetParent(camera.transform);
        }
    }

    public void DestroySplat()
    {
        var splats = GameObject.FindGameObjectsWithTag("Splat");
        foreach (GameObject splatters in splats)
        {
            splatAnim = splatters.gameObject.GetComponent<Animator>();
            splatAnim.Play("GlueGone");
        }

    }

    public void Text()
    {

        if (ps.poisoned)
        {
            poisonText.SetActive(true);
            reverseText.SetActive(false);
            highScoreText.SetActive(false);
            invincibleText.SetActive(false);
            levelChanged.SetActive(false);
        }
        else if (ps.invincible)
        {
            invincibleText.SetActive(true);
            poisonText.SetActive(false);
            reverseText.SetActive(false);
            highScoreText.SetActive(false);
            levelChanged.SetActive(false);
        }
        else if (ps.beenReversed)
        {
            reverseText.SetActive(true);
            poisonText.SetActive(false);
            highScoreText.SetActive(false);
            invincibleText.SetActive(false);
            levelChanged.SetActive(false);
        }
        else if (ScoreManager.sInstance.highScoreText)
        {
            highScoreText.SetActive(true);
            poisonText.SetActive(false);
            reverseText.SetActive(false);
            invincibleText.SetActive(false);
            levelChanged.SetActive(false);
            StartCoroutine(NewScoreTurnOff());
        }
        else if (GameplayManager.instance.newLevel)
        {
            levelChanged.SetActive(true);
            poisonText.SetActive(false);
            highScoreText.SetActive(false);
            reverseText.SetActive(false);
            invincibleText.SetActive(false);
            StartCoroutine(NewLevel());
        }
        else if (pp.usedSpecial)
        {
            specialMoveText.SetActive(true);
            levelChanged.SetActive(false);
            poisonText.SetActive(false);
            highScoreText.SetActive(false);
            reverseText.SetActive(false);
            invincibleText.SetActive(false);
        }
    }


    public void CancelText()
    {

        if (!ps.poisoned)
        {
            poisonText.SetActive(false);
        }
        if (!ps.invincible)
        {
            invincibleText.SetActive(false);
        }
        if (!ps.beenReversed)
        {
            reverseText.SetActive(false);
        }
        if (!ScoreManager.sInstance.highScoreText)
        {
            highScoreText.SetActive(false);
            StartCoroutine(NewScoreTurnOff());
        }
        if (!pp.usedSpecial)
        {
            specialMoveText.SetActive(false);
        }
        if (!GameplayManager.instance.newLevel)
        {
            levelChanged.SetActive(false);
        }

    }

    IEnumerator ResetMyLife()
    {
        yield return new WaitForSeconds(.5f);
        pp.Continue();
        Time.timeScale = 0;
    }
    IEnumerator NewScoreTurnOff()
    {
        yield return new WaitForSeconds(5f);
        highScoreText.SetActive(false);
        ScoreManager.sInstance.highScoreText = false;
    }

    public IEnumerator NewLevel()
    {
        yield return new WaitForSeconds(5f);
        GameplayManager.instance.newLevel = false;
        levelChanged.SetActive(false);
    }
    IEnumerator AdCountDown(float five)
    {

        yield return new WaitForSecondsRealtime(.25f);
        five--;
        switch (five)
        {
            case 5:
                numImages[5].SetActive(true);
                numImages[1].SetActive(false);
                numImages[2].SetActive(false);
                numImages[3].SetActive(false);
                numImages[4].SetActive(false);
                break;
            case 4:
                numImages[0].SetActive(false);
                numImages[1].SetActive(true);
                break;
            case 3:
                numImages[1].SetActive(false);
                numImages[2].SetActive(true);
                break;
            case 2:
                numImages[2].SetActive(false);
                numImages[3].SetActive(true);
                break;
            case 1:
                numImages[3].SetActive(false);
                numImages[4].SetActive(true);
                break;
            case 0:
                adBAnim.enabled = true;
                StartCoroutine(KillAdButton());
                break;
        }
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(AdCountDown(five));
    }

    public void StartTutorial()
    {
        StartCoroutine(DelayStart());
    }
    public IEnumerator DelayStart()
    {
        if (pausedGame == false)
        {
            Time.timeScale = 0f;
            float pauseTime = Time.realtimeSinceStartup + 4f;
            while (Time.realtimeSinceStartup < pauseTime)
                yield return 0;
            countDown.SetActive(false);
            //This is checking first playthrough
            if (DataController.instance.firstPlay)
            {
                TapMeUp();
            }
            if (ps.lives >= 1)
            {
                pa.HandleDisableDeath();
            }
            else if (ps.lives < 1)
            {
                pa.HandleDisableDeath();
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            StopCoroutine(DelayStart());
            countDown.SetActive(false);
            Time.timeScale = 1f;
        }

    }

    IEnumerator KillAdButton()
    {
        yield return new WaitForSeconds(1.0f);
        adButton.SetActive(false);
    }

    public void RewardMe()
    {
        secondPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    IEnumerator HandleResurrection()
    {
        ps.ResetItems();
        StartCoroutine(ResetMyLife());
        secondPanel.SetActive(false);
        //AdController.instance.rewardAd = false;
        yield return new WaitForSeconds(1.0f);
        rewardFlash.SetActive(false);
        secondChanceActive = false;
    }

    public void CallMySpecial()
    {
        reset = true;
        int myCharacter = DataController.instance.selectedIndex;
        if (specialBar.value >= myScore)
        {
            pp.SpecialMove(myCharacter);
            sliderButton.gameObject.SetActive(false);
            textChild1.gameObject.SetActive(false);
            specialBar.gameObject.SetActive(false);
            specialBarParticle.SetActive(false);
        }
    }


    public void CheckSpecialMove(float scorePassed)
    {
        if (!reset)
        {
            if (specialBar.value >= myScore)
            {
                if (DataController.instance.selectedIndex < 3)
                {
                    textChild2.gameObject.SetActive(true);
                }
                else
                    textChild1.gameObject.SetActive(true);
            }
            else
            {
                textChild1.gameObject.SetActive(false);
                textChild2.gameObject.SetActive(false);
            }
            specialBar.value = specialBar.value + scorePassed;
        }
        else
        {
            reset = false;
        }
    }

    public void EndMySpecial()
    {
        reset = false;
        specialBar.value = 0;
        sliderButton.gameObject.SetActive(true);
        textChild1.gameObject.SetActive(false);
        specialBar.gameObject.SetActive(true);
        specialBarParticle.SetActive(true);

    }

    /******************************** Tutorial Segment
    /******************
    /*************************
    tutorial Magic Happens
    */

    void TutorialPlayThrough()
    {
        tutorialPanel.SetActive(true);
        tutorialPops.gameObject.SetActive(true);
        tutorialPopAnim.Play("PopUpPanels");
    }
    public void TapPlay()
    {
        tutorialPopAnim.Play("ReversePop");
    }

    public void ObjectivePlay()
    {
        tutorialPopAnim.Play("SecondReversePop");
    }
    public void ObjectivePopUp()
    {
        tutorialPops.gameObject.SetActive(false);
        ObjectivePops.gameObject.SetActive(true);
        tutorialPopAnim.Play("PopUpPanels");
    }

    public void ReverseObjectivePopUp()
    {
        ObjectivePops.gameObject.SetActive(false);
        tutorialPops.gameObject.SetActive(false);
        HandleStart();
    }

    //This will begin  the game in tutorialmode
    void HandleStart()
    {
        StartCoroutine(DelayStart());
        countDown.SetActive(true);
        tutorialPops.gameObject.SetActive(false);
    }

    //This handles the flash of the Tap and Hold
    void TapMeUp()
    {
        StartCoroutine(ShowingTaps());
    }

    IEnumerator ShowingTaps()
    {

        tutorialReminders.gameObject.SetActive(true);
        tutorialPopAnim.Play("TapHoldFlash");
        specialMoveReminder.gameObject.SetActive(false);
        yield return new WaitForSeconds(4.0f);
        tutorialReminders.gameObject.SetActive(false);
        specialMoveReminder.gameObject.SetActive(false);
    }

    //Pod Popup Panel
    public void PodPopUp()
    {
        PodPops.gameObject.SetActive(true);
        tutorialPopAnim.Play("PodPopUp");
        Time.timeScale = 0;

    }

    //Continue gameplay until next pop up
    public void PodPopReverse()
    {
        tutorialPopAnim.Play("PodPopReverse");
    }
    public void ContinueWithGame()
    {
        PodPops.gameObject.SetActive(false);
        StartCoroutine(DelayStart());
        countDown.SetActive(true);
    }

    //Collectible Pop up
    public void CollectiblePopUp()
    {
        CollectiblePops.gameObject.SetActive(true);
        tutorialPopAnim.Play("CollectiblePopUp");
        Time.timeScale = 0;
    }

    //This begins the animation of the Closing Collectible
    public void CloseCollectiblePop()
    {
        tutorialPopAnim.Play("ReverseCollectiblePop");
    }

    //This is called from Tutorial Animations Script
    public void ContinueAfterCollectible()
    {
        CollectiblePops.gameObject.SetActive(false);
        StartCoroutine(DelayStart());
        countDown.SetActive(true);
    }

    //When we die for the first time Pop Up Tutorial

    public void DeathPopUp()
    {
        Time.timeScale = 0f;
        DeathPops.gameObject.SetActive(true);
        tutorialPopAnim.Play("DeathPopUp");
    }

    public void ReverseDeathPop()
    {
        tutorialPopAnim.Play("ReverseDeathPop");
    }

    //This brings back the game at the end of the Animation ReverseDeathPop
    public void AfterDeathPop()
    {
        DeathPops.gameObject.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(ResetMyLife());
    }

    // This is only when you have a special which we have to enable a character with a special
    public void SpecialMovePopUp()
    {
        FingerPops.gameObject.SetActive(true);
        tutorialPopAnim.Play("SpecialPopUp");
        Time.timeScale = 0f;
        pm.CheckForSwipe();
    }
    //Check for finger swipe
    public void CallTutorialSwipe()
    {
        tutorialPopAnim.Play("SpecialPopUp");
    }

    public void HandleMySpecialPop()
    {
        Time.timeScale = 1f;
        StartCoroutine(DelayStart());
        countDown.SetActive(true);
        tutorialPanel.SetActive(false);
        DataController.instance.firstSpecial = false;
        DataController.instance.SaveGameData();
    }
}
