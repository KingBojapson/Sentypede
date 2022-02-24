using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    bool leftSide;
    bool rightSide;

    Vector2 leftStartCast;
    Vector2 LeftEndCast;
    Vector2 rightStartCast;
    Vector2 rightEndCast;
    Animator coneAnim;
    Vector2 conePos;

    void Awake()
    {
        coneAnim = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        conePos = this.transform.position;
        //Left Check
        leftStartCast.x = conePos.x + .5f;
        leftStartCast.y = conePos.y;
        LeftEndCast = leftStartCast;
        LeftEndCast.x = conePos.x + 3.5f;
        LeftEndCast.y = conePos.y - 2.0f;

        //Right Check
        rightStartCast.x = conePos.x - 0.5f;
        rightStartCast.y = conePos.y + .75f;
        rightEndCast = rightStartCast;
        rightEndCast.x = conePos.x - 3.5f;
        rightEndCast.y = conePos.y + 3.0f;
        rightSide = Physics2D.Linecast(leftStartCast, LeftEndCast, 1 << LayerMask.NameToLayer("Player"));
        leftSide = Physics2D.Linecast(rightStartCast, rightEndCast, 1 << LayerMask.NameToLayer("Player"));


    }

    public void HandleDeath()
    {
        if (leftSide)
        {
            coneAnim.enabled = true;
            coneAnim.SetBool("hitLeft", true);
            coneAnim.Play("ConeHit");
        }
        else if (rightSide)
        {
            coneAnim.enabled = true;
            coneAnim.SetBool("hitRight", true);
            coneAnim.Play("ConeHitRight");
        }
        Destroy(this.gameObject, .6f);
    }
}
