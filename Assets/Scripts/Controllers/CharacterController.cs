using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
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

    GameObject destination;
    Rigidbody rb;
    bool launch =true;

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
        anim = this.GetComponent<Animator> ();
        myCollider = this.transform.Find ("collider").gameObject;
        rb = myCollider.GetComponent<Rigidbody> ();
        IdleColFix = new ColliderFix ("Idle", new Vector3 (0, 1.3f, -1.18f), new Vector3 (90, 0, 0));
        FlyColFix = new ColliderFix ("Idle", new Vector3 (0, 1.22f, -1.18f), new Vector3 (90, 0, 0));
        WLColFix = new ColliderFix ("Idle", new Vector3 (-0.13f, 0, 0.16f), new Vector3 (-90, 0, 0));
        WRColFix = new ColliderFix ("Idle", new Vector3 (0, 1.3f, -1.18f), new Vector3 (-90, 0, 0));
        SwimColFix = new ColliderFix ("Idle", new Vector3 (0, 1.3f, -1.18f), new Vector3 (0, 0, 0));
        GameObject firstCannon = TrajectoryManager.Instance.cannons_rings_endpoints[0];
        transform.position = firstCannon.transform.position+ new Vector3(offsetX,offsetY,offsetZ);
        Idle ();
    }

    // Update is called once per frame
    void Update () {
        if (isWallWalking) {
            StartWallWalking ();
        }

        if(launch){
            transform.Translate(0,0,0.1f);
            
        }
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

    public void setDestination(GameObject go){
        destination=go;
        transform.LookAt(destination.transform);
    }

    public void launchCharacter(){
        launch=true;
    }

}