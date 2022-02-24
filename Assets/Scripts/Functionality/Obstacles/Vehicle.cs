using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{

    bool leftSpotted;
    bool rightSpotted;
    bool topSpotted;
    bool mySide;
    bool touchingLand;

    public float rightSpeed;
    public float leftSpeed;
    public float myY;
    float followCamera;

    public float explosionRelease = .2f;
    float explosionReleaseReturn = .2f;

    public int numberChoosen;

    public GameObject vehicleExplosion;

    Vector2 carCurrentPos;

    //casting
    Vector2 leftStartCast;
    Vector2 LeftEndCast;
    Vector2 rightStartCast;
    Vector2 rightEndCast;
    Vector2 topStartCast;
    Vector2 topEndCast;

    Rigidbody2D vRB;

    private void Awake()
    {

        rightSpeed = 1250f;
        leftSpeed = 850f;
        vRB = GetComponent<Rigidbody2D>();
        myY = this.transform.rotation.y;
        numberChoosen = Random.Range(0, 3);
    }
    private void FixedUpdate()
    {

        carCurrentPos = this.transform.position;

        if (myY > 0)
        {
            //Left Check
            leftStartCast.x = carCurrentPos.x + .5f;
            leftStartCast.y = carCurrentPos.y;
            LeftEndCast = leftStartCast;
            LeftEndCast.x = carCurrentPos.x + 3.5f;
            LeftEndCast.y = carCurrentPos.y - 2.0f;

            rightSpotted = Physics2D.Linecast(leftStartCast, LeftEndCast, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawLine(leftStartCast, LeftEndCast, Color.red);

            //Right Check
            rightStartCast.x = carCurrentPos.x - 0.5f;
            rightStartCast.y = carCurrentPos.y + .75f;
            rightEndCast = rightStartCast;
            rightEndCast.x = carCurrentPos.x - 3.5f;
            rightEndCast.y = carCurrentPos.y + 3.0f;

            leftSpotted = Physics2D.Linecast(rightStartCast, rightEndCast, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawLine(rightStartCast, rightEndCast, Color.green);

            //Top Check
            topStartCast.x = carCurrentPos.x + .5f;
            topStartCast.y = carCurrentPos.y + .5f;
            topEndCast = carCurrentPos;
            topEndCast.x = carCurrentPos.x + 10f;
            topEndCast.y = carCurrentPos.y + 5f;

            topSpotted = Physics2D.Linecast(topStartCast, topEndCast, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawLine(topStartCast, topEndCast, Color.blue);

            if (topSpotted || leftSpotted || rightSpotted)
            {
                CarAttribute(numberChoosen);
            }
        }
        else if (myY <= 0)
        {
            //Left Check
            leftStartCast.x = carCurrentPos.x + .5f;
            leftStartCast.y = carCurrentPos.y + .75f;
            LeftEndCast = leftStartCast;
            LeftEndCast.x = carCurrentPos.x + 3.5f;
            LeftEndCast.y = carCurrentPos.y + 3.0f;

            rightSpotted = Physics2D.Linecast(leftStartCast, LeftEndCast, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawLine(leftStartCast, LeftEndCast, Color.red);

            //Right Check
            rightStartCast.x = carCurrentPos.x - 0.78f;
            rightStartCast.y = carCurrentPos.y + .05f;
            rightEndCast = rightStartCast;
            rightEndCast.x = carCurrentPos.x - 3.0f;
            rightEndCast.y = carCurrentPos.y - 1.5f;

            leftSpotted = Physics2D.Linecast(rightStartCast, rightEndCast, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawLine(rightStartCast, rightEndCast, Color.green);

            //Top Check
            topStartCast.x = carCurrentPos.x + .5f;
            topStartCast.y = carCurrentPos.y + .5f;
            topEndCast = carCurrentPos;
            topEndCast.x = carCurrentPos.x - 10f;
            topEndCast.y = carCurrentPos.y + 5f;

            topSpotted = Physics2D.Linecast(topStartCast, topEndCast, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawLine(topStartCast, topEndCast, Color.blue);

            if (topSpotted || leftSpotted || rightSpotted)
            {
                CarAttribute(numberChoosen);
            }
        }

    }
    void CarAttribute(int number)
    {
        switch (number)
        {
            case 0:
                if (topSpotted)
                {
                    SoundManager.soundInstance.CarStartUp();
                }
                break;
            case 1:
                if (rightSpotted)
                {
                    MakeVehicleMoveRight();
                }
                else if (leftSpotted)
                {
                    MakeVehicleMoveLeft();
                }
                break;
            case 2:
                var explosion = Instantiate(vehicleExplosion, transform.position, transform.rotation) as GameObject;
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(explosion.gameObject, 0.45f);
                Destroy(this.gameObject, 0.55f);
                break;
            case 3:
                //instantiate tire after explosion
                break;
        }
    }

    void MakeVehicleMoveLeft()
    {
        vRB.AddForce(Vector2.left * leftSpeed * Time.deltaTime);
    }
    void MakeVehicleMoveRight()
    {
        vRB.AddForce(Vector2.right * rightSpeed * Time.deltaTime);
    }

    public void HandleDeath()
    {
        var explosion = Instantiate(vehicleExplosion, transform.position, transform.rotation) as GameObject;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        CameraShake.cmInstance.Shake(.25f, .22f);
        Destroy(explosion.gameObject, 0.45f);
        Destroy(this.gameObject, 0.55f);

    }
}
