using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour {
    public GameObject myPath;
    public int index;
    GameObject vfx;
    GameObject cannon;
    Vector2 beginTouchPos;
    bool touchDidMove = false;
    float epsilon = 5;
    float rotationSpeed;
    float limitRotDegree;
    GameObject kaboom;
    public string state = "standBy";
    bool shoot = false;
    public bool ok = false;
    string rotate = "false";
    Animator anim;
    public GameObject rotateTowards;
    float rotationSmooth;

    bool rotateNow = false;
    // Start is called before the first frame update
    void Start () {
        cannon = transform.Find ("Cannon").gameObject;
        kaboom = cannon.transform.Find ("kaboom").gameObject;
        anim = kaboom.GetComponent<Animator> ();
        vfx = transform.Find ("Light").gameObject;
        rotationSpeed = GameManager.Instance.can_RotationSpeed;
        limitRotDegree = GameManager.Instance.can_RotationDegree;
        rotationSmooth = GameManager.Instance.can_Rot_NewPath;
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
                                if (transform.eulerAngles.y < limitRotDegree || transform.eulerAngles.y >= (360 - limitRotDegree - 1)) {
                                    transform.Rotate (0, 1 * rotationSpeed * Time.deltaTime, 0);
                                    beginTouchPos = touch.position;
                                }
                            }
                            //swipe left
                            if ((touch.position.x < beginTouchPos.x)) {
                                if (transform.eulerAngles.y > (360 - limitRotDegree) || transform.eulerAngles.y <= limitRotDegree + 1) {
                                    transform.Rotate (0, -1 * rotationSpeed * Time.deltaTime, 0);
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
                    CharacterController.Instance.StopForce (false);
                    GameManager.Instance.launchCharacter ();
                    UIManager.Instance.decStamina = true;
                    // GameManager.Instance.StartFollowPath (index);
                    CharacterController.Instance.follow = true;
                    state = "launched";
                    reset_blendshape ();
                }
            }
        } else {
            if (rotateNow) {
                // Smoothly rotates towards target 
                var targetPoint = rotateTowards.transform.position;
                var targetRotation = Quaternion.LookRotation (targetPoint - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);
                if (Quaternion.Angle (targetRotation, transform.rotation) < 0.4f) {
                    if (transform.tag == "cannon") {
                        GameManager.Instance.GetCurrentCanon ().GetComponent<CannonController> ().ShootCannon ();
                        GameManager.Instance.GetCurrentCanon ().GetComponent<CannonController> ().setState ("standBy");
                    }
                    rotateNow = false;
                }
            }
        }

    }

    public void setState (string s) {
        state = s;
    }

    public void reset_blendshape () {
        anim.SetBool ("ok", false);
    }

    public void ShootCannon () {
        shoot = true;
    }
    private void OnCollisionEnter (Collision other) {
        if (other.gameObject.tag == "Player") {
            CharacterController.Instance.reset ();
            if (myPath != null) {
                myPath.SetActive (true);
            }
            bool launch = GameManager.Instance.GetPlayerState ();
            if (!launch) {
                if (state == "standBy") {
                    GameManager.Instance.SetCurrentCanon (this.gameObject);
                    GameManager.Instance.SetDirection ();
                }
            } else {
                GameManager.Instance.StopFlying ();
                state = "adjustRotation";
                GameManager.Instance.uninit ();
                GameManager.Instance.SetCurrentCanon (this.gameObject);
                if (this.tag == "cannon") {
                    rotateNow = true;
                    GameManager.Instance.RotateEnv ();
                }
            }
            if (GameManager.Instance.staminaValue < 100) {
                UIManager.Instance.refullStamina = true;
            }
        }
    }

}