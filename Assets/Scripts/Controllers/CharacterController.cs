using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
namespace PathCreation.Examples{}

public class CharacterController : MonoBehaviour {
    public static bool inCanon=true;
    Animator anim;
    GameObject myCollider;
    ColliderFix IdleColFix;
    ColliderFix FlyColFix;
    ColliderFix WRColFix;
    ColliderFix WLColFix;
    ColliderFix SwimColFix;
    bool isWallWalking = false;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public float wallWalkingSpeed;
    bool launch = true;
    GameObject myCharacter;
    GameObject body;
    float speed;
    GameObject myPoint;
    bool moveTowardsPoint=false;
     float lerpTime;
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
        anim = myCharacter.GetComponent<Animator> ();
        IdleColFix = new ColliderFix ("Idle", new Vector3 (0, 0.145f, 0.063f), new Vector3 (90, 0, 0));
        FlyColFix = new ColliderFix ("Idle", new Vector3 (0, 0.145f, 0.17f), new Vector3 (90, 0, 0));
        WLColFix = new ColliderFix ("Idle", new Vector3 (-0.086f, 0.145f, 0.06f), new Vector3 (-90, 0, 0));
        WRColFix = new ColliderFix ("Idle", new Vector3 (0.086f, 0.145f, 0.06f), new Vector3 (-90, 0, 0));
        SwimColFix = new ColliderFix ("Idle", new Vector3 (0, 1.3f, -1.18f), new Vector3 (0, 0, 0));
        Idle();
       // GameObject firstCannon = TrajectoryManager.Instance.cannons_rings_endpoints[0];
        //transform.position = firstCannon.transform.position + new Vector3 (offsetX, offsetY, offsetZ);
    }

    // Update is called once per frame
    void Update () {
        if(inCanon){
            resetCanon();
        }
        if (isWallWalking) {
            StartWallWalking ();
        }


        if(launch){
            CinemachineSwitcher.Instance.playAnim("main");
            transform.Translate(0,0,1*speed*Time.deltaTime);
        }
    }


    public void SetSpeed(float s){
        this.GetComponent<PathFollower>().speed=s;
        speed=s;
    }
    public void RedirectToPath (PathCreator path) {
        this.GetComponent<PathFollower>().pathCreator=path;
         this.GetComponent<PathFollower>().startFollowing=true;

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
    }

    public void resetCanon(){

        inCanon=false;
    }

    public void SetDirection(){
      GameObject dir= GameManager.Instance.GetCurrentCanon().gameObject.transform.Find("Cannon").gameObject.transform.Find("direction").gameObject;
        transform.position=dir.transform.position;
        transform.rotation=dir.transform.rotation;
    }

}