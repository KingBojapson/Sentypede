using System.Collections;
using UnityEngine;


// This is supposed to handle the properties of the player from invincibility to resurrecting back in the game
//This also goes hand in hand with the player status which should be tweaked
public class PlayerProperties : MonoBehaviour
{

    PlayerMovement playerMovement;
    PlayerStatus playerStatus;

    Transform playerPart;
    Transform internalP;

    public GameObject playerDeath;
    PolygonCollider2D playerBod;

    public GameObject[] nums;
    public GameObject[] specialProjectile;

    public bool shockwaveEffect, usedSpecial;
    Transform myShockwave;

    SpriteRenderer internalPodColor;
    public SpriteRenderer SR;

    //This is to delay the 
    public float guiTimer = 1.5f;


    bool alreadyUsed;
    //Achievement variables


    //Gathering attributes for specials
    Rigidbody2D specialProjectileRB;
    float projectileSpeed;
    float iceballForce = 250f;

    float iceballRelease = .2f;
    float iceballReleaseReturn = .2f;

    Coroutine specialShoot = null;
    Coroutine specialEnd = null;

    //Animation
    Animator playerAnim;
    private void Awake()
    {
        //this gets access to the internal components of the player
        //This purpose is solely for animation if you want the components independent of the main character
        internalP = this.transform.GetChild(0);
        playerAnim = internalP.GetComponent<Animator>();
        playerBod = internalP.GetComponent<PolygonCollider2D>();
        playerMovement = this.GetComponent<PlayerMovement>();
        playerStatus = this.GetComponent<PlayerStatus>();
        SR = internalP.GetComponent<SpriteRenderer>();
        //for specials and shockwave
        myShockwave = internalP.GetChild(5);
        playerAnim.SetBool("hasDied", false);
    }
    public void WhatToDo(int playStateValue)
    {
        switch (playStateValue)
        {
            case 1:
                if (playerStatus.hasDied && playerStatus.lives < 1)
                {
                    SR.enabled = false;
                    playerBod.enabled = false;
                    SoundManager.soundInstance.Die();
                    playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
                    GameplayManager.instance.GameOver();
                    DataController.instance.finishedFirstPlay = true;
                    CameraShake.cmInstance.Shake(.25f, .22f);
                    var explosion = Instantiate(playerDeath.gameObject, transform.position, transform.rotation);
                    SoundManager.soundInstance.Die();
                    Destroy(explosion.gameObject, 0.5f);
                }
                else if (playerStatus.hasDied && playerStatus.lives >= 1)
                {
                    SR.enabled = false;
                    playerBod.enabled = false;
                    CameraShake.cmInstance.Shake(.25f, .22f);
                    var explosion = Instantiate(playerDeath.gameObject, transform.position, transform.rotation);
                    SoundManager.soundInstance.Die();
                    Destroy(explosion.gameObject, 0.5f);
                    playerStatus.lives = playerStatus.lives - 1;
                    GameMenuController.gmmInstance.RemoveLife(playerStatus.lives);
                    if (DataController.instance.firstPlay && !alreadyUsed)
                    {
                        alreadyUsed = true;
                        GameMenuController.gmmInstance.DeathPopUp();

                    }
                    shockwaveEffect = true;
                }
                break;
            case 2:
                //This is for my reversals
                if (playerStatus.beenReversed)
                {
                    playerMovement.reversedTime = true;
                    GameMenuController.gmmInstance.Text();
                    //activate particle system for reversals
                }
                else
                {
                    playerStatus.beenReversed = false;
                    playerMovement.reversedTime = false;
                }
                break;
            case 3:
                //this is for invincibility
                if (playerStatus.invincible)
                {
                    StartCoroutine(ActivateInvincibility());
                    StopCoroutine(PoisonedColorTraits());
                }
                break;
            case 4:
                int random = Random.Range(0, 8);
                StartCoroutine(playerStatus.GlueRelease());
                GameMenuController.gmmInstance.CallMyTrap(random);
                break;
            case 5:
                //This will handle killed by tnt
                if (playerStatus.hasDied && playerStatus.explosionDeath && playerStatus.lives < 1)
                {
                    CameraShake.cmInstance.Shake(.35f, .25f);
                    SoundManager.soundInstance.Die();
                    GameplayManager.instance.GameOver();
                    SR.enabled = false;
                    playerBod.enabled = false;
                }
                else
                {
                    CameraShake.cmInstance.Shake(.35f, .25f);
                    SoundManager.soundInstance.Die();
                    SR.enabled = false;
                    playerBod.enabled = false;
                    playerStatus.lives = playerStatus.lives - 1;
                    GameMenuController.gmmInstance.RemoveLife(playerStatus.lives);
                }
                break;
            case 6:
                //This will handle killed by vehicle
                if (playerStatus.hasDied && playerStatus.hitByCar && playerStatus.lives < 1)
                {
                    CameraShake.cmInstance.Shake(.35f, .25f);
                    SoundManager.soundInstance.Die();
                    GameplayManager.instance.GameOver();
                    SR.enabled = false;
                    playerBod.enabled = false;
                }
                else if (playerStatus.hasDied && playerStatus.hitByCar && playerStatus.lives > 1)
                {
                    CameraShake.cmInstance.Shake(.35f, .25f);
                    SoundManager.soundInstance.Die();
                    SR.enabled = false;
                    playerBod.enabled = false;
                    playerStatus.lives = playerStatus.lives - 1;
                    GameMenuController.gmmInstance.RemoveLife(playerStatus.lives);
                }
                break;
            case 7:
                //This will handle my stabs
                if (playerStatus.hasDied && playerStatus.stabbed && playerStatus.lives < 1)
                {
                    CameraShake.cmInstance.Shake(.35f, .25f);
                    GameplayManager.instance.GameOver();
                    SoundManager.soundInstance.Die();
                }
                else
                {
                    SR.enabled = false;
                    playerBod.enabled = false;
                    SoundManager.soundInstance.Die();
                    playerStatus.lives = playerStatus.lives - 1;
                    GameMenuController.gmmInstance.RemoveLife(playerStatus.lives);
                }
                break;
            case 9:
                PoisonedTraits(playerStatus.poisonTimer);
                StartCoroutine(PoisonedColorTraits());
                GameMenuController.gmmInstance.Text();
                if (playerStatus.hasDied && playerStatus.lives < 1)
                {
                    GameplayManager.instance.GameOver();
                    SoundManager.soundInstance.Die();
                }
                else if (playerStatus.lives >= 1 && playerStatus.hasDied)
                {
                    SR.enabled = false;
                    playerBod.enabled = false;
                    SoundManager.soundInstance.Die();
                    playerStatus.lives = playerStatus.lives - 1;
                    GameMenuController.gmmInstance.RemoveLife(playerStatus.lives);
                }
                break;
            case 10:
                if (playerStatus.poisoned)
                {
                    StopCoroutine(PoisonedColorTraits());
                    playerStatus.poisoned = false;
                }
                break;
            case 12:
                if (playerStatus.hasDied && playerStatus.lives <= 0)
                {
                    SoundManager.soundInstance.Die();
                    GameplayManager.instance.GameOver();
                }
                else if (playerStatus.hasDied && playerStatus.lives >= 1)
                {
                    SR.enabled = false;
                    playerBod.enabled = false;
                    SoundManager.soundInstance.Die();
                    playerStatus.lives = playerStatus.lives - 1;
                    GameMenuController.gmmInstance.RemoveLife(playerStatus.lives);
                }
                break;
        }
    }

    public void PickUp(int pickUpNumber)
    {
        switch (pickUpNumber)
        {
            case 0:
                playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
                playerStatus.pickupType = PlayerStatus.PlayerCollectibles.NoPickUp;
                break;
            case 1:
                playerStatus.pickupType = PlayerStatus.PlayerCollectibles.Pod;
                break;
        }

    }

    public void PoisonedTraits(float myTimeLimit)
    {
        if (myTimeLimit > 0 && playerStatus.poisoned)
        {
            if (playerMovement.podContainer.Count < 2)
            {
                playerStatus.hasDied = true;
            }
        }
        else
        {
            playerStatus.poisoned = false;
            playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
        }
    }//PoisonedTraits

    public IEnumerator BeeStingColorChange()
    {
        while (playerStatus.beeStingsTimer > 0 && playerStatus.beeAttack)
        {
            SR.color = new Color(0f / 255f, 208f / 255f, 155f / 255f);
            if (playerMovement.podContainer.Count > 1)
            {
                foreach (var pod in playerMovement.podContainer)
                {
                    if (pod.tag == "Head")
                    {
                        continue;
                    }
                    else
                    {
                        var parentPod = pod.GetChild(0);
                        internalPodColor = parentPod.GetComponent<SpriteRenderer>();
                        internalPodColor.color = new Color(0f / 255f, 208f / 255f, 155f / 255f);
                    }
                }
            }
            yield return new WaitForSeconds(1f);

            SR.color = Color.white;
            if (playerMovement.podContainer.Count > 1)
            {
                foreach (var pod in playerMovement.podContainer)
                {
                    if (pod.tag == "Head")
                    {
                        continue;
                    }
                    else
                    {
                        var parentPod = pod.GetChild(0);
                        internalPodColor = parentPod.GetComponent<SpriteRenderer>();
                        internalPodColor.color = Color.white;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(ActivateInvincibility());
        }
    }//Bee Sting Effect
    public IEnumerator PoisonedColorTraits()
    {
        while (playerStatus.poisonTimer > 0 && playerStatus.poisoned)
        {
            SR.color = new Color(63f / 255f, 42f / 255f, 152f / 255f);
            if (playerMovement.podContainer.Count > 1)
            {
                foreach (var pod in playerMovement.podContainer)
                {
                    if (pod.tag == "Head")
                    {
                        continue;
                    }
                    else
                    {
                        var internalPod = pod.GetChild(0);
                        internalPodColor = internalPod.GetComponent<SpriteRenderer>();
                        internalPodColor.color = new Color(63f / 255f, 42f / 255f, 152f / 255f);
                    }
                }
            }

            yield return new WaitForSeconds(1f);

            SR.color = Color.white;
            if (playerMovement.podContainer.Count > 1)
            {
                foreach (var pod in playerMovement.podContainer)
                {
                    if (pod.tag == "Head")
                    {
                        continue;
                    }
                    else
                    {
                        var internalPod = pod.GetChild(0);
                        internalPodColor = internalPod.GetComponent<SpriteRenderer>();
                        internalPodColor.color = Color.white;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(PoisonedColorTraits());
        }
        playerStatus.poisonTimer = playerStatus.poisonTimerReset;
        playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
        playerStatus.poisoned = false;
    }//PoisonedColor

    public IEnumerator ActivateInvincibility()
    {
        while (playerStatus.invincibleTime > 0 && playerStatus.invincible)
        {
            SR.color = new Color(0f / 255f, 208f / 255f, 155f / 255f);
            if (playerMovement.podContainer.Count > 1)
            {
                foreach (var pod in playerMovement.podContainer)
                {
                    if (pod.tag == "Head")
                    {
                        continue;
                    }
                    else
                    {
                        var parentPod = pod.GetChild(0);
                        internalPodColor = parentPod.GetComponent<SpriteRenderer>();
                        internalPodColor.color = new Color(0f / 255f, 208f / 255f, 155f / 255f);
                    }
                }
            }
            yield return new WaitForSeconds(1f);

            SR.color = Color.white;
            if (playerMovement.podContainer.Count > 1)
            {
                foreach (var pod in playerMovement.podContainer)
                {
                    if (pod.tag == "Head")
                    {
                        continue;
                    }
                    else
                    {
                        var parentPod = pod.GetChild(0);
                        internalPodColor = parentPod.GetComponent<SpriteRenderer>();
                        internalPodColor.color = Color.white;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(ActivateInvincibility());
        }
    }//End of Invincibility Color Changes

    //called during death
    public void Continue()
    {
        SR.enabled = true;
        playerBod.enabled = true;
        playerAnim.SetBool("hasDied", true);
    }

    //This checks to see if you have special abilities if you do go to the next stages
    public void SpecialMove(int myCharacter)
    {
        if (myCharacter == 3)
        {
            if (DataController.instance.firstSpecial)
            {
                GameMenuController.gmmInstance.SpecialMovePopUp();
            }
            shockwaveEffect = true;
            myShockwave.gameObject.SetActive(true);
            usedSpecial = true;
            GameMenuController.gmmInstance.Text();
            specialEnd = StartCoroutine(EndSpecials(myCharacter));
        }
        else if (myCharacter == 4)
        {
            if (DataController.instance.firstSpecial)
            {
                GameMenuController.gmmInstance.SpecialMovePopUp();
            }
            playerStatus.specialInvincible = true;
            usedSpecial = true;
            GameMenuController.gmmInstance.Text();
            playerStatus.playerState = PlayerStatus.PlayerStats.Invincible;
            specialEnd = StartCoroutine(EndSpecials(myCharacter));
        }
        else if (myCharacter == 5)
        {
            if (DataController.instance.firstSpecial)
            {
                GameMenuController.gmmInstance.SpecialMovePopUp();
            }
            usedSpecial = true;
            playerAnim.SetBool("usedSpecial", true);
            playerAnim.SetInteger("rabbit", myCharacter);
            GameMenuController.gmmInstance.Text();
            SR.sortingLayerName = "Land";
            specialEnd = StartCoroutine(EndSpecials(myCharacter));
        }
        else if (myCharacter == 6)
        {
            if (DataController.instance.firstSpecial)
            {
                GameMenuController.gmmInstance.SpecialMovePopUp();
            }
            usedSpecial = true;
            GameMenuController.gmmInstance.Text();
            specialShoot = StartCoroutine(ShootSpecial(myCharacter));
            specialEnd = StartCoroutine(EndSpecials(myCharacter));
        }
        else if (myCharacter == 7)
        {
            if (DataController.instance.firstSpecial)
            {
                GameMenuController.gmmInstance.SpecialMovePopUp();
            }
            playerStatus.specialInvincible = true;
            playerAnim.SetBool("usedSpecial", true);
            playerAnim.SetInteger("frank", myCharacter);
            usedSpecial = true;
            GameMenuController.gmmInstance.Text();
            specialEnd = StartCoroutine(EndSpecials(myCharacter));

        }

    }

    //This is really to shoot projectiles
    /*
    The Iceman - iceballs
    The Elephant - peanuts
    The Tank - missiles
    etc.
    */
    IEnumerator ShootSpecial(int characterIndex)
    {
        Rigidbody2D projectileBody;
        var iceball = Instantiate(specialProjectile[0], transform.position, transform.rotation) as GameObject;
        iceball.transform.Rotate(0f, 0f, 90f);
        projectileBody = iceball.GetComponent<Rigidbody2D>();
        projectileBody.AddForce(-Vector2.up * iceballForce);
        yield return new WaitForSeconds(1.1f);
        specialShoot = StartCoroutine(ShootSpecial(characterIndex));
    }

    //This end all specials after six seconds
    IEnumerator EndSpecials(int characterNumber)
    {
        yield return new WaitForSeconds(6f);
        if (characterNumber == 3)
        {
            shockwaveEffect = false;
            myShockwave.gameObject.SetActive(false);
            GameMenuController.gmmInstance.EndMySpecial();
            usedSpecial = false;
            GameMenuController.gmmInstance.CancelText();
        }
        else if (characterNumber == 4)
        {
            playerStatus.specialInvincible = false;
            usedSpecial = false;
            playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
            playerStatus.invincibilityParticles.SetActive(false);
            GameMenuController.gmmInstance.EndMySpecial();

            GameMenuController.gmmInstance.CancelText();
        }
        else if (characterNumber == 5)
        {
            playerAnim.SetBool("usedSpecial", false);
            playerAnim.SetInteger("rabbit", 0);
            SR.sortingLayerName = "Pod";
            playerStatus.specialInvincible = false;
            usedSpecial = false;
            playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
            GameMenuController.gmmInstance.EndMySpecial();

            GameMenuController.gmmInstance.CancelText();
        }
        else if (characterNumber == 6)
        {
            playerAnim.Play("PlayerBounce");
            SR.sortingLayerName = "Pod";
            StopCoroutine(specialShoot);
            usedSpecial = false;
            playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
            GameMenuController.gmmInstance.EndMySpecial();
            GameMenuController.gmmInstance.CancelText();
        }
        else if (characterNumber == 7)
        {
            playerStatus.specialInvincible = false;
            playerAnim.SetBool("usedSpecial", false);
            playerAnim.SetInteger("frank", 0);
            SR.sortingLayerName = "Pod";
            usedSpecial = false;
            playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
            GameMenuController.gmmInstance.EndMySpecial();

            GameMenuController.gmmInstance.CancelText();
        }
    }

}// End of Player Properties
