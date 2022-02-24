using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    PlayerProperties playerProps;
    PlayerMovement playerMovement;
    Transform particleHolder;
    public Transform playerBoundaries;
    GameObject reversalParticles;
    GameObject invincibilityParticles;
    GameObject poisonedParticles;
    public GameObject beeStingParticles;

    //Different types of deaths within the game
    public bool comboActive, hasPod, beeAttack,
    stabbed, beenReversed, explosionDeath,
    hasDied, poisoned, tutPickUp, tutPod;

    bool glueTrap, hasFruit, gotStar, obstacleHit, poisonActive, hitCrate;
    public bool wallHit;
    public int podCounter;
    public int landCounter;
    public int crateCounter;
    public int starCounter;
    public int lives = 2;

    //These are for Acheivement Checks
    public bool landHit;
    public bool hitByCar;

    string whatIHit;


    //MY Timers
    // For Sentypede Combo Section
    float comboTimer = 2.5f;
    float resetComboTimer = 2.5f;

    // Timers for set actions
    public float poisonTimer = 20f;
    public float poisonTimerReset = 20f;

    public float beeStingsTimer = 10f;
    public float resetBeeStingsTimer = 10f;

    float glueTimer = 12f;
    float glueTimerReset = 12f;

    float reverseTimer = 15f;
    float reverseTimerReset = 15f;

    public bool invincible;
    public bool specialInvincible;
    public float invincibleTime = 20f;
    float resetInvincibleTime = 20f;

    //This is what happens when Pickup item
    public enum PlayerCollectibles
    {
        NoPickUp = 0,
        Pod = 1,
        Strawberry = 2,
        Orange = 3,
        Cherry = 4,
        Star = 5,
        Poison = 6,
        Reversed = 7,
    }

    // Stats of Player

    public enum PlayerStats
    {
        Normal = 0,
        Dead = 1,
        Reversed = 2,
        Invincible = 3,
        Trapped = 4,
        Exploded = 5,
        CarExplosion = 6,
        Stabbed = 7,
        Slipped = 8,
        Poisoned = 9,
        LandKill = 10,
        Crate = 11,
        Coned = 12
    }

    //Timers for property sets
    public PlayerCollectibles pickupType = PlayerCollectibles.NoPickUp;
    public PlayerStats playerState = PlayerStats.Normal;

    Transform playerPart;

    void Awake()
    {
        playerProps = this.GetComponent<PlayerProperties>();
        playerMovement = this.GetComponent<PlayerMovement>();
        particleHolder = this.transform.GetChild(1);

        invincibilityParticles = particleHolder.GetChild(0).gameObject;
        reversalParticles = particleHolder.GetChild(1).gameObject;
        poisonedParticles = particleHolder.GetChild(3).gameObject;

        //When player is special ability and invincibles we will enable these items
        playerBoundaries = this.transform.GetChild(2);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        HandleWhatITouch();
    }

    void HandleWhatITouch()
    {
        if (specialInvincible)
        {
            if (beenReversed)
            {
                playerMovement.reversedTime = false;
                reversalParticles.SetActive(false);
                reverseTimer = reverseTimerReset;
                beenReversed = false;
            }

            if (poisoned)
            {
                poisonedParticles.SetActive(false);
                playerMovement.CancelInvoke("CheckRemovedPod");
                poisonTimer = poisonTimerReset;
                poisoned = false;
            }
            stabbed = false;
            invincibilityParticles.SetActive(true);
            playerState = PlayerStats.Invincible;
        }

        if (playerProps.shockwaveEffect)
        {
            if (invincible)
            {
                pickupType = PlayerCollectibles.NoPickUp;
                invincibilityParticles.SetActive(false);
                invincibleTime = resetInvincibleTime;
                GameMenuController.gmmInstance.invincibleText.SetActive(false);
                GameMenuController.gmmInstance.CancelText();
                playerState = PlayerStats.Normal;
                invincible = false;
            }
            if (beenReversed)
            {
                playerMovement.reversedTime = false;
                reversalParticles.SetActive(false);
                reverseTimer = reverseTimerReset;
                beenReversed = false;
                GameMenuController.gmmInstance.CancelText();
            }

            if (poisoned)
            {
                poisonedParticles.SetActive(false);
                playerMovement.CancelInvoke("CheckRemovedPod");
                poisonTimer = poisonTimerReset;
                poisoned = false;
                GameMenuController.gmmInstance.CancelText();
            }
        }

        if (glueTrap && !invincible || glueTrap && !playerProps.shockwaveEffect || glueTrap && !specialInvincible)
        {
            glueTimer -= Time.deltaTime;

            if (glueTimer <= 0.0f)
            {
                glueTrap = false;
                GameMenuController.gmmInstance.DestroySplat();
                glueTimer = glueTimerReset;
                pickupType = PlayerCollectibles.NoPickUp;
                playerState = PlayerStats.Normal;
            }

        }

        if (beenReversed)
        {
            reverseTimer -= Time.deltaTime;

            if (reverseTimer <= 0.0f)
            {
                beenReversed = false;
                reverseTimer = reverseTimerReset;
                reversalParticles.SetActive(false);
                pickupType = PlayerCollectibles.NoPickUp;
                GameMenuController.gmmInstance.CancelText();
                playerState = PlayerStats.Normal;
            }

        }

        if (beeAttack && beeStingsTimer > 0)
        {
            beeStingsTimer -= Time.deltaTime;
        }
        else
        {
            if (beeStingsTimer <= 0.0f)
            {
                beeStingsTimer = resetBeeStingsTimer;
                beeStingParticles.SetActive(false);
            }
        }

        if (poisoned && poisonTimer > 0
            && !invincible && !playerProps.shockwaveEffect && !specialInvincible)
        {
            pickupType = PlayerCollectibles.Poison;
            poisonTimer -= Time.deltaTime;
        }
        else
        {
            if (poisonTimer <= 0 && poisoned)
            {
                StopCoroutine(playerProps.PoisonedColorTraits());
                poisonedParticles.SetActive(false);
                poisonTimer = poisonTimerReset;
                pickupType = PlayerCollectibles.NoPickUp;
                playerMovement.CancelInvoke("CheckRemovedPod");
                GameMenuController.gmmInstance.poisonText.SetActive(false);
                poisoned = false;
                GameMenuController.gmmInstance.CancelText();
            }
        }

        if (comboActive)
        {
            if (comboActive && podCounter >= 1)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0.0f)
                {
                    hasPod = false;
                    comboActive = false;
                    podCounter = 0;
                    comboTimer = resetComboTimer;
                }
            }
        }

        if (invincible)
        {
            beeAttack = false;
            if (invincibleTime > 0 && invincible)
            {
                GameMenuController.gmmInstance.Text();
                invincibleTime -= Time.deltaTime;
            }
            else if (invincibleTime <= 0 && invincible)
            {
                starCounter = 0;
                invincibleTime = resetInvincibleTime;
                invincible = false;
                playerState = PlayerStats.Normal;
                pickupType = PlayerCollectibles.NoPickUp;
                invincibilityParticles.SetActive(false);
                GameMenuController.gmmInstance.CancelText();
                invincibleTime = resetInvincibleTime;
                GameMenuController.gmmInstance.invincibleText.SetActive(false);
                playerState = PlayerStats.Normal;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D iHit)
    {
        int playValue;
        string objectName = iHit.tag;
        int fruitNumber;
        int randomVoice;

        switch (objectName)
        {
            case "strawberry":
                if (DataController.instance.firstPlay && !tutPickUp)
                {
                    tutPickUp = true;
                    GameMenuController.gmmInstance.CollectiblePopUp();
                }
                var myStrawberry = iHit.gameObject.GetComponent<Fruit>();
                myStrawberry.HandleDeath();
                fruitNumber = (int)PlayerCollectibles.Strawberry;
                randomVoice = Random.Range(0, 3);
                SoundManager.soundInstance.FruitSound(randomVoice);
                ScoreManager.sInstance.CheckWhatFruitIAm(fruitNumber);
                Destroy(iHit.gameObject);
                break;

            case "cherry":
                if (DataController.instance.firstPlay && !tutPickUp)
                {
                    tutPickUp = true;
                    GameMenuController.gmmInstance.CollectiblePopUp();
                }
                var cherry = iHit.gameObject.GetComponent<Fruit>();
                cherry.HandleDeath();
                fruitNumber = (int)PlayerCollectibles.Cherry;
                randomVoice = Random.Range(0, 3);
                SoundManager.soundInstance.FruitSound(randomVoice);
                ScoreManager.sInstance.CheckWhatFruitIAm(fruitNumber);
                Destroy(iHit.gameObject);
                break;

            case "orange":
                if (DataController.instance.firstPlay && !tutPickUp)
                {
                    tutPickUp = true;
                    GameMenuController.gmmInstance.CollectiblePopUp();
                }
                var orange = iHit.gameObject.GetComponent<Fruit>();
                orange.HandleDeath();
                fruitNumber = (int)PlayerCollectibles.Orange;
                randomVoice = Random.Range(0, 3);
                SoundManager.soundInstance.FruitSound(randomVoice);
                ScoreManager.sInstance.CheckWhatFruitIAm(fruitNumber);
                Destroy(iHit.gameObject);
                break;

            case "star":
                if (DataController.instance.firstPlay && !tutPickUp)
                {
                    tutPickUp = true;
                    GameMenuController.gmmInstance.CollectiblePopUp();
                }
                gotStar = true;
                var star = iHit.gameObject.GetComponent<Star>();
                star.HandleDeath();
                SoundManager.soundInstance.StarSound();
                starCounter++;
                if (starCounter >= 10)
                {

                    playerBoundaries.gameObject.SetActive(true);
                    invincible = true;
                    ScoreManager.sInstance.invincibleCount++;
                    if (ScoreManager.sInstance.invincibleCount >= 2)
                    {
                        //GameServices.instance.AchievementUnlocked("InvincibleMuch");
                        //GameServices.instance.AchievementUnlocked("invincSenty2020");
                    }
                    if (invincible)
                    {
                        if (beenReversed)
                        {
                            playerMovement.reversedTime = false;
                            reversalParticles.SetActive(false);
                            reverseTimer = reverseTimerReset;
                            beenReversed = false;
                        }

                        if (poisoned)
                        {
                            poisonedParticles.SetActive(false);
                            playerMovement.CancelInvoke("CheckRemovedPod");
                            poisonTimer = poisonTimerReset;
                            poisoned = false;
                        }
                        playerState = PlayerStats.Invincible;
                        playValue = (int)playerState;
                        invincibilityParticles.SetActive(true);
                        playerProps.WhatToDo(playValue);
                    }
                    else
                    {
                        playerState = PlayerStats.Normal;
                        invincible = false;
                    }
                }
                Destroy(iHit.gameObject);
                gotStar = false;
                break;

            case "Pod":
                if (DataController.instance.firstPlay && !tutPod)
                {
                    tutPod = true;
                    GameMenuController.gmmInstance.PodPopUp();
                }
                GameObject myPod = iHit.gameObject;
                var getPod = myPod.GetComponent<Pod>();
                getPod.HandleMe();
                comboActive = true;
                pickupType = PlayerCollectibles.Pod;
                playValue = (int)pickupType;
                playerProps.PickUp(playValue);
                playerMovement.PodPlacement(iHit.gameObject);
                podCounter++;
                SoundManager.soundInstance.PodCollectSound();
                if (comboActive && podCounter >= 1)
                {
                    StartCoroutine(CheckForCombo(comboTimer));
                }
                Destroy(iHit.gameObject);
                break;

            case "Barriers":
                if (playerProps.shockwaveEffect || invincible || specialInvincible)
                {
                    wallHit = false;
                }
                else
                {
                    wallHit = true;
                }

                if (wallHit)
                {
                    hasDied = true;
                    playerState = PlayerStats.Dead;
                    playValue = (int)playerState;
                    playerProps.WhatToDo(playValue);
                }
                break;

            case "Obstacle":
                if (!invincible || !playerProps.shockwaveEffect || !specialInvincible)
                {
                    obstacleHit = true;
                }
                if (obstacleHit)
                {
                    playerState = PlayerStats.Dead;
                    hasDied = true;
                    playValue = (int)playerState;
                    playerProps.WhatToDo(playValue);
                }
                break;

            case "Land":
                var landers = iHit.gameObject.GetComponent<Land>();
                landers.HandleMe();
                if (!playerProps.shockwaveEffect
                && !invincible
                && !specialInvincible)
                {
                    landHit = true;
                    hasDied = true;
                    playerState = PlayerStats.Dead;
                    playValue = (int)playerState;
                    playerProps.WhatToDo(playValue);
                }

                if (playerProps.shockwaveEffect || invincible || specialInvincible)
                {
                    hasDied = false;
                    landCounter++;
                    landHit = false;
                    comboActive = true;
                    if (comboActive && landCounter >= 1)
                    {
                        StartCoroutine(CheckForCombo(comboTimer));
                    }
                    playerState = PlayerStats.Normal;
                }
                Destroy(iHit.gameObject);
                break;

            case "SpiderLand":
                var lander = iHit.gameObject.GetComponent<Land>();
                lander.HandleMe();
                if (!invincible && !playerProps.shockwaveEffect && !specialInvincible && !playerProps.usedSpecial)
                {
                    hasDied = true;
                    playerState = PlayerStats.Dead;
                    playValue = (int)playerState;
                    playerProps.WhatToDo(playValue);
                }

                if (invincible || playerProps.shockwaveEffect || playerProps.usedSpecial)
                {
                    landCounter++;
                    comboActive = true;
                    ScoreManager.sInstance.spiderKillCount++;
                    if (ScoreManager.sInstance.spiderKillCount == 4)
                    {
                        //GameServices.instance.AchievementUnlocked("SpiderKiller");
                        //GameServices.instance.AchievementUnlocked("spiderSenty2020");
                    }
                    if (comboActive && landCounter >= 1)
                    {
                        StartCoroutine(CheckForCombo(comboTimer));
                    }
                    playerState = PlayerStats.Normal;
                    Destroy(iHit.gameObject);
                }
                break;

            case "Crate":
                var getCrate = iHit.gameObject.GetComponent<Crate>();
                getCrate.DestroyCrate();
                SoundManager.soundInstance.TNTSound();
                if (!playerProps.shockwaveEffect && !invincible && !specialInvincible && !playerProps.usedSpecial)
                {
                    hitCrate = true;
                    hasDied = true;
                    playerState = PlayerStats.Dead;
                    playValue = (int)playerState;
                    playerProps.WhatToDo(playValue);
                }
                else if (invincible || playerProps.shockwaveEffect || specialInvincible || playerProps.usedSpecial)
                {
                    crateCounter++;
                    comboActive = true;
                    if (comboActive && crateCounter >= 1)
                    {
                        StartCoroutine(CheckForCombo(comboTimer));
                    }
                }
                Destroy(iHit.gameObject);
                break;

            case "explosive-obstacle":
                var tnt = iHit.gameObject.GetComponent<TNT>();
                tnt.Explosive();
                SoundManager.soundInstance.TNTSound();
                if (!playerProps.shockwaveEffect && !invincible && !specialInvincible && !!playerProps.usedSpecial)
                {
                    explosionDeath = true;
                    if (explosionDeath)
                    {
                        hasDied = true;
                        playerState = PlayerStats.Exploded;
                        playValue = (int)playerState;
                        playerProps.WhatToDo(playValue);
                    }
                }

                else if (invincible || playerProps.shockwaveEffect || specialInvincible || playerProps.usedSpecial)
                {
                    crateCounter++;
                    comboActive = true;
                    if (comboActive && crateCounter >= 1)
                    {
                        StartCoroutine(CheckForCombo(comboTimer));
                    }
                }
                Destroy(iHit.gameObject);
                break;
            case "Reversal":
                if (DataController.instance.firstPlay && !tutPickUp)
                {
                    tutPickUp = true;
                    GameMenuController.gmmInstance.CollectiblePopUp();
                }
                SoundManager.soundInstance.ReversedSound();
                if (!invincible && !playerProps.shockwaveEffect && !playerProps.usedSpecial && !specialInvincible)
                {
                    beenReversed = true;
                    if (beenReversed)
                    {
                        playerState = PlayerStats.Reversed;
                        playValue = (int)playerState;
                        reversalParticles.SetActive(true);
                        playerProps.WhatToDo(playValue);
                    }
                    else
                    {
                        playerState = PlayerStats.Normal;
                        playValue = (int)playerState;
                        reversalParticles.SetActive(false);
                        beenReversed = false;
                    }
                }
                Destroy(iHit.gameObject);
                break;

            case "spike":

                SoundManager.soundInstance.SpikeHit();
                if (!playerProps.shockwaveEffect && !invincible && !specialInvincible && !playerProps.usedSpecial)
                {
                    stabbed = true;
                    hasDied = true;
                    var spikeParticle = iHit.gameObject.GetComponent<Spike>();
                    spikeParticle.HandleMe();
                    playerState = PlayerStats.Stabbed;
                    playValue = (int)playerState;
                    playerProps.WhatToDo(playValue);
                }
                Destroy(iHit.gameObject);
                break;

            case "skull":
                if (DataController.instance.firstPlay && !tutPickUp)
                {
                    tutPickUp = true;
                    GameMenuController.gmmInstance.CollectiblePopUp();
                }
                if (!invincible && !playerProps.shockwaveEffect && !specialInvincible && !playerProps.usedSpecial)
                {
                    poisoned = true;
                    if (poisoned)
                    {
                        poisonedParticles.SetActive(true);
                        playerState = PlayerStats.Poisoned;
                        playValue = (int)playerState;
                        playerProps.WhatToDo(playValue);
                        playerMovement.InvokeRepeating("CheckRemovedPod", 2.0f, 6.0f);
                    }
                    else
                    {
                        poisoned = false;
                        poisonTimer = poisonTimerReset;
                        poisonedParticles.SetActive(false);
                    }
                }
                Destroy(iHit.gameObject);
                break;

            case "Stuck":
                glueTrap = true;
                playerState = PlayerStats.Trapped;
                playValue = (int)playerState;
                playerProps.WhatToDo(playValue);
                Destroy(iHit.gameObject, 0.25f);
                break;

            case "Vehicle":
                GameObject vehicleBoom = iHit.gameObject;
                var getBoom = vehicleBoom.GetComponent<Vehicle>();
                getBoom.HandleDeath();
                SoundManager.soundInstance.VehicleExplosion();
                if (!invincible && !playerProps.shockwaveEffect && !specialInvincible)
                {
                    hitByCar = true;
                    if (hitByCar)
                    {
                        //GameServices.instance.AchievementUnlocked("RoadKill");
                        hasDied = true;
                        playerState = PlayerStats.CarExplosion;
                        playValue = (int)playerState;
                        playerProps.WhatToDo(playValue);
                    }
                }
                break;

            case "Cone":
                var cone = iHit.gameObject.GetComponent<Cone>();
                cone.HandleDeath();
                if (!invincible && !playerProps.shockwaveEffect && !specialInvincible)
                {
                    hasDied = true;
                    playerState = PlayerStats.Coned;
                    playValue = (int)playerState;
                    playerProps.WhatToDo(playValue);
                }
                else
                {
                    playerState = PlayerStats.Normal;
                    hasDied = false;
                }
                break;

            case "Bees":
                //eventually we will make a time based 
                SoundManager.soundInstance.BeeStings();
                beeStingParticles.SetActive(true);
                break;
        }
    }
    public IEnumerator CheckForCombo(float timer)
    {
        if (timer > 0)
        {
            switch (podCounter)
            {
                case 2:
                    var myNums = Instantiate(playerProps.nums[0], transform.position, transform.rotation);
                    Destroy(myNums.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 25;
                    GameMenuController.gmmInstance.CheckSpecialMove(25f);
                    break;
                case 3:
                    var myNums2 = Instantiate(playerProps.nums[1], transform.position, transform.rotation);
                    Destroy(myNums2.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 50;
                    GameMenuController.gmmInstance.CheckSpecialMove(50f);
                    break;
                case 4:
                    var myNums3 = Instantiate(playerProps.nums[2], transform.position, transform.rotation);
                    Destroy(myNums3.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 100;
                    GameMenuController.gmmInstance.CheckSpecialMove(100f);
                    break;
                case 5:
                    var myNums4 = Instantiate(playerProps.nums[3], transform.position, transform.rotation);
                    Destroy(myNums4.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 200;
                    GameMenuController.gmmInstance.CheckSpecialMove(200f);
                    break;
                case 6:
                    var myNums5 = Instantiate(playerProps.nums[4], transform.position, transform.rotation);
                    Destroy(myNums5.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 300;
                    GameMenuController.gmmInstance.CheckSpecialMove(300f);
                    break;
                case 7:
                    var myNums6 = Instantiate(playerProps.nums[5], transform.position, transform.rotation);
                    Destroy(myNums6.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 400;
                    GameMenuController.gmmInstance.CheckSpecialMove(400f);
                    break;
                case 8:
                    var myNums7 = Instantiate(playerProps.nums[6], transform.position, transform.rotation);
                    Destroy(myNums7.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 500;
                    GameMenuController.gmmInstance.CheckSpecialMove(500f);
                    break;
                case 9:
                    var myNums8 = Instantiate(playerProps.nums[6], transform.position, transform.rotation);
                    Destroy(myNums8.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 500;
                    GameMenuController.gmmInstance.CheckSpecialMove(500f);
                    break;
                case 10:
                    var myNums9 = Instantiate(playerProps.nums[6], transform.position, transform.rotation);
                    Destroy(myNums9.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 500;
                    GameMenuController.gmmInstance.CheckSpecialMove(500f);
                    break;
                case 11:
                    var myNums10 = Instantiate(playerProps.nums[6], transform.position, transform.rotation);
                    Destroy(myNums10.gameObject, 0.75f);
                    ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 500;
                    GameMenuController.gmmInstance.CheckSpecialMove(500f);
                    break;
            }

            if (invincible || playerProps.shockwaveEffect)
            {
                switch (landCounter)
                {
                    case 1:
                        var myNums1 = Instantiate(playerProps.nums[0], transform.position, transform.rotation);
                        Destroy(myNums1.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 25;
                        GameMenuController.gmmInstance.CheckSpecialMove(25f);
                        break;
                    case 2:
                        var myNums2 = Instantiate(playerProps.nums[1], transform.position, transform.rotation);
                        Destroy(myNums2.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 50;
                        GameMenuController.gmmInstance.CheckSpecialMove(50f);
                        break;
                    case 3:
                        var myNums3 = Instantiate(playerProps.nums[2], transform.position, transform.rotation);
                        Destroy(myNums3.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 100;
                        GameMenuController.gmmInstance.CheckSpecialMove(100f);
                        break;
                    case 4:
                        var myNums4 = Instantiate(playerProps.nums[3], transform.position, transform.rotation);
                        Destroy(myNums4.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 200;
                        GameMenuController.gmmInstance.CheckSpecialMove(200f);
                        break;
                    case 5:
                        var myNums5 = Instantiate(playerProps.nums[4], transform.position, transform.rotation);
                        Destroy(myNums5.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 300;
                        GameMenuController.gmmInstance.CheckSpecialMove(300f);
                        break;
                    case 6:
                        var myNums6 = Instantiate(playerProps.nums[5], transform.position, transform.rotation);
                        Destroy(myNums6.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 400;
                        GameMenuController.gmmInstance.CheckSpecialMove(400f);
                        break;
                    case 7:
                        var myNums = Instantiate(playerProps.nums[6], transform.position, transform.rotation);
                        Destroy(myNums.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 500;
                        GameMenuController.gmmInstance.CheckSpecialMove(500f);
                        break;
                    case 8:
                        var myNums7 = Instantiate(playerProps.nums[6], transform.position, transform.rotation);
                        Destroy(myNums7.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 500;
                        GameMenuController.gmmInstance.CheckSpecialMove(500f);
                        break;
                    case 9:
                        var myNums8 = Instantiate(playerProps.nums[6], transform.position, transform.rotation);
                        Destroy(myNums8.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 500;
                        GameMenuController.gmmInstance.CheckSpecialMove(500f);
                        break;
                    case 10:
                        var myNums9 = Instantiate(playerProps.nums[6], transform.position, transform.rotation);
                        Destroy(myNums9.gameObject, 0.75f);
                        ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 500;
                        GameMenuController.gmmInstance.CheckSpecialMove(500f);
                        break;
                }
                if (invincible || playerProps.shockwaveEffect)
                {
                    switch (crateCounter)
                    {
                        case 1:
                            var myNums1 = Instantiate(playerProps.nums[0], transform.position, transform.rotation);
                            Destroy(myNums1.gameObject, 0.75f);
                            ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 25;
                            GameMenuController.gmmInstance.CheckSpecialMove(50f);
                            break;
                        case 2:
                            var myNums2 = Instantiate(playerProps.nums[1], transform.position, transform.rotation);
                            Destroy(myNums2.gameObject, 0.75f);
                            ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 50;
                            GameMenuController.gmmInstance.CheckSpecialMove(100f);
                            break;
                        case 3:
                            var myNums3 = Instantiate(playerProps.nums[2], transform.position, transform.rotation);
                            Destroy(myNums3.gameObject, 0.75f);
                            ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 100;
                            GameMenuController.gmmInstance.CheckSpecialMove(200f);
                            break;
                        case 4:
                            var myNums4 = Instantiate(playerProps.nums[3], transform.position, transform.rotation);
                            Destroy(myNums4.gameObject, 0.75f);
                            ScoreManager.sInstance.scoreValue = ScoreManager.sInstance.scoreValue + 200;
                            GameMenuController.gmmInstance.CheckSpecialMove(300f);
                            break;
                    }
                }
            }

            yield return new WaitForSeconds(timer);
            podCounter = 0;
            landCounter = 0;
            hasPod = false;
            comboActive = false;
            comboTimer = resetComboTimer;
            StopCoroutine(CheckForCombo(timer));
        }
    }//ComboActivator

    public void ResetItems()
    {
        playerState = PlayerStats.Normal;
        StopCoroutine(GlueRelease());
        poisonTimer = poisonTimerReset;
        glueTimer = glueTimerReset;
        reverseTimer = reverseTimerReset;
        hasDied = false;
        explosionDeath = false;
        poisoned = false;
        stabbed = false;
        reversalParticles.SetActive(false);
        invincibilityParticles.SetActive(false);
        poisonedParticles.SetActive(false);
    }


    //This gets rid of glue trap pop ups
    public IEnumerator GlueRelease()
    {
        if (glueTimer <= 0)
        {
            glueTimer = glueTimerReset;
            GameMenuController.gmmInstance.DestroySplat();
            glueTrap = false;
            playerState = PlayerStats.Normal;
            StopCoroutine(GlueRelease());
        }
        yield return new WaitForSeconds(6.0f);
        StartCoroutine(GlueRelease());
    }//This gets rid of glue objects on screen

}//End of Script
