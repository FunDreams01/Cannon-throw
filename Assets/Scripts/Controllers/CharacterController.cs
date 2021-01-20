using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Animator anim;
    GameObject myCollider;

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
    void Start()
    {
        anim=this.GetComponent<Animator>();
        myCollider=this.transform.Find("collider").gameObject;
        Idle();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Idle(){
       resetAnimParam();
       anim.SetBool("isIdle",true);
    }

    public void Fly(){
        resetAnimParam();
        anim.SetBool("isFlying",true);
    }

    public void WalkLeft(){
        resetAnimParam();
        anim.SetBool("isWalkingLeft",true);
    }

    public void WalkRight(){
        resetAnimParam();
        anim.SetBool("isWalkingRight",true);
    }

    public void Swim(){
        anim.SetBool("isSwimming",true);
    }
    
    void resetAnimParam(){
        anim.SetBool("isIdle",false);
        anim.SetBool("isFlying",false);
        anim.SetBool("isWalkingRight",false);
        anim.SetBool("isWalkingLeft",false);
        anim.SetBool("isSwimming",false);
    }

}
