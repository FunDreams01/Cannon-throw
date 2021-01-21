using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceSelector : MonoBehaviour
{
    Animator anim;
    public int animSpeed=1;
    bool forceSlected=false;
    // Start is called before the first frame update
    void Start()
    {
        anim=this.GetComponent<Animator>();
        anim.speed = animSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!forceSlected){
        if(Input.GetKey(KeyCode.Return)){
            selectForce();
        }
        }
                    Debug.DrawRay(transform.position, Vector3.down*500, Color.green);


    }

    void selectForce(){
        anim.enabled=false;
    }
}
