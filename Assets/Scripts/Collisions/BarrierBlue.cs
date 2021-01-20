using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBlue : MonoBehaviour
{
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            GameManager.Instance.hitObstacle();
        }
    }
    
}
