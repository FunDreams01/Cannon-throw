using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
namespace PathCreation.Examples { }

public class CharacterController : MonoBehaviour {
    public GameObject endpoint;
    public GameObject endPath;
    public bool canMove = false;
    public float translateSpeed;
    public float translateLimit;
    Vector3 initPos;
    GameObject point;
    public PathFollower[] path_followers;
    public static bool inCanon = true;
    public float fallHeight;
    bool backToTrack = false;
    GameObject trackpoint;
    public float fallSpeed;
    Animator anim;
    GameObject myCollider;
    ColliderFix IdleColFix;
    ColliderFix FlyColFix;
    ColliderFix WRColFix;
    ColliderFix WLColFix;
    ColliderFix SwimColFix;
    bool isWallWalking = false;
    public float wallWalkingSpeed;
    public bool launch = false;
    Vector2 beginTouchPos;
    bool touchDidMove;
    bool fall = false;
    bool isswimming = false;
    GameObject myCharacter;
    GameObject body;
    public float speed;
    GameObject myPoint;
    bool moveTowardsPoint = false;
    public float slerpSpeed;
    bool rotate = false;
    bool startAlign = false;
    private static CharacterController _instance;
    public static CharacterController Instance {

        get {
            if (_instance == null) {
                _instance = FindObjectOfType<CharacterController> ();
                if (_instance == null) {
                    GameObject go = new GameObject ();
                    go.name = typeof (CharacterController).Name;
                    _instance = go.AddComponent<CharacterController> ();
                }
            }
            return _instance;
        }
    }

    private void Awake () {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy (gameObject);
        }
    }
    // Start is called before the first frame update
    void Start () {
        body = this.transform.Find ("body").gameObject;
        myCharacter = body.transform.Find ("myCharacter").gameObject;
        myCollider = body.transform.Find ("collider").gameObject;
        path_followers = this.GetComponents<PathFollower> ();
        anim = myCharacter.GetComponent<Animator> ();
        IdleColFix = new ColliderFix ("Idle", new Vector3 (0, 0.145f, 0.063f), new Vector3 (90, 0, 0));
        FlyColFix = new ColliderFix ("Idle", new Vector3 (0, 0.145f, 0.17f), new Vector3 (90, 0, 0));
        WLColFix = new ColliderFix ("Idle", new Vector3 (-0.086f, 0.145f, 0.06f), new Vector3 (-90, 0, 0));
        WRColFix = new ColliderFix ("Idle", new Vector3 (0.086f, 0.145f, 0.06f), new Vector3 (-90, 0, 0));
        SwimColFix = new ColliderFix ("Idle", new Vector3 (0, 1.3f, -1.18f), new Vector3 (0, 0, 0));
        Idle ();
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        if (inCanon) {
            resetCanon ();
        }
        if (isWallWalking) {
            StartWallWalking ();
        }

        if (launch) {
            transform.Translate (0, 0, 1 * speed * Time.deltaTime);
            if (backToTrack) {
                if (trackpoint != null) {
                    if (transform.position.x > trackpoint.transform.position.x) {
                        transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                    } else if (transform.position.x <= trackpoint.transform.position.x) {
                        transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                    }
                }
            }
            if (canMove) {

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
                                if (transform.position.x >= 0) {
                                    if (transform.position.x < translateLimit) {
                                        transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                                    }
                                } else {
                                    transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                                }
                                beginTouchPos = touch.position;
                            }
                            //swipe left
                            if ((touch.position.x < beginTouchPos.x)) {
                                if (transform.position.x >= 0) {
                                    transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                                } else {
                                    if (transform.position.x > -translateLimit) {
                                        transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                                    }
                                }
                                beginTouchPos = touch.position;
                            }
                            break;
                    }
                }

                if (Input.GetKey (KeyCode.RightArrow)) {
                    if (transform.position.x >= 0) {
                        if (transform.position.x < translateLimit) {
                            transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                        }
                    } else {
                        transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                    }
                }
                if (Input.GetKey (KeyCode.LeftArrow)) {
                    if (transform.position.x >= 0) {
                        transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                    } else {
                        if (transform.position.x > -translateLimit) {
                            transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                        }
                    }
                }
            }

        } else {
            if (isswimming) {

            } else {
                if (!fall) {
                    SetDirection ();
                } else {
                     if (transform.position.y > fallHeight) {
                    transform.Translate (0, -1 * speed * Time.deltaTime, 0);
                    }
                }
            }

        }

    }

    public void RedirectToPoint (GameObject go) {
        point = go;
        rotate = true;
    }
    public bool GetPlayerState () {
        return launch;
    }
    public void StopFlying () {
        launch = false;
    }
    public float GetSpeed () {
        return speed;
    }
    public void StartFollowPath (int i) {
        path_followers[i].enabled = true;
    }

    public void StopFollowPath (int i) {
        path_followers[i].enabled = false;
    }
    public void DestroyTrajectory (GameObject path) {
        Destroy (path);
        transform.rotation = Quaternion.Euler (0, transform.rotation.y, transform.rotation.z);
        launch = true;
    }
    public void SetSpeed (float s) {
        speed = s;
    }
    public void RedirectToPath (PathCreator path) {
        this.GetComponent<PathFollower> ().pathCreator = path;
    }

    void StartWallWalking () {
        transform.Translate (Vector3.forward * wallWalkingSpeed * Time.deltaTime);
    }

    public void StopWallWalking () {
        isWallWalking = false;
    }

    public void ActivateWallWalking () {
        isWallWalking = true;
    }
    public void Idle () {
        anim.SetInteger ("animParam", 0);
        myCollider.transform.position = transform.TransformPoint (IdleColFix.position);
        myCollider.transform.eulerAngles = IdleColFix.rotation;
    }

    public void Fall () {
        //launch fall animation
        fall = true;
        launch = false;
    }
    public void Fly () {
        anim.SetInteger ("animParam", 1);
        myCollider.transform.position = transform.TransformPoint (FlyColFix.position);
        myCollider.transform.eulerAngles = FlyColFix.rotation;
    }

    public void WalkLeft () {
        anim.SetInteger ("animParam", 2);
        myCollider.transform.position = transform.TransformPoint (WLColFix.position);
        myCollider.transform.eulerAngles = WLColFix.rotation;
    }

    public void WalkRight () {
        anim.SetInteger ("animParam", 3);
        myCollider.transform.position = transform.TransformPoint (WRColFix.position);
        myCollider.transform.eulerAngles = WRColFix.rotation;
    }

    public void Swim () {
        anim.SetInteger ("animParam", 4);
        myCollider.transform.position = transform.TransformPoint (SwimColFix.position);
        myCollider.transform.eulerAngles = SwimColFix.rotation;
        isswimming = true;
    }

    public void launchCharacter () {
        launch = true;
        FollowPath (0);
        Fly ();
    }

    public void FollowPath (int i) {
        path_followers[i].enabled = true;
    }
    public void resetCanon () {

        inCanon = false;
    }

    public void SetDirection () {
        GameObject dir = GameManager.Instance.GetCurrentCanon ().gameObject.transform.Find ("direction").gameObject;
        transform.position = dir.transform.position;
        transform.rotation = dir.transform.rotation;
    }

    public void StartMove () {
        canMove = true;
    }
    public void StopMove () {
        canMove = false;
    }

    public void AlignEnd () {
        endPath.transform.position = transform.position;
        transform.LookAt (endpoint.transform);
        endPath.transform.LookAt (endpoint.transform);
    }

    public void SetBackTOtrack (bool b, GameObject g) {
        backToTrack = b;
        trackpoint = g;
    }
}