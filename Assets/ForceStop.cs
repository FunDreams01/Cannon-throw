using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceStop : MonoBehaviour
{
        void OnTriggerEnter(Collider other){
        if(other.gameObject.tag=="Player"){
            bool launch = GameManager.Instance.GetPlayerState ();
           if(launch){
               CharacterController.Instance.StopForce(true);
           }
        }
    }
}
