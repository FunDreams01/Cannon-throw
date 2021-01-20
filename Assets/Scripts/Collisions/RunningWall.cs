using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            if(this.gameObject.tag == "wallLeft"){
                GameManager.Instance.WalkLeft();
            }else if(this.gameObject.tag == "wallRight"){
                GameManager.Instance.WalkRight();
            }
        }
    }

    private void OnCollisionExit(Collision other){
        if(other.gameObject.tag == "Player"){
            GameManager.Instance.Fly();
            //redirect to trajectory
        }
    }
}
