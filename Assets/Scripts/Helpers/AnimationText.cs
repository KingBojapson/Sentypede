using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationText : MonoBehaviour {

    List<Animator> animators;
    float waitBetween = .15f;
    float waitEnd = .25f;

    private void OnEnable()
    {
        animators = new List<Animator>(GetComponentsInChildren<Animator>());
        StartCoroutine(DoAnimation());
    }
    IEnumerator DoAnimation() 
    {
        while (true) 
        { 
            foreach(var animator in animators) 
            {
                animator.SetTrigger("DoAnim");
                yield return new WaitForSeconds(waitBetween);
            }
            yield return new WaitForSeconds(waitEnd);
        }
    }
}
