using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class GameData
{

    int podCount;
    int highScore;
    int playCount;
    int dailyReward;
    int lives;
    int selectedIndex;
    float musicSettings;
    float fxSettings;
    int interAd;
    int socialTime;
    bool havePosted;
    bool hasPaid;
    bool firstSpecial;

    bool[] characters;

    public int PodCount
    {
        get
        {
            return podCount;
        }
        set
        {
            podCount = value;
        }
    }
    public int DailyReward
    {
        get
        {
            return dailyReward;
        }
        set
        {
            dailyReward = value;
        }
    }
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            lives = value;
        }
    }
    public int HighScore
    {
        get
        {
            return highScore;
        }
        set
        {
            highScore = value;
        }
    }

    public int PlayCount
    {
        get
        {
            return playCount;
        }
        set
        {
            playCount = value;
        }
    }

    public int SelectedIndex
    {
        get
        {
            return selectedIndex;
        }
        set
        {
            selectedIndex = value;
        }
    }

    public bool[] Characters
    {
        get
        {
            return characters;
        }
        set
        {
            characters = value;
        }
    }

    public float MusicSettings
    {
        get
        {
            return musicSettings;
        }
        set
        {
            musicSettings = value;
        }
    }

    public float FxSettings
    {
        get
        {
            return fxSettings;
        }
        set
        {
            fxSettings = value;
        }
    }

    public int InterAd
    {
        get
        {
            return interAd;
        }
        set
        {
            interAd = value;
        }
    }

    public int SocialTime
    {
        get
        {
            return socialTime;
        }
        set
        {
            socialTime = value;
        }
    }

    public bool HavePosted
    {
        get
        {
            return havePosted;
        }
        set
        {
            havePosted = value;
        }
    }
    public bool HasPaid
    {
        get
        {
            return hasPaid;
        }
        set
        {
            hasPaid = value;
        }
    }
    public bool FirstSpecial
    {
        get
        {
            return firstSpecial;
        }
        set
        {
            hasPaid = value;
        }
    }
}//class
