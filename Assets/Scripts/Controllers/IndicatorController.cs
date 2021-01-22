using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour {
    GameObject hitIndicator;
    Vector2 beginTouchPos;
    Vector2 endTouchPos;
    float timeTouchBegan;
    float tapTime;
    bool touchDidMove = false;
    public float tapTimeThreshold = 0.5f;
    // Start is called before the first frame update
    void Start () {
        hitIndicator = transform.Find ("HitIndicator").gameObject;
    }

    // Update is called once per frame
    void Update () {
        if (!hitIndicator.GetComponent<ForceSelector> ().forceSlected) {
            if ((Input.touchCount > 0)) {
                Touch touch = Input.GetTouch (0);
                switch (touch.phase) {
                    case TouchPhase.Began:
                        beginTouchPos = touch.position;
                        timeTouchBegan = Time.time;
                        touchDidMove = false;
                        break;

                    case TouchPhase.Moved:
                        touchDidMove = true;
                        break;

                    case TouchPhase.Ended:
                        endTouchPos = touch.position;
                        tapTime = Time.time - timeTouchBegan;

                        if (!touchDidMove && tapTime <= tapTimeThreshold) {
                            Ray raycast = Camera.main.ScreenPointToRay (Input.touches[0].position);
                            RaycastHit raycastHit;
                            if (Physics.Raycast (raycast, out raycastHit)) {
                                if (raycastHit.collider.CompareTag ("indicator")) {
                                    hitIndicator.GetComponent<ForceSelector> ().selectForce ();
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}