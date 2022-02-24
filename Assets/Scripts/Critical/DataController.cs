using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// This is the script that creates and saves data for the player real simple just storing in a file only accessible by skilled end users
// alot of the terms are self explanatory
public class DataController : MonoBehaviour
{

    public static DataController instance;

    GameData gameData;

    string gameFile = "/CubeyData.dat";
    //[HideInInspector]
    public int highPod, highScore, selectedIndex, playTimeCount, adCount, dailyReward, lifeCount;
    public float musicPlay, fxPlay;
    //[HideInInspector]
    public bool[] characters;
    public bool firstPlay;
    public bool finishedFirstPlay;
    public bool inTutorial;
    public bool facebookPost;
    public bool haveRatedGame;
    public bool removeAd;

    public bool firstSpecial;

    private void Awake()
    {
        MakeInstance();
        InitializeGameData();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

    }

    void InitializeGameData()
    {
        LoadGameData();

        //Setting up the initial values for the game

        if (gameData == null)
        {
            highPod = 50000;
            firstPlay = true;
            inTutorial = true;
            haveRatedGame = false;
            facebookPost = false;
            firstSpecial = true;
            highScore = 0;
            selectedIndex = 0;
            adCount = 0;
            musicPlay = 1;
            fxPlay = 1;
            lifeCount = 5;
            dailyReward = 0;
            playTimeCount = 0;

            characters = new bool[8];
            characters[0] = true;

            for (int i = 1; i < characters.Length; i++)
            {
                characters[i] = false;
            }

            gameData = new GameData();
            gameData.Characters = characters;
            gameData.MusicSettings = musicPlay;
            gameData.FxSettings = fxPlay;
            gameData.PodCount = highPod;
            gameData.HighScore = highScore;
            gameData.InterAd = adCount;
            gameData.PlayCount = playTimeCount;
            gameData.SelectedIndex = selectedIndex;
            gameData.MusicSettings = musicPlay;
            gameData.FirstSpecial = firstSpecial;

            SaveGameData();
        }
    }
    public void SaveGameData()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            file = File.Create(Application.persistentDataPath + gameFile);

            if (gameData != null)
            {
                gameData.Characters = characters;
                gameData.PlayCount = playTimeCount;
                gameData.PodCount = highPod;
                gameData.HighScore = highScore;
                gameData.SelectedIndex = selectedIndex;
                gameData.MusicSettings = musicPlay;
                gameData.FxSettings = fxPlay;
                gameData.InterAd = adCount;
                gameData.Lives = lifeCount;
                gameData.FirstSpecial = firstSpecial;
                bf.Serialize(file, gameData);
            }
        }
        catch
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    void LoadGameData()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            file = File.Open(Application.persistentDataPath + gameFile, FileMode.Open);

            gameData = (GameData)bf.Deserialize(file);

            if (gameData != null)
            {
                firstPlay = false;
                inTutorial = false;
                characters = gameData.Characters;
                playTimeCount = gameData.PlayCount;
                highPod = gameData.PodCount;
                highScore = gameData.HighScore;
                selectedIndex = gameData.SelectedIndex;
                musicPlay = gameData.MusicSettings;
                fxPlay = gameData.FxSettings;
                adCount = gameData.InterAd;
                firstSpecial = gameData.FirstSpecial;
            }
        }
        catch
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }
}
