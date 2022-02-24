using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {

    Transform text;
    Transform gift;
    Transform mButton;
    Animator textAnim;
    Animator giftAnim;
    Animator button;

    private void Awake()
    {
        text = this.transform.GetChild(0);
        gift = this.transform.GetChild(1);
        mButton = this.transform.GetChild(2);
        textAnim = text.gameObject.GetComponent<Animator>();
        giftAnim = gift.gameObject.GetComponent<Animator>();
        button = mButton.gameObject.GetComponent<Animator>();
    }

    void ThankYou() 
    {
        text.gameObject.SetActive(true);
        textAnim.Play("ThankYouText");
    }

    void GiftPop()
    {
        gift.gameObject.SetActive(true);
        giftAnim.Play("GiftPopUp");
    }

    void ButtonStretch() 
    {
        mButton.gameObject.SetActive(true);
        button.Play("Button");
    }
}
