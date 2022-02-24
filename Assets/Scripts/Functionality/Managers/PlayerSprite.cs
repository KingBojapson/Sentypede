using UnityEngine;
using UnityEngine.UI;


/*
 * Script: Character Controller
 * Written By: Michael Bolden
 * Started Date: 9/05/2020
 */


public class PlayerSprite : MonoBehaviour
{

    public static PlayerSprite pInstance;
    string characterPath;
    public Image myIcon;
    Sprite[] playerSprite;
    SpriteRenderer spriteRend;
    Sprite[] podSpriteSheet;
    GameObject[] myPods;


    SpriteRenderer myPodSpriteRend;
    string podPath;
    string prefabLocations;
    string splatLocation;

    public bool hasFace;

    //this will track my position and move the pods shortly after

    private void Awake()
    {
        prefabLocations = "Prefabs/Collectibles/Pod Holders";
        splatLocation = "Art/Blobs/Blobs";
        podPath = "Art/Characters/Pods/Pods";
        characterPath = "Art/Characters/FCSOne";
        spriteRend = GetComponent<SpriteRenderer>();
        ChangeMyPod();
        CheckForSpriteChange();
    }

    // Update is called once per frame
    public void CheckForSpriteChange()
    {
        playerSprite = Resources.LoadAll<Sprite>(characterPath);
        switch (DataController.instance.selectedIndex)
        {
            case 0:
                spriteRend.sprite = playerSprite[0];
                myIcon.sprite = playerSprite[0];
                break;
            case 1:
                spriteRend.sprite = playerSprite[1];
                myIcon.sprite = playerSprite[1];
                break;
            case 2:
                spriteRend.sprite = playerSprite[2];
                myIcon.sprite = playerSprite[2];
                break;
            case 3:
                spriteRend.sprite = playerSprite[3];
                myIcon.sprite = playerSprite[3];
                hasFace = true;
                break;
            case 4:
                spriteRend.sprite = playerSprite[4];
                myIcon.sprite = playerSprite[4];
                hasFace = true;
                break;
            case 5:
                spriteRend.sprite = playerSprite[5];
                myIcon.sprite = playerSprite[5];
                hasFace = true;
                break;
            case 6:
                spriteRend.sprite = playerSprite[6];
                myIcon.sprite = playerSprite[6];
                hasFace = true;
                break;
            case 7:
                spriteRend.sprite = playerSprite[7];
                myIcon.sprite = playerSprite[7];
                hasFace = true;
                break;
        }
    }//CheckForSpriteChange
    void ChangeMyPod()
    {
        podSpriteSheet = Resources.LoadAll<Sprite>(podPath);
        myPods = Resources.LoadAll<GameObject>(prefabLocations);

        for (int i = 0; i < myPods.Length; i++)
        {
            Transform theSprite = myPods[i].gameObject.transform;

            foreach (Transform indiePod in theSprite)
            {
                myPodSpriteRend = indiePod.GetComponentInChildren<SpriteRenderer>();

                if (DataController.instance.selectedIndex == 0)
                {
                    myPodSpriteRend.sprite = podSpriteSheet[4];
                }
                else if (DataController.instance.selectedIndex == 1)
                {
                    myPodSpriteRend.sprite = podSpriteSheet[8];
                }
                else if (DataController.instance.selectedIndex == 2)
                {
                    myPodSpriteRend.sprite = podSpriteSheet[10];
                }
                else if (DataController.instance.selectedIndex == 3)
                {
                    myPodSpriteRend.sprite = podSpriteSheet[14];
                }
                else if (DataController.instance.selectedIndex == 4)
                {
                    myPodSpriteRend.sprite = podSpriteSheet[3];
                }
                else if (DataController.instance.selectedIndex == 5)
                {
                    myPodSpriteRend.sprite = podSpriteSheet[0];
                }
                else if (DataController.instance.selectedIndex == 6)
                {
                    myPodSpriteRend.sprite = podSpriteSheet[12];
                }
                else if (DataController.instance.selectedIndex == 7)
                {
                    myPodSpriteRend.sprite = podSpriteSheet[6];
                }
            }
        }//PodSpriteChecker
    }
}//class
