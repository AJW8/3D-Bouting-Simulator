using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cam;
    public GameObject mapSprite;
    public float minSpeed;
    public float maxSpeed;
    public float speedTransitionTime;
    public Vector2 maxCameraRotation;
    public float rollFactor;
    private RectTransform mapSpriteRT;
    private Quaternion mapSpriteOrigin;
    private float arenaRadius;
    private bool speedingUp;
    private bool slowingDown;
    private float speedUpFactor;
    private float steerFactor;
    private bool inBout;
    private float slowTimeFactor;

    // Start is called before the first frame update
    void Start()
    {
        mapSpriteRT = mapSprite.GetComponent<RectTransform>();
        mapSpriteOrigin = mapSpriteRT.rotation;
        speedingUp = false;
        slowingDown = false;
        speedUpFactor = 0;
        steerFactor = 0;
        inBout = false;
        slowTimeFactor = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.forward, transform.position);
        transform.Rotate((minSpeed + (maxSpeed - minSpeed) * (speedUpFactor / speedTransitionTime)) * Mathf.Rad2Deg * Time.deltaTime / (arenaRadius * slowTimeFactor), steerFactor * Time.deltaTime / slowTimeFactor, 0, Space.Self);
        transform.position = transform.up * arenaRadius;
        transform.Rotate(0, 0, -steerFactor * rollFactor);
        mapSpriteRT.rotation = mapSpriteOrigin;
        mapSpriteRT.Rotate(0, 0, -steerFactor / 2);
        if (!inBout)
        {
            cam.transform.rotation = transform.rotation;
            steerFactor = (Input.mousePosition.x * 2f / Screen.width - 1f) * maxCameraRotation.x;
            cam.transform.Rotate((1f - Input.mousePosition.y * 2f / Screen.height) * maxCameraRotation.y, steerFactor / 2, 0);
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                speedingUp = true;
                slowingDown = false;
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow)) speedingUp = false;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                speedingUp = false;
                slowingDown = true;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow)) slowingDown = false;
            if (speedingUp)
            {
                speedUpFactor += Time.deltaTime;
                if (speedUpFactor > speedTransitionTime)
                {
                    speedingUp = false;
                    speedUpFactor = speedTransitionTime;
                }
            }
            else if (slowingDown)
            {
                speedUpFactor -= Time.deltaTime;
                if (speedUpFactor < 0)
                {
                    slowingDown = false;
                    speedUpFactor = 0;
                }
            }
        }
        if (inBout) return;
    }

    public Vector3 GetFuturePosition(float seconds)
    {
        Quaternion rotation = transform.rotation;
	    transform.rotation = Quaternion.LookRotation(transform.forward, transform.position);
        transform.Rotate((minSpeed + (maxSpeed - minSpeed) * (speedUpFactor / speedTransitionTime)) * Mathf.Rad2Deg * seconds / (arenaRadius * slowTimeFactor), steerFactor * seconds / slowTimeFactor, 0, Space.Self);
        Vector3 futurePosition = transform.up;
        transform.rotation = rotation;
        return futurePosition;
    }

    public void SetArenaRadius(float r)
    {
        arenaRadius = r;
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
