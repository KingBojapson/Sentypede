using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This changes the background size based on width and height of screen needs to be tweaked for the suburb area
public class BGScaler : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 tempScale = transform.localScale;

        float width = spriteRenderer.sprite.bounds.size.x;
        float height = spriteRenderer.sprite.bounds.size.y;

        float worldHeight = Camera.main.orthographicSize * 2;
        float worldWidth = worldHeight / Screen.height * Screen.width + 2;

        tempScale.x = worldWidth / width;
        tempScale.y = worldHeight / height;

        transform.localScale = tempScale;
    }
}
