using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyCollision : MonoBehaviour
{
       private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag=="cannon_stop"){
            CharacterController.Instance.StopFlying();
            Debug.Log("------------");
        }}
}
