using UnityEngine;

public class Star : MonoBehaviour
{

    Vector2 pos;
    public float amplitude = 0.25f;
    public float frequency = 1.5f;
    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    public GameObject starExplosion;
    SpriteRenderer starRender;


    private void Awake()
    {
        posOffset = transform.position;
        starRender = this.gameObject.GetComponent<SpriteRenderer>();
        //FixPosition();
    }
    private void FixPosition()
    {
        pos = new Vector2(this.transform.position.x, .2f);
        this.transform.position = pos;
    }
    private void FixedUpdate()
    {
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

    public void HandleDeath()
    {
        starRender.enabled = false;
        var starExplode = Instantiate(starExplosion, transform.position, transform.rotation) as GameObject;
        starExplode.transform.position = this.transform.position;
        Destroy(starExplode, 0.4f);
        Destroy(this.gameObject, 0.5f);
    }

}
