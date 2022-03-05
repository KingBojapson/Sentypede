
using UnityEngine;

public class AdAnimations : MonoBehaviour
{
    void ReturnGame()
    {
        GameMenuController.gmmInstance.AfterAdPlay();
    }
}
