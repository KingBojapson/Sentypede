using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{

    public GameObject[] availableChars;

    public GameObject[] specialText;

    public GameObject noSpecialText;

    public GameObject unlockFor;
    public GameObject selected;
    public GameObject notEnough;
    public GameObject minusHolder;

    public GameObject selectButton;
    public GameObject unlockButton;

    public GameObject lockIcon;
    Animator menuCharAnim;

    int currentIndex;
    public int convertPrice;

    string characterPrice;

    public Text myPods;
    public Text cost;
    public Text minus;


    bool[] chars;

    void Awake()
    {
        InitializeChars();
    }

    void InitializeChars()
    {
        currentIndex = DataController.instance.selectedIndex;

        for (int i = 0; i < availableChars.Length; i++)
        {
            availableChars[i].SetActive(false);
        }

        availableChars[currentIndex].SetActive(true);
        HandleSpecialsText();
        chars = DataController.instance.characters;
    }

    void HandleSpecialsText()
    {
        if (currentIndex == 0)
        {
            noSpecialText.SetActive(true);
            specialText[0].SetActive(false);
            specialText[1].SetActive(false);
            specialText[2].SetActive(false);
            specialText[3].SetActive(false);
            specialText[4].SetActive(false);
        }
        else if (currentIndex == 1)
        {
            noSpecialText.SetActive(true);
            specialText[0].SetActive(false);
            specialText[1].SetActive(false);
            specialText[2].SetActive(false);
            specialText[3].SetActive(false);
            specialText[4].SetActive(false);
        }
        else if (currentIndex == 2)
        {
            noSpecialText.SetActive(true);
            specialText[0].SetActive(false);
            specialText[1].SetActive(false);
            specialText[2].SetActive(false);
            specialText[3].SetActive(false);
            specialText[4].SetActive(false);
        }
        else if (currentIndex == 3)
        {
            noSpecialText.SetActive(false);
            specialText[0].SetActive(true);
            specialText[1].SetActive(false);
        }
        else if (currentIndex == 4)
        {
            noSpecialText.SetActive(false);
            specialText[0].SetActive(false);
            specialText[1].SetActive(true);
            specialText[2].SetActive(false);
            specialText[3].SetActive(false);
        }
        else if (currentIndex == 5)
        {
            noSpecialText.SetActive(false);
            specialText[0].SetActive(false);
            specialText[1].SetActive(false);
            specialText[2].SetActive(true);
            specialText[3].SetActive(false);
        }
        else if (currentIndex == 6)
        {
            noSpecialText.SetActive(false);
            specialText[1].SetActive(false);
            specialText[2].SetActive(false);
            specialText[3].SetActive(true);
            specialText[4].SetActive(false);
        }
        else if (currentIndex == 7)
        {
            noSpecialText.SetActive(false);
            noSpecialText.SetActive(false);
            specialText[2].SetActive(false);
            specialText[3].SetActive(false);
            specialText[4].SetActive(true);
        }
    }
    public void NextChar()
    {
        notEnough.SetActive(false);
        menuCharAnim = availableChars[currentIndex].gameObject.GetComponent<Animator>();
        menuCharAnim.Play("CharFadeOut");

        StartCoroutine(KillChar(currentIndex));
        if (currentIndex + 1 == availableChars.Length)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        availableChars[currentIndex].SetActive(true);
        HandleSpecialsText();

        CheckIfCharacterIsUnlocked();
    }

    IEnumerator KillChar(int charNum)
    {
        yield return new WaitForSeconds(0.25f);
        availableChars[charNum].SetActive(false);

    }
    public void PreviousChar()
    {

        availableChars[currentIndex].SetActive(false);
        unlockFor.SetActive(false);
        notEnough.SetActive(false);
        if (currentIndex - 1 == -1)
        {
            currentIndex = availableChars.Length - 1;
        }
        else
        {
            currentIndex--;
        }

        availableChars[currentIndex].SetActive(true);
        HandleSpecialsText();
        CheckIfCharacterIsUnlocked();
    }

    void CheckIfCharacterIsUnlocked()
    {
        if (chars[currentIndex])
        {
            if (currentIndex == DataController.instance.selectedIndex)
            {
                selected.SetActive(true);
                unlockFor.SetActive(false);
                lockIcon.SetActive(false);
                unlockButton.SetActive(false);
                selectButton.SetActive(false);
            }
            else
            {
                unlockFor.SetActive(false);
                selected.SetActive(false);
                lockIcon.SetActive(false);
                selectButton.SetActive(true);
                unlockButton.SetActive(false);
            }
        }
        else
        {
            // if hero is locked
            selected.SetActive(false);
            unlockFor.SetActive(true);
            unlockButton.SetActive(true);
            selectButton.SetActive(false);
            lockIcon.SetActive(true);


            switch (currentIndex)
            {
                case 1:
                    characterPrice = "100";
                    minus.text = "- 100";
                    break;
                case 2:
                    characterPrice = "200";
                    minus.text = "- 200";
                    break;
                case 3:
                    characterPrice = "400";
                    minus.text = "- 400";
                    break;
                case 4:
                    characterPrice = "600";
                    minus.text = "- 600";
                    break;
                case 5:
                    characterPrice = "1100";
                    minus.text = "- 1100";
                    break;
                case 6:
                    characterPrice = "2200";
                    minus.text = "- 2200";
                    break;
                case 7:
                    characterPrice = "3000";
                    minus.text = "- 3000";
                    break;
            }
            cost.text = characterPrice;
        }
    }

    public void SelectChar()
    {
        convertPrice = System.Convert.ToInt32(characterPrice);
        if (!chars[currentIndex])
        {
            if (currentIndex != DataController.instance.selectedIndex)
            {
                if (DataController.instance.highPod >= convertPrice)
                {
                    lockIcon.SetActive(false);
                    unlockFor.SetActive(false);
                    unlockButton.SetActive(false);
                    selected.SetActive(true);
                    minusHolder.SetActive(true);
                    StartCoroutine(KillText());
                    DataController.instance.highPod -= convertPrice;
                    chars[currentIndex] = true;
                    myPods.text = DataController.instance.highPod.ToString();
                    DataController.instance.selectedIndex = currentIndex;
                    DataController.instance.characters = chars;
                    DataController.instance.SaveGameData();
                }
                else
                {
                    notEnough.SetActive(true);
                    unlockFor.SetActive(false);
                }
            }
        }
        else
        {
            notEnough.SetActive(false);
            unlockFor.SetActive(false);
            selected.SetActive(true);
            selectButton.SetActive(false);
            DataController.instance.selectedIndex = currentIndex;
            DataController.instance.SaveGameData();
        }
    }

    IEnumerator KillText()
    {
        yield return new WaitForSeconds(1f);
        minusHolder.SetActive(false);
    }
}//Class
