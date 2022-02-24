using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// I seperated the First menu fro the game menu so anything that happens in the main menu this is the primary portion of main menu UI elements
public class MainMenuController : MonoBehaviour
{

    public static MainMenuController mmInstance;

    [Header("Menus")]
    public GameObject characterSelectMenu;
    public GameObject settingsMenu;
    public GameObject welcome;
    public GameObject aboutUs;
    public GameObject ratingsPanel;
    public GameObject postingPanel;
    public GameObject sharingPanel;
    public GameObject shopPanel;
    public GameObject leaderBoardPanel;

    public Button giftPress;

    public GameObject socialPanel;
    Animator welcomePopUp;
    Animator settingsPop;


    [Header("Text For Character Menu")]
    public Text myPodCount;
    public bool inGame;

    bool receivedGift;

    public ParticleSystem podParticles;

    public GameObject openGift;
    public GameObject closedGift;

    int socButtonPress;
    Animator socialPanelAnim;
    Animator aboutAnim;
    Animator postingAnim;
    Animator charSelectAnim;
    Animator ratingAnim;
    Animator sharingAnim;
    Animator shopAnim;
    Animator leaderAnim;

    //The very first thing that is call in Unity ther is another function called Start but I rarily use it
    private void Awake()
    {
        MakeInstance();
        welcomePopUp = welcome.gameObject.GetComponent<Animator>();
        settingsPop = settingsMenu.gameObject.GetComponent<Animator>();
        aboutAnim = aboutUs.GetComponent<Animator>();
        ratingAnim = ratingsPanel.GetComponent<Animator>();
        postingAnim = postingPanel.GetComponent<Animator>();
        sharingAnim = sharingPanel.GetComponent<Animator>();
        shopAnim = shopPanel.GetComponent<Animator>();
        leaderAnim = leaderBoardPanel.GetComponent<Animator>();

        inGame = false;
        characterSelectMenu.SetActive(false);
        charSelectAnim = characterSelectMenu.GetComponent<Animator>();
        Time.timeScale = 1f;
        DataController.instance.playTimeCount++;
        if (DataController.instance.playTimeCount >= 4)
        {
            ratingsPanel.SetActive(true);
        }
        else
        {
            ratingsPanel.SetActive(false);
        }

        if (DataController.instance.highPod >= 500)
        {
            //GameServices.instance.AchievementUnlocked("Collect500");
            //GameServices.instance.AchievementUnlocked("Senty500");
        }


        if (DataController.instance.adCount >= 4)
        {
            AdController.instance.ShowInterstitial();
            DataController.instance.adCount = 0;
        }

        DataController.instance.SaveGameData();
    }

    void Update()
    {
        if (!receivedGift)
        {
            CheckFirstTime();
        }
    }

    // make it to where I can access it in other scripts without having to get access to the game object
    void MakeInstance()
    {
        if (mmInstance == null)
        {
            mmInstance = this;
        }
        else if (mmInstance != null)
        {
            Destroy(gameObject);
        }

    }

    //This checks for first time play through
    void CheckFirstTime()
    {
        if (!DataController.instance.firstPlay && DataController.instance.inTutorial)
        {
            welcome.SetActive(true);
            DataController.instance.inTutorial = false;
        }
    }

    // This loads the actual game play first menu
    public void PlayGame()
    {
        inGame = true;
        SceneManager.LoadScene("MainLevel");
        SoundManager.soundInstance.PlayBackgroundMusic();
        DataController.instance.adCount++;
    }


    //This will check if tutorial but not used currently
    public void PlayTut()
    {
        inGame = true;
        SceneManager.LoadScene("Tutorial");
        SoundManager.soundInstance.PlayBackgroundMusic();
    }

    //Character selection menu
    public void CharacterSelect()
    {
        characterSelectMenu.SetActive(true);
        myPodCount.text = DataController.instance.highPod.ToString();
        charSelectAnim.Play("InOut");
        podParticles.Play();
    }
    //Shop panel access
    public void ShopPanelAccess()
    {
        shopPanel.SetActive(true);
    }
    public void Settings()
    {
        settingsMenu.SetActive(true);
        settingsPop.Play("InOut");
    }

    public void ReturnSettingsHome()
    {
        settingsPop.Play("OutIn");
        DataController.instance.musicPlay = SoundManager.soundInstance.musicVolume;
        DataController.instance.fxPlay = SoundManager.soundInstance.fxVolume;
        DataController.instance.SaveGameData();
    }

    public void ReturnCharHome()
    {
        charSelectAnim.Play("OutIn");
        podParticles.Stop();
    }

    public void ReturnShopHome()
    {
        shopAnim.Play("OutBack");
        StartCoroutine(CloseShop());
    }
    IEnumerator CloseShop()
    {
        yield return new WaitForSeconds(.5f);
        shopPanel.SetActive(false);
    }
    public void LeaderBoardEnter()
    {
        leaderAnim.Play("InZoom");
        leaderBoardPanel.SetActive(true);
    }
    public void LeaderBoardExit()
    {
        StartCoroutine(CloseLead());
    }
    IEnumerator CloseLead()
    {
        yield return new WaitForSeconds(.1f);
        leaderBoardPanel.SetActive(false);
    }
    public void ReceiveGift()
    {
        receivedGift = true;
        closedGift.SetActive(false);
        openGift.SetActive(true);
        StartCoroutine(PlayDropAway());

    }

    IEnumerator PlayDropAway()
    {
        //PopUpText
        yield return new WaitForSeconds(1.6f);
        welcomePopUp.Play("DipAwayScreen");
        yield return new WaitForSeconds(1.0f);
        welcome.SetActive(false);
    }

    public void Socials()
    {
        socButtonPress++;
        socialPanelAnim = socialPanel.GetComponent<Animator>();

        if (socButtonPress >= 2)
        {
            socialPanelAnim.Play("SocialSlideDown");
            socButtonPress = 0;
        }
        else
        {
            socialPanelAnim.Play("SocialSlideUp");
        }
    }

    public void ActivateAbout()
    {
        aboutUs.SetActive(true);
    }

    public void WebPage()
    {
        Application.OpenURL("https://www.acgstu.com");
    }

    public void Privacy()
    {
        Application.OpenURL("https://www.acgstu.com/Resources/html/privacypolicy");
    }

    public void AboutExit()
    {
        aboutAnim.Play("DipDown");
        StartCoroutine(DisableAbout());
    }

    public void RateUs()
    {
#if UNITY_IPHONE
        Application.OpenURL("https://itunes.apple.com/app/id1494611989");
#endif

#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.ACG_Studios.Sentypede");
#endif
    }

    public void Posting()
    {
        postingPanel.SetActive(true);
    }

    public void PostExit()
    {
        postingAnim.Play("rateout");
        StartCoroutine(DisablePosting());
    }

    IEnumerator DisablePosting()
    {
        yield return new WaitForSeconds(1.0f);
        postingPanel.SetActive(false);

    }

    public void ShowShare()
    {
        sharingPanel.SetActive(true);
    }

    public void DisableShare()
    {
        sharingAnim.Play("rateout");
        StartCoroutine(DisableS());
    }
    IEnumerator DisableS()
    {
        yield return new WaitForSeconds(1.0f);
        sharingPanel.SetActive(false);
    }

    public void TPostExit()
    {
        postingPanel.SetActive(false);
    }



    public void DisableRating()
    {
        ratingAnim.Play("rateout");
        DataController.instance.playTimeCount = 0;
        DataController.instance.SaveGameData();
        StartCoroutine(DisableR());
    }



    IEnumerator DisableR()
    {
        yield return new WaitForSeconds(1.0f);
        ratingsPanel.SetActive(false);

    }
    IEnumerator DisableAbout()
    {
        yield return new WaitForSeconds(1.1f);
        aboutUs.SetActive(false);
    }

    // Social Media Stuff
    public void PostTwitter()
    {
        //GameServices.instance.TwitterAction();
    }

    public IEnumerator ExitShare()
    {
        yield return new WaitForSeconds(0f);
    }
}//class
