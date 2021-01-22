using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    GameObject cannon;
    Vector2 beginTouchPos;
    bool touchDidMove = false;
    public float epsilon = 0.2f;
    public float rotationSpeed;
    public float limitRotDegree;
    // Start is called before the first frame update
    void Start () {
        cannon = transform.Find ("Cannon").gameObject;
    }

    // Update is called once per frame
    void Update () {
        if ((Input.touchCount > 0)) {
            Touch touch = Input.GetTouch (0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    beginTouchPos = touch.position;
                    touchDidMove = false;
                    break;

                case TouchPhase.Moved:
                    touchDidMove = true;
                    if ((touch.position.x > beginTouchPos.x) && ((touch.position.y - beginTouchPos.y) < epsilon)) {
                        cannon.transform.Rotate (0,1*rotationSpeed*Time.deltaTime, 0);
                    } else if ((touch.position.x < beginTouchPos.x) && ((touch.position.y - beginTouchPos.y) < epsilon)) {
                        cannon.transform.Rotate (0, -1*rotationSpeed*Time.deltaTime, 0);
                    }
                    break;
            }
        }
    }
}