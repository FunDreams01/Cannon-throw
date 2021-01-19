using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Animator anim;
    GameObject myCollider;
    // Start is called before the first frame update
    void Start()
    {
        anim=this.GetComponent<Animator>();
        myCollider=this.transform.Find("collider").gameObject;
        // rotate collider
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnCollisionEnter(Collision collision)
    {
        GameObject GO= collision.gameObject;
        string tag=collision.gameObject.tag;
        if(tag=="barrierBlue"){
            LevelManager.mistakes--;
            if(LevelManager.mistakes==0){
                // show x
                // fall and lose
                // show lose panel?
            }
        }else if(tag=="coin"){
            Destroy(GO);
            LevelManager.score=LevelManager.score+LevelManager.coinBonus;
            //update UI
        }else if(tag=="water"){
            // rotate collider
            //activat animation
        }else if(tag=="wallLeft"){
            // rotate collider
            //activat animation
        }else if(tag=="wallRight"){
            // rotate collider
            //activat animation
        }
        // else if tag= canon
    }
}
