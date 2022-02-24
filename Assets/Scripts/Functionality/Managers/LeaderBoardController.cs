using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//  This script is going to be used to talk to ACG Leaderboards system and handle displaying leaderboards
public class LeaderBoardController : MonoBehaviour
{

    public static LeaderBoardController lbInstance;
    string myDevice;
    string avatar;

    string email;
    string username;
    int highScore;
    int lowScore;

    Image avatarImage;

    public InputField emailField;
    public InputField usernameField;

    public GameObject leaderBoardPanel;
    public GameObject signUpScreen;
    public GameObject chooseAvatarScreen;

    public GameObject[] avatars;

    public GameObject welcome;
    // Start is called before the first frame update

    void Awake()
    {
        myDevice = SystemInfo.deviceModel;
        MakeInstance();
    }

    void MakeInstance()
    {
        if (lbInstance == null)
        {
            lbInstance = this;
        }
        else if (lbInstance != null)
        {
            Destroy(gameObject);
        }

    }

    public void SignUp()
    {
        welcome.SetActive(false);
        signUpScreen.SetActive(true);
    }

    public void ChooseAvatarScreen()
    {
        signUpScreen.SetActive(false);
        chooseAvatarScreen.SetActive(true);
    }

    public void ChoosenAvatar()
    {
    }
    public void SubmitInformation()
    {
        email = emailField.text;
        username = usernameField.text;
        highScore = DataController.instance.highScore;
    }
}
