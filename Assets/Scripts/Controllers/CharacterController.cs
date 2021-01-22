using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Animator anim;
    GameObject myCollider;
    ColliderFix IdleColFix;
    ColliderFix FlyColFix;
    ColliderFix WRColFix;
    ColliderFix WLColFix;
    ColliderFix SwimColFix;
    bool isWallWalking=false;

    public float wallWalkingSpeed;
    Rigidbody rb;
    

    private static CharacterController _instance;
    public static CharacterController Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<CharacterController>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(CharacterController).Name;
                    _instance=go.AddComponent<CharacterController>();
                }
            }
        return _instance;
       }
   }

    private void Awake(){
        if(_instance == null){
            _instance=this;
        } else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start(){
        anim=this.GetComponent<Animator>();
        myCollider=this.transform.Find("collider").gameObject;
        rb=myCollider.GetComponent<Rigidbody>();
        IdleColFix= new ColliderFix("Idle", new Vector3(0,1.3f,-1.18f), new Vector3(90,0,0));
        FlyColFix= new ColliderFix("Idle", new Vector3(0,1.22f,-1.18f), new Vector3(90,0,0));
        WLColFix= new ColliderFix("Idle", new Vector3(-0.13f,0,0.16f), new Vector3(-90,0,0));
        WRColFix= new ColliderFix("Idle", new Vector3(0,1.3f,-1.18f), new Vector3(-90,0,0));
        SwimColFix= new ColliderFix("Idle", new Vector3(0,1.3f,-1.18f), new Vector3(0,0,0));
      
        Idle();
    }

    // Update is called once per frame
    void Update() {
        if(isWallWalking){
            StartWallWalking();
        }
    }

    void StartWallWalking(){
         transform.Translate(Vector3.forward*wallWalkingSpeed*Time.deltaTime);      
    }

    public void StopWallWalking(){
        isWallWalking=false;
    }
    public void Idle(){
       anim.SetInteger("animParam",0);
       myCollider.transform.position= transform.TransformPoint(IdleColFix.position);
       myCollider.transform.Rotate(IdleColFix.rotation);
    }

    public void Fly(){
        anim.SetInteger("animParam",1);
        myCollider.transform.position= transform.TransformPoint(FlyColFix.position);
        myCollider.transform.Rotate(FlyColFix.rotation);
    }

    public void WalkLeft(){
       anim.SetInteger("animParam",2);
        isWallWalking=true;
        myCollider.transform.position= transform.TransformPoint(WLColFix.position);
        myCollider.transform.Rotate(WLColFix.rotation);
    }

    public void WalkRight(){
       anim.SetInteger("animParam",3);
        isWallWalking=true;
        myCollider.transform.position= transform.TransformPoint(WRColFix.position);
        myCollider.transform.Rotate(WRColFix.rotation);
    }

    public void Swim(){
       anim.SetInteger("animParam",4);
        myCollider.transform.position= transform.TransformPoint(SwimColFix.position);
        myCollider.transform.Rotate(SwimColFix.rotation);
    }
    
}
