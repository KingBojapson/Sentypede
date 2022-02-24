using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimations : MonoBehaviour
{
    public void HandleObjectivePop()
    {
        GameMenuController.gmmInstance.ObjectivePopUp();
    }

    public void ObjectiveReversePop()
    {
        GameMenuController.gmmInstance.ReverseObjectivePopUp();
    }

    public void ContinueWithGame()
    {
        GameMenuController.gmmInstance.ContinueWithGame();
    }

    public void CloseCollectiblePop()
    {
        GameMenuController.gmmInstance.ContinueAfterCollectible();
    }

    public void AfterDeathPop()
    {
        GameMenuController.gmmInstance.AfterDeathPop();
    }

    public void HandleMySpecialPop()
    {
        GameMenuController.gmmInstance.HandleMySpecialPop();
    }
}
