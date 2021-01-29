using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour {
    public GameObject vfx;
    public Text dist;
    public Text pos;
    GameObject cannon;
    Vector2 beginTouchPos;
    bool touchDidMove = false;
    public float epsilon = 5;
    public float rotationSpeed = 40;
    public float limitRotDegree = 25;
    public GameObject kaboom;
    public string state = "standBy";
    public bool shoot = false;
    public bool ok = false;
    string rotate = "false";
    Animator anim;
    public GameObject rotateTowards;
    public float rotationSmooth;
    public bool rotateNow = false;
    // Start is called before the first frame update
    void Start () {
        cannon = transform.Find ("Cannon").gameObject;
        kaboom = cannon.transform.Find ("kaboom").gameObject;
        anim = kaboom.GetComponent<Animator> ();
        vfx = transform.Find ("Light").gameObject;
    }

    // Update is called once per frame
    void Update () {
        if (ok) {
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
                //kaboom!
                if (shoot) {
                    anim.SetBool ("ok", true);
                    if (GameManager.Instance.GetSelectedForce () == "perfect") {
                        vfx.SetActive (true);
                        vfx.GetComponent<ParticleSystem> ().Play ();
                    }
                    if (anim.GetCurrentAnimatorStateInfo (0).IsName ("anim2")) {
                        shoot = false;
                    }
                } else {
                    GameManager.Instance.launchCharacter ();
                    GameManager.Instance.StartFollowPath (0);
                    state = "launched";
                }
            }
        } else {
            if (rotateNow) {
              // Smoothly rotates towards target 
            var targetPoint = rotateTowards.transform.position;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth); 
            if(Quaternion.Angle(targetRotation, transform.rotation)<0.01f){
                rotateNow=false;
            }   
            }
        }

    }

    public void setState (string s) {
        state = s;
    }

    public void ShootCannon () {
        shoot = true;
    }
    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            bool launch = GameManager.Instance.GetPlayerState ();
            if (!launch) {
                if (state == "standBy") {
                    GameManager.Instance.SetCurrentCanon (this.gameObject);
                    GameManager.Instance.SetDirection ();
                    GameManager.Instance.changeCam ("closeToFar");
                }
            } else {
                GameManager.Instance.StopFlying ();
                state = "adjustRotation";
                GameManager.Instance.uninit();
                GameManager.Instance.SetCurrentCanon (this.gameObject);
                if (this.tag == "cannon") {
                    rotateNow = true;
                    GameManager.Instance.RotateEnv();
                    GameManager.Instance.changeCam("closeToFar");
                }
            }

        }
    }

}