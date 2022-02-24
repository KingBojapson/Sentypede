using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This grabs all the collisions in the game and destroys them if the game didn't destroy it first
public class ObjectCollector : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.Land)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.ObH)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.CH)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.PH)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.Reversal)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.Berry)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.Orange)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.Cherry)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.Stuck)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.Star)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.Skull)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.EObs)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag == Constants.Slick)
        {
            Destroy(collision.gameObject);
        }
    }
}
