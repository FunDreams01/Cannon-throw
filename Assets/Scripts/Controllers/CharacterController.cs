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
        rotateCollider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void rotateCollider(){

    }


}
