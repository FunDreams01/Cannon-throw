using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeUpCam : MonoBehaviour
{
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag=="Player"){
           CameraFollow.isCloseUp=true;
        }
    }
}
