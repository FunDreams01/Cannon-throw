using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour {
    public Text dist;
    public Text pos;
    GameObject cannon;
    Vector2 beginTouchPos;
    bool touchDidMove = false;
    public float epsilon = 5;
    public float rotationSpeed = 40;
    public float limitRotDegree = 25;

    public string state = "standBy";
    // Start is called before the first frame update
    void Start () {
        cannon = transform.Find ("Cannon").gameObject;
    }

    // Update is called once per frame
    void Update () {
        if (state == "standBy") {
            if ((Input.touchCount > 0)) {
                Touch touch = Input.GetTouch (0);
                switch (touch.phase) {
                    case TouchPhase.Began:
                        beginTouchPos = touch.position;
                        touchDidMove = false;
                        break;

                    case TouchPhase.Moved:
                        touchDidMove = true;
                        //swipe right
                        if ((touch.position.x > beginTouchPos.x)) {
                            if (cannon.transform.eulerAngles.y < limitRotDegree || cannon.transform.eulerAngles.y >= (360 - limitRotDegree - 1)) {
                                cannon.transform.Rotate (0, 1 * rotationSpeed * Time.deltaTime, 0);
                                pos.text = cannon.transform.eulerAngles.y.ToString ();
                                beginTouchPos = touch.position;
                            }

                        }
                        //swipe left
                        if ((touch.position.x < beginTouchPos.x)) {
                            if (cannon.transform.eulerAngles.y > (360 - limitRotDegree) || cannon.transform.eulerAngles.y <= limitRotDegree + 1) {
                                cannon.transform.Rotate (0, -1 * rotationSpeed * Time.deltaTime, 0);
                                pos.text = cannon.transform.eulerAngles.y.ToString ();
                                beginTouchPos = touch.position;
                            }
                        }
                        break;
                }
                GameManager.Instance.SetDirection ();
            }
        } else if (state == "launch") {
            transform.Find ("SwitchCam").gameObject.SetActive (true);
            GameManager.Instance.launchCharacter ();
            GameManager.Instance.StartFollowPath (0);
            state = "launched";
        }
    }

    public void setState (string s) {
        state = s;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            bool launch = GameManager.Instance.GetPlayerState ();
            if (!launch) {
                if (state == "standBy") {
                    GameManager.Instance.SetCurrentCanon (this.gameObject);
                    GameManager.Instance.SetDirection ();
                }

            } else {
                state = "adjustRotation";
                GameManager.Instance.StopFlying ();
                GameManager.Instance.SetCurrentCanon (this.gameObject);
                Debug.Log(this.gameObject.name);
                // GameManager.Instance.RotateEnv(envRotation);
            }

        }
    }

}