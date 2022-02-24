using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    Transform myParent;
    PlayerMovement playerMovement;
    PlayerStatus playerStatus;
    PlayerProperties playerProps;
    public GameObject dustCloud;
    public GameObject[] halos;
    public GameObject shockwave;
    public Transform playerLocator;
    Animator myAnim;
    void Awake()
    {
        myParent = this.transform.parent;
        myAnim = this.GetComponent<Animator>();
        playerMovement = myParent.gameObject.GetComponent<PlayerMovement>();
        playerStatus = myParent.gameObject.GetComponent<PlayerStatus>();
        playerProps = myParent.gameObject.GetComponent<PlayerProperties>();
    }

    // After Dying Animations
    void MoveOutOfScreenView()
    {
        playerMovement.currentPos = new Vector2(3.97f, playerLocator.transform.position.y);
        myParent.transform.position = playerMovement.currentPos;
    }
    void MoveIntoScreenView()
    {
        playerMovement.currentPos = new Vector2(0f, playerMovement.currentPos.y);
        myParent.transform.position = playerMovement.currentPos;
        if (playerStatus.lives == 1)
        {
            halos[0].SetActive(true);
        }
        else if (playerStatus.lives < 1)
        {
            halos[1].SetActive(true);
        }
    }

    public void HandleDisableDeath()
    {
        StartCoroutine(ResetLife());
    }

    IEnumerator ResetLife()
    {
        yield return new WaitForSeconds(0f);
        dustCloud.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(ResetComps());
    }

    IEnumerator ResetComps()
    {
        yield return new WaitForSeconds(5f);
        if (this.transform.childCount > 4)
        {
            for (int child = 0; child < transform.childCount; child++)
            {
                Transform myChild = transform.GetChild(child);
                if (myChild.gameObject.tag == "particles")
                {
                    DestroyImmediate(myChild.gameObject);
                }
            }
        }
        shockwave.SetActive(false);
        playerStatus.stabbed = false;
        playerStatus.poisoned = false;
        playerStatus.hasDied = false;
        playerStatus.beenReversed = false;
        playerStatus.playerState = PlayerStatus.PlayerStats.Normal;
        playerStatus.pickupType = PlayerStatus.PlayerCollectibles.NoPickUp;
        playerProps.shockwaveEffect = false;
    }

    void Stomp()
    {
        myAnim.SetBool("hasDied", false);
        playerStatus.hasDied = false;
        dustCloud.SetActive(true);
        playerProps.shockwaveEffect = true;
        if (playerProps.shockwaveEffect)
        {
            shockwave.SetActive(true);
            playerStatus.playerBoundaries.gameObject.SetActive(true);
        }
        GameMenuController.gmmInstance.Resurrection();
    }
}
