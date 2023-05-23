using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject mapSprite;
    public float speed;
    public float rollFactor;
    private RectTransform mapSpriteRT;
    private Quaternion mapSpriteOrigin;
    private float arenaRadius;
    private float steerFactor;
    private bool inBout;
    private float slowTimeFactor;

    // Start is called before the first frame update
    void Start()
    {
        mapSpriteRT = mapSprite.GetComponent<RectTransform>();
        mapSpriteOrigin = mapSpriteRT.rotation;
        steerFactor = 0;
        inBout = false;
        slowTimeFactor = 1;
    }

    // Update is called once per frame
    void Update()
    {
	transform.rotation = Quaternion.LookRotation(transform.forward, transform.position);
        transform.Rotate(speed * Mathf.Rad2Deg * Time.deltaTime / (arenaRadius * slowTimeFactor), steerFactor * Time.deltaTime / slowTimeFactor, 0, Space.Self);
        transform.position = transform.up * arenaRadius;
        transform.Rotate(0, 0, -steerFactor * rollFactor);
    }

    public Vector3 GetFuturePosition(float seconds)
    {
        Quaternion rotation = transform.rotation;
	    transform.rotation = Quaternion.LookRotation(transform.forward, transform.position);
        transform.Rotate(speed * Mathf.Rad2Deg * seconds / (arenaRadius * slowTimeFactor), steerFactor * seconds / slowTimeFactor, 0, Space.Self);
        Vector3 futurePosition = transform.up;
        transform.rotation = rotation;
        return futurePosition;
    }

    public void SetArenaRadius(float r)
    {
        arenaRadius = r;
    }

    public void SetMapSpritePosition(Vector2 p)
    {
        mapSpriteRT.anchoredPosition = p;
    }

    public void SetMapSpriteBearing(float b)
    {
        mapSpriteRT.rotation = mapSpriteOrigin;
        mapSpriteRT.Rotate(0, 0, b);
    }

    public void SetInBout(bool i)
    {
        inBout = i;
    }

    public void SetSlowTimeFactor(float f)
    {
        slowTimeFactor = f;
    }
}
