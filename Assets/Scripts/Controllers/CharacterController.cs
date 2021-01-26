using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
namespace PathCreation.Examples { }

public class CharacterController : MonoBehaviour {
    GameObject point;
    public PathFollower[] path_followers;
    public static bool inCanon = true;
    public float fallHeight;
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
    bool fall = false;
    GameObject myCharacter;
    GameObject body;
    float speed = 5;
    GameObject myPoint;
    bool moveTowardsPoint = false;
    public float slerpSpeed;
    bool rotate = false;
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
        /* anim = myCharacter.GetComponent<Animator> ();
         IdleColFix = new ColliderFix ("Idle", new Vector3 (0, 0.145f, 0.063f), new Vector3 (90, 0, 0));
         FlyColFix = new ColliderFix ("Idle", new Vector3 (0, 0.145f, 0.17f), new Vector3 (90, 0, 0));
         WLColFix = new ColliderFix ("Idle", new Vector3 (-0.086f, 0.145f, 0.06f), new Vector3 (-90, 0, 0));
         WRColFix = new ColliderFix ("Idle", new Vector3 (0.086f, 0.145f, 0.06f), new Vector3 (-90, 0, 0));
         SwimColFix = new ColliderFix ("Idle", new Vector3 (0, 1.3f, -1.18f), new Vector3 (0, 0, 0));
         Idle();*/
        // GameObject firstCannon = TrajectoryManager.Instance.cannons_rings_endpoints[0];
        //transform.position = firstCannon.transform.position + new Vector3 (offsetX, offsetY, offsetZ);
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
            if (rotate) {
                Vector3 targetDirection = point.transform.position - transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = slerpSpeed * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards (transform.forward, targetDirection, singleStep, 0.0f);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation (newDirection);
            }
        } else {
            if (!fall) {
                SetDirection ();
            } else {
                transform.Translate (0, -1 * speed * Time.deltaTime, 0);
                if (transform.position.y <= fallHeight) {
                    ScenesManager.Instance.LoadScene ("CannonThrow");
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
        myCollider.transform.Rotate (IdleColFix.rotation);
    }

    public void Fall () {
        //launch fall animation
        fall = true;
        launch = false;
    }
    public void Fly () {
        Debug.Log ("fly");
        anim.SetInteger ("animParam", 1);
        myCollider.transform.position = transform.TransformPoint (FlyColFix.position);
        myCollider.transform.Rotate (FlyColFix.rotation);
    }

    public void WalkLeft () {
        Debug.Log ("walk left");
        anim.SetInteger ("animParam", 2);
        myCollider.transform.position = transform.TransformPoint (WLColFix.position);
        myCollider.transform.Rotate (WLColFix.rotation);
    }

    public void WalkRight () {
        anim.SetInteger ("animParam", 3);
        myCollider.transform.position = transform.TransformPoint (WRColFix.position);
        myCollider.transform.Rotate (WRColFix.rotation);
    }

    public void Swim () {
        anim.SetInteger ("animParam", 4);
        myCollider.transform.position = transform.TransformPoint (SwimColFix.position);
        myCollider.transform.Rotate (SwimColFix.rotation);
    }

    public void launchCharacter () {
        launch = true;
        FollowPath (0);
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

}