using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Anything dealing with player speed controls or moving left to right and pod pickups
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D myBody;

    //My Movement
    float slideLeft = 3.2f;
    float slideRight = 3.2f;

    float followCamera;
    float touchTimeStart, touchTimeFinish;
    Transform grabPlayer;
    Transform head;
    public bool havePressedButton;
    [Header("Pod Speed Movement")]
    float podSpeedLeft = .13f;
    float podSpeedRight = .13f;
    float nextPodY = .4f;

    float reversePodSpeedL;
    float reversePodSpeedR;
    float LerpTimeX = .09f;
    float LerpTimeY = 0f;
    float dist;
    GameObject lastPod;
    Transform curBodyPart;
    Transform prevBodyPart;
    PlayerStatus playerStats;
    PlayerProperties playerProps;

    SpriteRenderer mySprite;
    public bool reversedTime;
    float reversalTimeLimit = 10.5f;
    Vector2 replacePos, startTouch, endTouch;
    public Vector2 currentPos;
    float reverseRecall = 10.5f;

    public List<Transform> podContainer = new List<Transform>();

    PlayerSprite playSprite;

    private void Awake()
    {
        grabPlayer = this.transform.GetChild(0);
        head = grabPlayer.transform.GetChild(0);
        myBody = this.GetComponent<Rigidbody2D>();
        playerStats = this.GetComponent<PlayerStatus>();
        playerProps = this.GetComponent<PlayerProperties>();
        playSprite = grabPlayer.GetComponent<PlayerSprite>();
        mySprite = grabPlayer.GetComponent<SpriteRenderer>();
        podContainer.Add(head);
        lastPod = head.gameObject;
        curBodyPart = head;
        prevBodyPart = head;
        currentPos.x = 0f;
    }

    void Update()
    {
        currentPos.y = this.transform.position.y;
    }
    private void FixedUpdate()
    {
        replacePos = this.transform.position;
        followCamera = GameplayManager.instance.camMoveSpeed;
        if (reversedTime == false)
        {
            podSpeedLeft = .13f;
            podSpeedRight = .13f;
            Slider();
        }
        else
        {
            ReverseMovement();
        }

        if (podContainer.Count > 0 && havePressedButton)
        {
            reversePodSpeedL = podSpeedLeft;
            reversePodSpeedR = podSpeedRight;
            podContainer[0].Translate(Vector2.left * reversePodSpeedL * Time.fixedDeltaTime);
            MovePod();
        }
        else if (podContainer.Count > 0 && !havePressedButton)
        {
            podContainer[0].Translate(Vector2.right * reversePodSpeedR * Time.fixedDeltaTime);
            MovePod();
        }

        //IF we have special ability then watch for swipe
        if (GameMenuController.gmmInstance.hasSpecial)
        {
            SwipeForSpecial();
        }

        SpaceForSpecial();
    }

    void Slider()
    {
        this.transform.position += new Vector3(0, -Time.smoothDeltaTime * followCamera);

        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            myBody.velocity = Vector2.left * slideLeft;
            havePressedButton = true;
            if (playSprite.hasFace)
            {
                mySprite.flipX = true;
            }
        }
        else
        {
            myBody.velocity = Vector2.right * slideRight;
            havePressedButton = false;
            mySprite.flipX = false;
        }
    }
    public void ReverseMovement()
    {
        this.transform.position += new Vector3(0, -Time.smoothDeltaTime * followCamera);
        if (!playerStats.invincible)
        {
            reversalTimeLimit -= Time.deltaTime;

            if (reversalTimeLimit > 0)
            {

                if (Input.GetMouseButton(0) || Input.touchCount > 0)
                {
                    myBody.velocity = Vector2.right * slideRight;
                    havePressedButton = true;
                    if (playSprite.hasFace)
                    {
                        mySprite.flipX = false;
                    }

                }
                else
                {
                    myBody.velocity = Vector2.left * slideLeft;
                    havePressedButton = false;
                    if (playSprite.hasFace)
                    {
                        mySprite.flipX = true;
                    }
                }
            }
            else
            {
                reversedTime = false;
                GameMenuController.gmmInstance.reverseText.SetActive(false);
                reversalTimeLimit = reverseRecall;
                playerStats.beenReversed = false;
                playerStats.playerState = PlayerStatus.PlayerStats.Normal;
            }
        }
        else
        {
            GameMenuController.gmmInstance.reverseText.SetActive(false);
        }
    }

    private void MovePod()
    {
        float distance = 0f;
        if (podContainer.Count > 1 && havePressedButton)
        {

            podContainer[1].GetComponent<Rigidbody2D>().AddForce(Vector2.left * podSpeedLeft * this.transform.position);

            for (int i = 1; i < podContainer.Count; i++)
            {
                curBodyPart = podContainer[i];

                if (podContainer.Count <= 1)
                {
                    prevBodyPart = curBodyPart;
                }
                else
                {
                    prevBodyPart = podContainer[i - 1];
                }


                distance = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

                Vector3 newPos = prevBodyPart.position;

                newPos.z = podContainer[0].position.z;

                //Try 2 Lerps, one on the x pos and one on the Y
                Vector3 pos = curBodyPart.position;

                pos.x = Mathf.Lerp(pos.x, newPos.x, LerpTimeX);
                pos.y = Mathf.Lerp(pos.y, newPos.y, LerpTimeY);

                curBodyPart.position = pos;
            }
        }
        else
        {
            if (podContainer.Count > 1 && !havePressedButton)
            {

                podContainer[1].GetComponent<Rigidbody2D>().AddForce(Vector2.right * podSpeedRight * this.transform.position);

                for (int i = 1; i < podContainer.Count; i++)
                {
                    curBodyPart = podContainer[i];
                    if (podContainer.Count <= 1)
                    {
                        prevBodyPart = curBodyPart;
                    }
                    else
                    {
                        prevBodyPart = podContainer[i - 1];
                    }

                    distance = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

                    Vector3 newPos = prevBodyPart.position;

                    newPos.z = podContainer[0].position.z;

                    //Try 2 Lerps, one on the x pos and one on the Y
                    Vector3 pos = curBodyPart.position;

                    pos.x = Mathf.Lerp(pos.x, newPos.x, LerpTimeX);
                    pos.y = Mathf.Lerp(pos.y, newPos.y, LerpTimeY);

                    curBodyPart.position = pos;
                }
            }
        }
    }

    //Remember to make the current pod grabbed the lastpod for the poisoned attribute
    public void PodPlacement(GameObject myPod)
    {
        Animator podAnim;
        Transform getPod;
        Transform internalPod;
        GameObject intPodParticle;
        Destroy(myPod);
        myPod = Instantiate(myPod.gameObject, transform.position, transform.rotation);
        internalPod = myPod.transform.GetChild(0);
        intPodParticle = internalPod.transform.GetChild(1).gameObject;
        Destroy(intPodParticle);

        var newPlacement = myPod.transform;
        newPlacement.SetParent(head.transform);
        getPod = myPod.transform.GetChild(0);
        podAnim = getPod.GetComponent<Animator>();
        podAnim.enabled = true;
        if (ScoreManager.sInstance.podCount <= 0)
        {
            myPod.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1.2f);
            lastPod = myPod;
            podContainer.Add(lastPod.transform);
            ScoreManager.sInstance.podCount++;
        }
        else if (ScoreManager.sInstance.podCount >= 1)
        {
            myPod.transform.position = new Vector2(lastPod.gameObject.transform.position.x, lastPod.gameObject.transform.position.y + nextPodY);
            lastPod = myPod;
            podContainer.Add(lastPod.transform);
            ScoreManager.sInstance.podCount++;
        }
        else
        {
            playerStats.hasPod = false;
        }
    }

    public void CheckRemovedPod()
    {
        ScoreManager.sInstance.podCount--;
        int lastCountPod = podContainer.Count - 1;
        for (int pNum = 0; pNum <= lastCountPod; pNum++)
        {
            if (pNum == lastCountPod)
            {
                if (curBodyPart.tag == "Head")
                {
                    GameMenuController.gmmInstance.GameOver();
                }
                podContainer.RemoveAt(pNum);
                //Explosion for the internal pods
                //var grabbedPod = Instantiate(playerStats.podExplode, lastPod.transform.position, lastPod.transform.rotation);
                Destroy(lastPod);
                //Destroy(grabbedPod.gameObject, 0.4f);

                curBodyPart = prevBodyPart;
                lastPod = curBodyPart.gameObject;
            }
        }
    }

    public void SecondChancePlacement()
    {
        currentPos = new Vector2(0, currentPos.y - 2.5f);
        this.transform.position = currentPos;
    }

    void SpaceForSpecial()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameMenuController.gmmInstance.CallMySpecial();
        }
    }
    public void SwipeForSpecial()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouch = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouch = Input.GetTouch(0).position;

            if (endTouch.x > startTouch.x + 1.5f)
            {
                GameMenuController.gmmInstance.CallMySpecial();
            }
        }
    }
    public void CheckForSwipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouch = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouch = Input.GetTouch(0).position;

            if (endTouch.x > startTouch.x + 1.5f)
            {
                GameMenuController.gmmInstance.CallTutorialSwipe();
            }
        }
    }
}
