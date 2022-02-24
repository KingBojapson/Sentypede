using System.Collections;
using UnityEngine;

public class GiftScreenAnimation : MonoBehaviour {

    Transform giftBox;
    Transform continueButton;
    Transform stars;
    Transform textObject;

    // Use this for initialization
    private void Awake()
    {
        giftBox = this.transform.GetChild(1);
        continueButton = this.transform.GetChild(2);
        stars = this.transform.GetChild(3);
        textObject = this.transform.GetChild(4);
    }

    void GiftBox() 
    {
        giftBox.gameObject.SetActive(true);
    }
    IEnumerator StarsActivate() 
    {
        yield return new WaitForSeconds(.4f);
        stars.gameObject.SetActive(true);
    }
    IEnumerator PodText() 
    {
        yield return new WaitForSeconds(.6f);
        textObject.gameObject.SetActive(true);
    }
    IEnumerator Continue() 
    {
        yield return new WaitForSeconds(.4f);
        continueButton.gameObject.SetActive(true);
    }
}
