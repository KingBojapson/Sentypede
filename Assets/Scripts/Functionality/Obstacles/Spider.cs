using System.Collections;
using UnityEngine;

public class Spider : MonoBehaviour
{

    public GameObject spiderVenom;
    Vector2 spiderPos;
    public float venomForce;
    public float venomRelease = .2f;
    float venomReleaseReturn = .2f;

    SpriteRenderer spiderRend;
    public bool leftSpotted, rightSpotted;

    Vector2 leftStartCast;
    Vector2 LeftEndCast;
    Vector2 rightStartCast;
    Vector2 rightEndCast;

    private void Awake()
    {
        spiderRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spiderPos = this.transform.position;
        Raycasting();
    }

    void Raycasting()
    {
        leftStartCast.x = spiderPos.x + .5f;
        leftStartCast.y = spiderPos.y;
        LeftEndCast = leftStartCast;
        LeftEndCast.x = spiderPos.x + 2.5f;

        rightSpotted = Physics2D.Linecast(leftStartCast, LeftEndCast, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(leftStartCast, LeftEndCast, Color.red);

        rightStartCast.x = spiderPos.x - 0.5f;
        rightStartCast.y = spiderPos.y;
        rightEndCast = rightStartCast;
        rightEndCast.x = spiderPos.x - 2.5f;

        leftSpotted = Physics2D.Linecast(rightStartCast, rightEndCast, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(rightStartCast, rightEndCast, Color.green);

        if (rightSpotted)
        {
            AttackRight();
            spiderRend.flipX = true;
        }
        else if (leftSpotted)
        {
            AttackLeft();
        }
    }

    void AttackLeft()
    {
        venomRelease -= Time.deltaTime;
        if (venomRelease <= 0)
        {
            venomRelease = venomReleaseReturn;
            var spit = Instantiate(spiderVenom, transform.position, transform.rotation) as GameObject;
            spit.transform.Rotate(0f, 0f, 90f);
            spit.GetComponent<Rigidbody2D>().AddForce(Vector2.left * venomForce * Time.deltaTime);
        }

    }

    void AttackRight()
    {
        venomRelease -= Time.deltaTime;

        if (venomRelease <= 0)
        {
            venomRelease = venomReleaseReturn;
            var spit = Instantiate(spiderVenom, transform.position, transform.rotation) as GameObject;
            spit.transform.Rotate(0f, 0f, -90f);
            spit.GetComponent<Rigidbody2D>().AddForce(Vector2.right * venomForce * Time.deltaTime);
        }
    }
}
