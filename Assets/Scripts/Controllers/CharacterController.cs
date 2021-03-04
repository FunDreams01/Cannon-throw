    using System.Collections.Generic;
    using System.Collections;
    using PathCreation;
    using UnityEngine;
    namespace PathCreation.Examples { }

    public class CharacterController : MonoBehaviour {
        public GameObject endpoint;
        public GameObject endPath;
        float rotationInitSpeed;
        public string sideCollision = "";
        bool canMove = false;
        float resetRotation_timer;
        public float timer;
        public bool resetRot = false;
        float translateSpeed;
        public bool stopForce = false;
        float translateLimit;
        public float fallTimer;
        float rotationSpeed;
        float limitRotDegree;
        public bool reachedEndline = false;
        Vector3 initPos;
        GameObject point;
        Rigidbody rb;
        public PathFollower[] path_followers;
        public static bool inCanon = true;
        float fallHeight;
        bool backToTrack = false;
        GameObject trackpoint;
        float fallSpeed;
        Animator anim;
        GameObject myCollider;
        ColliderFix IdleColFix;
        ColliderFix FlyColFix;
        ColliderFix WRColFix;
        ColliderFix WLColFix;
        ColliderFix SwimColFix;
        Vector3 modelFixWL;
        Vector3 modelFixWR;
        bool isWallWalking = false;
        float wallWalkingSpeed;
        public bool launch = false;
        public bool boom = false;
        Vector2 beginTouchPos;
        bool touchDidMove;
        bool rotate_init = false;
        public bool fall = false;
        bool isswimming = false;
        GameObject myCharacter;
        GameObject body;
        bool reset_character = true;
        float speed;
        GameObject myPoint;
        bool moveTowardsPoint = false;
        bool rotate = false;
        bool startAlign = false;
        public bool moveRight = false;
        public bool moveLeft = false;
        public float rotationleft = 360;
        float dashSpeed;
        public bool follow = false;
        public GameObject pathTarget;
        public int nbr_spins = 0;
        public bool spin = false;

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
            rb = GetComponent<Rigidbody> ();
            stopForce = false;
            body = this.transform.Find ("body").gameObject;
            myCharacter = body.transform.Find ("myCharacter").gameObject;
            myCollider = body.transform.Find ("collider").gameObject;
            path_followers = this.GetComponents<PathFollower> ();
            anim = myCharacter.GetComponent<Animator> ();
            IdleColFix = new ColliderFix ("Idle", new Vector3 (0, 0.32f, 0.7f), new Vector3 (90, 0, 0));
            FlyColFix = new ColliderFix ("Idle", new Vector3 (0, 0.44f, 0.38f), new Vector3 (90, 0, 0));
            WLColFix = new ColliderFix ("Idle", new Vector3 (0, 0, 0), new Vector3 (-90, 0, 0));
            WRColFix = new ColliderFix ("Idle", new Vector3 (0, 0, 0), new Vector3 (-90, 0, 0));
            SwimColFix = new ColliderFix ("Idle", new Vector3 (0, 1.3f, -1.18f), new Vector3 (-62.538f, 0, 0));
            modelFixWL = new Vector3 (-0.22f, -1.71f, 0);
            modelFixWR = new Vector3 (0.22f, -1, 0);
            Idle ();
            initPos = transform.position;
            translateSpeed = GameManager.Instance.ch_TranslateSped;
            translateLimit = GameManager.Instance.ch_TranslateLimit;
            limitRotDegree = GameManager.Instance.ch_RotationLimit;
            rotationSpeed = GameManager.Instance.ch_RotationSpeed;
            wallWalkingSpeed = GameManager.Instance.wallSpeed;
            fallTimer = GameManager.Instance.fallTimer;
            dashSpeed = GameManager.Instance.dashSpeed;
            resetRotation_timer = GameManager.Instance.resetRotation_timer;
            timer = resetRotation_timer;
            rotationInitSpeed = GameManager.Instance.rotationInitSpeed;
        }

        // Update is called once per frame
        void FixedUpdate () {
            if (inCanon) {
                resetCanon ();
            }
            if (isWallWalking) {
                StartWallWalking ();
            }

            if (nbr_spins > 0) {
                spin = true;
            } else {
                spin = false;
            }

            if (spin) {
                float rotation = dashSpeed * Time.deltaTime;
                if (rotationleft > rotation) {
                    rotationleft -= rotation;
                } else {
                    rotation = rotationleft;
                    rotationleft = 0;
                    rotationleft = 360;
                    reset_character = true;
                    nbr_spins--;
                }
                myCharacter.transform.Rotate (0, 0, -rotation);
            }

            if (launch) {
                if (!stopForce) {
                    if (follow) {
                        if (pathTarget != null) {
                            //rotate rowatrds target
                            // Smoothly rotates towards target 
                            var targetPoint = pathTarget.transform.position;
                            var targetRotation = Quaternion.LookRotation (targetPoint - transform.position, Vector3.up);
                            transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime*20);
                            var dir = transform.position - pathTarget.transform.position;
                            dir = dir.normalized;
                        transform.Translate (0, 0, 1 * speed * Time.deltaTime);
                        }
                    } else {
                        transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.identity, Time.deltaTime * 20);
                        transform.Translate (0, 0, 1 * speed * Time.deltaTime);
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
                                if (moveRight) {
                                    if ((touch.position.x > beginTouchPos.x)) {
                                        resetRot = false;
                                        timer = resetRotation_timer;
                                        rotate_init = false;
                                        if (transform.position.x >= 0) {
                                            if (transform.position.x < translateLimit) {
                                                //   transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                                                transform.position = transform.position + Vector3.right * Time.deltaTime * translateSpeed;

                                            }
                                        } else {
                                            //transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                                            transform.position = transform.position + Vector3.right * Time.deltaTime * translateSpeed;
                                        }
                                        if ((transform.eulerAngles.z < limitRotDegree) || transform.eulerAngles.z >= (360 - limitRotDegree - 10)) {
                                            transform.Rotate (0, 0, 1 * rotationSpeed * Time.deltaTime);
                                        }
                                        beginTouchPos = touch.position;
                                    }
                                }

                                //swipe left
                                if (moveLeft) {
                                    if ((touch.position.x < beginTouchPos.x)) {
                                        resetRot = false;
                                        timer = resetRotation_timer;
                                        rotate_init = false;
                                        if (transform.position.x >= 0) {
                                            //  transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                                            transform.position = transform.position + Vector3.left * Time.deltaTime * translateSpeed;
                                        } else {
                                            if (transform.position.x > -translateLimit) {
                                                //transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                                                transform.position = transform.position + Vector3.left * Time.deltaTime * translateSpeed;
                                            }
                                        }
                                        if (transform.eulerAngles.z > (360 - limitRotDegree) || transform.eulerAngles.z <= limitRotDegree + 10) {
                                            transform.Rotate (0, 0, -1 * rotationSpeed * Time.deltaTime);
                                        }
                                        beginTouchPos = touch.position;
                                    }
                                }

                                break;
                            case TouchPhase.Ended:
                                resetRot = true;
                                break;
                        }
                    }

                    if (moveRight) {
                        if (Input.GetKey (KeyCode.RightArrow)) {
                            resetRot = false;
                            timer = resetRotation_timer;
                            rotate_init = false;
                            if (transform.position.x >= 0) {
                                if (transform.position.x < translateLimit) {
                                    //   transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                                    transform.position = transform.position + Vector3.right * Time.deltaTime * translateSpeed;
                                }
                            } else {
                                //transform.Translate (1 * Time.deltaTime * translateSpeed, 0, 0);
                                transform.position = transform.position + Vector3.right * Time.deltaTime * translateSpeed;
                            }
                            if ((transform.eulerAngles.z < limitRotDegree) || transform.eulerAngles.z >= (360 - limitRotDegree - 10)) {
                                transform.Rotate (0, 0, 1 * rotationSpeed * Time.deltaTime);
                            }
                        }

                        if (Input.GetKeyUp (KeyCode.RightArrow)) {
                            resetRot = true;
                        }
                    }
                    if (resetRot) {
                        if (timer > 0) {
                            timer = timer - Time.deltaTime;
                        } else {
                            rotate_init = true;
                            resetRot = false;
                            timer = resetRotation_timer;
                        }
                    }

                    if (rotate_init) {
                        transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.identity, Time.deltaTime * rotationInitSpeed);
                        if (transform.rotation == Quaternion.identity) {
                            rotate_init = false;
                        }
                    }

                    if (moveLeft) {
                        if (Input.GetKey (KeyCode.LeftArrow)) {
                            resetRot = false;
                            timer = resetRotation_timer;
                            rotate_init = false;
                            if (transform.position.x >= 0) {
                                //  transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                                transform.position = transform.position + Vector3.left * Time.deltaTime * translateSpeed;

                            } else {
                                if (transform.position.x > -translateLimit) {
                                    //transform.Translate (-1 * Time.deltaTime * translateSpeed, 0, 0);
                                    transform.position = transform.position + Vector3.left * Time.deltaTime * translateSpeed;

                                }
                            }
                            if (transform.eulerAngles.z > (360 - limitRotDegree) || transform.eulerAngles.z <= limitRotDegree + 10) {
                                transform.Rotate (0, 0, -1 * rotationSpeed * Time.deltaTime);
                            }
                        }
                        if (Input.GetKeyUp (KeyCode.LeftArrow)) {
                            resetRot = true;
                        }
                    }
                }

            } else {
                UIManager.Instance.decStamina = false;
                if (isswimming) {
                    transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 0, transform.eulerAngles.z);
                } else {
                    if (!fall) {
                        SetDirection ();
                    } else {
                        if (fallTimer > 0) {
                            fallTimer = fallTimer - Time.deltaTime;
                            transform.rotation = Quaternion.Euler (0, 0, 0);
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
            anim.SetInteger ("animParam", 0);
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
            transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y + 360, transform.rotation.eulerAngles.z + 360);
            Destroy (path);
            launch = true;
        }
        public void StraightenPlayer () {
            transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x + 360, 0, transform.rotation.eulerAngles.z + 360);
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
            myCharacter.transform.localPosition = Vector3.zero;
        }

        public void Fall () {
            //launch fall animation
            fall = true;
            launch = false;
            CinemachineSwitcher.Instance.playAnim ("down");
            CinemachineSwitcher.Instance.StopFollowing ("down");
        }
        public void Fly () {
            anim.SetInteger ("animParam", 1);
            myCollider.transform.position = transform.TransformPoint (FlyColFix.position);
            myCollider.transform.localEulerAngles = FlyColFix.rotation;
            myCharacter.transform.localPosition = Vector3.zero;
        }

        public void WalkLeft () {
            anim.SetInteger ("animParam", 2);
            myCollider.transform.position = transform.TransformPoint (WLColFix.position);
            myCollider.transform.localEulerAngles = WLColFix.rotation;
            myCharacter.transform.localPosition = modelFixWL;
        }

        public void WalkRight () {
            anim.SetInteger ("animParam", 3);
            myCollider.transform.position = transform.TransformPoint (WRColFix.position);
            myCollider.transform.localEulerAngles = WRColFix.rotation;
            myCharacter.transform.localPosition = modelFixWR;
        }

        public void Swim () {
            anim.SetInteger ("animParam", 4);
            // myCollider.transform.position = transform.TransformPoint (SwimColFix.position);
            //  myCollider.transform.localEulerAngles = SwimColFix.rotation;
            // myCharacter.transform.localPosition = Vector3.zero;
            //  myCharacter.transform.localEulerAngles=SwimColFix.rotation;
            //this.transform.localEulerAngles=new Vector3(transform.rotation.x,0,transform.rotation.z);
            launch = false;
            isswimming = true;
            //CinemachineSwitcher.Instance.playAnim ("side");
        }

        public void launchCharacter () {
            launch = true;
            boom = true;
            if (GameManager.Instance.GetSelectedForce () == "perfect") {
                reset_character = false;
                spin = true;
            }
            Fly ();
        }

        public void FollowPath (int i) {
            path_followers[i].enabled = true;
        }
        public void resetCanon () {
            inCanon = false;
        }

        public void SetDirection () {
            if (GameManager.Instance.GetCurrentCanon ().gameObject.transform.Find ("direction").gameObject != null) {
                GameObject dir = GameManager.Instance.GetCurrentCanon ().gameObject.transform.Find ("direction").gameObject;
                transform.position = dir.transform.position;
                transform.rotation = dir.transform.rotation;
            }

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

        public void reset () {
            myCharacter.transform.localPosition = new Vector3 (0, 0, 0);
            myCharacter.transform.localRotation = Quaternion.identity;
        }

        public void StopForce (bool x) {
            stopForce = x;
        }

        public void Jump () {
            this.gameObject.GetComponent<Jump2TargetFinal> ().enabled = true;
        }

        public void freeze () {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }