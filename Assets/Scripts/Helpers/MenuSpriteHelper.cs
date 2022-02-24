using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSpriteHelper : MonoBehaviour {

	public static MenuSpriteHelper instance;

	[Header("Menu Art Changes")]
	Sprite[] landCharacters;
	public Image landCharRend;
	string landCharPath;

	Sprite[] chooseChar;
	public Image chooseCharRend;
	string chooseCharPath;

	//This is for the character selection page image change
	Sprite[] charSelectionSprite;
	public Image charSelectionImage;
 
	GameObject myPlayer;

	private void Awake()
	{
		MakeInstance();
		landCharPath = "Art/UI/FCCharacter";
		chooseCharPath = "Art/Characters/FCSOne";
	}
	// Use this for initialization

	void Start () 
	{
		
	}

	void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

    }

    public void CheckingForCharacterChange()
	{
		landCharacters = Resources.LoadAll<Sprite>(landCharPath);
		chooseChar = Resources.LoadAll<Sprite>(chooseCharPath);
        charSelectionSprite = Resources.LoadAll<Sprite>(chooseCharPath);
        /*
        if (DataController.instance.selectedIndex == 0)
        {
			landCharRend.sprite = landCharacters[0];
			chooseCharRend.sprite = chooseChar[0];
            charSelectionImage.sprite = charSelectionSprite[0];
        }
        else if (DataController.instance.selectedIndex == 1)
        {
			landCharRend.sprite = landCharacters[1];
			chooseCharRend.sprite = chooseChar[1];
            charSelectionImage.sprite = charSelectionSprite[1];
        }
        else if (DataController.instance.selectedIndex == 2)
        {
			landCharRend.sprite = landCharacters[2];
			chooseCharRend.sprite = chooseChar[2];
            charSelectionImage.sprite = charSelectionSprite[2];
        }
        else if (DataController.instance.selectedIndex == 3)
        {
			landCharRend.sprite = landCharacters[3];
			chooseCharRend.sprite = chooseChar[3];
            charSelectionImage.sprite = charSelectionSprite[3];
        }
        else if (DataController.instance.selectedIndex == 4)
        {
			landCharRend.sprite = landCharacters[6];
			chooseCharRend.sprite = chooseChar[4];
            charSelectionImage.sprite = charSelectionSprite[4];
        }
        else if (DataController.instance.selectedIndex == 5)
        {
			landCharRend.sprite = landCharacters[7];
			chooseCharRend.sprite = chooseChar[5];
            charSelectionImage.sprite = charSelectionSprite[5];
        }
        else if (DataController.instance.selectedIndex == 6)
        {
			landCharRend.sprite = landCharacters[4];
			chooseCharRend.sprite = chooseChar[6];
            charSelectionImage.sprite = charSelectionSprite[6];
        }
        else if (DataController.instance.selectedIndex == 7)
        {
            landCharRend.sprite = landCharacters[5];
            chooseCharRend.sprite = chooseChar[7];
            charSelectionImage.sprite = charSelectionSprite[7];
        }
        */
    }

}//class
